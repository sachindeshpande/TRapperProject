// Capture
//
// Written by Richard L Rosenheim
//            richard@rosenheims.com
//            May 21, 2005
//
// Based in a large part upon the DxScan and DxPlayer samples by David Wohlferd
//
// Portions Copyright 2005 - Richard L Rosenheim

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

using System.Collections.Generic;

using DirectShowLib;

using ProjectCommon;
using Utilities;
using Logging;

namespace DirectShowPlayer
{

    public class VideoBufferEventArgs : EventArgs
    {
        public VideoBufferEventArgs(Bitmap pVideoBufferBitmap)
        {
            mVideoBufferBitmap = pVideoBufferBitmap;
        }

        public Bitmap mVideoBufferBitmap;
    }

    public class FrameMilestoneEventArgs : EventArgs
    {
        public FrameMilestoneEventArgs()
        {
        }

    }

    public enum GraphState
    {
        Stopped,
        Paused,
        Running,
        Exiting
    }

    /// <summary> Summary description for MainForm. </summary>
    public class Capture : ISampleGrabberCB, IDisposable
    {
        #region Member variables

        /// <summary> graph builder interface. </summary>
        private IFilterGraph2 m_FilterGraph;
        private Control mParentWindow;

        /// <summary> Dimensions of the image, calculated once in constructor. </summary>
        private int m_videoWidth;
        private int m_videoHeight;
        private int m_stride;
        public int m_Count;

        private const int VIDEO_FILEPATH_INDEX = 1;

        private FrameOverlay mFrameOverlay;
        private Image mCurrentOverlayImage;
        private ImageAttributes mAttr;
        private float mLeft;
        private float mTop;
        private float mImageWidth;
        private float mImageHeight;

        private string m_String;
        private Bitmap bitmapOverlay;
        private Bitmap bitmapRender;
        private Font fontOverlay;
        private Font transparentFont;
        private SolidBrush transparentBrush;

        private bool mParentWindowDisplay ;

        // Current state of the graph (can change async)
        volatile private GraphState m_State = GraphState.Stopped;


#if DEBUG
        // Allow you to "Connect to remote graph" from GraphEdit
        DsROTEntry m_rot = null;
#endif

        #endregion

        #region API

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] uint Length);

        #endregion

        #region Declarations
        [ComVisible(false), ComImport,
            Guid("e436ebb5-524f-11ce-9f53-0020af0ba770")]
        public class AsyncReader
        {
        }

        // Declare the enum w/o using uint, so it can
        // be used in vb
        [Flags]
        enum WindowStyleFlags
        {
            OVERLAPPED = 0x00000000,
            POPUP = -2147483648,
            CHILD = 0x40000000,
            MINIMIZE = 0x20000000,
            VISIBLE = 0x10000000,
            DISABLED = 0x08000000,
            CLIPSIBLINGS = 0x04000000,
            CLIPCHILDREN = 0x02000000,
            MAXIMIZE = 0x01000000,
            BORDER = 0x00800000,
            DLGFRAME = 0x00400000,
            VSCROLL = 0x00200000,
            HSCROLL = 0x00100000,
            SYSMENU = 0x00080000,
            THICKFRAME = 0x00040000,
            GROUP = 0x00020000,
            TABSTOP = 0x00010000,
            MINIMIZEBOX = 0x00020000,
            MAXIMIZEBOX = 0x00010000,
        }

        #endregion

        #region Event Handlers

        public delegate void OnCaptureShutdownEvent(object sender, EventArgs args);
        public delegate void OnFrameMilestoneEvent(object sender, FrameMilestoneEventArgs args);
        public delegate void OnVideoBufferReceivedEvent(object sender, VideoBufferEventArgs args);

        public event OnCaptureShutdownEvent CaptureShutdownEvent;
        public event OnFrameMilestoneEvent FrameMilestoneEvent;
        public event OnVideoBufferReceivedEvent VideoBufferReceivedEvent;

        #endregion

        /// <summary> release everything. </summary>
        public void Dispose()
        {
            ConsoleLogger.logMessage("In Dispose");
            m_State = GraphState.Stopped;
            CloseInterfaces();

            if (bitmapOverlay != null)
                bitmapOverlay.Dispose();
            if (fontOverlay != null)
                fontOverlay.Dispose();
            if (transparentBrush != null)
                transparentBrush.Dispose();
            if (transparentFont != null)
                transparentFont.Dispose();
        }
        ~Capture()
        {
            ConsoleLogger.logMessage("In ~Capture");
            CloseInterfaces();

            if (bitmapOverlay != null)
                bitmapOverlay.Dispose();
            if (fontOverlay != null)
                fontOverlay.Dispose();
            if (transparentBrush != null)
                transparentBrush.Dispose();
            if (transparentFont != null)
                transparentFont.Dispose();
        }

        /// <summary> Use capture device zero, default frame rate and size</summary>
        public Capture(string pVideoInfoFilepath, string TextString, Control hWin, bool pParentWindowDisplay, VideoUserOptions pVideoUserOptions)
        {
            m_Count = 0;
            m_String = TextString;
            mParentWindowDisplay = pParentWindowDisplay;

            mFrameOverlay = new FrameOverlay(pVideoInfoFilepath,pVideoUserOptions);
            mFrameOverlay.loadFrameOverlayHeader();

            _Capture(mFrameOverlay.getVideofileToPlay(), hWin);

            mFrameOverlay.loadFrameOverlayData(m_videoWidth, m_videoHeight);

            mParentWindow = hWin;

            mLeft = 0;
            mTop = 0;

            mAttr = new ImageAttributes();
            mAttr.SetColorKey(Color.Blue, Color.Blue);

//            Shape.initializeShapes(m_videoWidth,m_videoHeight);
        }

        /// <summary> Use capture device zero, default frame rate and size</summary>
        public Capture(string FileName, string TextString, Control hWin, bool pParentWindowDisplay,VideoUserOptions pVideoUserOptions,Bitmap pRender)
            : this(FileName, TextString, hWin, pParentWindowDisplay, pVideoUserOptions)
        {
            bitmapRender = pRender;

        }

        private string getVideoFilename(string pVideoInfoFilepath)
        {
            try
            {
                CSVFileParser l_Parser = new CSVFileParser();
                l_Parser.startParsingCSVData(pVideoInfoFilepath, 0, 0);

                string[] row = l_Parser.getNextRow(0);

                l_Parser.close();

                return row[VIDEO_FILEPATH_INDEX];
            }
            catch (CSVFileException e)
            {
                throw e;
            }

        }

        /// <summary> capture the next image </summary>
        public void Start()
        {
            if (m_State == GraphState.Paused || m_State == GraphState.Stopped)
            {
                IVideoWindow videoWindow = m_FilterGraph as IVideoWindow;
//                videoWindow.put_FullScreenMode(OABool.True);
                videoWindow.put_MessageDrain(mParentWindow.Handle);

                Resume();
            }
        }

        public void Resume()
        {
            IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

            int hr = mediaCtrl.Run();
            DsError.ThrowExceptionForHR(hr);

            m_State = GraphState.Running;
        }

        public GraphState getGraphState()
        {
            return m_State;
        }

        /// <summary>
        ///  Returns an interface to the event notification interface
        /// </summary>
        public IMediaEventEx MediaEventEx
        {
            get
            {
                return (IMediaEventEx)m_FilterGraph;
            }
        }


        // Internal capture
        private void _Capture(string FileName, Control hWin)
        {
            try
            {
                // Set up the capture graph
                SetupGraph(FileName, hWin);
                SetupBitmap();
            }
            catch(Exception e)
            {
                Dispose();
                ConsoleLogger.logMessage(e.Message);
                throw e;
            }
        }

        /// <summary> build the capture graph for grabber. </summary>
        private void SetupGraph(string FileName, Control hWin)
        {
            int hr;

            IBaseFilter ibfRenderer = null;
            ISampleGrabber sampGrabber = null;
            IBaseFilter capFilter = null;
            IPin iPinInFilter = null;
            IPin iPinOutFilter = null;
            IPin iPinInDest = null;
            IBasicAudio basicAudio = null;

            // Get the graphbuilder object
            m_FilterGraph = new FilterGraph() as IFilterGraph2;
#if DEBUG
            m_rot = new DsROTEntry(m_FilterGraph);
#endif

            try
            {
                // Get the SampleGrabber interface
                sampGrabber = new SampleGrabber() as ISampleGrabber;

                // Add the video source
                hr = m_FilterGraph.AddSourceFilter(FileName, "Ds.NET FileFilter", out capFilter);
                DsError.ThrowExceptionForHR(hr);

                // Hopefully this will be the video pin
                IPin iPinOutSource = DsFindPin.ByDirection(capFilter, PinDirection.Output, 0);

                IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                ConfigureSampleGrabber(sampGrabber);

                iPinInFilter = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);
                iPinOutFilter = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Output, 0);

                // Add the frame grabber to the graph
                hr = m_FilterGraph.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);

                hr = m_FilterGraph.Connect(iPinOutSource, iPinInFilter);
                DsError.ThrowExceptionForHR(hr);

                // Get the default video renderer
                ibfRenderer = (IBaseFilter)new VideoRendererDefault();

                // Add it to the graph
                hr = m_FilterGraph.AddFilter(ibfRenderer, "Ds.NET VideoRendererDefault");
                DsError.ThrowExceptionForHR(hr);
                iPinInDest = DsFindPin.ByDirection(ibfRenderer, PinDirection.Input, 0);

                // Connect the graph.  Many other filters automatically get added here
                hr = m_FilterGraph.Connect(iPinOutFilter, iPinInDest);
                DsError.ThrowExceptionForHR(hr);

                SaveSizeInfo(sampGrabber);

                // Set the output window

                IVideoWindow videoWindow = m_FilterGraph as IVideoWindow;
                hr = videoWindow.put_Owner(hWin.Handle);
                DsError.ThrowExceptionForHR(hr);

                hr = videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
                DsError.ThrowExceptionForHR(hr);

                hr = videoWindow.put_Visible(OABool.True);
                DsError.ThrowExceptionForHR(hr);

                //TODO : Need a better way to hide the video in the parent Window
                Rectangle rc = hWin.ClientRectangle;
                if (mParentWindowDisplay)
                    hr = videoWindow.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
                else
                    hr = videoWindow.SetWindowPosition(0, 0, 0, 0);
                DsError.ThrowExceptionForHR(hr);


                IGraphBuilder graphBuilder = m_FilterGraph as IGraphBuilder;
                ICaptureGraphBuilder2 captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

                // Attach the filter graph to the capture graph
                hr = captureGraphBuilder.SetFiltergraph(graphBuilder);
                DsError.ThrowExceptionForHR(hr);

                hr = captureGraphBuilder.RenderStream(null, MediaType.Audio, capFilter, null, null);

            }
            finally
            {
                if (capFilter != null)
                {
                    Marshal.ReleaseComObject(capFilter);
                    capFilter = null;
                }
                if (sampGrabber != null)
                {
                    Marshal.ReleaseComObject(sampGrabber);
                    sampGrabber = null;
                }
                if (ibfRenderer != null)
                {
                    Marshal.ReleaseComObject(ibfRenderer);
                    ibfRenderer = null;
                }
                if (iPinInFilter != null)
                {
                    Marshal.ReleaseComObject(iPinInFilter);
                    iPinInFilter = null;
                }
                if (iPinOutFilter != null)
                {
                    Marshal.ReleaseComObject(iPinOutFilter);
                    iPinOutFilter = null;
                }
                if (iPinInDest != null)
                {
                    Marshal.ReleaseComObject(iPinInDest);
                    iPinInDest = null;
                }
            }
        }

        /// <summary> Read and store the properties </summary>
        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            int hr;

            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();
            hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
            media = null;
        }
        /// <summary> Set the options on the sample grabber </summary>
        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            AMMediaType media;
            int hr;

            // Set the media type to Video/RBG24
            media = new AMMediaType();
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.ARGB32;
            media.formatType = FormatType.VideoInfo;

            hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);
            media = null;

            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary> Shut down capture </summary>
        private void CloseInterfaces()
        {
            int hr;
            ConsoleLogger.logMessage("In CloseInterfaces");

            try
            {
                if (m_FilterGraph != null)
                {
                    IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

                    // Stop the graph
                    hr = mediaCtrl.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

#if DEBUG
            if (m_rot != null)
            {
                m_rot.Dispose();
            }
#endif

            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }
            GC.Collect();

            if(CaptureShutdownEvent != null)
                CaptureShutdownEvent(this, new EventArgs());

            mFrameOverlay.dispose();
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {

            try
            {
                Graphics g;
                String s;

                SizeF d;

/*
                g = Graphics.FromImage(bitmapOverlay);
                g.Clear(System.Drawing.Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Prepare to put the specified string on the image
                g.DrawRectangle(System.Drawing.Pens.Blue, 0, 0, m_videoWidth - 1, m_videoHeight - 1);
                g.DrawRectangle(System.Drawing.Pens.Blue, 1, 1, m_videoWidth - 3, m_videoHeight - 3);

                d = g.MeasureString(m_String, fontOverlay);

                FrameOverlayDisplayItems lOverlayDisplayItemList = mFrameOverlay.getFrameOverlayDisplayItems(m_Count);

                if (lOverlayDisplayItemList != null)
                {
                    int lNumOverlayDisplayItems = lOverlayDisplayItemList.Count;


                    for (int lIndex = 0; lIndex < lNumOverlayDisplayItems; lIndex++)
                    {
                        FrameOverlayItem lOverlayItem = lOverlayDisplayItemList.get(lIndex);
                        if (lOverlayItem.FrameEvent)
                            FrameMilestoneEvent(this, new FrameMilestoneEventArgs());
                        else if (lOverlayItem.PauseRequest)
                        {
                            IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;
                            mediaCtrl.Pause();
//                            System.Threading.Thread.Sleep(lOverlayItem.OverlayDuration);
                            mediaCtrl.Run();
                        }
                        else
                        {
                            double lLeftPosition = lOverlayItem.getLeftPosition(m_Count);
                            if (lOverlayItem.CaptionType == ProjectConstants.TRAINING_IMAGE_CAPTION_OPTION)
                            {
                                g.DrawImage(lOverlayItem.OverlayImage, (float)lLeftPosition, (float)lOverlayItem.mTop,
                                    (float)lOverlayItem.mImageWidth, (float)lOverlayItem.mImageHeight);
                            }
                            else
                                g.DrawString(lOverlayItem.OverlayText, lOverlayItem.FontOverlay, lOverlayItem.TextColor,
                                    (float)lLeftPosition, (float)lOverlayItem.mTop, System.Drawing.StringFormat.GenericTypographic);
                        }

                    }
                }
                

                //            Shape.updateGraphics(g, m_videoWidth, m_videoHeight);

                g.Dispose();
*/

                // need to flip the bitmap so it's the same orientation as the
                // video buffer
//                bitmapOverlay.RotateFlip(RotateFlipType.RotateNoneFlipY);


                // create and copy the video's buffer image to a bitmap
                Bitmap v;
                v = new Bitmap(m_videoWidth, m_videoHeight, m_stride,
                    PixelFormat.Format32bppArgb, pBuffer);
//                g = Graphics.FromImage(v);

//                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // draw the overlay bitmap over the video's bitmap
//                g.DrawImage(bitmapOverlay, 0, 0, bitmapOverlay.Width, bitmapOverlay.Height);

                v.RotateFlip(RotateFlipType.RotateNoneFlipY);



//                VideoBufferReceivedEvent(this, new VideoBufferEventArgs(v));


                // dispose of the various objects
//                g.Dispose();
                v.Dispose();

                // Increment frame number.  Done this way, frame are zero indexed.
                m_Count++;
//                Console.WriteLine("m_Count = " + m_Count);

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }            

            return 0;
        }

        void SetupBitmap()
        {
            int fSize;

            // scale the font size in some portion to the video image
            fSize = 4 * (m_videoWidth / 64);
            bitmapOverlay = new Bitmap(m_videoWidth, m_videoHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            fontOverlay = new Font("Times New Roman", fSize, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);

            // scale the font size in some portion to the video image
            fSize = 5 * (m_videoWidth / 64);
            transparentFont = new Font("Tahoma", fSize, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);
            transparentBrush = new SolidBrush(Color.FromArgb(96, 0, 0, 255));
        }

        public void setDisplayString(string displayString)
        {
            m_String = displayString;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> getFrameEventList()
        {
            return mFrameOverlay.getFrameEventList();
        }

        public void addInformation(string pKey, int lNumStars)
        {
            mFrameOverlay.addRatingItem(pKey, lNumStars);
        }

        // Pause the capture graph.
        public void Pause()
        {
            // If we are playing
            if (m_State == GraphState.Running)
            {
                IMediaControl lmediaCtrl = (IMediaControl)m_FilterGraph;
                int hr = lmediaCtrl.Pause();
                DsError.ThrowExceptionForHR(hr);

                m_State = GraphState.Paused;
            }
        }
        // Pause the capture graph.
        public void Stop()
        {
            // Can only Stop when playing or paused
            if (m_State == GraphState.Running || m_State == GraphState.Paused)
            {
                IMediaControl lmediaCtrl = (IMediaControl)m_FilterGraph;
                int hr = lmediaCtrl.Stop();
                DsError.ThrowExceptionForHR(hr);

                m_State = GraphState.Stopped;
            }
        }

        public void fullScreenMode(bool pFullScreenOption)
        {
            IVideoWindow videoWindow = m_FilterGraph as IVideoWindow;

            if (pFullScreenOption)
                videoWindow.put_FullScreenMode(OABool.True);
            else
                videoWindow.put_FullScreenMode(OABool.False);
        }

        public void cleanup()
        {
            if (mFrameOverlay != null)
                mFrameOverlay.cleanup();
        }

    }
}


/*
                if (mCurrentFrameOverlayItem != null)
                {
                    if (m_Count == mCurrentFrameOverlayItem.FrameNumber)
                    {
                        mCurrentOverlayImage = Image.FromFile(mCurrentFrameOverlayItem.ImagePath);
                        Bitmap lImageBitmap = new Bitmap(mCurrentOverlayImage);
                        lImageBitmap.MakeTransparent(bitmapOverlay.GetPixel(1, 1));
                        //lImageBitmap.MakeTransparent();
                        mCurrentOverlayImage = (Image)lImageBitmap;
                        mLeft = m_videoWidth * mCurrentFrameOverlayItem.XPositionRatio;
                        mTop = m_videoHeight * mCurrentFrameOverlayItem.YPositionRatio;
                        mImageWidth = m_videoWidth * mCurrentFrameOverlayItem.WidthRatio;
                        mImageHeight = m_videoHeight * mCurrentFrameOverlayItem.HeightRatio;
                    }


                    if (m_Count >= mCurrentFrameOverlayItem.FrameNumber &&
                        m_Count <= mCurrentFrameOverlayItem.FrameNumber + mCurrentFrameOverlayItem.OverlayDuration)
                        g.DrawImage(mCurrentOverlayImage, mLeft, mTop, mImageWidth, mImageHeight);


                    if (m_Count == mCurrentFrameOverlayItem.FrameNumber + mCurrentFrameOverlayItem.OverlayDuration)
                        mCurrentFrameOverlayItem = mFrameOverlay.getNextFrameOverlayItem();
                }
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

using Utilities;
using ProjectCommon;
using Logging;

namespace DirectShowPlayer
{
    public class FrameOverlay
    {
        private int mCurrentRetrievalIndex;
        private List<FrameOverlayItem> mFrameOverlayItemList;
        private Dictionary<int, FrameOverlayDisplayItems> mFrameOverlayStorage;

//        public int VideoWidth { set; get; }
//        public int VideoHeight { set; get; }
        private string mVideoInfoFilepath;
        private string mVideoFilepath { set; get; }
        private string mSlowMoVideoFilepath { set; get; }
        public VideoUserOptions VideoUserOptions;

        public int FPS { set; get; }
        public int Duration { set; get; }
        public double SlowMoRatio { set; get; }

        //This object/package does not kwow the meaning of any event.
        //Each event is just like a string to this
        //Its only purpose is to send an event with the string to the client
        //The client will understand the events meaning
        private List<string> mFrameMilestoneEventList;

        private int mMaxFrameCountForOverlayItems;

        private static Dictionary<string,ItemsRating> NumberOfItemsForStars = new Dictionary<string,ItemsRating>();

        public FrameOverlay(string pVideoInfoFilepath,VideoUserOptions pVideoUserOptions)
        {
            mVideoInfoFilepath = pVideoInfoFilepath;
            mFrameOverlayItemList = new List<FrameOverlayItem>();
            mCurrentRetrievalIndex = -1;

            VideoUserOptions = pVideoUserOptions;
            
            mFrameOverlayStorage = new Dictionary<int, FrameOverlayDisplayItems>();

            mFrameMilestoneEventList = new List<string>();

//            NumberOfItemsForStars = new Dictionary<string, ItemsRating>();

            //For test
//            addRatingItem("PartsOfTheFeetScoring", 5);
//            addRatingItem("ToeStandsScoring", 4);
//            addRatingItem("PopOversScoring", 3);
        }

        public void dispose()
        {
            mFrameMilestoneEventList.Clear();
            mFrameOverlayStorage.Clear();
            mFrameOverlayItemList.Clear();
        }

        public FrameOverlayItem getNextFrameOverlayItem()
        {
            if (mCurrentRetrievalIndex == mFrameOverlayItemList.Count - 1)
                return null;

            mCurrentRetrievalIndex++;
            return mFrameOverlayItemList[mCurrentRetrievalIndex];
        }

        public string getVideofileToPlay()
        {
            if (!VideoUserOptions.SlowMo)
                return mVideoFilepath;
            else
                return mSlowMoVideoFilepath;
        }

        public void loadFrameOverlayHeader()
        {
            try
            {
                CSVFileParser l_Parser = new CSVFileParser();
                l_Parser.startParsingCSVData(mVideoInfoFilepath, 0, 0);

                string[] row;


                row = l_Parser.getNextRow(0);
                mVideoFilepath = ProjectConstants.PROJECT_MEDIA_PATH + @"\" + row[1];

                row = l_Parser.getNextRow(0);
                mSlowMoVideoFilepath = ProjectConstants.PROJECT_MEDIA_PATH + @"\" + row[1];

                row = l_Parser.getNextRow(0);
                FPS = Convert.ToInt32(row[1]);

                row = l_Parser.getNextRow(0);
                Duration = Convert.ToInt32(row[1]);

                row = l_Parser.getNextRow(0);
                SlowMoRatio = Convert.ToDouble(row[1]);

            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        public void loadFrameOverlayData(int pVideoWidth,int pVideoHeight)
        {
            try
            {
                CSVFileParser l_Parser = new CSVFileParser();
                l_Parser.startParsingCSVData(mVideoInfoFilepath, 0, 0);

                string[] row;


                //Skip 6 Row
                l_Parser.getNextRow(0);
                l_Parser.getNextRow(0);
                l_Parser.getNextRow(0);
                l_Parser.getNextRow(0);
                l_Parser.getNextRow(0);
                l_Parser.getNextRow(0);

                while ((row = l_Parser.getNextRow(0)) != null)
                {
                    FrameOverlayItem lFrameOverlayItem = new FrameOverlayItem(this, row, pVideoWidth, pVideoHeight);
                    mFrameOverlayItemList.Add(lFrameOverlayItem);

                    if (lFrameOverlayItem.FrameEvent)
                        mFrameMilestoneEventList.Add(lFrameOverlayItem.FrameEventName);

                }

                l_Parser.close();

                setMaxFrameCountForOverlayItems();
                createFrameOverlayStructure();
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }


        public System.Collections.ObjectModel.ReadOnlyCollection<string> getFrameEventList()
        {
            return mFrameMilestoneEventList.AsReadOnly();
        }


        private void setMaxFrameCountForOverlayItems()
        {
            mMaxFrameCountForOverlayItems = 0;
            int lNumFrameOverlayItems = mFrameOverlayItemList.Count;

            for (int lIndex = 0; lIndex < lNumFrameOverlayItems; lIndex++)
            {
                FrameOverlayItem lOverlayItem = mFrameOverlayItemList[lIndex];
                if (lOverlayItem.FrameNumber + lOverlayItem.OverlayDuration > mMaxFrameCountForOverlayItems)
                    mMaxFrameCountForOverlayItems = lOverlayItem.FrameNumber + lOverlayItem.OverlayDuration;
            }
        }

        private void createFrameOverlayStructure()
        {
            int lNumFrameOverlayItems = mFrameOverlayItemList.Count;
 
            for(int lFrameCount = 0; lFrameCount < mMaxFrameCountForOverlayItems; lFrameCount++)
            {
                FrameOverlayDisplayItems lOverlayDisplayItems = new FrameOverlayDisplayItems();
                for (int lIndex = 0; lIndex < lNumFrameOverlayItems; lIndex++)
                {
                    FrameOverlayItem lOverlayItem = mFrameOverlayItemList[lIndex];

                    if (lOverlayItem.isDisplayOn(lFrameCount))
                        lOverlayDisplayItems.add(lOverlayItem);
                }

                if (lOverlayDisplayItems.Count > 0)
                    mFrameOverlayStorage.Add(lFrameCount, lOverlayDisplayItems);
            }
        }

        public FrameOverlayDisplayItems getFrameOverlayDisplayItems(int pFrameCount)
        {
            if (!mFrameOverlayStorage.ContainsKey(pFrameCount))
                return null;

            return mFrameOverlayStorage[pFrameCount];

        }

        public void addRatingItem(string pSegmentName, int pNumStars)
        {
            ItemsRating lRatingItem = new ItemsRating(pSegmentName, pNumStars);
            if (NumberOfItemsForStars.ContainsKey(pSegmentName))
                NumberOfItemsForStars.Remove(pSegmentName);
            NumberOfItemsForStars.Add(pSegmentName,lRatingItem);
        }

        public ItemsRating getRatingItem(string pSegmentName)
        {
            if (NumberOfItemsForStars.ContainsKey(pSegmentName))
                return NumberOfItemsForStars[pSegmentName];
            else
                return null;
        }

        public void cleanup()
        {
            //TODO : Need to add cleanup code
            /*
            string[] fileNameList = Directory.GetFiles(ProjectConstants.PROJECT_TEMP_PATH);

            foreach (string filename in fileNameList)
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(filename);

                //detect whether its a directory or file
                if (!((attr & FileAttributes.Directory) == FileAttributes.Directory))
                    File.Delete(filename);
            }
             * */

        }

    }

    #region FrameOverlayDisplayItems
    public class FrameOverlayDisplayItems
    {
        List<FrameOverlayItem> mOverlayDisplayItems;

        public int Count
        {
            get
            {
                return mOverlayDisplayItems.Count;
            }
        }

        public FrameOverlayDisplayItems()
        {
            mOverlayDisplayItems = new List<FrameOverlayItem>();
        }

        public void add(FrameOverlayItem pFrameOverlayItem)
        {
            mOverlayDisplayItems.Add(pFrameOverlayItem);
        }

        public FrameOverlayItem get(int pIndex)
        {
            if (pIndex >= Count)
                return null;

            return mOverlayDisplayItems[pIndex];
        }


    }

    #endregion

    #region FrameOverlayItem
    public class FrameOverlayItem
    {
        private FrameOverlay mParent;

        private const int FRAME_OVERLAY_VIDEO_FILE_ROW_INDEX = 1;

        public int FrameNumber { get; set;  }
        public int OverlayDuration { get; set; }
        public double mLeft { get; set; }
        public double mTop { get; set; }
        public double mImageWidth { get; set; }
        public double mImageHeight { get; set; }
        public int CaptionType;
        public Image OverlayImage { get; set; }
        public string OverlayText { get; set; }
        public Font FontOverlay { get; set; }
        public Brush TextColor { get; set; }
        public bool Flashing { get; set; }
        public bool Moving { get; set; }
        public bool FrameEvent { get; set; }
        public bool PauseRequest { get; set; }
        public string FrameEventName { get; set; }

        public FrameOverlayItem(FrameOverlay pParent,string[] row,int pVideoWidth,int pVideoHeight)
        {
            mParent = pParent;
            loadFrameOverlayData(row,pVideoWidth,pVideoHeight);

        }

        private void loadFrameOverlayData(string[] row,int pVideoWidth,int pVideoHeight)
        {
            FrameEvent = false;
            PauseRequest = false;

            double FrameTime = Convert.ToDouble(row[0]);
            double lOverlayFrameDuration = Convert.ToDouble(row[1]);

            if (!mParent.VideoUserOptions.SlowMo)
            {
                FrameNumber = (int)(FrameTime * (double)mParent.FPS);
                OverlayDuration = (int)(lOverlayFrameDuration * (double)mParent.FPS);
            }
            else
            {
                FrameNumber = (int)(FrameTime * (double)mParent.FPS * mParent.SlowMoRatio);
                OverlayDuration = (int)(lOverlayFrameDuration * (double)mParent.FPS * mParent.SlowMoRatio);
            }


            double lXPositionRatio = (float)Convert.ToDouble(row[2]);
            double lYPositionRatio = (float)Convert.ToDouble(row[3]);
            double lWidthRatio = (float)Convert.ToDouble(row[4]);
            double lHeightRatio = (float)Convert.ToDouble(row[5]);

            mLeft = pVideoWidth * lXPositionRatio;
            mTop = pVideoHeight * lYPositionRatio;
            mImageWidth = pVideoWidth * lWidthRatio;
            mImageHeight = pVideoHeight * lHeightRatio;

            string lCaptionString = row[6];

            if (lCaptionString.CompareTo(ProjectConstants.TRAINING_EVENT_STRING) == 0)
            {
                FrameEvent = true;
                FrameEventName = row[7];
                return;
            }
            else if (lCaptionString.CompareTo(ProjectConstants.TRAINING_IMAGE_CAPTION_STRING) == 0)
            {
                CaptionType = ProjectConstants.TRAINING_IMAGE_CAPTION_OPTION;
                string lImagePath = ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + row[7];
                OverlayImage = Image.FromFile(lImagePath);
            }
            else if (lCaptionString.CompareTo(ProjectConstants.TRAINING_OBJECT_CAPTION_STRING) == 0)
            {
                CaptionType = ProjectConstants.TRAINING_IMAGE_CAPTION_OPTION;
                string lKey = row[15];
                ItemsRating lRating = mParent.getRatingItem(lKey);
                if (lRating != null)
                    OverlayImage = lRating.getImage((int)mImageWidth, (int)mImageHeight,row);
                else
                    OverlayImage = Image.FromFile(ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + ProjectConstants.TRAINING_BLANK_SCORING_IMAGE_FILENAME);
            }
            else if (lCaptionString.CompareTo(ProjectConstants.TRAINING_PAUSE_CAPTION_STRING) == 0)
            {
                CaptionType = ProjectConstants.TRAINING_PAUSE_CAPTION_OPTION;
                PauseRequest = true;
            }
            else
            {
                CaptionType = ProjectConstants.TRAINING_TEXT_CAPTION_OPTION;
                OverlayText = row[7];
                int lFSize = (int)(Convert.ToDouble(row[11]) * pVideoWidth);
                FontStyle lFontStyle = new FontStyle();
                if (Convert.ToBoolean(row[13].ToLower()))
                    lFontStyle = FontStyle.Bold;
                if (Convert.ToBoolean(row[14].ToLower()))
                    lFontStyle = lFontStyle | FontStyle.Italic;

                FontOverlay = new Font(row[10], lFSize, lFontStyle,
                    System.Drawing.GraphicsUnit.Point);

                string lTextColor = row[12];
                switch (lTextColor)
                {
                    case "White": TextColor = System.Drawing.Brushes.White;
                        break;
                    case "Red": TextColor = System.Drawing.Brushes.Red;
                        break;
                    case "Green": TextColor = System.Drawing.Brushes.Green;
                        break;
                    case "Blue": TextColor = System.Drawing.Brushes.Blue;
                        break;
                    case "Black": TextColor = System.Drawing.Brushes.Black;
                        break;
                    default:
                        TextColor = System.Drawing.Brushes.White;
                        break;
                }

            }

            if (row.Length > 9 && row[8].CompareTo("") != 0)
                Flashing = Convert.ToBoolean(row[8].ToLower());
            if (row.Length > 10 && row[9].CompareTo("") != 0)
                Moving = Convert.ToBoolean(row[9].ToLower());
        }

        public bool isDisplayOn(int pVideoFrameNumber)
        {
            if (pVideoFrameNumber >= FrameNumber &&
                        pVideoFrameNumber <= FrameNumber + OverlayDuration)
            {
                if (!Flashing || pVideoFrameNumber%2 ==0)
                    return true;
            }

            return false;
        }

        public double getLeftPosition(int pVideoFrameNumber)
        {
            if (Moving)
                return mLeft + (pVideoFrameNumber - FrameNumber);
            else
                return mLeft;
        }

    }

    #endregion


    public class DynamicCaption
    {
        public DynamicCaption()
        {
        }
    }


    public class ItemsRating : DynamicCaption
    {
        private string mKeyValue;
        private int mNumStars;

        private static Image mRatingImage;

        public ItemsRating(string pKeyValue, int pNumStars)
        {
            mKeyValue = pKeyValue;
            mNumStars = pNumStars;

            if (mRatingImage == null)
            {
                string lImagePath = ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + ProjectConstants.TRAINING_RATING_IMAGE_FILENAME;
                mRatingImage = Image.FromFile(lImagePath);
            }
        }

        public Image getImage(int pWidth, int pHeight,string []row)
        {
            Bitmap lBitmap = new Bitmap(pWidth, pHeight);
            Graphics graphicsObject = Graphics.FromImage(lBitmap);

            int lFSize = (int)(pWidth * 0.05);
            FontStyle lFontStyle = new FontStyle();
                lFontStyle = FontStyle.Bold;
                lFontStyle = lFontStyle | FontStyle.Italic;

            Font lFontOverlay = new Font("Impact", lFSize, lFontStyle,
                System.Drawing.GraphicsUnit.Point);

            Image lBackGroundImage = Image.FromFile(ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + ProjectConstants.TRAINING_BLANK_SCORING_IMAGE_FILENAME);
            graphicsObject.DrawImage(lBackGroundImage,0, 0, pWidth,pHeight);

            for (int lStarNumber = 0; lStarNumber < mNumStars; lStarNumber++)
            {
                graphicsObject.DrawImage(mRatingImage, 
                    (float)(pWidth * 0.1) + (float)(lStarNumber *0.1 * pWidth), 
                    (int)(pHeight * 0.3),
                    (int)(pWidth * 0.08), 
                    (int)(pHeight * 0.3));
            }

            string displayString = row[16];
            graphicsObject.DrawString(displayString, lFontOverlay, System.Drawing.Brushes.Red,
                (int)(pWidth * 0.6), (int)(pHeight * 0.3), System.Drawing.StringFormat.GenericTypographic);

            string lDateString = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() +
                "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
            string lBitmapFilename = ProjectConstants.PROJECT_TEMP_PATH + @"\" + mKeyValue + lDateString + ".JPG";
            lBitmap.Save(lBitmapFilename);

            return Image.FromFile(lBitmapFilename);

        }

    }


}

/*
//            Bitmap lImageBitmap = new Bitmap(mCurrentOverlayImage);
//            lImageBitmap.MakeTransparent(bitmapOverlay.GetPixel(1, 1));

 *                 Type lBrushType = typeof(Brushes);
                TextColor = (Brush)lBrushType.InvokeMember("PeachPuff",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty,
                null, null, new object[] { });
 * 
*/
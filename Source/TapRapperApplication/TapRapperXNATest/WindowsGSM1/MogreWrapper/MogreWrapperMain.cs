using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Mogre;

using Utilities;
using ProjectCommon;

using DirectShowPlayer;
//using DirectShowLib;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

using System.Reflection;
using Logging;

//using WindowsFormsApplication1;

namespace MogreWrapper
{

    public class MogreWrapperMain
    {
        protected List<WiimoteOrgeControlShape> mDynamicShapes = new List<WiimoteOrgeControlShape>();

        public float WiimoteXPosition { get; set; }

        private SceneNode mWiimoteSceneNode;
        private SceneNode mWiimote1SceneNode;
        private SceneNode mWiimote2SceneNode;
        private SceneNode mCubeSceneNode;

        public float MaxRotation { get; set; }
        public float CurrentPitchRotationAngle { get; set; }
        public float ShoeRotationAngle { get; set; } //This value indicates the amount of rotation for the shoe
        //This sign of this value is also useful. The sign is used as a flag , which indicates the direction of the shoe rotation. Default direction is true which is value > 0 (Not sure which direction yet)
        //Reverse direction is value < 0
        //Flag value changes everytime shoe rotation changes. This was needed to avoid a rounding error in the initial shoe angle limit check

        public float InitialShoeRollAngle { get; set; }
        public float InitialShoePitchAngle { get; set; }
        protected float Angle360Rotation { get; set; }
        private Quaternion mInitialShoeAxis;

        private Capture cam = null;

        private int mvideoWidth;
        private int mvideoHeight;

        private Entity mCubeEntity;
        private Texture mCubeTexture;
//        private Bitmap mTextureBitmap;
        private Queue<Bitmap> mTextureBitmapList;
        private Graphics mVideoGraphics;
        public int TexWidth { get; set; }
        public int TexHeight { get; set; }
        private Quaternion mQuatDirection;
        private Vector3 mTapNodePosition;

        public int mStepIndex = 0;

        private Root mRoot;
        private SceneManager mSceneManager;
        protected SceneNode mNode;

        protected Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving

        protected Form mParentWindow;
        protected float mWidth;
        protected float mHeight;

        protected float mWalkSpeed = 100.0f;  // The speed at which the object is moving

        public MogreWrapperMain()
        {
            WiimoteXPosition = +15f;
            MaxRotation = 30;
            CurrentPitchRotationAngle = 0;
            ShoeRotationAngle = 5;
            Angle360Rotation = 1;

            TexWidth = 0;
            TexHeight = 0;

            mTextureBitmapList = new Queue<Bitmap>();
        }

        public void Initialize(Root pRoot, SceneManager pSceneManager, Form pParentWindow, float pWidth, float pHeight)
        {
            mRoot = pRoot;
            mSceneManager = pSceneManager;
            mParentWindow = pParentWindow;

            mWidth = pWidth;
            mHeight = pHeight;
        }

        private void drawShoe(SceneNode p_Node, string shoeName)
        {
            // Create knot objects so we can see movement
            Entity l_Entity = mSceneManager.CreateEntity(shoeName, "sole01_SOLE.002.mesh");
            l_Entity.SetMaterialName("Examples/Fish");
            p_Node.AttachObject(l_Entity);
        }

        private void drawCircle(SceneNode p_Node, string name)
        {
            SceneNode l_ShoeTargetNode = p_Node.CreateChildSceneNode(p_Node.Name + name, new Vector3(0f, 0f, 0f));
            Entity l_Entity = mSceneManager.CreateEntity(name, SceneManager.PrefabType.PT_SPHERE);
            l_Entity.SetMaterialName("Examples/Fish");
            l_ShoeTargetNode.AttachObject(l_Entity);
            l_ShoeTargetNode.Scale(0.1f, 0.1f, 0.1f);
        }

        #region Video Embedding

        protected void drawLine(SceneNode p_Node, string lineName, Vector3 p_Point1, Vector3 p_Point2)
        {
            // create material (colour)
            MaterialPtr moMaterial = MaterialManager.Singleton.Create(lineName, "debugger");
            moMaterial.ReceiveShadows = false;
            moMaterial.GetTechnique(0).SetLightingEnabled(true);
            moMaterial.GetTechnique(0).GetPass(0).SetDiffuse(0, 0, 1, 0);
            moMaterial.GetTechnique(0).GetPass(0).SetAmbient(0, 0, 1);
            moMaterial.GetTechnique(0).GetPass(0).SetSelfIllumination(0, 0, 1);
            moMaterial.Dispose();  // dispose pointer, not the material


            // create line object
            ManualObject manOb = mSceneManager.CreateManualObject(lineName);
            manOb.Begin(lineName, RenderOperation.OperationTypes.OT_LINE_LIST);
            manOb.Position(p_Point1.x, p_Point1.y, p_Point1.z);
            manOb.Position(p_Point2.x, p_Point2.y, p_Point2.z);
            // ... maybe more points
            manOb.End();

            // create SceneNode and attach the line
            //            p_Node.Position = Vector3.ZERO;
            p_Node.AttachObject(manOb);
        }


        private void setTextureDimensions(bool p_DontModifyDimensions)
        {
            // get width and height to the next square of two
            int twoSquared;

            for (twoSquared = 2; TexWidth == 0 || TexHeight == 0; twoSquared *= 2)
            {
                if (TexWidth == 0 && twoSquared >= mvideoWidth)
                    TexWidth = twoSquared;
                if (TexHeight == 0 && twoSquared >= mvideoHeight)
                    TexHeight = twoSquared;
            }
            if (p_DontModifyDimensions)
            {
                // back to the original dimensions
                TexWidth = mvideoWidth;
                TexHeight = mvideoHeight;
            }
        }

        protected unsafe void setupVideoGraphicsObject()
        {

            this.mCubeTexture = TextureManager.Singleton.CreateManual(
                "MyCubeTexture",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                TextureType.TEX_TYPE_2D,
                (uint)this.TexWidth,
                (uint)this.TexHeight,
                0,
                Mogre.PixelFormat.PF_A8R8G8B8);

            MaterialPtr inMat = MaterialManager.Singleton.Create("MyCubeMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            inMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("MyCubeTexture");
            mCubeEntity.SetMaterialName("MyCubeMat");

            // draw bitmap to texture
            HardwarePixelBufferSharedPtr texBuffer = this.mCubeTexture.GetBuffer();
            texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
            PixelBox pb = texBuffer.CurrentLock;

            using (Bitmap bm = new Bitmap(
                (int)this.mCubeTexture.Width,
                (int)this.mCubeTexture.Height,
                (int)((this.mCubeTexture.Width * 4) + (pb.RowSkip * 4)),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                pb.data))
            {
                mVideoGraphics = Graphics.FromImage(bm);
            }

            texBuffer.Unlock();
            texBuffer.Dispose();
        }

        protected void setupVideo()
        {
            mCubeSceneNode = mSceneManager.RootSceneNode.CreateChildSceneNode("CubeNode",
                new Vector3(0f, 0f, 0f));
            // Create knot objects so we can see movement
//            mCubeEntity = mSceneManager.CreateEntity("CubeShape", SceneManager.PrefabType.PT_CUBE);
            mCubeEntity = mSceneManager.CreateEntity("CubeShape", SceneManager.PrefabType.PT_PLANE);
            mCubeSceneNode.AttachObject(mCubeEntity);
            mCubeSceneNode.Scale(1.5f, 1.5f, 1.5f);

            mvideoWidth = 1024;
            mvideoHeight = 768;

            setTextureDimensions(false);

            setupVideoGraphicsObject();

            Thread lRenderVideoThread = new Thread(new ThreadStart(renderVideoThread));
            lRenderVideoThread.Start();

            cam = new Capture(Configuration.getConfiguration().TrainingVideoFile, "Test String", mParentWindow, false, new VideoUserOptions(),null);
            cam.VideoBufferReceivedEvent += new Capture.OnVideoBufferReceivedEvent(OnVideoBufferReceivedEvent);
            cam.Start();
        }

        private unsafe void ConvertBitmap(Bitmap pTextureBitmap)
        {
            
            if (this.mCubeTexture == null)
            {
                this.mCubeTexture = TextureManager.Singleton.CreateManual(
                    "MyCubeTexture",
                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                    TextureType.TEX_TYPE_2D,
                    (uint)this.TexWidth,
                    (uint)this.TexHeight,
                    0,
                    Mogre.PixelFormat.PF_A8R8G8B8);

                MaterialPtr inMat = MaterialManager.Singleton.Create("MyCubeMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                inMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("MyCubeTexture");
                mCubeEntity.SetMaterialName("MyCubeMat");
            }

            if (pTextureBitmap != null)
            {
                // draw bitmap to texture
                HardwarePixelBufferSharedPtr texBuffer = this.mCubeTexture.GetBuffer();

                texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
                PixelBox pb = texBuffer.CurrentLock;

                using (Bitmap bm = new Bitmap(
                    (int)this.mCubeTexture.Width,
                    (int)this.mCubeTexture.Height,
                    (int)((this.mCubeTexture.Width * 4) + (pb.RowSkip * 4)),
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    pb.data))
                {
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        //g.DrawImage(this.mTextureBitmap, 0, 0);
                        g.DrawImage(pTextureBitmap, new System.Drawing.Rectangle(0, 0, (int)this.mCubeTexture.Width, (int)this.mCubeTexture.Height));
                    }
                }

                texBuffer.Unlock();
                texBuffer.Dispose();
            }
            

            /*
            // draw bitmap to texture
            HardwarePixelBufferSharedPtr texBuffer = this.mCubeTexture.GetBuffer();
            texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
            mVideoGraphics.DrawImage(this.mTextureBitmap, new System.Drawing.Rectangle(0, 0, (int)this.mCubeTexture.Width, (int)this.mCubeTexture.Height));
            texBuffer.Unlock();
             */
        }

        private void setMaterialTexture(Entity p_Entity)
        {
            MaterialPtr inMat = MaterialManager.Singleton.Create("MyCubeMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            inMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("MyCubeTexture");
            mCubeEntity.SetMaterialName("MyCubeMat");
        }


        public void OnVideoBufferReceivedEvent(object sender, VideoBufferEventArgs evt)
        {
            Bitmap lTextureBitmap = (Bitmap)evt.mVideoBufferBitmap.Clone();
            mTextureBitmapList.Enqueue(lTextureBitmap);
        }

        public void renderVideoThread()
        {
            while (true)
            {
                if (mTextureBitmapList.Count == 0)
                    Thread.Sleep(1);
                else
                {
                    Bitmap lTextureBitmap = mTextureBitmapList.Dequeue();
                    ConvertBitmap(lTextureBitmap);
                }
            }
        }

        #endregion Video Embedding

        #region Scene Creation

        protected void setupLighting()
        {
            Light light1 = mSceneManager.CreateLight("Light1");
            light1.Type = Light.LightTypes.LT_SPOTLIGHT;
            light1.DiffuseColour = ColourValue.Red;
            light1.SpecularColour = ColourValue.Red;

            light1.Direction = new Vector3(0, -1, 0);
            light1.Position = new Vector3(0, 30, 0);

            light1.SetSpotlightRange(new Degree(35), new Degree(50));

            Light light2 = mSceneManager.CreateLight("Light2");
            light2.Type = Light.LightTypes.LT_SPOTLIGHT;
            light2.DiffuseColour = ColourValue.Red;
            light2.SpecularColour = ColourValue.Red;

            light2.Direction = new Vector3(0, 1, 0);
            light2.Position = new Vector3(0, -30, 0);

            light2.SetSpotlightRange(new Degree(35), new Degree(50));

            Light light3 = mSceneManager.CreateLight("Light3");
            light3.Type = Light.LightTypes.LT_SPOTLIGHT;
            light3.DiffuseColour = ColourValue.White;
            light3.SpecularColour = ColourValue.White;

            light3.Direction = new Vector3(0, 0, -1);
            light3.Position = new Vector3(0, 0, 5);

            light3.SetSpotlightRange(new Degree(100), new Degree(150));

            Light mainLight = mSceneManager.CreateLight("MainLight");
            mainLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            mainLight.DiffuseColour = new ColourValue(.25f, .25f, 0);
            mainLight.SpecularColour = new ColourValue(.25f, .25f, 0);
            mainLight.Direction = new Vector3(0, -1, 0);

        }

        protected void setupTapMotion()
        {
            float l_Wiimote1XPosition = -1 * mWidth / 32;
            float l_Wiimote2XPosition = 1 * mWidth / 32;

            float l_StartingYPosition = mHeight / 1.5f;
            float l_EndingYPosition = -1 * mHeight / 1.5f;

            float l_StartingZPosition = -1 * mWidth / 2;
            float l_EndingZPosition = mWidth / 2;

            mNode = mSceneManager.RootSceneNode.CreateChildSceneNode("WiimoteNode",
                new Vector3(0f, 0f, 0f));

            SceneNode l_WimmoteTapMotionNode = mNode.CreateChildSceneNode("WiimoteTapMotionNode",
                new Vector3(0f, 0f, 0f));

            SceneNode l_Wiimote1LineNode = l_WimmoteTapMotionNode.CreateChildSceneNode("Wiimote1LineNode",
                new Vector3(0f, 0f, 0f));
            drawLine(l_Wiimote1LineNode, "Wiimote1LineNode", new Vector3(0f, l_StartingYPosition, l_StartingZPosition),
                new Vector3(0f, l_EndingYPosition, l_EndingZPosition));
            SceneNode l_Wiimote1LineShoeNode = l_Wiimote1LineNode.CreateChildSceneNode("Wiimote1LineShoeNode", new Vector3(0f, l_EndingYPosition / 8, l_EndingZPosition / 8));
            drawCircle(l_Wiimote1LineShoeNode, "WiimoteLine1Shoe");
            l_Wiimote1LineNode.Translate(l_Wiimote1XPosition, 0f, 0f);
            l_Wiimote1LineNode.Translate(0, 0f, 0f);

            SceneNode l_Wiimote2LineNode = l_WimmoteTapMotionNode.CreateChildSceneNode("Wiimote2LineNode",
                new Vector3(0f, 0f, 0f));
            drawLine(l_Wiimote2LineNode, "Wiimote2LineNode", new Vector3(0, l_StartingYPosition, l_StartingZPosition),
                new Vector3(0, l_EndingYPosition, l_EndingZPosition));
            SceneNode l_Wiimote2LineShoeNode = l_Wiimote2LineNode.CreateChildSceneNode("Wiimote2LineShoeNode", new Vector3(0f, l_EndingYPosition / 8, l_EndingZPosition / 8));
            drawCircle(l_Wiimote2LineShoeNode, "WiimoteLine2Shoe");
            l_Wiimote2LineNode.Translate(l_Wiimote2XPosition, 0f, 0f);

            mWiimote1SceneNode = l_Wiimote1LineShoeNode.CreateChildSceneNode("Wiimote1SceneNode",
                new Vector3(0f, 0f, 0f));

            mWiimote2SceneNode = l_Wiimote2LineShoeNode.CreateChildSceneNode("Wiimote2SceneNode",
                new Vector3(0f, 0f, 0f));

            WiimoteOrgeControlCSVParser l_Parser = new WiimoteOrgeControlCSVParser();
            l_Parser.loadShapeData(mDynamicShapes);

            int l_ShapeIndex = 0;
            SceneNode l_SceneNode = null;

            Mogre.Degree l_ShoeYawAngle = new Mogre.Degree(90);
            Mogre.Degree l_ShoePitchAngle = new Mogre.Degree(-20);

            Mogre.Degree l_DegreeRollAngle = new Mogre.Degree(30);


            foreach (WiimoteOrgeControlShape l_Shape in mDynamicShapes)
            {
                l_ShapeIndex++;

                if (l_Shape.Foot.CompareTo(ProjectCommon.ProjectConstants.SHAPE_LEFT_FOOT) == 0)
                    l_SceneNode = mWiimote1SceneNode.CreateChildSceneNode("Wiimote1Node" + l_ShapeIndex, new Vector3(0,//l_Wiimote1XPosition,
                                                                                                                    l_StartingYPosition * l_Shape.Time,
                                                                                                                    l_StartingZPosition * l_Shape.Time));
                else
                    l_SceneNode = mWiimote2SceneNode.CreateChildSceneNode("Wiimote2Node" + l_ShapeIndex, new Vector3(0,//l_Wiimote2XPosition,
                                                                                                                    l_StartingYPosition * l_Shape.Time,
                                                                                                                    l_StartingZPosition * l_Shape.Time));               
                l_SceneNode.Scale(2f, 2f, 2f);
                l_SceneNode.Yaw(l_ShoeYawAngle);
//                l_SceneNode.Pitch(l_ShoePitchAngle);
//                l_SceneNode.Roll(30);
                 
                InitialShoeRollAngle = l_SceneNode.Orientation.Roll.ValueDegrees;
                InitialShoePitchAngle = l_SceneNode.Orientation.Pitch.ValueDegrees;

                mInitialShoeAxis = l_SceneNode.Orientation;
                mInitialShoeAxis.w = InitialShoeRollAngle;

                drawShoe(l_SceneNode, "Shoe" + l_ShapeIndex);
  
                //TODO : Need to get back movable text
                // create entity
//                MovableText msg = new MovableText("txt001", l_Shape.Message, "BlueHighway", 6, new ColourValue(200, 50, 200));
//                msg.SetTextAlignment(MovableText.HorizontalAlignment.H_CENTER, MovableText.VerticalAlignment.V_ABOVE);
//                msg.AdditionalHeight = 8.0f;
                                
                // attach to a SceneNode
//                l_SceneNode.AttachObject(msg);


            }
            



            /*
            //            SceneNode l_TestShoeNode = mNode.CreateChildSceneNode("TestShoeNode",new Vector3(0f, 0f, 0f));
                        SceneNode l_TestShoeNode = SceneManager.RootSceneNode.CreateChildSceneNode("TestShoeNode",new Vector3(0f, 0f, 0f));
            
                        drawShoe(l_TestShoeNode,"TestShoe");
                        l_TestShoeNode.Scale(5f, 5f, 5f);
                        Mogre.Degree l_ShoeYawAngle = new Mogre.Degree(90);
                        l_TestShoeNode.Yaw(l_ShoeYawAngle.ValueRadians);
                        Mogre.Degree l_ShoePitchAngle = new Mogre.Degree(-20);
                        l_TestShoeNode.Pitch(l_ShoePitchAngle.ValueRadians);
                        */

            mNode.Translate(new Vector3(mWidth / 8, 0f, 0f));

            Mogre.Degree l_DegreeAngle = new Mogre.Degree(-15);
            //            l_WimmoteTapMotionNode.Yaw(l_DegreeAngle.ValueRadians);
            mQuatDirection = new Quaternion(l_DegreeAngle.ValueRadians, Vector3.UNIT_Z);

            mNode.Rotate(mQuatDirection);

            mDirection = new Vector3(0f, l_EndingYPosition / 8, l_EndingZPosition / 8).NormalisedCopy;

            mTapNodePosition = mDirection;
        }

        public void CreateSceneManager()
        {
//            setupTapMotion();

            setupLighting();

            setupVideo();
        }

        # endregion


        public bool FrameStarted(FrameEvent evt)
        {
            if (mWiimote1SceneNode.NumChildren() == 0)
                return true;

            float move = mWalkSpeed * evt.timeSinceLastFrame;

            mWiimote1SceneNode.Translate(mDirection * move);
            mWiimote2SceneNode.Translate(mDirection * move);

            SceneNode l_CheckNode = (SceneNode)mWiimote1SceneNode.GetChild(0);

            if ((l_CheckNode.Orientation.Pitch.ValueDegrees >= (InitialShoePitchAngle + MaxRotation) && ShoeRotationAngle > 0) ||
                (l_CheckNode.Orientation.Pitch.ValueDegrees < (InitialShoePitchAngle - MaxRotation) && ShoeRotationAngle < 0))
            {
                ShoeRotationAngle = ShoeRotationAngle * -1;
            }


//            Console.Out.WriteLine("Initial Angle = " + InitialShoeRollAngle + " : Orientation = " + l_CheckNode.Orientation.Roll.ValueDegrees + "  mShoeRotationAngle = " + ShoeRotationAngle);
            Console.Out.WriteLine("Roll = " + l_CheckNode.Orientation.Roll.ValueDegrees + " : Pitch = " + l_CheckNode.Orientation.Pitch.ValueDegrees + " : Yaw = " + l_CheckNode.Orientation.Yaw.ValueDegrees + " : ShoeRotationAngle = " + ShoeRotationAngle);

            Mogre.Degree l_DegreeAngle = new Mogre.Degree(5);

            Node.ChildNodeIterator l_Iterator1 = mWiimote1SceneNode.GetChildIterator();
            
            foreach (SceneNode l_Node in l_Iterator1)
            {
//                l_Node.Pitch(l_DegreeAngle.ValueRadians,  Node.TransformSpace.TS_PARENT);
                l_Node.Pitch(l_DegreeAngle.ValueRadians);
            }


            Node.ChildNodeIterator l_Iterator2 = mWiimote2SceneNode.GetChildIterator();
            foreach (SceneNode l_Node in l_Iterator2)
            {
//                l_Node.Pitch(l_DegreeAngle.ValueRadians,  Node.TransformSpace.TS_PARENT);
                l_Node.Pitch(l_DegreeAngle.ValueRadians);
            }

            Mogre.Degree l_VideoAngle = new Mogre.Degree(Angle360Rotation);
            if (mCubeSceneNode != null)
                mCubeSceneNode.Yaw(l_VideoAngle.ValueRadians);

            return true;
        }
        
        #region Property Mapping

        public SimpleProperty[] MogrePropertyList
        {
            get
            {
                Type lType = this.GetType();
                PropertyInfo[] lPropertyList = lType.GetProperties();

                // -1 To exclude MogrePropertyList
                SimpleProperty[] lPropertyTable = new SimpleProperty[lPropertyList.Length -1];

                for (int index = 0; index < lPropertyList.Length; index++)
                {
                    if (lPropertyList[index].DeclaringType.Name.CompareTo("MogreWrapperMain") == 0 &&
                        lPropertyList[index].Name.CompareTo("MogrePropertyList") != 0)
                    {
                        lPropertyTable[index] = new SimpleProperty();
                        lPropertyTable[index].Name = lPropertyList[index].Name;
                        lPropertyTable[index].Value = lPropertyList[index].GetValue(this, null);
                    }
                }

                return lPropertyTable;
                //PropertyInfo[] lPropertyList = mType.GetProperties();
            }
            set
            {
                if (value == null) return;
                SimpleProperty[] lPropertyTable = (SimpleProperty[])value;

                Type lType = this.GetType();

                for (int index = 0; index < lPropertyTable.Length; index++)
                {
                    string lName = lPropertyTable[index].Name;
                    PropertyInfo pInfo = lType.GetProperty(lName);

                    if (pInfo.PropertyType.FullName.CompareTo("System.Single") == 0)
                    {
                        System.Single propertyValue = (float)Convert.ToDouble(lPropertyTable[index].Value.ToString());
                        pInfo.SetValue(this, propertyValue, null);
                    }
                    else if (pInfo.PropertyType.FullName.CompareTo("System.Int32") == 0)
                    {
                        System.Int32 propertyValue = Convert.ToInt32(lPropertyTable[index].Value.ToString());
                        pInfo.SetValue(this, propertyValue, null);
                    }


                }

            }
        }
        #endregion
        

    }


    public class WiimoteOrgeControlShape
    {
        public string Foot;
        public string Message;
        public float Time;

        public static WiimoteOrgeControlShape getShape(string[] row)
        {
            WiimoteOrgeControlShape l_Shape = new WiimoteOrgeControlShape();
            l_Shape.Foot = row[ProjectConstants.SHAPE_FOOT_INDEX];
            l_Shape.Message = row[ProjectConstants.SHAPE_STEP_NAME_INDEX];
            l_Shape.Time = (float)Convert.ToDouble(row[ProjectConstants.SHAPE_TIME_INDEX]);

            return l_Shape;
        }
    }

    class WiimoteOrgeControlCSVParser
    {
        private CSVFileParser m_Parser;

        public void loadShapeData(List<WiimoteOrgeControlShape> p_DynamicShapes)
        {
            m_Parser = new CSVFileParser();
            m_Parser.startParsingCSVData(ProjectConstants.SHAPE_LIST_PATH, 0, 0);

            string[] row;

            while ((row = m_Parser.getNextRow(0)) != null)
            {
                WiimoteOrgeControlShape l_Shape = WiimoteOrgeControlShape.getShape(row);
                p_DynamicShapes.Add(l_Shape);
            }
        }
    }

}

/*
    public class MogrePropertyDataSource
    {
        private Type mType;
        private object mPropertyObject;

        MogrePropertyDataSource(object pPropertyObject)
        {
            mType = pPropertyObject.GetType();
            mPropertyObject = pPropertyObject;
        }

        protected PropertyInfo[] getPropertyList()
        {
            return mType.GetProperties();
            //PropertyInfo[] lPropertyList = mType.GetProperties();
        }

        public void setProperty(string pName, string pValue)
        {
            PropertyInfo lPropertyInfo = mType.GetProperty(pName);
            lPropertyInfo.SetValue(mPropertyObject, pValue, null);
        }

        public SimpleProperty[] MogrePropertyList
        {
            get
            {
                Type lType = this.GetType();
                PropertyInfo[] lPropertyList = lType.GetProperties();

                SimpleProperty[] lPropertyTable = new SimpleProperty[lPropertyList.Length];
                return lPropertyTable;
            }
            set
            {

            }
        }
    }
*/
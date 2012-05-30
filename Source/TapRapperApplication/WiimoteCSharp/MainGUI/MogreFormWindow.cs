using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace MainGUI
{

    internal class DefaultInputHandler
    {
        // Fields
        private const int INTERVAL = 0x11;
        private bool mLastFocus;
        private Point mLastLocation;
        protected float mRot = -0.2f;
        private bool mRotating;
        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
        protected float mTrans = 10f;
        protected Vector3 mTranslate = Vector3.ZERO;
        internal Form1 mWindow;

        // Methods
        internal DefaultInputHandler(Form1 win)
        {
            this.mWindow = win;
            win.KeyDown += new KeyEventHandler(this.HandleKeyDown);
            win.KeyUp += new KeyEventHandler(this.HandleKeyUp);
            win.MouseDown += new MouseEventHandler(this.HandleMouseDown);
            win.MouseUp += new MouseEventHandler(this.HandleMouseUp);
            win.Disposed += new EventHandler(this.win_Disposed);
            win.LostFocus += new EventHandler(this.win_LostFocus);
            win.GotFocus += new EventHandler(this.win_GotFocus);
            this.mTimer.Interval = 0x11;
            this.mTimer.Enabled = true;
            this.mTimer.Tick += new EventHandler(this.Timer_Tick);
        }

        protected virtual void HandleKeyDown(object sender, KeyEventArgs e)
        {
            float mTrans = this.mTrans;
            switch (e.KeyCode)
            {
                case Keys.Q:
                case Keys.Prior:
                    this.mTranslate.y = mTrans;
                    return;

                case Keys.R:
                case Keys.End:
                case Keys.Home:
                case Keys.B:
                case Keys.C:
                    break;

                case Keys.S:
                case Keys.Down:
                    this.mTranslate.z = mTrans;
                    return;

                case Keys.W:
                case Keys.Up:
                    this.mTranslate.z = -mTrans;
                    return;

                case Keys.Next:
                case Keys.E:
                    this.mTranslate.y = -mTrans;
                    break;

                case Keys.Left:
                case Keys.A:
                    this.mTranslate.x = -mTrans;
                    return;

                case Keys.Right:
                case Keys.D:
                    this.mTranslate.x = mTrans;
                    return;

                default:
                    return;
            }
        }

        protected virtual void HandleKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                case Keys.Prior:
                case Keys.Next:
                case Keys.E:
                    this.mTranslate.y = 0f;
                    break;

                case Keys.R:
                case Keys.End:
                case Keys.Home:
                case Keys.B:
                case Keys.C:
                    break;

                case Keys.S:
                case Keys.W:
                case Keys.Up:
                case Keys.Down:
                    this.mTranslate.z = 0f;
                    return;

                case Keys.Left:
                case Keys.Right:
                case Keys.A:
                case Keys.D:
                    this.mTranslate.x = 0f;
                    return;

                default:
                    return;
            }
        }

        protected virtual void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Hide();
                this.mRotating = true;
            }
        }

        private void HandleMouseMove(Point delta)
        {
            if (this.mRotating)
            {
                this.mWindow.Camera.Yaw(new Degree(delta.X * this.mRot));
                this.mWindow.Camera.Pitch(new Degree(delta.Y * this.mRot));
            }
        }

        protected virtual void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Show();
                this.mRotating = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.mLastFocus)
            {
                Point position = Cursor.Position;
                position.X -= this.mLastLocation.X;
                position.Y -= this.mLastLocation.Y;
                this.HandleMouseMove(position);
            }
            this.mLastLocation = Cursor.Position;
            this.mLastFocus = this.mWindow.Focused;
            if (this.mLastFocus)
            {
                Camera camera = this.mWindow.Camera;
                camera.Position += this.mWindow.Camera.Orientation * this.mTranslate;
            }
        }

        private void win_Disposed(object sender, EventArgs e)
        {
            this.mTimer.Enabled = false;
        }

        private void win_GotFocus(object sender, EventArgs e)
        {
            this.mTimer.Enabled = true;
        }

        private void win_LostFocus(object sender, EventArgs e)
        {
            this.mTimer.Enabled = false;
            this.mTranslate = Vector3.ZERO;
        }
    }


    public partial class Form1
    {
        // Fields
        private Camera mCamera;
        private Root mRoot;
        private SceneManager mSceneMgr;
        private Viewport mViewport;
        private RenderWindow mWindow;

        // Events
        public event SceneEventHandler SceneCreating;

        protected virtual void CreateCamera()
        {
            this.mCamera = this.mSceneMgr.CreateCamera("MainCamera");
            this.mCamera.NearClipDistance = 1f;
            this.mCamera.Position = new Vector3(0f, 0f, 300f);
            this.mCamera.LookAt(Vector3.ZERO);
        }

        protected virtual void CreateInputHandler()
        {
            new DefaultInputHandler(this);
        }

        protected virtual void CreateRenderWindow(IntPtr handle)
        {
            this.mRoot.Initialise(false, "Main Ogre Window");
            NameValuePairList miscParams = new NameValuePairList();
            if (handle != IntPtr.Zero)
            {
                miscParams["externalWindowHandle"] = handle.ToString();
                this.mWindow = this.mRoot.CreateRenderWindow("Autumn main RenderWindow", 800, 600, false, miscParams);
            }
            else
            {
                this.mWindow = this.mRoot.CreateRenderWindow("Autumn main RenderWindow", 800, 600, false);
            }
        }

        protected virtual void CreateSceneManager()
        {
            this.mSceneMgr = this.mRoot.CreateSceneManager(SceneType.ST_GENERIC, "Main SceneManager");
        }

        protected virtual void CreateViewport()
        {
            this.mViewport = this.mWindow.AddViewport(this.mCamera);
            this.mViewport.BackgroundColour = new ColourValue(0f, 0f, 0f, 1f);
        }


        public void Initialize()
        {
            if (this.mRoot == null)
            {
                this.InitializeOgre(this.ogrePanel.Handle);
            }
//            base.Show();
        }

        public void Go()
        {
            if (this.mRoot == null)
            {
                this.InitializeOgre(this.ogrePanel.Handle);
            }

            base.Show();

            bool flag = true;
            
            while (flag && (this.mRoot != null))
            {
                flag = this.mRoot.RenderOneFrame();
                Application.DoEvents();
            }
        }

        public void StartEvents()
        {
            bool flag = true;
            while (flag && (this.mRoot != null))
            {
                flag = this.mRoot.RenderOneFrame();
                Application.DoEvents();
            }
        }

        private void InitializeMogreComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x318, 0x23d);
            base.FormBorderStyle = FormBorderStyle.Fixed3D;
            base.MaximizeBox = false;
            base.Name = "OgreWindow";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Mogre Render Window";
            base.ResumeLayout(false);
        }

        public void InitializeOgre(IntPtr handle)
        {
            if (this.mRoot != null)
            {
                throw new Exception("Ogre is already initialized!");
            }
            Splash splash = new Splash();
            splash.Show();
            try
            {
                splash.Increment("Creating the root object...");
                this.mRoot = new Root();
//                this.mRoot.RenderSystem._setCullingMode(CullingMode.CULL_CLOCKWISE);
                splash.Increment("Loading resources...");
                this.InitResources();
                splash.Increment("Setting up DirectX...");
                this.SetupDirectX();
                splash.Increment("Creating the window...");
                this.CreateRenderWindow(handle);
                splash.Increment("Initializing resources...");
                ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
                splash.Increment("Creating Ogre objects...");
                this.CreateSceneManager();
                this.CreateCamera();
                this.CreateViewport();
                splash.Increment("Creating input handler...");
                this.CreateInputHandler();
                splash.Increment("Creating scene...");
                base.Disposed += new EventHandler(this.OgreWindow_Disposed);
                this.OnSceneCreating();
            }
            finally
            {
                splash.Close();
                splash.Dispose();
            }
        }

        protected virtual void InitResources()
        {
            ConfigFile file = new ConfigFile();
            file.Load("resources.cfg", "\t:=", true);
            ConfigFile.SectionIterator sectionIterator = file.GetSectionIterator();
            while (sectionIterator.MoveNext())
            {
                string currentKey = sectionIterator.CurrentKey;
                foreach (KeyValuePair<string, string> pair in sectionIterator.Current)
                {
                    string key = pair.Key;
                    string name = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(name, key, currentKey);
                }
            }
        }

        private void OgreWindow_Disposed(object sender, EventArgs e)
        {
            this.mRoot.Dispose();
            this.mRoot = null;
            this.mWindow = null;
            this.mCamera = null;
            this.mViewport = null;
            this.mSceneMgr = null;
        }

        protected virtual void OnSceneCreating()
        {
            if (this.SceneCreating != null)
            {
                this.SceneCreating(this);
            }
        }

        private void SetupDirectX()
        {
            RenderSystem renderSystemByName = this.mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
            this.mRoot.RenderSystem = renderSystemByName;
            renderSystemByName.SetConfigOption("Full Screen", "No");
            renderSystemByName.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
        }

        // Properties
        public Camera Camera
        {
            get
            {
                return this.mCamera;
            }
            protected set
            {
                this.mCamera = value;
            }
        }

        public RenderWindow RenderWindow
        {
            get
            {
                return this.mWindow;
            }
            protected set
            {
                this.mWindow = value;
            }
        }

        public Root Root
        {
            get
            {
                return this.mRoot;
            }
        }

        public SceneManager SceneManager
        {
            get
            {
                return this.mSceneMgr;
            }
            protected set
            {
                this.mSceneMgr = value;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return this.mViewport;
            }
            protected set
            {
                this.mViewport = value;
            }
        }

        // Nested Types
        public delegate void SceneEventHandler(Form1 win);
    }

    internal class Splash : Form
    {
        // Fields
        private IContainer components;
        private Label LoadingText;
        private ProgressBar Progress;

        // Methods
        public Splash()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Increment(string text)
        {
            this.Progress.Increment(1);
            this.LoadingText.Text = text;
            base.Update();
            Application.DoEvents();
        }

        private void InitializeComponent()
        {
            this.LoadingText = new Label();
            this.Progress = new ProgressBar();
            base.SuspendLayout();
            this.LoadingText.AutoEllipsis = true;
            this.LoadingText.BackColor = Color.Transparent;
            this.LoadingText.ForeColor = Color.White;
            this.LoadingText.Location = new Point(9, 0x9f);
            this.LoadingText.Name = "LoadingText";
            this.LoadingText.Size = new Size(0x178, 13);
            this.LoadingText.TabIndex = 0;
            this.LoadingText.Text = "Loading...";
            this.LoadingText.UseWaitCursor = true;
            this.Progress.Location = new Point(12, 0xaf);
            this.Progress.Maximum = 8;
            this.Progress.Name = "Progress";
            this.Progress.Size = new Size(0x175, 14);
            this.Progress.Step = 1;
            this.Progress.TabIndex = 1;
            this.Progress.UseWaitCursor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
//            this.BackgroundImage = Resources.Splash;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(400, 200);
            base.ControlBox = false;
            base.Controls.Add(this.Progress);
            base.Controls.Add(this.LoadingText);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "Splash";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            base.UseWaitCursor = true;
            base.ResumeLayout(false);
        }

        public void Show()
        {
            base.Show();
            Application.DoEvents();
        }

    }
}

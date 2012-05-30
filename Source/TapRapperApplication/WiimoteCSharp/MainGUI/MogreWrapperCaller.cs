using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

using Utilities;
using ProjectCommon;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

using System.Reflection;

using MogreWrapper;

//using WindowsFormsApplication1;

namespace MainGUI
{

    public abstract class OgreControl : Form1
    {
        protected AnimationState mAnimationState = null; //The AnimationState the moving object
        protected float mDistance = 0.0f;              //The distance the object has left to travel
        protected Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards
        protected LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints

        protected bool mWalking = false;
        protected Entity mEntity;
        protected SceneNode mNode;

        protected float m_Width;
        protected float m_Height;

        protected MogreWrapperMain mMogreWrapper;

        internal OgreControl()
            : base()
        {
            //            m_Width = this.ogrePanel.Width;
            //            m_Height = this.ogrePanel.Height;

            m_Width = 640;
            m_Height = 480;
        }


        protected override void CreateSceneManager()
        {
            // Create SceneManager
            SceneManager = Root.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);

            //Set ambient light
            SceneManager.AmbientLight = ColourValue.White;

        }

        protected override void CreateInputHandler()
        {
            base.CreateInputHandler();
//            this.Root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);

        }

        void consoleOutput(string msg)
        {
            //            Console.Out.WriteLine(msg);
        }

        abstract public bool FrameStarted(FrameEvent evt);
    }

    public class WiimoteOgreControl : OgreControl
    {

        delegate void SetMogrePropertyBindingSourceCallback(object[] dataStore);

        public WiimoteOgreControl()
            : base()
        {
            mMogreWrapper = new MogreWrapperMain();
            this.mogreWrapperMainBindingSource.DataSource = mMogreWrapper.MogrePropertyList;
        }

        protected override void CreateSceneManager()
        {
            base.CreateSceneManager();

            mMogreWrapper.Initialize(Root, SceneManager, (Form) this,m_Width, m_Height);
            mMogreWrapper.CreateSceneManager();

            //            setupVideo();
        }

        public override bool FrameStarted(FrameEvent evt)
        {
            return mMogreWrapper.FrameStarted(evt);
        }

        protected override void setMogreProperties()
        {
            if (mMogreWrapper != null)
                mMogreWrapper.MogrePropertyList = (SimpleProperty[])mogreWrapperMainBindingSource.DataSource;
        }

        internal void SetMogrePropertyBindingSource(object[] dataStore)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.MogrePropertyView.InvokeRequired)
            {
                SetMogrePropertyBindingSourceCallback d = new SetMogrePropertyBindingSourceCallback(SetMogrePropertyBindingSource);
                this.Invoke(d, new object[] { dataStore });
            }
            else
            {
                this.wiimoteReferenceDataStoreBindingSource.DataSource = (SimpleProperty[])dataStore;
            }
        }

    }
}
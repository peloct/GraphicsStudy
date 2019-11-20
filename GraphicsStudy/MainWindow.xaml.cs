using System;
using System.Windows;
using System.Windows.Controls;
using GraphicsStudy.UnitStudies;
using OpenTK;

namespace GraphicsStudy
{
    public abstract class IUnitStudy
    {
        protected GLControl glControl;

        public void SetGLControl(GLControl glControl)
        {
            this.glControl = glControl;
        }

        public virtual void InitComponent(Grid userControlRoot)
        {
            userControlRoot.Width = 0f;
        }

        public abstract void OnInit(object data);
        public abstract void OnLoad(object data);
        public abstract void OnResize(object data);
        public abstract void OnPaint(object data);
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private class NullUnitStudy : IUnitStudy
        {
            public override void InitComponent(Grid userControlRoot)
            {
                userControlRoot.Width = 0;
            }

            public override void OnInit(object data) { }

            public override void OnLoad(object data) { }

            public override void OnPaint(object data) { }

            public override void OnResize(object data) { }
        }

        private IUnitStudy unitStudy = new NullUnitStudy();

        public MainWindow()
        {
            unitStudy = new SampleStudy();
            InitializeComponent();
            unitStudy.InitComponent(userControlRoot);
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            unitStudy.SetGLControl(glControl);
            unitStudy.OnInit(null);
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            unitStudy.OnLoad(null);
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            unitStudy.OnPaint(null);
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            unitStudy.OnResize(null);
        }
    }
}

using System;
using System.Windows;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsStudy
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            Debug.WriteLine("WindowsFormsHost_Initialized");
            glControl.MakeCurrent();
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("GLControl_Load");
            GL.ClearColor(new Color4(0.631f, 0.6f, 0.227f, 1f));
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            glControl.SwapBuffers();
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format("GLControl_Resize : width = {0} height = {1}", glControl.Width, glControl.Height));
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }
    }
}

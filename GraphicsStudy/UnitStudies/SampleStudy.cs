using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Windows.Controls;

namespace GraphicsStudy.UnitStudies
{
    class SampleStudy : IUnitStudy
    {
        public override void InitComponent(Grid userControlRoot)
        {
            userControlRoot.Children.Add(new UserControls.SampleControl());
        }

        public override void OnInit(object data)
        {
            glControl = Utils.FindName<GLControl>("glControl");
            glControl.MakeCurrent();
        }

        public override void OnLoad(object data)
        {
            GL.ClearColor(new Color4(0.631f, 0.6f, 0.227f, 1f));
        }

        public override void OnPaint(object data)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            glControl.SwapBuffers();
        }

        public override void OnResize(object data)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }
    }
}

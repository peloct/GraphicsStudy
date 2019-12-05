using System;
using System.Windows.Controls;
using GraphicsStudy.Rendering;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsStudy.UI
{
    /// <summary>
    /// Scene.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Scene : UserControl
    {
        private Shader shader;
        private FbxSDK.Mesh mesh;

        private int vertexArray;

        int vertexBuffer;
        int indexBuffer;

        public Scene()
        {
            InitializeComponent();
        }

        public void ShowMesh(FbxSDK.Node node, FbxSDK.Mesh mesh)
        {
            this.mesh = mesh;
            glControl.MakeCurrent();
            vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length, mesh.vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length, mesh.indices, BufferUsageHint.StaticDraw);
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            shader = Shader.Create(
                @"C:\Users\peloc\Desktop\GraphicsStudy\GraphicsStudy\Resources\Shaders\TransformPos.vert",
                @"C:\Users\peloc\Desktop\GraphicsStudy\GraphicsStudy\Resources\Shaders\SimpleColor.frag");
            GL.ClearColor(new Color4(0.631f, 0.6f, 0.227f, 1f));
            GL.CreateProgram();
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            glControl.SwapBuffers();
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }
    }
}

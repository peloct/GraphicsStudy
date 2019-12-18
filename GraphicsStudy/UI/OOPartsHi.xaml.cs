using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using GraphicsStudy.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsStudy.UI
{
    /// <summary>
    /// Scene.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OOPartsHi : UserControl
    {
        private struct VertexData
        {
            public float x;
            public float y;
            public float z;
            public int boneIndex1;
            public int boneIndex2;
            public float boneWeight1;
            public float boneWeight2;
            public void SetCoordinate(Vector3 vec)
            {
                x = vec.X;
                y = vec.Y;
                z = vec.Z;
            }

            public static int GetCoordSize() { return sizeof(float) * 3; }
            public static int GetBoneIndexSize() { return sizeof(int) * 2; }
            public static int GetBoneWeightSize() { return sizeof(float) * 2; }
            public static int GetBoneIndexPos() { return GetCoordSize(); }
            public static int GetBoneWeightPos() { return GetCoordSize() + GetBoneIndexSize(); }
            public static int GetSize() { return GetCoordSize() + GetBoneIndexSize() + GetBoneWeightSize(); }
        }

        Vector3 camPos = new Vector3(0, 0, 5);

        private int vertexArray;
        private int polygonCount;

        private Shader shader;

        private Matrix4 projection;
        private Matrix4 view;

        private Matrix4[] bones = new Matrix4[2];
        private float secondBoneZRot = 0;
        private float secondBoneXRot = 0;

        public OOPartsHi()
        {
            InitializeComponent();
        }

        private void CreateTestMesh()
        {
            int layerCount = 50;
            int circleVertexCount = 10;

            Vector3[] layerShape = new Vector3[circleVertexCount];
            float angleStep = (float)(2 * Math.PI / circleVertexCount);

            for (int i = 0; i < circleVertexCount; ++i)
            {
                float s = (float)Math.Sin(i * angleStep);
                float c = (float)Math.Cos(i * angleStep);
                layerShape[i] = new Vector3(0, s, -c);
            }

            int layerShapeLength = layerShape.Length;
            int vertexCount = layerCount * layerShapeLength;

            Vector3[] controlPoints = new Vector3[vertexCount];

            int width = layerCount - 1;
            float meshWidth = 10;

            for (int i = 0; i < layerCount; ++i)
            {
                float half = width * 0.5f;
                Vector3 offset = new Vector3(meshWidth * (i - half) / width, 0, 0);

                for (int j = 0; j < layerShapeLength; ++j)
                    controlPoints[layerShapeLength * i + j] = offset + layerShape[j];
            }

            int[] indices = new int[width * layerShapeLength * 2 * 3];
            int nextIndex = 0;

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < layerShapeLength; ++j)
                {
                    int lowerLeft = layerShapeLength * i + j;
                    int lowerRight = layerShapeLength * (i + 1) + j;
                    int upperLeft = layerShapeLength * i + (j + 1) % layerShapeLength;
                    int upperRight = layerShapeLength * (i + 1) + (j + 1) % layerShapeLength;

                    indices[nextIndex++] = upperRight;
                    indices[nextIndex++] = lowerLeft;
                    indices[nextIndex++] = lowerRight;

                    indices[nextIndex++] = lowerLeft;
                    indices[nextIndex++] = upperRight;
                    indices[nextIndex++] = upperLeft;
                }
            }

            VertexData[] vertices = new VertexData[vertexCount];

            for (int i = 0; i < vertexCount; ++i)
            {
                int layerIndex = i / layerShapeLength;
                float x = layerIndex / (float)width;

                float weight = 3 * (x - 0.5f) + 0.5f;
                if (weight < 0)
                    weight = 0;
                if (weight > 1)
                    weight = 1;

                weight = 1 - weight;

                vertices[i].SetCoordinate(controlPoints[i]);
                vertices[i].boneIndex1 = 0;
                vertices[i].boneIndex2 = 1;
                vertices[i].boneWeight1 = weight;
                vertices[i].boneWeight2 = 1 - weight;
            }

            glControl.MakeCurrent();

            vertexArray = GL.GenVertexArray();
            polygonCount = indices.Length / 3;

            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexData.GetSize(), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexData.GetSize(), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribIPointer(1, 2, VertexAttribIntegerType.Int, VertexData.GetSize(), new IntPtr(VertexData.GetBoneIndexPos()));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, VertexData.GetSize(), VertexData.GetBoneWeightPos());
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }

        private void UpdateProjectionMatrix()
        {
            float aspect = glControl.Width / (float)glControl.Height;
            projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, aspect, 0.1f, 1000);
        }

        private void UpdateViewMatrix()
        {
            view = Matrix4.LookAt(camPos, Vector3.Zero, new Vector3(0, 1, 0));
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            CompositionTarget.Rendering += InvalidateGLControl;
            glControl.KeyPress += OnKeyPress;

            glControl.MakeCurrent();
            shader = Shader.Create(
                @"C:\Users\peloc\Desktop\GraphicsStudy\GraphicsStudy\Resources\Shaders\TransformPos.vert",
                @"C:\Users\peloc\Desktop\GraphicsStudy\GraphicsStudy\Resources\Shaders\SimpleColor.frag");

            GL.ClearColor(new Color4(0f, 0f, 0f, 1f));
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);

            bones[0] = bones[1] = Matrix4.Identity;
            secondBoneZRot = 0;

            CreateTestMesh();

            UpdateProjectionMatrix();
            UpdateViewMatrix();
        }

        private void InvalidateGLControl(object sender, EventArgs e)
        {
            glControl.Invalidate();
        }

        private void OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            const float speed = 0.01f;

            Vector3 frontDir = -camPos / camPos.Length;
            Vector3 right = Vector3.Cross(frontDir, new Vector3(0, 1, 0));
            right /= right.Length;

            if (e.KeyChar == 'w')
                camPos += frontDir * speed;
            else if (e.KeyChar == 's')
                camPos += frontDir * -speed;
            else if (e.KeyChar == 'q')
                camPos.Y += speed;
            else if (e.KeyChar == 'e')
                camPos.Y -= speed;
            else if (e.KeyChar == 'a')
                camPos += right * -speed;
            else if (e.KeyChar == 'd')
                camPos += right * speed;
            else if (e.KeyChar == 'o')
                secondBoneZRot += (float)(Math.PI / 90);
            else if (e.KeyChar == 'p')
                secondBoneZRot -= (float)(Math.PI / 90);
            else if (e.KeyChar == 'k')
                secondBoneXRot += (float)(Math.PI / 90);
            else if (e.KeyChar == 'l')
                secondBoneXRot -= (float)(Math.PI / 90);

            DebugUtil.WriteLine(camPos);
            UpdateViewMatrix();
            bones[1] = Matrix4.CreateRotationX(secondBoneXRot) * Matrix4.CreateRotationZ(secondBoneZRot);
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();

            GL.BindVertexArray(vertexArray);

            Matrix4 mvp = view * projection;

            //shader.SetMatrix4("bone1", ref bones[0]);
            //shader.SetMatrix4("bone2", ref bones[1]);
            shader.SetMatrix4Array("bones", bones);

            shader.SetColor("color", new Color4(1f, 1f, 1f, 1f));
            shader.SetMatrix4("mvp", ref mvp);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.DrawElements(BeginMode.Triangles, polygonCount * 3, DrawElementsType.UnsignedInt, 0);
          
            mvp.M43 -= 0.001f;
            shader.SetColor("color", new Color4(1f, 0f, 0f, 1f));
            shader.SetMatrix4("mvp", ref mvp);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawElements(BeginMode.Triangles, polygonCount * 3, DrawElementsType.UnsignedInt, 0);

            glControl.SwapBuffers();
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            UpdateProjectionMatrix();
        }
    }
}

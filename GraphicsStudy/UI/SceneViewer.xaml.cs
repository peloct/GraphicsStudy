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
    public partial class SceneViewer : UserControl
    {
        private class Node
        {
            public string name;
            public Matrix4 toParent;
            public Matrix4 localToWorld;
            public Node[] children;
        }

        private class MeshInfo
        {
            public Node node;
            public int vertexArray;
            public int polygonCount;
        }

        private Node rootNode;
        private List<MeshInfo> meshInfoList = new List<MeshInfo>();

        private FbxSDK.Scene scene;
        private Shader shader;

        Vector3 camPos = new Vector3(0, 0, 5);

        private Matrix4 projection;
        private Matrix4 view;

        private float globalScale = 1;
        private Matrix4 globalTrans = Matrix4.Identity;

        public SceneViewer()
        {
            InitializeComponent();
            globalScaleSlider.Value = 1f;
        }

        public void ShowScene(FbxSDK.Scene scene)
        {
            this.scene = scene;
            glControl.MakeCurrent();
            Matrix4 identity = Matrix4.Identity;
            rootNode = ConstructNodeTree(scene.GetRootNode(), ref identity);
        }

        private Node ConstructNodeTree(FbxSDK.Node node, ref Matrix4 parentMatrix)
        {
            Node ret = new Node();
            ret.name = node.GetName();
            ret.toParent = GetTransform(node);
            ret.localToWorld = ret.toParent * parentMatrix;

            var attribute = node.GetAttribute();

            if (attribute != null && attribute.GetAttributeType() == FbxSDK.NodeAttributeType.Mesh)
                LoadMesh(ret, node);

            int childCount = node.GetChildCount();
            ret.children = new Node[childCount];
            for (int i = 0; i < childCount; ++i)
                ret.children[i] = ConstructNodeTree(node.GetChild(i), ref ret.localToWorld);

            return ret;
        }

        private Node SearchNode(Node node, ref string name)
        {
            if (node.name == name)
                return node;

            for (int i = 0; i < node.children.Length; ++i)
            {
                Node result = SearchNode(node.children[i], ref name);
                if (result != null)
                    return result;
            }

            return null;
        }

        private void LoadMesh(Node node, FbxSDK.Node fbxNode)
        {
            var mesh = (FbxSDK.Mesh)fbxNode.GetAttribute();
            int polygonCount = mesh.GetPolygonCount();

            float[] vertexDatas = new float[polygonCount * 3 * 3];
            int size = 0;

            for (int i = 0; i < polygonCount; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    var coordinate = mesh.GetCoordinate(i, j);

                    vertexDatas[size++] = (float)coordinate.x;
                    vertexDatas[size++] = (float)coordinate.y;
                    vertexDatas[size++] = (float)coordinate.z;
                }
            }

            int deformerCount = mesh.GetSkinDeformerCount();
            for (int i = 0; i < deformerCount; ++i)
            {
                int clusterCount = mesh.GetClusterCount(i);
                for (int j = 0; j < clusterCount; ++j)
                {
                    var cluster = mesh.GetCluster(i, j);
                    var link = cluster.GetLink();
                    string linkName = link.GetName();

                    Node linkNode = SearchNode(rootNode, ref linkName);

                    if (linkNode == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Cannot find bone " + linkName);
                        continue;
                    }

                    int controlPointCount = cluster.GetControlIndicesCount();
                    for (int k = 0; k < controlPointCount; ++k)
                    {
                        int controlPointIndex = cluster.GetControlPointIndex(k);
                        double weight = cluster.GetControlPointWeight(k);
                    }
                }
            }

            int newVertexArray = GL.GenVertexArray();

            meshInfoList.Add(new MeshInfo()
            {
                node = node,
                vertexArray = newVertexArray,
                polygonCount = polygonCount
            });

            GL.BindVertexArray(newVertexArray);

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, size * sizeof(float), vertexDatas, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }

        private Matrix4 GetTransform(FbxSDK.Node node)
        {
            Matrix4 transform = Matrix4.CreateScale(Convert(node.GetScaling()));
            FbxSDK.Vector3 rotation = node.GetRotation();

            switch (node.GetRotationOrder())
            {
                case FbxSDK.RotationOrder.EulerXYZ:
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    break;
                case FbxSDK.RotationOrder.EulerXZY:
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    break;
                case FbxSDK.RotationOrder.EulerYXZ:
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    break;
                case FbxSDK.RotationOrder.EulerYZX:
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    break;
                case FbxSDK.RotationOrder.EulerZXY:
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    break;
                case FbxSDK.RotationOrder.EulerZYX:
                    transform *= Matrix4.CreateRotationZ((float)rotation.z);
                    transform *= Matrix4.CreateRotationY((float)rotation.y);
                    transform *= Matrix4.CreateRotationX((float)rotation.x);
                    break;
            }

            transform *= Matrix4.CreateTranslation(Convert(node.GetTranslation()));
            return transform;
        }

        private Vector3 Convert(FbxSDK.Vector3 vec)
        {
            return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
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

            //LoadTriangle();

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

            System.Diagnostics.Debug.WriteLine(camPos);
            UpdateViewMatrix();
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            for (int i = 0; i < meshInfoList.Count; ++i)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                Matrix4 mvp = meshInfoList[i].node.localToWorld * globalTrans * view * projection;
                shader.SetMatrix4("mvp", ref mvp);
                shader.SetColor("color", new Color4(1f, 1f, 1f, 1f));
                GL.BindVertexArray(meshInfoList[i].vertexArray);
                GL.DrawArrays(PrimitiveType.Triangles, 0, meshInfoList[i].polygonCount * 3);

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                mvp.M43 -= 0.0001f;
                shader.SetMatrix4("mvp", ref mvp);
                shader.SetColor("color", new Color4(1f, 0f, 0f, 1f));
                GL.DrawArrays(PrimitiveType.Triangles, 0, meshInfoList[i].polygonCount * 3);
            }

            glControl.SwapBuffers();
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            UpdateProjectionMatrix();
        }

        private void globalScaleSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            globalScale = (float)e.NewValue;
            globalTrans = Matrix4.CreateScale(globalScale);
        }
    }
}

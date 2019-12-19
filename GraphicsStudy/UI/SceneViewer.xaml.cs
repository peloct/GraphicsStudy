using System;
using System.Reflection;
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
        private struct VertexData
        {
            public float x;
            public float y;
            public float z;

            public byte boneIndex1;
            public byte boneIndex2;
            public byte boneIndex3;
            public byte boneIndex4;

            public byte boneIndex5;
            public byte boneIndex6;
            public byte boneIndex7;
            public byte boneIndex8;

            public byte boneIndex9;
            public byte boneIndex10;
            public byte boneIndex11;
            public byte boneIndex12;

            public byte boneIndex13;
            public byte boneIndex14;
            public byte boneIndex15;
            public byte boneIndex16;

            public float boneWeight1;
            public float boneWeight2;
            public float boneWeight3;
            public float boneWeight4;

            public float boneWeight5;
            public float boneWeight6;
            public float boneWeight7;
            public float boneWeight8;

            public float boneWeight9;
            public float boneWeight10;
            public float boneWeight11;
            public float boneWeight12;

            public float boneWeight13;
            public float boneWeight14;
            public float boneWeight15;
            public float boneWeight16;

            private static FieldInfo[] indexFieldInfos = null;
            private static FieldInfo[] weightFieldInfos = null;

            public static void SetBoneInfo(ref VertexData target, int fieldIndex, byte boneIndex, float boneWeight)
            {
                Type type = typeof(VertexData);

                if (indexFieldInfos == null)
                {
                    indexFieldInfos = new FieldInfo[16];
                    weightFieldInfos = new FieldInfo[16];

                    for (int i = 0; i < 16; ++i)
                    {
                        indexFieldInfos[i] = type.GetField(string.Format("boneIndex{0}", i + 1));
                        weightFieldInfos[i] = type.GetField(string.Format("boneWeight{0}", i + 1));
                    }
                }

                object boxed = (object)target; // hello gc! :)
                indexFieldInfos[fieldIndex].SetValue(boxed, boneIndex);
                weightFieldInfos[fieldIndex].SetValue(boxed, boneWeight);
                target = (VertexData)boxed;
            }

            public static int GetBoneIndexPos(int index)
            {
                return sizeof(float) * 3 + index * sizeof(byte) * 4;
            }

            public static int GetBoneWeightPos(int index)
            {
                return sizeof(float) * 3 + sizeof(byte) * 16 + index * sizeof(float) * 4;
            }

            public static int GetSize()
            {
                return sizeof(float) * 3 + sizeof(byte) * 16 + sizeof(float) * 16;
            }
        }

        private const int MaxBoneCount = 50;

        private class BoneInfo
        {
            public int boneIndex;
            public FbxSDK.Cluster cluster;
            public MeshInfo ownerMeshInfo;
        }

        private class MeshInfo
        {
            public FbxSDK.Mesh mesh;
            public FbxSDK.Node node;

            public int vertexArray;

            public int polygonCount;
            public int controlPointsCount;
            public int[] nextSkinningIndex;

            public Matrix4 geometryToWorld;
            public Matrix4[] boneTrasforms;
        }

        private List<BoneInfo> boneInfoList = new List<BoneInfo>();
        private List<MeshInfo> meshInfoList = new List<MeshInfo>();
        private List<FbxSDK.AnimationStack> animationList = new List<FbxSDK.AnimationStack>(); 
        private List<FbxSDK.Pose> poseList = new List<FbxSDK.Pose>();

        private FbxSDK.Scene scene;
        private Shader shader;

        Vector3 camPos = new Vector3(0, 0, 5);

        private Matrix4 projection;
        private Matrix4 view;

        private float globalScale = 1;
        private Matrix4 globalTrans = Matrix4.Identity;

        private FbxSDK.Time start;
        private FbxSDK.Time stop;
        private FbxSDK.Time curTime;
        private FbxSDK.Time oneFrame;

        private FbxSDK.Pose curPose;

        private NodeHierarchy nodeHierarchy;

        public SceneViewer()
        {
            InitializeComponent();
            globalScaleSlider.Value = 1f;
        }

        public void ShowScene(NodeHierarchy nodeHierarchy, FbxSDK.Scene scene)
        {
            this.scene = scene;
            this.nodeHierarchy = nodeHierarchy;

            int animationCount = scene.GetAnimStackCount();
            for (int i = 0; i < animationCount; ++i)
            {
                var animStack = scene.GetAnimStack(i);
                animationList.Add(animStack);
            }

            oneFrame = new FbxSDK.Time(0, 0, 0, 1, scene.GetGlobalTimeMode());
            start = stop = curTime = FbxSDK.Time.Infinite();

            if (animationList.Count > 0)
            {
                var timeSpan = scene.GetAnimTimelineTimeSpan(animationList[0]);
                start = timeSpan.start;
                stop = timeSpan.stop;
                curTime = start;
                scene.SetCurrentAnimStack(animationList[0]);

                for (int i = 0; i < animationList.Count; ++i)
                {
                    int animationID = i;
                    Button animationButton = new Button();
                    animationButton.Content = animationList[i].GetName();
                    animationButton.Click += (a, b) =>
                    {
                        var span = scene.GetAnimTimelineTimeSpan(animationList[animationID]);
                        scene.SetCurrentAnimStack(animationList[animationID]);
                        start = span.start;
                        stop = span.stop;
                        curTime = start;
                    };
                    panel.Children.Add(animationButton);
                }
            }

            int poseCount = scene.GetPoseCount();
            for (int i = 0; i < poseCount; ++i)
                poseList.Add(scene.GetPose(0));

            if (poseList.Count > 0)
            {
                Button deselectPoseButton = new Button();
                deselectPoseButton.Content = "Deselect Pose";
                deselectPoseButton.Click += (a, b) =>
                {
                    curPose = null;
                };
                panel.Children.Add(deselectPoseButton);

                for (int i = 0; i < poseCount; ++i)
                {
                    int poseID = i;
                    Button poseButton = new Button();
                    poseButton.Content = "Select Pose " + i;
                    poseButton.Click += (a, b) =>
                    {
                        curPose = poseList[poseID];
                    };
                    panel.Children.Add(poseButton);
                }
            }
        }

        private void UpdateMeshInfos()
        {
            Matrix4 identity = Matrix4.Identity;
            UpdateMeshInfoRecursively(scene.GetRootNode(), ref identity);
        }

        private void UpdateMeshInfoRecursively(FbxSDK.Node node, ref Matrix4 parentMatrix)
        {
            int childCount = node.GetChildCount();

            Matrix4 globalTransform = GetGlobalTransform(node, parentMatrix);

            FbxSDK.Mesh mesh = node.GetAttribute() as FbxSDK.Mesh;
            if (mesh != null)
            {
                Matrix4 meshTransform = GetGeometry(node) * globalTransform;

                MeshInfo meshInfo = FindMeshInfo(mesh);
                if (meshInfo == null)
                    meshInfo = LoadMesh(mesh);

                meshInfo.geometryToWorld = meshTransform;
            }

            for (int i = 0; i < childCount; ++i)
                UpdateMeshInfoRecursively(node.GetChild(i), ref globalTransform);
        }

        private MeshInfo FindMeshInfo(FbxSDK.Mesh mesh)
        {
            return meshInfoList.Find(v => v.mesh == mesh);
        }

        private MeshInfo LoadMesh(FbxSDK.Mesh mesh)
        {
            MeshInfo meshInfo = new MeshInfo();
            meshInfoList.Add(meshInfo);
            meshInfo.mesh = mesh;
            meshInfo.node = mesh.GetNode();
            int polygonCount = mesh.GetPolygonCount();
            int controlPointsCount = mesh.GetControlPointsCount();
            meshInfo.polygonCount = polygonCount;
            meshInfo.controlPointsCount = controlPointsCount;
            meshInfo.nextSkinningIndex = new int[controlPointsCount];
            meshInfo.boneTrasforms = new Matrix4[MaxBoneCount];

            int vertexArray = GL.GenVertexArray();
            meshInfo.vertexArray = vertexArray;

            GL.BindVertexArray(vertexArray);

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            int elementBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);

            VertexData[] vertexBufferData = new VertexData[controlPointsCount];
            int[] vertexBufferBoneCounter = new int[controlPointsCount];

            for (int i = 0; i < controlPointsCount; ++i)
            {
                Vector3 coordinate = Converter.Convert(mesh.GetControlPoint(i));

                vertexBufferData[i] = new VertexData()
                {
                    x = coordinate.X,
                    y = coordinate.Y,
                    z = coordinate.Z
                };
            }

            int skinDeformerCount = mesh.GetSkinDeformerCount();
            byte boneIndex = 0;

            for (int i = 0; i < skinDeformerCount; ++i)
            {
                FbxSDK.Skin skin = mesh.GetSkinDeformer(i);

                var skinningType = skin.GetSkinningType();

                if (skinningType == FbxSDK.SkinningType.Linear || skinningType == FbxSDK.SkinningType.Rigid)
                {
                    int clusterCount = skin.GetClusterCount();

                    for (int j = 0; j < clusterCount; ++j)
                    {
                        FbxSDK.Cluster cluster = skin.GetCluster(j);
                        FbxSDK.Node link = cluster.GetLink();

                        if (link == null)
                            continue;

                        if (cluster.HasAssociateModel())
                            DebugUtil.WriteLine("No support for associate model.");

                        int indexCount = cluster.GetControlIndicesCount();
                        bool isValidBone = false;

                        for (int k = 0; k < indexCount; ++k)
                        {
                            int controlPointIndex = cluster.GetControlPointIndex(k);
                            float controlPointWeight = (float)cluster.GetControlPointWeight(k);

                            if (vertexBufferBoneCounter[controlPointIndex] == 16)
                                continue;

                            isValidBone = true;
                            int fieldIndex = vertexBufferBoneCounter[controlPointIndex];
                            VertexData.SetBoneInfo(ref vertexBufferData[controlPointIndex], fieldIndex, boneIndex, controlPointWeight);
                            ++vertexBufferBoneCounter[controlPointIndex];
                        }

                        if (isValidBone)
                        {
                            boneInfoList.Add(new BoneInfo()
                            {
                                cluster = cluster,
                                boneIndex = boneIndex,
                                ownerMeshInfo = meshInfo
                            });

                            ++boneIndex;
                        }
                    }
                }
                else
                {
                    DebugUtil.WriteLine("No support for dual quaternion and blend."); // TODO : Dual Quaternion rigging
                }
            }

            int[] indices = new int[polygonCount * 3];

            for (int i = 0; i < polygonCount; ++i)
            {
                indices[3 * i] = mesh.GetControlPointIndex(i, 0);
                indices[3 * i + 1] = mesh.GetControlPointIndex(i, 1);
                indices[3 * i + 2] = mesh.GetControlPointIndex(i, 2);
            }

            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferData.Length * VertexData.GetSize(), vertexBufferData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexData.GetSize(), 0);
            GL.EnableVertexAttribArray(0);

            for (int i = 0; i < 4; ++i)
            {
                int attribIndexIndex = i + 1;
                GL.VertexAttribIPointer(attribIndexIndex, 4, VertexAttribIntegerType.UnsignedByte, VertexData.GetSize(), new IntPtr(VertexData.GetBoneIndexPos(i)));
                GL.EnableVertexAttribArray(attribIndexIndex);
            }

            for (int i = 0; i < 4; ++i)
            {
                int attribIndexIndex = i + 5;
                GL.VertexAttribPointer(attribIndexIndex, 4, VertexAttribPointerType.Float, false, VertexData.GetSize(), VertexData.GetBoneWeightPos(i));
                GL.EnableVertexAttribArray(attribIndexIndex);
            }

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);

            return meshInfo;
        }

        private void UpdateBoneInfos()
        {
            for (int i = 0; i < boneInfoList.Count; ++i)
            {
                var cluster = boneInfoList[i].cluster;
                var node = boneInfoList[i].ownerMeshInfo.node;
                var link = cluster.GetLink();

                var initialGeometryToWorld = Converter.Convert(cluster.GetTransformMatrix());
                var initialWorldToLink = Converter.Convert(cluster.GetTransformLinkMatrix()).Inverted();
                var curWorldToGeometry = boneInfoList[i].ownerMeshInfo.geometryToWorld.Inverted();
                var curLinkToWorld = GetGlobalTransform(link);

                Matrix4 initialGeometryToCurGeometry = initialGeometryToWorld * initialWorldToLink * curLinkToWorld * curWorldToGeometry;

                boneInfoList[i].ownerMeshInfo.boneTrasforms[boneInfoList[i].boneIndex] = initialGeometryToCurGeometry;
            }
        }

        private Matrix4 GetGlobalTransform(FbxSDK.Node node, Matrix4? parentGlobalTransform = null)
        {
            Matrix4 globalTransform = Matrix4.Identity;
            bool isGlobalTransformReady = false;

            if (curPose != null)
            {
                int nodeIndex = curPose.Find(node);
                if (nodeIndex >= 0)
                {
                    Matrix4 poseMat = Converter.Convert(curPose.GetMatrix(nodeIndex));

                    if (curPose.IsBindPose() || !curPose.IsLocalMatrix(nodeIndex))
                    {
                        globalTransform = poseMat;
                    }
                    else
                    {
                        if (parentGlobalTransform.HasValue)
                        {
                            globalTransform = poseMat * parentGlobalTransform.Value;
                        }
                        else
                        {
                            if (node.GetParent() != null)
                                globalTransform = poseMat * GetGlobalTransform(node.GetParent());
                        }
                    }

                    isGlobalTransformReady = true;
                }
            }

            if (isGlobalTransformReady == false)
                globalTransform = Converter.Convert(node.EvaluateGlobalTransform(curTime));

            return globalTransform;
        }

        private Matrix4 GetGeometry(FbxSDK.Node node)
        {
            return Converter.Convert(node.GetGeometryOffset());
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
                @"C:\Users\peloc\Desktop\GraphicsStudy\GraphicsStudy\Resources\Shaders\SkinningTransform.vert",
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

            if (e.KeyChar == 'i')
                camPos += frontDir * speed;
            else if (e.KeyChar == 'k')
                camPos += frontDir * -speed;
            else if (e.KeyChar == 'o')
                camPos.Y += speed;
            else if (e.KeyChar == 'u')
                camPos.Y -= speed;
            else if (e.KeyChar == 'j')
                camPos += right * -speed;
            else if (e.KeyChar == 'l')
                camPos += right * speed;

            DebugUtil.WriteLine(camPos);
            UpdateViewMatrix();
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (start != FbxSDK.Time.Infinite())
            {
                curTime += oneFrame;
                if (curTime > stop)
                    curTime = start;
            }

            glControl.MakeCurrent();

            if (scene != null)
            {
                var selectedNode = nodeHierarchy.CurSelected;

                if (selectedNode != null)
                {
                    // Action to do when some node has been selected.
                }

                UpdateMeshInfos();
                UpdateBoneInfos();
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            for (int i = 0; i < meshInfoList.Count; ++i)
            {
                GL.BindVertexArray(meshInfoList[i].vertexArray);

                Matrix4 mvp = meshInfoList[i].geometryToWorld * globalTrans * view * projection;

                shader.SetMatrix4Array("skinningTransforms", meshInfoList[i].boneTrasforms);

                shader.SetMatrix4("mvp", ref mvp);
                shader.SetColor("color", new Color4(1f, 1f, 1f, 1f));

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.DrawElements(BeginMode.Triangles, meshInfoList[i].polygonCount * 3, DrawElementsType.UnsignedInt, 0);

                mvp.M43 -= 0.001f;
                shader.SetMatrix4("mvp", ref mvp);
                shader.SetColor("color", new Color4(1f, 0f, 0f, 1f));
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.DrawElements(BeginMode.Triangles, meshInfoList[i].polygonCount * 3, DrawElementsType.UnsignedInt, 0);
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

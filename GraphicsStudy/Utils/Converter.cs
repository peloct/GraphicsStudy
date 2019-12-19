using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsStudy
{
    public static class Converter
    {
        public static Vector3 Convert(FbxSDK.Vector3 vec)
        {
            return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
        }

        public static Matrix4 Convert(FbxSDK.Matrix matrix) // TODO : Add RotationOrder field in FbxSDK.Matrix
        {
            Matrix4 transform = Matrix4.CreateScale(Convert(matrix.scaling));
            FbxSDK.Vector3 rotation = matrix.rotation;
            double deg2PI = Math.PI / 180;

            switch (matrix.rotationOrder)
            {
                case FbxSDK.RotationOrder.EulerXYZ:
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    break;
                case FbxSDK.RotationOrder.EulerXZY:
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    break;
                case FbxSDK.RotationOrder.EulerYXZ:
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    break;
                case FbxSDK.RotationOrder.EulerYZX:
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    break;
                case FbxSDK.RotationOrder.EulerZXY:
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    break;
                case FbxSDK.RotationOrder.EulerZYX:
                    transform *= Matrix4.CreateRotationZ((float)(rotation.z * deg2PI));
                    transform *= Matrix4.CreateRotationY((float)(rotation.y * deg2PI));
                    transform *= Matrix4.CreateRotationX((float)(rotation.x * deg2PI));
                    break;
            }

            transform *= Matrix4.CreateTranslation(Convert(matrix.translation));
            return transform;
        }
    }
}

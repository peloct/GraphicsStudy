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

        public static Matrix4 Convert(FbxSDK.Matrix matrix, FbxSDK.RotationOrder rotationOrder)
        {
            Matrix4 transform = Matrix4.CreateScale(Convert(matrix.scaling));
            FbxSDK.Vector3 rotation = matrix.rotation;

            switch (rotationOrder)
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

            transform *= Matrix4.CreateTranslation(Convert(matrix.translation));
            return transform;
        }
    }
}

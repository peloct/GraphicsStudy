using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsStudy.Rendering
{
    class Shader
    {
        private int program = 0;

        public static Shader Create(string vertexShaderPath, string fragmentShaderPath)
        {
            if (!File.Exists(vertexShaderPath) || !File.Exists(fragmentShaderPath))
                return null;

            try
            {
                string vertexShaderText = null;
                string fragmentShaderText = null;

                using (FileStream fs = File.OpenRead(vertexShaderPath))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        vertexShaderText = sr.ReadToEnd();
                    }
                }

                using (FileStream fs = File.OpenRead(fragmentShaderPath))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        fragmentShaderText = sr.ReadToEnd();
                    }
                }

                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexShaderText);
                GL.CompileShader(vertexShader);
                DebugUtil.WriteLine(GL.GetShaderInfoLog(vertexShader));

                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentShaderText);
                GL.CompileShader(fragmentShader);
                DebugUtil.WriteLine(GL.GetShaderInfoLog(fragmentShader));

                int program = GL.CreateProgram();
                GL.AttachShader(program, vertexShader);
                GL.AttachShader(program, fragmentShader);
                GL.LinkProgram(program);
                DebugUtil.WriteLine(GL.GetProgramInfoLog(program));

                GL.DetachShader(program, vertexShader);
                GL.DetachShader(program, fragmentShader);
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);

                Shader shader = new Shader();
                shader.program = program;
                return shader;
            }
            catch(Exception e)
            {
                DebugUtil.WriteLine(e);
            }

            return null;
        }

        private Shader() { }

        public void Use()
        {
            GL.UseProgram(program);
        }

        public void SetColor(string propertyName, Color4 color)
        {
            int location = GL.GetUniformLocation(program, propertyName);
            GL.Uniform4(location, color);
        }

        public void SetMatrix4(string propertyName, ref Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(program, propertyName);
            GL.UniformMatrix4(location, false, ref matrix);
        }
    }
}

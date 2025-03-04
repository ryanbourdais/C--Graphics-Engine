using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLEngine;
using OpenGLEngine.Resources;

namespace OpenGLEngine.Sandboxes
{
    public class TriangleSandbox : GameWindow
    {
        public TriangleSandbox(int width, int height, string title) : base(
        GameWindowSettings.Default,
        new NativeWindowSettings
        {
            ClientSize = new Vector2i(width, height),
            Title = title,
            // This is needed to run on macOS
            Flags = ContextFlags.ForwardCompatible
        })
        {
        }

        // Fields for your Game class
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        protected Shader _shader = null!;
        private uint[] _indices = null!;
        private System.Diagnostics.Stopwatch _timer = new System.Diagnostics.Stopwatch();
        private Texture _texture = null!;


        protected override void OnLoad()
        {
            base.OnLoad();
            // Set clear color to dark blue
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            _shader = new Shader("Shaders/texture.vert", "Shaders/texture.frag");

            // Create a triangle
            float[] vertices =
            {
                //Position                  Texture coordinates
                0.0f,  0.5f, 0.0f,         0.5f, 1.0f,  // top center
                0.5f, -0.5f, 0.0f,         1.0f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,        0.0f, 0.0f,  // bottom left
            };

            _indices = new uint[] {
                0, 1, 2
            };

            _timer.Start();
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Position attribute (3 floats)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture coordinate attribute (2 floats)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Load the texture using your Texture class
            _texture = new Texture("Data/brick-texture.jpg");
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // Clear the color buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            
            // Bind the texture
            _texture.Use();
            _shader.SetInt("ourTexture", 0);
            
            double timeValue = _timer.Elapsed.TotalSeconds;
            float brightness = (float)Math.Sin(timeValue) * 0.25f + 0.65f;
            int animatedColorLocation = GL.GetUniformLocation(_shader.Handle, "animatedColor");
            GL.Uniform4(animatedColorLocation, brightness, brightness, brightness, 1.0f);

            int useAnimatedColorLoc = GL.GetUniformLocation(_shader.Handle, "useAnimatedColor");
            GL.Uniform1(useAnimatedColorLoc, 0);

            int timeLocation = GL.GetUniformLocation(_shader.Handle, "time");
            GL.Uniform1(timeLocation, (float)timeValue);
            
            Matrix4 transform = Matrix4.CreateRotationZ((float)timeValue); // Rotate around Z axis
            int transformLoc = GL.GetUniformLocation(_shader.Handle, "transform");
            GL.UniformMatrix4(transformLoc, false, ref transform);

            int useTransformLoc = GL.GetUniformLocation(_shader.Handle, "useTransform");
            GL.Uniform1(useTransformLoc, 0);

            Matrix4 scale = Matrix4.CreateScale(new Vector3((float)Math.Sin(timeValue) * 0.25f + 0.65f, (float)Math.Sin(timeValue) * 0.25f + 0.65f, (float)Math.Sin(timeValue) * 0.25f + 0.65f));
            int scaleLoc = GL.GetUniformLocation(_shader.Handle, "scale");
            GL.UniformMatrix4(scaleLoc, false, ref scale);

            int useScaleLoc = GL.GetUniformLocation(_shader.Handle, "useScale");
            GL.Uniform1(useScaleLoc, 0);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            // Swap buffers
            SwapBuffers();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            // Update viewport when window is resized
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            _shader.Dispose();
            base.OnUnload();
        }
    }
}
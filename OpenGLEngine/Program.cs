using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLEngine;

namespace OpenGLEngine
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title) : base(
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
        private Shader _shader = null!;


        protected override void OnLoad()
        {
            base.OnLoad();
            // Set clear color to dark blue
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            // Create a triangle
           float[] vertices = {
                0.0f,  0.5f, 0.0f,  // Top
                -0.5f, -0.5f, 0.0f,  // Bottom left
                 0.5f, -0.5f, 0.0f   // Bottom right
            };

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // Clear the color buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
            _shader.Dispose();
            base.OnUnload();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "OpenGLEngine"))
            {
                game.Run();
            }
        }
    }
}
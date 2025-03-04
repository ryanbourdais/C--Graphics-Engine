using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenGLEngine.Resources;
using OpenGLEngine.Structs;
using System.IO;

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
        private System.Diagnostics.Stopwatch _timer = new System.Diagnostics.Stopwatch();
        private Mesh _cube1 = null!;
        private Mesh _cube2 = null!;
        private Mesh _sphere = null!;
        private Shader _meshShader = null!;

        protected override void OnLoad()
        {
            base.OnLoad();
            // Set clear color to dark blue
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            _meshShader = new Shader("Shaders/mesh.vert", "Shaders/mesh.frag");
            _timer.Start();

             // Create meshes
            _cube1 = MeshFactory.CreateCube(1.0f, 1.0f, 1.0f);
            _cube2 = MeshFactory.CreateCube(0.5f, 0.5f, 0.5f);
            _sphere = MeshFactory.CreateSphere(0.7f, 32, 16);
            
            // Make sure the texture file exists
            string texturePath = "Data/brick-texture.jpg";
            if (!File.Exists(texturePath))
            {
                Console.WriteLine($"Warning: Texture file not found at {texturePath}");
                // You might want to use a fallback texture or handle this differently
            }
            
            // Apply textures using your TexturedMeshFactory
            _cube1 = TexturedMeshFactory.CreateTexturedMesh(_cube1, "Data/brick-texture.jpg");
            _cube2 = TexturedMeshFactory.CreateTexturedMesh(_cube2, "Data/brick-texture.jpg");
            _sphere = TexturedMeshFactory.CreateTexturedMesh(_sphere, "Data/brick-texture.jpg");

            // Set initial transformations
            _cube1.Transform.SetPosition(new Vector3(-1.5f, -1.0f, 0.0f));
            _cube2.Transform.SetPosition(new Vector3(1.5f, 0.0f, 0.0f));
            _sphere.Transform.SetPosition(new Vector3(0.0f, 1.5f, 0.0f));
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // Clear the color buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Calculate time-based values for animation
            float time = (float)_timer.Elapsed.TotalSeconds;

            // Update transformations
            _cube1.Transform.SetRotation(new Vector3(0, time * 50, 0));
            _cube2.Transform.SetRotation(new Vector3(time * 50, 0, 0));
            _sphere.Transform.SetRotation(new Vector3(0, 0, time * 50));

            // Move cube1 back and forth using a sine wave
            float x = MathF.Sin(time) * 1.5f; // Oscillate between -1.5 and 1.5
            _cube1.Transform.SetPosition(new Vector3(x, _cube1.Transform.Position.Y, _cube1.Transform.Position.Z));

            // Set up view and projection matrices
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -5.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / Size.Y,
                0.1f,
                100.0f);

            // Draw objects
            _meshShader.Use();
            _meshShader.SetMatrix4("view", view);
            _meshShader.SetMatrix4("projection", projection);
            
            _cube1.Draw(_meshShader);
            _cube2.Draw(_meshShader);
            _sphere.Draw(_meshShader);
            
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
            _cube1.Dispose();
            _cube2.Dispose();
            _sphere.Dispose();
            _meshShader.Dispose();
            base.OnUnload();
        }
    }
}
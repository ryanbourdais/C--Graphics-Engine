using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenGLEngine.Resources;
using OpenGLEngine.Structs;
using System.IO;
using OpenGLEngine.Shaders;

namespace OpenGLEngine.Sandboxes    
{
    public class SceneManagerSandbox : GameWindow
    {
        public SceneManagerSandbox(int width, int height, string title) : base(
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
        private Shader _meshShader = null!;
        private Scene _scene = new Scene();

        protected override void OnLoad()
        {
            base.OnLoad();
            // Set clear color to dark blue
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            _meshShader = new Shader("Shaders/mesh.vert", "Shaders/mesh.frag");
            _timer.Start();

            _scene = new Scene();
            _scene.ActiveShader = _meshShader;

            // Create game objects
            var cube1 = new GameObject("Cube1");
            cube1.Mesh = MeshFactory.CreateCube(1.0f, 1.0f, 1.0f);

            var cube2 = new GameObject("Cube2");
            cube2.Mesh = MeshFactory.CreateCube(0.5f, 0.5f, 0.5f);

            var sphere = new GameObject("Sphere");
            sphere.Mesh = MeshFactory.CreateSphere(0.7f, 32, 16);

            // Apply textures
            cube1.Mesh = TexturedMeshFactory.CreateTexturedMesh(cube1.Mesh, "Data/fabric-texture.jpg");
            cube2.Mesh = TexturedMeshFactory.CreateTexturedMesh(cube2.Mesh, "Data/ice-texture.jpg");
            sphere.Mesh = TexturedMeshFactory.CreateTexturedMesh(sphere.Mesh, "Data/rust-texture.jpg");

            // Set initial transformations
            cube1.Transform.SetPosition(new Vector3(-1.5f, -1.0f, 0.0f));
            cube2.Transform.SetPosition(new Vector3(1.5f, 0.0f, 0.0f));
            sphere.Transform.SetPosition(new Vector3(0.0f, 1.5f, 0.0f));

            // Add game objects to scene
            _scene.AddGameObject(cube1);
            _scene.AddGameObject(cube2);
            _scene.AddGameObject(sphere);
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // Clear the color buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Calculate time-based values for animation
            float time = (float)_timer.Elapsed.TotalSeconds;

            // Update transforms
            var cube1 = _scene.GetGameObjectByName("Cube1");
            var cube2 = _scene.GetGameObjectByName("Cube2");
            var sphere = _scene.GetGameObjectByName("Sphere");

            if (cube1 != null)
            {
                cube1.Transform.SetRotation(new Vector3(0, time * 50, 0));

                // Move cube1 back and forth using a sine wave
                float x = MathF.Sin(time) * 1.5f;
                cube1.Transform.SetPosition(new Vector3(x, cube1.Transform.Position.Y, cube1.Transform.Position.Z));
            }   
            if (cube2 != null)
            {
                cube2.Transform.SetRotation(new Vector3(time * 50, 0, 0));
            }
            if (sphere != null)
            {
                sphere.Transform.SetRotation(new Vector3(0, 0, time * 50));
            }

            // Setup view and projection matrices
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -5.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / Size.Y,
                0.1f,
                100.0f);

            // Set view and projection matrices in the shader
            if (_scene.ActiveShader != null)
            {
                _scene.ActiveShader.Use();
                _scene.ActiveShader.SetMatrix4("view", view);
                _scene.ActiveShader.SetMatrix4("projection", projection);
            }

            // Render the scene
            _scene.Render();

            SwapBuffers();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            // Update the scene
            _scene.Update((float)e.Time);
        }
        
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            // Update viewport when window is resized
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            _meshShader.Dispose();
            base.OnUnload();
        }
    }
}
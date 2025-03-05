using OpenGLEngine.Sandboxes;

namespace OpenGLEngine
{    
    class Program
    {
        static void Main(string[] args)
        {
            using (SceneManagerSandbox game = new SceneManagerSandbox(800, 600, "OpenGLEngine"))
            {
                game.Run();
            }
        }
    }
}
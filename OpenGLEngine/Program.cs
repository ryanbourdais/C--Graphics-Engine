using OpenGLEngine;

namespace OpenGLEngine
{    
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
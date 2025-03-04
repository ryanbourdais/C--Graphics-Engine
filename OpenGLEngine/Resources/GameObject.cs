using OpenGLEngine.Structs;

namespace OpenGLEngine.Resources
{
    public class GameObject
    {
        public Mesh Mesh { get; set; }
        public Transform Transform { get; set; }
        
        public void Draw(Shader shader)
        {
            Mesh.Draw(shader);
        }
    }
}

using OpenTK.Mathematics;
using OpenGLEngine.Structs;
using OpenGLEngine.Shaders;

namespace OpenGLEngine.Resources
{
    public class GameObject
    {
        public string Name { get; set; }
        public Mesh? Mesh { get; set; }
        public Transform Transform { get; set; }
        
        public GameObject(string name = "GameObject")
        {
            Name = name;
            Transform = new Transform();
            Transform.Owner = this;
        }
        public virtual void Render(Shader shader)
        {
            if (Mesh != null)
            {
                Mesh.Transform = Transform;
                Mesh.Draw(shader);
            }
        }

        public virtual void Update(float deltaTime)
        {
            // Empty - should be overriden in derived classes
            // deltaTime is the time since the last frame
        }

        public void AddChild(GameObject child)
        {
            if (child != null)
            {
                Transform.AddChild(child.Transform);
            }
        }

        public void RemoveChild(GameObject child)
        {
            if (child != null)
            {
                Transform.RemoveChild(child.Transform);
            }
        }
    }
}

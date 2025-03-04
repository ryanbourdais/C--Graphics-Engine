using OpenGLEngine.Resources;

namespace OpenGLEngine.Resources
{
    public class Scene
    {
        public List<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }
        
        
    }
}

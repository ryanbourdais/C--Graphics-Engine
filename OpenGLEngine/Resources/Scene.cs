using System.Collections.Generic;
using OpenGLEngine.Shaders;

namespace OpenGLEngine.Resources
{
    public class Scene
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private Dictionary<string, GameObject> gameObjectMap = new Dictionary<string, GameObject>();
        public Shader? ActiveShader { get; set; }
        public void AddGameObject(GameObject gameObject)
        {
            if(gameObject == null) return;
            gameObjects.Add(gameObject);

            if(!string.IsNullOrEmpty(gameObject.Name) && !gameObjectMap.ContainsKey(gameObject.Name))
            {
                gameObjectMap[gameObject.Name] = gameObject;
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            if(gameObject == null) return;

            gameObjects.Remove(gameObject);

            if(!string.IsNullOrEmpty(gameObject.Name) && gameObjectMap.ContainsKey(gameObject.Name))
            {
                gameObjectMap.Remove(gameObject.Name);
            }
        }

        public GameObject? GetGameObjectByName(string name)
        {
            if(gameObjectMap.TryGetValue(name, out GameObject? gameObject))
            {
                return gameObject;
            }
            return null;
        }
        
        public void Update(float deltaTime)
        {
            // Update all root objects
            foreach(var gameObject in gameObjects)
            {
                // Update game object deltaTime
                UpdateGameObjectHierarchy(gameObject, deltaTime);
            }
        }

        // Helper method to update the game object hierarchy
        private void UpdateGameObjectHierarchy(GameObject gameObject, float deltaTime)
        {
            // Update this game object
            gameObject.Update(deltaTime);
            
            foreach(var childTransform in gameObject.Transform.Children)
            {
                var childGameObject = childTransform.Owner;
                if(childGameObject != null)
                {
                    UpdateGameObjectHierarchy(childGameObject, deltaTime);
                }
            }
        }

        // Helper method for finding a game object by its transform
        private GameObject? FindGameObjectByTransform(Transform transform)
        {
            return transform.Owner;
        }

        public void Render()
        {
            if (ActiveShader == null) return;

            ActiveShader.Use();

            // Only render root objects, children will be rendered automatically
            foreach(var gameObject in gameObjects)
            {
                RenderGameObjectHierarchy(gameObject, ActiveShader);
            }
        }

        // Helper method for rendering the game object hierarchy
        private void RenderGameObjectHierarchy(GameObject gameObject, Shader shader)
        {
            // Render this game object
            gameObject.Render(shader);

            // Render all children
            foreach(var childTransform in gameObject.Transform.Children)
            {
                var childGameObject = FindGameObjectByTransform(childTransform);
                if(childGameObject != null)
                {
                    RenderGameObjectHierarchy(childGameObject, shader);
                }
            }
        }
    }
}

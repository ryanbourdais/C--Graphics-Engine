// File: OpenGLEngine/Resources/CameraController.cs
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLEngine.Resources;

namespace OpenGLEngine.Resources
{
    public class CameraController
    {
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastMousePosition;
        
        public CameraController(Camera camera)
        {
            _camera = camera;
        }
        
        public void Update(KeyboardState keyboard, float deltaTime)
        {
            // Process keyboard input
            if (keyboard.IsKeyDown(Keys.W))
                _camera.ProcessKeyboard(CameraMovement.Forward, deltaTime);
            if (keyboard.IsKeyDown(Keys.S))
                _camera.ProcessKeyboard(CameraMovement.Backward, deltaTime);
            if (keyboard.IsKeyDown(Keys.A))
                _camera.ProcessKeyboard(CameraMovement.Left, deltaTime);
            if (keyboard.IsKeyDown(Keys.D))
                _camera.ProcessKeyboard(CameraMovement.Right, deltaTime);
        }
        
        public void OnMouseMove(float x, float y)
        {
            if (_firstMove)
            {
                _lastMousePosition = new Vector2(x, y);
                _firstMove = false;
                return;
            }
            
            // Calculate mouse offset
            float xOffset = x - _lastMousePosition.X;
            float yOffset = _lastMousePosition.Y - y; // Reversed since y-coordinates go from bottom to top
            
            _lastMousePosition = new Vector2(x, y);
            
            // Process mouse movement for camera
            _camera.ProcessMouseMovement(xOffset, yOffset);
        }
        
        public void OnMouseWheel(float offset)
        {
            // Process mouse scroll for camera zoom
            _camera.ProcessMouseScroll(offset);
        }
    }
}
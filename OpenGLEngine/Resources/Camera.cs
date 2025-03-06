using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLEngine.Resources
{
    public enum CameraMovement
    {
        Forward,
        Backward,
        Left,
        Right
    }

    public class Camera
    {
        // Camera position and orientation
        public Vector3 Position { get; set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 WorldUp { get; set; }
        
        // Euler angles
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        
        // Camera options
        public float MovementSpeed { get; set; }
        public float MouseSensitivity { get; set; }
        public float Zoom { get; set; }
        
        // Projection parameters
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }

        private Vector3 _velocity = Vector3.Zero;
        private float _smoothTime = 0.1f;

        public Camera(Vector3 position = default, Vector3 worldUp = default, float yaw = -90.0f, float pitch = 0.0f)
        {
            Position = position;
            WorldUp = worldUp == default ? Vector3.UnitY : worldUp;
            Yaw = yaw;
            Pitch = pitch;

            // Default Camera Options
            MovementSpeed = 2.5f;
            MouseSensitivity = 0.1f;
            Zoom = 45.0f;

            // Default Projection Settings
            FieldOfView = 45.0f;
            AspectRatio = 16.0f / 9.0f;
            NearPlane = 0.1f;
            FarPlane = 100.0f;

            // Initialize camera vectors
            UpdateCameraVectors();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(FieldOfView),
                AspectRatio,
                NearPlane,
                FarPlane
            );
        }

        public void ProcessKeyboard(CameraMovement direction, float deltaTime)
        {
            Vector3 targetVelocity = Vector3.Zero;
            float speed = MovementSpeed;
            
            if (direction == CameraMovement.Forward)
                targetVelocity += Front * speed;
            if (direction == CameraMovement.Backward)
                targetVelocity -= Front * speed;
            if (direction == CameraMovement.Left)
                targetVelocity -= Right * speed;
            if (direction == CameraMovement.Right)
                targetVelocity += Right * speed;
            
            // Smooth damp
            _velocity = Vector3.Lerp(_velocity, targetVelocity, 1 - MathF.Exp(-deltaTime / _smoothTime));
            
            // Apply velocity
            Position += _velocity * deltaTime;
        }

        public void ProcessMouseMovement(float xOffset, float yOffset, bool constrainPitch = true)
        {
            xOffset *= MouseSensitivity;
            yOffset *= MouseSensitivity;

            Yaw += xOffset;
            Pitch += yOffset;

            if (constrainPitch)
            {
                if (Pitch > 89.0f)
                    Pitch = 89.0f;
                if (Pitch < -89.0f)
                    Pitch = -89.0f;
            }

            UpdateCameraVectors();
        }

        public void ProcessMouseScroll(float yOffset)
        {
            Zoom -= yOffset;

            if (Zoom < 1.0f)
                Zoom = 1.0f;
            if (Zoom > 45.0f)
                Zoom = 45.0f;

            // Update field of view based on zoom
            FieldOfView = Zoom;
        }

        public void UpdateCameraVectors()
        {
            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));

            Front = Vector3.Normalize(front);

            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }
}

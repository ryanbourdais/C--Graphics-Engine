using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenGLEngine.Resources
{
    public class Transform
    {
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;

        public Vector3 Position
        {
            get => _position;
            private set => _position = value;
        }

        public Vector3 Rotation
        {
            get => _rotation;
            private set => _rotation = value;
        }

        public Vector3 Scale
        {
            get => _scale;
            private set => _scale = value;
        }

        public Transform? Parent { get; private set; }
        public List<Transform> Children { get; } = new List<Transform>();

        public Transform()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
        }

        public Matrix4 GetModelMatrix()
        {
            //Create rotation matrices for each axis
            Matrix4 rotX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X));
            Matrix4 rotY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y));
            Matrix4 rotZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z));

            //Combine the rotations
            Matrix4 rotation = rotX * rotY * rotZ;

            //Create scale matrix
            Matrix4 scale = Matrix4.CreateScale(_scale);

            //Create Translation matrix
            Matrix4 translation = Matrix4.CreateTranslation(_position);

            return scale * rotation * translation;
        }

        // Move the object relative to its current position in the world
        public void Translate(Vector3 offset)
        {
            _position += offset;
        }

        // Set the position of the object
        public void SetPosition(Vector3 position)
        {
            _position = position;
        }
        
        //Rotate the object relative to its current rotation
        public void Rotate(Vector3 eulerAngles)
        {
            _rotation += eulerAngles;

            // Normalize angles to keep them in a reasonable range
            _rotation.X %= 360.0f;
            _rotation.Y %= 360.0f;
            _rotation.Z %= 360.0f;
        }

        // Set the object's absolute rotation
        public void SetRotation(Vector3 eulerAngles)
        {
            _rotation = eulerAngles;
        }

        // Scale the object relative to its current scale
        public void ScaleBy(Vector3 scale)
        {
            _scale *= scale;
        }

        // Set the object's absolute scale
        public void SetScale(Vector3 scale)
        {
            _scale = scale;
        }

        // Reset the transform to the identity
        public void Reset()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
        }

        public void AddChild(Transform child)
        {
            if (child == null) return;

            child.Parent?.Children.Remove(child);

            child.Parent = this;
            Children.Add(child);
        }

        public void RemoveChild(Transform child)
        {
            if (child == null) return;

            if (child.Parent == this)
            {
                child.Parent = null;
                Children.Remove(child);
            }
        }

        public Matrix4 GetWorldMatrix()
        {
            Matrix4 localMatrix = GetModelMatrix();

            if(Parent != null)
            {
                return localMatrix * Parent.GetWorldMatrix();
            }

            return localMatrix;
        }
    }
}

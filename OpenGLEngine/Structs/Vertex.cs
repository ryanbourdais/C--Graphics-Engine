using OpenTK.Mathematics;

namespace OpenGLEngine.Structs
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinates;

        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoordinates)
        {
            Position = position;
            Normal = normal;
            TextureCoordinates = textureCoordinates;
        }
    }

}
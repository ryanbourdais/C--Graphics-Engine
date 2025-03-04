using System;
using System.IO;

namespace OpenGLEngine.Resources
{
    public static class TexturedMeshFactory
    {
        /// <summary>
        /// Creates a mesh with the specified texture applied to it
        /// </summary>
        /// <param name="baseMesh">The base mesh to apply the texture to</param>
        /// <param name="texturePath">Path to the texture file</param>
        /// <returns>A new mesh with the texture applied</returns>
        public static Mesh CreateTexturedMesh(Mesh baseMesh, string texturePath)
        {
            // Validate inputs
            if (baseMesh == null)
            {
                throw new ArgumentNullException(nameof(baseMesh), "Base mesh cannot be null");
            }

            if (string.IsNullOrEmpty(texturePath))
            {
                throw new ArgumentException("Texture path cannot be null or empty", nameof(texturePath));
            }

            if (!File.Exists(texturePath))
            {
                throw new FileNotFoundException($"Texture file not found: {texturePath}");
            }

            // Load the texture
            Texture texture = new Texture(texturePath);
            
            // Create a new mesh with the same vertices and indices but with the texture
            return new Mesh(baseMesh.Vertices, baseMesh.Indices, texture);
        }
    }
}

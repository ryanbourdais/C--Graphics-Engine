using OpenTK.Mathematics;
using OpenGLEngine.Structs;
using OpenGLEngine.Resources;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace OpenGLEngine.Resources
{
    public static class MeshFactory
    {
        public static Mesh CreatePlane(float width = 1.0f, float depth = 1.0f, int widthSegments = 1, int depthSegments = 1)
        {
            // Ensure we have at least 1 segment
            widthSegments = Math.Max(1, widthSegments);
            depthSegments = Math.Max(1, depthSegments);
            
            // Calculate the number of vertices and indices
            int vertexCount = (widthSegments + 1) * (depthSegments + 1);
            int indexCount = widthSegments * depthSegments * 6; // 2 triangles per segment, 3 indices per triangle
            
            // Create arrays for vertices and indices
            Vertex[] vertices = new Vertex[vertexCount];
            int[] indices = new int[indexCount];
            
            // Calculate the size of each segment
            float segmentWidth = width / widthSegments;
            float segmentDepth = depth / depthSegments;
            
            // Calculate the starting position (centered at origin)
            float startX = -width / 2;
            float startZ = -depth / 2;
            
            // Generate vertices
            for (int z = 0; z <= depthSegments; z++)
            {
                for (int x = 0; x <= widthSegments; x++)
                {
                    int index = z * (widthSegments + 1) + x;
                    
                    // Calculate position
                    float posX = startX + x * segmentWidth;
                    float posZ = startZ + z * segmentDepth;
                    
                    // Calculate texture coordinates (0,0 at bottom-left, 1,1 at top-right)
                    float texU = (float)x / widthSegments;
                    float texV = (float)z / depthSegments;
                    
                    // Create the vertex
                    vertices[index] = new Vertex
                    {
                        Position = new Vector3(posX, 0, posZ),
                        Normal = Vector3.UnitY, // Normal points up for a plane on XZ
                        TextureCoordinates = new Vector2(texU, texV)
                    };
                }
            }
            
            // Generate indices (two triangles per grid cell)
            int indexOffset = 0;
            for (int z = 0; z < depthSegments; z++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    // Calculate the indices of the four corners of this grid cell
                    int topLeft = z * (widthSegments + 1) + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = (z + 1) * (widthSegments + 1) + x;
                    int bottomRight = bottomLeft + 1;
                    
                    // First triangle (top-left, bottom-left, bottom-right)
                    indices[indexOffset++] = topLeft;
                    indices[indexOffset++] = bottomLeft;
                    indices[indexOffset++] = bottomRight;
                    
                    // Second triangle (top-left, bottom-right, top-right)
                    indices[indexOffset++] = topLeft;
                    indices[indexOffset++] = bottomRight;
                    indices[indexOffset++] = topRight;
                }
            }
            
            // Create and return the mesh (with no textures for now)
            return new Mesh(vertices, indices, null);
        }
        
        // You can add more factory methods here for other shapes
        public static Mesh CreateCube(float width = 1.0f, float height = 1.0f, float depth = 1.0f) 
        { 
            // Calculate the half dimensions
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            float halfDepth = depth / 2;

            // Create vertices for all 8 corners of the cube
            Vertex[] vertices = new Vertex[]
            {
                // Front face (positive Z)
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, halfDepth), Normal = Vector3.UnitZ, TextureCoordinates = new Vector2(0, 0) },
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, halfDepth), Normal = Vector3.UnitZ, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, halfDepth), Normal = Vector3.UnitZ, TextureCoordinates = new Vector2(1, 1) },
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, halfDepth), Normal = Vector3.UnitZ, TextureCoordinates = new Vector2(0, 1) },
                
                // Back face (negative Z)
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, -halfDepth), Normal = -Vector3.UnitZ, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, -halfDepth), Normal = -Vector3.UnitZ, TextureCoordinates = new Vector2(1, 1) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, -halfDepth), Normal = -Vector3.UnitZ, TextureCoordinates = new Vector2(0, 1) },
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, -halfDepth), Normal = -Vector3.UnitZ, TextureCoordinates = new Vector2(0, 0) },
                
                // Top face (positive Y)
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, -halfDepth), Normal = Vector3.UnitY, TextureCoordinates = new Vector2(0, 1) },
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, halfDepth), Normal = Vector3.UnitY, TextureCoordinates = new Vector2(0, 0) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, halfDepth), Normal = Vector3.UnitY, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, -halfDepth), Normal = Vector3.UnitY, TextureCoordinates = new Vector2(1, 1) },
                
                // Bottom face (negative Y)
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, -halfDepth), Normal = -Vector3.UnitY, TextureCoordinates = new Vector2(0, 0) },
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, -halfDepth), Normal = -Vector3.UnitY, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, halfDepth), Normal = -Vector3.UnitY, TextureCoordinates = new Vector2(1, 1) },
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, halfDepth), Normal = -Vector3.UnitY, TextureCoordinates = new Vector2(0, 1) },
                
                // Right face (positive X)
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, -halfDepth), Normal = Vector3.UnitX, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, -halfDepth), Normal = Vector3.UnitX, TextureCoordinates = new Vector2(1, 1) },
                new Vertex { Position = new Vector3(halfWidth, halfHeight, halfDepth), Normal = Vector3.UnitX, TextureCoordinates = new Vector2(0, 1) },
                new Vertex { Position = new Vector3(halfWidth, -halfHeight, halfDepth), Normal = Vector3.UnitX, TextureCoordinates = new Vector2(0, 0) },
                
                // Left face (negative X)
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, -halfDepth), Normal = -Vector3.UnitX, TextureCoordinates = new Vector2(0, 0) },
                new Vertex { Position = new Vector3(-halfWidth, -halfHeight, halfDepth), Normal = -Vector3.UnitX, TextureCoordinates = new Vector2(1, 0) },
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, halfDepth), Normal = -Vector3.UnitX, TextureCoordinates = new Vector2(1, 1) },
                new Vertex { Position = new Vector3(-halfWidth, halfHeight, -halfDepth), Normal = -Vector3.UnitX, TextureCoordinates = new Vector2(0, 1) },
            };

            // Create indices for the 12 triangles (6 faces * 2 triangles)
            int[] indices = new int[]
            {
                // Front face
                0, 1, 2,
                2, 3, 0,
                
                // Back face
                4, 5, 6,
                6, 7, 4,
                
                // Top face
                8, 9, 10,
                10, 11, 8,
                
                // Bottom face
                12, 13, 14,
                14, 15, 12,
                
                // Right face
                16, 17, 18,
                18, 19, 16,
                
                // Left face
                20, 21, 22,
                22, 23, 20
            };

            // Create and return the mesh (with no textures for now)
            return new Mesh(vertices, indices, null);
        }

        public static Mesh CreateSphere(float radius = 1.0f, int sectors = 36, int stacks = 18)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<int> indices = new List<int>();
            
            // Ensure we have at least 3 sectors and 2 stacks
            sectors = Math.Max(3, sectors);
            stacks = Math.Max(2, stacks);
            
            float sectorStep = 2 * MathF.PI / sectors;
            float stackStep = MathF.PI / stacks;
            
            // Generate vertices
            for (int i = 0; i <= stacks; ++i)
            {
                float stackAngle = MathF.PI / 2 - i * stackStep;  // starting from pi/2 to -pi/2
                float xy = radius * MathF.Cos(stackAngle);        // r * cos(u)
                float z = radius * MathF.Sin(stackAngle);         // r * sin(u)
                
                // Add (sectors+1) vertices per stack
                // The first and last vertices have same position and normal, but different tex coords
                for (int j = 0; j <= sectors; ++j)
                {
                    float sectorAngle = j * sectorStep;           // starting from 0 to 2pi
                    
                    // Vertex position
                    float x = xy * MathF.Cos(sectorAngle);        // r * cos(u) * cos(v)
                    float y = xy * MathF.Sin(sectorAngle);        // r * cos(u) * sin(v)
                    
                    // Normalized vertex normal
                    Vector3 normal = Vector3.Normalize(new Vector3(x, y, z));
                    
                    // Vertex tex coord between [0, 1]
                    float s = (float)j / sectors;
                    float t = (float)i / stacks;
                    
                    vertices.Add(new Vertex
                    {
                        Position = new Vector3(x, y, z),
                        Normal = normal,
                        TextureCoordinates = new Vector2(s, t)
                    });
                }
            }
            
            // Generate indices
            // k1--k1+1
            // |  / |
            // | /  |
            // k2--k2+1
            int k1, k2;
            for (int i = 0; i < stacks; ++i)
            {
                k1 = i * (sectors + 1);     // beginning of current stack
                k2 = k1 + sectors + 1;      // beginning of next stack
                
                for (int j = 0; j < sectors; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding the first and last stacks
                    
                    // k1 => k2 => k1+1
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }
                    
                    // k1+1 => k2 => k2+1
                    if (i != (stacks - 1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
            
            return new Mesh(vertices.ToArray(), indices.ToArray(), null);
        }
    }
}

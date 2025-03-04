using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Runtime.InteropServices;
using OpenGLEngine;
using OpenGLEngine.Structs;

namespace OpenGLEngine.Resources
{
   
    public class Mesh : IDisposable
    {
        public Vertex[] Vertices;
        public int[] Indices;
        public Texture? Texture;
        public Transform Transform;

        private int _vao, _vbo, _ebo;

        public Mesh(Vertex[] vertices, int[] indices, Texture? texture = null)
        {
            Vertices = vertices;
            Indices = indices;
            Texture = texture;

            Transform = new Transform();

            SetupMesh();
        }

        private void SetupMesh()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            // Calculate the stride based on the size of the Vertex struct
            int vertexSize = Marshal.SizeOf<Vertex>();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * vertexSize, Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(int), Indices, BufferUsageHint.StaticDraw);

            // Position attribute (offset 0)
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

            // Normal attribute (offset = size of Vector3)
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Normal").ToInt32());

            // Texture coordinate attribute (offset = size of Vector3 + size of Vector3)
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("TextureCoordinates").ToInt32());

            GL.BindVertexArray(0);
        }

        public void Draw(Shader shader)
        {

            shader.SetMatrix4("model", Transform.GetModelMatrix());

            if (Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                string name = Texture.path ?? "texture_diffuse";
                
                shader.SetInt("material.texture_diffuse1", 0);
                GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);
            }
            
            GL.ActiveTexture(TextureUnit.Texture0);

            // Draw the mesh
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
        }
    }
}
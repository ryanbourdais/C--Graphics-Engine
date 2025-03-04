using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenGLEngine.Resources
{
    public class Texture
    {
        public int Handle { get; set; }
        public string? path { get; set; }
        public Texture()
        {
            Handle = 0;
            path = null;
        }

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            Use();
            // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            // This will correct that, making the texture display properly.
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }


    }
}
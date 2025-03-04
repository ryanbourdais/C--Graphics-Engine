#version 330 core
out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D ourTexture;  // Texture sampler
// For now, just use a solid color
void main()
{
    FragColor = texture(ourTexture, TexCoord);
}
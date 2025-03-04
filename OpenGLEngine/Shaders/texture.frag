#version 330 core

in vec2 TexCoord;  // Make sure this matches the output from vertex shader
out vec4 FragColor;

uniform sampler2D ourTexture;  // Texture sampler

void main()
{
    FragColor = texture(ourTexture, TexCoord);
}

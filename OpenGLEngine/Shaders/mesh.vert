#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec2 TexCoord;

uniform mat4 model;         // Model matrix
uniform mat4 view;          // View matrix
uniform mat4 projection;    // Projection matrix

void main()
{
    // Transform vertex position using all matrices
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    
    // Pass texture coordinates to fragment shader
    TexCoord = aTexCoord;
}
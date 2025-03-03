#version 330 core
layout (location = 0) in vec3 aPos; // the position variable has attribute position 0
layout (location = 1) in vec3 aColor; // the color variable has attribute position 1

uniform mat4 transform = mat4(1.0);
uniform bool useTransform = false;
uniform mat4 scale = mat4(1.0);
uniform bool useScale = false;

out vec3 vertexColor; // output a color to the fragment shader
void main()
{
    vec4 position = vec4(aPos, 1.0);
    if (useTransform)
    {
        position = position * transform;
    }
    if (useScale)
    {
        position = position * scale;
    }
    gl_Position = position;
    vertexColor = aColor; // set the output color to the input color
}
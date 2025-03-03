#version 330 core

out vec4 FragColor;

in vec3 vertexColor;
uniform vec4 animatedColor;
uniform bool useAnimatedColor;
uniform float time;

void main()
{
    if (useAnimatedColor)
    {
        float angle = time;

        mat3 colorRotation = mat3(
            cos(angle), sin(angle), 0.0,
            -sin(angle), cos(angle), 0.0,
            0.0, 0.0, 1.0
        );

        vec3 rotatedColor = colorRotation * vertexColor;

        float brightness = animatedColor.g;
        vec3 finalColor = rotatedColor * brightness;
        FragColor = vec4(finalColor, 1.0);
    }
    else
    {
        FragColor = vec4(vertexColor, 1.0);
    }
}

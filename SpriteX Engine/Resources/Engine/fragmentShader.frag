#version 330 core

uniform vec4 uColor;
uniform sampler2D uTexture;

in vec2 texCoords;

out vec4 FragColor;

void main()
{
	FragColor = texture(uTexture, texCoords) * uColor;
}
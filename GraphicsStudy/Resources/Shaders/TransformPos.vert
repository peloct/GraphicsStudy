#version 430 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in ivec2 aBoneIndex;
layout (location = 2) in vec2 aBoneWeight;

uniform mat4 mvp;
uniform mat4 bones[2];

void main()
{
	vec4 pos = vec4(aPos, 1.0);

	vec4 newPos = vec4(0, 0, 0, 0);
	
	newPos += aBoneWeight[0] * bones[aBoneIndex[0]] * pos;
	newPos += aBoneWeight[1] * bones[aBoneIndex[1]] * pos;

	gl_Position = mvp * newPos;
}
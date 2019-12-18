#version 430 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in uvec4 aBoneIndex[4];
layout (location = 5) in vec4 aBoneWeight[4];

uniform mat4 mvp;
uniform mat4 skinningTransforms[50];

void main()
{
    vec4 pos = vec4(aPos, 1.0);
	float totalWeight = 0;

	vec4 blendPos = vec4(0, 0, 0, 0);

	for (int i = 0; i < 4; ++i)
	{
		for (int j = 0; j < 4; ++j)
		{
		    blendPos += aBoneWeight[i][j] * skinningTransforms[aBoneIndex[i][j]] * pos;
			totalWeight += aBoneWeight[i][j];
		}
	}

	if (totalWeight > 0)
		pos = blendPos / totalWeight;

	gl_Position = mvp * pos;
}
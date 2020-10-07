layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec3 aNormal;
 
out vec4 fragcol; 
out vec3 apos;
out vec3 anormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    gl_Position = projection * view * model * vec4(aPos, 1.0);
    fragcol = aColor;
	apos = aPos;
    anormal = mat3(transpose(inverse(model))) * aNormal;
} 
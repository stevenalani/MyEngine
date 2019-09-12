attribute vec3 aPos;
attribute vec4 aColor;
 
varying vec4 fragcol; 
varying vec3 apos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    gl_Position = projection * view * model * vec4(aPos, 1.0);
    fragcol = aColor;
	apos = aPos;
} 
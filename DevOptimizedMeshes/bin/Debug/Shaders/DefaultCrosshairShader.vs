#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 aColor;
  
out vec4 fragcol; 

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec4 ambient;
uniform vec4 specular;
uniform vec4 diffuse;

void main()
{
    gl_Position = projection * view * model * vec4(aPos,1.0);
    fragcol = aColor;
} 
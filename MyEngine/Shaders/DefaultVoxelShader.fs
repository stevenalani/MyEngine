#version 330 core
out vec4 FragColor; 

uniform vec4 ambientLight;
uniform vec4 diffuseLight;
uniform vec3 diffuseLightpos;
uniform vec4 specular;

in vec4 fragcol;
in vec3 anormal;

vec3 diffuselight(vec3 fragpos,vec3 normal, vec3 lightpos){
    vec3 lightdir = lightpos - fragpos;

    float diff = max(dot(normal, lightdir), 0.0);
    vec3 diffuse = diff * diffuseLight.xyz;
    return diffuse;
}
vec3 spec(){
    return vec3(0);
}
void main()
{
    vec3 normal = cross(dFdy(anormal.xyz), dFdx(anormal.xyz));
	normal = normalize(normal);
    vec3 ambient = ambientLight.xyz * ambientLight.w;
    vec3 diffuse = diffuselight(anormal, normal, diffuseLightpos);
    FragColor = vec4(fragcol.xyz * (ambient + diffuse),fragcol.w);
}
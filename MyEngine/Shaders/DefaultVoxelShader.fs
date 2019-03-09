#version 330 core
out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform vec3 viewpos;

in vec4 fragcol;
in vec3 apos;

void main()
{
    vec3 ambient = lightColor * ambientStrength;
    vec3 norm = normalize(cross(dFdy(apos.xyz), dFdx(apos.xyz)));
    vec3 lightDir = normalize(lightPos - apos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 viewDir = normalize(viewpos - apos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * fragcol;
    FragColor = vec4(result, fragcol.w);

}

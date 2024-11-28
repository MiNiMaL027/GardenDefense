shader_type spatial;

uniform sampler2D base_texture; // Базова текстура
uniform float transparency = 0.7; // Прозорість
uniform vec3 tint_color = vec3(0.8, 0.9, 1.0); // Колір скла

void fragment() {
    vec4 base_color = texture(base_texture, UV); // Зчитування текстури

    // Змішування базового кольору та скла
    ALBEDO = mix(base_color.rgb, tint_color, transparency);
    ALPHA = 1.0 - transparency;

    // Параметри блиску
    SPECULAR = 1.0;
    ROUGHNESS = 0.1;
}

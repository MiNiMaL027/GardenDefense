shader_type canvas_item;

uniform float fill_duration : hint_range(0.8, 10.0) = 0.8; // Тривалість одного циклу заповнення в секундах
uniform float time = 0.f;

void fragment() {
    float elapsed_time = mod(time, fill_duration);

    vec2 center = vec2(0.5); // центр кнопки
    float radius = 0.4; // радіус кола

    vec2 pos = UV - center;
    float distance = length(pos);

    // Перевірка, чи знаходиться точка в межах кола
    if (distance <= radius) {
        float angle = degrees(atan(pos.y, pos.x));
        
        float end_angle = (elapsed_time / fill_duration) * 360.0;
        
        if (end_angle < 0.0) end_angle += 360.0;
        
        float adjusted_angle = angle;
        if (adjusted_angle < 0.0) adjusted_angle += 360.0;
        
        if (adjusted_angle <= end_angle) {
            COLOR = vec4(1.0);
        } else {
            COLOR = texture(TEXTURE, UV);
        }
    } else {
        COLOR = texture(TEXTURE, UV);
    }
}
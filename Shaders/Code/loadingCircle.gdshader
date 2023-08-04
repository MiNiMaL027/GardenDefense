shader_type canvas_item;

uniform float fill_duration : hint_range(0.8, 10.0) = 0.8; // Тривалість одного циклу заповнення в секундах

void fragment() {
    vec2 center = vec2(0.5); // центр кнопки
    float radius = 0.4; // радіус кола

    vec2 pos = UV - center;
    float distance = length(pos);

    // Перевірка, чи знаходиться точка в межах кола
    if (distance <= radius) {
        // Визначаємо кут між лінією радіуса і точкою
        float angle = degrees(atan(pos.y, pos.x));
        
        // Обчислюємо пройдений час від початку заповнення
        float elapsed_time = mod(TIME, fill_duration);
        
        // Визначаємо кут для заповнення дуги на основі пройденого часу
        float end_angle = (elapsed_time / fill_duration) * 360.0;
        
        // Коригуємо кути для позитивних значень, щоб вони були від 0 до 360 градусів
        if (end_angle < 0.0) end_angle += 360.0;
        
        // Обчислюємо реальний кут точки на колі
        float adjusted_angle = angle;
        if (adjusted_angle < 0.0) adjusted_angle += 360.0;
        
        // Якщо кут знаходиться в діапазоні заповнення дуги, малюємо біле коло
        if (adjusted_angle <= end_angle) {
            COLOR = vec4(1.0);
        } else {
            // Якщо кут поза діапазоном заповнення, використовуємо текстуру кнопки
            COLOR = texture(TEXTURE, UV);
        }
    } else {
        // Якщо точка знаходиться поза колом, залишаємо її без змін
        COLOR = texture(TEXTURE, UV);
    }
}
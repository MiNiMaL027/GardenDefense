shader_type canvas_item;

uniform float cutoff : hint_range(0.0, 1.0) = 0.0; // Поточний рівень видалення від 0 до 1
uniform float transparency : hint_range(0.0, 1.0) = 0.5; // Стала прозорість квадрата

void fragment() {
    // Отримуємо координати фрагмента у діапазоні від 0 до 1
    vec2 uv = UV;

    // Якщо координата y нижче поточного рівня cutoff, робимо піксель прозорим
    if (uv.y > cutoff) {
        discard;
    } else {
        // Відображаємо текстуру об'єкта
        vec4 color = texture(TEXTURE, UV);
        // Додаємо сталу прозорість
        color.a = transparency;
        COLOR = color;
    }
}
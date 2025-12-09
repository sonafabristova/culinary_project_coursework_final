using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class Пользователи
{
    public int IdПользователя { get; set; }

    public string Имя { get; set; } = null!;

    public string Пароль { get; set; } = null!;

    public string Логин { get; set; } = null!;

    public string? Телефон { get; set; }

    public string Почта { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Меню> Менюs { get; set; } = new List<Меню>();

    public virtual ICollection<Рецепты> Рецептыs { get; set; } = new List<Рецепты>();

    public virtual ICollection<СпискиПокупок> СпискиПокупокs { get; set; } = new List<СпискиПокупок>();
}

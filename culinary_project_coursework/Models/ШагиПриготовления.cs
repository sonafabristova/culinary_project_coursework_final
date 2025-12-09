using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class ШагиПриготовления
{
    public int IdШага { get; set; }

    public int FkРецепта { get; set; }

    public int НомерШага { get; set; }

    public string Описание { get; set; } = null!;

    public string? Изображение { get; set; }

    public virtual Рецепты FkРецептаNavigation { get; set; } = null!;
}

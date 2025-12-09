using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class СоставБлюда
{
    public int IdСостава { get; set; }

    public int FkРецепта { get; set; }

    public int FkИнгредиента { get; set; }

    public decimal Количество { get; set; }

    public virtual Ингредиенты FkИнгредиентаNavigation { get; set; } = null!;

    public virtual Рецепты FkРецептаNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class ПриемыПищи
{
    public int IdПриема { get; set; }

    public int FkЧеловека { get; set; }

    public int FkДня { get; set; }

    public string ТипПриема { get; set; } = null!;

    public int? FkРецепта { get; set; }

    public int? Порции { get; set; }

    public virtual ДниМеню FkДняNavigation { get; set; } = null!;

    public virtual Рецепты? FkРецептаNavigation { get; set; }

    public virtual ЛюдиВМеню FkЧеловекаNavigation { get; set; } = null!;
}

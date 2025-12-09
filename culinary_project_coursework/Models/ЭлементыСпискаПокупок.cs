using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class ЭлементыСпискаПокупок
{
    public int IdЭлемента { get; set; }

    public int FkСписка { get; set; }

    public int FkИнгредиента { get; set; }

    public decimal Количество { get; set; }

    public bool? IsPurchased { get; set; }

    public virtual Ингредиенты FkИнгредиентаNavigation { get; set; } = null!;

    public virtual СпискиПокупок FkСпискаNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class Ингредиенты
{
    public int IdИнгредиента { get; set; }

    public string Название { get; set; } = null!;

    public int? FkЕдиницыИзмерения { get; set; }

    public decimal? СрСтоимость { get; set; }

    public virtual ЕдиницаИзмерения? FkЕдиницыИзмеренияNavigation { get; set; }

    public virtual ICollection<СоставБлюда> СоставБлюдаs { get; set; } = new List<СоставБлюда>();

    public virtual ICollection<ЭлементыСпискаПокупок> ЭлементыСпискаПокупокs { get; set; } = new List<ЭлементыСпискаПокупок>();
}

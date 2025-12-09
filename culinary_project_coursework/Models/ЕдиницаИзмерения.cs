using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class ЕдиницаИзмерения
{
    public int IdЕдиницыИзмерения { get; set; }

    public string Название { get; set; } = null!;

    public virtual ICollection<Ингредиенты> Ингредиентыs { get; set; } = new List<Ингредиенты>();
}

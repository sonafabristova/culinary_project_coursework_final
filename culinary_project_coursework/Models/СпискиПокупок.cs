using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class СпискиПокупок
{
    public int IdСписка { get; set; }

    public int FkПользователя { get; set; }

    public string Название { get; set; } = null!;

    public bool? IsCompleted { get; set; }

    public virtual Пользователи FkПользователяNavigation { get; set; } = null!;

    public virtual ICollection<ЭлементыСпискаПокупок> ЭлементыСпискаПокупокs { get; set; } = new List<ЭлементыСпискаПокупок>();
}

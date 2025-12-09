using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class Меню
{
    public int IdМеню { get; set; }

    public string Название { get; set; } = null!;

    public int FkПользователя { get; set; }

    public int КоличествоДней { get; set; }

    public int КоличествоЧеловек { get; set; }

    public bool? IsActive { get; set; }

    public virtual Пользователи FkПользователяNavigation { get; set; } = null!;

    public virtual ICollection<ДниМеню> ДниМенюs { get; set; } = new List<ДниМеню>();

    public virtual ICollection<ЛюдиВМеню> ЛюдиВМенюs { get; set; } = new List<ЛюдиВМеню>();
}

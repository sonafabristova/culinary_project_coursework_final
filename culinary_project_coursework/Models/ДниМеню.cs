using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class ДниМеню
{
    public int IdДня { get; set; }

    public int FkМеню { get; set; }

    public int НомерДня { get; set; }

    public string? НазваниеДня { get; set; }

    public virtual Меню FkМенюNavigation { get; set; } = null!;

    public virtual ICollection<ПриемыПищи> ПриемыПищиs { get; set; } = new List<ПриемыПищи>();
}

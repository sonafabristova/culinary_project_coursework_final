using System;
using System.Collections.Generic;

namespace culinary_project_coursework.Models;

public partial class Рецепты
{
    public int IdРецепта { get; set; }

    public string Название { get; set; } = null!;

    public string? Описание { get; set; }

    public int ВремяПриготовления { get; set; }

    public decimal? Белки { get; set; }

    public decimal? Жиры { get; set; }

    public decimal? Углеводы { get; set; }

    public int? Калории { get; set; }

    public bool? IsSystemRecipe { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Изображение { get; set; }

    public virtual Пользователи? CreatedByUser { get; set; }

    public virtual ICollection<ПриемыПищи> ПриемыПищиs { get; set; } = new List<ПриемыПищи>();

    public virtual ICollection<СоставБлюда> СоставБлюдаs { get; set; } = new List<СоставБлюда>();

    public virtual ICollection<ШагиПриготовления> ШагиПриготовленияs { get; set; } = new List<ШагиПриготовления>();
}

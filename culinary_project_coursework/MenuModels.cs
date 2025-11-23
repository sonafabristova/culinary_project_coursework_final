using System.Collections.Generic;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework
{
    // Основной класс для дня меню
    public class MenuDay
    {
        public int DayNumber { get; set; }
        public string DayTitle { get; set; }
        public List<MenuPerson> People { get; set; } = new List<MenuPerson>();
    }

    // Класс для человека в меню
    public class MenuPerson
    {
        public string PersonId { get; set; }
        public string PersonTitle { get; set; }
        public MenuMeal Breakfast { get; set; } = new MenuMeal();
        public MenuMeal Lunch { get; set; } = new MenuMeal();
        public MenuMeal Dinner { get; set; } = new MenuMeal();
    }

    // Класс для приема пищи
    public class MenuMeal
    {
        public string DishName { get; set; } = "Не выбрано";
        public Recipe SelectedRecipe { get; set; }
        public bool IsSelected => SelectedRecipe != null;
    }

    // Класс для элемента списка покупок
    public class ShoppingItem
    {
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Category { get; set; }
    }

    // Класс для хранения всего меню
    public class MenuPlan
    {
        public int DaysCount { get; set; }
        public int PeopleCount { get; set; }
        public List<MenuDay> Days { get; set; } = new List<MenuDay>();
        public string MenuName { get; set; }
    }
}
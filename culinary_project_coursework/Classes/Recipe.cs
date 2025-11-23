using System.Collections.Generic;

namespace culinary_project_coursework.Classes
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; } // в минутах
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public int UserId { get; set; } // 0 для системных рецептов
        public bool IsSystemRecipe { get; set; }

        // Списки ингредиентов и шагов приготовления
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<CookingStep> CookingSteps { get; set; } = new List<CookingStep>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Логика взаимодействия для RecipeDetailsWindow.xaml
    /// </summary>
    public partial class RecipeDetailsWindow : Window
    {
        public RecipeDetailsWindow(Recipe recipe)
        {
            InitializeComponent();
            LoadRecipeData(recipe);
        }

        private void LoadRecipeData(Recipe recipe)
        {
            // Устанавливаем заголовок окна и название рецепта
            this.Title = $"Рецепт - {recipe.Name}";
            TitleText.Text = recipe.Name;

            // БЖУ и калории
            ProteinsText.Text = $"{recipe.Proteins}г";
            FatsText.Text = $"{recipe.Fats}г";
            CarbsText.Text = $"{recipe.Carbohydrates}г";
            CaloriesText.Text = $"{recipe.Calories} ккал";

            // Время приготовления
            TimeText.Text = $"{recipe.CookingTime} минут";

            // Ингредиенты
            var ingredientsData = recipe.Ingredients
                .Select(i => new IngredientDisplay
                {
                    Name = i.Name,
                    Amount = $"{i.Amount}{i.Unit}"
                })
                .ToList();
            IngredientsList.ItemsSource = ingredientsData;

            // Шаги приготовления
            if (recipe.CookingSteps.Any())
            {
                var steps = recipe.CookingSteps
                    .OrderBy(s => s.StepNumber)
                    .Select(s => $"Шаг {s.StepNumber}: {s.Description}")
                    .ToList();
                StepsList.ItemsSource = steps;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Вспомогательный класс для отображения ингредиентов
    public class IngredientDisplay
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}


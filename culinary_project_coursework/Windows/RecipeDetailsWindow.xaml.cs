using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace culinary_project_coursework.Windows
{
    public partial class RecipeDetailsWindow : Window
    {
        public RecipeDetailsWindow(Рецепты recipe)
        {
            InitializeComponent();
            LoadRecipeData(recipe);
        }

        private void LoadRecipeData(Рецепты recipe)
        {
            // Устанавливаем заголовок окна и название рецепта
            this.Title = $"Рецепт - {recipe.Название}";
            TitleText.Text = recipe.Название;

            // БЖУ и калории
            ProteinsText.Text = $"{recipe.Белки?.ToString("0.##") ?? "0"}г";
            FatsText.Text = $"{recipe.Жиры?.ToString("0.##") ?? "0"}г";
            CarbsText.Text = $"{recipe.Углеводы?.ToString("0.##") ?? "0"}г";
            CaloriesText.Text = $"{recipe.Калории ?? 0} ккал";

            // Время приготовления
            TimeText.Text = $"{recipe.ВремяПриготовления} минут";

            // Ингредиенты
            if (recipe.СоставБлюдаs != null && recipe.СоставБлюдаs.Any())
            {
                var ingredientsData = recipe.СоставБлюдаs
                    .Select(i => new RecipeIngredientDisplay
                    {
                        Name = i.FkИнгредиентаNavigation?.Название ?? "Неизвестно",
                        Amount = $"{i.Количество} {i.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? "г"}"
                    })
                    .ToList();
                IngredientsList.ItemsSource = ingredientsData;
            }
            else
            {
                IngredientsList.ItemsSource = new List<RecipeIngredientDisplay>
                {
                    new RecipeIngredientDisplay { Name = "Нет информации об ингредиентах", Amount = "" }
                };
            }

            // Шаги приготовления
            if (recipe.ШагиПриготовленияs != null && recipe.ШагиПриготовленияs.Any())
            {
                var steps = recipe.ШагиПриготовленияs
                    .OrderBy(s => s.НомерШага)
                    .Select(s => $"Шаг {s.НомерШага}: {s.Описание}")
                    .ToList();
                StepsList.ItemsSource = steps;
            }
            else
            {
                StepsList.ItemsSource = new List<string> { "Нет информации о шагах приготовления" };
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Вспомогательный класс для отображения ингредиентов
    public class RecipeIngredientDisplay
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}
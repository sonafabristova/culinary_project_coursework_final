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

            // ОТЛАДКА: Показываем информацию в MessageBox
            string debugInfo = $"Рецепт: {recipe.Название}\n";
            debugInfo += $"ID рецепта: {recipe.IdРецепта}\n";
            debugInfo += $"СоставБлюдаs != null: {recipe.СоставБлюдаs != null}\n";
            debugInfo += $"Количество ингредиентов: {recipe.СоставБлюдаs?.Count ?? 0}\n\n";

            if (recipe.СоставБлюдаs != null && recipe.СоставБлюдаs.Any())
            {
                debugInfo += "Ингредиенты:\n";
                foreach (var ingredient in recipe.СоставБлюдаs.Take(3)) // Первые 3
                {
                    debugInfo += $"- {ingredient.FkИнгредиентаNavigation?.Название ?? "Без названия"}: " +
                                $"{ingredient.Количество} " +
                                $"{ingredient.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? ""}\n";
                }
            }
            else
            {
                debugInfo += "Нет ингредиентов!";
            }

            // Показываем отладочную информацию
            MessageBox.Show(debugInfo, "Отладка загрузки рецепта", MessageBoxButton.OK, MessageBoxImage.Information);

            // Ингредиенты
            if (recipe.СоставБлюдаs != null && recipe.СоставБлюдаs.Any())
            {
                var ingredientsData = recipe.СоставБлюдаs
                    .GroupBy(i => i.FkИнгредиента)
                    .Select(g => g.First())
                    .Select(i => new RecipeIngredientDisplay
                    {
                        Name = i.FkИнгредиентаNavigation?.Название ?? "Неизвестно",
                        Amount = FormatIngredientAmount(i.Количество, i.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? "г")
                    })
                    .ToList();

                // Показываем, что попадает в список
                string listDebug = $"Будет отображено {ingredientsData.Count} ингредиентов:\n";
                foreach (var item in ingredientsData)
                {
                    listDebug += $"- {item.Name}: {item.Amount}\n";
                }
                MessageBox.Show(listDebug, "Отладка списка ингредиентов", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private string FormatIngredientAmount(decimal amount, string unit)
        {
            // Простая проверка - целое ли число
            if (amount == Math.Truncate(amount))
            {
                // Целое число
                return $"{(int)amount} {unit}";
            }
            else
            {
                // Дробное число, убираем лишние нули
                return $"{amount:0.##} {unit}";
            }
        }
       

        // Альтернативный вариант для nullable decimal
        
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
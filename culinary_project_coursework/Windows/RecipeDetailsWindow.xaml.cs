using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
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
            this.Title = $"Рецепт - {recipe.Название}";
            TitleText.Text = recipe.Название;

            // Загружаем изображение рецепта
            LoadRecipeImage(recipe.Изображение);

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
                    .GroupBy(i => i.FkИнгредиента)
                    .Select(g => g.First())
                    .Select(i => new RecipeIngredientDisplay
                    {
                        Name = i.FkИнгредиентаNavigation?.Название ?? "Неизвестно",
                        Amount = FormatIngredientAmount(i.Количество, i.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? "г")
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

        private void LoadRecipeImage(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    RecipeImage.Source = null;
                    return;
                }

                
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;

                DirectoryInfo dir = new DirectoryInfo(exeDir);
                string projectRoot = dir.Parent?.Parent?.Parent?.FullName;

                string fullPath = Path.Combine(projectRoot, relativePath.Replace("/", "\\"));


                if (File.Exists(fullPath))
                {
                    RecipeImage.Source = new BitmapImage(new Uri(fullPath));
                   
                }
                else
                {
                    RecipeImage.Source = null;
                   
                }
            }
            catch (Exception ex)
            {
                RecipeImage.Source = null;
               
            }
        }

        private string FormatIngredientAmount(decimal amount, string unit)
        {
            if (amount == Math.Truncate(amount))
            {
                return $"{(int)amount} {unit}";
            }
            else
            {
                return $"{amount:0.##} {unit}";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class RecipeIngredientDisplay
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}
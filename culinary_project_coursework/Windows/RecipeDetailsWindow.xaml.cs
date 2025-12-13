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
        private Рецепты _recipe;

        public RecipeDetailsWindow(Рецепты recipe)
        {
            InitializeComponent();
            _recipe = recipe;
            LoadRecipeData();
        }

        private void LoadRecipeData()
        {
            try
            {
                // Устанавливаем заголовок окна и название рецепта
                this.Title = $"Рецепт - {_recipe.Название}";
                TitleText.Text = _recipe.Название;

                // Для отладки показываем путь к изображению
                Console.WriteLine($"Путь к изображению в рецепте: {_recipe.Изображение ?? "NULL"}");

                // Загружаем изображение рецепта
                LoadRecipeImage(_recipe.Изображение);

                // БЖУ и калории
                ProteinsText.Text = $"{_recipe.Белки?.ToString("0.##") ?? "0"}г";
                FatsText.Text = $"{_recipe.Жиры?.ToString("0.##") ?? "0"}г";
                CarbsText.Text = $"{_recipe.Углеводы?.ToString("0.##") ?? "0"}г";
                CaloriesText.Text = $"{_recipe.Калории ?? 0} ккал";

                // Время приготовления
                TimeText.Text = $"{_recipe.ВремяПриготовления} минут";

                // Загружаем ингредиенты из БД, если они не загружены
                LoadIngredientsFromDatabase();

                // Загружаем шаги из БД, если они не загружены
                LoadStepsFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных рецепта: {ex.Message}", "Ошибка");
            }
        }

        private void LoadRecipeImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    Console.WriteLine($"Загружаем изображение по пути: {imagePath}");

                    // Преобразуем относительный путь в абсолютный
                    string fullPath = GetFullImagePath(imagePath);
                    Console.WriteLine($"Полный путь: {fullPath}");
                    Console.WriteLine($"Файл существует: {File.Exists(fullPath)}");

                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine("Файл найден! Загружаем...");
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(fullPath);
                        bitmap.EndInit();

                        RecipeImage.Source = bitmap;
                        Console.WriteLine("Изображение успешно загружено!");
                        return;
                    }
                    else
                    {
                        MessageBox.Show($"Изображение не найдено по пути:\n{fullPath}", "Ошибка");
                    }
                }
                else
                {
                    Console.WriteLine("Путь к изображению пустой или null");
                }

                // Если фото нет или не найдено - просто оставляем пустым
                RecipeImage.Source = null;
            }
            catch (Exception ex)
            {
                RecipeImage.Source = null;
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}\n\nПуть: {imagePath}", "Ошибка");
            }
        }

        private string GetFullImagePath(string relativePath)
        {
            try
            {
                // Очищаем путь от лишних символов
                relativePath = relativePath.Trim();
                Console.WriteLine($"Очищенный путь: '{relativePath}'");

                // Если путь уже абсолютный
                if (Path.IsPathRooted(relativePath))
                {
                    Console.WriteLine("Путь абсолютный");
                    return relativePath;
                }

                // Базовая директория приложения
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine($"BaseDirectory: {baseDir}");

                // Пробуем разные варианты
                List<string> possiblePaths = new List<string>();

                // 1. Корень проекта (поднимаемся на 3 уровня из bin/Debug/netX.0)
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(baseDir);
                    string projectRoot = dir.Parent?.Parent?.Parent?.FullName;
                    if (!string.IsNullOrEmpty(projectRoot))
                    {
                        possiblePaths.Add(Path.Combine(projectRoot, relativePath.Replace("/", "\\")));
                        Console.WriteLine($"Путь 1 (проект): {possiblePaths.Last()}");
                    }
                }
                catch { }

                // 2. Из папки с исполняемым файлом
                possiblePaths.Add(Path.Combine(baseDir, relativePath.Replace("/", "\\")));
                Console.WriteLine($"Путь 2 (base): {possiblePaths.Last()}");

                // 3. Если Images/Recipes/ в начале пути
                if (relativePath.StartsWith("Images/Recipes/") || relativePath.StartsWith("Images\\Recipes\\"))
                {
                    // Убираем Images/Recipes/ из начала
                    string fileName = Path.GetFileName(relativePath);
                    DirectoryInfo dir = new DirectoryInfo(baseDir);
                    string projectRoot = dir.Parent?.Parent?.Parent?.FullName;
                    if (!string.IsNullOrEmpty(projectRoot))
                    {
                        string path = Path.Combine(projectRoot, "Images", "Recipes", fileName);
                        possiblePaths.Add(path);
                        Console.WriteLine($"Путь 3 (Images/Recipes): {path}");
                    }
                }

                // 4. В папке Images рядом с исполняемым файлом
                possiblePaths.Add(Path.Combine(baseDir, "Images", "Recipes", Path.GetFileName(relativePath)));
                Console.WriteLine($"Путь 4 (base/Images): {possiblePaths.Last()}");

                // Ищем первый существующий файл
                foreach (var path in possiblePaths)
                {
                    try
                    {
                        string fullPath = Path.GetFullPath(path);
                        if (File.Exists(fullPath))
                        {
                            Console.WriteLine($"Найден файл: {fullPath}");
                            return fullPath;
                        }
                    }
                    catch { }
                }

                // Если не нашли, возвращаем первый вариант
                return possiblePaths.FirstOrDefault() ?? relativePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetFullImagePath: {ex.Message}");
                return relativePath;
            }
        }

        private void LoadIngredientsFromDatabase()
        {
            try
            {
                // Если ингредиенты уже загружены
                if (_recipe.СоставБлюдаs != null && _recipe.СоставБлюдаs.Any())
                {
                    DisplayIngredients();
                    return;
                }

                // Загружаем ингредиенты из БД
                using (var db = new BdCourseContext())
                {
                    var ingredients = db.СоставБлюдаs
                        .Include(s => s.FkИнгредиентаNavigation)
                            .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)
                        .Where(s => s.FkРецепта == _recipe.IdРецепта)
                        .ToList();

                    if (ingredients.Any())
                    {
                        var ingredientsData = ingredients
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ингредиентов: {ex.Message}", "Ошибка");
                IngredientsList.ItemsSource = new List<RecipeIngredientDisplay>
                {
                    new RecipeIngredientDisplay { Name = "Ошибка загрузки ингредиентов", Amount = "" }
                };
            }
        }

        private void DisplayIngredients()
        {
            var ingredientsData = _recipe.СоставБлюдаs
                .Select(i => new RecipeIngredientDisplay
                {
                    Name = i.FkИнгредиентаNavigation?.Название ?? "Неизвестно",
                    Amount = FormatIngredientAmount(i.Количество, i.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? "г")
                })
                .ToList();

            IngredientsList.ItemsSource = ingredientsData;
        }

        private void LoadStepsFromDatabase()
        {
            try
            {
                // Если шаги уже загружены
                if (_recipe.ШагиПриготовленияs != null && _recipe.ШагиПриготовленияs.Any())
                {
                    DisplaySteps();
                    return;
                }

                // Загружаем шаги из БД
                using (var db = new BdCourseContext())
                {
                    var steps = db.ШагиПриготовленияs
                        .Where(s => s.FkРецепта == _recipe.IdРецепта)
                        .OrderBy(s => s.НомерШага)
                        .ToList();

                    if (steps.Any())
                    {
                        var stepsList = steps
                            .Select(s => $"Шаг {s.НомерШага}: {s.Описание}")
                            .ToList();
                        StepsList.ItemsSource = stepsList;
                    }
                    else
                    {
                        StepsList.ItemsSource = new List<string> { "Нет информации о шагах приготовления" };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки шагов: {ex.Message}", "Ошибка");
                StepsList.ItemsSource = new List<string> { "Ошибка загрузки шагов приготовления" };
            }
        }

        private void DisplaySteps()
        {
            var steps = _recipe.ШагиПриготовленияs
                .OrderBy(s => s.НомерШага)
                .Select(s => $"Шаг {s.НомерШага}: {s.Описание}")
                .ToList();
            StepsList.ItemsSource = steps;
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
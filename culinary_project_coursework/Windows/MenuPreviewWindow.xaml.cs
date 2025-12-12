using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace culinary_project_coursework.Windows
{
    public partial class MenuPreviewWindow : Window
    {
        private object _menuData;
        private Dictionary<string, Рецепты> _recipeCache = new Dictionary<string, Рецепты>();

        public MenuPreviewWindow(object menuData)
        {
            InitializeComponent();
            _menuData = menuData;
            LoadRecipes();
            DisplayMenu();
        }

        private void LoadRecipes()
        {
            try
            {
                var type = _menuData.GetType();
                var selectedRecipesProp = type.GetProperty("SelectedRecipes");

                if (selectedRecipesProp != null)
                {
                    var selectedRecipes = selectedRecipesProp.GetValue(_menuData) as Dictionary<string, int?>;

                    if (selectedRecipes != null)
                    {
                        using (var context = new BdCourseContext())
                        {
                            var allRecipes = context.Рецептыs.ToList();

                            foreach (var recipeEntry in selectedRecipes)
                            {
                                if (recipeEntry.Value.HasValue)
                                {
                                    var recipe = allRecipes.FirstOrDefault(r => r.IdРецепта == recipeEntry.Value.Value);
                                    if (recipe != null)
                                    {
                                        _recipeCache[recipeEntry.Key] = recipe;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}", "Ошибка");
            }
        }

        private void DisplayMenu()
        {
            try
            {
                var type = _menuData.GetType();
                var menuName = type.GetProperty("MenuName")?.GetValue(_menuData) as string;
                var daysCount = (int?)type.GetProperty("DaysCount")?.GetValue(_menuData) ?? 1;
                var peopleCount = (int?)type.GetProperty("PeopleCount")?.GetValue(_menuData) ?? 1;
                var recipeNames = type.GetProperty("RecipeNames")?.GetValue(_menuData) as Dictionary<string, string>;

                this.Title = $"Просмотр меню: {menuName}";

                var daysList = new List<object>();

                for (int day = 1; day <= daysCount; day++)
                {
                    var dayData = new
                    {
                        DayTitle = $"День {day}",
                        DayNumber = day,
                        People = new List<object>()
                    };

                    for (int person = 1; person <= peopleCount; person++)
                    {
                        var personId = $"{day}-{person}";

                        //  названия рецептов
                        string breakfast = "Не выбрано";
                        string lunch = "Не выбрано";
                        string dinner = "Не выбрано";

                        //  объекты рецептов
                        Рецепты breakfastRecipe = null;
                        Рецепты lunchRecipe = null;
                        Рецепты dinnerRecipe = null;

                        var breakfastKey = $"{personId}-breakfast";
                        var lunchKey = $"{personId}-lunch";
                        var dinnerKey = $"{personId}-dinner";

                        if (recipeNames != null)
                        {
                            recipeNames.TryGetValue(breakfastKey, out breakfast);
                            recipeNames.TryGetValue(lunchKey, out lunch);
                            recipeNames.TryGetValue(dinnerKey, out dinner);
                        }

                        // получаем рецепты -
                        _recipeCache.TryGetValue(breakfastKey, out breakfastRecipe);
                        _recipeCache.TryGetValue(lunchKey, out lunchRecipe);
                        _recipeCache.TryGetValue(dinnerKey, out dinnerRecipe);

                        var personData = new
                        {
                            PersonId = personId,
                            PersonTitle = $"Человек {person}",
                            Breakfast = breakfast ?? "Не выбрано",
                            Lunch = lunch ?? "Не выбрано",
                            Dinner = dinner ?? "Не выбрано",
                            BreakfastRecipe = breakfastRecipe,
                            LunchRecipe = lunchRecipe,
                            DinnerRecipe = dinnerRecipe
                        };

                        dayData.People.Add(personData);
                    }

                    daysList.Add(dayData);
                }

                DaysItemsControl.ItemsSource = daysList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения меню: {ex.Message}", "Ошибка");
            }
        }

        private void CreateShoppingListButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var shoppingList = GenerateShoppingList();

                if (shoppingList.Count == 0)
                {
                    MessageBox.Show("Нет выбранных рецептов для создания списка покупок",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var shoppingListWindow = new ShoppingListWindow(shoppingList);
                shoppingListWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания списка покупок: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Dictionary<string, decimal> GenerateShoppingList()
        {
            var shoppingList = new Dictionary<string, decimal>();

            try
            {
                var type = _menuData.GetType();
                var selectedRecipesProp = type.GetProperty("SelectedRecipes");

                if (selectedRecipesProp == null)
                {
                    MessageBox.Show("Нет данных о выбранных рецептах", "Информация");
                    return shoppingList;
                }

                var selectedRecipes = selectedRecipesProp.GetValue(_menuData) as Dictionary<string, int?>;

                if (selectedRecipes == null || selectedRecipes.Count == 0)
                {
                    MessageBox.Show("Нет выбранных рецептов", "Информация");
                    return shoppingList;
                }

                using (var context = new BdCourseContext()) 
                {
                    var recipeIds = new List<int>();
                    foreach (var recipeId in selectedRecipes.Values)
                    {
                        if (recipeId.HasValue && recipeId > 0)
                        {
                            recipeIds.Add(recipeId.Value);
                        }
                    }

                    if (recipeIds.Count == 0)
                        return shoppingList;

                    
                    var allIngredients = context.СоставБлюдаs
                        .Include(s => s.FkИнгредиентаNavigation)
                            .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)
                        .Where(s => recipeIds.Contains(s.FkРецепта))
                        .ToList();


                    foreach (var ingredient in allIngredients)
                    {
                        string ingredientName = ingredient.FkИнгредиентаNavigation?.Название ?? "Неизвестно";
                        decimal quantity = ingredient.Количество;

                        string unit = ingredient.FkИнгредиентаNavigation?.FkЕдиницыИзмеренияNavigation?.Название ?? "г";

                        string key = $"{ingredientName} ({unit})";

                        if (shoppingList.ContainsKey(key))
                        {
                            shoppingList[key] += quantity;
                        }
                        else
                        {
                            shoppingList[key] = quantity;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации списка: {ex.Message}", "Ошибка");
            }

            return shoppingList;
        }
       

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var type = _menuData.GetType();
                var daysCount = (int?)type.GetProperty("DaysCount")?.GetValue(_menuData) ?? 1;
                var peopleCount = (int?)type.GetProperty("PeopleCount")?.GetValue(_menuData) ?? 1;

                CreateMenuWindow createMenuWindow = new CreateMenuWindow(daysCount, peopleCount);
                createMenuWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }


        private void ViewRecipeDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is Рецепты recipe && recipe != null)
            {
                var detailsWindow = new RecipeDetailsWindow(recipe);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Рецепт не выбран", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
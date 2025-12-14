using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Classes;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    public partial class CreateMenuWindow : Window
    {
        private MenuPlan _menuPlan;
        private Dictionary<string, string> _recipeNames = new Dictionary<string, string>();

        public CreateMenuWindow(int daysCount, int peopleCount)
        {
            InitializeComponent();
            InitializeMenuPlan(daysCount, peopleCount);
        }

        private void InitializeMenuPlan(int daysCount, int peopleCount)
        {
            _menuPlan = new MenuPlan
            {
                DaysCount = daysCount,
                PeopleCount = peopleCount,
                MenuName = $"Меню на {daysCount} дней для {peopleCount} человек"
            };

            TitleTextBlock.Text = _menuPlan.MenuName;

            for (int day = 1; day <= daysCount; day++)
            {
                var dayModel = new MenuDay
                {
                    DayNumber = day,
                    DayTitle = $"День {day}"
                };

                for (int person = 1; person <= peopleCount; person++)
                {
                    dayModel.People.Add(new MenuPerson
                    {
                        PersonId = $"{day}-{person}",
                        PersonTitle = $"Человек {person}"
                    });
                }

                _menuPlan.Days.Add(dayModel);
            }

            DaysItemsControl.ItemsSource = _menuPlan.Days;
        }

        private void SelectBreakfast_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var personId = button.Tag.ToString();
            SelectRecipeForMeal(personId, "breakfast", button);
        }

        private void SelectLunch_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var personId = button.Tag.ToString();
            SelectRecipeForMeal(personId, "lunch", button);
        }

        private void SelectDinner_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var personId = button.Tag.ToString();
            SelectRecipeForMeal(personId, "dinner", button);
        }

        private void SelectRecipeForMeal(string personId, string mealType, Button button)
        {
            try
            {
               
                var recipeSelectionWindow = new RecipeSelectionWindow
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var result = recipeSelectionWindow.ShowDialog();

                if (result == true && recipeSelectionWindow.SelectedRecipe != null)
                {
                    var selectedRecipe = recipeSelectionWindow.SelectedRecipe;
                    UpdateButtonWithRecipe(button, selectedRecipe.Название);

                    // Сохраняем в словарь
                    var key = $"{personId}-{mealType}";
                    _recipeNames[key] = selectedRecipe.Название;

                    UpdateMenuPlanWithRecipe(personId, mealType, selectedRecipe.Название);

                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора рецепта: {ex.Message}", "Ошибка");
            }
        }
        private void UpdateButtonWithRecipe(Button button, string recipeName)
        {
            // Обновляем содержимое кнопки
            button.Content = recipeName;
            button.FontWeight = FontWeights.Normal;
            button.FontStyle = FontStyles.Italic;
            button.Foreground = System.Windows.Media.Brushes.DarkGreen;
            button.ToolTip = $"Выбрано: {recipeName}";
        }

        private void UpdateMenuPlanWithRecipe(string personId, string mealType, string recipeName)
        {
            try
            {
                var parts = personId.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int day) && int.TryParse(parts[1], out int person))
                {
                    var dayModel = _menuPlan.Days[day - 1];
                    var personModel = dayModel.People[person - 1];

                    switch (mealType.ToLower())
                    {
                        case "breakfast":
                            personModel.Breakfast.DishName = recipeName;
                            break;
                        case "lunch":
                            personModel.Lunch.DishName = recipeName;
                            break;
                        case "dinner":
                            personModel.Dinner.DishName = recipeName;
                            break;
                    }

                    DaysItemsControl.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления меню: {ex.Message}", "Ошибка");
            }
        }

       

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем ID рецептов
                var selectedRecipeIds = new Dictionary<string, int?>();
                var recipeNames = new Dictionary<string, string>();

                using (var context = new BdCourseContext())
                {
                    foreach (var entry in _recipeNames)
                    {
                        recipeNames[entry.Key] = entry.Value;

                        var recipe = context.Рецептыs
                            .FirstOrDefault(r => r.Название == entry.Value);

                        if (recipe != null)
                        {
                            selectedRecipeIds[entry.Key] = recipe.IdРецепта;

                            
                        }
                        else
                        {
                            selectedRecipeIds[entry.Key] = null;
                            MessageBox.Show($"Рецепт '{entry.Value}' не найден в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }

                // Создаем объект с данными для передачи
                var menuData = new
                {
                    MenuName = _menuPlan.MenuName,
                    DaysCount = _menuPlan.DaysCount,
                    PeopleCount = _menuPlan.PeopleCount,
                    RecipeNames = recipeNames,
                    SelectedRecipes = selectedRecipeIds  
                };

                var menuPreviewWindow = new MenuPreviewWindow(menuData);
                menuPreviewWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения меню: {ex.Message}", "Ошибка");
            }
        }

     

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menuWindow = new Menu();
            menuWindow.Show();
            this.Close();
        }

       
    }
}
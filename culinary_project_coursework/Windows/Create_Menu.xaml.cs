using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    public partial class CreateMenuWindow : Window
    {
        private MenuPlan _menuPlan;

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

            // Создаем структуру дней и людей
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
            SelectRecipeForMeal(personId, "breakfast");
        }

        private void SelectLunch_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var personId = button.Tag.ToString();
            SelectRecipeForMeal(personId, "lunch");
        }

        private void SelectDinner_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var personId = button.Tag.ToString();
            SelectRecipeForMeal(personId, "dinner");
        }

        private void SelectRecipeForMeal(string personId, string mealType)
        {
            // Временная реализация - простое сообщение
            MessageBox.Show($"Выбор блюда для {mealType}\nPersonId: {personId}", "Выбор рецепта");

            // Для демонстрации тестовое блюдо
            UpdatePersonMeal(personId, mealType, "Выбранное блюдо");
        }

        private void UpdatePersonMeal(string personId, string mealType, string dishName)
        {
            var parts = personId.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[0], out int day) && int.TryParse(parts[1], out int person))
            {
                var dayModel = _menuPlan.Days[day - 1];
                var personModel = dayModel.People[person - 1];

                switch (mealType.ToLower())
                {
                    case "breakfast":
                        personModel.Breakfast.DishName = dishName;
                        break;
                    case "lunch":
                        personModel.Lunch.DishName = dishName;
                        break;
                    case "dinner":
                        personModel.Dinner.DishName = dishName;
                        break;
                }

                // Обновляем отображение
                DaysItemsControl.Items.Refresh();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveMenuToDatabase();
                MessageBox.Show("Меню успешно сохранено!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении меню: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var menuPreviewWindow = new MenuPreviewWindow(_menuPlan);
            menuPreviewWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menuWindow = new Menu();
            menuWindow.Show();
            this.Close();
        }

        private void SaveMenuToDatabase()
        {
            // TODO: Реализовать сохранение в базу данных
            Console.WriteLine("Сохранение меню в базу данных...");
        }
       
       
        
    }
}
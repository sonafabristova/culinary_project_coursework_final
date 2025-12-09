using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel; // ДОБАВИТЬ
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    public partial class My_recipes : Window
    {
        // ИЗМЕНИТЬ: Использовать ObservableCollection
        public ObservableCollection<Рецепты> UserRecipes { get; set; }

        public My_recipes()
        {
            InitializeComponent();

            // ИНИЦИАЛИЗИРОВАТЬ коллекцию
            UserRecipes = new ObservableCollection<Рецепты>();

            // Установить DataContext
            DataContext = this;

            // Загрузить рецепты
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                // Получаем рецепты текущего пользователя ИЗ БАЗЫ ДАННЫХ
                if (AppContext.CurrentUser != null)
                {
                    var recipes = AppContext.GetUserRecipes(AppContext.CurrentUser.IdПользователя);

                    // Очистить и заполнить коллекцию
                    UserRecipes.Clear();
                    foreach (var recipe in recipes)
                    {
                        UserRecipes.Add(recipe);
                    }

                    Console.WriteLine($"Загружено {UserRecipes.Count} пользовательских рецептов");

                    if (UserRecipes.Count == 0)
                    {
                        MessageBox.Show("У вас пока нет сохраненных рецептов");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не авторизован");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}");
                UserRecipes.Clear();
            }
        }

        private void Recipes_Change(object sender, SelectionChangedEventArgs e)
        {
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);
                BoxRecipes.SelectedItem = null;
            }
        }

        private void ShowRecipeDetails(Рецепты recipe)
        {
            RecipeDetailsWindow detailsWindow = new RecipeDetailsWindow(recipe);
            detailsWindow.ShowDialog();
        }

        private void AddMyRecipesClick(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddRecipeWindow
            {
                Owner = this
            };

            var result = addWindow.ShowDialog();

            if (result == true && addWindow.NewRecipe != null)
            {
                try
                {
                    // Устанавливаем автора рецепта
                    addWindow.NewRecipe.CreatedByUserId = AppContext.CurrentUser?.IdПользователя;
                    addWindow.NewRecipe.IsSystemRecipe = false;

                    // Добавляем рецепт в базу данных
                    AppContext.AddRecipe(addWindow.NewRecipe);

                    // Обновляем список
                    if (AppContext.CurrentUser != null)
                    {
                        // Перезагружаем рецепты из БД
                        var recipes = AppContext.GetUserRecipes(AppContext.CurrentUser.IdПользователя);
                        UserRecipes.Clear();
                        foreach (var recipe in recipes)
                        {
                            UserRecipes.Add(recipe);
                        }
                    }

                    MessageBox.Show("Рецепт успешно добавлен!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления рецепта: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonDeleteRecipe(object sender, RoutedEventArgs e)
        {
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить рецепт '{selectedRecipe.Название}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Удаляем рецепт из базы данных
                        AppContext.DeleteRecipe(selectedRecipe.IdРецепта);

                        // Удаляем из коллекции
                        UserRecipes.Remove(selectedRecipe);

                        MessageBox.Show("Рецепт успешно удален!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления рецепта: {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт для удаления.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonMenuClick(object sender, RoutedEventArgs e)
        {
            Menu menuWindow = new Menu();
            menuWindow.Show();
            this.Close();
        }

        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
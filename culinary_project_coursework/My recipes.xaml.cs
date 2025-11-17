using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace culinary_project_coursework
{
    public partial class My_recipes : Window
    {
        public List<Recipe> UserRecipes { get; set; }

        public My_recipes()
        {
            InitializeComponent();

            // Инициализируем список рецептов как в лабораторной
            UserRecipes = AppContext.Recipes.Where(r => !r.IsSystemRecipe).ToList();

            DataContext = this;
        }

        private void Recipes_Change(object sender, SelectionChangedEventArgs e)
        {
            // Обработчик выбора рецепта
            if (BoxRecipes.SelectedItem is Recipe selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);

                // Сбрасываем выделение, чтобы можно было кликнуть на тот же рецепт снова
                BoxRecipes.SelectedItem = null;
            }
        }

        private void ShowRecipeDetails(Recipe recipe)
        {
            // Открываем новое окно с деталями рецепта
            var detailsWindow = new RecipeDetailsWindow(recipe)
            {
                Owner = this // Устанавливаем текущее окно как владельца
            };
            detailsWindow.ShowDialog();
        }

        private void AddMyRecipesClick(object sender, RoutedEventArgs e)
        {
            // Открываем окно добавления рецепта
            var addWindow = new AddRecipeWindow
            {
                Owner = this
            };

            var result = addWindow.ShowDialog();

            if (result == true && addWindow.NewRecipe != null)
            {
                // Добавляем рецепт в общий список
                AppContext.Recipes.Add(addWindow.NewRecipe);

                // Обновляем список пользовательских рецептов
                UserRecipes = AppContext.Recipes.Where(r => !r.IsSystemRecipe).ToList();

                // Обновляем отображение ListBox
                BoxRecipes.Items.Refresh();

                MessageBox.Show("Рецепт успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonDeleteRecipe(object sender, RoutedEventArgs e)
        {
            // Аналогично ButtonDeleteCar в лабораторной
            if (BoxRecipes.SelectedItem is Recipe selectedRecipe)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить рецепт '{selectedRecipe.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    AppContext.Recipes.Remove(selectedRecipe);
                    UserRecipes = AppContext.Recipes.Where(r => !r.IsSystemRecipe).ToList();
                    BoxRecipes.Items.Refresh();

                    MessageBox.Show("Рецепт успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
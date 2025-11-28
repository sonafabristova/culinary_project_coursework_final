using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    public partial class My_recipes : Window
    {
        public List<Recipe> UserRecipes { get; set; }

        public My_recipes()
        {
            InitializeComponent();

            UserRecipes = AppContext.GetUserRecipes(AppContext.CurrentUser.Id);

            DataContext = this;
        }

        private void Recipes_Change(object sender, SelectionChangedEventArgs e)
        {
            // Обработчик выбора рецепта
            if (BoxRecipes.SelectedItem is Recipe selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);

                BoxRecipes.SelectedItem = null;
            }
        }

        private void ShowRecipeDetails(Recipe recipe)
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
                AppContext.Recipes.Add(addWindow.NewRecipe);

                UserRecipes = AppContext.GetUserRecipes(AppContext.CurrentUser.Id);

                BoxRecipes.Items.Refresh();

                MessageBox.Show("Рецепт успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonDeleteRecipe(object sender, RoutedEventArgs e)
        {
          
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
                    UserRecipes = AppContext.GetUserRecipes(AppContext.CurrentUser.Id);
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
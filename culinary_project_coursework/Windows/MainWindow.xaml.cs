using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows 
{
    public partial class MainWindow : Window
    {
        public List<Recipe> SystemRecipes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Список системных рецептов
            SystemRecipes = AppContext.Recipes.Where(r => r.IsSystemRecipe).ToList();

            DataContext = this;
        }

        public MainWindow(User user) : this()
        {
            // Дополнительная логика для пользователя, если нужно
        }

        private void Recipes_Change(object sender, SelectionChangedEventArgs e)
        {
            // Обработчик выбора рецепта - срабатывает при клике на рецепт
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
            };
            detailsWindow.ShowDialog();
          
        }

        private void ButtonMyRecipesClick(object sender, RoutedEventArgs e)
        {
            My_recipes myRecipesWindow = new My_recipes();
            myRecipesWindow.Show();
            this.Close();
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
    }
}
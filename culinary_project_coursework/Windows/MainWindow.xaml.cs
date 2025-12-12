using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    public partial class MainWindow : Window
    {
       
        public ObservableCollection<Рецепты> SystemRecipes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SystemRecipes = new ObservableCollection<Рецепты>();

            DataContext = this;

            LoadRecipes();
        }

        public MainWindow(Пользователи user) : this()
        {
            AppContext.CurrentUser = user;
        }

        private void LoadRecipes()
        {
            try
            {
                SystemRecipes.Clear();

                // Всегда свежие данные
                var recipes = AppContext.GetSystemRecipes();

                foreach (var recipe in recipes)
                {
                    SystemRecipes.Add(recipe);
                }

                if (SystemRecipes.Count == 0)
                {
                    MessageBox.Show("В базе данных нет системных рецептов.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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
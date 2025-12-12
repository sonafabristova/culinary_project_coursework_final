using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace culinary_project_coursework.Windows
{
    public partial class RecipeSelectionWindow : Window
    {
        public Рецепты SelectedRecipe { get; private set; }

        public RecipeSelectionWindow()
        {
            InitializeComponent();
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    var recipes = db.Рецептыs
                        .Include(r => r.CreatedByUser)
                        .Include(r => r.СоставБлюдаs)
                            .ThenInclude(s => s.FkИнгредиентаNavigation)
                        .Include(r => r.ШагиПриготовленияs)
                        .ToList();

                    var filteredRecipes = new List<Рецепты>();

                    foreach (var recipe in recipes)
                    {
                       
                        if (recipe.IsSystemRecipe == true)
                        {
                            filteredRecipes.Add(recipe);
                        }
                       
                        else if (AppContext.CurrentUser != null &&
                                 recipe.CreatedByUserId == AppContext.CurrentUser.IdПользователя)
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }

                   
                    RecipesListBox.ItemsSource = filteredRecipes;

                   

                    if (filteredRecipes.Count == 0)
                    {
                        MessageBox.Show("Нет доступных рецептов для выбора. Создайте свой рецепт или дождитесь добавления системных рецептов.",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RecipesListBox.ItemsSource = new List<Рецепты>();
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Рецепты selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void RecipesListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Рецепты selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
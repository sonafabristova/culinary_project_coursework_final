using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    public partial class RecipeSelectionWindow : Window
    {
        public Recipe SelectedRecipe { get; private set; }

        public RecipeSelectionWindow()
        {
            InitializeComponent();
            LoadRecipes();
        }

        private void LoadRecipes()
        {
            //  рецепты из AppContext
            var recipes = AppContext.Recipes;
            RecipesListBox.ItemsSource = recipes;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Recipe selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void RecipesListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Recipe selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore; // ДОБАВИТЬ

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
                using (var db = new WithIngContext())
                {
                    var recipes = db.Рецептыs
                .Include(r => r.CreatedByUser)
                .Include(r => r.СоставБлюдаs)
                    .ThenInclude(s => s.FkИнгредиентаNavigation)
                .Include(r => r.ШагиПриготовленияs)
                .ToList(); // Сначала загружаем все

                    // Фильтруем в памяти
                    var filteredRecipes = new List<Рецепты>();

                    foreach (var recipe in recipes)
                    {
                        if (recipe.IsSystemRecipe == true)
                        {
                            // Всегда показываем системные рецепты
                            filteredRecipes.Add(recipe);
                        }
                        else if (AppContext.CurrentUser != null &&
                                 recipe.CreatedByUserId == AppContext.CurrentUser.IdПользователя)
                        {
                            // Показываем пользовательские рецепты только их владельцу
                            filteredRecipes.Add(recipe);
                        }
                    }

                    RecipesListBox.ItemsSource = filteredRecipes;

                    Console.WriteLine($"Загружено {filteredRecipes.Count} рецептов для выбора");

                    if (filteredRecipes.Count == 0)
                    {
                        MessageBox.Show("Нет доступных рецептов для выбора");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}");
                RecipesListBox.ItemsSource = new List<Рецепты>();
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Рецепты selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;

                // Проверяем, что зависимости загружены
                Console.WriteLine($"Выбран рецепт: {selectedRecipe.Название}");
                Console.WriteLine($"Ингредиентов: {selectedRecipe.СоставБлюдаs?.Count ?? 0}");
                Console.WriteLine($"Шагов: {selectedRecipe.ШагиПриготовленияs?.Count ?? 0}");

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
            if (RecipesListBox.SelectedItem is Рецепты selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}

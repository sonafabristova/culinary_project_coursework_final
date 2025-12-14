using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace culinary_project_coursework.Windows
{
    public partial class My_recipes : Window
    {
        public ObservableCollection<Рецепты> UserRecipes { get; set; }
        private Рецепты _selectedForDeletion;

        public My_recipes()
        {
            InitializeComponent();

            UserRecipes = new ObservableCollection<Рецепты>();
            DataContext = this;

            LoadRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                if (AppContext.CurrentUser != null)
                {
                    var recipes = AppContext.GetUserRecipes(AppContext.CurrentUser.IdПользователя);

                    UserRecipes.Clear();
                    foreach (var recipe in recipes)
                    {
                        UserRecipes.Add(recipe);
                    }

           

                    if (UserRecipes.Count == 0)
                    {
                        MessageBox.Show("У вас пока нет сохраненных рецептов");
                    }
                }
                else
                {
                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}");
                UserRecipes.Clear();
            }
        }

        // ОДИН КЛИК - просмотр деталей
        private void BoxRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe && e.AddedItems.Count > 0)
            {
                ShowRecipeDetails(selectedRecipe);
                // Не сбрасываем выделение сразу, чтобы пользователь видел какой рецепт выбран
            }
        }

        // ДВА КЛИКА - выбор для удаления
        private void BoxRecipes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                _selectedForDeletion = selectedRecipe;

               
            }
        }

        private void ShowRecipeDetails(Рецепты recipe)
        {
            try
            {
                var fullRecipe = LoadFullRecipe(recipe.IdРецепта);

                if (fullRecipe != null)
                {
                    var detailsWindow = new RecipeDetailsWindow(fullRecipe);
                    detailsWindow.Owner = this;
                    detailsWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить данные рецепта", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private Рецепты LoadFullRecipe(int recipeId)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    return db.Рецептыs
                        .Include(r => r.СоставБлюдаs)
                            .ThenInclude(s => s.FkИнгредиентаNavigation)
                                .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)
                        .Include(r => r.ШагиПриготовленияs)
                        .FirstOrDefault(r => r.IdРецепта == recipeId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецепта: {ex.Message}", "Ошибка");
                return null;
            }
        }

        private void ButtonDeleteRecipe(object sender, RoutedEventArgs e)
        {
            // Используем рецепт, выбранный двойным кликом, или текущий выделенный
            var recipeToDelete = _selectedForDeletion ?? BoxRecipes.SelectedItem as Рецепты;

            if (recipeToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить рецепт '{recipeToDelete.Название}'?\n\n" +
                    "Это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppContext.DeleteRecipe(recipeToDelete.IdРецепта);

                        UserRecipes.Remove(recipeToDelete);

                        
                        _selectedForDeletion = null;
                        BoxRecipes.SelectedItem = null;

                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления рецепта: {ex.Message}\n\n" +
                            "Возможно, рецепт используется в других записях.",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
               
            }
        }

        private void BoxRecipes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);
            }
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
                    addWindow.NewRecipe.CreatedByUserId = AppContext.CurrentUser?.IdПользователя;
                    addWindow.NewRecipe.IsSystemRecipe = false;

                    AppContext.AddRecipe(addWindow.NewRecipe);

                    if (AppContext.CurrentUser != null)
                    {
                        var recipes = AppContext.GetUserRecipes(AppContext.CurrentUser.IdПользователя);
                        UserRecipes.Clear();
                        foreach (var recipe in recipes)
                        {
                            UserRecipes.Add(recipe);
                        }
                    }

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления рецепта: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
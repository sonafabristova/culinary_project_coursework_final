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

      
        private void BoxRecipes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);
                BoxRecipes.SelectedItem = null; 
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
            if (BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить рецепт '{selectedRecipe.Название}'?\n\n" +
                    "Это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppContext.DeleteRecipe(selectedRecipe.IdРецепта);

                        UserRecipes.Remove(selectedRecipe);

                        MessageBox.Show("Рецепт успешно удален!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Пожалуйста, выберите рецепт для удаления.\n\n" +
                    "Кликните по рецепту в списке, а затем нажмите кнопку 'Удалить рецепт'.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

       
        private void BoxRecipes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && BoxRecipes.SelectedItem is Рецепты selectedRecipe)
            {
                ShowRecipeDetails(selectedRecipe);
                BoxRecipes.SelectedItem = null;
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
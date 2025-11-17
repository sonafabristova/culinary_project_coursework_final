using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace culinary_project_coursework
{
    public partial class My_recipes : Window
    {
        public My_recipes()
        {
            InitializeComponent();
            LoadMyRecipes();
        }

        private void LoadMyRecipes()
        {
            // Очищаем контейнер для рецептов
            MyRecipesContainer.Children.Clear();

            // Получаем рецепты текущего пользователя (если авторизован)
            var userRecipes = AppContext.Recipes.Where(r => !r.IsSystemRecipe).ToList();

            if (userRecipes.Any())
            {
                // Добавляем каждый пользовательский рецепт
                foreach (var recipe in userRecipes)
                {
                    var recipeBorder = CreateRecipeBorder(recipe);
                    MyRecipesContainer.Children.Add(recipeBorder);
                }
            }
            else
            {
                // Сообщение, если рецептов нет
                var noRecipesText = new TextBlock()
                {
                    Text = "У вас пока нет собственных рецептов.\nНажмите 'Добавить рецепт', чтобы создать первый!",
                    FontSize = 16,
                    FontStyle = FontStyles.Italic,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 50, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                MyRecipesContainer.Children.Add(noRecipesText);
            }
        }

        private Border CreateRecipeBorder(Recipe recipe)
        {
            var border = new Border()
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 15),
                Padding = new Thickness(15),
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(5)
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // Название рецепта
            var nameTextBlock = new TextBlock()
            {
                Text = recipe.Name,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = System.Windows.Media.Brushes.DarkBlue
            };
            Grid.SetRow(nameTextBlock, 0);

            // Описание
            var descriptionTextBlock = new TextBlock()
            {
                Text = recipe.Description,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 10),
                TextWrapping = TextWrapping.Wrap
            };
            Grid.SetRow(descriptionTextBlock, 1);

            // БЖУ и калории
            var nutritionStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 10)
            };

            nutritionStackPanel.Children.Add(new TextBlock()
            {
                Text = $"Б: {recipe.Proteins}г ",
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 10, 0),
                Foreground = System.Windows.Media.Brushes.DarkGreen
            });

            nutritionStackPanel.Children.Add(new TextBlock()
            {
                Text = $"Ж: {recipe.Fats}г ",
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 10, 0),
                Foreground = System.Windows.Media.Brushes.DarkOrange
            });
            nutritionStackPanel.Children.Add(new TextBlock()
            {
                Text = $"У: {recipe.Carbohydrates}г ",
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 10, 0),
                Foreground = System.Windows.Media.Brushes.DarkRed
            });

            nutritionStackPanel.Children.Add(new TextBlock()
            {
                Text = $"Калории: {recipe.Calories} ккал",
                FontWeight = FontWeights.SemiBold,
                Foreground = System.Windows.Media.Brushes.DarkRed
            });

            Grid.SetRow(nutritionStackPanel, 2);

            // Время приготовления и ингредиенты
            var detailsStackPanel = new StackPanel();

            // Время приготовления
            detailsStackPanel.Children.Add(new TextBlock()
            {
                Text = $" Время приготовления: {recipe.CookingTime} минут",
                Margin = new Thickness(0, 0, 0, 5),
                Foreground = System.Windows.Media.Brushes.DarkBlue
            });

            // Ингредиенты (первые 3)
            if (recipe.Ingredients.Any())
            {
                var ingredientsText = new TextBlock()
                {
                    Text = "Ингредиенты: " + string.Join(", ", recipe.Ingredients.Take(3).Select(i => $"{i.Name} {i.Amount}{i.Unit}")),
                    Margin = new Thickness(0, 0, 0, 5),
                    TextWrapping = TextWrapping.Wrap
                };
                detailsStackPanel.Children.Add(ingredientsText);

                if (recipe.Ingredients.Count > 3)
                {
                    ingredientsText.Text += "...";
                }
            }

            // Индикатор пользовательского рецепта
            var userRecipeIndicator = new TextBlock()
            {
                Text = "⭐️ Ваш рецепт",
                FontWeight = FontWeights.SemiBold,
                Foreground = System.Windows.Media.Brushes.Goldenrod,
                Margin = new Thickness(0, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            detailsStackPanel.Children.Add(userRecipeIndicator);

            Grid.SetRow(detailsStackPanel, 3);

            // Добавляем все элементы в grid
            grid.Children.Add(nameTextBlock);
            grid.Children.Add(descriptionTextBlock);
            grid.Children.Add(nutritionStackPanel);
            grid.Children.Add(detailsStackPanel);

            border.Child = grid;

            // Добавляем обработчик клика для просмотра деталей рецепта
            border.MouseLeftButtonDown += (s, e) =>
            {
                ShowRecipeDetails(recipe);
            };

            // Меняем курсор при наведении
            border.Cursor = System.Windows.Input.Cursors.Hand;

            return border;
        }

        private void ShowRecipeDetails(Recipe recipe)
        {
            MessageBox.Show(
                $"⭐️ ВАШ РЕЦЕПТ\n\n" +
                $"Название: {recipe.Name}\n\n" +
                $"Описание: {recipe.Description}\n\n" +
                $"Время приготовления: {recipe.CookingTime} минут\n\n" +
                $"Пищевая ценность:\n" +
                $"• Белки: {recipe.Proteins}г\n" +
                $"• Жиры: {recipe.Fats}г\n" +
                $"• Углеводы: {recipe.Carbohydrates}г\n" +
                $"• Калории: {recipe.Calories} ккал\n\n" +
                $"Ингредиенты:\n" +
                string.Join("\n", recipe.Ingredients.Select(i => $"• {i.Name} - {i.Amount}{i.Unit}")) +
                (recipe.CookingSteps.Any() ? $"\n\nШаги приготовления:\n" +
                string.Join("\n", recipe.CookingSteps.OrderBy(s => s.StepNumber).Select(s => $"{s.StepNumber}. {s.Description}")) : ""),
                "Детали вашего рецепта",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void AddMyRecipesClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция добавления нового рецепта будет реализована позже");
            // CreateRecipeWindow createWindow = new CreateRecipeWindow();
            // createWindow.ShowDialog();
            // LoadMyRecipes(); // Перезагружаем список после добавления
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

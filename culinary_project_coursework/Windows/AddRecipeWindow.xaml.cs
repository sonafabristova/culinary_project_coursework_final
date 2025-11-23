using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace culinary_project_coursework.Windows 
{
    public partial class AddRecipeWindow : Window
    {
        public ObservableCollection<IngredientInput> Ingredients { get; set; }
        public ObservableCollection<CookingStepInput> Steps { get; set; }

        public Recipe NewRecipe { get; private set; }

        public AddRecipeWindow()
        {
            InitializeComponent();

            Ingredients = new ObservableCollection<IngredientInput>();
            Steps = new ObservableCollection<CookingStepInput>();

            IngredientsList.ItemsSource = Ingredients;
            StepsList.ItemsSource = Steps;

            // Добавляем первый ингредиент и шаг по умолчанию
            AddIngredient_Click(null, null);
            AddStep_Click(null, null);
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredients.Add(new IngredientInput());
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is IngredientInput ingredient)
            {
                Ingredients.Remove(ingredient);
            }
        }

        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            Steps.Add(new CookingStepInput { StepNumber = Steps.Count + 1 });
        }

        private void RemoveStep_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CookingStepInput step)
            {
                Steps.Remove(step);
                // Перенумеровываем оставшиеся шаги
                for (int i = 0; i < Steps.Count; i++)
                {
                    Steps[i].StepNumber = i + 1;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название рецепта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем новый рецепт
            NewRecipe = new Recipe
            {
                Id = AppContext.Recipes.Count > 0 ? AppContext.Recipes.Max(r => r.Id) + 1 : 1,
                Name = NameTextBox.Text.Trim(),
                Description = DescriptionTextBox.Text.Trim(),
                CookingTime = int.TryParse(TimeTextBox.Text, out int time) ? time : 0,
                Proteins = double.TryParse(ProteinsTextBox.Text, out double proteins) ? proteins : 0,
                Fats = double.TryParse(FatsTextBox.Text, out double fats) ? fats : 0,
                Carbohydrates = double.TryParse(CarbsTextBox.Text, out double carbs) ? carbs : 0,
                Calories = double.TryParse(CaloriesTextBox.Text, out double calories) ? calories : 0,
                UserId = AppContext.CurrentUser?.Id ?? 0,
                IsSystemRecipe = false
            };

            // Добавляем ингредиенты
            foreach (var ingredientInput in Ingredients.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                NewRecipe.Ingredients.Add(new Ingredient
                {
                    Id = NewRecipe.Ingredients.Count + 1,
                    Name = ingredientInput.Name.Trim(),
                    Amount = double.TryParse(ingredientInput.Amount, out double amount) ? amount : 0,
                    Unit = ingredientInput.Unit?.ToString() ?? "г",
                    RecipeId = NewRecipe.Id
                });
            }

            // Добавляем шаги приготовления
            foreach (var stepInput in Steps.Where(s => !string.IsNullOrWhiteSpace(s.Description)))
            {
                NewRecipe.CookingSteps.Add(new CookingStep
                {
                    Id = NewRecipe.CookingSteps.Count + 1,
                    StepNumber = stepInput.StepNumber,
                    Description = stepInput.Description.Trim(),
                    RecipeId = NewRecipe.Id
                });
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    // Вспомогательные классы для ввода данных
    public class IngredientInput
    {
        public string Name { get; set; } = "";
        public string Amount { get; set; } = "0";
        public object Unit { get; set; } = "г";
    }

    public class CookingStepInput
    {
        public int StepNumber { get; set; }
        public string Description { get; set; } = "";
    }
}
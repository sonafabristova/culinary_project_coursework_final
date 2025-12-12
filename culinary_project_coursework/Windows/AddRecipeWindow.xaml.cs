using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    public partial class AddRecipeWindow : Window
    {
        public ObservableCollection<IngredientInput> Ingredients { get; set; }
        public ObservableCollection<CookingStepInput> Steps { get; set; }

        public Рецепты NewRecipe { get; private set; }
        public AddRecipeWindow()
        {
            InitializeComponent();

            Ingredients = new ObservableCollection<IngredientInput>();
            Steps = new ObservableCollection<CookingStepInput>();

            IngredientsList.ItemsSource = Ingredients;
            StepsList.ItemsSource = Steps;

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

            if (Ingredients.Count(i => !string.IsNullOrWhiteSpace(i.Name)) == 0)
            {
                MessageBox.Show("Добавьте хотя бы один ингредиент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Steps.Count(s => !string.IsNullOrWhiteSpace(s.Description)) == 0)
            {
                MessageBox.Show("Добавьте хотя бы один шаг приготовления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем новый рецепт 
            NewRecipe = new Рецепты
            {
                Название = NameTextBox.Text.Trim(),
                Описание = DescriptionTextBox.Text.Trim(),
                ВремяПриготовления = int.TryParse(TimeTextBox.Text, out int time) ? time : 0,
                Белки = decimal.TryParse(ProteinsTextBox.Text, out decimal proteins) ? proteins : 0,
                Жиры = decimal.TryParse(FatsTextBox.Text, out decimal fats) ? fats : 0,
                Углеводы = decimal.TryParse(CarbsTextBox.Text, out decimal carbs) ? carbs : 0,
                Калории = int.TryParse(CaloriesTextBox.Text, out int calories) ? calories : 0,
                CreatedByUserId = AppContext.CurrentUser?.IdПользователя ?? 0,
                IsSystemRecipe = false
            };

           
            NewRecipe.СоставБлюдаs = new List<СоставБлюда>();
            NewRecipe.ШагиПриготовленияs = new List<ШагиПриготовления>();

            // + ингредиенты
            foreach (var ingredientInput in Ingredients.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                var ingredient = AppContext.FindOrCreateIngredient(ingredientInput.Name);
                if (ingredient != null)
                {
                    var unit = ingredientInput.Unit?.ToString() ?? "г";

                    NewRecipe.СоставБлюдаs.Add(new СоставБлюда
                    {
                        FkИнгредиента = ingredient.IdИнгредиента,
                        Количество = decimal.TryParse(ingredientInput.Amount, out decimal amount) ? amount : 0
                        
                    });
                }
            }

            // + шаги приготовления
            foreach (var stepInput in Steps.Where(s => !string.IsNullOrWhiteSpace(s.Description)))
            {
                NewRecipe.ШагиПриготовленияs.Add(new ШагиПриготовления
                {
                    НомерШага = stepInput.StepNumber,
                    Описание = stepInput.Description.Trim()
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

    //  классы для ввода данных
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
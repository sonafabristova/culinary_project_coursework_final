using System.Windows;

namespace culinary_project_coursework.Windows
{
    public partial class DishSelectionWindow : Window
    {
        public string SelectedDish { get; private set; }

        public DishSelectionWindow()
        {
            InitializeComponent();
            LoadDishes();
        }

        private void LoadDishes()
        {
            // TODO: Загрузить блюда из базы данных
            // Временные данные для примера
            var dishes = new[]
            {
                new { Name = "Овсяная каша с фруктами" },
                new { Name = "Омлет с овощами" },
                new { Name = "Блины с творогом" },
                new { Name = "Салат Цезарь" },
                new { Name = "Куриный суп" },
                new { Name = "Гречневая каша с грибами" },
        
            };

            DishesListBox.ItemsSource = dishes;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (DishesListBox.SelectedItem != null)
            {
                var selected = DishesListBox.SelectedItem;
                SelectedDish = selected.GetType().GetProperty("Name").GetValue(selected).ToString();
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
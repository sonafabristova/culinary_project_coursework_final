using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void CreateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранные значения
            var selectedDays = DaysComboBox.SelectedItem as ComboBoxItem;
            var selectedPeople = PeopleComboBox.SelectedItem as ComboBoxItem;

            if (selectedDays == null || selectedPeople == null ||
                selectedDays.Content.ToString() == "Выберите количество дней" ||
                selectedPeople.Content.ToString() == "Выберите количество человек")
            {
                MessageBox.Show("Пожалуйста, выберите количество дней и человек");
                return;
            }

            // Парсим количество дней и человек
            int daysCount = ParseDaysCount(selectedDays.Content.ToString());
            int peopleCount = ParsePeopleCount(selectedPeople.Content.ToString());

            // Создаем окно создания меню с параметрами
            CreateMenuWindow createMenuWindow = new CreateMenuWindow(daysCount, peopleCount);
            createMenuWindow.Show();
            this.Close();
        }

        private int ParseDaysCount(string daysText)
        {
            return daysText.Split(' ')[0] switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                _ => 1
            };
        }

        private int ParsePeopleCount(string peopleText)
        {
            return peopleText.Split(' ')[0] switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                _ => 1
            };
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

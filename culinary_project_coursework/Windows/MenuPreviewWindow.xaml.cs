using System.Collections.Generic;
using System.Windows;

namespace culinary_project_coursework.Windows
{
    public partial class MenuPreviewWindow : Window
    {
        private MenuPlan _menuPlan;

        public MenuPreviewWindow(MenuPlan menuPlan)
        {
            InitializeComponent();
            _menuPlan = menuPlan;
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            DaysItemsControl.ItemsSource = _menuPlan.Days;
        }

        private void CreateShoppingListButton_Click(object sender, RoutedEventArgs e)
        {
            var shoppingListWindow = new ShoppingListWindow(_menuPlan);
            shoppingListWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CreateMenuWindow createMenuWindow = new CreateMenuWindow(_menuPlan.DaysCount, _menuPlan.PeopleCount);
            createMenuWindow.Show();
            this.Close();
        }
    }
}
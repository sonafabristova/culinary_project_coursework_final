using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    public partial class ShoppingListWindow : Window
    {
        private ObservableCollection<ShoppingItem> _shoppingItems;
        private MenuPlan _menuPlan;

        public ShoppingListWindow(MenuPlan menuPlan)
        {
            InitializeComponent();
            _menuPlan = menuPlan;
            InitializeShoppingList();
        }

        private void InitializeShoppingList()
        {
            _shoppingItems = new ObservableCollection<ShoppingItem>();

            // Генерируем список только если есть выбранные блюда
            if (HasSelectedDishes())
            {
                GenerateShoppingListFromMenu();
            }
            else
            {
                // Если блюда не выбраны, показываем сообщение
                _shoppingItems.Add(new ShoppingItem
                {
                    ProductName = "Сначала выберите блюда в меню",
                    Quantity = ""
                });
            }

            ProductsItemsControl.ItemsSource = _shoppingItems;
        }

        private bool HasSelectedDishes()
        {
            // Проверяем, есть ли хотя бы одно выбранное блюдо во всем меню
            foreach (var day in _menuPlan.Days)
            {
                foreach (var person in day.People)
                {
                    if (person.Breakfast.IsSelected || person.Lunch.IsSelected || person.Dinner.IsSelected)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void GenerateShoppingListFromMenu()
        {
            
            var allIngredients = new Dictionary<string, double>();

            foreach (var day in _menuPlan.Days)
            {
                foreach (var person in day.People)
                {
                    // завтрак
                    if (person.Breakfast.IsSelected && person.Breakfast.SelectedRecipe != null)
                    {
                        AddRecipeIngredients(allIngredients, person.Breakfast.SelectedRecipe);
                    }

                    // обед
                    if (person.Lunch.IsSelected && person.Lunch.SelectedRecipe != null)
                    {
                        AddRecipeIngredients(allIngredients, person.Lunch.SelectedRecipe);
                    }

                    // ужин
                    if (person.Dinner.IsSelected && person.Dinner.SelectedRecipe != null)
                    {
                        AddRecipeIngredients(allIngredients, person.Dinner.SelectedRecipe);
                    }
                }
            }

            // ингредиенты в список покупок
            foreach (var ingredient in allIngredients)
            {
                _shoppingItems.Add(new ShoppingItem
                {
                    ProductName = ingredient.Key,
                    Quantity = $"{ingredient.Value} г"
                });
            }

            
            if (!_shoppingItems.Any())
            {
                _shoppingItems.Add(new ShoppingItem
                {
                    ProductName = "Нет данных об ингредиентах для выбранных блюд",
                    Quantity = ""
                });
            }
        }

        private void AddRecipeIngredients(Dictionary<string, double> allIngredients, Recipe recipe)
        {
            if (recipe.Ingredients == null) return;

            foreach (var ingredient in recipe.Ingredients)
            {
                var key = ingredient.Name;
                if (allIngredients.ContainsKey(key))
                {
                    allIngredients[key] += ingredient.Amount;
                }
                else
                {
                    allIngredients[key] = ingredient.Amount;
                }
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            _shoppingItems.Add(new ShoppingItem { ProductName = "Новый продукт", Quantity = "1 шт" });
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Для удаления выберите продукт", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!_shoppingItems.Any() ||
                _shoppingItems.Any(item => item.ProductName.Contains("Сначала выберите") ||
                                         item.ProductName.Contains("Нет данных")))
            {
                MessageBox.Show("Нет данных для сохранения. Сначала выберите блюда в меню.",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Сохранить список покупок",
                FileName = "Список_покупок.txt"
            };

            if (saveDialog.ShowDialog() == true)
            {
                SaveShoppingListToFile(saveDialog.FileName);
                MessageBox.Show("Список покупок успешно сохранен!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SaveShoppingListToFile(string filePath)
        {

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("СПИСОК ПОКУПОК");
                writer.WriteLine();

                foreach (var item in _shoppingItems)
                {
                    writer.WriteLine($"{item.ProductName} - {item.Quantity}");
                }

                writer.WriteLine();
                writer.WriteLine($"Всего продуктов: {_shoppingItems.Count}");
            }
            
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPreviewWindow previewWindow = new MenuPreviewWindow(_menuPlan);
            previewWindow.Show();
            this.Close();
        }
        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
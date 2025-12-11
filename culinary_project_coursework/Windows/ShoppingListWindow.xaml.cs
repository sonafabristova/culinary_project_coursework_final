using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using culinary_project_coursework.Classes;
using System.Windows.Controls;
using System.Windows.Media;

namespace culinary_project_coursework.Windows
{
    public partial class ShoppingListWindow : Window
    {
        private ObservableCollection<ShoppingItem> _shoppingItems;
        private MenuPlan _menuPlan;
        private ShoppingItem _selectedItem;

        // Конструктор для MenuPlan (старый)
        public ShoppingListWindow(MenuPlan menuPlan)
        {
            InitializeComponent();
            _menuPlan = menuPlan;
            InitializeShoppingList();
            SetupItemSelection();
        }

        // Новый конструктор для Dictionary<string, decimal>
        public ShoppingListWindow(Dictionary<string, decimal> ingredients)
        {
            InitializeComponent();
            InitializeShoppingListFromDictionary(ingredients);
            SetupItemSelection();
        }

        private void SetupItemSelection()
        {
            // Простой подход: используем Tag свойство Border для хранения ссылки на Border
            Loaded += (s, e) =>
            {
                // После загрузки окна, подписываемся на события кликов
                SubscribeToItemClicks();
            };
        }

        private void SubscribeToItemClicks()
        {
            // Находим все Border элементы и подписываемся на клики
            var borders = FindBordersInItemsControl();
            foreach (var border in borders)
            {
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            }
        }

        private List<Border> FindBordersInItemsControl()
        {
            var borders = new List<Border>();

            // Простой поиск Border элементов на первом уровне вложенности
            for (int i = 0; i < ProductsItemsControl.Items.Count; i++)
            {
                var container = ProductsItemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                if (container != null)
                {
                    // Ищем Border среди дочерних элементов
                    var border = FindFirstBorder(container);
                    if (border != null)
                    {
                        borders.Add(border);
                    }
                }
            }

            return borders;
        }

        private Border FindFirstBorder(DependencyObject parent)
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Border border)
                {
                    return border;
                }

                // Ищем дальше только если это не FrameworkElement с DataContext
                if (!(child is FrameworkElement))
                {
                    var childBorder = FindFirstBorder(child);
                    if (childBorder != null)
                    {
                        return childBorder;
                    }
                }
            }

            return null;
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is ShoppingItem item)
            {
                // Сбрасываем выделение у всех Border элементов
                ResetAllBorderBackgrounds();

                // Выделяем текущий элемент
                border.Background = System.Windows.Media.Brushes.LightBlue;
                _selectedItem = item;
            }
        }

        private void ResetAllBorderBackgrounds()
        {
            // Сбрасываем фон у всех Border элементов
            for (int i = 0; i < ProductsItemsControl.Items.Count; i++)
            {
                var container = ProductsItemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                if (container != null)
                {
                    var border = FindFirstBorder(container);
                    if (border != null)
                    {
                        border.Background = System.Windows.Media.Brushes.White;
                    }
                }
            }
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

        private void InitializeShoppingListFromDictionary(Dictionary<string, decimal> ingredients)
        {
            _shoppingItems = new ObservableCollection<ShoppingItem>();

            if (ingredients != null && ingredients.Count > 0)
            {
                foreach (var item in ingredients.OrderBy(i => i.Key))
                {
                    // Разбираем строку с единицей измерения
                    var productName = item.Key;
                    string quantityText = $"{item.Value:0.##}";

                    // Если в ключе есть единица измерения в скобках
                    if (item.Key.Contains('(') && item.Key.Contains(')'))
                    {
                        int start = item.Key.IndexOf('(') + 1;
                        int end = item.Key.IndexOf(')');
                        string unit = item.Key.Substring(start, end - start);
                        productName = item.Key.Substring(0, item.Key.IndexOf('(')).Trim();
                        quantityText = $"{item.Value:0.##} {unit}";
                    }
                    else
                    {
                        quantityText = $"{item.Value:0.##} г";
                    }

                    _shoppingItems.Add(new ShoppingItem
                    {
                        ProductName = productName,
                        Quantity = quantityText
                    });
                }
            }
            else
            {
                _shoppingItems.Add(new ShoppingItem
                {
                    ProductName = "Нет выбранных рецептов",
                    Quantity = ""
                });
            }

            ProductsItemsControl.ItemsSource = _shoppingItems;
        }

        private bool HasSelectedDishes()
        {
            if (_menuPlan == null || _menuPlan.Days == null)
                return false;

            // Проверяем, есть ли хотя бы одно выбранное блюдо во всем меню
            foreach (var day in _menuPlan.Days)
            {
                if (day.People == null) continue;

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
                    Quantity = $"{ingredient.Value:0.##} г"
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

            // После добавления нового элемента нужно обновить подписки на события
            SubscribeToItemClicks();
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                var result = MessageBox.Show($"Удалить продукт '{_selectedItem.ProductName}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _shoppingItems.Remove(_selectedItem);
                    _selectedItem = null;
                    ResetAllBorderBackgrounds();
                }
            }
            else
            {
                MessageBox.Show("Для удаления сначала выберите продукт, кликнув по нему", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_shoppingItems.Any() ||
                _shoppingItems.Any(item => item.ProductName.Contains("Сначала выберите") ||
                                         item.ProductName.Contains("Нет данных") ||
                                         item.ProductName.Contains("Нет выбранных рецептов")))
            {
                MessageBox.Show("Нет данных для сохранения. Сначала выберите блюда в меню.",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Сохранить список покупок",
                FileName = $"Список_покупок_{DateTime.Now:yyyyMMdd_HHmm}.txt"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    SaveShoppingListToFile(saveDialog.FileName);
                    MessageBox.Show("Список покупок успешно сохранен!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения файла: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveShoppingListToFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
              
                writer.WriteLine("СПИСОК ПОКУПОК");
         
                writer.WriteLine();
              
                writer.WriteLine();

                int index = 1;
                foreach (var item in _shoppingItems)
                {
                    if (!string.IsNullOrEmpty(item.ProductName) &&
                        !item.ProductName.Contains("Сначала выберите") &&
                        !item.ProductName.Contains("Нет данных") &&
                        !item.ProductName.Contains("Нет выбранных рецептов"))
                    {
                        writer.WriteLine($"{index}. {item.ProductName} - {item.Quantity}");
                        index++;
                    }
                }

                writer.WriteLine();
                writer.WriteLine($"Всего продуктов: {index - 1}");
                writer.WriteLine("=".PadRight(50, '='));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    // Класс ShoppingItem
    public class ShoppingItem
    {
        public string ProductName { get; set; }
        public string Quantity { get; set; }
    }
}
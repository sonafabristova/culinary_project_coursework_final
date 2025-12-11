using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    public partial class DishSelectionWindow : Window
    {
        public Рецепты SelectedRecipe { get; private set; }
        private List<Рецепты> _allRecipes = new List<Рецепты>();

        public DishSelectionWindow()
        {
            InitializeComponent();
            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {
            try
            {
                // Загружаем все рецепты (системные и пользовательские)
                _allRecipes.Clear();
                
                // Системные рецепты
                var systemRecipes = AppContext.GetSystemRecipes();
                _allRecipes.AddRange(systemRecipes);
                
                // Пользовательские рецепты (если пользователь авторизован)
                if (AppContext.CurrentUser != null)
                {
                    var userRecipes = AppContext.GetUserRecipes(AppContext.CurrentUser.IdПользователя);
                    _allRecipes.AddRange(userRecipes);
                }

                // Загружаем в ComboBox все рецепты по умолчанию
                LoadRecipesToComboBox("all");
                
                if (_allRecipes.Count == 0)
                {
                    MessageBox.Show("В базе данных нет доступных рецептов", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRecipesToComboBox(string recipeType)
        {
            RecipesComboBox.Items.Clear();

            // Добавляем пустой элемент
            RecipesComboBox.Items.Add(new ComboBoxItem { 
                Content = "Выберите рецепт",
                Tag = "empty"
            });

            // Фильтруем рецепты по типу
            IEnumerable<Рецепты> filteredRecipes = recipeType switch
            {
                "system" => _allRecipes.Where(r => r.IsSystemRecipe == true),
                "user" => _allRecipes.Where(r => r.IsSystemRecipe == false),
                _ => _allRecipes // "all"
            };

            // Добавляем рецепты в ComboBox
            foreach (var recipe in filteredRecipes.OrderBy(r => r.Название))
            {
                RecipesComboBox.Items.Add(recipe);
            }

            // Выбираем первый элемент (пустой)
            RecipesComboBox.SelectedIndex = 0;
        }

        private void RecipeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeTypeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                string recipeType = selectedItem.Tag.ToString();
                LoadRecipesToComboBox(recipeType);
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesComboBox.SelectedItem is Рецепты selectedRecipe)
            {
                SelectedRecipe = selectedRecipe;
                this.DialogResult = true;
                this.Close();
            }
            else if (RecipesComboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                if (comboBoxItem.Tag?.ToString() == "empty")
                {
                    MessageBox.Show("Пожалуйста, выберите рецепт из списка", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
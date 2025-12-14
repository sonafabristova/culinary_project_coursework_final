using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using culinary_project_coursework.Models;
using System.Windows.Input;

namespace culinary_project_coursework.Windows
{
    public partial class AddRecipeWindow : Window
    {
        public ObservableCollection<IngredientInput> Ingredients { get; set; }
        public ObservableCollection<CookingStepInput> Steps { get; set; }

        public Рецепты NewRecipe { get; private set; }
        private string _selectedImagePath; // Путь к выбранному изображению
        private string _savedImagePath; // Относительный путь для сохранения в БД

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

        // Кнопка добавления фото
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            SelectAndLoadImage();
        }

        private void SelectAndLoadImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите фото рецепта",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadSelectedImage(openFileDialog.FileName);
            }
        }

        private void LoadSelectedImage(string imagePath)
        {
            try
            {
                // Загружаем изображение
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();

                RecipeImage.Source = bitmap;
                _selectedImagePath = imagePath;

                // Показываем кнопку удаления и скрываем кнопку "+"
                RemovePhotoButton.Visibility = Visibility.Visible;
                AddPhotoButton.Visibility = Visibility.Collapsed;

                // Меняем курсор на стандартный
                PhotoBorder.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка удаления фото
        private void RemovePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeImage.Source = null;
            _selectedImagePath = null;
            _savedImagePath = null;

            // Скрываем кнопку удаления и показываем кнопку "+"
            RemovePhotoButton.Visibility = Visibility.Collapsed;
            AddPhotoButton.Visibility = Visibility.Visible;

            // Восстанавливаем курсор "рука"
            PhotoBorder.Cursor = Cursors.Hand;
        }

        // Сохраняем изображение в папку проекта Images/Recipes/
        private bool SaveImageToProjectFolder()
        {
            if (string.IsNullOrEmpty(_selectedImagePath))
                return false;

            try
            {
                // Получаем путь к КОРНЮ ПРОЕКТА (не к bin/Debug/)
                string projectRoot = GetProjectRootDirectory();
             
                string imagesFolder = Path.Combine(projectRoot, "Images", "Recipes");
             

                // Создаем папки если их нет
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                    
                }

                // Генерируем уникальное имя файла
                string extension = Path.GetExtension(_selectedImagePath);
                string fileName = GenerateFileName(extension);
                string destinationPath = Path.Combine(imagesFolder, fileName);

                // Копируем файл
                File.Copy(_selectedImagePath, destinationPath, true);
               
                _savedImagePath = $"Images/Recipes/{fileName}";
               
                if (File.Exists(destinationPath))
                {
                    return true;
                }
                else
                {
                   
                    return false;
                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _savedImagePath = null;
                return false;
            }
        }

        // Получаем путь к корню проекта
        private string GetProjectRootDirectory()
        {
            try
            {
                // Текущая директория (обычно bin/Debug/netX.0/)
                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
             

                // Поднимаемся на 3 уровня вверх: bin/Debug/netX.0 -> корень проекта
                DirectoryInfo dir = new DirectoryInfo(currentDir);

                // Если есть .csproj файл в текущей директории или выше
                while (dir != null)
                {
                    // Ищем .csproj файл
                    var csprojFiles = dir.GetFiles("*.csproj");
                    if (csprojFiles.Length > 0)
                    {
                       
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }

                // Если не нашли .csproj, поднимаемся на 3 уровня
                string projectRoot = Directory.GetParent(currentDir)?.Parent?.Parent?.FullName;
                if (!string.IsNullOrEmpty(projectRoot))
                {
                   
                    return projectRoot;
                }

                return currentDir;
            }
            catch (Exception ex)
            {
               
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private string GenerateFileName(string extension)
        {
            string recipeName = NameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(recipeName))
            {
                string safeName = MakeFileNameSafe(recipeName);
                return $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
            }

            
            return $"recipe_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
        }

        private string MakeFileNameSafe(string fileName)
        {
            
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), "_");
            }

           
            fileName = fileName.Replace(" ", "_");

            if (fileName.Length > 50)
            {
                fileName = fileName.Substring(0, 50);
            }

            return fileName.ToLower();
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
                MessageBox.Show("Введите название рецепта", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Ingredients.Count(i => !string.IsNullOrWhiteSpace(i.Name)) == 0)
            {
                MessageBox.Show("Добавьте хотя бы один ингредиент", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Steps.Count(s => !string.IsNullOrWhiteSpace(s.Description)) == 0)
            {
                MessageBox.Show("Добавьте хотя бы один шаг приготовления", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем изображение в папку проекта
            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                bool saved = SaveImageToProjectFolder();
                if (!saved)
                {
                    var result = MessageBox.Show("Не удалось сохранить изображение. Продолжить без фото?",
                                              "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                        return;
                }
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
                IsSystemRecipe = false,
                Изображение = _savedImagePath // Сохраняем относительный путь
            };

           
            // Проверяем, что файл существует
            if (!string.IsNullOrEmpty(_savedImagePath))
            {
                string projectRoot = GetProjectRootDirectory();
                string fullPath = Path.Combine(projectRoot, _savedImagePath.Replace("/", "\\"));
               

                if (!File.Exists(fullPath))
                {
                    MessageBox.Show($"Внимание: файл изображения не найден!\n{fullPath}\n" +
                                  "Рецепт будет сохранен, но изображение не отобразится.",
                                  "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

         
            NewRecipe.СоставБлюдаs = new List<СоставБлюда>();
            NewRecipe.ШагиПриготовленияs = new List<ШагиПриготовления>();

            // Добавляем ингредиенты
            foreach (var ingredientInput in Ingredients.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                var ingredient = AppContext.FindOrCreateIngredient(ingredientInput.Name);
                if (ingredient != null)
                {
                    NewRecipe.СоставБлюдаs.Add(new СоставБлюда
                    {
                        FkИнгредиента = ingredient.IdИнгредиента,
                        Количество = decimal.TryParse(ingredientInput.Amount, out decimal amount) ? amount : 0
                    });
                }
            }

            // Добавляем шаги приготовления
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
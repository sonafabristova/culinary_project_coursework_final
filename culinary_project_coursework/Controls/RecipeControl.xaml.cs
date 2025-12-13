using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Controls
{
    public partial class RecipeControl : UserControl
    {
        public RecipeControl()
        {
            InitializeComponent();
            Loaded += RecipeControl_Loaded;
        }

        private void RecipeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is Рецепты recipe)
            {
                LoadRecipeImage(recipe.Изображение);
            }
            else
            {
                ShowPlaceholder();
            }
        }

        private void LoadRecipeImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string fullPath = GetFullImagePath(imagePath);

                    if (File.Exists(fullPath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(fullPath);
                        bitmap.EndInit();

                        RecipeImage.Source = bitmap;
                        RecipeImage.Visibility = Visibility.Visible;
                        ImagePlaceholder.Visibility = Visibility.Collapsed;
                        return;
                    }
                }

                // Если фото нет или не найдено
                ShowPlaceholder();
            }
            catch
            {
                ShowPlaceholder();
            }
        }

        private string GetFullImagePath(string relativePath)
        {
            try
            {
                // Очищаем путь от лишних символов
                relativePath = relativePath.Trim();

                // Если путь уже абсолютный
                if (Path.IsPathRooted(relativePath))
                {
                    return relativePath;
                }

           
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                // Корень проекта (поднимаемся на 3 уровня из bin/Debug/netX.0)
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(baseDir);
                    string projectRoot = dir.Parent?.Parent?.Parent?.FullName;
                    if (!string.IsNullOrEmpty(projectRoot))
                    {
                        string path = Path.Combine(projectRoot, relativePath.Replace("/", "\\"));
                        if (File.Exists(path))
                            return path;
                    }
                }
                catch { }

                // Из папки с исполняемым файлом
                string path2 = Path.Combine(baseDir, relativePath.Replace("/", "\\"));
                if (File.Exists(path2))
                    return path2;

                // Если Images/Recipes/ в начале пути
                if (relativePath.StartsWith("Images/Recipes/") || relativePath.StartsWith("Images\\Recipes\\"))
                {
                    string fileName = Path.GetFileName(relativePath);
                    DirectoryInfo dir = new DirectoryInfo(baseDir);
                    string projectRoot = dir.Parent?.Parent?.Parent?.FullName;
                    if (!string.IsNullOrEmpty(projectRoot))
                    {
                        string path = Path.Combine(projectRoot, "Images", "Recipes", fileName);
                        if (File.Exists(path))
                            return path;
                    }
                }

                //  В папке Images рядом 
                string path4 = Path.Combine(baseDir, "Images", "Recipes", Path.GetFileName(relativePath));
                if (File.Exists(path4))
                    return path4;

             
                return Path.Combine(baseDir, relativePath.Replace("/", "\\"));
            }
            catch
            {
                return relativePath;
            }
        }

        private void ShowPlaceholder()
        {
            RecipeImage.Source = null;
            RecipeImage.Visibility = Visibility.Collapsed;
            ImagePlaceholder.Visibility = Visibility.Visible;
        }
    }
}
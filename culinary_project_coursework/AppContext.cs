using System.Collections.Generic;
using System.Linq;
using System.Windows;
using culinary_project_coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace culinary_project_coursework
{
    public static class AppContext
    {
        // Текущий пользователь ID
        public static int? CurrentUserId { get; set; }

        // Свойство CurrentUser для совместимости
        public static Пользователи CurrentUser
        {
            get
            {
                if (CurrentUserId == null) return null;

                try
                {
                    using (var db = new BdCourseContext())
                    {
                        return db.Пользователиs
                            .FirstOrDefault(u => u.IdПользователя == CurrentUserId.Value);
                    }
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                CurrentUserId = value?.IdПользователя;
            }
        }
        public static List<Рецепты> ReloadRecipes()
        {
            using (var db = new BdCourseContext())
            {
                return db.Рецептыs
                    .Include(r => r.CreatedByUser)
                    .Include(r => r.СоставБлюдаs)
                        .ThenInclude(s => s.FkИнгредиентаNavigation)
                    .Include(r => r.ШагиПриготовленияs)
                    .ToList();
            }
        }
        // Свойство Users для совместимости
        public static List<Пользователи> Users
        {
            get
            {
                try
                {
                    using (var db = new BdCourseContext())
                    {
                        return db.Пользователиs
                            .Where(u => u.IsActive == true)
                            .ToList();
                    }
                }
                catch
                {
                    return new List<Пользователи>();
                }
            }
        }

        // Свойство Recipes для совместимости
        // Исправленное свойство Recipes
        public static List<Рецепты> Recipes
        {
            get
            {
                try
                {
                    using (var db = new BdCourseContext())
                    {
                        return db.Рецептыs
                            .Include(r => r.CreatedByUser)
                            .Include(r => r.СоставБлюдаs)
                                .ThenInclude(s => s.FkИнгредиентаNavigation)
                                    .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)
                            .Include(r => r.ШагиПриготовленияs)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки всех рецептов: {ex.Message}");
                    return new List<Рецепты>();
                }
            }
        }

        // Метод для получения системных рецептов (ВСЕГДА СВЕЖИЕ)
        // Исправленный метод в AppContext.cs
        public static List<Рецепты> GetSystemRecipes()
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    return db.Рецептыs
                        .Include(r => r.CreatedByUser)
                        .Include(r => r.СоставБлюдаs)
                            .ThenInclude(s => s.FkИнгредиентаNavigation)
                                .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)
                        .Include(r => r.ШагиПриготовленияs)
                        .Where(r => r.IsSystemRecipe == true && r.CreatedByUserId == null) // Явно проверяем NULL
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки системных рецептов: {ex.Message}");
                return new List<Рецепты>();
            }
        }
        // Исправленный метод для пользовательских рецептов
        public static List<Рецепты> GetUserRecipes(int userId)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    return db.Рецептыs
                        .Include(r => r.CreatedByUser)
                        .Include(r => r.СоставБлюдаs)
                            .ThenInclude(s => s.FkИнгредиентаNavigation)
                                .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)  // Добавлено
                        .Include(r => r.ШагиПриготовленияs)
                        .Where(r => r.CreatedByUserId == userId && r.IsSystemRecipe == false)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользовательских рецептов: {ex.Message}");
                return new List<Рецепты>();
            }
        }
        

        // Аутентификация
        public static Пользователи AuthenticateUser(string login, string password)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    var user = db.Пользователиs
                        .FirstOrDefault(u => u.Логин == login && u.Пароль == password && u.IsActive == true);

                    if (user != null)
                    {
                        CurrentUserId = user.IdПользователя;
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка аутентификации: {ex.Message}");
                return null;
            }
        }

        // Регистрация
        public static bool RegisterUser(string name, string login, string password, string phone)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    // Проверка существования пользователя
                    if (db.Пользователиs.Any(u => u.Логин == login))
                        return false;

                    // Создание нового пользователя
                    var newUser = new Пользователи
                    {
                        Имя = name,
                        Логин = login,
                        Пароль = password,
                        Телефон = phone,
                        Почта = $"{login}@example.com",
                        IsActive = true
                    };

                    db.Пользователиs.Add(newUser);
                    db.SaveChanges();

                    CurrentUserId = newUser.IdПользователя;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
                return false;
            }
        }

        // Добавление рецепта
        // Добавление рецепта
        public static void AddRecipe(Рецепты recipe)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    // Отслеживаем изменения для отладки
                    db.ChangeTracker.StateChanged += (sender, args) =>
                    {
                    };

                    // Добавляем рецепт
                    db.Рецептыs.Add(recipe);

                    // Пробуем сохранить
                    int changes = db.SaveChanges();

                    MessageBox.Show($"Рецепт успешно добавлен!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException dbEx)
            {

                MessageBox.Show($"Ошибка БД при добавлении рецепта",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                string fullError = $"Сообщение: {ex.Message}\n\n";

                if (ex.InnerException != null)
                {
                    fullError += $"Внутренняя ошибка: {ex.InnerException.Message}\n\n";

                    if (ex.InnerException.InnerException != null)
                    {
                        fullError += $"Детали: {ex.InnerException.InnerException.Message}";
                    }
                }

                MessageBox.Show($"Ошибка добавления рецепта:\n\n{fullError}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


       
        // Удаление рецепта
        public static void DeleteRecipe(int recipeId)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    var recipe = db.Рецептыs.Find(recipeId);
                    if (recipe != null)
                    {
                        db.Рецептыs.Remove(recipe);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления рецепта: {ex.Message}");
            }
        }

        // Проверка существования пользователя
        public static bool UserExists(string login)
        {
            try
            {
                using (var db = new BdCourseContext())
                {
                    return db.Пользователиs.Any(u => u.Логин == login);
                }
            }
            catch
            {
                return false;
            }
        }

        
        public static Ингредиенты FindOrCreateIngredient(string name)
        {
            using (var context = new BdCourseContext())
            {
                var ingredient = context.Ингредиентыs
                    .FirstOrDefault(i => i.Название.ToLower() == name.ToLower());

                if (ingredient == null)
                {
                    ingredient = new Ингредиенты
                    {
                        Название = name,
                        FkЕдиницыИзмерения = 1 // По умолчанию граммы
                                               // ПРОБЛЕМА: Возможно где-то еще устанавливается IdИнгредиента
                    };
                    context.Ингредиентыs.Add(ingredient);
                    context.SaveChanges();
                }

                return ingredient; // ← Возвращает объект с ID
            }
        }
    }
}
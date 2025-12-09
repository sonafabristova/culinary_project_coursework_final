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
                    using (var db = new WithIngContext())
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
            using (var db = new WithIngContext())
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
                    using (var db = new ForCwContext())
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
                    using (var db = new WithIngContext())
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
                using (var db = new WithIngContext())
                {
                    return db.Рецептыs
                        .Include(r => r.CreatedByUser)
                        .Include(r => r.СоставБлюдаs)
                            .ThenInclude(s => s.FkИнгредиентаNavigation)
                                .ThenInclude(i => i.FkЕдиницыИзмеренияNavigation)  // Добавлено
                        .Include(r => r.ШагиПриготовленияs)
                        .Where(r => r.IsSystemRecipe == true)
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
                using (var db = new WithIngContext())
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
                using (var db = new WithIngContext())
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
                using (var db = new WithIngContext())
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
        public static void AddRecipe(Рецепты recipe)
        {
            try
            {
                using (var db = new WithIngContext())
                {
                    db.Рецептыs.Add(recipe);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления рецепта: {ex.Message}");
            }
        }

        // Удаление рецепта
        public static void DeleteRecipe(int recipeId)
        {
            try
            {
                using (var db = new WithIngContext())
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
                using (var db = new WithIngContext())
                {
                    return db.Пользователиs.Any(u => u.Логин == login);
                }
            }
            catch
            {
                return false;
            }
        }

        // Метод для проверки данных в БД (отладка)
        public static void CheckDatabase()
        {
            try
            {
                using (var db = new WithIngContext())
                {
                    var recipes = db.Рецептыs.ToList();
                    var message = $"В БД {recipes.Count} рецептов:\n";

                    foreach (var recipe in recipes)
                    {
                        var stepsCount = db.ШагиПриготовленияs.Count(s => s.FkРецепта == recipe.IdРецепта);
                        message += $"- {recipe.Название} (ID: {recipe.IdРецепта}, Шагов: {stepsCount})\n";
                    }

                    MessageBox.Show(message, "Проверка БД");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Метод для принудительного обновления данных
        public static void RefreshData()
        {
            // Создаем новый контекст для следующего запроса
            Console.WriteLine("Данные обновлены");
        }
    }
}
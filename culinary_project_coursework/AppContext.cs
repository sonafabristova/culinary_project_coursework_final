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
                        Console.WriteLine("Обнаружены изменения в трекере");
                    };

                    // Добавляем рецепт
                    db.Рецептыs.Add(recipe);

                    // Пробуем сохранить
                    int changes = db.SaveChanges();

                    MessageBox.Show($"Рецепт успешно добавлен! Изменений: {changes}",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Детальная информация об ошибке БД
                string errorDetails = GetDbErrorDetails(dbEx);

                MessageBox.Show($"Ошибка БД при добавлении рецепта:\n\n{errorDetails}",
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

        // Метод для получения детальной информации об ошибке БД
        private static string GetDbErrorDetails(DbUpdateException dbEx)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Тип ошибки: {dbEx.GetType().Name}");
            sb.AppendLine($"Сообщение: {dbEx.Message}");

            if (dbEx.InnerException != null)
            {
                sb.AppendLine($"\nВнутренняя ошибка: {dbEx.InnerException.Message}");

                // Пробуем получить детали SQL ошибки
                var sqlEx = dbEx.InnerException as Microsoft.Data.SqlClient.SqlException;
                if (sqlEx != null)
                {
                    sb.AppendLine($"\nSQL Ошибка #{sqlEx.Number}: {sqlEx.Message}");

                    foreach (Microsoft.Data.SqlClient.SqlError error in sqlEx.Errors)
                    {
                        sb.AppendLine($"- Ошибка #{error.Number}: {error.Message}");
                        sb.AppendLine($"  Процедура: {error.Procedure}, Строка: {error.LineNumber}");
                    }
                }
            }

            // Информация о сущностях, вызвавших ошибку
            sb.AppendLine("\nСущности с ошибками:");
            foreach (var entry in dbEx.Entries)
            {
                sb.AppendLine($"- Тип: {entry.Entity.GetType().Name}, Состояние: {entry.State}");

                if (entry.State == EntityState.Added)
                {
                    foreach (var property in entry.Properties)
                    {
                        sb.AppendLine($"  {property.Metadata.Name} = {property.CurrentValue ?? "NULL"}");
                    }
                }
            }

            return sb.ToString();
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

        // Метод для проверки данных в БД (отладка)
        public static void CheckDatabase()
        {
            try
            {
                using (var db = new BdCourseContext())
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
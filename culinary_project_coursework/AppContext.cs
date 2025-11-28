using System.Collections.Generic;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework
{
    public static class AppContext
    {
        public static List<User> Users = new List<User>()
        {
            new User() { Id = 0, Login = "1", Password = "1", Name = "Пользователь 1", PhoneNumber = "111" },
            new User() { Id = 1, Login = "2", Password = "2", Name = "Пользователь 2", PhoneNumber = "222" },
            new User() { Id = 2, Login = "3", Password = "3", Name = "Пользователь 3", PhoneNumber = "333" }
        };

        public static User CurrentUser { get; set; }

        // Тестовые рецепты
        public static List<Recipe> Recipes = new List<Recipe>()
        {
            // Системные рецепты (IsSystemRecipe = true)
            new Recipe()
            {
                Id = 1,
                Name = "Омлет с овощами",
                Description = "Вкусный и полезный завтрак",
                CookingTime = 20,
                Calories = 210,
                Proteins = 15,
                Fats = 12,
                Carbohydrates = 8,
                UserId = 0,
                IsSystemRecipe = true,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 1, Name = "Яйца", Amount = 3, Unit = "шт", RecipeId = 1 },
                    new Ingredient { Id = 2, Name = "Помидор", Amount = 150, Unit = "г", RecipeId = 1 },
                    new Ingredient { Id = 3, Name = "Перец болгарский", Amount = 100, Unit = "г", RecipeId = 1 },
                    new Ingredient { Id = 4, Name = "Лук репчатый", Amount = 50, Unit = "г", RecipeId = 1 },
                    new Ingredient { Id = 5, Name = "Молоко", Amount = 50, Unit = "мл", RecipeId = 1 },
                    new Ingredient { Id = 6, Name = "Масло растительное", Amount = 15, Unit = "мл", RecipeId = 1 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 1, StepNumber = 1, Description = "Нарезать овощи кубиками", RecipeId = 1 },
                    new CookingStep { Id = 2, StepNumber = 2, Description = "Взбить яйца с молоком, солью и перцем", RecipeId = 1 },
                    new CookingStep { Id = 3, StepNumber = 3, Description = "Обжарить овощи на сковороде 5 минут", RecipeId = 1 },
                    new CookingStep { Id = 4, StepNumber = 4, Description = "Залить яичной смесью и готовить под крышкой 10 минут", RecipeId = 1 }
                }
            },
            new Recipe()
            {
                Id = 2,
                Name = "Гречневая каша",
                Description = "Простая и полезная каша",
                CookingTime = 25,
                Calories = 150,
                Proteins = 5,
                Fats = 2,
                Carbohydrates = 30,
                UserId = 0,
                IsSystemRecipe = true,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 7, Name = "Гречка", Amount = 100, Unit = "г", RecipeId = 2 },
                    new Ingredient { Id = 8, Name = "Вода", Amount = 200, Unit = "мл", RecipeId = 2 },
                    new Ingredient { Id = 9, Name = "Соль", Amount = 2, Unit = "г", RecipeId = 2 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 5, StepNumber = 1, Description = "Промыть гречку", RecipeId = 2 },
                    new CookingStep { Id = 6, StepNumber = 2, Description = "Залить водой в соотношении 1:2", RecipeId = 2 },
                    new CookingStep { Id = 7, StepNumber = 3, Description = "Варить на медленном огне 20 минут", RecipeId = 2 },
                    new CookingStep { Id = 8, StepNumber = 4, Description = "Посолить и дать настояться 5 минут", RecipeId = 2 }
                }
            },
            new Recipe()
            {
                Id = 3,
                Name = "Куриный суп с лапшой",
                Description = "Ароматный и согревающий суп",
                CookingTime = 45,
                Calories = 180,
                Proteins = 12,
                Fats = 6,
                Carbohydrates = 20,
                UserId = 0,
                IsSystemRecipe = true,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 10, Name = "Куриное филе", Amount = 200, Unit = "г", RecipeId = 3 },
                    new Ingredient { Id = 11, Name = "Лапша", Amount = 100, Unit = "г", RecipeId = 3 },
                    new Ingredient { Id = 12, Name = "Морковь", Amount = 1, Unit = "шт", RecipeId = 3 },
                    new Ingredient { Id = 13, Name = "Лук репчатый", Amount = 1, Unit = "шт", RecipeId = 3 },
                    new Ingredient { Id = 14, Name = "Картофель", Amount = 2, Unit = "шт", RecipeId = 3 },
                    new Ingredient { Id = 15, Name = "Зелень", Amount = 10, Unit = "г", RecipeId = 3 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 9, StepNumber = 1, Description = "Отварить куриное филе до готовности", RecipeId = 3 },
                    new CookingStep { Id = 10, StepNumber = 2, Description = "Нарезать овощи кубиками", RecipeId = 3 },
                    new CookingStep { Id = 11, StepNumber = 3, Description = "Добавить овощи в бульон и варить 15 минут", RecipeId = 3 },
                    new CookingStep { Id = 12, StepNumber = 4, Description = "Добавить лапшу и варить еще 10 минут", RecipeId = 3 },
                    new CookingStep { Id = 13, StepNumber = 5, Description = "Подавать с зеленью", RecipeId = 3 }
                }
            },
            new Recipe()
            {
                Id = 4,
                Name = "Салат Цезарь",
                Description = "Классический салат с курицей и сухариками",
                CookingTime = 30,
                Calories = 320,
                Proteins = 18,
                Fats = 22,
                Carbohydrates = 15,
                UserId = 0,
                IsSystemRecipe = true,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 16, Name = "Куриное филе", Amount = 150, Unit = "г", RecipeId = 4 },
                    new Ingredient { Id = 17, Name = "Листья салата", Amount = 100, Unit = "г", RecipeId = 4 },
                    new Ingredient { Id = 18, Name = "Пармезан", Amount = 50, Unit = "г", RecipeId = 4 },
                    new Ingredient { Id = 19, Name = "Сухарики", Amount = 30, Unit = "г", RecipeId = 4 },
                    new Ingredient { Id = 20, Name = "Соус Цезарь", Amount = 50, Unit = "мл", RecipeId = 4 },
                    new Ingredient { Id = 21, Name = "Помидоры черри", Amount = 100, Unit = "г", RecipeId = 4 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 14, StepNumber = 1, Description = "Обжарить куриное филе до золотистой корочки", RecipeId = 4 },
                    new CookingStep { Id = 15, StepNumber = 2, Description = "Порвать листья салата руками", RecipeId = 4 },
                    new CookingStep { Id = 16, StepNumber = 3, Description = "Нарезать помидоры черри пополам", RecipeId = 4 },
                    new CookingStep { Id = 17, StepNumber = 4, Description = "Смешать все ингредиенты и заправить соусом", RecipeId = 4 },
                    new CookingStep { Id = 18, StepNumber = 5, Description = "Посыпать пармезаном и сухариками", RecipeId = 4 }
                }
            },
            new Recipe()
            {
                Id = 5,
                Name = "Шоколадные маффины",
                Description = "Нежные шоколадные кексы",
                CookingTime = 35,
                Calories = 280,
                Proteins = 5,
                Fats = 15,
                Carbohydrates = 35,
                UserId = 0,
                IsSystemRecipe = true,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 22, Name = "Мука", Amount = 200, Unit = "г", RecipeId = 5 },
                    new Ingredient { Id = 23, Name = "Сахар", Amount = 150, Unit = "г", RecipeId = 5 },
                    new Ingredient { Id = 24, Name = "Какао", Amount = 50, Unit = "г", RecipeId = 5 },
                    new Ingredient { Id = 25, Name = "Яйца", Amount = 2, Unit = "шт", RecipeId = 5 },
                    new Ingredient { Id = 26, Name = "Молоко", Amount = 100, Unit = "мл", RecipeId = 5 },
                    new Ingredient { Id = 27, Name = "Разрыхлитель", Amount = 10, Unit = "г", RecipeId = 5 },
                    new Ingredient { Id = 28, Name = "Шоколадные дропсы", Amount = 50, Unit = "г", RecipeId = 5 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 19, StepNumber = 1, Description = "Смешать сухие ингредиенты", RecipeId = 5 },
                    new CookingStep { Id = 20, StepNumber = 2, Description = "Добавить яйца и молоко, тщательно перемешать", RecipeId = 5 },
                    new CookingStep { Id = 21, StepNumber = 3, Description = "Добавить шоколадные дропсы", RecipeId = 5 },
                    new CookingStep { Id = 22, StepNumber = 4, Description = "Разлить по формочкам и выпекать 25 минут при 180°C", RecipeId = 5 },
                    new CookingStep { Id = 23, StepNumber = 5, Description = "Дать остыть перед подачей", RecipeId = 5 }
                }
            },

            // Рецепты от пользователя (IsSystemRecipe = false, UserId = 1)
            new Recipe()
            {
                Id = 6,
                Name = "Семейный рецепт борща",
                Description = "Борщ по бабушкиному рецепту",
                CookingTime = 90,
                Calories = 190,
                Proteins = 8,
                Fats = 10,
                Carbohydrates = 18,
                UserId = 0, // ID пользователя, который создал рецепт
                IsSystemRecipe = false, // Это пользовательский рецепт
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 29, Name = "Свекла", Amount = 2, Unit = "шт", RecipeId = 6 },
                    new Ingredient { Id = 30, Name = "Капуста", Amount = 200, Unit = "г", RecipeId = 6 },
                    new Ingredient { Id = 31, Name = "Картофель", Amount = 3, Unit = "шт", RecipeId = 6 },
                    new Ingredient { Id = 32, Name = "Морковь", Amount = 1, Unit = "шт", RecipeId = 6 },
                    new Ingredient { Id = 33, Name = "Лук", Amount = 1, Unit = "шт", RecipeId = 6 },
                    new Ingredient { Id = 34, Name = "Говядина", Amount = 300, Unit = "г", RecipeId = 6 },
                    new Ingredient { Id = 35, Name = "Томатная паста", Amount = 2, Unit = "ст.л", RecipeId = 6 },
                    new Ingredient { Id = 36, Name = "Сметана", Amount = 50, Unit = "г", RecipeId = 6 },
                    new Ingredient { Id = 37, Name = "Уксус", Amount = 1, Unit = "ч.л", RecipeId = 6 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 24, StepNumber = 1, Description = "Отварить говядину до готовности (около 1 часа)", RecipeId = 6 },
                    new CookingStep { Id = 25, StepNumber = 2, Description = "Натереть свеклу и морковь на терке", RecipeId = 6 },
                    new CookingStep { Id = 26, StepNumber = 3, Description = "Пассеровать лук, морковь и свеклу с томатной пастой", RecipeId = 6 },
                    new CookingStep { Id = 27, StepNumber = 4, Description = "Добавить уксус к свекле для сохранения цвета", RecipeId = 6 },
                    new CookingStep { Id = 28, StepNumber = 5, Description = "Нарезать картофель и капусту, добавить в бульон", RecipeId = 6 },
                    new CookingStep { Id = 29, StepNumber = 6, Description = "Варить 20 минут, затем добавить зажарку", RecipeId = 6 },
                    new CookingStep { Id = 30, StepNumber = 7, Description = "Подавать со сметаной и зеленью", RecipeId = 6 }
                }
            },
            new Recipe()
            {
                Id = 7,
                Name = "Фирменные сырники",
                Description = "Пышные сырники с ванильным ароматом",
                CookingTime = 30,
                Calories = 240,
                Proteins = 14,
                Fats = 12,
                Carbohydrates = 18,
                UserId = 2, // Другой пользователь
                IsSystemRecipe = false, // Это пользовательский рецепт
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 38, Name = "Творог", Amount = 400, Unit = "г", RecipeId = 7 },
                    new Ingredient { Id = 39, Name = "Яйца", Amount = 2, Unit = "шт", RecipeId = 7 },
                    new Ingredient { Id = 40, Name = "Мука", Amount = 4, Unit = "ст.л", RecipeId = 7 },
                    new Ingredient { Id = 41, Name = "Сахар", Amount = 3, Unit = "ст.л", RecipeId = 7 },
                    new Ingredient { Id = 42, Name = "Ванильный сахар", Amount = 1, Unit = "ч.л", RecipeId = 7 },
                    new Ingredient { Id = 43, Name = "Соль", Amount = 1, Unit = "щепотка", RecipeId = 7 },
                    new Ingredient { Id = 44, Name = "Масло для жарки", Amount = 50, Unit = "мл", RecipeId = 7 },
                    new Ingredient { Id = 45, Name = "Сметана", Amount = 100, Unit = "г", RecipeId = 7 }
                },
                CookingSteps = new List<CookingStep>
                {
                    new CookingStep { Id = 31, StepNumber = 1, Description = "Протереть творог через сино", RecipeId = 7 },
                    new CookingStep { Id = 32, StepNumber = 2, Description = "Добавить яйца, сахар, ванильный сахар и соль", RecipeId = 7 },
                    new CookingStep { Id = 33, StepNumber = 3, Description = "Постепенно добавлять муку, чтобы тесто не было липким", RecipeId = 7 },
                    new CookingStep { Id = 34, StepNumber = 4, Description = "Сформировать сырники и обвалять в муке", RecipeId = 7 },
                    new CookingStep { Id = 35, StepNumber = 5, Description = "Обжарить на среднем огне до золотистой корочки", RecipeId = 7 },
                    new CookingStep { Id = 36, StepNumber = 6, Description = "Подавать со сметаной или вареньем", RecipeId = 7 }
                }
            }
        };


        public static List<Recipe> GetUserRecipes(int userId)
        {
            return Recipes.Where(r => r.UserId == userId && !r.IsSystemRecipe).ToList();
        }


        public static List<Recipe> GetSystemRecipes()
        {
            return Recipes.Where(r => r.IsSystemRecipe).ToList();
        }
    }
}
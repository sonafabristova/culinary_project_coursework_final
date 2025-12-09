using System.Configuration;
using System.Data;
using System.Windows;
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Тест подключения к БД
            TestDatabaseConnection();

            Autor authWindow = new Autor();
            authWindow.Show();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var db = new ForCwContext())
                {
                    // Проверяем, можем ли подключиться к базе
                    bool canConnect = db.Database.CanConnect();

                    if (canConnect)
                    {
                        var userCount = db.Пользователиs.Count();
                        var recipeCount = db.Рецептыs.Count();

                        Console.WriteLine($"Подключение успешно! Пользователей: {userCount}, Рецептов: {recipeCount}");
                        MessageBox.Show($"Подключение к БД успешно!\nПользователей: {userCount}\nРецептов: {recipeCount}");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось подключиться к базе данных");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД");
            }
        }
    }

}

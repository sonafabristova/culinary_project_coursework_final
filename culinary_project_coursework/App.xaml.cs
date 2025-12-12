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

          
            TestDatabaseConnection();

            Autor authWindow = new Autor();
            authWindow.Show();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var db = new WithIngContext())
                {
          
                    bool canConnect = db.Database.CanConnect();

                    if (canConnect)
                    {
                        var userCount = db.Пользователиs.Count();
                        var recipeCount = db.Рецептыs.Count();

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

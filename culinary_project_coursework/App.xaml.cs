using System.Configuration;
using System.Data;
using System.Windows;

namespace culinary_project_coursework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Autor authWindow = new Autor();
            authWindow.Show();
        }
    }

}

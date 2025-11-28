using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using culinary_project_coursework.Classes;

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Логика взаимодействия для Autor.xaml
    /// </summary>
    public partial class Autor : Window
    {
        public Autor()
        {
            InitializeComponent();
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxLogin.Text) && !string.IsNullOrWhiteSpace(BoxPass.Text))
            {
                User? authUser = AppContext.Users.FirstOrDefault(q => q.Login == BoxLogin.Text && q.Password == BoxPass.Text);

                if (authUser != null)
                {
                    // ВАЖНО: Устанавливаем текущего пользователя в AppContext
                    AppContext.CurrentUser = authUser;

                    MainWindow mainWindow = new MainWindow(authUser);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();
            regWindow.ShowDialog();
        }
    }
}


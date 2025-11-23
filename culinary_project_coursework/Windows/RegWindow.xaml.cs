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

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        

        public User RegUser { get; private set; }

        public RegWindow()
        {
            InitializeComponent();
        }

        private void ButtonCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BoxLogin.Text) && !string.IsNullOrEmpty(BoxPass.Text) &&
                !string.IsNullOrEmpty(BoxName.Text) && !string.IsNullOrEmpty(BoxPhoneNumber.Text))
            {
                if (AppContext.Users.Any(u => u.Login == BoxLogin.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }

                RegUser = new User()
                {
                    Id = AppContext.Users.Count,
                    Login = BoxLogin.Text,
                    Password = BoxPass.Text,
                    Name = BoxName.Text,
                    PhoneNumber = BoxPhoneNumber.Text,
                    
                };

                AppContext.Users.Add(RegUser);

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}


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
using culinary_project_coursework.Models;

namespace culinary_project_coursework.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void ButtonCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BoxLogin.Text) && !string.IsNullOrEmpty(BoxPass.Text) &&  !string.IsNullOrEmpty(BoxName.Text) && !string.IsNullOrEmpty(BoxPhoneNumber.Text))
            {
                if (AppContext.RegisterUser(BoxName.Text, BoxLogin.Text, BoxPass.Text, BoxPhoneNumber.Text))
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                }
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
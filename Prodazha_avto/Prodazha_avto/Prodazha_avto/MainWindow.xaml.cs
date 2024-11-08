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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prodazha_avto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var currentUser = App.Context.CarUser.FirstOrDefault(p => p.login == LoginTextBox.Text && p.password == PasswordBox.Password);
                

                if(currentUser != null)
                {
                    switch (currentUser.role)
                    {
                        case 1:
                            App.CurrentUser = currentUser;
                            Pokupatel pokupatel = new Pokupatel();
                            pokupatel.Show();
                            break;

                        case 2:
                            App.CurrentUser = currentUser;
                            Prodavec prodavec = new Prodavec();
                            prodavec.Show();
                            break;
                        default:
                            MessageBox.Show("Неизвестный пользователь");
                            break;
                    }

                }
                else 
                {
                    MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoginTextBox.Text = "";
            }
            catch 
            {
                MessageBox.Show("Ошибка подключения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Открыть форму регистрации
            Registration registrationPage = new Registration();
            registrationPage.Show();
        }
    }
}

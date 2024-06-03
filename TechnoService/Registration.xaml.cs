using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechnoService.Models;

namespace TechnoService
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void ToAuthoButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegistrationClick(object sender, RoutedEventArgs e)
        {
            Check();
            if ((NameBox.Background == Brushes.GreenYellow) && (PhoneBox.Background == Brushes.GreenYellow))
            {
                string name = NameBox.Text;
                string phone = PhoneBox.Text;
                string login = LoginBox.Text;
                string password = PasswordBox.Password; // Для PasswordBox используйте Password свойство

                // Валидация данных (если необходимо)
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                // Вызов метода для регистрации пользователя
                try
                {
                    DBManager.RegisterUser(name, phone, login, password);
                    MessageBox.Show("Регистрация успешна.");
                    // Можно закрыть окно регистрации или очистить поля
                    MainWindow mainWindow2 = new MainWindow();
                    mainWindow2.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
                }
            }
        }

        public void Check()
        {
            string name = NameBox.Text.Trim();
            string pattern = "^[a-zA-Zа-яА-Я ]+$";

            if (!Regex.IsMatch(name, pattern))
            {
                NameBox.Background = Brushes.IndianRed;
                NameBox.ToolTip = "Введите ФИО, используя только буквы";
            }
            else
            {
                NameBox.Background = Brushes.GreenYellow;
                LoginBox.Background = Brushes.GreenYellow;
                PasswordBox.Background = Brushes.GreenYellow;
            }

            string phone_number = PhoneBox.Text.Trim();
            string phone_pattern = "^8[0-9]{10}$";

            if (!Regex.IsMatch(phone_number, phone_pattern))
            {
                PhoneBox.Background = Brushes.IndianRed;
                PhoneBox.ToolTip = "Используйте формат номера 80000000000";
            }
            else
            {
                PhoneBox.Background = Brushes.GreenYellow;
                LoginBox.Background = Brushes.GreenYellow;
                PasswordBox.Background = Brushes.GreenYellow;
            }
        }

        
    }
}

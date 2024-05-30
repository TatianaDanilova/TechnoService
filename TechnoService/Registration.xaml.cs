using System;
using System.Collections.Generic;
using System.Linq;
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
            if ((NameBox.Background == Brushes.Transparent) && (PhoneBox.Background == Brushes.Transparent))
            {
                Operator oper = new Operator();
                oper.Show();
                this.Close();
            }
        }

        public void Check()
        {
            string name = NameBox.Text.Trim();
            string pattern = "^[a-zA-Zа-яА-Я]+$";

            if (!Regex.IsMatch(name, pattern))
            {
                NameBox.Background = Brushes.IndianRed;
                NameBox.ToolTip = "Введите ФИО, используя только буквы";
            }
            else
            {
                NameBox.Background = Brushes.Transparent;
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
                PhoneBox.Background = Brushes.Transparent;
            }
        }

        
    }
}

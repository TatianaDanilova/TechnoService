using TechnoService.Models;
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

namespace TechnoService
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Helper db = new Helper();
        }

        private void ToRegButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void EntryClick(object sender, RoutedEventArgs e)
        {
            String login = LoginBox.Text;
            String password = PasswordBox.Password;
            Int32 UserId = DBManager.AuthenticateUser(login, password);
            CurrentUser.userId = UserId;
            if (UserId >= 0)
            {
                User user = DBManager.GetUserById(UserId);
                CurrentUser.fio = user.FIO;

                CurrentUser.phone = user.Phone;
                CurrentUser.login = user.Login;
                CurrentUser.password = user.Password;
                CurrentUser.type = user.Type;

                if (user.Type == "Заказчик")
                {
                    Client profile = new Client();
                    profile.Show();
                    this.Hide();
                }
                if (user.Type == "Оператор")
                {
                    Operator profile = new Operator();
                    profile.Show();
                    this.Hide();
                }
                if (user.Type == "Мастер")
                {
                    Master profile = new Master();
                    profile.Show();
                    this.Hide();
                }
                if (user.Type == "Менеджер")
                {
                    Manager profile = new Manager();
                    profile.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}

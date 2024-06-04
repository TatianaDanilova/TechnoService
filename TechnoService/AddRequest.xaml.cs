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
using TechnoService.Models;

namespace TechnoService
{
    /// <summary>
    /// Логика взаимодействия для AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Window
    {
        public AddRequest()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            string techType = TypeBox.Text;
            string model = ModelBox.Text;
            string description = DescriptionBox.Text;
            int clientId = CurrentUser.userId; // Предполагается, что CurrentUser.UserId доступен и содержит ID текущего пользователя

            // Валидация данных (если необходимо)
            if (string.IsNullOrWhiteSpace(techType) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            // Вызов метода для добавления заявки
            try
            {
                DBManager.AddRequest(techType, model, description, clientId);
                MessageBox.Show("Заявка успешно добавлена.");
                // Можно закрыть окно добавления заявки или очистить поля
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заявки: {ex.Message}");
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

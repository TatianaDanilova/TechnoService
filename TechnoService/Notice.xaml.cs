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
    /// Логика взаимодействия для Notice.xaml
    /// </summary>
    public partial class Notice : Window
    {
        public Notice()
        {
            InitializeComponent();
            // Получение списка уведомлений для текущего пользователя (clientId)
            int clientId = CurrentUser.userId; // Предполагается, что вы храните информацию о текущем пользователе в объекте CurrentUser
            List<Notification> notifications = DBManager.GetNotificationsByClientId(clientId);

            // Привязка списка уведомлений к источнику данных для DataGrid
            NoticeList.ItemsSource = notifications;

        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

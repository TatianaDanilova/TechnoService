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

namespace TechnoService
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        public Client()
        {
            InitializeComponent();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddRequest addRequest = new AddRequest();
            addRequest.Show();
        }

        private void NoticeClick(object sender, RoutedEventArgs e)
        {
            Notice notice = new Notice();
            notice.Show();
        }

        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
        }
    }
}

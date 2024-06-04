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
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public Manager()
        {
            InitializeComponent();
            CompleteRequestBox.Text = DBManager.GetNumberOfDoneRequests().ToString();
            AverageTimeBox.Text = DBManager.GetAverageTime().ToString();
        }

        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            string type = TypeBox.Text;
            RequestBox.Text = DBManager.GetNumberOfDoneRequestsByType(type).ToString();
            AverageBox.Text = DBManager.GetMidTimeOfDoneRequestsByType(type).ToString();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

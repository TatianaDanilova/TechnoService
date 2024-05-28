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
    /// Логика взаимодействия для Operator.xaml
    /// </summary>
    public partial class Operator : Window
    {
        public Operator()
        {
            InitializeComponent();
        }

        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
        }
    }
}

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
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();
            TypeBox.Text = CurrentRequest.techType;
            ModelBox.Text = CurrentRequest.model;
            DescriptoinBox.Text = CurrentRequest.problemDescription;
            DetailBox.Text = CurrentRequest.repairPart;
            StatusBox.Text = CurrentRequest.status;
            StartDateBox.Text = CurrentRequest.startDate;
            FinishDateBox.Text = CurrentRequest.endDate;
            MasterBox.Text = CurrentRequest.master;
            ClientBox.Text = CurrentRequest.client;
            CommentBox.Text = CurrentRequest.comment;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

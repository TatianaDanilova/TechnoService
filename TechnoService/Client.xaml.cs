using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
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

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedStatus = selectedItem.Content.ToString();
                int clientId = CurrentUser.userId;

                DataTable requests = DBManager.GetRequests(selectedStatus, clientId);
                RequestStatus.ItemsSource = requests.DefaultView;
            }
        }
        private void RequestStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestStatus.SelectedItem is DataRowView rowView)
            {
                int requestId = Convert.ToInt32(rowView["RequestId"]);
                Request request = DBManager.GetRequestById(requestId);
                User client = DBManager.GetClientByRequestId(requestId);
                User master = DBManager.GetMasterByRequestId(requestId);
                Comment comment = DBManager.GetCommentsByRequestId(requestId);
                CurrentRequest.requestId = requestId;

                if (request != null)
                {
                    CurrentRequest.startDate = request.StartDate.ToString();
                    CurrentRequest.techType = request.TechType;
                    CurrentRequest.model = request.Model;
                    CurrentRequest.problemDescription = request.ProblemDescription;
                    CurrentRequest.status = request.Status;
                    CurrentRequest.endDate = request.EndDate.ToString();
                    CurrentRequest.repairPart = request.RepairPart;
                    CurrentRequest.client = client.FIO;
                    CurrentRequest.master = "";
                    CurrentRequest.comment = "";
                }
                if (master != null)
                {
                    CurrentRequest.master = master.FIO;
                }
                if (comment != null)
                {
                    CurrentRequest.comment = comment.Message;
                }
            }
            Info info = new Info();
            info.Show();
        }
    }
}

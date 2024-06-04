using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Operator.xaml
    /// </summary>
    public partial class Operator : Window
    {
        public List<User> Masters { get; set; }
        public List<Request> Requests { get; set; }

        public Operator()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            Masters = DBManager.GetMasters();
            Requests = DBManager.GetAllRequests(); // Используем новый метод
            RequestMasterGrid.ItemsSource = Requests;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is User selectedMaster && RequestMasterGrid.SelectedItem is Request selectedRequest)
            {
                try
                {
                    DBManager.UpdateRequestMaster(selectedRequest.RequestId, selectedMaster.UserId);
                    selectedRequest.MasterId = selectedMaster.UserId; // Обновляем данные в объекте Request
                    MessageBox.Show("Мастер назначен успешно.");
                    User client = DBManager.GetClientByRequestId(selectedRequest.RequestId);
                    int clientid = client.UserId;
                    int  requestId = selectedRequest.RequestId;
                    string message = $"Заявке №{selectedRequest.RequestId} назначен специалист";
                    DBManager.AddNotification(clientid, message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при назначении мастера: {ex.Message}");
                }
            }
            
        }

        private void RequestStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestMasterGrid.SelectedItem is Request selectedRequest)
            {
                MessageBox.Show($"RequestId: {selectedRequest.RequestId}", "Debug Info");

                int requestId = selectedRequest.RequestId;
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
                    CurrentRequest.client = client?.FIO ?? "";
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

        
    }
}

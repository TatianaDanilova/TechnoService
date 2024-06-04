using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechnoService.Models;
using System.ComponentModel.Design;

namespace TechnoService
{
    /// <summary>
    /// Логика взаимодействия для Master.xaml
    /// </summary>
    public partial class Master : Window
    {
        public Master()
        {
            InitializeComponent();
            LoadRequests();
           
        }

        private void LoadRequests()
        {
            int currentMasterId = CurrentUser.userId; 
            var requests = DBManager.LoadRequestsForCurrentMaster(currentMasterId);

            RequestStatusMaster.ItemsSource = requests;
        }

        private void RequestStatusMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.Row.Item is Request editedRequest)
            {
                if (e.Column.Header.ToString() == "Комментарий")
                {
                    var textBox = e.EditingElement as TextBox;
                    string newComment = textBox.Text;

                    if (editedRequest.Comment == null)
                    {
                        // Создаем новый комментарий, если его еще нет
                        Comment comment = new Comment
                        {
                            Message = newComment,
                            RequestId = editedRequest.RequestId,
                            MasterId = CurrentUser.userId, // Возможно, вам нужно определить MasterId
                            CommentDate = DateTime.Now // Устанавливаем текущую дату
                        };

                        // Добавляем комментарий в базу данных
                        DBManager.AddComment(comment);

                        MessageBox.Show("Комментарий успешно добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        Comment comment = DBManager.GetCommentsByRequestId(editedRequest.RequestId);
                        int commentId = comment.CommentId;
                        DBManager.UpdateComment(commentId, newComment);
                        MessageBox.Show("Комментарий успешно изменен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                if (e.Column.Header.ToString() == "Сломанная деталь")
                {
                    var textBox = e.EditingElement as TextBox;
                    string newRepairPart = textBox.Text;
                    DBManager.UpdateRepairPart(editedRequest.RequestId, newRepairPart);
                    MessageBox.Show("Сломанная деталь успешно добавлена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (e.Column.Header.ToString() == "Статус")
                {
                    // Получаем текущую дату и время
                    DateTime currentDateTime = DateTime.Now;
                    var textBox = e.EditingElement as TextBox;
                    string newStatus = textBox.Text;
                    // Проверяем, что статус изменился на "Готов к выдаче"
                    DBManager.UpdateStatus(editedRequest.RequestId, newStatus);
                    editedRequest.Status = newStatus; // Обновляем данные в объекте Request

                    MessageBox.Show("Статус успешно изменен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (newStatus == "Готов к выдаче")
                    {
                        // Получаем идентификатор заявки
                        int requestId = editedRequest.RequestId;

                        // Обновляем поле EndDate в таблице Requests
                        DBManager.UpdateEndDate(requestId, currentDateTime);
                    }
                }
            }
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

        private void RequestStatusMaster_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestStatusMaster.SelectedItem is Request selectedRequest)
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
    }
}

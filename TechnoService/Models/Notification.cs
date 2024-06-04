using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int ClientId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}

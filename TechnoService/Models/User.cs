using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Type { get; set; } // Заказчик, Оператор, Мастер, Менеджер
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public string TechType { get; set; }
        public string Model { get; set; }
        public string ProblemDescription { get; set; }
        public string Status { get; set; } // Новая заявка, В процессе ремонта, Готов к выдаче
        public DateTime? EndDate { get; set; }
        public string RepairPart { get; set; }
        public int? MasterId { get; set; }
        public int ClientId { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public string Message { get; set; }
        public Comment Comment { get; set; }

        public User Master { get; set; }
        public User Client { get; set; }
    }
}

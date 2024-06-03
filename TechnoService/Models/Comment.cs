using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoService.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Message { get; set; }
        public int RequestId { get; set; }
        public int MasterId { get; set; }
        public DateTime CommentDate { get; set; }

        public Request Request { get; set; }
        public User Master { get; set; }
    }
}

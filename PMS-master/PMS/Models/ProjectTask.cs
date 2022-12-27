using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}

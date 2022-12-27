using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class StudentRoom
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
    }
}

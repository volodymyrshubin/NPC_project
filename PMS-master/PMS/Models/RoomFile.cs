using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class RoomFile
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int StudentId { get; set; }
        public string FileName { get; set; }
        public string FilePth { get; set; }
    }
}

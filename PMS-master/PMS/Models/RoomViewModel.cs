using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int StudentRoomId { get; set; }
        public string ProjectName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Year { get; set; }
        
        public string ProjectType { get; set; }
        
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public List<SelectListItem> drpStudents { get; set; }
       
        public long[] StudentIds { get; set; }
    }
}

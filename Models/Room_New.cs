using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public string Availability { get; set; }
        public string Features { get; set; }
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }

        public virtual Hotel Hotel { get; set; } // Navigation property

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class Booking
    {

        public int BookingID { get; set; }          
        public int UserID { get; set; }           
        public int RoomID { get; set; }           
        public DateTime CheckInDate { get; set; } 
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = "Active"; 
        public int PaymentID { get; set; }

    }
}

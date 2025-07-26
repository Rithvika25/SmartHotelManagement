using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.DTOs
{
    public class BookingDetailsDto
    {
        public int BookingID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
    }
}

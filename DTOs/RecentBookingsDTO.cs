using SmartHotelBookingSystem.Models;

namespace SmartHotelBookingSystem.DTOs
{
    public class RecentBookingsDto
    {
        public int BookingID { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public Room Room { get; set; }
    }

}
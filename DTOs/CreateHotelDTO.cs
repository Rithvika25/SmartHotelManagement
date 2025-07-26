using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.DTOs
{
   public  class CreateHotelDTO
    {
        public int HotelID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ManagerID { get; set; }
        public string Amenities { get; set; }
        public double Rating { get; set; }
        public string ImageURL { get; set; }
    }
}

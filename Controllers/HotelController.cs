using SmartHotelBookingSystem.BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.Models;
using SmartHotelBookingSystem.DTOs;
using System.Collections.Generic;
using System;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelBLL _hotelLogic;

        public HotelController(HotelBLL hotelLogic)
        {
            _hotelLogic = hotelLogic;
        }

        [HttpGet]
        public IActionResult GetHotels()
        {
            var hotels = _hotelLogic.GetAllHotels();
            return hotels.Count > 0 ? Ok(hotels) : NotFound("No hotels found.");
        }

        [HttpGet("{id}")]
        public IActionResult GetHotel(int id)
        {
            var hotels = _hotelLogic.GetAllHotels();
            var hotel = hotels.Find(h => h.HotelID == id);
            return hotel != null ? Ok(hotel) : NotFound("Hotel not found.");
        }

        [HttpPost]
        public IActionResult CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            var hotel = new Hotel
            {
                HotelID = hotelDTO.HotelID,
                Name = hotelDTO.Name,
                Location = hotelDTO.Location,
                ManagerID = hotelDTO.ManagerID,
                Amenities = hotelDTO.Amenities,
                Rating = hotelDTO.Rating,
                ImageURL = hotelDTO.ImageURL
            };

            if (hotel.Rating < 0 || hotel.Rating > 5)
                return BadRequest("Rating must be between 0 and 5.");

            int result = _hotelLogic.InsertHotel(hotel);
            return result > 0 ? Ok(hotel) : StatusCode(500, "Error creating hotel.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHotel([FromBody] UpdateHotelDTO updateHotel, int id)
        {
            var hotel = new Hotel
            {
                Name = updateHotel.Name,
                Location = updateHotel.Location,
                ManagerID = updateHotel.ManagerID,
                Amenities = updateHotel.Amenities,
                Rating = updateHotel.Rating,
                IsActive = updateHotel.IsActive,
                ImageURL = updateHotel.ImageURL
            };

            if (hotel.Rating < 0 || hotel.Rating > 5)
                return BadRequest("Rating must be between 0 and 5.");

            int result = _hotelLogic.UpdateHotel(hotel, id);
            return result > 0 ? Ok(hotel) : StatusCode(500, "Error updating hotel.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHotel(int id)
        {
            int deleteStatus = _hotelLogic.DeleteHotel(id);
            return deleteStatus > 0 ? Ok("Hotel deleted successfully.") : NotFound("Hotel not found.");
        }

        //[HttpGet("filter/rating")]
        //public IActionResult FilterHotelsByRating([FromQuery] double minRating, [FromQuery] double maxRating)
        //{
        //    var hotels = _hotelLogic.FilterHotelsByRating(minRating, maxRating);
        //    return hotels.Count > 0 ? Ok(hotels) : NotFound("No hotels found within this rating range.");
        //}


        [HttpGet("filter/amenities")]
        public IActionResult FilterHotelsByAmenities([FromQuery] string amenities)
        {
            var hotels = _hotelLogic.FilterHotelsByAmenities(amenities);
            return hotels.Count > 0 ? Ok(hotels) : NotFound("No hotels found with these amenities.");
        }

        [HttpGet("availability")]
        public IActionResult GetHotelsByAvailability([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var hotels = _hotelLogic.ReadHotelsByAvailability(startDate, endDate);
            return hotels.Count > 0 ? Ok(hotels) : NotFound("No available hotels found for selected dates.");
        }
    }
}

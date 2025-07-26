using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.Models;
using SmartHotelBookingSystem.DTOs;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHotelBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository _repository;

        public BookingController(BookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingDTO booking)
        {
            if (booking == null)
                return BadRequest("Invalid booking data.");

            _repository.AddBooking(booking);
            return Ok("Booking added successfully.");
        }


        [HttpGet]
        public ActionResult<List<Booking>> GetAllBookings()
        {
            var bookings = _repository.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetRecentBookings(int id)
        {
            var bookings = _repository.GetBookingsByBookingID(id);

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No recent bookings found for this user.");
            }

            return Ok(JsonConvert.SerializeObject(bookings, Formatting.Indented));
        }

        [HttpGet("recentBookings/{id}")] // ✅ Unique route
        public IActionResult GetRecentBookingsOfUser(int id)
        {
            var bookings = _repository.GetRecentBookings(id);

            if (bookings == null || bookings.Rows.Count == 0) // ✅ Properly checks for empty data
            {
                return NotFound("No recent bookings found for this user.");
            }

            return Ok(JsonConvert.SerializeObject(bookings, Formatting.Indented));
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] DateTime checkInDate)
        {
            try
            {
                _repository.UpdateBooking(id, checkInDate);
                return Ok("Booking updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                _repository.DeleteBooking(id);
                return Ok("Booking deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getBookingId/{userId}/{roomId}/{hotelId}")]
        public IActionResult GetBookingId(int userId, int roomId, int hotelId)
        {
            int? bookingId = _repository.GetBookingId(userId, roomId, hotelId);

            if (bookingId == null)
            {
                return NotFound("No booking found for the given criteria.");
            }

            return Ok(new { BookingID = bookingId });
        }

    }
}
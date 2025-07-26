using SmartHotelBookingSystem.BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.Models;
using System.Collections.Generic;
using System.Data;
using SmartHotelBookingSystem.DataAccess.ADO;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomBLL _roomBLL;

        public RoomController(DB1 dalObject)
        {
            _roomBLL = new RoomBLL(dalObject);
        }

        // GET: api/Room
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetRooms()
        {
            try
            {
                var dataTable = _roomBLL.FetchAllActiveRooms();

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return NotFound("No rooms found in the database.");

                var roomList = _roomBLL.ConvertDataTableToList(dataTable); // Fix: Convert to List<Room>
                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Room/{id} (Fetch Room by RoomID)
        //[HttpGet("{id}")]
        //public ActionResult<Room> GetRoomById(int id)
        //{
        //    try
        //    {
        //        var dataTable = _roomBLL.(id); // Fix: Fetch Room by RoomID

        //        if (dataTable == null || dataTable.Rows.Count == 0)
        //            return NotFound($"Room with ID {id} not found.");

        //        var roomList = _roomBLL.ConvertDataTableToList(dataTable);
        //        return Ok(roomList[0]); // Return single room object
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        // POST: api/Room
        [HttpPost]
        public ActionResult CreateRoom([FromBody] Room newRoom)
        {
            try
            {
                int result = _roomBLL.InsertRoom(newRoom);

                if (result > 0)
                    return Ok("Room created successfully.");
                else
                    return BadRequest("Failed to create room.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Room/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            try
            {
                updatedRoom.RoomID = id;
                int result = _roomBLL.UpdateRoom(updatedRoom);

                if (result > 0)
                    return Ok("Room updated successfully.");
                else
                    return NotFound($"Room with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Room/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteRoom(int id)
        {
            try
            {
                int deleteStatus = _roomBLL.DeleteRoom(id);

                if (deleteStatus > 0)
                    return Ok("Room deleted successfully.");
                else
                    return NotFound($"Room with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("updateAvailability/{roomID}")]
        public IActionResult UpdateRoomAvailability(int roomID)
        {
            if (roomID <= 0)
            {
                return BadRequest("Invalid Room ID.");
            }

            var result = _roomBLL.UpdateRoomAvailabilityToBooked(roomID);

            if (result > 0)
            {
                return Ok(new { message = $"Room {roomID} has been successfully booked." });
            }
            else
            {
                return StatusCode(500, $"Failed to update room {roomID}. Please try again.");
            }
        }
        // GET: api/Room/Hotel/{hotelID}
        [HttpGet("Hotel/{hotelID}")]
        public ActionResult<IEnumerable<Room>> GetRoomsByHotel(int hotelID)
        {
            try
            {
                var dataTable = _roomBLL.FetchRoomsByHotel(hotelID);

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return NotFound($"No rooms found for Hotel ID {hotelID}.");

                var roomList = _roomBLL.ConvertDataTableToList(dataTable); // Fix: Convert to List<Room>
                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}



using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.DTOs;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBooking(BookingDTO bookingDto)
        {
            var booking = new Booking
            {
                UserID = bookingDto.UserID,
                RoomID = bookingDto.RoomID,
                CheckInDate = bookingDto.CheckInDate,
                CheckOutDate = bookingDto.CheckOutDate,
                Status = bookingDto.Status,
                PaymentID = bookingDto.PaymentID
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }


        public List<Booking> GetAllBookings()
        {
            return _context.Bookings.ToList();
        }

        public List<BookingDetailsDto> GetBookingsByBookingID(int bookingID)
        {
            DB1 db = new DB1();

            string query = @"
                SELECT 
                    B.BookingID,
                    B.CheckInDate,
                    B.CheckOutDate,
                    H.Name AS HotelName,
                    R.Type AS RoomType,
                    B.Status,
                    R.Price AS TotalCost
                FROM [SmartHotelDB].[dbo].[Bookings] B
                JOIN [SmartHotelDB].[dbo].[Room] R ON B.RoomID = R.RoomID
                JOIN [SmartHotelDB].[dbo].[Hotel] H ON R.HotelID = H.HotelID
                WHERE B.BookingID = @BookingID";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BookingID", bookingID)
            };

            DataTable dt = db.FetchData(query, parameters);
            return ConvertDataTableToBookingList(dt);
        }

        private List<BookingDetailsDto> ConvertDataTableToBookingList(DataTable dataTable)
        {
            List<BookingDetailsDto> resultList = new List<BookingDetailsDto>();

            foreach (DataRow row in dataTable.Rows)
            {
                var booking = new BookingDetailsDto
                {
                    BookingID = Convert.ToInt32(row["BookingID"]),
                    CheckInDate = Convert.ToDateTime(row["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(row["CheckOutDate"]),
                    HotelName = row["HotelName"].ToString(),
                    RoomType = row["RoomType"].ToString(),
                    Status = row["Status"].ToString(),
                    TotalCost = Convert.ToDecimal(row["TotalCost"])
                };

                resultList.Add(booking);
            }

            return resultList;
        }

        public void UpdateBooking(int bookingID, DateTime checkInDate)
        {
            var existingBooking = _context.Bookings.Find(bookingID);
            if (existingBooking != null)
            {
                existingBooking.CheckInDate = checkInDate;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Booking not found.");
            }
        }
        public DataTable GetRecentBookings(int userId)
        {
            DB1 db = new DB1();

            string query = @"
        SELECT 
            B.BookingID,
            H.Name AS HotelName,
            R.Type AS RoomType,
            B.CheckInDate AS CheckInDate,
            B.CheckOutDate AS CheckOutDate,
            R.Price AS TotalCost,
            B.Status
        FROM [SmartHotelDB].[dbo].[Bookings] B
        JOIN [SmartHotelDB].[dbo].[Room] R ON B.RoomID = R.RoomID
        JOIN [SmartHotelDB].[dbo].[Hotel] H ON R.HotelID = H.HotelID
        WHERE B.UserID = @userId";

            // Creating the parameter list
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@userId", userId)
             };

            // Execute the query and return DataTable
            DataTable dt = db.FetchData(query, parameters);
            return dt;
        }




        public void DeleteBooking(int bookingID)
        {
            var bookingToDelete = _context.Bookings.Find(bookingID);
            if (bookingToDelete != null)
            {
                _context.Bookings.Remove(bookingToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Booking not found.");
            }
        }
        public int? GetBookingId(int userId, int roomId, int hotelId)
        {
            DB1 db = new DB1();

            string query = @"
        SELECT B.BookingID
        FROM [SmartHotelDB].[dbo].[Bookings] B
        JOIN [SmartHotelDB].[dbo].[Room] R ON B.RoomID = R.RoomID
        WHERE B.UserID = @UserID AND B.RoomID = @RoomID AND R.HotelID = @HotelID";

            // Creating parameter list
            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@UserID", userId),
        new SqlParameter("@RoomID", roomId),
        new SqlParameter("@HotelID", hotelId)
    };

            // Fetch data from database
            DataTable dt = db.FetchData(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["BookingID"]);  // ✅ Returns BookingID if found
            }

            return null; // ✅ Return null if no booking exists
        }

        public List<Dictionary<string, object>> ConvertDataTableToList(DataTable dataTable)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowData[column.ColumnName] = row[column];  // ✅ Convert each row to dictionary
                }

                resultList.Add(rowData);
            }

            return resultList;
        }

    }
}
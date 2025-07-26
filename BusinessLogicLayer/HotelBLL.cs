using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class HotelBLL
    {
        private readonly DB1 _dalObject;

        public HotelBLL(DB1 dalObject)
        {
            _dalObject = dalObject;
        }

        // ✅ Fixed: Insert method returns an int (affected rows) instead of attempting DataTable conversion
        public int InsertHotel(Hotel hotel)
        {
            if (hotel.Rating < 0 || hotel.Rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            string insertQuery = @"INSERT INTO [SmartHotelDB].[dbo].[Hotel]
                                            ([HotelID], [Name], [Location], [ManagerID], [Amenities], [Rating], [IsActive], [ImageURL])
                                            VALUES (@HotelID, @Name, @Location, @ManagerID, @Amenities, @Rating, 1, @ImageURL)";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", hotel.HotelID),
                new nameValuePair("@Name", hotel.Name),
                new nameValuePair("@Location", hotel.Location),
                new nameValuePair("@ManagerID", hotel.ManagerID),
                new nameValuePair("@Amenities", hotel.Amenities),
                new nameValuePair("@Rating", hotel.Rating),
                new nameValuePair("@ImageURL", hotel.ImageURL)
            };

            return _dalObject.InsertUpdateOrDelete(insertQuery, nvp, false);
        }

        // ✅ Fixed: Update method properly returns affected row count
        public int UpdateHotel(Hotel hotel, int id)
        {
            string updateQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                   SET [Name] = @Name, [Location] = @Location, [ManagerID] = @ManagerID,
                                       [Amenities] = @Amenities, [Rating] = @Rating, [IsActive] = @IsActive, [ImageURL] = @ImageURL
                                   WHERE [HotelID] = @Id";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@Id", id),
                new nameValuePair("@Name", hotel.Name),
                new nameValuePair("@Location", hotel.Location),
                new nameValuePair("@ManagerID", hotel.ManagerID),
                new nameValuePair("@Amenities", hotel.Amenities),
                new nameValuePair("@Rating", hotel.Rating),
                new nameValuePair("@IsActive", hotel.IsActive),
                new nameValuePair("@ImageURL", hotel.ImageURL)
            };

            return _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
        }

        // ✅ Fixed: Delete method correctly returns affected row count
        public int DeleteHotel(int id)
        {
            string deleteHotelQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotel]
                                        SET [IsActive] = 0
                                        WHERE [HotelID] = @HotelID";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", id)
            };

            return _dalObject.InsertUpdateOrDelete(deleteHotelQuery, nvp, false);
        }

        // ✅ Fixed: Convert `DataTable` to `List<Hotel>` for amenity filtering
        public List<Hotel> FilterHotelsByAmenities(string amenity)
        {
            string query = @"
                SELECT *
                FROM [SmartHotelDB].[dbo].[Hotel]
                WHERE [IsActive] = 1
                AND EXISTS (
                    SELECT 1
                    FROM STRING_SPLIT([Amenities], ',') AS amenity
                    WHERE TRIM(amenity.value) = @Amenity
                );";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@Amenity", amenity)
            };

            DataTable dt = _dalObject.FetchData(query, nvp);
            return ConvertDataTableToList(dt);
        }

        // ✅ Fixed: Availability filter correctly returns `List<Hotel>`
        public List<Hotel> ReadHotelsByAvailability(DateTime startDate, DateTime endDate)
        {
            string query = @"SELECT DISTINCT H.*
                             FROM [SmartHotelDB].[dbo].[Hotel] H
                             LEFT JOIN [SmartHotelDB].[dbo].[Room] R ON H.HotelID = R.HotelID
                             LEFT JOIN [SmartHotelDB].[dbo].[Bookings] B ON R.RoomID = B.RoomID
                             WHERE (B.RoomID IS NULL OR (B.CheckOutDate < @startDate OR B.CheckInDate > @endDate))
                             AND H.[IsActive] = 1
                             AND R.[Availability] = 'Available';
                             ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            DataTable dt = _dalObject.FetchData(query, parameters);
            return ConvertDataTableToList(dt);
        }

        // ✅ Fixed: General hotel retrieval returns `List<Hotel>`
        public List<Hotel> GetAllHotels()
        {
            string query = "SELECT * FROM [SmartHotelDB].[dbo].[Hotel] WHERE [IsActive] = 1";
            DataTable dt = _dalObject.FetchData(query);
            return ConvertDataTableToList(dt);
        }

        // ✅ Helper method: Converts DataTable rows to List<Hotel> objects
        public List<Hotel> ConvertDataTableToList(DataTable dataTable)
        {
            var hotelList = new List<Hotel>();
            foreach (DataRow row in dataTable.Rows)
            {
                var hotel = new Hotel
                {
                    HotelID = row.Field<int?>("HotelID") ?? 0,
                    Name = row.Field<string>("Name") ?? string.Empty,
                    Location = row.Field<string>("Location") ?? string.Empty,
                    ManagerID = row.Field<int?>("ManagerID") ?? 0,
                    Amenities = row.Field<string>("Amenities") ?? string.Empty,
                    Rating = row.Field<double?>("Rating") ?? 0,
                    IsActive = row.Field<bool?>("IsActive") ?? false,
                    ImageURL = row.Field<string>("ImageURL") ?? string.Empty
                };
                hotelList.Add(hotel);
            }
            return hotelList;
        }
    }
}

using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class RoomBLL
    {
        private readonly DB1 _dalObject;

        public RoomBLL(DB1 dalObject)
        {
            _dalObject = dalObject;
        }

        #region RoomTable

        public int InsertRoom(Room room)
        {
            string insertQuery = @"INSERT INTO [dbo].[Room]
                                   ([HotelID], [Type], [Price], [Availability], [Features], [IsActive], [ImageURL])
                                   VALUES (@HotelID, @Type, @Price, @Availability, @Features, @IsActive, @ImageURL)";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", room.HotelID),
                new nameValuePair("@Type", room.Type),
                new nameValuePair("@Price", room.Price),
                new nameValuePair("@Availability", room.Availability),
                new nameValuePair("@Features", room.Features),
                new nameValuePair("@IsActive", room.IsActive),
                new nameValuePair("@ImageURL", room.ImageURL)
            };

            try
            {
                return _dalObject.InsertUpdateOrDelete(insertQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error inserting room: " + exp.Message);
                return 0;
            }
        }

        public int UpdateRoom(Room room)
        {
            string updateQuery = @"UPDATE [dbo].[Room]
                                   SET [HotelID]=@HotelID,
                                       [Type] = @Type,
                                       [Price] = @Price,
                                       [Availability] = @Availability,
                                       [Features] = @Features,
                                       [IsActive] = @IsActive,
                                       [ImageURL] = @ImageURL
                                   WHERE [RoomID] = @RoomID";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@HotelID", room.HotelID),
                new nameValuePair("@Type", room.Type),
                new nameValuePair("@Price", room.Price),
                new nameValuePair("@Availability", room.Availability),
                new nameValuePair("@Features", room.Features),
                new nameValuePair("@IsActive", room.IsActive),
                new nameValuePair("@ImageURL", room.ImageURL),
                new nameValuePair("@RoomID", room.RoomID)
            };

            try
            {
                return _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error updating room: " + exp.Message);
                return 0;
            }
        }

        public int DeleteRoom(int roomID)
        {
            string deleteRoomQuery = @"UPDATE [dbo].[Room]
                                       SET [IsActive] = 0
                                       WHERE [RoomID] = @RoomID";

            nameValuePairList nvp = new nameValuePairList
            {
                new nameValuePair("@RoomID", roomID)
            };

            try
            {
                return _dalObject.InsertUpdateOrDelete(deleteRoomQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error deleting room: " + exp.Message);
                return 0;
            }
        }

        public DataTable FetchAllActiveRooms()
        {
            string fetchQuery = @"SELECT * FROM [dbo].[Room] WHERE [IsActive] = 1";

            try
            {
                return _dalObject.FetchData(fetchQuery);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error fetching rooms: " + exp.Message);
                return null;
            }
        }

        public DataTable FetchRoomsByHotel(int hotelID)
        {
            string query = @"SELECT RoomID, HotelID, Type, Price, Availability, Features, IsActive, ImageURL
                     FROM [dbo].[Room]
                     WHERE HotelID = @HotelID AND IsActive = 1";

            var nvp = new nameValuePairList
    {
        new nameValuePair("@HotelID", hotelID) // Fix: Ensure parameter is correctly declared
    };

            try
            {
                return _dalObject.FetchData(query, nvp); // Fetch data using DB1 class
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rooms for Hotel ID {hotelID}: {ex.Message}");
                return null; // Return null in case of error
            }
        }
        public int UpdateRoomAvailabilityToBooked(int roomID)
        {
            string updateQuery = @"UPDATE [dbo].[Room]
                           SET [Availability] = 'Booked'
                           WHERE [RoomID] = @RoomID";

            nameValuePairList nvp = new nameValuePairList
            {
                 new nameValuePair("@RoomID", roomID)
            };

            try
            {
                return _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Error updating room availability for Room ID {roomID}: {exp.Message}");
                return 0;
            }
        }

        public List<Room> ConvertDataTableToList(DataTable dataTable)
        {
            var roomList = new List<Room>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var room = new Room
                    {
                        RoomID = row.Field<int?>("RoomID") ?? 0,
                        HotelID = row.Field<int?>("HotelID") ?? 0,
                        Type = row.Field<string>("Type") ?? string.Empty,

                        Price = (float)(row.Field<Double?>("Price") ?? 0),
                        Availability = row.Field<string>("Availability") ?? string.Empty,
                        Features = row.Field<string>("Features") ?? string.Empty,
                        IsActive = row.Field<bool?>("IsActive") ?? false,
                        ImageURL = row.Field<string>("ImageURL") ?? string.Empty
                    };

                    roomList.Add(room);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error converting DataRow to Room_New: {ex.Message}", ex);
                }
            }

            return roomList;
        }

    }

    #endregion
}


﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Model
{
    public class Room:Base<Room>
    {
        //public int RoomID { get; set; }
        public string Name { get; set; }

        private int _roomTypeID;
        public RoomType RoomType
        {
            get
            {
                if (_roomTypeID == -1)
                    return null;
                return RoomType.Items[_roomTypeID];
            }
            set
            {
                if (value == null)
                    _roomTypeID = -1;
                else
                    _roomTypeID = value.ID;
            }
        }
        //List<Client> GuestPersons { get; set; }
        List<Client> BookedPerson
        {
            get
            {
                var res = new List<Client>();
                foreach (var bk in Booking.Items.Values)
                    if (bk.BookingRoom == this)
                        res.Add(bk.Client);
                return res;
            }
        }
        List<Booking> Bookings
        {
            get
            {
                var res = new List<Booking>();
                foreach (var bk in Booking.Items.Values)
                    if (bk.BookingRoom == this)
                        res.Add(bk);
                return res;
            }
        }

        public override void Insert()
        {
            try
            {
                conn.Open();
                string query = @"insert into tbRoom
                            (Name,_roomTypeID)
                            values (@Name,@_roomTypeID)";
                // 2. define parameters used in command object
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@Name";
                param1.Value = this.Name;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@_roomTypeID";
                param2.Value = this._roomTypeID;
                
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public override List<Room> Retrieve()
        {
            try
            {
                conn.Open();
                string query = @"select * from tbRoom";
                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(query, conn);

                // 2. Call Execute reader to get query results
                SqlDataReader rdr = cmd.ExecuteReader();
                List<Room> list = new List<Room>();
                Items.Clear();
                while (rdr.Read())
                {
                    Room temp = new Room();
                    temp.ID = Convert.ToInt32(rdr[0]);
                    temp.Name = rdr[1].ToString();
                    temp._roomTypeID = Convert.ToInt32(rdr[2]);                    
                    list.Add(temp);
                    //словник об'єктів
                    Items.Add(temp.ID, temp);
                }
                conn.Close();
                return list;
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public override void Update()
        {
            try
            {
                conn.Open();
                // prepare command string
                string query = @"
                update tbRoom
                set Name = @Name,
                    _roomTypeID = @_roomTypeID
                where RoomID = @RoomID";

                // 1. Instantiate a new command with command text only

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@Name";
                param1.Value = this.Name;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@_roomTypeID";
                param2.Value = this._roomTypeID;
                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@RoomID";
                param3.Value = this.ID;

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                // 3. Call ExecuteNonQuery to send command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public override void Delete()
        {
            try
            {
                // Open the connection
                conn.Open();

                // prepare command string
                string query = @"
                 delete from tbRoom
                 where RoomID = @RoomID";
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@RoomID";
                param1.Value = this.ID;
                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add(param1);
                cmd.ExecuteNonQuery();

            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                    //Items.Remove(this.ID);
                }
            }
        }
    }
}

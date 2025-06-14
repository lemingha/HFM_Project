using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Device
    {
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string device_manufacturer { get; set; }
        public string device_seri { get; set; }
        public string device_type { get; set; }
        public string device_group { get; set; }
        public int device_maintenance_cycle { get; set; }
        public DateTime device_maintenance_start { get; set; }
        public DateTime device_stockout_date { get; set; }
        public int device_condition { get; set; }
        public DateTime device_received_date { get; set; }
        public string device_note { get; set; }
        public string FK_contract_id { get; set; }
        public string FK_status_id { get; set; }
        public string FK_room_id { get; set; }

        public string contact_address { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }

        public Device(DataRow row)
        {
            device_id = row["device_id"]?.ToString();
            device_name = row["device_name"]?.ToString();
            device_manufacturer = row["device_manufacturer"]?.ToString();
            device_seri = row["device_seri"]?.ToString();
            device_type = row["device_type"]?.ToString();
            device_group = row["device_group"]?.ToString();

            device_maintenance_cycle = row["device_maintenance_cycle"] != DBNull.Value ? Convert.ToInt32(row["device_maintenance_cycle"]) : 0;
            device_maintenance_start = row["device_maintenance_start"] != DBNull.Value ? Convert.ToDateTime(row["device_maintenance_start"]) : DateTime.MinValue;
            device_stockout_date = row["device_stockout_date"] != DBNull.Value ? Convert.ToDateTime(row["device_stockout_date"]) : DateTime.MinValue;
            device_condition = row["device_condition"] != DBNull.Value ? Convert.ToInt32(row["device_condition"]) : 0;
            device_received_date = row["device_received_date"] != DBNull.Value ? Convert.ToDateTime(row["device_received_date"]) : DateTime.MinValue;
            device_note = row["device_note"]?.ToString();

            FK_status_id = (string)row["FK_status_id"];
            FK_contract_id = (string)row["FK_contact_id"];
            FK_room_id = (string)row["FK_room_id"];

            contact_address = row["contact_address"] != DBNull.Value ? row["contact_address"].ToString() : "";
            room_name = row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            status_name = row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";

        }
    }
}

using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class DeviceDetail
    {
        public List<Status> Status { get; set; }
        public List<Room> rooms { get; set; }
        public List<Contact> contacts { get; set; }
    }
}
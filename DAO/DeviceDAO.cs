using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Hospital_Test.Models;
using System.IO;
using Hospital_Test.DAO;
using Hospital_Test.Models.Login;

namespace Hospital_Test.DAO
{
    public class DeviceDAO
    {
        private static DeviceDAO instance;
        public static DeviceDAO Instance
        {
            get { if (instance == null) instance = new DeviceDAO(); return DeviceDAO.instance; }
            private set { DeviceDAO.instance = value; }
        }
        private DeviceDAO() { }

        public List<Device> GetDeviceList()
        {
            List<Device> items = DataProvider<Device>.Instance.GetListItem("tbl_device");
            return items;
        }

        public Device GetDeviceModelbyId(string ID)
        {
            Device item = DataProvider<Device>.Instance.GetItem("device_id", ID, "tbl_device");
            return item;
        }

        public void AddDevice(Device device)
        {
            DataTable data = DataProvider<Device>.Instance.LoadData();
            DataRow newDevice = data.NewRow();

            var allAttr = typeof(Device).GetProperties(); // Lấy danh sách attributes của class Device

            foreach (var attr in allAttr)
                newDevice[attr.Name] = attr.GetValue(device);


            data.Rows.Add(newDevice);

            DataProvider<Device>.Instance.UpdateData(data);
        }


    }
}

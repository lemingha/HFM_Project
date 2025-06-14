using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Hospital_Test.Models;
using Hospital_Test.DAO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Reflection.Metadata;
using System.Collections;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Configuration;
using DocumentFormat.OpenXml.Office2013.Excel;
using Mono.TextTemplating;
using DocumentFormat.OpenXml.VariantTypes;

namespace Hospital_Test.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("Trangchu");
        }
        public IActionResult Trangchu() {

            return View("~/Views/Shared/Trangchu.cshtml");
        }

        //Hien thi form them thiet bi
        public IActionResult Thietbi_Add()
        {
            List<Status> statuses;
            statuses = DataProvider<Status>.Instance.GetListItem("tbl_status");
   
            List<Room> rooms;
            rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

            List<Contact> contacts;
            contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");

            DeviceDetail devicedetails = new DeviceDetail();
            devicedetails.Status = statuses;
            devicedetails.rooms = rooms;
            devicedetails.contacts = contacts;

            return RedirectToAction("Thietbi");
        }

        [HttpPost]
        public IActionResult Thietbi_Add(string tbiID, string tbiName, string tbiManufacturer, string tbiSeri,
        string tbiType, string tbiGroup, int tbiMaintenance_cycle, string tbiMaintenance_start, string tbiStockout_date, 
        int tbiCondition, string tbiReceived_date, string tbiNote)
        {
            DateTime TBIMaintenance_start = new DateTime();
            TBIMaintenance_start = DateTime.Parse(tbiMaintenance_start);
            string device_maintenancestart = TBIMaintenance_start.ToString("yyyy-MM-dd");
 
            DateTime TBIStockout_date = new DateTime();
            TBIStockout_date = DateTime.Parse(tbiStockout_date);
            string device_stockoutdate = TBIStockout_date.ToString("yyyy-MM-dd");

            DateTime TBIReceived_date = new DateTime();
            TBIReceived_date = DateTime.Parse(tbiReceived_date);
            string device_receiveddate = TBIReceived_date.ToString("yyyy-MM-dd");

            //string query =

            return RedirectToAction("Thietbi");
        }

        //Hien thi danh sach thiet bi
        public IActionResult Thietbi()
        {
            //khởi tạo
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];

            field = field == null ? "All" : field;
            sortOrder = sortOrder == null ? "Name" : sortOrder;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Device> deviceList = new ItemDisplay<Device>();
            deviceList.SortOrder = sortOrder;
            deviceList.CurrentSearchField = searchField;
            deviceList.CurrentSearchString = searchString;
            deviceList.CurrentPage = currentPage;


           string query = @"
               SELECT
                    de.*,
                    r.room_name,
                    s.status_name,
                    c.contact_address
                FROM dbo.tbl_device de
                LEFT JOIN dbo.tbl_contact c ON de.FK_contact_id = c.contact_id
                LEFT JOIN dbo.tbl_room r ON de.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON de.FK_status_id = s.status_id";
              

            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItemQuery(query);
            devices = Function.Instance.searchItems(devices, deviceList);
            devices = Function.Instance.sortItems(devices, deviceList.SortOrder);
            deviceList.Paging(devices, 10);

            var deviceForm = new DeviceDetail
            {
                Status = DataProvider<Status>.Instance.GetListItem("tbl_status"),
                rooms = DataProvider<Room>.Instance.GetListItem("tbl_room"),
                contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact")

            };

            // Tạo view model tổng hợp
            var viewTbiModel = new DevicePageViewModel
            {
                DeviceList = deviceList,
                DeviceForm = deviceForm
            };

            return View("~/Views/Shared/Thietbi.cshtml", viewTbiModel);
        }

        [HttpPost]
        public IActionResult Thietbi(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Thietbi", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }

        //public IActionResult Thietbi_Detail()
        //{
        //    var urlQuery = Request.HttpContext.Request.Query;
        //    string ID = urlQuery["device_id"];
        //    Device Device;

        //    Device = DataProvider<Device>.Instance.GetItem("device_id",ID,"device");
        //    return View("~/Views/Shared/PatientDetail.cshtml", Device);
        //}

       
        public IActionResult Baotri_Add()
        {
            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device");
            List<Room> rooms;
            rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

            MaintainDetail maintaindetails = new MaintainDetail();
            maintaindetails.devices = devices;
            maintaindetails.rooms = rooms;

            return RedirectToAction("Baotri");
        }

        [HttpPost]
        public IActionResult Baotri_Add(string btrname, string btrID, string Btrdate, string btrDelivery, int btrDeliPhone, string btrMaintain, int btrMaintainPhone )
        {
            DateTime btrdate = new DateTime();
            btrdate = DateTime.Parse(Btrdate);
            string maintainDate = btrdate.ToString("yyyy-MM-dd");

            //string query = String.Format("Insert into dbo.tbl_Surgery(Surgery_ID, FK_Patient_ID, Surgery_Time, FK_Staff_Main, FK_Room_ID) " +
            //"values({0},{1}, '{2}', {3}, {4} )", surgeryID, pID, surgerytime, surmain, roomID);
            //DataProvider<Staff>.Instance.ExcuteQuery(query);
            return RedirectToAction("Baotri");
        }

        public IActionResult Baotri()
        {
            // Lấy tham số từ query như hiện tại
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];

            field = field == null ? "All" : field;
            sortOrder = sortOrder == null ? "Name" : sortOrder;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Maintain> maintainList = new ItemDisplay<Maintain>();
            maintainList.SortOrder = sortOrder;
            maintainList.CurrentSearchField = searchField;
            maintainList.CurrentSearchString = searchString;
            maintainList.CurrentPage = currentPage;

            string query = @"
               SELECT
                    m.*,
                    d.device_name,
                    r.room_name,
                    s.status_name
                FROM dbo.tbl_maintain m
                LEFT JOIN dbo.tbl_device d ON m.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON m.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON m.FK_status_id = s.status_id
                WHERE s.status_id LIKE '0%'";

            List<Maintain> maintain = DataProvider<Maintain>.Instance.GetListItemQuery(query);
            maintain = Function.Instance.searchItems(maintain, maintainList);
            maintain = Function.Instance.sortItems(maintain, maintainList.SortOrder);
            maintainList.Paging(maintain, 10);

            // Lấy dữ liệu devices và rooms cho form lập kế hoạch
            var maintainForm = new MaintainDetail
            {
                devices = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device"),
                rooms = DataProvider<Room>.Instance.GetListItem("", "tbl_room")
            };

            // Tạo view model tổng hợp
            var viewModel = new MaintainPageViewModel
            {
                MaintainList = maintainList,
                MaintainForm = maintainForm
            };

            return View("~/Views/Shared/Baotri.cshtml", viewModel);
        }
        [HttpPost]
        public IActionResult Baotri(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Baotri", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }

        public IActionResult Suachua()
        {
            // Khởi tạo
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            /// Lấy query, không có => đặt mặc định
            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];
            field = field == null ? "All" : field;

            sortOrder = sortOrder == null ? "Name" : sortOrder; ;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Repair> RepairList = new ItemDisplay<Repair>();
            RepairList.SortOrder = sortOrder;
            RepairList.CurrentSearchField = searchField;
            RepairList.CurrentSearchString = searchString;
            RepairList.CurrentPage = currentPage;

            string query = @"
               SELECT
                    re.*,
                    d.device_name,
                    r.room_name,
                    s.status_name
                FROM dbo.tbl_repair re
                LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON re.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON re.FK_status_id = s.status_id
                WHERE s.status_id LIKE '1%'";

            List<Repair> Repair;
            Repair = DataProvider<Repair>.Instance.GetListItemQuery(query);
            Repair = Function.Instance.searchItems(Repair, RepairList);
            Repair = Function.Instance.sortItems(Repair, RepairList.SortOrder);
            RepairList.Paging(Repair, 10);

            return View("~/Views/Shared/Suachua.cshtml", RepairList);
        }
        [HttpPost]
        public IActionResult Suachua(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Suachua", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }
        public IActionResult Kho()
        {
            // Khởi tạo
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            /// Lấy query, không có => đặt mặc định
            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];
            field = field == null ? "All" : field;

            sortOrder = sortOrder == null ? "Name" : sortOrder; ;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Storage> StorageList = new ItemDisplay<Storage>();
            StorageList.SortOrder = sortOrder;
            StorageList.CurrentSearchField = searchField;
            StorageList.CurrentSearchString = searchString;
            StorageList.CurrentPage = currentPage;



            return View("~/Views/Shared/Kho.cshtml", StorageList);
        }
        public IActionResult Taichinh_Hopdong()
        {
            return View("~/Views/Shared/Taichinh_Hopdong.cshtml");
        }
    }

}

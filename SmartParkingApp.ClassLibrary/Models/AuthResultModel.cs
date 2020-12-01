using System;

namespace SmartParkingApp.ClassLibrary.Models
{
    public class AuthResultModel
    {
        public string acces_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime expiration { get; set; }
        public string username { get; set; }
        public string userrole { get; set; }
        public string car_plate_number { get; set; }
    }
}

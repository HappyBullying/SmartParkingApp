using System;

namespace SmartParkingApp.ClassLibrary.Models
{
    public class TokenResponseModel
    {
        public string action_token { get; set; }
        public DateTime expiraton { get; set; }
        public string refresh_token { get; set; }
        public string role { get; set; }
        public string user_identifier { get; set; }
        public string user_credential_type { get; set; }
    }
}

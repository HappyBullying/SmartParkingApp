using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingApp.ClassLibrary
{
    public class ParkingManager
    {
        private string rootUrl = "http://localhost:5000";
        private HttpClient client;
        private SecureString refreshToken;
        private DateTime? expirationTime;
        public User CurrentUser { get; set; }


        public ParkingManager()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            refreshToken = new SecureString();
        }


        private async void CheckJwt()
        {
            if (!(expirationTime != null && expirationTime.Value >= DateTime.UtcNow.AddSeconds(25)))
            {
                // Refresh
                UriBuilder ub = new UriBuilder(rootUrl + "/api/account/refreshtoken");

                StringBuilder sb = new StringBuilder();
                IntPtr valuePtr = Marshal.SecureStringToGlobalAllocUnicode(refreshToken);
                for (int i = 0; i < refreshToken.Length; i++)
                {
                    short unicodeChar = Marshal.ReadInt16(valuePtr, i * 2);
                    sb.Append((char)unicodeChar);
                }

                string json = JsonConvert.SerializeObject(new { RefreshToken = sb.ToString() });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
                string resultString = await response.Content.ReadAsStringAsync();
                ResponseModel responseResult = JsonConvert.DeserializeObject<ResponseModel>(resultString);
                JObject jOData = (JObject)responseResult.Data;
                AuthResultModel authResult = jOData.ToObject<AuthResultModel>();

                refreshToken.Clear();
                for (int i = 0; i < authResult.refresh_token.Length; i++)
                {
                    refreshToken.AppendChar(authResult.refresh_token[i]);
                }
                expirationTime = authResult.expiration.AddHours(3);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.acces_token);
            }
        }



        /// <summary>
        /// Returns Active session for user if he closed the application and didn't payed
        /// </summary>
        public async Task<ResponseModel> GetActiveSessionForUser()
        {
            CheckJwt();
            UriBuilder uB = new UriBuilder(rootUrl + "/api/parking/getactivesessionforuser");
            uB.Query = "username=" + CurrentUser.Username;
            HttpResponseMessage response = await client.GetAsync(uB.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }






        /// <summary>
        /// Returns all parking session that are inside time interval
        /// </summary>
        public async Task<ResponseModel> GetSessionsInPeriod(int userId, DateTime since, DateTime until)
        {
            // IEnumerable<ParkingSession>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getsessionsinperiod");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", userId.ToString() },
                {"since", since.ToString() },
                {"until", until.ToString() }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }




        /// <summary>
        /// Returns all payed parking session that are inside time interval
        /// </summary>
        public async Task<ResponseModel> GetPayedSessionsInPeriod(int userId, DateTime since, DateTime until)
        {
            // IEnumerable<ParkingSession>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getpayedsessionsinperiod");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", userId.ToString() },
                {"since", since.ToString() },
                {"until", until.ToString() }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            ResponseModel rM = JsonConvert.DeserializeObject<ResponseModel>(resultString);

            return rM;
        }





        /// <summary>
        /// Returns past sessions if user is owner
        /// </summary>
        public async Task<ResponseModel> GetActiveSesstionsForOwner(int userId)
        {
            // IEnumerable<ParkingSession>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getactivesesstionsforowner");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", userId.ToString() }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }



        /// <summary>
        /// Returns All Past Sessions if user is owner
        /// </summary>
        public async Task<ResponseModel> GetPastSesstionsForOwner(int userId)
        {
            // IEnumerable<ParkingSession>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getpastsesstionsforowner");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", userId.ToString() }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }





        /// <summary>
        /// Gets Percentage of occupied space
        /// </summary>
        public async Task<ResponseModel> GetPercentageofOccupiedSpace(int userId)
        {
            // double
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getpercentageofoccupiedspace");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", userId.ToString() }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        /// <summary>
        /// Gets list of tariffs
        /// </summary>
        public async Task<ResponseModel> GetTariffs()
        {
            // List<Tariff>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/gettariffs");
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        /// <summary>
        /// Get completed parking sessions for user
        /// </summary>
        public async Task<ResponseModel> GetCompletedSessionsForUser()
        {
            // IEnumerable<ParkingSession>
            CheckJwt();

            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getcompletedsessionsforuser");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userName", CurrentUser.Username }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }

            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            ResponseModel rM = JsonConvert.DeserializeObject<ResponseModel>(resultString);
            JArray dataObject = (JArray)rM.Data;
            rM.Data = dataObject.ToObject<IList<ParkingSession>>();
            return rM;
        }


        /// <summary>
        /// Registers new user
        /// </summary>
        public async Task<ResponseModel> RegisterUser(User usr)
        {
            UriBuilder ub = new UriBuilder(rootUrl + "/api/account/register");
            string json = JsonConvert.SerializeObject(usr);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            CurrentUser = usr;
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        public async Task<ResponseModel> RegisterOwner(OwnerRegisterModel oUsr)
        {
            UriBuilder ub = new UriBuilder(rootUrl + "/api/account/registerowner");
            string json = JsonConvert.SerializeObject(oUsr);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        /// <summary>
        /// Login method
        /// </summary>
        public async Task<ResponseModel> Login(User usr)
        {
            this.CurrentUser = usr;
            UriBuilder ub = new UriBuilder(rootUrl + "/api/account/login");
            string json = JsonConvert.SerializeObject(new
            {
                Username = usr.Username,
                Password = usr.Password
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            ResponseModel rM = JsonConvert.DeserializeObject<ResponseModel>(resultString);
            JObject jOData = (JObject)rM.Data;
            
            AuthResultModel aRM = jOData.ToObject<AuthResultModel>();
            CurrentUser.CarPlateNumber = aRM.car_plate_number;
            refreshToken.Clear();
            for (int i = 0; i < aRM.refresh_token.Length; i++)
            {
                refreshToken.AppendChar(aRM.refresh_token[i]);
            }
            expirationTime = aRM.expiration;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", aRM.acces_token);
            
            return rM;
            
        }



        /// <summary>
        /// Returns previous parking sessions for user
        /// </summary>
        public async Task<ResponseModel> GetPastSessionsForUser(int userId)
        {
            // List<ParkingSession>
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getpastsessionsforuser");
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }

        public async Task<ResponseModel> EnterParking()
        {
            // ParkingSession
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/enterparking");
            string json = JsonConvert.SerializeObject(new
            {
                CarPlateNumber = CurrentUser.CarPlateNumber,
                TicketId = -1
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            ResponseModel responseResult = JsonConvert.DeserializeObject<ResponseModel>(resultString);
            JObject dataObject = (JObject)responseResult.Data;
            responseResult.Data = dataObject.ToObject<ParkingSession>();
            return responseResult;
        }



        public async Task<ResponseModel> TryLeaveParking(int ticketNumber, ParkingSession session)
        {
            // bool
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/tryleaveparking");
            string json = JsonConvert.SerializeObject(new
            {
                CarPlateNumber = session.CarPlateNumber,
                TicketId = ticketNumber
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        /// <summary>
        /// Calculates remaining parking cost
        /// </summary>
        public async Task<ResponseModel> GetRemainingCost(int ticketNumber)
        {
            // decimal
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/getremainingcost");
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userName", CurrentUser.Username }
            }))
            {
                ub.Query = await content.ReadAsStringAsync();
            }
            HttpResponseMessage response = await client.GetAsync(ub.Uri);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }


        public async Task<ResponseModel> PayForParking(int ticketNumber, string carPlateNumber)
        {
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/payforparking");
            string json = JsonConvert.SerializeObject(new
            {
                CarPlateNumber = carPlateNumber,
                TicketId = ticketNumber
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }

        public async Task<ResponseModel> TryLeaveParkingByCarPlateNumber(string carPlateNumber, ParkingSession session)
        {
            // bool
            CheckJwt();
            UriBuilder ub = new UriBuilder(rootUrl + "/api/parking/tryleaveparkingbycarplatenumber");
            string json = JsonConvert.SerializeObject(new
            {
                CarPlateNumber = carPlateNumber,
                TicketId = session.TicketNumber
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ub.Uri, data);
            string resultString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(resultString);
        }
    }
}

using Newtonsoft.Json;
using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartParkingApp.ClassLibrary
{
    public class ParkingManager
    {
        private List<ParkingSession> pastSessions;
        private List<ParkingSession> activeSessions;
        private List<Tariff> tariffTable;
        private List<User> users;

        private int parkingCapacity;
        private int freeLeavePeriod;
        private int nextTicketNumber;
        private readonly string DataPath;




        public ParkingManager(string dataPath)
        {
            DataPath = dataPath;
            LoadData();
        }



        /// <summary>
        /// Returns Active session for user if he closed the application and didn't payed
        /// </summary>
        public ParkingSession GetActiveSessionForUser(int userId)
        {
            ParkingSession ret = activeSessions.Find(ps => ps.UserId == userId);
            return ret;
        }






        /// <summary>
        /// Returns all parking session that are inside time interval
        /// </summary>
        public IEnumerable<ParkingSession> GetSessionsInPeriod(int userId, DateTime since, DateTime until)
        {
            User usr = users.Find(u => u.Id == userId && u.UserRole == UserRole.Owner);
            if (usr == null)
                return null;
            if (usr.UserRole != UserRole.Owner)
                return null;

            List<ParkingSession> ret = new List<ParkingSession>();

            IEnumerable<ParkingSession> past = from tmp in pastSessions
                                               where (tmp.EntryDt >= since) && (tmp.ExitDt <= until)
                                               select tmp;
            ret.AddRange(past);
            IEnumerable<ParkingSession> act = from tmp in activeSessions
                                              where (tmp.EntryDt >= since)
                                              select tmp;

            //IEnumerator<ParkingSession> enumer = act.GetEnumerator();
            // Set exit dates to until
            //while(enumer.MoveNext())
            //{
            //    ret.Add(new ParkingSession
            //    {
            //        EntryDt = enumer.Current.EntryDt,
            //        ExitDt = until,
            //        CarPlateNumber = enumer.Current.CarPlateNumber,
            //        TicketNumber = enumer.Current.TicketNumber
            //    });
            //}

            ret.AddRange(act);

            return ret;
        }




        /// <summary>
        /// Returns all payed parking session that are inside time interval
        /// </summary>
        public IEnumerable<ParkingSession> GetPayedSessionsInPeriod(int userId, DateTime since, DateTime until)
        {
            User usr = users.Find(u => u.Id == userId && u.UserRole == UserRole.Owner);
            if (usr == null)
                return null;
            if (usr.UserRole != UserRole.Owner)
                return null;

            List<ParkingSession> ret = (from tmp in pastSessions
                                              where
                     (tmp.PaymentDt >= since) && (tmp.PaymentDt <= until)
                                              select tmp).ToList();
            IEnumerable<ParkingSession> active = from tmp in activeSessions
                                                  where (tmp.PaymentDt >= since) && (tmp.PaymentDt <= until) &&
                                                  (tmp.PaymentDt != null) select tmp;

            ret.AddRange(active);
            return ret;
        }





        /// <summary>
        /// Returns past sessions if user is owner
        /// </summary>
        public IEnumerable<ParkingSession> GetActiveSesstionsForOwner(int userId)
        {
            bool isOwner = users.Any(u => u.Id == userId && u.UserRole == UserRole.Owner);

            if (isOwner)
            {
                return activeSessions;
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// Returns All Past Sessions if user is owner
        /// </summary>
        public IEnumerable<ParkingSession> GetPastSesstionsForOwner(int userId)
        {
            bool isOwner = users.Any(u => u.Id == userId && u.UserRole == UserRole.Owner);

            if (isOwner)
            {
                return pastSessions;
            }
            else
            {
                return null;
            }
        }





        /// <summary>
        /// Gets Percentage of occupied space
        /// </summary>
        public double GetPercentageofOccupiedSpace(int userId)
        {
            User usr = users.Find(u => u.Id == userId);
            
            if (usr == null)
            {
                return -1;
            }

            if (usr.UserRole != UserRole.Owner)
            {
                return -1;
            }


            double taken = activeSessions.Count;
            double rate = taken / parkingCapacity;
            return rate * 100.0d;
        }


        /// <summary>
        /// Gets list of tariffs
        /// </summary>
        public List<Tariff> GetTariffs()
        {
            return tariffTable;
        }


        /// <summary>
        /// Get completed parking sessions for user
        /// </summary>
        public IEnumerable<ParkingSession> GetCompletedSessionsForUser(int userId)
        {
            // Select from pastSessions list only those sessions that are ralative to
            // user with specified ID
            // If query won't succeed then ret will not be null

            IEnumerable<ParkingSession> ret = from tmp in pastSessions
                                              where tmp.UserId == userId
                                              select tmp;
            return ret;
        }



        /// <summary>
        /// Returns User object by ID
        /// </summary>
        public User GetUserById(int userId)
        {
            User ret = users.Find(u => u.Id == userId);
            return ret;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        public string RegisterNewUser(User usr)
        {
            if (users.Count != 0)
            {
                bool alreadyExist = users.Any(u => u.Name == usr.Name);
                if (alreadyExist)
                {
                    return "User with this name already exists";
                }

                int maxUserId = users.Max(u => u.Id);
                usr.Id = maxUserId + 1;
            }
            else
            {
                usr.Id = 1;
            }

            // Add user to users collection
            users.Add(usr);
            Serialize(DataPath + "\\Users.json", users);
            return "Successfully";
        }


        /// <summary>
        /// Login method
        /// </summary>
        public string Login(User usr)
        {
            User found = users.Find(u => u.Name == usr.Name);


            // Authorize via phone nubmer
            if (found == null)
            {
                found = users.Find(u => u.Phone == usr.Name);
            }

            if (found == null)
            {
                return "No user with this name was found";
            }

            if (usr.PasswordHash == found.PasswordHash)
            {
                if (found.UserRole != usr.UserRole)
                    return "You can not authorize this account using app for " + usr.UserRole;
                else
                    return found.Id.ToString();
            }
            else
            {
                return "Invalid username or password";
            }
        }



        /// <summary>
        /// Returns previous parking sessions for user
        /// </summary>
        public List<ParkingSession> GetPastSessionsForUser(int userId)
        {
            List<ParkingSession> ret = (from tmp in pastSessions
                                        where tmp.UserId == userId
                                        select tmp).ToList();
            return ret;
        }

        public ParkingSession EnterParking(string carPlateNumber)
        {
            if (activeSessions.Count >= parkingCapacity || activeSessions.Any(s => s.CarPlateNumber == carPlateNumber))
                return null;

            var session = new ParkingSession
            {
                CarPlateNumber = carPlateNumber,
                EntryDt = DateTime.Now,
                TicketNumber = nextTicketNumber++,
                User = users.FirstOrDefault(u => u.CarPlateNumber == carPlateNumber)
            };
            session.UserId = session.User?.Id;

            activeSessions.Add(session);
            SaveData();
            return session;
        }



        public bool TryLeaveParkingWithTicket(int ticketNumber, ParkingSession session)
        {
            session = GetSessionByTicketNumber(ticketNumber);

            DateTime currentDt = DateTime.Now;  // Getting the current datetime only once

            double diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
            if (diff <= freeLeavePeriod)
            {
                session.TotalPayment = 0;
                CompleteSession(session, currentDt);
                return true;
            }
            else
            {
                session = null;
                return false;
            }
        }

        public decimal GetRemainingCost(int ticketNumber)
        {
            DateTime currentDt = DateTime.Now;
            ParkingSession session = GetSessionByTicketNumber(ticketNumber);

            double diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
            return GetCost(diff);
        }

        public void PayForParking(int ticketNumber, decimal amount)
        {
            ParkingSession session = GetSessionByTicketNumber(ticketNumber);
            session.TotalPayment = (session.TotalPayment ?? 0) + amount;
            session.PaymentDt = DateTime.Now;
            SaveData();
        }

        public bool TryLeaveParkingByCarPlateNumber(string carPlateNumber, ParkingSession session)
        {
            session = activeSessions.FirstOrDefault(s => s.CarPlateNumber == carPlateNumber);
            if (session == null)
                return false;

            DateTime currentDt = DateTime.Now;

            if (session.PaymentDt != null)
            {
                if ((currentDt - session.PaymentDt.Value).TotalMinutes <= freeLeavePeriod)
                {
                    CompleteSession(session, currentDt);
                    return true;
                }
                else
                {
                    session = null;
                    return false;
                }
            }
            else
            {
                // No payment, within free leave period -> allow exit
                if ((currentDt - session.EntryDt).TotalMinutes <= freeLeavePeriod)
                {
                    session.TotalPayment = 0;
                    CompleteSession(session, currentDt);
                    return true;
                }
                else
                {
                    // The session has no connected customer
                    if (session.User == null)
                    {
                        session = null;
                        return false;
                    }
                    else
                    {
                        session.TotalPayment = GetCost((currentDt - session.EntryDt).TotalMinutes - freeLeavePeriod);
                        session.PaymentDt = currentDt;
                        CompleteSession(session, currentDt);
                        return true;
                    }
                }
            }
        }

        #region Helper methods
        private ParkingSession GetSessionByTicketNumber(int ticketNumber)
        {
            var session = activeSessions.FirstOrDefault(s => s.TicketNumber == ticketNumber);
            if (session == null)
                throw new ArgumentException($"Session with ticket number = {ticketNumber} does not exist");
            return session;
        }

        private decimal GetCost(double diffInMinutes)
        {
            var tariff = tariffTable.FirstOrDefault(t => t.Minutes >= diffInMinutes) ?? tariffTable.Last();
            return tariff.Rate;
        }

        private T Deserialize<T>(string fileName)
        {
            using (var sr = new StreamReader(fileName))
            {
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        private void Serialize<T>(string fileName, T data)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(jsonWriter, data);
                }
            }
        }

        private class ParkingData
        {
            public List<ParkingSession> PastSessions { get; set; }
            public List<ParkingSession> ActiveSessions { get; set; }
            public int Capacity { get; set; }
        }

        //private const string TariffsFileName = "data\\tariffs.json";
        //private const string ParkingDataFileName = "data\\parkingdata.json";
        //private const string UsersFileName = "data\\users.json";

        private void LoadData()
        {
            tariffTable = Deserialize<List<Tariff>>(DataPath + "\\Tariffs.json") ?? new List<Tariff>();
            ParkingData data = Deserialize<ParkingData>(DataPath + "\\ParkingData.json");
            users = Deserialize<List<User>>(DataPath + "\\Users.json") ?? new List<User>();

            if (data != null)
            {
                parkingCapacity = data.Capacity;
                pastSessions = (data.PastSessions == null) ? new List<ParkingSession>() : data.PastSessions;
                activeSessions = (data.ActiveSessions == null) ? new List<ParkingSession>() : data.ActiveSessions;
            }
            else
            {
                parkingCapacity = 0;
                pastSessions = new List<ParkingSession>();
                activeSessions = new List<ParkingSession>();
            }
            if (tariffTable.Count != 0)
                freeLeavePeriod = tariffTable.First().Minutes;
            else
                freeLeavePeriod = 0;

            nextTicketNumber = activeSessions.Count > 0 ? activeSessions.Max(s => s.TicketNumber) + 1 : 1;
        }

        private void SaveData()
        {
            ParkingData data = new ParkingData
            {
                Capacity = parkingCapacity,
                ActiveSessions = activeSessions,
                PastSessions = pastSessions
            };

            Serialize(DataPath + "\\ParkingData.json", data);
        }

        private void CompleteSession(ParkingSession session, DateTime currentDt)
        {
            session.ExitDt = currentDt;
            activeSessions.Remove(session);
            pastSessions.Add(session);
            SaveData();
        }
        #endregion
    }
}

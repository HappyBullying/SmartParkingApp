using Newtonsoft.Json;
using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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
                    return "You can not authorize this account using app for" + usr.UserRole;
                else
                   return "Successfully";
            }
            else
            {
                return "Invalid username or password";
            }
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



        public bool TryLeaveParkingWithTicket(int ticketNumber, out ParkingSession session)
        {
            session = GetSessionByTicketNumber(ticketNumber);

            var currentDt = DateTime.Now;  // Getting the current datetime only once

            var diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
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

        public bool TryLeaveParkingByCarPlateNumber(string carPlateNumber, out ParkingSession session)
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

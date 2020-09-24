using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using SmartParkingApp.ClassLibrary.Logging;

namespace SmartParkingApp.ClassLibrary
{
    public class ParkingManager
    {
        private LocalJsonDb db;

        public ParkingManager()
        {
            
        }


        public void Initialize(string dataPath)
        {
            db = new LocalJsonDb(dataPath);
            // Create logger which logs into a specific file
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.File(formatter: new CustomTextFormatter(), "SmartParkingAppLogs.log").
                CreateLogger();
            db.LoadData();
        }




        /// <summary>
        /// Returns Active session for user if he closed the application and didn't payed
        /// </summary>
        public ParkingSession GetActiveSessionForUser(int userId)
        {
            ParkingSession ret = db.ActiveSessions.Find(ps => ps.UserId == userId);
            return ret;
        }






        /// <summary>
        /// Returns all parking session that are inside time interval
        /// </summary>
        public IEnumerable<ParkingSession> GetSessionsInPeriod(int userId, DateTime since, DateTime until)
        {
            User usr = db.Users.Find(u => u.Id == userId && u.UserRole == UserRole.Owner);
            if (usr == null)
                return null;
            if (usr.UserRole != UserRole.Owner)
                return null;

            List<ParkingSession> ret = new List<ParkingSession>();

            IEnumerable<ParkingSession> past = from tmp in db.PastSessions
                                               where (tmp.EntryDt >= since) && (tmp.ExitDt <= until)
                                               select tmp;
            ret.AddRange(past);
            IEnumerable<ParkingSession> act = from tmp in db.ActiveSessions
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
            User usr = db.Users.Find(u => u.Id == userId && u.UserRole == UserRole.Owner);
            if (usr == null)
                return null;
            if (usr.UserRole != UserRole.Owner)
                return null;

            List<ParkingSession> ret = (from tmp in db.PastSessions
                                              where
                     (tmp.PaymentDt >= since) && (tmp.PaymentDt <= until)
                                              select tmp).ToList();
            IEnumerable<ParkingSession> active = from tmp in db.ActiveSessions
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
            bool isOwner = db.Users.Any(u => u.Id == userId && u.UserRole == UserRole.Owner);

            if (isOwner)
            {
                return db.ActiveSessions;
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
            bool isOwner = db.Users.Any(u => u.Id == userId && u.UserRole == UserRole.Owner);

            if (isOwner)
            {
                return db.PastSessions;
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
            User usr = db.Users.Find(u => u.Id == userId);
            
            if (usr == null)
            {
                return -1;
            }

            if (usr.UserRole != UserRole.Owner)
            {
                return -1;
            }


            double taken = db.ActiveSessions.Count;
            double rate = taken / db.ParkingCapacity;
            return rate * 100.0d;
        }


        /// <summary>
        /// Gets list of tariffs
        /// </summary>
        public List<Tariff> GetTariffs()
        {
            return db.TariffTable;
        }


        /// <summary>
        /// Get completed parking sessions for user
        /// </summary>
        public IEnumerable<ParkingSession> GetCompletedSessionsForUser(int userId)
        {
            // Select from db.PastSessions list only those sessions that are ralative to
            // user with specified ID
            // If query won't succeed then ret will not be null

            IEnumerable<ParkingSession> ret = from tmp in db.PastSessions
                                              where tmp.UserId == userId
                                              select tmp;
            return ret;
        }



        /// <summary>
        /// Returns User object by ID
        /// </summary>
        public User GetUserById(int userId)
        {
            User ret = db.Users.Find(u => u.Id == userId);
            return ret;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        public string RegisterNewUser(User usr)
        {
            if (db.Users.Count != 0)
            {
                bool alreadyExist = db.Users.Any(u => u.Name == usr.Name);
                if (alreadyExist)
                {
                    return "User with this name already exists";
                }

                int maxUserId = db.Users.Max(u => u.Id);
                usr.Id = maxUserId + 1;
            }
            else
            {
                usr.Id = 1;
            }
            Log.Information("New User Registered {UserName} {UserId}", usr.Name, usr.Id);
            // Add user to db.Users collection
            db.Users.Add(usr);
            db.Serialize(db.DataPath + Path.DirectorySeparatorChar + "Users.json", db.Users);
            return "Successfully";
        }


        /// <summary>
        /// Login method
        /// </summary>
        public string Login(User usr)
        {
            User found = db.Users.Find(u => u.Name == usr.Name);


            // Authorize via phone nubmer
            if (found == null)
            {
                found = db.Users.Find(u => u.Phone == usr.Name);
            }

            if (found == null)
            {
                Log.Information("No user was found with the provided credentials: {UserName} {Phone}", usr.Name, usr.Phone);
                return "No user with this name was found";
            }

            // Check password
            if (usr.PasswordHash == found.PasswordHash)
            {
                if (found.UserRole != usr.UserRole)
                    return "You can not authorize this account using app for " + usr.UserRole;
                else
                    return found.Id.ToString();
            }
            else
            {
                Log.Information("Invalid user details dupplied");
                return "Invalid username or password";
            }
        }



        /// <summary>
        /// Returns previous parking sessions for user
        /// </summary>
        public List<ParkingSession> GetPastSessionsForUser(int userId)
        {
            List<ParkingSession> ret = (from tmp in db.PastSessions
                                        where tmp.UserId == userId
                                        select tmp).ToList();
            return ret;
        }

        public ParkingSession EnterParking(string carPlateNumber)
        {
            if (db.ActiveSessions.Count >= db.ParkingCapacity || db.ActiveSessions.Any(s => s.CarPlateNumber == carPlateNumber))
                return null;

            var session = new ParkingSession
            {
                CarPlateNumber = carPlateNumber,
                EntryDt = DateTime.Now,
                TicketNumber = db.NextTicketNumber++,
                User = db.Users.FirstOrDefault(u => u.CarPlateNumber == carPlateNumber)
            };
            session.UserId = session.User?.Id;

            db.ActiveSessions.Add(session);
            db.SaveData();

            Log.Information("New parking session started {CarPlateNumber}", carPlateNumber);
            return session;
        }



        public bool TryLeaveParkingWithTicket(int ticketNumber, ParkingSession session)
        {
            session = GetSessionByTicketNumber(ticketNumber);

            DateTime currentDt = DateTime.Now;  // Getting the current datetime only once

            double diff = (currentDt - (session.PaymentDt ?? session.EntryDt)).TotalMinutes;
            if (diff <= db.FreeLeavePeriod)
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


        /// <summary>
        /// Calculates remaining parking cost
        /// </summary>
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
            db.SaveData();
        }

        public bool TryLeaveParkingByCarPlateNumber(string carPlateNumber, ParkingSession session)
        {
            session = db.ActiveSessions.FirstOrDefault(s => s.CarPlateNumber == carPlateNumber);
            if (session == null)
                return false;

            DateTime currentDt = DateTime.Now;

            if (session.PaymentDt != null)
            {
                if ((currentDt - session.PaymentDt.Value).TotalMinutes <= db.FreeLeavePeriod)
                {
                    CompleteSession(session, currentDt);
                    Log.Information("Parking session completed successfully {SessionEntryDate} {SessionExitDate} {TotalPayment}", 
                        session.EntryDt, session.ExitDt, session.TotalPayment);
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
                if ((currentDt - session.EntryDt).TotalMinutes <= db.FreeLeavePeriod)
                {
                    session.TotalPayment = 0;
                    Log.Information("Parking session completed successfully {SessionEntryDate} {SessionExitDate}",
                       session.EntryDt, session.ExitDt);

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
                        session.TotalPayment = GetCost((currentDt - session.EntryDt).TotalMinutes - db.FreeLeavePeriod);
                        session.PaymentDt = currentDt;
                        CompleteSession(session, currentDt);
                        return true;
                    }
                }
            }
        }



        private void CompleteSession(ParkingSession session, DateTime currentDt)
        {
            session.ExitDt = currentDt;
            db.ActiveSessions.Remove(session);
            db.PastSessions.Add(session);
            db.SaveData();
        }

        private decimal GetCost(double diffInMinutes)
        {
            var tariff = db.TariffTable.FirstOrDefault(t => t.Minutes >= diffInMinutes) ?? db.TariffTable.Last();
            return tariff.Rate;
        }


        private ParkingSession GetSessionByTicketNumber(int ticketNumber)
        {
            var session = db.ActiveSessions.FirstOrDefault(s => s.TicketNumber == ticketNumber);
            if (session == null)
            {
                Exception e = new ArgumentException($"Session with ticket number = {ticketNumber} does not exist");
                Log.Error("Error while creating user {Error} {Stacktrace} {InnerException} {Source}",
                    e.Message, e.StackTrace, e.InnerException, e.Source);
                throw e;
            }
            return session;
        }

    }
}

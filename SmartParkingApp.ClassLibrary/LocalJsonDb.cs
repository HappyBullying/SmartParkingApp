using Newtonsoft.Json;
using SmartParkingApp.ClassLibrary.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartParkingApp.ClassLibrary
{
    class LocalJsonDb
    {
        public List<ParkingSession> PastSessions { get; set; }
        public List<ParkingSession> ActiveSessions { get; set; }
        public List<Tariff> TariffTable { get; set; }
        public List<User> Users { get; set; }

        public readonly string DataPath;
        public int ParkingCapacity { get; set; }
        public int FreeLeavePeriod { get; set; }
        public int NextTicketNumber { get; set; }

        public LocalJsonDb(string path)
        {
            DataPath = path;
        }


        #region Db methods
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

        public void Serialize<T>(string fileName, T data)
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


        public void LoadData()
        {
            char sep = Path.DirectorySeparatorChar;
            TariffTable = Deserialize<List<Tariff>>(DataPath + sep + "Tariffs.json") ?? new List<Tariff>();
            ParkingData data = Deserialize<ParkingData>(DataPath + sep + "ParkingData.json");
            Users = Deserialize<List<User>>(DataPath + sep + "Users.json") ?? new List<User>();

            if (data != null)
            {
                ParkingCapacity = data.Capacity;
                PastSessions = (data.PastSessions == null) ? new List<ParkingSession>() : data.PastSessions;
                ActiveSessions = (data.ActiveSessions == null) ? new List<ParkingSession>() : data.ActiveSessions;
            }
            else
            {
                ParkingCapacity = 0;
                PastSessions = new List<ParkingSession>();
                ActiveSessions = new List<ParkingSession>();
            }
            if (TariffTable.Count != 0)
                FreeLeavePeriod = TariffTable.First().Minutes;
            else
                FreeLeavePeriod = 0;

            NextTicketNumber = ActiveSessions.Count > 0 ? ActiveSessions.Max(s => s.TicketNumber) + 1 : 1;
        }

        public void SaveData()
        {
            char sep = Path.DirectorySeparatorChar;
            ParkingData data = new ParkingData
            {
                Capacity = ParkingCapacity,
                ActiveSessions = ActiveSessions,
                PastSessions = PastSessions
            };

            Serialize(DataPath + sep + "ParkingData.json", data);
        }

        #endregion
    }
}

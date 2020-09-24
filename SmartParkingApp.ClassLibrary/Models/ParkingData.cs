using System.Collections.Generic;

namespace SmartParkingApp.ClassLibrary.Models
{
    class ParkingData
    {
        public List<ParkingSession> PastSessions { get; set; }
        public List<ParkingSession> ActiveSessions { get; set; }
        public int Capacity { get; set; }
    }
}

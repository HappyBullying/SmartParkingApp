using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartParkingApp.ClassLibrary.Models;

namespace SmartParkingApp.ClassLibrary.Data
{
    public class LocalJsonDb
    {
        /// <summary>
        /// Stores users in sorted by Id order
        /// </summary>
        public SortedDictionary<int, User> Users { get; private set; }

        /// <summary>
        /// Stores all parkingsessions
        /// </summary>
        public List<ParkingSession> ParkingSessions { get; private set; }

        public LocalJsonDb()
        {

        }
    }
}

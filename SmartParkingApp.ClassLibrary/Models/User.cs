namespace SmartParkingApp.ClassLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // The password should be stored in a hashed form
        public string PasswordHash { get; set; }
        public string CarPlateNumber { get; set; }
        public string Phone { get; set; }
        public string UserRole { get; set; }
    }
}

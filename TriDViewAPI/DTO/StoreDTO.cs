using TriDViewAPI.Models;

namespace TriDViewAPI.DTO
{
    public class StoreDTO
    {
        public int Id { get; set; }
        //public int UserID { get; set; }
        //public User UserRegistered { get; set; } = new User();
        //public DateTimeOffset DateTimeRegistered { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public string LogoKey { get; set; }
        public int PlanID { get; set; }
        public bool IsActive { get; set; }
        public string Base64File { get; set; }
    }
}

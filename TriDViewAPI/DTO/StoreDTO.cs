using TriDViewAPI.Models;

namespace TriDViewAPI.DTO
{
    public class StoreDTO
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public string? LogoKey { get; set; }
        public int PlanID { get; set; }
        public bool? IsActive { get; set; }
        //public byte[]? LogoBytes { get; set; } // Renamed to LogoBytes for clarity
        //public IFormFile formFile { get; set; }
        public string? Base64File { get; set; }
    }
}

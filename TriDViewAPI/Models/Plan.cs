namespace TriDViewAPI.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public List<Store> Stores { get; set; } = new List<Store>();
    }
}

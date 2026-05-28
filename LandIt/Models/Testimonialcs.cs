namespace LandIt.Models
{
    public class Testimonial
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

        
        public int? BookingId { get; set; } // Which booking it came from 
        public Booking? Booking { get; set; }

        public string Content { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false; // admin approves before showing
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
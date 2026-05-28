namespace LandIt.Models
{
    public class Recruiter
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public Region Region { get; set; }
        public decimal HourlyRate { get; set; }
        public RecruiterStatus Status { get; set; } = RecruiterStatus.Pending;
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }
        public ICollection<RecruiterReview> Reviews { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RecruiterAvailability> Availabilities { get; set; }
    }
}

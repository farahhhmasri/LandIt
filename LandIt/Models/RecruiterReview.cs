namespace LandIt.Models
{
    public class RecruiterReview
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int RecruiterId { get; set; }
        public Recruiter Recruiter { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

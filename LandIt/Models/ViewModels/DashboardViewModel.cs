namespace LandIt.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string? Search { get; set; }

        public List<Recruiter> Recruiters { get; set; } = new();

        public Region? RegionFilter { get; set; }

        public RecruiterStatus? StatusFilter { get; set; }
    }
}

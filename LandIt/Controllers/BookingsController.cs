using LandIt.Data;
using LandIt.Models;
using LandIt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LandIt.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingsController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Create Booking - Show Form
        [HttpGet]
        public async Task<IActionResult> Create(int recruiterId)
        {
            var recruiter = await _context.Recruiters
                .Include(r => r.TimeSlots)
                .FirstOrDefaultAsync(r => r.Id == recruiterId);

            if (recruiter == null)
                return NotFound();

            var availableSlots = recruiter.TimeSlots
                .Where(t =>
                    !t.IsBooked &&
                    t.IsActive &&
                    t.StartTime > DateTime.UtcNow)
                .OrderBy(t => t.StartTime)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.StartTime:dd MMM yyyy | hh:mm tt} - {t.EndTime:hh:mm tt}"
                })
                .ToList();

            var model = new BookingViewModel
            {
                RecruiterId = recruiter.Id,
                RecruiterName = recruiter.FullName,
                RecruiterTitle = recruiter.Title,
                Company = recruiter.Company,
                HourlyRate = recruiter.HourlyRate,
                AvailableSlots = availableSlots
            };

            return View(model);
        }


        // Create Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var recruiter = await _context.Recruiters
                .Include(r => r.TimeSlots)
                .FirstOrDefaultAsync(r => r.Id == model.RecruiterId);

            if (recruiter == null)
                return NotFound();

            var selectedSlot = await _context.TimeSlots
                .FirstOrDefaultAsync(t =>
                    t.Id == model.SelectedTimeSlotId &&
                    !t.IsBooked &&
                    t.IsActive);

            if (selectedSlot == null)
            {
                ModelState.AddModelError("", "Selected slot is unavailable.");
            }

            if (!ModelState.IsValid)
            {
                model.AvailableSlots = recruiter.TimeSlots
                    .Where(t =>
                        !t.IsBooked &&
                        t.IsActive &&
                        t.StartTime > DateTime.UtcNow)
                    .OrderBy(t => t.StartTime)
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.StartTime:dd MMM yyyy | hh:mm tt} - {t.EndTime:hh:mm tt}"
                    })
                    .ToList();

                return View(model);
            }

            var booking = new Booking
            {
                UserId = user.Id,
                RecruiterId = recruiter.Id,
                TimeSlotId = selectedSlot.Id,

                JobTitle = model.JobTitle,
                CompanyTarget = model.CompanyTarget,
                CandidateNotes = model.CandidateNotes,
                Notes = model.Notes,

                Status = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            selectedSlot.IsBooked = true;

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            TempData["ToastMessageSuccess"] =
                "Your session has been booked successfully.";

            return RedirectToAction(nameof(MyBookings));
        }


        // List Bookings
        public async Task<IActionResult> MyBookings()
        {
            return RedirectToAction("MyBookings", "Account");
        }

        // Cancel Booking
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var booking = await _context.Bookings
                .Include(b => b.TimeSlot)
                .FirstOrDefaultAsync(b =>
                    b.Id == id &&
                    b.UserId == user.Id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Cancelled;

            if (booking.TimeSlot != null)
            {
                booking.TimeSlot.IsBooked = false;
            }

            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["ToastMessageSuccess"] =
                "Booking cancelled successfully.";

            return RedirectToAction(nameof(MyBookings));
        }
    }
}
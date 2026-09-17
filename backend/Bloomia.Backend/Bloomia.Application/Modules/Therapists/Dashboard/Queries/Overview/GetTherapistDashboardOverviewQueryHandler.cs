using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Overview
{
    public class GetTherapistDashboardOverviewQueryHandler(IAppDbContext ctx, IAppCurrentUser currentUser) : IRequestHandler<GetTherapistDashboardOverviewQuery, TherapistDashboardOverviewDto>
    {
        public async Task<TherapistDashboardOverviewDto> Handle(GetTherapistDashboardOverviewQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            if (userId is null)
                throw new BloomiaBusinessRuleException("AUTH", "Korisnik nije autentifikovan.");

            var therapistId = await ctx.Therapists
               .Where(x => x.UserId == userId)
               .Select(x => (int?)x.Id).FirstOrDefaultAsync(ct);

            if (therapistId is null)
                throw new BloomiaBusinessRuleException("THERAPIST", "Terapeut nije pronadjen.");

            var now = DateTime.UtcNow;

            var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var nextMonth = monthStart.AddMonths(1);

            var monthlyAppointments = ctx.Appointments
                .AsNoTracking()
                .Where(x =>
                     x.TherapistAvailability.TherapistId == therapistId &&
                     x.ScheduledAtUtc >= monthStart &&
                     x.ScheduledAtUtc < nextMonth);

            var appointmentsThisMonth = await monthlyAppointments.CountAsync(ct);
            var pastAppointmentsThisMonth = await monthlyAppointments.CountAsync(x => x.ScheduledAtUtc.AddHours(1) <= now, ct);

            var activeClients = await monthlyAppointments
                    .Select(x => x.ClientId).Distinct().CountAsync(ct);

            var averageRating = await ctx.Therapists
                .Where(x => x.Id == therapistId.Value)
                .Select(x => x.RatingAvg).FirstAsync(ct);

            var upcomingAppointments = await ctx.Appointments.AsNoTracking()
                .Where(x => x.TherapistAvailability.TherapistId == therapistId &&
                            x.ScheduledAtUtc >= now)
                .OrderBy(x => x.ScheduledAtUtc)
                .Take(5)
                .Select(x => new TherapistUpcomingAppointmentDto
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.Client.User.Firstname + " " + x.Client.User.Lastname,
                    ScheduledAtUtc = x.ScheduledAtUtc,
                    SessionType = x.SessionType
                }).ToListAsync(ct);

            foreach(var appointment in upcomingAppointments)
            {
                appointment.ScheduledAtUtc = DateTime.SpecifyKind(appointment.ScheduledAtUtc, DateTimeKind.Utc);
            }

            return new TherapistDashboardOverviewDto
            {
                AppointmentsThisMonth = appointmentsThisMonth,
                PastAppointmentsThisMonth = pastAppointmentsThisMonth,
                ActiveClients = activeClients,
                AverageRating = (float)Math.Round(averageRating, 1),
                UpcomingAppointments = upcomingAppointments
            };
        }
    }
}

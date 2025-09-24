using System;


namespace misas_thai_street_cuisine_2._0.Services;

public class DeadlineService
{
	// Returns the second and fourth Monday of the given month at 5pm
	public List<DateTime> GetSecondAndFourthMondayDeadline(DateTime from)
	{
		var mondays = new List<DateTime>();
		var firstDay = new DateTime(from.Year, from.Month, 1);
		var day = firstDay;
		while (mondays.Count < 4)
		{
			if (day.DayOfWeek == DayOfWeek.Monday)
			{
				mondays.Add(day.AddHours(17));
			}
			day = day.AddDays(1);
		}
		return new List<DateTime> { mondays[1], mondays[3] };
	}

	// Returns the second and fourth Wednesday of the given month
	public List<DateTime> GetSecondAndFourthWednesday(DateTime from)
	{
		var wednesdays = new List<DateTime>();
		var firstDay = new DateTime(from.Year, from.Month, 1);
		var day = firstDay;
		while (wednesdays.Count < 4)
		{
			if (day.DayOfWeek == DayOfWeek.Wednesday)
			{
				wednesdays.Add(day);
			}
			day = day.AddDays(1);
		}
		return new List<DateTime> { wednesdays[1], wednesdays[3] };
	}

	// Returns the next available delivery Wednesday and deadline Monday
	public (DateTime deliveryWednesday, DateTime deadlineMonday) GetNextAvailableDelivery(DateTime now)
	{
		var thisMonthMondays = GetSecondAndFourthMondayDeadline(now);
		var thisMonthWednesdays = GetSecondAndFourthWednesday(now);
		var nextMonth = now.AddMonths(1);
		var nextMonthMondays = GetSecondAndFourthMondayDeadline(nextMonth);
		var nextMonthWednesdays = GetSecondAndFourthWednesday(nextMonth);

		// Find the next available delivery and deadline
		for (int i = 0; i < 2; i++)
		{
			if (now <= thisMonthMondays[i])
			{
				return (thisMonthWednesdays[i], thisMonthMondays[i]);
			}
		}
		// If past both deadlines, return next month's second Wednesday and Monday
		return (nextMonthWednesdays[0], nextMonthMondays[0]);
	}

	// Returns the next two available delivery Wednesdays
	public List<DateTime> GetUpcomingDeliveryWednesdays(DateTime now)
	{
		var dates = new List<DateTime>();
		var (firstDelivery, _) = GetNextAvailableDelivery(now);
		dates.Add(firstDelivery);

		// Find the next delivery after the first
		var month = firstDelivery.Month;
		var year = firstDelivery.Year;
		var wednesdays = GetSecondAndFourthWednesday(new DateTime(year, month, 1));
		if (firstDelivery == wednesdays[0])
		{
			dates.Add(wednesdays[1]);
		}
		else
		{
			// If first is the fourth Wednesday, get next month's second Wednesday
			var nextMonthWednesdays = GetSecondAndFourthWednesday(new DateTime(year, month, 1).AddMonths(1));
			dates.Add(nextMonthWednesdays[0]);
		}
		return dates;
	}
}

using System;


namespace misas_thai_street_cuisine_2._0.Services;

public class DeadlineService
{
	// Manual override dates - set these to override calculated dates
	public DateTime? ManualFirstDeliveryDate { get; set; } = null;
	public DateTime? ManualSecondDeliveryDate { get; set; } = null;
	// Returns the Monday deadlines for the second and fourth Wednesday deliveries at 5pm
	public List<DateTime> GetSecondAndFourthMondayDeadline(DateTime from)
	{
		// Get the second and fourth Wednesdays first
		var wednesdays = GetSecondAndFourthWednesday(from);
		var deadlines = new List<DateTime>();
		
		// Calculate the Monday before each Wednesday at 5:00 PM
		foreach (var wednesday in wednesdays)
		{
			// Monday is 2 days before Wednesday
			var mondayDeadline = wednesday.AddDays(-2).Date.AddHours(17); // 5:00 PM
			deadlines.Add(mondayDeadline);
		}
		
		return deadlines;
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
		// Check for manual overrides first
		if (IsUsingManualDates())
		{
			var manualDeadline = GetManualDeadlineMonday(now);
			var firstDelivery = ManualFirstDeliveryDate!.Value;
			
			// If we're still before the manual deadline, return first manual delivery
			if (now <= manualDeadline)
			{
				return (firstDelivery, manualDeadline);
			}
			// If past the first manual deadline but before second delivery, return second delivery
			else if (ManualSecondDeliveryDate.HasValue && now < ManualSecondDeliveryDate.Value)
			{
				// Calculate deadline for second delivery (Monday before at 5pm)
				var secondDelivery = ManualSecondDeliveryDate.Value;
				
				int daysToSubtract;
				switch (secondDelivery.DayOfWeek)
				{
					case DayOfWeek.Monday: daysToSubtract = 7; break;
					case DayOfWeek.Tuesday: daysToSubtract = 1; break;
					case DayOfWeek.Wednesday: daysToSubtract = 2; break;
					case DayOfWeek.Thursday: daysToSubtract = 3; break;
					case DayOfWeek.Friday: daysToSubtract = 4; break;
					case DayOfWeek.Saturday: daysToSubtract = 5; break;
					case DayOfWeek.Sunday: daysToSubtract = 6; break;
					default: daysToSubtract = 1; break;
				}
				
				var secondDeadline = secondDelivery.AddDays(-daysToSubtract).Date.AddHours(17);
				return (secondDelivery, secondDeadline);
			}
		}
		
		// Fall back to calculated dates
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
		
		// Check for manual overrides first
		if (ManualFirstDeliveryDate.HasValue && ManualSecondDeliveryDate.HasValue)
		{
			// Use manual dates if both are set
			dates.Add(ManualFirstDeliveryDate.Value);
			dates.Add(ManualSecondDeliveryDate.Value);
			return dates;
		}
		
		// Fall back to calculated dates
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
	
	// Helper method to set manual delivery dates
	public void SetManualDeliveryDates(DateTime firstDate, DateTime secondDate)
	{
		ManualFirstDeliveryDate = firstDate;
		ManualSecondDeliveryDate = secondDate;
	}
	
	// Helper method to clear manual dates and return to calculated dates
	public void ClearManualDeliveryDates()
	{
		ManualFirstDeliveryDate = null;
		ManualSecondDeliveryDate = null;
	}
	
	// Helper method to check if manual dates are active
	public bool IsUsingManualDates()
	{
		return ManualFirstDeliveryDate.HasValue && ManualSecondDeliveryDate.HasValue;
	}
	
	// Get the deadline Monday for manual delivery dates
	public DateTime GetManualDeadlineMonday(DateTime now)
	{
		if (!ManualFirstDeliveryDate.HasValue)
			throw new InvalidOperationException("No manual first delivery date set");
			
		// Calculate the Monday before the first manual delivery date at 5:00 PM
		var firstDeliveryDate = ManualFirstDeliveryDate.Value;
		
		// Find the Monday before the delivery date
		int daysToSubtract;
		switch (firstDeliveryDate.DayOfWeek)
		{
			case DayOfWeek.Monday:
				daysToSubtract = 7; // Previous Monday
				break;
			case DayOfWeek.Tuesday:
				daysToSubtract = 1; // Previous Monday
				break;
			case DayOfWeek.Wednesday:
				daysToSubtract = 2; // Previous Monday
				break;
			case DayOfWeek.Thursday:
				daysToSubtract = 3; // Previous Monday
				break;
			case DayOfWeek.Friday:
				daysToSubtract = 4; // Previous Monday
				break;
			case DayOfWeek.Saturday:
				daysToSubtract = 5; // Previous Monday
				break;
			case DayOfWeek.Sunday:
				daysToSubtract = 6; // Previous Monday
				break;
			default:
				daysToSubtract = 1;
				break;
		}
		
		var deadlineMonday = firstDeliveryDate.AddDays(-daysToSubtract).Date.AddHours(17); // 5:00 PM
		return deadlineMonday;
	}
}

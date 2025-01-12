using System;
using System.Runtime.CompilerServices;

public struct SimDateTime
{
    const ushort FEBRUARY_LEAP_DAYS_COUNT = 29;

    public ushort Hour;
    public ushort Day;
    public ushort Month;
    public ushort Year;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimDateTime(ushort hour, ushort day, ushort month, ushort year)
    {
        if (hour > 23 || day < 1 || day > MonthToDaysCount(month, year) || month < 1 || month > 12)
            throw new ArgumentException($"SimDateTime :: Invalid start date ({hour}, {day}, {month}, {year})!");

        Hour = hour;
        Day = day;
        Month = month;
        Year = year;
    }

    public void Tick()
    {
        Hour++;

        if (Hour > 23)
        {
            Hour = 0;
            Day++;

            ushort daysCount = MonthToDaysCount(Month, Year);

            if (Day > daysCount)
            {
                Day = 1;
                Month++;

                if (Month > 12)
                {
                    Month = 1;
                    Year++;
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Difference(SimDateTime a, SimDateTime b, out bool dayChanged, out bool monthChanged, out bool yearChanged)
    {
        dayChanged = a.Day != b.Day;
        monthChanged = b.Month != a.Month;
        yearChanged = a.Year != b.Year;
    }

    static ushort MonthToDaysCount(ushort month, ushort year)
    {
        bool monthIsFebruary = month == 2;
        bool yearIsLeap = year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        ushort daysCountDefault = MonthToDaysCountDefault(month);

        return monthIsFebruary && yearIsLeap ? FEBRUARY_LEAP_DAYS_COUNT : daysCountDefault;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static ushort MonthToDaysCountDefault(ushort month) => month switch
    {
        1 => 31,
        2 => 28,
        3 => 31,
        4 => 30,
        5 => 31,
        6 => 30,
        7 => 31,
        8 => 31,
        9 => 30,
        10 => 31,
        11 => 30,
        12 => 31,

        _ => throw new Exception($"SimDateTime :: MonthToDaysCountDefault :: Invalid month ({month})!")
    };
}
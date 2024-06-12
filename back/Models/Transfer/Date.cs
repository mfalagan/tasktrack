namespace back.Models.Transfer
{
    public class DateData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public DateData(System.DateOnly date)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        public static implicit operator System.DateOnly(DateData date)
        {
            return new System.DateOnly(date.Year, date.Month, date.Day);
        }

        public static implicit operator DateData(System.DateOnly date)
        {
            return new DateData(date);
        }
    }
}
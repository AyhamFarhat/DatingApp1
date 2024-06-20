namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob){
            Console.WriteLine($"dateofbirth: {dob}"); // Should be the correct age based on the current date.
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            Console.WriteLine($"today: {today}"); // Should be the correct age based on the current date.
            var age = today.Year - dob.Year;
            if(dob > today.AddYears(-age)) age--;
            Console.WriteLine($"Calculated Age: {age}"); // Should be the correct age based on the current date.
            return age;

        }
    }
}
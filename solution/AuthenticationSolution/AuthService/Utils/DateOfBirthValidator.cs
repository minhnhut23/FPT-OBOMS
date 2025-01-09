namespace AuthService.Utils;

public class DateOfBirthValidator
{
    public static bool IsValid(DateTime dateOfBirth, int minAge = 0, int maxAge = 99)
    {
        var today = DateTime.Today;

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age >= minAge && age <= maxAge;
    }
}

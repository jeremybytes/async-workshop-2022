namespace DataProcessor.Library;

public record Person(int ID, string GivenName, string FamilyName,
    DateTime StartDate, int Rating, string FormatString)
{
    public override string ToString()
    {
        if (string.IsNullOrEmpty(FormatString))
            return $"{GivenName} {FamilyName}";
        return string.Format(FormatString, GivenName, FamilyName);
    }
}

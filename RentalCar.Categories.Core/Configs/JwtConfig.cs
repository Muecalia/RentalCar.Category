namespace RentalCar.Categories.Core.Configs;

public class JwtConfig
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; } = 0;
    //public int DurationInHour { get; set; }
}
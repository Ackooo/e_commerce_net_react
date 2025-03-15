namespace Domain.Shared.Configurations;

public class StripeSettings
{
    public required string PublishableKey { get; set; }
    public required string SecretKey { get; set; }
    public required string WhSecret { get; set; }

}

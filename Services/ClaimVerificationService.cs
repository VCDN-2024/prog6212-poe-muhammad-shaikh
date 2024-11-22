namespace CMCSApp1.Services

{
    using CMCSApp1.Models;

    public class ClaimVerificationService
    {
        private const double MaxHoursWorked = 40.0; // Maximum allowed hours
        private const double MinHourlyRate = 20.0;  // Minimum hourly rate
        private const double MaxHourlyRate = 100.0; // Maximum hourly rate
        private const double MaxTotalPayment = 5000.0; // Maximum total payment allowed

        // Validates the claim based on predefined rules
        public string ValidateClaim(Claim claim)
        {
            // Check hours worked
            if (claim.HoursWorked > MaxHoursWorked)
                return "Hours worked exceeds the maximum limit of 40.";

            // Check hourly rate
            if (claim.HourlyRate < MinHourlyRate || claim.HourlyRate > MaxHourlyRate)
                return "Hourly rate must be between R20 and R100.";

            // Check total payment (Total payment is HoursWorked * HourlyRate)
            double totalPayment = claim.HoursWorked * claim.HourlyRate;
            if (totalPayment > MaxTotalPayment)
                return "Total payment exceeds the maximum allowed amount of R5000.";

            // If all validations pass
            return null; // No validation errors
        }
    }
}

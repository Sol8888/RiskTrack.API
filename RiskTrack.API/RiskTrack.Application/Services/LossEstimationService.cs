namespace RiskTrack.API.RiskTrack.Application.Services
{
    public class LossEstimationService
    {
        public decimal CalculatePD(decimal dcr, int incidentDurationMinutes)
        {
            return dcr * incidentDurationMinutes;
        }

        public decimal CalculatePI(decimal pd)
        {
            return 0.4m * pd;
        }

        public decimal CalculateCR(int responseHours, decimal hourlyRate)
        {
            return responseHours * hourlyRate;
        }

        public decimal CalculateDPF(long piiRecords, decimal penaltyPerRecord)
        {
            return (piiRecords * penaltyPerRecord) / 1_000_000m;
        }

        public decimal CalculateDIRF(int errorCount, decimal reworkCost)
        {
            return errorCount * reworkCost;
        }

        public decimal CalculateCES(long piiRecords, decimal penaltyPerRecord)
        {
            return piiRecords * penaltyPerRecord;
        }

        public decimal CalculateLM(decimal pd, decimal pi, decimal cr, decimal dpf, decimal dirf, decimal ces)
        {
            return pd + pi + cr + dpf + dirf + ces;
        }
    }
}

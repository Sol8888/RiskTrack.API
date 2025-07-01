using RiskTrack.API.RiskTrack.Domain.Entities;

namespace RiskTrack.API.RiskTrack.Application.Services
{
    public class RiskCalculationService
    {
        public decimal CalculateVNA(Asset asset)
        {
            var ctp = asset.AnnualLicenseCostUsd.GetValueOrDefault() +
                      asset.AnnualSupportHours.GetValueOrDefault() * asset.EngineerHourlyRateUsd.GetValueOrDefault();

            var dcr = asset.RevenuePerMinuteUsd.GetValueOrDefault();

            var adi = asset.CriticalFlowPercentage.GetValueOrDefault() *
                     (asset.RevenuePerMinuteUsd.GetValueOrDefault() * 60 * 24 * 365);

            return ctp + dcr + adi;
        }

        public decimal CalculateSI(Asset asset)
        {
            var C = Math.Min(10, asset.AnnualCriticalVulnerabilities.GetValueOrDefault());
            var I = Math.Min(10, asset.DataCorruptionErrors.GetValueOrDefault());
            var D = (asset.MonthlyDowntimeMin.GetValueOrDefault() / 43200m) * 1000;

            var max = Math.Max(C, Math.Max(I, D));
            var avg = (C + I + D) / 3;

            return (0.5m * max) + (0.5m * avg);
        }

        public decimal CalculateALE(decimal lef, decimal lm)
        {
            return lef * lm;
        }
    }
}

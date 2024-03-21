namespace VatCalculator.Domain.Models;

public class TaxRate
{
    public TaxRate(AustrianVatRates vatRate)
    {
        ValidateVatRate(vatRate);

        VatRate = vatRate;
    }

    public AustrianVatRates VatRate { get; init; }

    private decimal VatRateDecimal => (decimal)VatRate / 100;

    public decimal CalculateVatAmount(decimal netAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(netAmount, 0);

        return netAmount * VatRateDecimal;
    }

    public decimal CalculateGrossAmount(decimal netAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(netAmount, 0);

        return netAmount * (1 + VatRateDecimal);
    }

    public decimal CalculateNetAmount(decimal grossAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(grossAmount, 0);

        return grossAmount / (1 + VatRateDecimal);
    }

    public decimal CalculateNetAmountFromVatAmount(decimal vatAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(vatAmount, 0);

        return vatAmount / VatRateDecimal;
    }

    private void ValidateVatRate(AustrianVatRates vatRate)
    {
        if (!Enum.IsDefined(typeof(AustrianVatRates), vatRate))
        {
            throw new ArgumentOutOfRangeException(nameof(vatRate));
        }
    }
}

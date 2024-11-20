

namespace Application.Helpers
{
    public static class PriceCalculator
    {
        public static double CalculatePrice(double printfullPrice)
        {
            double percentageToBeAdded = 30;
            double calculatedPrice = 0;

            calculatedPrice = printfullPrice + printfullPrice / 100 * percentageToBeAdded; 

            return calculatedPrice;
        }
    }
}

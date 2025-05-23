

namespace Application.Helpers
{
    public static class PriceCalculator
    {
        public static decimal AddKaymakToItemPrice(decimal printfullPrice)
        {
            decimal kaymakInPercent = 30;
            decimal calculatedPrice = 0;

            calculatedPrice = printfullPrice + printfullPrice / 100 * kaymakInPercent; 

            return calculatedPrice;
        }

        public static decimal AddKaymakToShiping(decimal shippingPrice)
        {
            decimal kaymakInPercent = 30;
            decimal calculatedPrice = 0;

            calculatedPrice = shippingPrice + shippingPrice / 100 * kaymakInPercent;

            return calculatedPrice;
        }
    }
}

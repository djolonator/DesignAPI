
namespace Application.Constants
{
    public static class Product
    {
        //product
        public const int productId = 1;

        //variants
        public const int in11x14 = 14125;
        public const int in16x23 = 19528;
        public const int in20x30 = 16365;

        public static Dictionary<int, int> ProductVariants()        
        { 
            var productVariants = new Dictionary<int, int>();
            productVariants.Add(1, 14125);
            productVariants.Add(2, 19528);
            productVariants.Add(3, 16365);

            return productVariants;
        }

    }
}

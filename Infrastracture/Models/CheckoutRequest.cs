
namespace Infrastracture.Models
{
    public class CheckoutRequest
    {
       public Recipient? Recipient { get; set; }
       public List<CartItem> CartItems { get; set; } = new List<CartItem>();
       public string? PaypallOrderId { get; set; }
       public string? PrintfullOrderId { get; set; }
    }

    public class Recipient
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }

    public class CartItem
    {
        public int DesignId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

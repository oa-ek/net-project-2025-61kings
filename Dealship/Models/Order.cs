namespace Dealship.Models
{

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Навігаційні властивості
        public virtual Customer Customer { get; set; }
        public virtual Car Car { get; set; }
    }
}

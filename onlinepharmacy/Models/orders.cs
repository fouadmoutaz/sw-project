namespace onlinepharmacy.Models
{
    public class orders
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public int userid { get; set; }
        public int quantity { get; set; }
        public DateTime orderdate { get; set; }

    }
}

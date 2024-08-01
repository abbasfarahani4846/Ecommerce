namespace Ecommerce.Models.ViewModels
{
    public class ProductCartViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ImageName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int RowSumPrice  { get; set; }
    }
}

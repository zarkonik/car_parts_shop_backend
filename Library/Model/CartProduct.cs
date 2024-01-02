namespace Library.Model
{
	public class CartProduct
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Picture { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public int selectedCount { get; set; }
	}
}

namespace Library.Model
{
	public class Group
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Picture { get; set; }

		public string Details { get; set; }

		public int HasVehicle { get; set; }

		public TypeVehicle Type { get; set; }
	}
}

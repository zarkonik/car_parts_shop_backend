using System.Data;

namespace Library.Model
{
	public class Transaction
	{
		public int Id { get; set; }	

		public string UserId { get; set; }
		
		public bool Status { get; set;}

		public string NumberOfVehicle { get; set; }

		public string Name { get; set; }

		public string Lastname { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string  DataTime { get; set; }
	
	}
}

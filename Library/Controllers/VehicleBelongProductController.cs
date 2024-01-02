using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class VehicleBelongProductController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputVehicleBelongProduct/{vehicle}/{product}")]
		[HttpPost]
		public ActionResult InputVehicleBelongProduct(int vehicle, int product)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [VehicleBelongProduct] OUTPUT INSERTED.Id  values ( @product_id, @vehicle_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@product_id", product);
					cmd.Parameters.AddWithValue("@vehicle_id", vehicle);

					con.Open();
					int i = (int)cmd.ExecuteScalar();
					if (i < 0)
					{
						return BadRequest(i);
					}
					con.Close();

				}
			}
			return Ok();
		}



		[Route("GetProductVehicle/{product}")]
		[HttpGet]
		public ActionResult<List<Vehicle>> GetProductVehicle(int product)
		{
			List<Vehicle> listVehicle = new List<Vehicle>();

			string query = "select *  from [Vehicle]" +
			"INNER JOIN VehicleBelongProduct " +
			"ON VehicleBelongProduct.vehicle_id = [Vehicle].Id " +
			"WHERE  VehicleBelongProduct.product_id =" + product ; //napravi drugi upit

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Vehicle vehicle = new Vehicle();
					vehicle.Id = Convert.ToInt32(rdr["Id"]);
					vehicle.Brand = Convert.ToString(rdr["Brand"]);
					vehicle.Model = Convert.ToString(rdr["Model"]);
					vehicle.Series = Convert.ToString (rdr["Series"]);
				

					listVehicle.Add(vehicle);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listVehicle;

		}


		[Route("DeleteVehicleBelongProduct/{vehicle}/{product}")]
		[HttpDelete]
		public ActionResult DeleteVehicleBelongProduct(int vehicle,int product)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "DELETE FROM [VehicleBelongProduct] where VehicleBelongProduct.vehicle_id =" + vehicle + "AND VehicleBelongProduct.product_id =" + product;
			SqlCommand cmd = new SqlCommand(query, connection);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
			return Ok();
		}


	}
}

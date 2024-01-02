using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VehicleContainGroupController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputVehicleContainGroup/{vehicle}/{group}")]
		[HttpPost]
		public ActionResult InputVehicleContainGroup(int vehicle, int group)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [VehicleContainGroup] OUTPUT INSERTED.Id  values (@vehicle_id, @group_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@vehicle_id", vehicle);
					cmd.Parameters.AddWithValue("@group_id", group);

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

		[Route("GetProductGroupVehicle/{group}/{vehicle}")]
		[HttpGet]
		public ActionResult<List<Product>> GetProductGroupVehicle(int group,int vehicle)
		{
			List<Product> listProduct = new List<Product>();

			string query = "select *  from [Product] AS p \r\n INNER JOIN ProductBelongGroup AS pg \r\nON  pg.product_id = p.Id \r\nINNER JOIN VehicleBelongProduct AS vp \r\nON vp.product_id = p.id \r\nWHERE vp.vehicle_id ="+vehicle+"AND pg.group_id =" + group + "\r\n" ; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Product product = new Product();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;

		}
		
	}
}

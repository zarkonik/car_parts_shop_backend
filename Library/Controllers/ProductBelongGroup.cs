using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductBelongGroup : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputProductInGroup/{product}/{group}")]
		[HttpPost]
		public ActionResult InputProductInGroup(int product, int group)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [ProductBelongGroup] OUTPUT INSERTED.Id  values (@group_id, @product_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@group_id", group);
					cmd.Parameters.AddWithValue("@product_id", product);

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

		[Route("GetProductFromGroup/{group}")]
		[HttpGet]
		public ActionResult<List<Product>> GetProductFromGroup(int group)
		{
			List<Product> products = new List<Product>();

			string query = "select * from [Product] INNER JOIN [ProductBelongGroup] ON ProductBelongGroup.product_id = Product.Id where ProductBelongGroup.group_id =" + group; //napravi drugi upit
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

					products.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return products;
		}



	}
}

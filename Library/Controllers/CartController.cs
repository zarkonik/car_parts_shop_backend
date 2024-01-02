using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CartController : ControllerBase
    {
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputProductInCart/{product}/{user}/{count}")]
		[HttpPost]
		public ActionResult<int> InputProductInCart( int user, int product,int count)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Cart] OUTPUT INSERTED.Id values (@user_id, @product_id,@selectedCount)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@user_id",user);
					cmd.Parameters.AddWithValue("@product_id", product);
					cmd.Parameters.AddWithValue("@selectedCount", count);

					con.Open();
					int i = (int)cmd.ExecuteScalar();
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();
		}

		[Route("GetProductsFromCart/{user}")]
		[HttpGet]
		public ActionResult<List<CartProduct>> GetProductsFromCart(int user)
		{
			List<CartProduct> listProduct = new List<CartProduct>();

			string query = "select * from [Product] JOIN Cart ON Cart.product_id = Product.Id where Cart.user_id  =  " + user; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					CartProduct product = new CartProduct();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);
					product.selectedCount = Convert.ToInt32(rdr["selectedCount"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;
		}


		[Route("UpdateSelectedCountInCart/{product_id}/{user_id}/{count}")]
		[HttpPut]
		public ActionResult UpdateSelectedCountInCart(int product_id,int user_id,int count)
		{
	

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "UPDATE [CART] SET selectedCount = @selectedCount Where  user_id = @user_id  AND product_id = @product_id ";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Parameters.AddWithValue("@product_id", product_id);
					cmd.Parameters.AddWithValue("@user_id", user_id);
					cmd.Parameters.AddWithValue("@selectedCount", count);

					con.Open();
					int i = cmd.ExecuteNonQuery();
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();

		}


		[Route("DeleteProductFromCart/{product}/{user}")]
		[HttpDelete]
		public ActionResult DeleteProductFromCart(int product,int user)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "DELETE FROM [Cart] where product_id='" + product + "' AND user_id='" + user + "'";
			SqlCommand cmd = new SqlCommand(query, connection);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
			return Ok();
		}



	}
}

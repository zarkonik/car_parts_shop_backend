using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductInformationController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetProductInformations")]
		[HttpGet]
		public ActionResult<List<ProductInformation>> GetProductInformations()
		{
			List<ProductInformation> listProductInformation = new List<ProductInformation>();

			string query = "select * from [ProductInformation]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					ProductInformation productInformation = new ProductInformation();
					productInformation.Id = Convert.ToInt32(rdr["Id"]);
					productInformation.Name = Convert.ToString(rdr["Name"]);

					listProductInformation.Add(productInformation);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProductInformation;
		}

		[Route("GetProductInformation/{product}")]
		[HttpGet]
		public ActionResult<List<FullProductInformation>> GetProductInformation(int product)
		{

			List<FullProductInformation> listProductInformation = new List<FullProductInformation>();

			string query = "select * from [ProductInformation] JOIN [ProductInformationData] ON ProductInformationData.productInformation_id = ProductInformation.Id  where ProductInformation.product_id =" +product; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					FullProductInformation productInformation = new FullProductInformation();
					productInformation.Id = Convert.ToInt32(rdr["Id"]);
					productInformation.Name = Convert.ToString(rdr["Name"]);
					productInformation.Data = Convert.ToString(rdr["Data"]);

					listProductInformation.Add(productInformation);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProductInformation;
		}




		[Route("InputProductInformation/{product}")]
		[HttpPost]
		public ActionResult<int> InputProductInformation(int product,[FromBody] ProductInformation productInformation)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [ProductInformation] OUTPUT INSERTED.Id values (@Name,@Product_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", productInformation.Name);
					cmd.Parameters.AddWithValue("@Product_id", product );

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
	}
}

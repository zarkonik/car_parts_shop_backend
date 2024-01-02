using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class MostPopularProductController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("CheckMostPopularExis/{product}/{user}")]
		[HttpGet]
		public ActionResult<bool> CheckMostPopularExis(int product, int user)
		{
			

			string query = "select * from [MostPopularProduct] where MostPopularProduct.product_id ="+product + "And MostPopularProduct.user_id ="+user ; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{


				if(rdr.Read())
				{
					connection.Close();
					return true;
				  }
				else
				{
					connection.Close();
					return false;
				}						
				//command.ExecuteNonQuery();	
			}

		
		}

		[Route("SetMostPopularExis")]
		[HttpPost]
		public ActionResult<int> SetMostPopularExis([FromBody] MostPopular mostPopular)
		{
			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into MostPopularProduct OUTPUT INSERTED.Id values (@product_id , @user_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@product_id", mostPopular.Product);
					cmd.Parameters.AddWithValue("@user_id", mostPopular.User);

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

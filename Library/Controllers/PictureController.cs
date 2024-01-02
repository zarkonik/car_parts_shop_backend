using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PictureController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputPicture/{product}")]
		[HttpPost]
		public ActionResult<int> InputPicture([FromBody] Picture picture, int product)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Picture] OUTPUT INSERTED.Id values ( @Data, @product_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
		
					cmd.Parameters.AddWithValue("@Data", picture.Data);
					cmd.Parameters.AddWithValue("@product_id", product);

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

		[Route("GetPictures/{product}")]
		[HttpGet]
		public ActionResult<List<Picture>> GetPictures(int product)
		{
			List<Picture> listPicture = new List<Picture>();

			string query = "select * from [Picture] where Picture.product_id  =  " + product; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Picture picture = new Picture();
					picture.Id = Convert.ToInt32(rdr["Id"]);
					picture.Data = Convert.ToString(rdr["Data"]);

					listPicture.Add(picture);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listPicture;
		}


	}
}

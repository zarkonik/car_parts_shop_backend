using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class CategoryBelongTypeController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputType/{type}/{category}")]
		[HttpPost]
		public ActionResult InputType(int type,int category)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [CategoryBelongType]  OUTPUT INSERTED.Id values (@type_id, @category_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					
						cmd.Connection = con;
						cmd.Parameters.AddWithValue("@type_id", type);
						cmd.Parameters.AddWithValue("@category_id", category);


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

	}
}

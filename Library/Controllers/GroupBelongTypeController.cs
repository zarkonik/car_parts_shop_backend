using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class GroupBelongTypeController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputType/{type}/{group}")]
		[HttpPost]
		public ActionResult InputType(int type, int group)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [GroupBelongType]  OUTPUT INSERTED.Id values ( @group_id, @type_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@group_id", group);
					cmd.Parameters.AddWithValue("@type_id", type);


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

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{

	[ApiController]
	[Route("[controller]")]

	public class VehicleContainEngine : ControllerBase
	{

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";

		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";



		[Route("InputEngine/{vehicle}/{engine}")]
		[HttpPost]
		public ActionResult InputEngine(int vehicle, int engine)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [VehicleContainEngine] OUTPUT INSERTED.Id  values (@vehicle_id, @engine_id )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@vehicle_id", vehicle);
					cmd.Parameters.AddWithValue("@engine_id", engine);

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

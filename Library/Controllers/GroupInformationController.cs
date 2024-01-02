using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GroupInformationController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputGroupInformation/{group}")]
		[HttpPost]
		public ActionResult<int> InputGroupInformation([FromBody] GroupInformation groupInformation, int group)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [GroupInformation] OUTPUT INSERTED.Id values (@Name,@group_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", groupInformation.Name);
					cmd.Parameters.AddWithValue("@group_id", group);

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

		[Route("GetGroupInformations/{group}")]
		[HttpGet]
		public ActionResult<List<GroupInformation>> GetGroupInformations( int group)
		{
			List<GroupInformation> listGroupInformation = new List<GroupInformation>();

			string query = "select * from [GroupInformation] where GroupInformation.group_id  =  " +group  ; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					GroupInformation groupInformation = new GroupInformation();
					groupInformation.Id= Convert.ToInt32 (rdr["Id"]);
					groupInformation.Name = Convert.ToString(rdr["Name"]);

					listGroupInformation.Add(groupInformation);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listGroupInformation;
		}


	}
}

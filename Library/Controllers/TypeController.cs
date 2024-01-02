using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class TypeController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetTypes")]
		[HttpGet]
		public ActionResult<List<TypeVehicle>> GetTypes()
		{
			List<TypeVehicle> listTypeVehicle = new List<TypeVehicle>();

			string query = "select * from [Type]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					TypeVehicle type = new TypeVehicle();
					type.Id = Convert.ToInt32(rdr["Id"]);
					type.Name = Convert.ToString(rdr["Name"]);
					type.Picture = Convert.ToString(rdr["Picture"]);
					listTypeVehicle.Add(type);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listTypeVehicle;
		}

		[Route("InputType")]
		[HttpPost]
		public ActionResult<int> InputType([FromBody] TypeVehicle type)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Type] OUTPUT INSERTED.Id values ( @Name , @Picture )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", type.Name );
					cmd.Parameters.AddWithValue("@Picture", type.Picture);

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

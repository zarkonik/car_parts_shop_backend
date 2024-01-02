using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	
	public class EngineController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetEngines")]
		[HttpGet]
		public ActionResult<List<Engine>> GetEngines()
		{
			List<Engine> listEngine = new List<Engine>();

			string query = "select * from [Engine]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Engine engine = new Engine();
					engine.Id = Convert.ToInt32(rdr["Id"]);
					engine.Name = Convert.ToString(rdr["Name"]);
					engine.Power = Convert.ToString(rdr["Power"]);
					engine.Volume = Convert.ToString(rdr["Volume"]);

					listEngine.Add(engine);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listEngine;
		}


		[Route("InputEngine")]
		[HttpPost]
		public ActionResult<int> InputEngine([FromBody] Engine engine)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Engine]  OUTPUT INSERTED.Id values (@Power, @Volume, @Name )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Power", engine.Power );
					cmd.Parameters.AddWithValue("@Volume", engine.Volume);
					cmd.Parameters.AddWithValue("@Name", engine.Name);

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

		[Route("GetEngine/{vehicle}")]
		[HttpGet]
		public ActionResult<Engine> GetEngine(int vehicle)
		{

			Engine engine = new Engine();

			string query = "select * from [Engine] JOIN VehicleContainEngine ON VehicleContainEngine.engine_id = Engine.Id where VehicleContainEngine.vehicle_id ="+ vehicle; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{
				rdr.Read();
		
				engine.Id = Convert.ToInt32(rdr["Id"]);
				engine.Name = Convert.ToString(rdr["Name"]);
				engine.Power = Convert.ToString(rdr["Power"]);
				engine.Volume = Convert.ToString(rdr["Volume"]);
				
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return engine;
		}

		[Route("DeleteProductContainEngine/{engine}/{product}")]
		[HttpDelete]
		public ActionResult DeleteProductContainEngine(int engine, int product)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "DELETE FROM [ProductContainEngine] where ProductContainEngine.engine_id =" + engine + "AND ProductContainEngine.product_id =" + product;
			SqlCommand cmd = new SqlCommand(query, connection);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
			return Ok();
		}





	}
}

using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class ProductContainEngineController : ControllerBase
	{

		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputEngine/{product}/{engine}")]
		[HttpPost]
		public ActionResult InputEngine(int product, int engine)
		{
			
				using (SqlConnection con = new SqlConnection(constr))
				{
					string query = "insert into [ProductContainEngine] OUTPUT INSERTED.Id  values (@product_id, @engine_id )";
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
	
							cmd.Connection = con;
							cmd.Parameters.AddWithValue("@product_id", product);
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



		[Route("GetProductEngine/{product}")]
		[HttpGet]
		public ActionResult<List<Engine>> GetProductEngine(int product)
		{
			List<Engine> listEngine = new List<Engine>();

			string query = "select *  from [Engine]" +
			"INNER JOIN ProductContainEngine " +
			"ON ProductContainEngine.engine_id = [engine].Id " +
			"WHERE  ProductContainEngine.product_id =" + product; //napravi drugi upit

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

	}
}

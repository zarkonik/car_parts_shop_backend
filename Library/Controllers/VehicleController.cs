using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VehicleController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";//

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetVehicles")]
		[HttpGet]
		public ActionResult<List<Vehicle>> GetVehicles()
		{
			List<Vehicle> listVehicle = new List<Vehicle>();

			string query = "select * from [Vehicle]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Vehicle vehicle = new Vehicle();
					vehicle.Id = Convert.ToInt32(rdr["Id"]);
					vehicle.Brand = Convert.ToString(rdr["Brand"]);
					vehicle.Model = Convert.ToString(rdr["Model"]);
					vehicle.Series = Convert.ToString(rdr["Series"]);
					vehicle.Picture = Convert.ToString(rdr["Picture"]);
					vehicle.PictureModel = Convert.ToString(rdr["PictureModel"]);
					vehicle.DateOf = Convert.ToDateTime(rdr["DateOf"]);
					vehicle.DateUntil = Convert.ToDateTime(rdr["DateUntil"]);
					listVehicle.Add(vehicle);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listVehicle;
		}

		[Route("InputVehicle")]
		[HttpPost]
		public ActionResult<int> InputVehicle([FromBody] Vehicle vehicle)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Vehicle]  OUTPUT INSERTED.Id values (@Brand, @Model, @Series, @Picture , @PictureModel , @DateOf, @DateUntil )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Brand", vehicle.Brand  );
					cmd.Parameters.AddWithValue("@Model", vehicle.Model );
					cmd.Parameters.AddWithValue("@Series", vehicle.Series );
					cmd.Parameters.AddWithValue("@Picture", vehicle.Picture);
					cmd.Parameters.AddWithValue("@PictureModel", vehicle.PictureModel );
					cmd.Parameters.AddWithValue("@DateOf", vehicle.DateOf );
					cmd.Parameters.AddWithValue("@DateUntil",  vehicle.DateUntil);

					con.Open();
					int i = (int)cmd.ExecuteScalar() ;
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();
		}


		[Route("GetBrands")]
		[HttpGet]
		public ActionResult<List<String>> GetBrands()
		{
			List<String> listBrand = new List<String>();

			string query = "select Brand from [Vehicle]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					String brand;
				
					brand = Convert.ToString(rdr["Brand"]);
				
					listBrand.Add(brand);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listBrand;
		}

		[Route("GetModel/{brand}")]
		[HttpGet]
		public ActionResult<List<String>> GetModel(string brand)
		{
			List<String> listModel = new List<String>();

			string query = "select Brand , Model from [Vehicle] where Brand='" + brand +"'"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					String model;

					model = Convert.ToString(rdr["Model"]);

					listModel.Add(model);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listModel;
		}

		[Route("GetSeries/{brand}/{model}")]
		[HttpGet]
		public ActionResult<List<String>> GetSeries(string brand, string model)
		{
			List<String> listSeries = new List<String>();

			string query = "select Brand, Model, Series from [Vehicle] where Brand='" + brand + "' AND Model='" + model + "'"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					String series;

					series = Convert.ToString(rdr["Series"]);

					listSeries.Add(series);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listSeries;
		}

		[Route("GetEngine/{brand}/{model}/{series}")]
		[HttpGet]
		public ActionResult<List<Engine>> GetEngine(string brand, string model,string series)
		{
			List<Engine> listEngine = new List<Engine>();

			string query = "select distinct [Engine].Id ,[Power] ,Volume ,[Name] from [Engine] " +
				"JOIN ProductContainEngine ON ProductContainEngine.engine_id = Engine.Id " +
				"JOIN VehicleBelongProduct ON VehicleBelongProduct.product_id= ProductContainEngine.product_id " +
				"JOIN Vehicle ON Vehicle.Id = VehicleBelongProduct.vehicle_id  " +
				"where Brand='" + brand + "' AND Model='" + model + "'" + " AND Series='" + series+ "'"; //napravi drugi upit
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

		[Route("GetVehicle/{brand}/{model}/{series}/{engine}")]
		[HttpGet]
		public ActionResult<Vehicle> GetVehicle(string brand, string model, string series, int engine)
		{
			Vehicle vehicle = new Vehicle();

			string query = "select * from [Vehicle] JOIN VehicleContainEngine ON VehicleContainEngine.vehicle_id = Vehicle.Id where Brand='" + brand + "' AND Model='" + model + "'" + " AND Series=" + series + " AND VehicleContainEngine.engine_id=" + engine; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();

				vehicle.Id = Convert.ToInt32(rdr["Id"]);
				vehicle.Brand = Convert.ToString(rdr["Brand"]);
				vehicle.Model = Convert.ToString(rdr["Model"]);
				vehicle.Series = Convert.ToString(rdr["Series"]);
				vehicle.Picture = Convert.ToString(rdr["Picture"]);
				vehicle.PictureModel = Convert.ToString(rdr["PictureModel"]);
				vehicle.DateOf = Convert.ToDateTime(rdr["DateOf"]);
				vehicle.DateUntil = Convert.ToDateTime(rdr["DateUntil"]);
				

				//command.ExecuteNonQuery();
				connection.Close();
			}

			return vehicle;
		}




	}
}

using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetProducts")]
		[HttpGet]
		public ActionResult<List<Product>> GetProducts()
		{
			List<Product> listProduct = new List<Product>();

			string query = "select * from [Product] "; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Product product = new Product();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32  (rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;
		}

		[Route("GetCountProducts/{group_id}")]
		[HttpGet]
		public ActionResult<int> GetCountProducts(int group_id)
		{
			int count = 0;

			string query = "select COUNT (*) AS number from [Product] JOIN ProductBelongGroup ON ProductBelongGroup.product_id = [Product].Id WHERE ProductBelongGroup.group_id = " + group_id ; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();

				count = Convert.ToInt32(rdr["number"]);
			
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return count;
		}




		[Route("GetDataProducts")]
		[HttpPost]
		public ActionResult<List<Product>> GetDataProducts([FromBody] NameData data)
		{
			List<Product> listProduct = new List<Product>();

			string query = "";

			for(int i=0;i< data.Data.Count;i++)
			{
				if (data.Data[i] != "")
				{
					query += " select Product.* from[Product]" +
						" JOIN[GroupInformationData]" +
						" ON GroupInformationData.product_id = Product.Id" +
						" JOIN[GroupInformation]" +
						" ON GroupInformation.Id = GroupInformationData.groupInformation_id" +
						" where GroupInformation.Name = '" + data.Names[i] + "'" +
						" AND GroupInformationData.Data = '" + data.Data[i] + "'";

					query += " INTERSECT";
				
				}
			}

			query = query.Trim();
			if (query.Contains(" "))
			{
				query = query.Substring(0, query.LastIndexOf(' ')).TrimEnd();
			}



			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Product product = new Product();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;
		}


		[Route("GetProduct/{group}/{sort}/{ppp}/{page}")]
		[HttpGet]
		public ActionResult<List<Product>> GetProduct(int group,string sort,int ppp, int page)
		{
			string query = "select * from [Product] " +
							"JOIN ProductBelongGroup " +
							"ON ProductBelongGroup.product_id = [Product].id" +
							" where ProductBelongGroup.group_id = " + group; //napravi drugi upit


			switch (sort)
			{
				case "Cena rastuce": {
						query += " Order by [Product].price ASC ,Product.Id OFFSET "+ ppp*page +" ROWS FETCH NEXT "+ppp+" ROWS ONLY ";  //napravi drugi upit
						break;	
				};
				case "Cena opadajuce":
					{
						query += " Order by [Product].price DESC ,Product.Id OFFSET "+ ppp*page +" ROWS FETCH NEXT "+ppp+" ROWS ONLY ";  //napravi drugi upit
						break;
					};
				case "Naziv opadajuce":
					{
						query += " Order by [Product].name DESC ,Product.Id OFFSET "+ ppp*page +" ROWS FETCH NEXT "+ppp+" ROWS ONLY "; //napravi drugi upit

						break;
					};
				case "Naziv rastuce":
					{
						query += " Order by [Product].name ASC ,Product.Id OFFSET "+ ppp*page +" ROWS FETCH NEXT "+ppp+" ROWS ONLY "; //napravi drugi upit

						break;
					};
				default: {
						query += " Order by Product.Id OFFSET " + ppp*page +" ROWS FETCH NEXT "+ppp+" ROWS ONLY ";

						break;
					}
			}
			
			List<Product> listProduct = new List<Product>();

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Product product = new Product();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}






			return listProduct;
		}

		[Route("GetProductVehicle")]
		[HttpPost]
		public ActionResult<List<Product>> GetProductVehicle([FromBody]VehicleSet vehicle)
		{
			string query = "select * from [Product] " +
				"JOIN ProductBelongGroup ON ProductBelongGroup.product_id = [Product].id " +
				"JOIN VehicleBelongProduct ON VehicleBelongProduct.product_id = [Product].id " +
				"JOIN Vehicle ON VehicleBelongProduct.vehicle_id = Vehicle.Id " +
				"JOIN ProductContainEngine ON ProductContainEngine.product_id = [Product].Id " +
				"JOIN Engine ON Engine.Id = ProductContainEngine.engine_id " +
				"where ProductBelongGroup.group_id = "+vehicle.Group +
				"AND Vehicle.Brand = '"+vehicle.Brand+"'" +
				"AND Vehicle.Model = '"+vehicle.Model +"'"+
				"AND Series = '"+vehicle.Series +"'" +
				"AND Engine.Power = "+ vehicle.Power +
				"AND Engine.Volume = " +vehicle.Volume ;


			switch (vehicle.Sort)
			{
				case "Cena rastuce": {
						query += " Order by [Product].price ASC ,Product.Id OFFSET "+ vehicle.PPP * vehicle.CurentPage +" ROWS FETCH NEXT "+vehicle.PPP+" ROWS ONLY ";  //napravi drugi upit
						break;	
				};
				case "Cena opadajuce":
					{
						query += " Order by [Product].price DESC ,Product.Id OFFSET "+ vehicle.PPP * vehicle.CurentPage + " ROWS FETCH NEXT "+vehicle.PPP + " ROWS ONLY ";  //napravi drugi upit
						break;
					};
				case "Naziv opadajuce":
					{
						query += " Order by [Product].name DESC ,Product.Id OFFSET "+ vehicle.PPP * vehicle.CurentPage + " ROWS FETCH NEXT "+vehicle.PPP +" ROWS ONLY "; //napravi drugi upit
						break;
					};
				case "Naziv rastuce":
					{
						query += " Order by [Product].name ASC , Product.Id OFFSET " + vehicle.PPP * vehicle.CurentPage + " ROWS FETCH NEXT " + vehicle.PPP + " ROWS ONLY "; //napravi drugi upit
						break;
					};
				default: {
						query += " Order by Product.Id OFFSET " + vehicle.PPP * vehicle.CurentPage +" ROWS FETCH NEXT "+vehicle.PPP +" ROWS ONLY ";
						break;
					}
			}

			List<Product> listProduct = new List<Product>();

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Product product = new Product();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Name = Convert.ToString(rdr["Name"]);
					product.Picture = Convert.ToString(rdr["Picture"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.Price = Convert.ToDecimal(rdr["Price"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;
		}


		[Route("GetSingleProduct/{product}")]
		[HttpGet]
		public ActionResult<Product> GetSingleProduct(int product)
		{
			Product newProduct = new Product();

			string query = "select * from [Product] join ProductBelongGroup ON ProductBelongGroup.product_id = Product.Id where Product.Id =" + product; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();

				newProduct.Id = Convert.ToInt32(rdr["Id"]);
				newProduct.Name = Convert.ToString(rdr["Name"]);
				newProduct.Picture = Convert.ToString(rdr["Picture"]);
				newProduct.Quantity = Convert.ToInt32(rdr["Quantity"]);
				newProduct.Price = Convert.ToDecimal(rdr["Price"]);
				newProduct.group_id = Convert.ToInt32(rdr["group_id"]);

				
				
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return newProduct;
		}



		[Route("InputProduct")]
		[HttpPost]
		public ActionResult<int> InputProduct([FromBody] Product product)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Product]  OUTPUT INSERTED.Id values (@Name, @Picture ,@Quantity, @Price )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", product.Name);
					cmd.Parameters.AddWithValue("@Picture", product.Picture);
					cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
					cmd.Parameters.AddWithValue("@Price", product.Price);

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

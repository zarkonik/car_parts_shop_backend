using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductInformationDataController :ControllerBase
	{

		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetProductInformationData")]
		[HttpGet]
		public ActionResult<List<ProductInformationData>> GetProductInformationData()
		{
			List<ProductInformationData> listProductInformationData = new List<ProductInformationData>();

			string query = "select * from [ProductInformationData]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					ProductInformationData productInformationData = new ProductInformationData();
					productInformationData.Id = Convert.ToInt32(rdr["Id"]);
					productInformationData.Data = Convert.ToString(rdr["Data"]);
					productInformationData.productInformation_id = Convert.ToInt32(rdr["productInformation_id"]);
					productInformationData.product_id = Convert.ToInt32(rdr["product_id"]);

					listProductInformationData.Add(productInformationData);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProductInformationData;
		}





		[Route("InputProductInformationData")]
		[HttpPost]
		public ActionResult<int> InputProductInformationData([FromBody] ProductInformationData productInformationData)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [ProductInformationData]  OUTPUT INSERTED.Id values (@Data, @productInformation_id,@product_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@data", productInformationData.Data);
					cmd.Parameters.AddWithValue("@productInformation_id", productInformationData.productInformation_id);
					cmd.Parameters.AddWithValue("@product_id", productInformationData.product_id);

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

using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GroupInformationDataController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputGroupInformationData")]
		[HttpPost]
		public ActionResult<int> InputGroupInformationData([FromBody] GroupInformationData groupInformationData)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [GroupInformationData]  OUTPUT INSERTED.Id values (@Data, @groupInformation_id,@group_id,@product_id)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@data", groupInformationData.Data);
					cmd.Parameters.AddWithValue("@groupInformation_id", groupInformationData.groupInformation_id);
					cmd.Parameters.AddWithValue("@group_id", groupInformationData.group_id);
					cmd.Parameters.AddWithValue("@product_id", groupInformationData.product_id);

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

		[Route("GetGroupInformationData/{group}/{product}")]
		[HttpGet]
		public ActionResult<List<FullProductInformation>> GetGroupInformationData(int group,int product)
		{
			List<FullProductInformation> listProductInformation = new List<FullProductInformation>();

			string query = "select * from [GroupInformation] JOIN [GroupInformationData] ON GroupInformationData.groupInformation_id = GroupInformation.Id where GroupInformationData.group_id  =" + group +"AND GroupInformationData.product_id =" + product; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					FullProductInformation productInformationData = new FullProductInformation();
					productInformationData.Id = Convert.ToInt32(rdr["Id"]);
					productInformationData.Data = Convert.ToString(rdr["Data"]);
					productInformationData.Name = Convert.ToString(rdr["Name"]);

					listProductInformation.Add(productInformationData);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProductInformation;
		}

		[Route("GetGroupInformationDataCount/{group}/{groupInformation}")]
		[HttpGet]
		public ActionResult<List<GroupInformationCount>> GetGroupInformationDataCount(int group, int groupInformation)
		{
			List<GroupInformationCount> GroupInformationCounts = new List<GroupInformationCount>();

			string query = "select  GroupInformationData.[Data] , COUNT(*)  AS Count from[GroupInformationData]"+
							" JOIN[GroupInformation]"+
							" ON GroupInformation.Id = GroupInformationData.groupInformation_id"+
							" where GroupInformationData.group_id =" +group+ "AND GroupInformationData.groupInformation_id = "+groupInformation+
							" GROUP BY GroupInformationData.[Data]"; //napravi drugi upit
			
			SqlConnection connection = new SqlConnection(constr);
			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					GroupInformationCount GroupInformationCount = new GroupInformationCount();
					GroupInformationCount.Data = Convert.ToString(rdr["Data"]);
					GroupInformationCount.Count  = Convert.ToInt32 (rdr["Count"]);

					GroupInformationCounts.Add(GroupInformationCount);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return GroupInformationCounts;
		}




	}
}

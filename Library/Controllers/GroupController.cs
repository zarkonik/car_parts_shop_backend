using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GroupController: ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetGroups")]
		[HttpGet]
		public ActionResult<List<Group>> GetGroups()
		{
			List<Group> listGroup= new List<Group>();

			string query = "select * from [Group] FULL JOIN GroupBelongType ON GroupBelongType.group_Id = [Group].Id LEFT JOIN [Type] ON Type.Id = GroupBelongType.type_Id"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Group group = new Group();
					group.Id = Convert.ToInt32(rdr["Id"]);
					group.Name = Convert.ToString(rdr["Name"]);
					group.Picture = Convert.ToString(rdr["Picture"]);
					group.Details = Convert.ToString(rdr["Details"]);
					group.HasVehicle = Convert.ToInt32 (rdr["Has_Vehicle"]);
					group.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_Id"]))
					{
						group.Type.Id = 0;
						group.Type.Name = "";
					}
					else
					{
						group.Type.Id = Convert.ToInt32(rdr["type_Id"]);
						group.Type.Name = Convert.ToString(rdr[9]);
					}
					listGroup.Add(group);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listGroup;
		}

		[Route("GetGroupsFromSubCategory/{subCategory}")]
		[HttpGet]
		public ActionResult<List<Group>> GetGroupsFromSubCategory(int subCategory)
		{
			List<Group> listGroup = new List<Group>();

			string query = "select * from [Group] g JOIN GroupBelongSubCategory ON GroupBelongSubCategory.group_id= g.id where GroupBelongSubCategory.subCategory_id = "+subCategory ; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Group group = new Group();
					group.Id = Convert.ToInt32(rdr["Id"]);
					group.Name = Convert.ToString(rdr["Name"]);
					group.Details = Convert.ToString(rdr["Details"]);
					group.HasVehicle = Convert.ToInt32(rdr["Has_Vehicle"]);

					listGroup.Add(group);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listGroup;
		}


		[Route("GetGroupsWithPictureFromSubCategory/{subCategory}")]
		[HttpGet]
		public ActionResult<List<Group>> GetGroupsWithPictureFromSubCategory(int subCategory)
		{
			List<Group> listGroup = new List<Group>();

			string query = "select * from [Group] g JOIN GroupBelongSubCategory ON GroupBelongSubCategory.group_id= g.id where GroupBelongSubCategory.subCategory_id = " + subCategory; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Group group = new Group();
					group.Id = Convert.ToInt32(rdr["Id"]);
					group.Name = Convert.ToString(rdr["Name"]);
					group.Picture = Convert.ToString(rdr["Picture"]);
					group.Details = Convert.ToString(rdr["Details"]);
					group.HasVehicle = Convert.ToInt32(rdr["Has_Vehicle"]);

					listGroup.Add(group);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listGroup;
		}




		[Route("GetGroup/{group}")]
		[HttpGet]
		public ActionResult<Group> GetGroup(int group)
		{
			Group groupNew = new Group();

			string query = "select * from [Group] AS g where g.Id =" + group; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

					rdr.Read();
				
					
					groupNew.Id = Convert.ToInt32(rdr["Id"]);
					groupNew.Name = Convert.ToString(rdr["Name"]);
					groupNew.Picture = Convert.ToString(rdr["Picture"]);
					groupNew.Details = Convert.ToString(rdr["Details"]);
					groupNew.HasVehicle = Convert.ToInt32 (rdr["Has_Vehicle"]);

				
				
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return groupNew;
		}

		[Route("GetGroupName/{name}")]
		[HttpGet]
		public ActionResult<Group> GetGroupName(string name)
		{
			Group groupNew = new Group();

			string query = "select * from [Group] AS g where g.name = '" + name +"'"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();


				groupNew.Id = Convert.ToInt32(rdr["Id"]);
				groupNew.Name = Convert.ToString(rdr["Name"]);
				groupNew.Picture = Convert.ToString(rdr["Picture"]);
				groupNew.Details = Convert.ToString(rdr["Details"]);
				groupNew.HasVehicle = Convert.ToInt32(rdr["Has_Vehicle"]);



				//command.ExecuteNonQuery();
				connection.Close();
			}

			return groupNew;
		}


		[Route("SearchGroup/{name}")]
		[HttpGet]
		public ActionResult<List<SearchParametar>> SearchGroup(string name)
		{
			List<SearchParametar> groups = new List<SearchParametar>();

			string query = " select Id , [Name] from[Group] AS g where g.Name LIKE '%" + name + "%'";

			SqlConnection connection = new SqlConnection(constr);



			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SearchParametar group = new SearchParametar();
					group.Id = Convert.ToInt32(rdr["Id"]);
					group.Name = Convert.ToString(rdr["Name"]);
					groups.Add(group);
				}

				//command.ExecuteNonQuery();
				connection.Close();
			}

			return groups;
		}



		[Route("InputGroup")]
		[HttpPost]
		public ActionResult<int> InputGroup([FromBody] Group group)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Group]  OUTPUT INSERTED.Id  values (@Name, @Picture ,@Details, @Has_Vehicle)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", group.Name);
					cmd.Parameters.AddWithValue("@Picture", group.Picture);
					cmd.Parameters.AddWithValue("@Details", group.Details);
					cmd.Parameters.AddWithValue("@Has_Vehicle", group.HasVehicle);


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
	}
}

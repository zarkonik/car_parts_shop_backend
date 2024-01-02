using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SubCategoryController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetSubCategories")]
		[HttpGet]
		public ActionResult<List<SubCategory>> GetSubCategories()
		{
			List<SubCategory> listSubCategory = new List<SubCategory>();

			string query = "select * from [SubCategory] FULL JOIN SubCategoryBelongType ON SubCategoryBelongType.subCategory_Id = SubCategory.Id LEFT JOIN [Type] ON Type.Id = SubCategoryBelongType.type_Id"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SubCategory subCategory = new SubCategory();
					subCategory.Id = Convert.ToInt32(rdr["Id"]);
					subCategory.Name = Convert.ToString(rdr["Name"]);
					subCategory.Picture = Convert.ToString(rdr["Picture"]);
					subCategory.Details = Convert.ToString(rdr["Details"]);
					subCategory.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_Id"]))
					{
						subCategory.Type.Id = 0;
						subCategory.Type.Name = "";
					}
					else
					{
						subCategory.Type.Id = Convert.ToInt32(rdr["type_Id"]);
						subCategory.Type.Name = Convert.ToString(rdr[8]);
					}


					listSubCategory.Add(subCategory);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listSubCategory;
		}

		[Route("GetSubCategoriesNotInCategory")]
		[HttpGet]
		public ActionResult<List<SubCategory>> GetSubCategoriesNotInCategory()
		{
			List<SubCategory> listSubCategory = new List<SubCategory>();

			string query = "select * from [SubCategory] LEFT JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id where SubCategoryBelongCategory.category_id IS NULL"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SubCategory subCategory = new SubCategory();
					subCategory.Id = Convert.ToInt32(rdr["Id"]);
					subCategory.Name = Convert.ToString(rdr["Name"]);
					subCategory.Picture = Convert.ToString(rdr["Picture"]);
					subCategory.Details = Convert.ToString(rdr["Details"]);
					

					listSubCategory.Add(subCategory);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listSubCategory;
		}

		[Route("GetSubCategoriesNotInCategory/{type}")]
		[HttpGet]
		public ActionResult<List<SubCategory>> GetSubCategoriesNotInCategory(string type)
		{
			List<SubCategory> listSubCategory = new List<SubCategory>();

			string query = "";

			if (type == "Najtrazeniji")
			{
				query = "select * from [SubCategory] LEFT JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id FULL JOIN SubCategoryBelongType ON SubCategory.Id = SubCategoryBelongType.subCategory_id LEFT JOIN [Type] ON SubCategoryBelongType.type_id = [Type].Id where SubCategoryBelongCategory.category_id IS NULL AND [Type].Name = 'Najtrazeniji' UNION select * from [SubCategory] LEFT JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id FULL JOIN SubCategoryBelongType ON SubCategory.Id = SubCategoryBelongType.subCategory_id LEFT JOIN [Type] ON SubCategoryBelongType.type_id = [Type].Id where SubCategoryBelongCategory.category_id IS NULL AND SubCategory.Id IN (SELECT b.subCategory_id  from (SELECT a.group_id ,  SUM(a.number) AS total_number , GroupBelongSubCategory.subCategory_id FROM ( select  [MostPopularProduct].product_id , COUNT(*) AS number , [ProductBelongGroup].group_id from [MostPopularProduct] JOIN ProductBelongGroup ON ProductBelongGroup.product_id = [MostPopularProduct].product_id group by [MostPopularProduct].product_id , [ProductBelongGroup].group_id) AS a JOIN GroupBelongSubCategory ON GroupBelongSubCategory.group_id = a.group_id group by  GroupBelongSubCategory.subCategory_id , a.group_id ORDER by total_number DESC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY) AS b ) ";

			}
			else
			{
				if (type != "Ostali")
				{
					query = "select * from [SubCategory] LEFT JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id FULL JOIN SubCategoryBelongType ON SubCategory.Id = SubCategoryBelongType.subCategory_id LEFT JOIN [Type] ON SubCategoryBelongType.type_id = [Type].Id where SubCategoryBelongCategory.category_id IS NULL AND [Type].Name = '" + type + "'"; //napravi drugi upit
				}
				else
				{
					query = "select * from [SubCategory] LEFT JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id FULL JOIN SubCategoryBelongType ON SubCategory.Id = SubCategoryBelongType.subCategory_id LEFT JOIN [Type] ON SubCategoryBelongType.type_id = [Type].Id WHERE SubCategoryBelongCategory.category_id IS NULL AND [Type].Name IS NULL";
				}
			}
			

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SubCategory subCategory = new SubCategory();
					subCategory.Id = Convert.ToInt32(rdr["Id"]);
					subCategory.Name = Convert.ToString(rdr["Name"]);
					subCategory.Picture = Convert.ToString(rdr["Picture"]);
					subCategory.Details = Convert.ToString(rdr["Details"]);
					subCategory.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_id"]))
					{
						subCategory.Type.Id = 0;
						subCategory.Type.Name = "";
					}
					else
					{
						subCategory.Type.Id = Convert.ToInt32(rdr["type_id"]);
						subCategory.Type.Name = Convert.ToString(rdr[11]);
					}
					listSubCategory.Add(subCategory);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listSubCategory;
		}




		[Route("GetSubCategoriesFromCategory/{category}")]
		[HttpGet]
		public ActionResult<List<SubCategory>> GetSubCategoriesFromCategory(int category)
		{
			List<SubCategory> listSubCategory = new List<SubCategory>();

			string query = "select * from [SubCategory] JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id = SubCategory.Id where SubCategoryBelongCategory.category_id = " + category; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SubCategory subCategory = new SubCategory();
					subCategory.Id = Convert.ToInt32(rdr["Id"]);
					subCategory.Name = Convert.ToString(rdr["Name"]);
					subCategory.Picture = Convert.ToString(rdr["Picture"]);
					subCategory.Details = Convert.ToString(rdr["Details"]);
					
					listSubCategory.Add(subCategory);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listSubCategory;
		}

		[Route("SearchSubCategory/{name}")]
		[HttpGet]
		public ActionResult<List<SearchParametar>> SearchSubCategory(string name)
		{
			List<SearchParametar> subCategories = new List<SearchParametar>();

			string query = "select Id , [Name] from[SubCategory] AS sc where sc.Name LIKE '%" + name + "%'";

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SearchParametar subCategory = new SearchParametar();

					subCategory.Id = Convert.ToInt32(rdr["Id"]);
					subCategory.Name = Convert.ToString(rdr["Name"]);
					subCategories.Add(subCategory);
				}

				//command.ExecuteNonQuery();
				connection.Close();
			}

			return subCategories;
		}

		[Route("GetSubCategory/{subCategory}")]
		[HttpGet]
		public ActionResult<SubCategory> GetSubCategory(int subCategory)
		{
			SubCategory subCategoryNew = new SubCategory();

			string query = "select * from [SubCategory] where SubCategory.id ="+subCategory; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();
				
					
					subCategoryNew.Id = Convert.ToInt32(rdr["Id"]);
					subCategoryNew.Name = Convert.ToString(rdr["Name"]);
					subCategoryNew.Picture = Convert.ToString(rdr["Picture"]);
					subCategoryNew.Details = Convert.ToString(rdr["Details"]);
				


				//command.ExecuteNonQuery();
				connection.Close();
			}

			return subCategoryNew;
		}

		[Route("GetSubCategoryName/{name}")]
		[HttpGet]
		public ActionResult<SubCategory> GetSubCategoryName(string name)
		{
			SubCategory subCategoryNew = new SubCategory();

			string query = "select * from [SubCategory] AS g where g.name = '" + name + "'"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();


				subCategoryNew.Id = Convert.ToInt32(rdr["Id"]);
				subCategoryNew.Name = Convert.ToString(rdr["Name"]);
				subCategoryNew.Picture = Convert.ToString(rdr["Picture"]);
				subCategoryNew.Details = Convert.ToString(rdr["Details"]);


				//command.ExecuteNonQuery();
				connection.Close();
			}

			return subCategoryNew;
		}


		[Route("InputSubCategory")]
		[HttpPost]
		public ActionResult<int> InputCategory([FromBody] SubCategory subCategory)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [SubCategory]  OUTPUT INSERTED.Id values (@Name, @Picture ,@Details )";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", subCategory.Name);
					cmd.Parameters.AddWithValue("@Picture", subCategory.Picture);
					cmd.Parameters.AddWithValue("@Details", subCategory.Details);

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

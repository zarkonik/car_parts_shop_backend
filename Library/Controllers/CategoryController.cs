using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("GetCategories")]
		[HttpGet]
		public ActionResult<List<Category>> GetCategories()
		{
			List<Category> listCategory = new List<Category>();

			string query = "select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Category category = new Category();
					category.Id = Convert.ToInt32(rdr["Id"]);
					category.Name = Convert.ToString(rdr["Name"]);
					category.Picture = Convert.ToString(rdr["Picture"]);
					category.Details = Convert.ToString(rdr["Details"]);
					category.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_Id"]))
					{
						category.Type.Id = 0;
						category.Type.Name = "";
					}
					else
					{
						category.Type.Id = Convert.ToInt32(rdr["type_Id"]);
						category.Type.Name = Convert.ToString(rdr[8]);
					}
					listCategory.Add(category);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listCategory;
		}


		[Route("GetCategories/{type}")]
		[HttpGet]
		public ActionResult<List<Category>> GetCategories(string type)
		{
			List<Category> listCategory = new List<Category>();

			string query = "";

			if (type == "Najtrazeniji")
			{
				query = "select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id where [Type].Name = 'Najtrazeniji' UNION select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id WHERE Category.Id  IN (SELECT c.category_id FROM (SELECT b.subCategory_id , SUM(b.total_number) AS category_number, SubCategoryBelongCategory.category_id  from (SELECT a.group_id ,  SUM(a.number) AS total_number , GroupBelongSubCategory.subCategory_id FROM ( select  [MostPopularProduct].product_id , COUNT(*) AS number , [ProductBelongGroup].group_id from [MostPopularProduct] JOIN ProductBelongGroup ON ProductBelongGroup.product_id = [MostPopularProduct].product_id group by [MostPopularProduct].product_id , [ProductBelongGroup].group_id) AS a JOIN GroupBelongSubCategory ON GroupBelongSubCategory.group_id = a.group_id group by  GroupBelongSubCategory.subCategory_id , a.group_id ) AS b JOIN SubCategoryBelongCategory ON SubCategoryBelongCategory.subCategory_id= b.subCategory_id GROUP BY b.subCategory_id,SubCategoryBelongCategory.category_id) AS c ORDER by category_number DESC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY )";
			}
			else
			{
				if (type != "Ostali")
				{
					query = "select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id WHERE [Type].Name = '" + type + "'"; //napravi drugi upit
				}
				else
				{
					query = "select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id WHERE [Type].Name  IS NULL";
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
					Category category = new Category();
					category.Id = Convert.ToInt32(rdr["Id"]);
					category.Name = Convert.ToString(rdr["Name"]);
					category.Picture = Convert.ToString(rdr["Picture"]);
					category.Details = Convert.ToString(rdr["Details"]);
					category.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_Id"]))
					{
						category.Type.Id = 0;
						category.Type.Name = "";
					}
					else
					{
						category.Type.Id = Convert.ToInt32(rdr["type_Id"]);
						category.Type.Name = Convert.ToString(rdr[8]);
					}
					listCategory.Add(category);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listCategory;
		}



		[Route("GetPopularCategories")]
		[HttpGet]
		public ActionResult<List<Category>> GetPopularCategories()
		{
			List<Category> listCategory = new List<Category>();

			string query = "select * from [Category] FULL JOIN CategoryBelongType ON CategoryBelongType.category_Id = Category.Id LEFT JOIN [Type] ON Type.Id = CategoryBelongType.type_Id"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Category category = new Category();
					category.Id = Convert.ToInt32(rdr["Id"]);
					category.Name = Convert.ToString(rdr["Name"]);
					category.Picture = Convert.ToString(rdr["Picture"]);
					category.Details = Convert.ToString(rdr["Details"]);
					category.Type = new TypeVehicle();
					if (Convert.IsDBNull(rdr["type_Id"]))
					{
						category.Type.Id = 0;
						category.Type.Name = "";
					}
					else
					{
						category.Type.Id = Convert.ToInt32(rdr["type_Id"]);
						category.Type.Name = Convert.ToString(rdr[8]);
					}
					listCategory.Add(category);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listCategory;
		}

		[Route("GetCategory/{category}")]
		[HttpGet]
		public ActionResult<Category> GetCategory(int category)
		{
			Category categoryNew = new Category();

			string query = "select * from [Category] where Category.id =" + category; 
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();


				categoryNew.Id = Convert.ToInt32(rdr["Id"]);
				categoryNew.Name = Convert.ToString(rdr["Name"]);
				categoryNew.Picture = Convert.ToString(rdr["Picture"]);
				categoryNew.Details = Convert.ToString(rdr["Details"]);



				//command.ExecuteNonQuery();
				connection.Close();
			}

			return categoryNew;
		}

		[Route("InputCategory")]
		[HttpPost]
		public ActionResult<int> InputCategory([FromBody] Category category)
		{

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [Category]  OUTPUT INSERTED.Id values (@Name, @Picture ,@Details ) ";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Name", category.Name);
					cmd.Parameters.AddWithValue("@Picture", category.Picture);
					cmd.Parameters.AddWithValue("@Details", category.Details);

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

		[Route("SearchCategory/{name}")]
		[HttpGet]
		public ActionResult<List<SearchParametar>> SearchCategory(string name)
		{
			List<SearchParametar> categories = new List<SearchParametar>();

			string query = " select Id , [Name] from [Category] AS c where c.Name LIKE '%" + name + "%'";

			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					SearchParametar category = new SearchParametar();
					category.Id = Convert.ToInt32(rdr["Id"]);
					category.Name = Convert.ToString(rdr["Name"]);
					categories.Add(category);
					//command.ExecuteNonQuery();
				}

				connection.Close();
			}

			return categories;
		}


		[Route("GetCategoryName/{name}")]
		[HttpGet]
		public ActionResult<Category> GetCategoryName(string name)
		{
			Category CategoryNew = new Category();

			string query = "select * from [Category] AS g where g.name = '" + name + "'"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();


				CategoryNew.Id = Convert.ToInt32(rdr["Id"]);
				CategoryNew.Name = Convert.ToString(rdr["Name"]);
				CategoryNew.Picture = Convert.ToString(rdr["Picture"]);
				CategoryNew.Details = Convert.ToString(rdr["Details"]);



				//command.ExecuteNonQuery();
				connection.Close();
			}

			return CategoryNew;
		}


	}
}

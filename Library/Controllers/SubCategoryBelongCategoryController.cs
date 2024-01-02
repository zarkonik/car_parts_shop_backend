using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SubCategoryBelongCategoryController :ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputSubCategory/{subCategory}")]
		[HttpPost]
		public ActionResult InputSubCategory(int subCategory,[FromBody] List<int> category)
		{
			if (category.Count > 0)
			{
				using (SqlConnection con = new SqlConnection(constr))
				{
					string query = "insert into [SubCategoryBelongCategory] OUTPUT INSERTED.Id values (@subCategory_id, @category_id )";
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
						foreach (int id in category)
						{
							cmd.Connection = con;
							cmd.Parameters.AddWithValue("@subCategory_id", subCategory);
							cmd.Parameters.AddWithValue("@category_id", id);

							con.Open();
							int i = (int)cmd.ExecuteScalar();
							if (i < 0)
							{
								return BadRequest(id);
							}
							con.Close();
						}
					}
				}
			}
			return Ok();
		}

		[Route("GetSubCategoryFromCategory/{category}")]
		[HttpGet]
		public ActionResult<List<SubCategory>> GetSubCategoryFromCategory(int category)
		{
			List<SubCategory> listSubCategory = new List<SubCategory>();

			string query = "select * from [SubCategory] INNER JOIN [SubCategoryBelongCategory] where SubCategoryBelongCategory.id_category =" + category; //napravi drugi upit
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


	}
}

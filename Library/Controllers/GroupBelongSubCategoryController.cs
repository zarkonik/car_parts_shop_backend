using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GroupBelongSubCategoryController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputSubCategory/{group}")]
		[HttpPost]
		public ActionResult InputSubCategory(int group, [FromBody] List<int> subCategory)
		{
			if (subCategory.Count > 0)
			{
				using (SqlConnection con = new SqlConnection(constr))
				{
					string query = "insert into [GroupBelongSubCategory]  OUTPUT INSERTED.Id values (@group_id, @subCategory_id )";
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
						foreach (int id in subCategory)
						{
							cmd.Connection = con;
							cmd.Parameters.AddWithValue("@group_id", group);
							cmd.Parameters.AddWithValue("@subCategory_id", id);

							con.Open();
							int i = cmd.ExecuteNonQuery();
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


	}
}

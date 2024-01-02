using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("Register")]
		[HttpPost]
		public ActionResult<int> Register([FromBody] User user)
		{
			string code = "abcd";

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "insert into [User] OUTPUT INSERTED.Id values (@Firstname, @Lastname, @Email, @Password, @Active, @Code ,@Admin)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@Firstname", user.Firstname);
					cmd.Parameters.AddWithValue("@Lastname", user.Lastname);
					cmd.Parameters.AddWithValue("@Email", user.Email);
					cmd.Parameters.AddWithValue("@Password", user.Password);
					cmd.Parameters.AddWithValue("@Active", 0);
					cmd.Parameters.AddWithValue("@Code", code);
					cmd.Parameters.AddWithValue("@Admin", 0);

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

		[Route("GetUser/{id_user}" )]
		[HttpGet]
		public ActionResult<User> GetUser(int id_user)
		{
			string query = "select * from [User] where id = " + id_user;
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;

			User userBack = new User();

			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();

				if(rdr == null)
				{
					return BadRequest();
				}

				userBack.Id = Convert.ToInt32(rdr["Id"]);
				userBack.Firstname = Convert.ToString(rdr["Firstname"]);
				userBack.Lastname = Convert.ToString(rdr["Lastname"]);
				userBack.Email = Convert.ToString(rdr["Email"]);
				userBack.Admin = Convert.ToInt32(rdr["Admin"]);
	
				connection.Close();
			}
			return Ok(userBack);
		}

		[Route("GetUsers") ]
		[HttpGet]
		public ActionResult<List<User>>GetUsers()
		{
			List<User> listUsers = new List<User>();

			string query = "select * from [User]"; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection );
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					User user = new User();
					user.Id = Convert.ToInt32(rdr["Id"]);
					user.Firstname  = Convert.ToString(rdr["Firstname"]);
					user.Lastname = Convert.ToString(rdr["Lastname"]);
					user.Email = Convert.ToString(rdr["Email"]);
					user.Admin = Convert.ToInt32(rdr["Admin"]);

					listUsers.Add(user);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listUsers;
		}

		[Route("Login")]
		[HttpPost]
		public ActionResult<User> Login([FromBody] User user)
		{
			
			string query = "select * from [User] where email = " + "'" + user.Email + "'";
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;

			User userBack = new User();

			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

					rdr.Read();
				
					if(rdr == null)
					{
						return BadRequest(404);
					}
				
					if(Convert.ToString(rdr["Password"]) != user.Password )
					{
						return BadRequest(403);
					}

					
					userBack.Id = Convert.ToInt32(rdr["Id"]);
					userBack.Firstname = Convert.ToString(rdr["Firstname"]);
					userBack.Lastname = Convert.ToString(rdr["Lastname"]);
					userBack.Email = Convert.ToString(rdr["Email"]);
					userBack.Admin = Convert.ToInt32(rdr["Admin"]);


				//command.ExecuteNonQuery();
				connection.Close();
			}

			return Ok(userBack);
		}

		[Route("UpdateUser")]
		[HttpPut]
		public ActionResult UpdateUser([FromBody] User user)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "UPDATE [User] SET Firstname = @Firstname, Lastname = @Lastname Where Id = @Id ";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@Firstname", user.Firstname);
			cmd.Parameters.AddWithValue("@Lastname", user.Lastname);
			cmd.Parameters.AddWithValue("@Id", user.Id);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();

			return Ok();
		}

		[Route("UpdateUserPassword")]
		[HttpPut]
		public ActionResult UpdateUserPassword([FromBody] User user)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "UPDATE [User] SET Password = @Password, Where Id = @Id ";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@Password", user.Password );
			cmd.Parameters.AddWithValue("@Id", user.Id);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();

			return Ok();
		}

		[Route("UpdateUserEmail")]
		[HttpPut]
		public ActionResult UpdateUserEmail([FromBody]User user)
		{
			string code = "abcd";

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "UPDATE [User] SET Email = @Email Where Id = @Id "+ " Password = " + "'" + user.Password + "'";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					
					cmd.Parameters.AddWithValue("@Email", user.Email);
					cmd.Parameters.AddWithValue("@Active", 0);
					cmd.Parameters.AddWithValue("@Code", code);
					cmd.Parameters.AddWithValue("@Id", user.Id);


					con.Open();
					int i = cmd.ExecuteNonQuery();
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();

		}

		[Route("UpdateAdminEmail")]
		[HttpPut]
		public ActionResult UpdateAdminEmail([FromBody] User user)
		{
			string code = "abcd";

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query = "UPDATE [User] SET Email = @Email Where Id = @Id ";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{

					cmd.Parameters.AddWithValue("@Email", user.Email);
					cmd.Parameters.AddWithValue("@Active", 0);
					cmd.Parameters.AddWithValue("@Code", code);
					cmd.Parameters.AddWithValue("@Id", user.Id);


					con.Open();
					int i = cmd.ExecuteNonQuery();
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();

		}


		[Route("CheckCode/{code}")]
		[HttpPost]
		public ActionResult CheckCode(string code,[FromBody]User user)
		{

			string query = "select * from [User] where id = " + user.Id;
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;

			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				rdr.Read();

				if (rdr == null)
				{
					return BadRequest(404);
				}

				if (Convert.ToString(rdr["Code"]) != code )
				{
					return BadRequest(403);
				}

				//command.ExecuteNonQuery();
				connection.Close();
			}

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query2 = "UPDATE [User] SET Email = @Email, Where Id = @Id ";
				using (SqlCommand cmd = new SqlCommand(query2, con))
				{
					cmd.Parameters.AddWithValue("@Active", 1);

					con.Open();
				
					int i = cmd.ExecuteNonQuery();
					if (i > 0)
					{
						return Ok(i);
					}
					con.Close();
				}
			}
			return BadRequest();
		}

		[Route("DeleteUser/{id_user}")]
		[HttpDelete]
		public ActionResult DeleteUser(int id_user)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "DELETE FROM [User] where Id='" + id_user + "'";
			SqlCommand cmd = new SqlCommand(query, connection);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
			return Ok();
		}

	}
}
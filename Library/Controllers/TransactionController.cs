using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Library.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TransactionController : ControllerBase
	{
		//private string constr = "Data Source=COMP\\SQL2023;Initial Catalog=Library;Integrated Security=True";

		private string constr = "Data Source=SQL6031.site4now.net;Initial Catalog=db_a9ef49_library;User Id=db_a9ef49_library_admin;Password=prvi1234";


		[Route("InputProductsInTransaction/{user_id}/{number_of_vehicle}/{name}/{lastname}/{address}/{phone}")]
		[HttpPost]
		public ActionResult<int> InputProductsInTransaction(int user_id, int number_of_vehicle,string name,string lastname, string address,string phone )
		{

			List<CartProduct> cart_products = new List<CartProduct>();

			string query = "select * from [Product] JOIN Cart ON Cart.product_id = Product.Id where Cart.user_id  =" + user_id; 
			SqlConnection connection = new SqlConnection(constr);


			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{
				while (rdr.Read())
				{
					CartProduct product = new CartProduct();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.Quantity = Convert.ToInt32(rdr["Quantity"]);
					product.selectedCount = Convert.ToInt32(rdr["selectedCount"]);

					if(product.Quantity < product.selectedCount )
					{
						return NotFound(product.Id);
					}

					cart_products.Add(product);
				}
			
				connection.Close();
			}

			int id_transaction;

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query3 = "insert into [Transaction] OUTPUT INSERTED.Id values (@user_id, @status, @number_of_vehicle,@Time,@name,@lastname,@address,@phone)";
				using (SqlCommand cmd = new SqlCommand(query3, con))
				{
					cmd.Connection = con;
					cmd.Parameters.AddWithValue("@user_id", user_id);
					cmd.Parameters.AddWithValue("@status", false );
					cmd.Parameters.AddWithValue("@number_of_vehicle", number_of_vehicle);
					cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString());
					cmd.Parameters.AddWithValue("@name", name);
					cmd.Parameters.AddWithValue("@lastname", lastname);
					cmd.Parameters.AddWithValue("@address", address);
					cmd.Parameters.AddWithValue("@phone",phone);

					con.Open();
					 id_transaction = (int)cmd.ExecuteScalar();
					
					con.Close();
				}
			}

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query2 = "UPDATE [Product] SET Quantity = @Quantity Where  Id = @Id ";

				foreach(CartProduct cart_product in cart_products )
				{
					using (SqlCommand cmd = new SqlCommand(query2, con))
					{

						cmd.Parameters.AddWithValue("@Id", cart_product.Id );
						cmd.Parameters.AddWithValue("@Quantity", cart_product.Quantity - cart_product.selectedCount );
			
						con.Open();
						int i = cmd.ExecuteNonQuery();
		
						con.Close();
					}
				}

			}

			using (SqlConnection con = new SqlConnection(constr))
			{
				string query3 = "insert into [ProductBelongTransaction] OUTPUT INSERTED.Id values (@product_id, @transaction_id, @count)";

				foreach (CartProduct cart_product in cart_products)
				{
					using (SqlCommand cmd = new SqlCommand(query3, con))
					{
						cmd.Connection = con;
						cmd.Parameters.AddWithValue("@product_id", cart_product.Id);
						cmd.Parameters.AddWithValue("@transaction_id", id_transaction);
						cmd.Parameters.AddWithValue("@count", cart_product.selectedCount);

						con.Open();
						int i = (int)cmd.ExecuteScalar();
						con.Close();
					}
				}
			}

			SqlConnection connect4 = new SqlConnection(constr);
			
			foreach (CartProduct cart_product in cart_products)
			{
				string query4 = "DELETE FROM [Cart] where product_id='" +cart_product.Id + "' AND user_id='" + user_id + "'";
				SqlCommand cmd = new SqlCommand(query4, connect4);
			
				connect4.Open();
				cmd.ExecuteNonQuery();
				connect4.Close();
			
			}

			return Ok(id_transaction);
		}


		[Route("GetProductsFromTransaction/{user}")]
		[HttpGet]
		public ActionResult<List<TransactionProduct>> GetProductsFromTransaction(int user)
		{
			List<TransactionProduct> listProduct = new List<TransactionProduct>();

			string query = "select * from [Transaction] JOIN ProductBelongTransaction ON ProductBelongTransaction.transaction_id = [Transaction].Id where [Transaction].user_id  =  " + user; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					TransactionProduct product = new TransactionProduct();
					product.Id = Convert.ToInt32(rdr["Id"]);
					product.ProductID = Convert.ToInt32(rdr["product_id"]);
					product.Count = Convert.ToInt32(rdr["count"]);

					listProduct.Add(product);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listProduct;
		}

		[Route("GetTransactions/{user}")]
		[HttpGet]
		public ActionResult<List<Transaction>> GetTransactions(int user)
		{
			List<Transaction> listTransaction = new List<Transaction>();

			string query = "select * from [Transaction] where [Transaction].user_id  =" + user; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Transaction transaction = new Transaction();
					transaction.Id = Convert.ToInt32(rdr["Id"]);
					transaction.UserId = Convert.ToString(rdr["user_id"]);
					transaction.Status = Convert.ToBoolean(rdr["status"]);
					transaction.NumberOfVehicle = Convert.ToString(rdr["number_of_vehicle"]);
					transaction.DataTime = Convert.ToString(rdr["Time"]);
					transaction.Name  = Convert.ToString(rdr["Name"]);
					transaction.Lastname = Convert.ToString(rdr["lastname"]);
					transaction.Address = Convert.ToString(rdr["address"]);
					transaction.Phone = Convert.ToString(rdr["phone"]);

					listTransaction.Add(transaction);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listTransaction;
		}

		[Route("GetTransactions")]
		[HttpGet]
		public ActionResult<List<Transaction>> GetTransactions()
		{
			List<Transaction> listTransaction = new List<Transaction>();

			string query = "select * from [Transaction] "; //napravi drugi upit
			SqlConnection connection = new SqlConnection(constr);

			SqlCommand command = new SqlCommand(query, connection);
			command.Connection = connection;
			connection.Open();

			using (SqlDataReader rdr = command.ExecuteReader())
			{

				while (rdr.Read())
				{
					Transaction transaction = new Transaction();
					transaction.Id = Convert.ToInt32(rdr["Id"]);
					transaction.UserId = Convert.ToString(rdr["user_id"]);
					transaction.Status = Convert.ToBoolean(rdr["status"]);
					transaction.NumberOfVehicle = Convert.ToString(rdr["number_of_vehicle"]);
					transaction.DataTime = Convert.ToString(rdr["Time"]);
					transaction.Name = Convert.ToString(rdr["Name"]);
					transaction.Lastname = Convert.ToString(rdr["lastname"]);
					transaction.Address = Convert.ToString(rdr["address"]);
					transaction.Phone = Convert.ToString(rdr["phone"]);

					listTransaction.Add(transaction);
				}
				//command.ExecuteNonQuery();
				connection.Close();
			}

			return listTransaction;
		}

		[Route("UpdateTransaction")]
		[HttpPut]
		public ActionResult UpdateTransaction([FromBody] Transaction transaction)
		{
			SqlConnection connection = new SqlConnection(constr);
			string query = "UPDATE [Transaction] SET Status = @Status Where Id = @Id ";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@Id", transaction.Id);
			cmd.Parameters.AddWithValue("@Status", transaction.Status);
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();

			return Ok();
		}


	}
}

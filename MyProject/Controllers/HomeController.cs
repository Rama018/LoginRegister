using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Task = MyProject.Models.Task;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        static string connectionString = "Data Source=DESKTOP-9HUR6QM\\SQLEXPRESS;Initial Catalog=MyProject;Integrated Security=True";

        private readonly ILogger<HomeController> _logger;
        public IConfiguration _configuration { get; }
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User e)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);


            string sql = "SELECT [hashpassword], [id] FROM [User] WHERE name=@name ";


            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            command.Parameters.AddWithValue("@name", e.Name);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {


                    // Retrieve values for each column
                    string hashpassword = reader.GetString(0);
                    int id = reader.GetInt32(1);

                    if (PasswordHelper.VerifyPassword(e.Password, hashpassword))
                    {
                        connection.Close();
                        return RedirectToAction("GetAll", "Task", new { id=id  });

                    }
                    else
                    {
                        TempData["alert"] = " wrong password";
                        connection.Close();
                        return RedirectToAction(nameof(Login));
                    }



                }
                else
                {
                    TempData["alert"] = " invalid username";
                    connection.Close();
                    return RedirectToAction(nameof(Login));
                }  
                
            }
          

        }

        [HttpGet]
        public IActionResult registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult registration(register reg)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                byte[] originalBytes = Encoding.UTF8.GetBytes(reg.HashPassword);
                string base64String = Convert.ToBase64String(originalBytes);
                string sqlname = "SELECT COUNT(*) FROM [User] WHERE name = @name";
                string sqlemail = "SELECT COUNT(*) FROM [User] WHERE email = @email";
                string sqlphone = "SELECT COUNT(*) FROM [User] WHERE phone = @phone";
                string salt = PasswordHelper.GenerateSalt();
                string sql2 = "insert into [User]  values('" + reg.Name + "','" + reg.Email + "','" + PasswordHelper.HashPassword(reg.HashPassword, salt) + "','" + reg.Phone + "')";

                SqlCommand command = new SqlCommand(sqlname, connection);
                SqlCommand command2 = new SqlCommand(sql2, connection);
                SqlCommand command3 = new SqlCommand(sqlemail, connection);
                SqlCommand command4 = new SqlCommand(sqlphone, connection);


                command.Parameters.AddWithValue("@name", reg.Name);
                command3.Parameters.AddWithValue("@email", reg.Email);
                command4.Parameters.AddWithValue("@phone", reg.Phone);

                int count = (int)command.ExecuteScalar();
                int count3 = (int)command3.ExecuteScalar();
                int count4 = (int)command4.ExecuteScalar();

                if (count > 0)
                {
                    TempData["alert"] = "Username already exists ";

                }

                if (count3 > 0)
                {
                    TempData["alert3"] = "Email already exists ";

                }
                if (count4 > 0)
                {
                    TempData["alert4"] = "Phone already exists ";

                }

                else
                {

                    command2.ExecuteNonQuery();
                    connection.Close();
                    return RedirectToAction("Login");

                }
                connection.Close();

                return View("registration");

            }
        }
      
        public IActionResult ViewTask()
        {
            return View();
        }
        public IActionResult Privacy()
        {


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
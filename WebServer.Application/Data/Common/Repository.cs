using SMS.Data.Models;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Sms.Data.Common
{
    public class Repository
    {
        private readonly SqlConnection _connection;
        private readonly string connectionString = "Server=DESKTOP-QCRS2FA\\SQLEXPRESS03;Database=RegistrationForm;Trusted_Connection=True;";

        public Repository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Add(User user)
        {
            if(CheckIfUserExists(user.Username))
            {
                throw new Exception("User with this username exists!");
            }
            string insertQuery = $"INSERT INTO Users(Id, Username, Email, Password, EmailConfirmed) VALUES('{user.Id}','{user.Username}','{user.Email}','{user.Password}',0)";
            SqlCommand command = new SqlCommand(insertQuery,_connection);

            try
            {
                _connection.Open();
                command.ExecuteNonQuery();
                
            }
            catch(Exception e)
            {
                throw new Exception("Could not add user to Database!");
            }
            finally
            {
                _connection.Close();
            }
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage("vkovachev04verify@gmail.com", user.Email);
                message.Body = "Confirm your email address:" + Environment.NewLine + $"http://localhost:8090/Home/ConfirmEmail?userId={user.Id}";
                message.Subject = "Confirmation";
                client.Credentials = new System.Net.NetworkCredential("vkovachev04verify@gmail.com", "ixqv jnjw apfh kgmo");
                client.Port = 587;
                client.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
        public bool CheckIfUserExists(string userName)
        {
            var searchQuery = $"SELECT COUNT(*) FROM Users WHERE Username='{userName}'";
            _connection.Open();
            using var command=new SqlCommand(searchQuery,_connection);
             bool output = command.ExecuteScalar().ToString() == "1";
            _connection.Close();
            return output;
        }
        public void ChangeUserInfo(string userId,AccountInfoViewModel model)
        {
            if(model.Password!=model.ConfirmPassword)
            {
                throw new Exception("Passwords don't match!");
            }
            if(model.Password.Length<6 &&model.Password.Length!=0)
            {
                throw new Exception("Passwords must be at least 6 characters long!");
            }
            var (isValid, validationError) = ValidateModel(model);

            if (!isValid)
            {
                throw new Exception(validationError);
            }
            var updateQuery = model.Password.Length==0? $"UPDATE Users SET Username='{model.Username}',Email='{model.Email}' WHERE Id='{userId}'" : $"UPDATE Users SET Username='{model.Username}',Email='{model.Email}',Password='{CalculateHash(model.Password)}' WHERE Id='{userId}'";
            using var command=new SqlCommand(updateQuery,_connection);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void ConfirmEmail(string userId)
        {
            var updateQuery = $"UPDATE Users SET EmailConfirmed=1 WHERE Id='{userId}'";
            SqlCommand command = new SqlCommand(updateQuery,_connection);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public AccountInfoViewModel GetUserInfo(string userId)
        {
            var getUserQuery = $"SELECT Username,Email FROM Users WHERE Id='{userId}'";

            using var command = new SqlCommand(getUserQuery, _connection);
            var model = new AccountInfoViewModel();
            _connection.Open();
            using var reader = command.ExecuteReader();
            {
                while (reader.Read())
                {
                    model.Username = reader[0].ToString();
                    model.Email = reader[1].ToString();
                }
                _connection.Close();
            }
            return model;

        }
        public void DeleteUser(string userId)
        {
            var deleteCommand = $"DELETE FROM Users WHERE Id='{userId}'";

            using var command = new SqlCommand(deleteCommand, _connection);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public string? Login(LoginViewModel model)
        {
            
            var getUserQuery = $"SELECT Id FROM Users WHERE Username ='{model.Username}' AND Password='{CalculateHash(model.Password)}'";
            var command = new SqlCommand(getUserQuery, _connection);
            _connection.Open();
            var userId = command.ExecuteScalar();
            _connection.Close();
            return userId == null ? null : userId.ToString();
        }

        public (bool registered, string error) Register(RegisterViewModel model)
        {
            bool registered = false;
            string error = null;

            var (isValid, validationError) = ValidateModel(model);

            if (!isValid)
            {
                return (isValid, validationError);
            }

            User user = new User()
            {
                Email = model.Email,
                Password = CalculateHash(model.Password),
                Username = model.Username,
            };

            try
            {
                Add(user);
                registered = true;
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return (registered, error);
        }

        private string CalculateHash(string password)
        {
            byte[] passworArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passworArray));
            }
        }
        public (bool isValid, string error) ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var errorResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, errorResult, true);

            if (isValid)
            {
                return (isValid, null);
            }

            string error = String.Join(", ", errorResult.Select(e => e.ErrorMessage));

            return (isValid, error);
        }

    }
}

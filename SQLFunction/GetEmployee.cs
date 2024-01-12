using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SQLFunction
{
    public static class GetEmployee
    {
        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get data from the database");
            List<Employee> _employee_lst = new List<Employee>();
            string _statement = "SELECT EmployeeID,Name,Age from Employees";
            SqlConnection _connection = GetConnection();

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Employee _employee = new Employee()
                    {
                        EmployeeID = _reader.GetInt32(0),
                  
                        Age = _reader.GetInt32(2)
                    };

                    _employee_lst.Add(_employee);
                }
            }
            _connection.Close();

            return new OkObjectResult(_employee_lst);
        }

        private static SqlConnection GetConnection()
        {
            string connectionString = "Server=tcp:employee-db.database.windows.net,1433;Initial Catalog=employee;Persist Security Info=False;User ID=admindb;Password=Tech@2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return new SqlConnection(connectionString);

        }
    }
}

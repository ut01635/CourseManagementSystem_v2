using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementSystem_v2
{
    internal class Program
    {
        
        static void Main(string[] args)
        {




        }

        private readonly string connectionString = "server=(localdb)\\MSSQLLocalDB;database=master";
        public void InitailizeDatabase()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string createDatabase = "IF NOT EXISTS (SELECT * FROM sys.database WHERE name ='CourseManagement')" +
                                         "CREATE DATABASE CourseManagement";

                using (SqlCommand cmd = new SqlCommand(createDatabase, con))
                {
                    cmd.ExecuteNonQuery();
                }

                con.ChangeDatabase("CourseManagement");

                string createTable = @" 
                    IF NOT EXISTS (SELECT  * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses')
                    CREATE TABLE Courses(
                        CourseId INT PRIMARY KEY,
                        Title NVARCHAR(100),
                        Duration  NVARCHAR(100),
                        Price decimal(18, 2)
                    );";

                string InsertData = @"""INSERT INTO Courses (CourseId,Title,Duration,Price) 
                                    VALUES (C_001 ,Java,6 Months, 1.00 );";

                using (SqlCommand command = new SqlCommand(createTable, con))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(InsertData,con))
                {
                    cmd.ExecuteNonQuery();
                }

            }

        }
    }
}

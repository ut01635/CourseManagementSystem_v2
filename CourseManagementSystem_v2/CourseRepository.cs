using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementSystem_v2
{
    public class CourseRepository
    {
        private readonly string ConnectionString = "server=(localdb)\\MSSQLLocalDB;database=master";
        private readonly string databaseConnectionString = "server=(localdb)\\MSSQLLocalDB; database = CourseManagement";
        //public void InitailizeDatabase()
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        con.Open();

        //        string createDatabase = "IF NOT EXISTS (SELECT * FROM sys.database WHERE name ='CourseManagement')" +
        //                                 "CREATE DATABASE CourseManagement";

        //        using (SqlCommand cmd = new SqlCommand(createDatabase, con))
        //        {
        //            cmd.ExecuteNonQuery();
        //        }

        //        con.ChangeDatabase("CourseManagement");

        //        string createTable = @" 
        //            IF NOT EXISTS (SELECT  * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses')
        //            CREATE TABLE Courses(
        //                CourseId INT PRIMARY KEY,
        //                Title NVARCHAR(100),
        //                Duration  NVARCHAR(100),
        //                Price decimal(18, 2)
        //            );";

        //        string InsertData = @"""INSERT INTO Courses (CourseId,Title,Duration,Price) 
        //                            VALUES (C_001 ,Java,6 Months, 1.00 );";

        //        using (SqlCommand command = new SqlCommand(createTable, con))
        //        {
        //            command.ExecuteNonQuery();
        //        }
        //        using (SqlCommand cmd = new SqlCommand(InsertData, con))
        //        {
        //            cmd.ExecuteNonQuery();
        //        }

        //    }

        //}

        public void InitailizeDatabase()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string createDatabase = "IF NOT EXISTS (SELECT * FROM sys.database WHERE name ='CourseManagement')" +
                                         "CREATE DATABASE CourseManagement";
                using (SqlCommand cmd = new SqlCommand(createDatabase, con))
                {
                    cmd.BeginExecuteNonQuery();
                }          

            }

            using (SqlConnection connection = new SqlConnection(databaseConnectionString))
            {
                connection.Open();
                //connection.ChangeDatabase("CourseManagement");

                string createTable = @" 
                    IF NOT EXISTS (SELECT  * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses')
                    CREATE TABLE Courses(
                        CourseId INT PRIMARY KEY,
                        Title NVARCHAR(100),
                        Duration  NVARCHAR(100),
                        Price decimal(18, 2)
                    );"
                ;

                using (SqlCommand command = new SqlCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        

        public void CreateCourse(string Title, string Duration, decimal Price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string capitalizeTitle = CapitalizeTitle(Title);
                    connection.Open();
                    connection.ChangeDatabase("CourseManagement");
                    string Query = "INSERT INTO Courses (Title,Duration,Price) VALUES (@Title,@Duration,@Price)";
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", capitalizeTitle);
                        cmd.Parameters.AddWithValue("@Duration", Duration);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Course Created Successfully");
                    }

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating car: {ex.Message}");
            }
        }

        public void UpdateCourse(int Id, string Title, string Duration, decimal Price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string capitalizeTitle = CapitalizeTitle(Title);
                    connection.Open();
                    connection.ChangeDatabase("CourseManagement");
                    string Query = "UPDATE Courses SET Tite = @title,Duration = @duration, Price=@price WHERE CourseId = @id";
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
                        cmd.Parameters.AddWithValue("@title", capitalizeTitle);
                        cmd.Parameters.AddWithValue("@duration", Duration);
                        cmd.Parameters.AddWithValue("@price", Price);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Course Updated Successfully");
                    }

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating course: {ex.Message}");
            }
        }


        public void DeleteCourse(int Id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CourseManagement");
                    string Query = "DELETE FROM Courses WHERE CourseId = @id";
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Course Deleted Successfully");
                    }

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting course: {ex.Message}");
            }
        }

        public Course ReadCourseById(int Id)
        {
            Course course = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CourseManagement");
                    string Query = "SELECT * FROM Courses WHERE CourseId = @id";
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string CourseId = reader.GetString(0);
                                string Title = reader.GetString(1);
                                string Duration = reader.GetString(2);
                                decimal Price = reader.GetDecimal(3);
                                course = new Course(CourseId, Title, Duration, Price);
                            }
                        }
                    }

                };
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return course;
        }

        public List<Course> ReadAllCourses()
        {
            var courseList = new List<Course>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("CourseManagement");
                    string Query = "SELECT * FROM Courses";
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string CourseId = reader.GetString(0);
                                string Title = reader.GetString(1);
                                string Duration = reader.GetString(2);
                                decimal Price = reader.GetDecimal(3);
                                courseList.Add(new Course(CourseId, Title, Duration, Price));
                            }
                        }
                    }

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return courseList;

        }



        public string CapitalizeTitle(string value)
        {
            string[] words = value.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join(" ", words);
        }



        public decimal ValidateCoursePrice()
        {
            decimal price = 0;
            do
            {
                Console.Write("Enter Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                {
                    Console.Error.WriteLine("Please enter positive price");
                }
            } while (price <= 0);

            return price;
        }



    }
}

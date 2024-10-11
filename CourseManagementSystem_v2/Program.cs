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
            CourseRepository repository = new CourseRepository();
            //repository.InitailizeDatabase();
            int choice;


            do
            {
                Console.Clear();
                Console.WriteLine("Course Management System: ");
                Console.WriteLine("1. Add a Course");
                Console.WriteLine("2. View All Courses");
                Console.WriteLine("3. Update a Course");
                Console.WriteLine("4. Delete a Course");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option:");



                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            CreateCourse(repository);
                            break;
                        case 2:
                            Console.Clear();
                            ReadCourses(repository);

                            break;
                        case 3:
                            Console.Clear();
                            UpdateCourse(repository);


                            break;
                        case 4:
                            Console.Clear();
                            DeleteCourse(repository);

                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("Exiting the system. good bye");
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        default:
                            //Console.Clear();
                            Console.WriteLine("Invalid opion please try again");
                            break;
                    }

                    if (choice != 5)
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (choice != 5);


            void CreateCourse( CourseRepository Repo)
            {
                Console.Write("Enter Course ID: ");
                string id = Console.ReadLine();

                Console.Write("Enter course Title: ");
                string title = Console.ReadLine();

                Console.Write("Enter Duration: ");
                string Duration = Console.ReadLine();

                
                decimal Price = Repo.ValidateCoursePrice();


                Repo.CreateCourse(id,title, Duration, Price);

            }

            void UpdateCourse( CourseRepository repo)
            {
                Console.Write("Enter Course ID to update: "); ;
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter new Title(leave blank to keep current): ");
                string title = Console.ReadLine();

                Console.Write("Enter new Duration(leave blank to keep current): ");
                string duration = Console.ReadLine();

                Console.Write("Enter new Price (leave blank to keep current): ");
                decimal priceInput = repo.ValidateCoursePrice();


               
                repo.UpdateCourse(id, title, duration, priceInput);
            }


            void DeleteCourse( CourseRepository repo)
            {
                Console.WriteLine("Enter course ID  to delete:");
                int id = int.Parse(Console.ReadLine());

                
                repo.DeleteCourse(id);
            }

            void ReadCourseById(CourseRepository repo)
            {
                Console.WriteLine("Enter FitnessProgram ID  to View:");
                int id = int.Parse(Console.ReadLine());

              
                var program = repo.ReadCourseById(id);
                Console.WriteLine(program.ToString());
            }

            void ReadCourses(CourseRepository repo)
            {
                var CourseList = repo.ReadAllCourses();

                if(CourseList != null)
                {
                    Console.WriteLine("---------------Available Courses---------");
                    foreach (var course in CourseList)
                    {
                        Console.WriteLine(course.ToString());
                    }

                }
                else
                {
                    Console.WriteLine("Course Not Found");
                }
                
            }

        }

        
    }
}

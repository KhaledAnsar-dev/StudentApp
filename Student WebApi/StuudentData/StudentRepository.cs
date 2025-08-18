using Microsoft.Data.SqlClient;
using StudentData;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using StudentShared.Dtos;
namespace StuudentData
{
    

    public class StudentRepository
    {
        static public StudentDTO GetStudentByID(int? studentID)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {

                    using (SqlCommand Command = new SqlCommand("SP_GetStudentByID", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        Command.Parameters.AddWithValue("@StudentID", studentID);

                        using (SqlDataReader reader = Command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new StudentDTO
                                   (
                                       reader.GetInt32(reader.GetOrdinal("StudentID")),
                                       reader.GetString(reader.GetOrdinal("Name")),
                                       reader.GetInt32(reader.GetOrdinal("Grade")),
                                       reader.GetInt32(reader.GetOrdinal("Age")));                                      
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }

            return null;
        }
        static public int AddNewStudent(StudentDTO newStudent)
        {
            int CreatedID = 0;

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {

                    using (SqlCommand Command = new SqlCommand("SP_AddNewStudent", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Name", newStudent.name);
                        Command.Parameters.AddWithValue("@Grade", newStudent.grade);
                        Command.Parameters.AddWithValue("@Age", newStudent.age);

                        SqlParameter OutputID = new SqlParameter("@StudentID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        Command.Parameters.Add(OutputID);

                        Connection.Open();

                        Command.ExecuteNonQuery();
                        CreatedID = (int)Command.Parameters["@StudentID"].Value;

                    }
                }

            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }
            return CreatedID;
        }
        static public bool UpdateStudent(StudentDTO updatedStudent)
        {
            int RowAffected = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("SP_UpdateStudent", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@StudentID", updatedStudent.studentID);
                        Command.Parameters.AddWithValue("@Name", updatedStudent.name);
                        Command.Parameters.AddWithValue("@Grade", updatedStudent.grade);
                        Command.Parameters.AddWithValue("@Age", updatedStudent.age);

                        Connection.Open();

                        RowAffected = Command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }
            return RowAffected > 0;
        }
        static public bool DeleteStudent(int studentID)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand("SP_DeleteStudent", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@StudentID", studentID);

                        Connection.Open();

                        RowAffected = Command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }
            return RowAffected > 0;
        }
        static public List<StudentDTO> GetAllStudents()
        {
            var studentList = new List<StudentDTO>();

            try
            {
               using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
               {
                   Connection.Open();
                   using (SqlCommand Command = new SqlCommand("SP_GetAllStudents", Connection))
                   {
                       Command.CommandType = CommandType.StoredProcedure;

                       using (SqlDataReader reader = Command.ExecuteReader())
                       {
                           while(reader.Read())
                           {
                               studentList.Add(new StudentDTO
                                   (
                                       reader.GetInt32(reader.GetOrdinal("StudentID")),
                                       reader.GetString(reader.GetOrdinal("Name")),
                                       reader.GetInt32(reader.GetOrdinal("Grade")),
                                       reader.GetInt32(reader.GetOrdinal("Age"))
                                   ));
                           }
                       }
                   }
               }
            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }

            return studentList;
        }
        static public List<StudentDTO> GetPassedStudents()
        {
            var studentList = new List<StudentDTO>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetPassedStudents", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                studentList.Add(new StudentDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetInt32(reader.GetOrdinal("Grade")),
                                        reader.GetInt32(reader.GetOrdinal("Age"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }

            return studentList;
        }
        static public double GetAverageGrade()
        {
            double averageGrade = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand("SP_GetAverageGrade", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        object result = Command.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            averageGrade = Convert.ToDouble(result);
                        }
                        else
                            averageGrade = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                //string methodName = MethodBase.GetCurrentMethod().Name;
                //clsEventLogger.LogError(ex.Message, methodName);
            }

            return averageGrade;
        }
    }
}

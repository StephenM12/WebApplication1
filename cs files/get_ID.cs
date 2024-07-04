using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;


namespace WebApplication1.cs_files
{
    public class get_ID
    {


        public (int roomID, int sectionID, int courseID, int instructorID) CheckAndInsertValues(SqlConnection connection, string room, string section, string course, string instructor)
        {
            int roomID = GetOrInsertRoom(connection, room);
            int sectionID = GetOrInsertSection(connection, section);
            int courseID = GetOrInsertCourse(connection, course);
            int instructorID = GetOrInsertInstructor(connection, instructor);

            return (roomID, sectionID, courseID, instructorID);
        }

        private int GetOrInsertRoom(SqlConnection connection, string room)
        {
            // Example logic for retrieving or inserting a room
            string query = "SELECT RoomID FROM Rooms WHERE RoomName = @RoomName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Rooms (RoomName) OUTPUT INSERTED.RoomID VALUES (@RoomName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertSection(SqlConnection connection, string section)
        {
            // Example logic for retrieving or inserting a section
            string query = "SELECT SectionID FROM Sections WHERE SectionName = @SectionName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SectionName", section);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Sections (SectionName) OUTPUT INSERTED.SectionID VALUES (@SectionName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@SectionName", section);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertCourse(SqlConnection connection, string course)
        {
            // Example logic for retrieving or inserting a course
            string query = "SELECT CourseID FROM Courses WHERE CourseCode = @CourseCode";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseCode", course);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Courses (CourseCode) OUTPUT INSERTED.CourseID VALUES (@CourseCode)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@CourseCode", course);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertInstructor(SqlConnection connection, string instructor)
        {
            // Example logic for retrieving or inserting an instructor
            string query = "SELECT InstructorID FROM Instructors WHERE InstructorName = @InstructorName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InstructorName", instructor);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Instructors (InstructorName) OUTPUT INSERTED.InstructorID VALUES (@InstructorName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@InstructorName", instructor);
                return (int)command.ExecuteScalar();
            }
        }


    }
}
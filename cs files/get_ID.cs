﻿using System;

//sql connection:
using System.Data.SqlClient;

//using WebApplication1.cs_files;

namespace WebApplication1.cs_files
{
    public class get_ID
    {
        public (int roomID, int sectionID, int courseID, int instructorID) CheckAndInsertValues(SqlConnection connection, string room, string section, string course, string instructor, int buildingid)
        {
            int roomID = GetOrInsertRoom(connection, room, buildingid, true);
            int sectionID = GetOrInsertSection(connection, section);
            int courseID = GetOrInsertCourse(connection, course);
            int instructorID = GetOrInsertInstructor(connection, instructor);

            return (roomID, sectionID, courseID, instructorID);
        }

        public int GetDayID(string dayName)
        {
            switch (dayName)
            {
                case "SUNDAY":
                    return 1;

                case "MONDAY":
                    return 2;

                case "TUESDAY":
                    return 3;

                case "WEDNESDAY":
                    return 4;

                case "THURSDAY":
                    return 5;

                case "FRIDAY":
                    return 6;

                case "SATURDAY":
                    return 7;

                default:
                    throw new ArgumentException("Invalid day name");
            }
        }

        public int GetOrInsertBuilding(SqlConnection connection, string building, bool addITEM = false)
        {
            // Example logic for retrieving or inserting a room
            string query = "SELECT BuildingID FROM Buildings WHERE BuildingName = @BuildingName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BuildingName", building);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            if (addITEM)
            {
                string insertQuery = "INSERT INTO Buildings (BuildingName) OUTPUT INSERTED.BuildingID VALUES (@BuildingName)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@BuildingName", building.ToUpper());
                    return (int)command.ExecuteScalar();
                }
            }

            // Log that the room was not found and not added
            Console.WriteLine("Building not found.");
            return -1;
        }

        public int GetOrInsertRoom(SqlConnection connection, string room, int buildingID, bool addITEM = false)
        {
            string query = "SELECT RoomID FROM Rooms WHERE RoomName = @RoomName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    int roomID = Convert.ToInt32(result);
                    // Log or output the retrieved RoomID for debugging
                    Console.WriteLine($"RoomID found: {roomID}");
                    return roomID;
                }
            }

            if (addITEM)
            {
                string insertQuery = "INSERT INTO Rooms (RoomName, BuildingID) OUTPUT INSERTED.RoomID VALUES (@RoomName, @BuildingID)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoomName", room.ToUpper());
                    command.Parameters.AddWithValue("@BuildingID", buildingID);
                    int newRoomID = (int)command.ExecuteScalar();
                    // Log or output the new RoomID for debugging
                    Console.WriteLine($"New RoomID inserted: {newRoomID}");
                    return newRoomID;
                }
            }

            // Log that the room was not found and not added
            Console.WriteLine("Room not found.");
            return -1;
        }

        public int GetOrInsertSection(SqlConnection connection, string section)
        {
            // Check if the section is empty or contains only whitespace
            //if (string.IsNullOrWhiteSpace(section))
            //{
            //    return -1; // Return -1 for empty or whitespace-only sections
            //}

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
                command.Parameters.AddWithValue("@SectionName", section.ToUpper());
                return (int)command.ExecuteScalar();
            }
        }

        public int GetOrInsertCourse(SqlConnection connection, string course)
        {
            // Check if the section is empty or contains only whitespace
            //if (string.IsNullOrWhiteSpace(course))
            //{
            //    return -1; // Return -1 for empty or whitespace-only sections
            //}

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
                command.Parameters.AddWithValue("@CourseCode", course.ToUpper());
                return (int)command.ExecuteScalar();
            }
        }

        public int GetOrInsertInstructor(SqlConnection connection, string instructor)
        {
            // Check if the section is empty or contains only whitespace
            //if (string.IsNullOrWhiteSpace(instructor))
            //{
            //    return -1; // Return -1 for empty or whitespace-only sections
            //}

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
                command.Parameters.AddWithValue("@InstructorName", instructor.ToUpper());
                return (int)command.ExecuteScalar();
            }
        }
    }
}
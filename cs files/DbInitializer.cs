using System;
using System.Data.SqlClient;

namespace WebApplication1.cs_files
{
    public static class DbInitializer
    {
        public static void EnsureTablesAndTriggersExist()
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                //connection.Open(); // Open the connection first
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Create tables in the correct order
                    EnsureTableExists(connection, "Buildings", GetCreateBuildingsTableQuery(), transaction);
                    EnsureTableExists(connection, "Sections", GetCreateSectionsTableQuery(), transaction);
                    EnsureTableExists(connection, "Courses", GetCreateCoursesTableQuery(), transaction);
                    EnsureTableExists(connection, "Instructors", GetCreateInstructorsTableQuery(), transaction);
                    EnsureTableExists(connection, "upload_SchedsTBL", GetCreateUploadSchedsTBLTableQuery(), transaction);
                    EnsureTableExists(connection, "Rooms", GetCreateRoomsTableQuery(), transaction);
                    EnsureTableExists(connection, "Schedule", GetCreateScheduleTableQuery(), transaction);
                    EnsureTableExists(connection, "userInfo", GetCreateUserInfoTableQuery(), transaction);
                    EnsureTableExists(connection, "pinCodes", GetCreatePinCodesTableQuery(), transaction);
                    EnsureTableExists(connection, "RoomRequest", GetCreateRoomRequestTableQuery(), transaction);
                    EnsureTableExists(connection, "RoomRequestHistory", GetCreateRoomRequestHistoryTableQuery(), transaction);
                    EnsureTableExists(connection, "EPaperAssignedRoom", GetCreateEPaperAssignedRoomTableQuery(), transaction);
                    EnsureTableExists(connection, "days_of_week", GetCreatedays_of_weekTableQuery(), transaction);
                    EnsureTableExists(connection, "Faculty", GetCreateFacultyTableQuery(), transaction);

                    // Ensure triggers exist
                    EnsureTriggerExists(connection, "trg_AfterInsert_RoomRequest", GetAfterInsertRoomRequestTriggerQuery(), transaction);
                    EnsureTriggerExists(connection, "trg_AfterUpdate_RoomRequest", GetAfterUpdateRoomRequestTriggerQuery(), transaction);
                    EnsureTriggerExists(connection, "trg_AfterUpdate_RoomRequest_DeployReject", GetAfterUpdateRoomRequestDeployRejectTriggerQuery(), transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error ensuring tables and triggers exist: " + ex.Message);
                }
                finally
                {
                    //connection.Close();
                }
            }
        }

        private static void EnsureTableExists(SqlConnection connection, string tableName, string createTableQuery, SqlTransaction transaction)
        {
            if (!TableExists(connection, tableName, transaction))
            {
                using (SqlCommand command = new SqlCommand(createTableQuery, connection, transaction))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{tableName} table created successfully.");
                }
            }
        }

        private static bool TableExists(SqlConnection connection, string tableName, SqlTransaction transaction)
        {
            string query = $@"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = @TableName";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TableName", tableName);
                return (int)command.ExecuteScalar() > 0;
            }
        }

        private static void EnsureTriggerExists(SqlConnection connection, string triggerName, string createTriggerQuery, SqlTransaction transaction)
        {
            if (!TriggerExists(connection, triggerName, transaction))
            {
                using (SqlCommand command = new SqlCommand(createTriggerQuery, connection, transaction))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{triggerName} trigger created successfully.");
                }
            }
        }

        private static bool TriggerExists(SqlConnection connection, string triggerName, SqlTransaction transaction)
        {
            string query = $@"
                SELECT COUNT(*)
                FROM sys.triggers
                WHERE name = @TriggerName";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TriggerName", triggerName);
                return (int)command.ExecuteScalar() > 0;
            }
        }

        // Trigger creation queries
        private static string GetAfterInsertRoomRequestTriggerQuery() => @"
            CREATE TRIGGER trg_AfterInsert_RoomRequest
            ON RoomRequest
            AFTER INSERT
            AS
            BEGIN
                INSERT INTO RoomRequestHistory (RequestID, RequestDateTime, Status, UpdatedBy, RequestedByEmail)
                SELECT 
                    i.RequestID, 
                    GETDATE() AS RequestDateTime, 
                    i.status, 
                    ' ' AS UpdatedBy,
                    i.RequestedByEmail
                FROM inserted i;
            END;";

        private static string GetAfterUpdateRoomRequestTriggerQuery() => @"
            CREATE TRIGGER trg_AfterUpdate_RoomRequest
            ON RoomRequest
            AFTER UPDATE
            AS
            BEGIN
                INSERT INTO RoomRequestHistory (RequestID, RequestDateTime, ApprovalDateTime, Status, UpdatedBy, RequestedByEmail)
                SELECT 
                    u.RequestID,
                    GETDATE() AS RequestDateTime,
                    CASE WHEN u.DateApproved IS NOT NULL THEN u.DateApproved ELSE NULL END AS ApprovalDateTime,
                    u.status,
                    u.UpdatedBy,
                    u.RequestedByEmail
                FROM inserted u
                WHERE (u.status = 'Accepted' OR u.status = 'Rejected') 
                AND (UPDATE(status) OR UPDATE(DateApproved));
            END;";

        private static string GetAfterUpdateRoomRequestDeployRejectTriggerQuery() => @"
            CREATE TRIGGER trg_AfterUpdate_RoomRequest_DeployReject
            ON RoomRequest
            AFTER UPDATE
            AS
            BEGIN
                INSERT INTO RoomRequestHistory (RequestID, RequestDateTime, ApprovalDateTime, Status, UpdatedBy, RequestedByEmail)
                SELECT 
                    u.RequestID,
                    GETDATE() AS RequestDateTime,           
                    CASE WHEN u.DateApproved IS NOT NULL THEN u.DateApproved ELSE NULL END AS ApprovalDateTime,
                    u.status, 
                    u.UpdatedBy,
                    u.RequestedByEmail
                FROM inserted u
                WHERE u.status IN ('Deployed', 'Rejected')
                AND (UPDATE(status) OR UPDATE(DateApproved));
            END;";

        // Table creation queries
        private static string GetCreateBuildingsTableQuery() => @"
            CREATE TABLE Buildings (
                BuildingID INT IDENTITY(1,1) PRIMARY KEY,
                BuildingName VARCHAR(50) NOT NULL
            );";

        private static string GetCreateRoomsTableQuery() => @"
            CREATE TABLE Rooms (
                RoomID INT IDENTITY(1,1) PRIMARY KEY,
                RoomName VARCHAR(50) NOT NULL,
                BuildingID INT,
                FOREIGN KEY (BuildingID) REFERENCES Buildings(BuildingID)
            );";

        private static string GetCreateSectionsTableQuery() => @"
            CREATE TABLE Sections (
                SectionID INT IDENTITY(1,1) PRIMARY KEY,
                SectionName VARCHAR(50) NOT NULL
            );";

        private static string GetCreateCoursesTableQuery() => @"
            CREATE TABLE Courses (
                CourseID INT IDENTITY(1,1) PRIMARY KEY,
                CourseCode VARCHAR(50) NOT NULL
            );";

        private static string GetCreateInstructorsTableQuery() => @"
            CREATE TABLE Instructors (
                InstructorID INT IDENTITY(1,1) PRIMARY KEY,
                InstructorName VARCHAR(100) NOT NULL
            );";

        private static string GetCreateUserInfoTableQuery() => @"
            CREATE TABLE userInfo (
                UserID INT IDENTITY(1,1) PRIMARY KEY,
                UserName VARCHAR(50) UNIQUE NOT NULL,
                UserPassword VARCHAR(50) NOT NULL,
                FirstName TEXT,
                LastName TEXT,
                Email VARCHAR(50) UNIQUE NOT NULL,
                UserLevel INT NOT NULL,
                Faculty VARCHAR(255)
            );

            INSERT INTO userInfo (UserName, UserPassword, FirstName, LastName, Email, UserLevel, Faculty)
            VALUES ('admin', 'yeah', 'admin', 'admin_user', 'adminexample@gmail.com', 1, 'N/A');";

        private static string GetCreatePinCodesTableQuery() => @"
            CREATE TABLE pinCodes (
                PinCode VARCHAR(4) PRIMARY KEY,
                ExpiryTime DATETIME
            );";

        private static string GetCreateScheduleTableQuery() => @"
            CREATE TABLE Schedule (
                ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
                RoomID INT,
                SectionID INT,
                CourseID INT,
                InstructorID INT,
                DayID INT, 
                StartTime TIME,
                EndTime TIME,
                ScheduleDate DATE,
                Remarks VARCHAR(255),
                BuildingID INT,
                UploadID INT,
                FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID),
                FOREIGN KEY (SectionID) REFERENCES Sections(SectionID),
                FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
                FOREIGN KEY (InstructorID) REFERENCES Instructors(InstructorID),
                FOREIGN KEY (BuildingID) REFERENCES Buildings(BuildingID),
                FOREIGN KEY (UploadID) REFERENCES upload_SchedsTBL(UploadID)
            );";

        private static string GetCreateUploadSchedsTBLTableQuery() => @"
            CREATE TABLE upload_SchedsTBL (
                UploadID INT IDENTITY(1,1) PRIMARY KEY,
                FileName VARCHAR(50) UNIQUE NOT NULL,
                ContentType NVARCHAR(200),
                Data VARBINARY(MAX),
                Uploader VARCHAR(20),
                UploadDate DATE
            );";

        private static string GetCreateRoomRequestTableQuery() => @"
            CREATE TABLE RoomRequest (
                RequestID INT IDENTITY(1,1) PRIMARY KEY,
                email VARCHAR(50),
                Course VARCHAR(50),
                Section VARCHAR(50),
                Instructor VARCHAR(50),
                Faculty VARCHAR(255),
                PurposeoftheRoom VARCHAR(255),
                Building VARCHAR(50),
                Room VARCHAR(50),
                StartDate DATE,
                EndDate DATE,
                startTime TIME,
                endTime TIME,
                DateApproved DATETIME,
                status VARCHAR(50),
                FileName NVARCHAR(255),
                FileData VARBINARY(MAX),
                ContentType NVARCHAR(200),
                RequestedByEmail VARCHAR(50),
                UpdatedBy VARCHAR(50),
                Requester_Faculty VARCHAR(255)
            );";

        private static string GetCreateRoomRequestHistoryTableQuery() => @"
            CREATE TABLE RoomRequestHistory (
                HistoryID INT IDENTITY(1,1) PRIMARY KEY,
                RequestID INT NOT NULL,
                RequestDateTime DATETIME NOT NULL,
                ApprovalDateTime DATETIME NULL,
                Status VARCHAR(50) NOT NULL,
                UpdatedBy VARCHAR(255),
                RequestedByEmail VARCHAR(50),
                FOREIGN KEY (RequestID) REFERENCES RoomRequest(RequestID)
            );";

        private static string GetCreateFacultyTableQuery() => @"
            CREATE TABLE Faculty (
            FacultyID INT IDENTITY(1,1) PRIMARY KEY, 
            FacultyCode VARCHAR(10) NOT NULL
        );";
        //for jm
        private static string GetCreateEPaperAssignedRoomTableQuery() => @"
            CREATE TABLE EPaperAssignedRoom (
            ID INT IDENTITY(1,1) PRIMARY KEY,
            EPD_ID VARCHAR(30),
            AssignedBuilding VARCHAR (50),
            AssignedRoomNumber VARCHAR(20),
            AssignedIPAddress VARCHAR(45),
            DeviceStatus VARCHAR(30),
            AssignedPHPFile VARCHAR(30)
            ); ";
        private static string GetCreatedays_of_weekTableQuery() => @"
            CREATE TABLE days_of_week (
            DayID INT PRIMARY KEY IDENTITY (1,1),
            day VARCHAR(30));";

    }
}

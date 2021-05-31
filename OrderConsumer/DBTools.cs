using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;


namespace OrderConsumer
{
    class DBTools
    {
        //constants
        private const string EXT_DATABASE = ".mdf";
        private const string EXT_LOG = ".ldf";
       

        //properties
        public string DatabaseName { get; set; }
        public string LogName { get; set; }
        public string Path { get; set; }

        //methods
        public void CreateDatabase()
        {
            String str;
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CreateDatabase"].ConnectionString);

            str = "CREATE DATABASE " + this.DatabaseName;

            if (!CheckIfDatabaseExists(this.DatabaseName))
            {
                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {
                    Console.WriteLine("DBTools.CreateDatabase: Please wait, creating database...");
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    Console.WriteLine("DBTools.CreateDatabase: Database is Created Successfully");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("DBTools.CreateDatabase:" + ex.ToString());
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            else
            {
                Console.WriteLine("DBTools.CreateDatabase: Database already exists!");
            }
        }

        public void CreateTable()
        {
            SqlCommand cmd;
            SqlConnection myConn = new SqlConnection();
            string sql;

            // Open the myConnection  
            if (myConn.State == ConnectionState.Open)
                myConn.Close();
            myConn.ConnectionString = ConfigurationManager.ConnectionStrings["myDatabase"].ConnectionString;
            myConn.Open();
            sql = "CREATE TABLE OrderEntries" +
            "(Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY," +
            "Status BIT, FileName CHAR(255), FileDate DATETIME, XMLContent XML )";
            cmd = new SqlCommand(sql, myConn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("DBTools.CreateTable: " + e.Message.ToString());
            }
            finally
            {
                myConn.Close();
            }

        }

        public bool CheckIfDatabaseExists(string databaseName)
        {
            using (var myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CreateDatabase"].ConnectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", myConn))
                {
                    myConn.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public void InsertRecord(bool status, string fileName, DateTime dateTime, string xmlContent)
        {
            string sql = string.Empty;

            // Prepare a proper parameterized query 
            sql = "insert into OrderEntries ([Status], [FileName],[FileDate],[XMLContent]) values(@status,@filename,@filedate,@xmlcontent)";

            // Create the connection (and be sure to dispose it at the end)
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["myDatabase"].ConnectionString))
            {
                try
                {
                    myConn.Open();

                    // Prepare the command to be executed on the db
                    using (SqlCommand cmd = new SqlCommand(sql, myConn))
                    {
                        cmd.Parameters.Add("@status", SqlDbType.Bit).Value = status;
                        cmd.Parameters.Add("@filename", SqlDbType.NChar).Value = fileName;
                        cmd.Parameters.Add("@filedate", SqlDbType.DateTime).Value = dateTime;
                        cmd.Parameters.Add("@xmlcontent", SqlDbType.Xml).Value = xmlContent;

                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            Console.WriteLine("DBTools.InsertRecord: Row inserted!!");
                        else
                            // Well this should never really happen
                            Console.WriteLine("DBTools.InsertRecord: No row inserted");
                    }
                }
                catch (Exception ex)
                {
                    //Show error message. Should be added to log file
                    Console.WriteLine("DBTools.InsertRecord: " + ex.Message);
                }
                finally
                {
                    myConn.Close();
                }
            }
        }

        public bool CheckIfRecordExists(string filename)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["myDatabase"].ConnectionString);

            DataTable dt = new DataTable();

            try
            {
                myConn.Open();
                String sql = "SELECT * FROM OrderEntries WHERE OrderEntries.FileName = @filename";
                SqlCommand cmd = new SqlCommand(sql, myConn);
                cmd.Parameters.AddWithValue("@filename", filename);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                               
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine("DBTools.CheckIfRecordExists: " + ex.Message);
                return false;
            }
            finally
            {
                myConn.Close();
            }
        }
    }
}

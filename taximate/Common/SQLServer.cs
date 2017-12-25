using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;

namespace Common
{
    class SQLServer
    {
        private SqlConnection conn;

        public SQLServer()
        {
            conn = new SqlConnection();
            //conn.ConnectionString = @"workstation id=JHEY;packet size=4096;user id=sa; password=burgundy; data source=192.168.0.75;persist security info=False;initial catalog=Webservice";
            conn.ConnectionString = ConfigurationSettings.AppSettings["ConnectionString"];

        }

        public void UpdateTransaction(string Trancode)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "updPollingIntegTransaction";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Trancode", SqlDbType.VarChar, 40);
            cmd.Parameters["@Trancode"].Value = Trancode;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        public void UpdateTransactionDetail(string Trancode, int JobId, string Action)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "updPollingIntegTransactionDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Trancode", SqlDbType.VarChar, 40);
            cmd.Parameters.Add("@JobId", SqlDbType.Int, 4);
            cmd.Parameters.Add("@Action", SqlDbType.Char, 1);
            cmd.Parameters["@Trancode"].Value = Trancode; 
            cmd.Parameters["@JobId"].Value = JobId;
            cmd.Parameters["@Action"].Value = Action;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch(Exception exc)
            {
                int i = 1;
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        public bool updCloseJobs(int JobId, string Action, decimal OriginalQuote, decimal ExtWaiting, decimal ExtParking, decimal ExtTolls, decimal ExtAmendment, decimal ExtPhone, decimal ExtCancellation, string ExtReason, float TotalHours)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insPollingIntegClosing";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@JobId", SqlDbType.Int, 4);
            cmd.Parameters.Add("@Action", SqlDbType.Char, 1);
            cmd.Parameters.Add("@OriginalQuote", SqlDbType.Money);
            cmd.Parameters.Add("@ExtWaiting", SqlDbType.Money);
            cmd.Parameters.Add("@ExtParking", SqlDbType.Money);
            cmd.Parameters.Add("@ExtTolls", SqlDbType.Money);
            cmd.Parameters.Add("@ExtAmendment", SqlDbType.Money);
            cmd.Parameters.Add("@ExtPhone", SqlDbType.Money);
            cmd.Parameters.Add("@ExtCancellation", SqlDbType.Money);
            cmd.Parameters.Add("@ExtReason", SqlDbType.VarChar,255);
            cmd.Parameters.Add("@TotalHours", SqlDbType.Float);
            cmd.Parameters["@JobId"].Value = JobId;
            cmd.Parameters["@Action"].Value = Action;            
            cmd.Parameters["@OriginalQuote"].Value = new SqlMoney(OriginalQuote);
            cmd.Parameters["@ExtWaiting"].Value = new SqlMoney(ExtWaiting);
            cmd.Parameters["@ExtParking"].Value = new SqlMoney(ExtParking);
            cmd.Parameters["@ExtTolls"].Value = new SqlMoney(ExtTolls);           
            cmd.Parameters["@ExtAmendment"].Value = new SqlMoney(ExtAmendment);
            cmd.Parameters["@ExtPhone"].Value = new SqlMoney(ExtPhone);
            cmd.Parameters["@ExtCancellation"].Value = new SqlMoney(ExtCancellation);         
            cmd.Parameters["@ExtReason"].Value = ExtReason;
            cmd.Parameters["@TotalHours"].Value = TotalHours;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }

            return true;


        }

        public DataSet GetAvailableJobs()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "getPollingIntegJobs";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, "AvailableJobs");

            return ds;
        }



        public void InsertPollingJobIdPollingJourneyDetails(int PollingJobId, int PollingJourneyDetails)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "InsertPollingJobIdPollingJourneyDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PollingJobId", SqlDbType.Int, 4);
            cmd.Parameters.Add("@PollingJourneyDetails", SqlDbType.Int, 4);
            cmd.Parameters["@PollingJobId"].Value = PollingJobId;
            cmd.Parameters["@PollingJourneyDetails"].Value = PollingJourneyDetails;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        public void InsertPollingIntegTransactionDetails(string Trancode, int PollingId, int JobId)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "InsertPollingIntegTransactionDetail";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Trancode", SqlDbType.VarChar, 40);
            cmd.Parameters.Add("@JobId", SqlDbType.Int, 4);
            cmd.Parameters.Add("@PollingId", SqlDbType.Int, 4);
            cmd.Parameters["@Trancode"].Value = Trancode;
            cmd.Parameters["@JobId"].Value = JobId;
            cmd.Parameters["@PollingId"].Value = PollingId;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        public void InsertPollingIntegTransaction(string Trancode)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "InsertPollingIntegTransactionDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Trancode", SqlDbType.VarChar, 40);
            cmd.Parameters["@Trancode"].Value = Trancode;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            return;
        }

        public bool IsNewJob(int JobId)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "getPollingJobStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@JobId", SqlDbType.Int, 4);
            cmd.Parameters["@JobId"].Value = JobId;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return false;
                else
                    return true;
            }
            finally
            {
                conn.Close();
            }

            return true;

        }


        //created store procedure for WorkOrder=1603
        public DataSet GetchangedPollingIntegJobs_Hist(int JobId)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "AmendedPollingIntegJobs";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@JobId", SqlDbType.Int, 4);
            cmd.Parameters["@JobId"].Value = JobId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds, "AvailableJobs");

            return ds;
        }

    }

    
}




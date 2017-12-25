using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAvailableJobs_Click(object sender, EventArgs e)
        {
            BTRRequest request = new BTRRequest();

            request.UserID = "Travel";
            request.Password = "1Q2w3e4R";
            request.Supplier = 10;
            request.Request = "AvailableJobs";
            request.DateStamp = DateTime.Now.ToString();
            GSPollingInteg.Service test = new GSPollingInteg.Service();

            //test.Url = "http://development.groundscope.co.uk/Integrations/ServicePartners/Services/Service.asmx";
            //test.Url = "http://staging.groundscope.co.uk/Integrations/ServiceProviders/Service/Service.asmx";
            test.Url = "http://www.groundcontroller.com/SPIntegration/Service.asmx";
             //test.Url = "http://localhost:1698/PollingIntegration/Service.asmx";
             //   http://192.168.0.75/Integrations/ServicePartners/Service/Service.asmx

            string sRequest = request.serialise();

            test.Timeout = 1000000;

            string response = test.GSService(sRequest);

            //BTRResponse responseObj = BTRResponse.Deserialize(response);

            int i = 0;
            
        }

        private void btnProcessAJ_Click(object sender, EventArgs e)
        {
            BTRRequest request = new BTRRequest();
            BTRResponse response = new BTRResponse();

            AvailableJobs aj = new AvailableJobs();
           // aj.Process(request, ref response);
            int i = 1;
        }

        private void btnAcceptJobs_Click(object sender, EventArgs e)

        {
            BTRRequest request = new BTRRequest();
            request.UserID = "Dev";
            request.Password = "Dev";
            request.Supplier = 11;
            request.Request = "ConfirmReceived";
            request.DateStamp = DateTime.Now.ToString();
            request.TransCode = "63351902807779429720";

            // Individual Jobs
            BTRRequest.JobDetail job1 = new BTRRequest.JobDetail();
            job1.Action = "O";
            job1.Jobid = 3956364;
            job1.ErrorCode = 0;
            job1.ErrorDesc = "NA";
            request.AddJobDetail(job1);

            //BTRRequest.JobDetail job2 = new BTRRequest.JobDetail();
            //job2.Action = "O";
            //job2.Jobid = 3956419;
            //job2.ErrorCode = 0;
            //job2.ErrorDesc = "NA";
            //request.AddJobDetail(job2);

            //BTRRequest.JobDetail job3 = new BTRRequest.JobDetail();
            //job3.Action = "O";
            //job3.Jobid = 3956389;
            //job3.ErrorCode = 0;
            //job3.ErrorDesc = "NA";
            //request.AddJobDetail(job3);

            





            /*
            BTRRequest.JobDetail job2 = new BTRRequest.JobDetail();
            job2.Action = "X";
            job2.Jobid = 3956419;
            job2.ErrorCode = 300;
            job2.ErrorDesc = "Unavailable to understand address";
            request.AddJobDetail(job2);

            BTRRequest.JobDetail job3 = new BTRRequest.JobDetail();
            job3.Action = "O";
            job3.Jobid = 3964792;
            job3.ErrorCode = 0;
            job3.ErrorDesc = "NA";
            request.AddJobDetail(job3);

            BTRRequest.JobDetail job4 = new BTRRequest.JobDetail();
            job4.Action = "O";
            job4.Jobid = 3964793;
            job4.ErrorCode = 0;
            job4.ErrorDesc = "NA";
            request.AddJobDetail(job4);

            BTRRequest.JobDetail job5 = new BTRRequest.JobDetail();
            job5.Action = "O";
            job5.Jobid = 3964796;
            job5.ErrorCode = 0;
            job5.ErrorDesc = "NA";
            request.AddJobDetail(job5);

            BTRRequest.JobDetail job6 = new BTRRequest.JobDetail();
            job6.Action = "O";
            job6.Jobid = 3964797;
            job6.ErrorCode = 0;
            job6.ErrorDesc = "NA";
            request.AddJobDetail(job6);
            */
            
            GSPollingInteg.Service test = new GSPollingInteg.Service();
            string sRequest = request.serialise();
            string response = test.GSService(sRequest);

            //BTRResponse responseObj = BTRResponse.Deserialize(response);
            int i = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            BTRRequest request = new BTRRequest();
            request.UserID = "Dev";
            request.Password = "Dev";
            request.Supplier = 11;
            request.Request = "CloseBooking";
            request.DateStamp = DateTime.Now.ToString();
            request.JobID = "3964797";
            request.Action = "C";

            // Closing Values
            request.OriginalQuote = "0";
            request.ExtWaiting = "1.0";
            request.ExtParking = "2.0";
            request.ExtTolls = "3";
            request.ExtAmendment = "4.0";
            request.ExtPhone = "5.0";
            request.ExtCancellation = "6.0";
            request.ExtReason = "7";
            request.TotalHours = "8.5";
            request.BatchedJobID = "0";
            request.LinkedJobID = "0";

            GSPollingInteg.Service test = new GSPollingInteg.Service();
            string sRequest = request.serialise();
            test.Timeout = 1000000;
            string response = test.GSService(sRequest);

            //BTRResponse responseObj = BTRResponse.Deserialize(response);

            int i = 0;
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            BTRRequest request = new BTRRequest();
            request.UserID = "Dev";
            request.Password = "Dev";
            request.Supplier = 11;
            request.Request = "CloseBooking";
            request.DateStamp = DateTime.Now.ToString();
            request.JobID = "3964793";
            request.Action = "R";

            GSPollingInteg.Service test = new GSPollingInteg.Service();
            string sRequest = request.serialise();
            test.Timeout = 1000000;
            string response = test.GSService(sRequest);

            //BTRResponse responseObj = BTRResponse.Deserialize(response);

            int i = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ConfirmReceived
    {
        public void Process(BTRRequest request, ref ResponseConfirmReceived response)
        {
            SQLServer sqlserver = new SQLServer();
            string trancode = request.TransCode;
            // Check if trancode has already been recieved
            sqlserver.UpdateTransaction(trancode + "-" + request.UserID);

            foreach (BTRRequest.JobDetail jobDetail in request.JobDetailArray)
            {
                sqlserver.UpdateTransactionDetail(trancode + "-" + request.UserID, jobDetail.Jobid, jobDetail.Action);

                // Add code here to move Successfully actions into Inprogress and Failures into Waiting
            }

            response.TransCode = trancode;
            response.Result = "OK";
            response.ErrorCode = 0;
            response.ErrorDesc = "NA";

            return;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class CloseBooking
    {
        public void Process(BTRRequest request, ref ResponseCloseBooking response)
        {
            SQLServer server = new SQLServer();

            response.JobID = int.Parse(request.JobID);
            response.Action = request.Action;
            response.Request = "CloseProcessed";

            if (request.Action == "C")
            {

                decimal originalQuote = decimal.Parse(request.OriginalQuote);
                decimal extWaiting = decimal.Parse(request.ExtWaiting);
                decimal extParking = decimal.Parse(request.ExtParking);
                decimal extTolls = decimal.Parse(request.ExtTolls);
                decimal extAmendment = decimal.Parse(request.ExtAmendment);
                decimal extPhone = decimal.Parse(request.ExtPhone);
                decimal extCancellation = decimal.Parse(request.ExtCancellation);
                float totalHours = float.Parse(request.TotalHours);

                if (server.updCloseJobs(int.Parse(request.JobID), request.Action, originalQuote, extWaiting, extParking, extTolls, extAmendment, extPhone, extCancellation, request.ExtReason, totalHours))
                {
                    response.ErrorCode = 0;
                    response.ErrorDesc = "NA";
                    response.Request = "OK";
                }
                else
                {
                    response.ErrorCode = 9000;
                    response.ErrorDesc = "Failed to update Db";
                    response.Request = "Error";
                }

            }
            else
            {
                // Add decline code here
                if (request.Action == "R")
                {
                    response.ErrorCode = 0;
                    response.ErrorDesc = "NA";
                    response.Request = "OK";
                }
                else
                {
                    response.ErrorCode = 9000;
                    response.ErrorDesc = "Action not valid";
                    response.Request = "Error";
                }
            }

            return;
        }
    }
}

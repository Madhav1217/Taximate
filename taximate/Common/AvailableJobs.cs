using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common
{
    public class AvailableJobs
    {
        public void Process(BTRRequest request, ref ResponseAvailableJobs response)
        {
            try
            {
                SQLServer server = new SQLServer();
                ParseAvailableJobs(server.GetAvailableJobs(), ref response, request.UserID);
                response.Result = "OK";
                response.ErrorDesc = "NA";
                response.ErrorCode = 0;
            }
            catch(Exception exc)
            {
                response.Result = "ERROR";
                response.ErrorDesc = exc.Message;
                response.ErrorCode = 9000;
                Log.LogToFile("Process:" + exc.Message + exc.StackTrace + exc.Source);
            }
            return;
        }

        private void ParseAvailableJobs(DataSet ds, ref ResponseAvailableJobs response, string UserID)
        {
            try
            {
                Random random = new Random();
                SQLServer server = new SQLServer();
                DataTable dt = ds.Tables["AvailableJobs"];

                string trancode = String.Format("{0}{1}", DateTime.Now.Ticks, random.Next(99).ToString());

                //Guid trancode = Guid.NewGuid();

                response.TransCode = trancode;
                trancode = String.Format("{0}-{1}", trancode, UserID);

                server.InsertPollingIntegTransaction(trancode);

                // Get a list of JobId contained in Available Jobs
                List<Int32> jobIds = new List<int>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (!jobIds.Contains(int.Parse(dr["JobId"].ToString())))
                        jobIds.Add(int.Parse(dr["JobId"].ToString()));
                }

                foreach (Int32 jobId in jobIds)
                {
                    ResponseAvailableJobs.JobDetail jobDetail = new ResponseAvailableJobs.JobDetail();

                    DataRow[] jobData = dt.Select(String.Format("JobId ={0}", jobId));
                    // Read Job Details
                    int id = int.Parse(jobData[0]["Id"].ToString());
                    DateTime pickupDateTime = DateTime.Parse(jobData[0]["PickupDateTime"].ToString());
                    string passenger = jobData[0]["Passenger"].ToString();
                    string mobile = jobData[0]["Mobile"].ToString();
                    string notes = jobData[0]["Notes"].ToString();

                    //created journeymileage column in table PollingIntegJobs and sending as a request  for WorkOrder=1603
                    string journeymileage = jobData[0]["JourneyMileage"].ToString();
                    // Remove Ride2Job comment
                    notes = notes.Replace("RIDE2JOB", "");

                    string serviceType = jobData[0]["ServiceType"].ToString();
                    int brand = int.Parse(jobData[0]["BrandId"].ToString());
                    string customer = jobData[0]["CustomerId"].ToString();
                    string contact = jobData[0]["Contact"].ToString();
                    string journeyType = jobData[0]["JourneyType"].ToString();
                    int cancelled = int.Parse(jobData[0]["Cancelled"].ToString());
                    DateTime entered = DateTime.Parse(jobData[0]["Entered"].ToString());
                    DateTime enteredJourneyDetails = DateTime.Parse(jobData[0][24].ToString());
                    int idJourneyDetailFirstRecord = int.Parse(jobData[0][15].ToString());

                    if (cancelled == 1)
                    {
                        jobDetail.Action = "D";
                    }
                    else
                    {
                        jobDetail.Action = CalculateJobAction(jobId);
                    }

                    // Insert into PollingIntegTransactionDetails / PollingIntegTransaction
                    server.InsertPollingIntegTransactionDetails(trancode.ToString(), id, jobId);

                    // Insert into PollingJobIdPollingJourneyDetails
                    foreach (DataRow dr in jobData)
                    {
                        int idJourneyDetail = int.Parse(dr[15].ToString());
                        server.InsertPollingJobIdPollingJourneyDetails(id, idJourneyDetail);
                    }

                    jobDetail.PickupDate = pickupDateTime.ToString("dd/MM/yyyy");
                    jobDetail.PickupTime = pickupDateTime.ToString("HH:mm");
                    jobDetail.Booker = contact.Trim();
                    //sednding hard-coded value in the  reponse replacing brandid value a  for WorkOrder=1603
                    jobDetail.Company = "GS";
                    //end 1603
                    jobDetail.Site = customer.Trim();
                    jobDetail.JobID = jobId;
                    jobDetail.LastChanged = entered < enteredJourneyDetails ? entered.ToString("dd/MM/yyyy HH:mm:ss") : enteredJourneyDetails.ToString("dd/MM/yyyy HH:mm:ss");
                    jobDetail.Mobile = mobile.Trim();
                    jobDetail.Passenger = passenger.Trim();
                    jobDetail.Notes = notes.Trim();
                    jobDetail.JourneyMileage = journeymileage.Trim();
                    jobDetail.BookerContact = "";
                    jobDetail.TravelID = "";
                    jobDetail.TravelNotes = "";

                    switch (serviceType)
                    {
                        case "Executive":
                            jobDetail.Service = "EXE";
                            break;
                        case "MPV":
                            jobDetail.Service = "MPV";
                            break;
                        case "Standard Car":
                            jobDetail.Service = "STD";
                            break;
                        default:
                            jobDetail.Service = serviceType.Substring(0,3);
                            break;
                    }

                    jobDetail.AllDayBooking = "N";
                    jobDetail.WaitAndReturn = "N";
                    jobDetail.PaymentType = 1;

                    switch (journeyType)
                    {
                        case "As Directed":
                            jobDetail.AllDayBooking = "Y";
                            break;
                        case "Wait & Return":
                            jobDetail.WaitAndReturn = "Y";
                            break;
                        default:
                            break;
                    }

                   

                    jobDetail.ChangeNotes = GetChangeNotes(jobId);

                    int lastStop = 0;

                    // Find Number of Stops
                    for (int i = jobData.Length - 1; i > 0; i--)
                    {
                        string Building = jobData[i]["Building"].ToString();
                        string Street = jobData[i]["Street"].ToString();
                        string Town = jobData[i]["Town"].ToString();
                        string County = jobData[i]["County"].ToString();
                        string Postcode = jobData[i]["Postcode"].ToString();

                        if (Building != "" || Street != "" || Town != "" ||
                            County != "" || Postcode != "")
                        {
                            lastStop = i;
                            break;
                        }
                    }

                    AirportAndFlight airportAndFlight = new AirportAndFlight();

                    string building = jobData[0]["Building"].ToString();
                    string street = jobData[0]["Street"].ToString();
                    string town = jobData[0]["Town"].ToString();
                    string county = jobData[0]["County"].ToString();
                    string postcode = jobData[0]["Postcode"].ToString();

                    // Pickup
                    if (CheckForFlight(building, street, town, county, postcode, ref airportAndFlight))
                    {
                        jobDetail.PickupAdd1 = airportAndFlight.Address1;
                        jobDetail.PickupAdd2 = airportAndFlight.Address2;
                        jobDetail.PickupTown = airportAndFlight.Town;
                        jobDetail.PickupCounty = airportAndFlight.County;
                        jobDetail.PickupPostCode = airportAndFlight.Postcode;
                        jobDetail.TravelID = airportAndFlight.FlightNumber;
                    }
                    else
                    {
                        jobDetail.PickupAdd1 = building;
                        jobDetail.PickupAdd2 = street;
                        jobDetail.PickupTown = town;
                        jobDetail.PickupCounty = county;
                        jobDetail.PickupPostCode = postcode;
                    }

                    // For As Directed done sent As Direct Journey Details Line
                    if (journeyType == "As Directed")
                        return;

                    // Via Addresses
                    for (int i = 1; i < lastStop; i++)
                    {
                        ResponseAvailableJobs.JobDetail.ViaDetail viaDetails = new ResponseAvailableJobs.JobDetail.ViaDetail();

                        building = jobData[i]["Building"].ToString();
                        street = jobData[i]["Street"].ToString();
                        town = jobData[i]["Town"].ToString();
                        county = jobData[i]["County"].ToString();
                        postcode = jobData[i]["Postcode"].ToString();

                        if (CheckForFlight(building, street, town, county, postcode, ref airportAndFlight))
                        {
                            viaDetails.ViaAdd1 = airportAndFlight.Address1;
                            viaDetails.ViaAdd2 = airportAndFlight.Address2;
                            viaDetails.ViaTown = airportAndFlight.Town;
                            viaDetails.ViaCounty = airportAndFlight.County;
                            viaDetails.ViaPostCode = airportAndFlight.Postcode;
                            jobDetail.TravelID = airportAndFlight.FlightNumber;
                        }
                        else
                        {
                            viaDetails.ViaAdd1 = building;
                            viaDetails.ViaAdd2 = street;
                            viaDetails.ViaTown = town;
                            viaDetails.ViaCounty = county;
                            viaDetails.ViaPostCode = postcode;
                        }

                        if (jobDetail.ViaDetailArray == null)
                            jobDetail.ViaDetailArray = new List<ResponseAvailableJobs.JobDetail.ViaDetail>();

                        jobDetail.ViaDetailArray.Add(viaDetails);
                    }

                    // Drop-off

                    building = jobData[lastStop]["Building"].ToString();
                    street = jobData[lastStop]["Street"].ToString();
                    town = jobData[lastStop]["Town"].ToString();
                    county = jobData[lastStop]["County"].ToString();
                    postcode = jobData[lastStop]["Postcode"].ToString();

                    if (CheckForFlight(building, street, town, county, postcode, ref airportAndFlight))
                    {
                        jobDetail.DropOffAdd1 = airportAndFlight.Address1;
                        jobDetail.DropOffAdd2 = airportAndFlight.Address2;
                        jobDetail.DropOffTown = airportAndFlight.Town;
                        jobDetail.DropOffCounty = airportAndFlight.County;
                        jobDetail.DropOffPostCode = airportAndFlight.Postcode;
                        jobDetail.TravelID = airportAndFlight.FlightNumber;
                    }
                    else
                    {
                        jobDetail.DropOffAdd1 = building;
                        jobDetail.DropOffAdd2 = street;
                        jobDetail.DropOffTown = town;
                        jobDetail.DropOffCounty = county;
                        jobDetail.DropOffPostCode = postcode;
                    }

                    // Add Job Detail into Response
                    if (response.JobDetailArray == null)
                        response.JobDetailArray = new List<ResponseAvailableJobs.JobDetail>();

                    response.JobDetailArray.Add(jobDetail);
                }

                // Update Transaction / Rows Number

                if (response.JobDetailArray != null)
                    response.RowsReturned = response.JobDetailArray.Count;

                // Save Messages in db for Confirmations
            }
            catch (Exception exc)
            {
                Log.LogToFile("ParseAvailableJobs:" + exc.Message + exc.StackTrace + exc.Source);
            }
            
        }

        private string CalculateJobAction(int jobId)
        {
            try
            {
                SQLServer server = new SQLServer();
                if (server.IsNewJob(jobId))
                    return "A";     // Assigned
                else
                    return "E";     // Modify
            }
            catch (Exception exc)
            {
                Log.LogToFile("CalculateJobAction:" + exc.Message + exc.StackTrace + exc.Source);
                return "X";
            }
        }

        private bool CheckForFlight(string Building,string Street,string Town,string County,string Postcode, ref AirportAndFlight AirportAndFlight)
        {
            return false;
        }


        //sending changed job history for last job id in the  response for WorkOrder=1603.
        // earlier hard-coded value "No Changes" was sending in the response.
        private string GetChangeNotes(int JobID)
        {
            try
            {
                int JOBIDRet = JobID;
                SQLServer server = new SQLServer();
                DataSet ds = new DataSet();
                String ChangeJobHist = "";
                ds = server.GetchangedPollingIntegJobs_Hist(JobID);
                if (ds.Tables[0].Rows.Count > 1)
                {
                    if ((ds.Tables[0].Rows[0]["JourneyType"].ToString()) != (ds.Tables[0].Rows[1]["JourneyType"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldJourneyType:" + ds.Tables[0].Rows[1]["JourneyType"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewJourneyType:" + ds.Tables[0].Rows[0]["JourneyType"].ToString();
                    }
                    if ((ds.Tables[0].Rows[0]["ServiceType"].ToString()) != (ds.Tables[0].Rows[1]["ServiceType"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldServiceType:" + ds.Tables[0].Rows[1]["ServiceType"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewServiceType:" + ds.Tables[0].Rows[0]["ServiceType"].ToString();
                    }


                    if ((ds.Tables[0].Rows[0]["Mobile"].ToString()) != (ds.Tables[0].Rows[1]["Mobile"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldMobile:" + ds.Tables[0].Rows[1]["Mobile"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewMobile:" + ds.Tables[0].Rows[0]["Mobile"].ToString();
                    }
                    if ((ds.Tables[0].Rows[0]["Passenger"].ToString()) != (ds.Tables[0].Rows[1]["Passenger"].ToString()))
                    {
                        ChangeJobHist = ChangeJobHist + "\r\nOldPassengername:" + ds.Tables[0].Rows[1]["Passenger"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewPassengername:" + ds.Tables[0].Rows[0]["Passenger"].ToString();
                    }

                    if ((ds.Tables[0].Rows[0]["PickupDateTime"].ToString()) != (ds.Tables[0].Rows[1]["PickupDateTime"].ToString()))
                    {
                        ChangeJobHist = "\r\nOldPickUpTime:" + ds.Tables[0].Rows[1]["PickupDateTime"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewPickUpTime:" + ds.Tables[0].Rows[0]["PickupDateTime"].ToString();
                    }
                    if ((ds.Tables[0].Rows[0]["notes"].ToString()) != (ds.Tables[0].Rows[1]["notes"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldnotes:" + ds.Tables[0].Rows[1]["notes"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewnotes:" + ds.Tables[0].Rows[0]["notes"].ToString();
                    }

                    if ((ds.Tables[0].Rows[0]["DROP1"].ToString()) != (ds.Tables[0].Rows[1]["DROP1"]).ToString())
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldDROP1:" + ds.Tables[0].Rows[0]["DROP1"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewDROP1:" + ds.Tables[0].Rows[1]["DROP1"].ToString();
                    }
                    if ((ds.Tables[0].Rows[0]["DROP2"].ToString()) != (ds.Tables[0].Rows[1]["DROP2"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldDROP2:" + ds.Tables[0].Rows[1]["DROP2"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewDROP2:" + ds.Tables[0].Rows[0]["DROP2"].ToString();
                    }

                    if ((ds.Tables[0].Rows[0]["DROP3"].ToString()) != (ds.Tables[0].Rows[1]["DROP3"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldDROP3:" + ds.Tables[0].Rows[1]["DROP3"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewDROP3:" + ds.Tables[0].Rows[0]["DROP3"].ToString();
                    }

                    if ((ds.Tables[0].Rows[0]["DROP4"].ToString()) != (ds.Tables[0].Rows[1]["DROP4"].ToString()))
                    {

                        ChangeJobHist = ChangeJobHist + "\r\nOldDROP4:" + ds.Tables[0].Rows[1]["DROP4"].ToString();
                        ChangeJobHist = ChangeJobHist + "\r\nNewDROP4:" + ds.Tables[0].Rows[0]["DROP4"].ToString();
                    }
                }
                else
                {
                    ChangeJobHist = "No Changes";
                }
                return ChangeJobHist;
            }
            catch (Exception exc)
            {
                Log.LogToFile("GetChangeNotes:" + exc.Message + exc.StackTrace + exc.Source);
                return "";
            }
        }
        //end 1603
    }

    
}

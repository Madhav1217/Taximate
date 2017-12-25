using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Common
{
	/// <summary>
	/// Summary description for BTRRequest.
	/// </summary>
	[Serializable]
	[XmlRoot("GSRequest")]
	public class BTRRequest
	{
		[XmlElement("UserID")]
		public string UserID;
		[XmlElement("Password")]
		public string Password;
		[XmlElement("Supplier")]
		public int Supplier;
		[XmlElement("Request")]
		public string Request;
		[XmlElement("DateStamp")]
		public string DateStamp;
		[XmlElement("TransCode")]
		public string TransCode;
		[XmlElement("JobID")]
		public string JobID;
		[XmlElement("Action")]
		public string Action;
		[XmlElement("OriginalQuote")]
		public string OriginalQuote;
		[XmlElement("ExtWaiting")]
		public string ExtWaiting;
		[XmlElement("ExtParking")]
		public string ExtParking;
		[XmlElement("ExtTolls")]
		public string ExtTolls;
		[XmlElement("ExtAmendment")]
		public string ExtAmendment;
		[XmlElement("ExtPhone")]
		public string ExtPhone;
		[XmlElement("ExtCancellation")]
		public string ExtCancellation;
		[XmlElement("ExtReason")]
		public string ExtReason;
		[XmlElement("TotalHours")]
		public string TotalHours;
		[XmlElement("BatchedJobID")]
		public string BatchedJobID;
		[XmlElement("LinkedJobID")]
		public string LinkedJobID;
		[XmlArray("RequestList")]
		[XmlArrayItem(Type=typeof(JobDetail))]
		public ArrayList JobDetailArray;

		public BTRRequest()
		{}

		public void AddJobDetail(JobDetail jobDetail)
		{
			if(JobDetailArray == null)
				JobDetailArray = new ArrayList();
			JobDetailArray.Add(jobDetail);
			return;
		}

		[Serializable]
		public class JobDetail
		{
			[XmlElement("JobID")]
			public int Jobid;
			[XmlElement("Action")]
			public string Action;
			[XmlElement("ErrorCode")]
			public int ErrorCode;
			[XmlElement("ErrorDesc")]
			public string ErrorDesc;

			public JobDetail()
			{

			}
		}

		public string serialise()
		{
			string SerializedObj = "";
			try
			{
				MemoryStream memStream = new MemoryStream();
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
				x.Serialize(memStream, this);
				SerializedObj = Encoding.ASCII.GetString(memStream.ToArray());
				SerializedObj = Regex.Replace(SerializedObj,"xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"","");
			}
			catch(Exception exp)
			{
				int i = 1;
			}
			return SerializedObj;
		}

        public static BTRRequest Deserialize(string xmlBTRResponse)
        {
            xmlBTRResponse = xmlBTRResponse.Replace("[PR&D]", "[PR&amp;D]");

            //xmlBTRResponse = "<?xml version=\"1.0\">" + xmlBTRResponse;
            XmlSerializer xs = new XmlSerializer(typeof(BTRRequest));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlBTRResponse));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            BTRRequest resultObj = null;
            try
            {
                resultObj = (BTRRequest)xs.Deserialize(memoryStream);
            }
            catch (Exception exc)
            {
                throw new Exception("Exception while trying to Deserialize Request: " + exc.Message, exc);
            }
            return resultObj;
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
	}
}

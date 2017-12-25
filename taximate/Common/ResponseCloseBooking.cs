using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// BTRResponse class used to hold information from BTR Response Xml
	///  in an easily accessable object
	///  
	///  Generate object using the static BTRResponse.Deserialize(xml) method
	/// </summary>
	[XmlRoot("GSResponse")]
	public class ResponseCloseBooking : BTRResponse
	{
		[XmlElement("JobID")]
		public int JobID;
		[XmlElement("Action")]
		public string Action;

        public ResponseCloseBooking()
        {

        }
	}
}
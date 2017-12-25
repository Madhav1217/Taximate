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
	public class ResponseAvailableJobs : BTRResponse
	{
		[XmlElement("TransCode")]
		public string TransCode;
        [XmlElement("RowsReturned")]
        public int RowsReturned;
        [XmlArray("RequestList")]
        public List<JobDetail> JobDetailArray;

        public ResponseAvailableJobs()
        {
            
        }

		public class JobDetail
		{
			[XmlElement("JobID")]
			public int JobID;
			[XmlElement("Action")]
			public string Action;
			[XmlElement("PickupDate")]
			public string PickupDate;
			[XmlElement("PickupTime")]
			public string PickupTime;
			[XmlElement("Passenger")]
			public string Passenger;
			[XmlElement("Mobile")]
			public string Mobile;
			[XmlElement("Notes")]
			public string Notes;
            [XmlElement("JourneyMileage")]
            public string JourneyMileage;
			[XmlElement("Seats")]
			public int Seats;
			[XmlElement("Service")]
			public string Service;
			[XmlElement("PickupID")]
			public int PickupID;
			[XmlElement("PickupAdd1")]
			public string PickupAdd1;
			[XmlElement("PickupAdd2")]
			public string PickupAdd2;
			[XmlElement("PickupTown")]
			public string PickupTown;
			[XmlElement("PickupCounty")]
			public string PickupCounty;
			[XmlElement("PickupPostCode")]
			public string PickupPostCode;
			[XmlElement("DropOffID")]
			public int DropOffID;
			[XmlElement("DropOffAdd1")]
			public string DropOffAdd1;
			[XmlElement("DropOffAdd2")]
			public string DropOffAdd2;
			[XmlElement("DropOffTown")]
			public string DropOffTown;
			[XmlElement("DropOffCounty")]
			public string DropOffCounty;
			[XmlElement("DropOffPostCode")]
			public string DropOffPostCode;
			[XmlElement("Company")]
			public string Company;
			[XmlElement("Site")]
			public string Site;
			[XmlElement("Booker")]
			public string Booker;
			[XmlElement("BookerContact")]
			public string BookerContact;
			[XmlElement("TravelID")]
			public string TravelID;
			[XmlElement("TravelNotes")]
			public string TravelNotes;
			[XmlElement("LastChanged")]
			public string LastChanged;
			[XmlElement("ChangeNotes")]
			public string ChangeNotes;
			[XmlElement("AllDayBookin")]
			public string AllDayBooking;
			[XmlElement("WaitAndReturn")]
			public string WaitAndReturn;
			[XmlElement("ReturnJob")]
			public int ReturnJob;
			[XmlElement("OriginalQuote")]
			public string OriginalQuote;
			[XmlElement("PaymentType")]
			public int PaymentType;
			[XmlElement("ExtPaxDetail")]
			public ExtPaxDetail[] ExtPaxDetailArray;
			[XmlElement("ViaDetail")]
			public List<ViaDetail> ViaDetailArray;

			public JobDetail(){}

			public class ExtPaxDetail
			{
				[XmlElement("ExtraPassenger")]
				public string ExtraPassenger;
				[XmlElement("ExtraPaxMobile")]
				public string ExtraPaxMobile;
				[XmlElement("ExtraPaxNotes")]
				public string ExtraPaxNotes;

				public ExtPaxDetail(){}
			}

			public class ViaDetail
			{
				[XmlElement("ViaAddID")]
				public int ViaAddID;
				[XmlElement("ViaAdd1")]
				public string ViaAdd1;
				[XmlElement("ViaAdd2")]
				public string ViaAdd2;
				[XmlElement("ViaTown")]
				public string ViaTown;
				[XmlElement("ViaCounty")]
				public string ViaCounty;
				[XmlElement("ViaPostCode")]
				public string ViaPostCode;

				public ViaDetail(){}
			}
		}
	}
}
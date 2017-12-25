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
	public class BTRResponse
	{
		[XmlElement("supplier")]
		public int Supplier;
		[XmlElement("Request")]
		public string Request;
		[XmlElement("Result")]
		public string Result;
		[XmlElement("ErrorCode")]
		public int ErrorCode;
		[XmlElement("ErrorDesc")]
		public string ErrorDesc;

		public BTRResponse()
        {
            
        }

		private static Byte[] StringToUTF8ByteArray(String pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		}

		public string serialise()
		{
			MemoryStream memStream = new MemoryStream();
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
			x.Serialize(memStream, this);
			string SerializedObj = Encoding.ASCII.GetString(memStream.ToArray());
            SerializedObj = SerializedObj.Replace("\r\n      ", "");
            SerializedObj = SerializedObj.Replace("\r\n  ","");
            SerializedObj = SerializedObj.Replace(">  <", "><");
			return SerializedObj;
		}


	}
}
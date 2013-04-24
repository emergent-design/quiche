using System.Xml.Serialization;
namespace Quiche.Data
{
	public class Setting
	{
		[XmlAttribute]
		public string Id				{ get; set; }
		[XmlAttribute]
		public string Value			 	{ get; set; }
	}
}

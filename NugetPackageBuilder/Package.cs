using System.Reflection;
using System.Xml.Serialization;

namespace NugetBuilder
{
    [XmlRoot(ElementName = "package")]
    public class Package
    {
        public Package()
        {

        }
        public Package(Assembly assembly)
        {
            MetaData = new MetaData(assembly);
        }

        private MetaData _metaData;

        [XmlElement(ElementName = "metadata")]
        public MetaData MetaData
        {
            get { return _metaData; }
            set { _metaData = value; }
        }
    }
}
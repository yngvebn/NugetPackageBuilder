using System;
using System.Reflection;
using System.Xml.Serialization;

namespace NugetBuilder
{
    [XmlRoot(ElementName = "metadata")]
    public class MetaData
    {
        public MetaData()
        {
            RequireLicenseAcceptance = false;
        }
        public MetaData(Assembly asm)
            : base()
        {
            string asmProduct = Attr<AssemblyProductAttribute>(asm).Product;
            string asmTitle = Attr<AssemblyTitleAttribute>(asm).Title;
            string asmDesc = Attr<AssemblyDescriptionAttribute>(asm).Description;
            string asmCopy = Attr<AssemblyCopyrightAttribute>(asm).Copyright;
            string asmVersion = Attr<AssemblyFileVersionAttribute>(asm).Version;
            string asmCompany = Attr<AssemblyCompanyAttribute>(asm).Company;

            Id = asmProduct;
            Version = asmVersion;
            Description = String.IsNullOrEmpty(asmDesc) ? "No description" : asmDesc;
            Copyright = asmCopy;

            Authors = String.IsNullOrEmpty(asmCompany) ? "Unknown Company" : asmCompany;
            Owners = Authors;
            ReleaseNotes = string.Format("Release of {2} v{0} ({1})", Version, DateTime.Now, asmTitle);

        }

        private T Attr<T>(Assembly asm) where T : class
        {
            object[] attrs = asm.GetCustomAttributes(typeof(T), false);
            if (attrs.Length == 0) return default(T);
            return attrs[0] as T;
        }

        private string _id;

        [XmlElement(ElementName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _version;

        [XmlElement(ElementName = "version")]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private string _authors;

        [XmlElement(ElementName = "authors")]
        public string Authors
        {
            get { return _authors; }
            set { _authors = value; }
        }

        private string _owners;

        [XmlElement(ElementName = "owners")]
        public string Owners
        {
            get { return _owners; }
            set { _owners = value; }
        }

        private bool _requireLicenseAcceptance;

        [XmlElement(ElementName = "requireLicenseAcceptance")]
        public bool RequireLicenseAcceptance
        {
            get { return _requireLicenseAcceptance; }
            set { _requireLicenseAcceptance = value; }
        }

        private string _description;

        [XmlElement(ElementName = "description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _releaseNotes;

        [XmlElement(ElementName = "releaseNotes")]
        public string ReleaseNotes
        {
            get { return _releaseNotes; }
            set { _releaseNotes = value; }
        }

        private string _copyright;

        [XmlElement(ElementName = "copyright")]
        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }

        private string _tags;

        [XmlElement(ElementName = "tags")]
        public string Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }
}
// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Specialized;
using System.Xml;

using OnTheFlyCompiler.Errors;

namespace OnTheFlyCompiler.Settings
{
	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		string language, methodName, methodPath;
		int verbose = 2;
		bool execute;
		StringCollection sources;

		#region Constructors
		#endregion

		#region Properties
		public bool Execute
		{
			get
			{
				return this.execute;
			}
			set
			{
				this.execute = value;
			}
		}
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}

		public string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}


		public string MethodPath
		{
			get
			{
				return this.methodPath;
			}
			set
			{
				this.methodPath = value;
			}
		}

		public StringCollection Sources
		{
			get
			{
				return this.sources;
			}
			set
			{
				this.sources = value;
			}
		}

		public int Verbose
		{
			get
			{
				return this.verbose;
			}
			set
			{
				this.verbose = value;
			}
		}
		#endregion

		#region Methods
		public static CompilerSettings Parse(XmlDocument doc)
		{
			CompilerSettings settings = new CompilerSettings();

			XmlNode nodeSettings = doc.SelectSingleNode("//settings");
			XmlNode nodeCompiler = nodeSettings.SelectSingleNode("compiler");
			XmlAttribute atrLanguage = nodeCompiler.Attributes["language"];
			if (atrLanguage != null)
			{
				settings.Language = atrLanguage.Value;
			}
			else
			{
				throw new ReadingXmlDescriptionException(new XmlException("Required attribute 'Language' is missed"));
			}

			XmlAttribute atrWarning = nodeCompiler.Attributes["warning"];
			if (atrWarning != null)
			{
				settings.WarningLevel = Convert.ToInt32(atrWarning.Value);
			}

			XmlNode nodeMethod = nodeSettings.SelectSingleNode("method");
			XmlAttribute atrPath = nodeMethod.Attributes["path"];
			if (atrPath != null)
			{
				settings.MethodPath = atrPath.Value;
			}
			else
			{
				throw new ReadingXmlDescriptionException(new XmlException("Required attribute 'Path' is missed"));
			}
			XmlAttribute atrName = nodeMethod.Attributes["name"];
			if (atrName != null)
			{
				settings.MethodName = atrName.Value;
			}
			else
			{
				throw new ReadingXmlDescriptionException(new XmlException("Required attribute 'Name' is missed"));
			}

			foreach (XmlNode nodeRef in nodeSettings.SelectNodes("references/item"))
			{
				settings.ReferencedAssemblies.Add(nodeRef.InnerText);
			}

			foreach (XmlNode nodeFile in nodeSettings.SelectNodes("files/item"))
			{
				settings.Sources.Add(System.IO.File.ReadAllText(nodeFile.InnerText));
			}

			return settings;
		}
		#endregion
	}
}

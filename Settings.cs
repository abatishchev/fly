// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

using OnTheFly.Errors;

namespace OnTheFly.Settings
{
	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		Dictionary<string, object> container = new Dictionary<string, object>();
		StringCollection sources = new StringCollection();

		string language, methodPath, methodName;

		int verbose = 2;

		#region Constructor
		public CompilerSettings()
		{
			//
		}

		public CompilerSettings(Dictionary<string, object> container)
		{
			this.container = container;
		}
		#endregion

		#region Properties
		public Dictionary<string, object> SettingsContainer
		{
			get
			{
				return this.container;
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
				this.container["language"] = value;
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
				this.container["map"] = value;
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
				this.container["name"] = value;
			}
		}

		public StringCollection Sources
		{
			get
			{
				return this.sources;
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
				this.container["verbose"] = value;
			}
		}
		#endregion

		#region Mehods
		public void Check()
		{
			foreach (string name in container.Keys)
			{
				if (container[name] == null)
				{
					throw new ParameterMissedException(name);
				}
			}
		}

		public static CompilerSettings Parse(XmlDocument doc)
		{
			CompilerSettings settings = new CompilerSettings();

			XmlNode nodeSettings = doc.SelectSingleNode("//settings");
			XmlNode nodeCompiler = nodeSettings.SelectSingleNode("compiler");
			XmlAttribute atrLanguage = nodeCompiler.Attributes["language"];
			if (atrLanguage != null)
			{
				settings.language = atrLanguage.Value;
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
				settings.methodPath = atrPath.Value;
			}
			else
			{
				throw new ReadingXmlDescriptionException(new XmlException("Required attribute 'Path' is missed"));
			}
			XmlAttribute atrName = nodeMethod.Attributes["name"];
			if (atrName != null)
			{
				settings.methodName = atrName.Value;
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
				settings.sources.Add(File.ReadAllText(nodeFile.InnerText));
			}

			return settings;
		}
		#endregion
	}
}

// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Xml;

using OnTheFlyCompiler.Errors;

namespace OnTheFlyCompiler.Settings
{
	public enum ParameterRole
	{
		ExecuteFlag,
		Language,
		MethodName,
		MethodPath,
		Sources,
		VerboseLevel,
		WarningLevel
	}

	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		ParameterContainer container;

		#region Constructors
		public CompilerSettings()
		{
			//
		}

		public CompilerSettings(ParameterContainer container)
		{
			this.container = container;
		}
		#endregion

		#region Properties
		public bool Execute
		{
			get
			{
				object value = this.container[ParameterRole.ExecuteFlag];
				return (value != null) ? (bool)value : false;
			}
			set
			{
				this.container[ParameterRole.ExecuteFlag] = value;
			}
		}
		public string Language
		{
			get
			{
				return container[ParameterRole.Language] as string;
			}
			set
			{
				container[ParameterRole.Language] = value;
			}
		}

		public string MethodName
		{
			get
			{
				return container[ParameterRole.MethodName] as string;
			}
			set
			{
				container[ParameterRole.MethodName] = value;
			}
		}


		public string MethodPath
		{
			get
			{
				return container[ParameterRole.MethodPath] as string;
			}
			set
			{
				container[ParameterRole.MethodPath] = value;
			}
		}

		public StringCollection Sources
		{
			get
			{
				return container[ParameterRole.Sources] as StringCollection;
			}
			set
			{
				container[ParameterRole.Sources] = value;
			}
		}

		public int Verbose
		{
			get
			{
				object value = this.container[ParameterRole.VerboseLevel];
				return (value != null) ? (int)value : 2;
			}
			set
			{
				container[ParameterRole.VerboseLevel] = value;
			}
		}
		#endregion

		#region Methods
		public static CompilerSettings Parse(string[] args)
		{
			CompilerSettings settings = new CompilerSettings();
			for (int i = 0; i < args.Length; i++)
			{
				string name = args[i];
				try
				{
					switch (name)
					{
						case "--debug":
							{
								settings.IncludeDebugInformation = true;
								break;
							}
						case "--exe":
							{
								settings.GenerateExecutable = true;
								break;
							}
						case "--exec":
							{
								settings.Execute = true;
								break;
							}
						case "-f":
							{
								string[] arr = args[++i].Split(';');
								foreach (string str in arr)
								{
									settings.Sources.Add(File.ReadAllText(str));
								}
								break;
							}
						case "-l":
							{
								settings.Language = args[++i];
								break;
							}
						case "--memory":
							{
								settings.GenerateInMemory = true;
								break;
							}
						case "-n":
							{
								settings.MethodName = args[++i];
								break;
							}
						case "-p":
							{
								settings.MethodPath = args[++i];
								break;
							}
						case "-r":
							{
								string[] arr = args[++i].Split(';');
								settings.ReferencedAssemblies.AddRange(arr);
								break;
							}
						case "-s":
							{
								string[] arr = args[++i].Split(';');
								settings.Sources.AddRange(arr);
								break;
							}
						case "--threat":
							{
								settings.TreatWarningsAsErrors = true;
								break;
							}
						case "-v":
							{
								int verbose = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
								if (verbose > 2)
								{
									throw new ParameterOutOfRangeException(name, verbose.ToString(CultureInfo.CurrentCulture));
								}
								else
								{
									settings.Verbose = verbose;
								}
								break;
							}
						case "-w":
							{
								try
								{
									int warning = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
									if (warning > 4)
									{
										throw new ParameterOutOfRangeException(name, warning.ToString(CultureInfo.CurrentCulture));
									}
									else
									{
										settings.WarningLevel = warning;
									}
								}
								catch
								{
									throw new ParameterNotSetException(name);
								}
								break;
							}
						case "-x":
							{
								try
								{
									string xml = String.Empty;
									XmlDocument doc = new XmlDocument();
									try
									{
										xml = args[++i];
										doc.Load(xml);
										settings = CompilerSettings.Parse(doc);
									}
									catch
									{
										throw new ParameterOutOfRangeException(name, xml);
									}
								}
								catch (NullReferenceException ex)
								{
									throw new ReadingXmlDescriptionException(ex);
								}

								break;
							}
						default:
							{
								throw new UnknownParameterException(name);
							}
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new ParameterNotSetException(name);
				}
			}
			return settings;
		}

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
				settings.Sources.Add(File.ReadAllText(nodeFile.InnerText));
			}

			return settings;
		}
		#endregion
	}

	public class ParameterContainer
	{
		Dictionary<ParameterRole, object> dic = new Dictionary<ParameterRole, object>();

		#region Operators
		public object this[ParameterRole role]
		{
			get
			{
				if (dic.ContainsKey(role))
				{
					return dic[role];
				}
				else
				{
					return null;
				}
			}
			set
			{
				if (dic.ContainsKey(role))
				{
					dic[role] = value;
				}
				else
				{
					dic.Add(role, value);
				}
			}
		}
		#endregion
	}
}

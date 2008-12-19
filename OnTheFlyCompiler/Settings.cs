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
		public static CompilerSettings Parse(string[] args)
		{
			CompilerSettings settings = new CompilerSettings();
			for (int i = 0; i < args.Length; i++)
			{
				string name = args[i];
				object value = null;

				if (settings.container.ContainsKey(name))
				{
					throw new ParameterAlreadySetException(name);
				}

				try
				{
					switch (name)
					{
						case "--debug":
							{
								value = true;
								settings.IncludeDebugInformation = true;
								break;
							}
						case "--exe":
							{
								value = true;
								settings.GenerateExecutable = true;
								break;
							}
						case "-f":
							{
								string[] arr = args[++i].Split(';');
								foreach (string str in arr)
								{
									settings.sources.Add(File.ReadAllText(str));
								}
								value = arr;
								break;
							}
						case "-l":
							{
								string str = args[++i];
								settings.language = str;
								value = str;
								break;
							}
						case "--memory":
							{
								value = true;
								settings.GenerateInMemory = true;
								break;
							}
						case "-n":
							{
								string str = args[++i];
								value = str;
								settings.methodName = str;
								break;
							}
						case "-p":
							{
								settings.methodPath = args[++i];
								break;
							}
						case "-r":
							{
								string[] arr = args[++i].Split(';');
								value = arr;
								settings.ReferencedAssemblies.AddRange(arr);
								break;
							}
						case "-s":
							{
								string[] arr = args[++i].Split(';');
								settings.sources.AddRange(arr);
								value = arr;
								break;
							}
						case "--threat":
							{
								value = true;
								settings.TreatWarningsAsErrors = true;
								break;
							}
						case "-v":
							{
								int level = Convert.ToInt32(args[++i]);
								if (level > 2)
								{
									throw new ParameterOutOfRangeException(name, level.ToString());
								}
								else
								{
									settings.verbose = level;
									value = level;
								}
								break;
							}
						case "-w":
							{
								try
								{
									int level = Convert.ToInt32(args[++i]);
									if (level > 4)
									{
										throw new ParameterOutOfRangeException(name, level.ToString());
									}
									else
									{
										settings.WarningLevel = level;
										value = level;
									}
								}
								catch
								{
									throw new ParameterOutOfRangeException(name, (string)value);
								}
								break;
							}
						case "-x":
							{
								try
								{
									string xml = String.Empty; ;
									XmlDocument doc = new XmlDocument();
									try
									{
										xml = args[++i];
										doc.Load(xml);
										settings = Parse(doc);
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
					if (value != null)
					{
						settings.container.Add(name, value);
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

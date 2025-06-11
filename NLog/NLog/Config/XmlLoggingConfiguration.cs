// Decompiled with JetBrains decompiler
// Type: NLog.Config.XmlLoggingConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace NLog.Config;

public class XmlLoggingConfiguration : LoggingConfigurationParser
{
  private readonly Dictionary<string, bool> _fileMustAutoReloadLookup = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private string _originalFileName;
  private readonly Stack<string> _currentFilePath = new Stack<string>();

  public XmlLoggingConfiguration([NotNull] string fileName)
    : this(fileName, LogManager.LogFactory)
  {
  }

  public XmlLoggingConfiguration([NotNull] string fileName, LogFactory logFactory)
    : base(logFactory)
  {
    using (XmlReader fileReader = this.CreateFileReader(fileName))
      this.Initialize(fileReader, fileName);
  }

  [Obsolete("Constructor with parameter ignoreErrors has limited effect. Instead use LogManager.ThrowConfigExceptions. Marked obsolete in NLog 4.7")]
  public XmlLoggingConfiguration([NotNull] string fileName, bool ignoreErrors)
    : this(fileName, ignoreErrors, LogManager.LogFactory)
  {
  }

  [Obsolete("Constructor with parameter ignoreErrors has limited effect. Instead use LogManager.ThrowConfigExceptions. Marked obsolete in NLog 4.7")]
  public XmlLoggingConfiguration([NotNull] string fileName, bool ignoreErrors, LogFactory logFactory)
    : base(logFactory)
  {
    using (XmlReader fileReader = this.CreateFileReader(fileName))
      this.Initialize(fileReader, fileName, ignoreErrors);
  }

  public XmlLoggingConfiguration([NotNull] XmlReader reader)
    : this(reader, (string) null)
  {
  }

  public XmlLoggingConfiguration([NotNull] XmlReader reader, [CanBeNull] string fileName)
    : this(reader, fileName, LogManager.LogFactory)
  {
  }

  public XmlLoggingConfiguration([NotNull] XmlReader reader, [CanBeNull] string fileName, LogFactory logFactory)
    : base(logFactory)
  {
    this.Initialize(reader, fileName);
  }

  [Obsolete("Constructor with parameter ignoreErrors has limited effect. Instead use LogManager.ThrowConfigExceptions. Marked obsolete in NLog 4.7")]
  public XmlLoggingConfiguration([NotNull] XmlReader reader, [CanBeNull] string fileName, bool ignoreErrors)
    : this(reader, fileName, ignoreErrors, LogManager.LogFactory)
  {
  }

  [Obsolete("Constructor with parameter ignoreErrors has limited effect. Instead use LogManager.ThrowConfigExceptions. Marked obsolete in NLog 4.7")]
  public XmlLoggingConfiguration(
    [NotNull] XmlReader reader,
    [CanBeNull] string fileName,
    bool ignoreErrors,
    LogFactory logFactory)
    : base(logFactory)
  {
    this.Initialize(reader, fileName, ignoreErrors);
  }

  internal XmlLoggingConfiguration([NotNull] string xmlContents, [CanBeNull] string fileName, LogFactory logFactory)
    : base(logFactory)
  {
    using (StringReader input = new StringReader(xmlContents))
    {
      using (XmlReader reader = XmlReader.Create((TextReader) input))
        this.Initialize(reader, fileName);
    }
  }

  public static XmlLoggingConfiguration CreateFromXmlString(string xml)
  {
    return XmlLoggingConfiguration.CreateFromXmlString(xml, LogManager.LogFactory);
  }

  public static XmlLoggingConfiguration CreateFromXmlString(string xml, LogFactory logFactory)
  {
    return new XmlLoggingConfiguration(xml, string.Empty, logFactory);
  }

  public static LoggingConfiguration AppConfig
  {
    get => System.Configuration.ConfigurationManager.GetSection("nlog") as LoggingConfiguration;
  }

  public bool? InitializeSucceeded { get; private set; }

  public bool AutoReload
  {
    get
    {
      return this._fileMustAutoReloadLookup.Count != 0 && this._fileMustAutoReloadLookup.Values.All<bool>((Func<bool, bool>) (mustAutoReload => mustAutoReload));
    }
    set
    {
      foreach (string key in this._fileMustAutoReloadLookup.Keys.ToList<string>())
        this._fileMustAutoReloadLookup[key] = value;
    }
  }

  public override IEnumerable<string> FileNamesToWatch
  {
    get
    {
      return this._fileMustAutoReloadLookup.Where<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>) (entry => entry.Value)).Select<KeyValuePair<string, bool>, string>((Func<KeyValuePair<string, bool>, string>) (entry => entry.Key));
    }
  }

  public override LoggingConfiguration Reload()
  {
    return !string.IsNullOrEmpty(this._originalFileName) ? (LoggingConfiguration) new XmlLoggingConfiguration(this._originalFileName, this.LogFactory) : base.Reload();
  }

  public static IEnumerable<string> GetCandidateConfigFilePaths()
  {
    return LogManager.LogFactory.GetCandidateConfigFilePaths();
  }

  public static void SetCandidateConfigFilePaths(IEnumerable<string> filePaths)
  {
    LogManager.LogFactory.SetCandidateConfigFilePaths(filePaths);
  }

  public static void ResetCandidateConfigFilePath()
  {
    LogManager.LogFactory.ResetCandidateConfigFilePath();
  }

  private XmlReader CreateFileReader(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return (XmlReader) null;
    fileName = fileName.Trim();
    return this.LogFactory.CurrentAppEnvironment.LoadXmlFile(fileName);
  }

  private void Initialize([NotNull] XmlReader reader, [CanBeNull] string fileName, bool ignoreErrors = false)
  {
    try
    {
      this.InitializeSucceeded = new bool?();
      this._originalFileName = fileName;
      int content = (int) reader.MoveToContent();
      NLogXmlElement nlogXmlElement = new NLogXmlElement(reader);
      if (!string.IsNullOrEmpty(fileName))
      {
        InternalLogger.Info<string>("Configuring from an XML element in {0}...", fileName);
        this.ParseTopLevel(nlogXmlElement, fileName, false);
      }
      else
        this.ParseTopLevel(nlogXmlElement, (string) null, false);
      this.InitializeSucceeded = new bool?(true);
      this.CheckParsingErrors(nlogXmlElement);
    }
    catch (Exception ex)
    {
      this.InitializeSucceeded = new bool?(false);
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException exception = new NLogConfigurationException(ex, "Exception when parsing {0}. ", new object[1]
      {
        (object) fileName
      });
      InternalLogger.Error(ex, exception.Message);
      if (!ignoreErrors && ((int) this.LogFactory.ThrowConfigExceptions ?? (this.LogFactory.ThrowExceptions ? 1 : (exception.MustBeRethrown() ? 1 : 0))) != 0)
        throw exception;
    }
  }

  private void CheckParsingErrors(NLogXmlElement rootContentElement)
  {
    string[] array = rootContentElement.GetParsingErrors().ToArray<string>();
    if (!((IEnumerable<string>) array).Any<string>())
      return;
    if (((int) LogManager.ThrowConfigExceptions ?? (LogManager.ThrowExceptions ? 1 : 0)) != 0)
      throw new NLogConfigurationException(string.Join(Environment.NewLine, array));
    foreach (string message in array)
      InternalLogger.Log(NLog.LogLevel.Warn, message);
  }

  private void ConfigureFromFile([NotNull] string fileName, bool autoReloadDefault)
  {
    if (this._fileMustAutoReloadLookup.ContainsKey(XmlLoggingConfiguration.GetFileLookupKey(fileName)))
      return;
    using (XmlReader reader = this.LogFactory.CurrentAppEnvironment.LoadXmlFile(fileName))
    {
      int content = (int) reader.MoveToContent();
      this.ParseTopLevel(new NLogXmlElement(reader, true), fileName, autoReloadDefault);
    }
  }

  private void ParseTopLevel(NLogXmlElement content, [CanBeNull] string filePath, bool autoReloadDefault)
  {
    content.AssertName("nlog", "configuration");
    switch (content.LocalName.ToUpperInvariant())
    {
      case "CONFIGURATION":
        this.ParseConfigurationElement(content, filePath, autoReloadDefault);
        break;
      case "NLOG":
        this.ParseNLogElement((ILoggingConfigurationElement) content, filePath, autoReloadDefault);
        break;
    }
  }

  private void ParseConfigurationElement(
    NLogXmlElement configurationElement,
    [CanBeNull] string filePath,
    bool autoReloadDefault)
  {
    InternalLogger.Trace(nameof (ParseConfigurationElement));
    configurationElement.AssertName("configuration");
    foreach (ILoggingConfigurationElement nlogElement in configurationElement.Elements("nlog").ToList<NLogXmlElement>())
      this.ParseNLogElement(nlogElement, filePath, autoReloadDefault);
  }

  private void ParseNLogElement(
    ILoggingConfigurationElement nlogElement,
    [CanBeNull] string filePath,
    bool autoReloadDefault)
  {
    InternalLogger.Trace(nameof (ParseNLogElement));
    nlogElement.AssertName("nlog");
    bool optionalBooleanValue = nlogElement.GetOptionalBooleanValue("autoReload", autoReloadDefault);
    if (!string.IsNullOrEmpty(filePath))
      this._fileMustAutoReloadLookup[XmlLoggingConfiguration.GetFileLookupKey(filePath)] = optionalBooleanValue;
    try
    {
      this._currentFilePath.Push(filePath);
      this.LoadConfig(nlogElement, Path.GetDirectoryName(filePath));
    }
    finally
    {
      this._currentFilePath.Pop();
    }
  }

  protected override bool ParseNLogSection(ILoggingConfigurationElement configSection)
  {
    if (!configSection.MatchesName("include"))
      return base.ParseNLogSection(configSection);
    string str = this._currentFilePath.Peek();
    bool autoReloadDefault = !string.IsNullOrEmpty(str) && this._fileMustAutoReloadLookup[XmlLoggingConfiguration.GetFileLookupKey(str)];
    this.ParseIncludeElement(configSection, !string.IsNullOrEmpty(str) ? Path.GetDirectoryName(str) : (string) null, autoReloadDefault);
    return true;
  }

  private void ParseIncludeElement(
    ILoggingConfigurationElement includeElement,
    string baseDirectory,
    bool autoReloadDefault)
  {
    includeElement.AssertName("include");
    string str1 = includeElement.GetRequiredValue("file", "nlog");
    bool optionalBooleanValue = includeElement.GetOptionalBooleanValue("ignoreErrors", false);
    try
    {
      str1 = this.ExpandSimpleVariables(str1);
      str1 = SimpleLayout.Evaluate(str1);
      string str2 = str1;
      if (baseDirectory != null)
        str2 = Path.Combine(baseDirectory, str1);
      if (File.Exists(str2))
      {
        InternalLogger.Debug<string>("Including file '{0}'", str2);
        this.ConfigureFromFile(str2, autoReloadDefault);
      }
      else if (str1.Contains("*"))
      {
        this.ConfigureFromFilesByMask(baseDirectory, str1, autoReloadDefault);
      }
      else
      {
        if (!optionalBooleanValue)
          throw new FileNotFoundException("Included file not found: " + str2);
        InternalLogger.Debug<string>("Skipping included file '{0}' as it can't be found", str2);
      }
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException configurationException = new NLogConfigurationException(ex, "Error when including '{0}'.", new object[1]
      {
        (object) str1
      });
      InternalLogger.Error(ex, configurationException.Message);
      if (!optionalBooleanValue)
        throw configurationException;
    }
  }

  private void ConfigureFromFilesByMask(
    string baseDirectory,
    string fileMask,
    bool autoReloadDefault)
  {
    string path = baseDirectory;
    if (Path.IsPathRooted(fileMask))
    {
      path = Path.GetDirectoryName(fileMask);
      if (path == null)
      {
        InternalLogger.Warn<string>("directory is empty for include of '{0}'", fileMask);
        return;
      }
      string fileName = Path.GetFileName(fileMask);
      if (fileName == null)
      {
        InternalLogger.Warn<string>("filename is empty for include of '{0}'", fileMask);
        return;
      }
      fileMask = fileName;
    }
    foreach (string file in Directory.GetFiles(path, fileMask))
      this.ConfigureFromFile(file, autoReloadDefault);
  }

  private static string GetFileLookupKey([NotNull] string fileName) => Path.GetFullPath(fileName);

  public override string ToString() => $"{base.ToString()}, FilePath={this._originalFileName}";
}

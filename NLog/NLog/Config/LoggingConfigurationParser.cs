// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Filters;
using NLog.Internal;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using NLog.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NLog.Config;

public abstract class LoggingConfigurationParser(LogFactory logFactory) : LoggingConfiguration(logFactory)
{
  private ConfigurationItemFactory _configurationItemFactory;

  protected void LoadConfig(ILoggingConfigurationElement nlogConfig, string basePath)
  {
    InternalLogger.Trace("ParseNLogConfig");
    nlogConfig.AssertName("nlog");
    this.SetNLogElementSettings(nlogConfig);
    List<ILoggingConfigurationElement> list = nlogConfig.Children.ToList<ILoggingConfigurationElement>();
    foreach (ILoggingConfigurationElement extensionsElement in list.Where<ILoggingConfigurationElement>((Func<ILoggingConfigurationElement, bool>) (child => child.MatchesName("extensions"))).ToList<ILoggingConfigurationElement>())
      this.ParseExtensionsElement(extensionsElement, basePath);
    List<ILoggingConfigurationElement> configurationElementList = new List<ILoggingConfigurationElement>();
    foreach (ILoggingConfigurationElement configurationElement in list)
    {
      if (configurationElement.MatchesName("rules"))
        configurationElementList.Add(configurationElement);
      else if (!configurationElement.MatchesName("extensions") && !this.ParseNLogSection(configurationElement))
        InternalLogger.Warn<string>("Skipping unknown 'NLog' child node: {0}", configurationElement.Name);
    }
    foreach (ILoggingConfigurationElement rulesElement in configurationElementList)
      this.ParseRulesElement(rulesElement, this.LoggingRules);
  }

  private void SetNLogElementSettings(ILoggingConfigurationElement nlogConfig)
  {
    IList<KeyValuePair<string, string>> sortedListFromConfig = LoggingConfigurationParser.CreateUniqueSortedListFromConfig(nlogConfig);
    bool? nullable = new bool?();
    bool flag = false;
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) sortedListFromConfig)
    {
      switch (keyValuePair.Key.ToUpperInvariant())
      {
        case "AUTORELOAD":
          continue;
        case "AUTOSHUTDOWN":
          this.LogFactory.AutoShutdown = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, true);
          continue;
        case "EXCEPTIONLOGGINGOLDSTYLE":
          this.ExceptionLoggingOldStyle = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, this.ExceptionLoggingOldStyle);
          continue;
        case "GLOBALTHRESHOLD":
          this.LogFactory.GlobalThreshold = this.ParseLogLevelSafe(keyValuePair.Key, keyValuePair.Value, this.LogFactory.GlobalThreshold);
          continue;
        case "INTERNALLOGFILE":
          string internalLogFile = keyValuePair.Value?.Trim();
          if (!string.IsNullOrEmpty(internalLogFile))
          {
            InternalLogger.LogFile = this.ExpandFilePathVariables(internalLogFile);
            continue;
          }
          continue;
        case "INTERNALLOGINCLUDETIMESTAMP":
          InternalLogger.IncludeTimestamp = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, InternalLogger.IncludeTimestamp);
          continue;
        case "INTERNALLOGLEVEL":
          InternalLogger.LogLevel = this.ParseLogLevelSafe(keyValuePair.Key, keyValuePair.Value, InternalLogger.LogLevel);
          flag = InternalLogger.LogLevel != NLog.LogLevel.Off;
          continue;
        case "INTERNALLOGTOCONSOLE":
          InternalLogger.LogToConsole = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, InternalLogger.LogToConsole);
          continue;
        case "INTERNALLOGTOCONSOLEERROR":
          InternalLogger.LogToConsoleError = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, InternalLogger.LogToConsoleError);
          continue;
        case "INTERNALLOGTOTRACE":
          InternalLogger.LogToTrace = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, InternalLogger.LogToTrace);
          continue;
        case "KEEPVARIABLESONRELOAD":
          this.LogFactory.KeepVariablesOnReload = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, this.LogFactory.KeepVariablesOnReload);
          continue;
        case "PARSEMESSAGETEMPLATES":
          nullable = this.ParseNullableBooleanValue(keyValuePair.Key, keyValuePair.Value, true);
          continue;
        case "THROWCONFIGEXCEPTIONS":
          this.LogFactory.ThrowConfigExceptions = this.ParseNullableBooleanValue(keyValuePair.Key, keyValuePair.Value, false);
          continue;
        case "THROWEXCEPTIONS":
          this.LogFactory.ThrowExceptions = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, this.LogFactory.ThrowExceptions);
          continue;
        case "USEINVARIANTCULTURE":
          if (this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, false))
          {
            this.DefaultCultureInfo = CultureInfo.InvariantCulture;
            continue;
          }
          continue;
        default:
          InternalLogger.Debug<string, string>("Skipping unknown 'NLog' property {0}={1}", keyValuePair.Key, keyValuePair.Value);
          continue;
      }
    }
    if (!flag && !InternalLogger.HasActiveLoggers())
      InternalLogger.LogLevel = NLog.LogLevel.Off;
    this._configurationItemFactory = ConfigurationItemFactory.Default;
    this._configurationItemFactory.ParseMessageTemplates = nullable;
  }

  private static IList<KeyValuePair<string, string>> CreateUniqueSortedListFromConfig(
    ILoggingConfigurationElement nlogConfig)
  {
    Dictionary<string, string> dict = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    foreach (KeyValuePair<string, string> keyValuePair in nlogConfig.Values)
    {
      if (!string.IsNullOrEmpty(keyValuePair.Key))
      {
        string key = keyValuePair.Key.Trim();
        if (dict.ContainsKey(key))
          InternalLogger.Debug<string, string>("Skipping duplicate value for 'NLog' property {0}. Value={1}", keyValuePair.Key, dict[key]);
        dict[key] = keyValuePair.Value;
      }
    }
    List<KeyValuePair<string, string>> sortedList = new List<KeyValuePair<string, string>>(dict.Count);
    AddHighPrioritySetting("ThrowExceptions");
    AddHighPrioritySetting("ThrowConfigExceptions");
    AddHighPrioritySetting("InternalLogLevel");
    AddHighPrioritySetting("InternalLogFile");
    AddHighPrioritySetting("InternalLogToConsole");
    foreach (KeyValuePair<string, string> keyValuePair in dict)
      sortedList.Add(keyValuePair);
    return (IList<KeyValuePair<string, string>>) sortedList;

    void AddHighPrioritySetting(string settingName)
    {
      if (!dict.ContainsKey(settingName))
        return;
      sortedList.Add(new KeyValuePair<string, string>(settingName, dict[settingName]));
      dict.Remove(settingName);
    }
  }

  private string ExpandFilePathVariables(string internalLogFile)
  {
    try
    {
      string result1;
      char directorySeparatorChar;
      if (LoggingConfigurationParser.ContainsSubStringIgnoreCase(internalLogFile, "${currentdir}", out result1))
      {
        string str1 = internalLogFile;
        string oldValue = result1;
        string currentDirectory = Directory.GetCurrentDirectory();
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str2 = directorySeparatorChar.ToString();
        string newValue = currentDirectory + str2;
        internalLogFile = str1.Replace(oldValue, newValue);
      }
      string result2;
      if (LoggingConfigurationParser.ContainsSubStringIgnoreCase(internalLogFile, "${basedir}", out result2))
      {
        string str3 = internalLogFile;
        string oldValue = result2;
        string domainBaseDirectory = this.LogFactory.CurrentAppEnvironment.AppDomainBaseDirectory;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str4 = directorySeparatorChar.ToString();
        string newValue = domainBaseDirectory + str4;
        internalLogFile = str3.Replace(oldValue, newValue);
      }
      string result3;
      if (LoggingConfigurationParser.ContainsSubStringIgnoreCase(internalLogFile, "${tempdir}", out result3))
      {
        string str5 = internalLogFile;
        string oldValue = result3;
        string userTempFilePath = this.LogFactory.CurrentAppEnvironment.UserTempFilePath;
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str6 = directorySeparatorChar.ToString();
        string newValue = userTempFilePath + str6;
        internalLogFile = str5.Replace(oldValue, newValue);
      }
      string result4;
      if (LoggingConfigurationParser.ContainsSubStringIgnoreCase(internalLogFile, "${processdir}", out result4))
      {
        string str7 = internalLogFile;
        string oldValue = result4;
        string directoryName = Path.GetDirectoryName(this.LogFactory.CurrentAppEnvironment.CurrentProcessFilePath);
        directorySeparatorChar = Path.DirectorySeparatorChar;
        string str8 = directorySeparatorChar.ToString();
        string newValue = directoryName + str8;
        internalLogFile = str7.Replace(oldValue, newValue);
      }
      if (internalLogFile.IndexOf('%') >= 0)
        internalLogFile = Environment.ExpandEnvironmentVariables(internalLogFile);
      return internalLogFile;
    }
    catch
    {
      return internalLogFile;
    }
  }

  private static bool ContainsSubStringIgnoreCase(
    string haystack,
    string needle,
    out string result)
  {
    int startIndex = haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase);
    result = startIndex >= 0 ? haystack.Substring(startIndex, needle.Length) : (string) null;
    return result != null;
  }

  private NLog.LogLevel ParseLogLevelSafe(
    string attributeName,
    string attributeValue,
    NLog.LogLevel @default)
  {
    try
    {
      return NLog.LogLevel.FromString(attributeValue?.Trim());
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      object[] objArray = new object[3]
      {
        (object) attributeName,
        (object) attributeValue,
        (object) @default
      };
      NLogConfigurationException configException = new NLogConfigurationException(ex, "attribute '{0}': '{1}' isn't valid LogLevel. {2} will be used.", objArray);
      if (this.MustThrowConfigException(configException))
        throw configException;
      return @default;
    }
  }

  protected virtual bool ParseNLogSection(ILoggingConfigurationElement configSection)
  {
    switch (configSection.Name?.Trim().ToUpperInvariant())
    {
      case "TIME":
        this.ParseTimeElement(configSection);
        return true;
      case "VARIABLE":
        this.ParseVariableElement(configSection);
        return true;
      case "VARIABLES":
        this.ParseVariablesElement(configSection);
        return true;
      case "APPENDERS":
      case "TARGETS":
        this.ParseTargetsElement(configSection);
        return true;
      default:
        return false;
    }
  }

  private void ParseExtensionsElement(
    ILoggingConfigurationElement extensionsElement,
    string baseDirectory)
  {
    extensionsElement.AssertName("extensions");
    foreach (ILoggingConfigurationElement child in extensionsElement.Children)
    {
      string str = (string) null;
      string type = (string) null;
      string assemblyFile = (string) null;
      string assemblyName = (string) null;
      foreach (KeyValuePair<string, string> keyValuePair in child.Values)
      {
        if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "prefix"))
          str = keyValuePair.Value + ".";
        else if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "type"))
          type = keyValuePair.Value;
        else if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "assemblyFile"))
          assemblyFile = keyValuePair.Value;
        else if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "assembly"))
          assemblyName = keyValuePair.Value;
        else
          InternalLogger.Debug<string, string, string>("Skipping unknown property {0} for element {1} in section {2}", keyValuePair.Key, child.Name, extensionsElement.Name);
      }
      if (!StringHelpers.IsNullOrWhiteSpace(type))
        this.RegisterExtension(type, str);
      if (!StringHelpers.IsNullOrWhiteSpace(assemblyFile))
        this.ParseExtensionWithAssemblyFile(baseDirectory, assemblyFile, str);
      else if (!StringHelpers.IsNullOrWhiteSpace(assemblyName))
        this.ParseExtensionWithAssembly(assemblyName, str);
    }
  }

  private void RegisterExtension(string type, string itemNamePrefix)
  {
    try
    {
      this._configurationItemFactory.RegisterType(Type.GetType(type, true), itemNamePrefix);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException configException = new NLogConfigurationException("Error loading extensions: " + type, ex);
      if (this.MustThrowConfigException(configException))
        throw configException;
    }
  }

  private void ParseExtensionWithAssemblyFile(
    string baseDirectory,
    string assemblyFile,
    string prefix)
  {
    try
    {
      this._configurationItemFactory.RegisterItemsFromAssembly(AssemblyHelpers.LoadFromPath(assemblyFile, baseDirectory), prefix);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException configException = new NLogConfigurationException("Error loading extensions: " + assemblyFile, ex);
      if (this.MustThrowConfigException(configException))
        throw configException;
    }
  }

  private void ParseExtensionWithAssembly(string assemblyName, string prefix)
  {
    try
    {
      this._configurationItemFactory.RegisterItemsFromAssembly(AssemblyHelpers.LoadFromName(assemblyName), prefix);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException configException = new NLogConfigurationException("Error loading extensions: " + assemblyName, ex);
      if (this.MustThrowConfigException(configException))
        throw configException;
    }
  }

  private void ParseVariableElement(ILoggingConfigurationElement variableElement)
  {
    string key = (string) null;
    string input = (string) null;
    foreach (KeyValuePair<string, string> keyValuePair in variableElement.Values)
    {
      if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "name"))
        key = keyValuePair.Value;
      else if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "value"))
        input = keyValuePair.Value;
      else
        InternalLogger.Debug<string, string, string>("Skipping unknown property {0} for element {1} in section {2}", keyValuePair.Key, variableElement.Name, "variables");
    }
    if (!LoggingConfigurationParser.AssertNonEmptyValue(key, "name", variableElement.Name, "variables") || !LoggingConfigurationParser.AssertNotNullValue(input, "value", variableElement.Name, "variables"))
      return;
    string str = this.ExpandSimpleVariables(input);
    this.Variables[key] = (SimpleLayout) str;
  }

  private void ParseVariablesElement(ILoggingConfigurationElement variableElement)
  {
    variableElement.AssertName("variables");
    foreach (ILoggingConfigurationElement child in variableElement.Children)
      this.ParseVariableElement(child);
  }

  private void ParseTimeElement(ILoggingConfigurationElement timeElement)
  {
    timeElement.AssertName("time");
    string itemName = (string) null;
    foreach (KeyValuePair<string, string> keyValuePair in timeElement.Values)
    {
      if (LoggingConfigurationParser.MatchesName(keyValuePair.Key, "type"))
        itemName = keyValuePair.Value;
      else
        InternalLogger.Debug<string, string, string>("Skipping unknown property {0} for element {1} in section {2}", keyValuePair.Key, timeElement.Name, timeElement.Name);
    }
    if (!LoggingConfigurationParser.AssertNonEmptyValue(itemName, "type", timeElement.Name, string.Empty))
      return;
    TimeSource instance = this._configurationItemFactory.TimeSources.CreateInstance(itemName);
    this.ConfigureObjectFromAttributes((object) instance, timeElement, true);
    InternalLogger.Info<TimeSource>("Selecting time source {0}", instance);
    TimeSource.Current = instance;
  }

  [ContractAnnotation("value:notnull => true")]
  private static bool AssertNotNullValue(
    string value,
    string propertyName,
    string elementName,
    string sectionName)
  {
    return value != null || LoggingConfigurationParser.AssertNonEmptyValue(string.Empty, propertyName, elementName, sectionName);
  }

  [ContractAnnotation("value:null => false")]
  private static bool AssertNonEmptyValue(
    string value,
    string propertyName,
    string elementName,
    string sectionName)
  {
    if (!StringHelpers.IsNullOrWhiteSpace(value))
      return true;
    if (((int) LogManager.ThrowConfigExceptions ?? (LogManager.ThrowExceptions ? 1 : 0)) != 0)
      throw new NLogConfigurationException($"Expected property {propertyName} on element name: {elementName} in section: {sectionName}");
    InternalLogger.Warn<string, string, string>("Skipping element name: {0} in section: {1} because property {2} is blank", elementName, sectionName, propertyName);
    return false;
  }

  private void ParseRulesElement(
    ILoggingConfigurationElement rulesElement,
    IList<LoggingRule> rulesCollection)
  {
    InternalLogger.Trace(nameof (ParseRulesElement));
    rulesElement.AssertName("rules");
    foreach (ILoggingConfigurationElement child in rulesElement.Children)
    {
      LoggingRule ruleElement = this.ParseRuleElement(child);
      if (ruleElement != null)
      {
        lock (rulesCollection)
          rulesCollection.Add(ruleElement);
      }
    }
  }

  private NLog.LogLevel LogLevelFromString(string text)
  {
    return NLog.LogLevel.FromString(this.ExpandSimpleVariables(text).Trim());
  }

  private LoggingRule ParseRuleElement(ILoggingConfigurationElement loggerElement)
  {
    string minLevel = (string) null;
    string maxLevel = (string) null;
    string enableLevels = (string) null;
    string ruleName = (string) null;
    string str1 = (string) null;
    bool flag1 = true;
    bool flag2 = false;
    string writeTargets = (string) null;
    string filterDefaultAction = (string) null;
    foreach (KeyValuePair<string, string> keyValuePair in loggerElement.Values)
    {
      switch (keyValuePair.Key?.Trim().ToUpperInvariant())
      {
        case "APPENDTO":
          writeTargets = keyValuePair.Value;
          continue;
        case "ENABLED":
          flag1 = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, true);
          continue;
        case "FILTERDEFAULTACTION":
          filterDefaultAction = keyValuePair.Value;
          continue;
        case "FINAL":
          flag2 = this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, false);
          continue;
        case "LEVEL":
          enableLevels = keyValuePair.Value;
          continue;
        case "LEVELS":
          enableLevels = StringHelpers.IsNullOrWhiteSpace(keyValuePair.Value) ? "," : keyValuePair.Value;
          continue;
        case "LOGGER":
          str1 = keyValuePair.Value;
          continue;
        case "MAXLEVEL":
          maxLevel = keyValuePair.Value;
          continue;
        case "MINLEVEL":
          minLevel = keyValuePair.Value;
          continue;
        case "NAME":
          if (loggerElement.MatchesName("logger"))
          {
            str1 = keyValuePair.Value;
            continue;
          }
          ruleName = keyValuePair.Value;
          continue;
        case "RULENAME":
          ruleName = keyValuePair.Value;
          continue;
        case "WRITETO":
          writeTargets = keyValuePair.Value;
          continue;
        default:
          InternalLogger.Debug<string, string, string>("Skipping unknown property {0} for element {1} in section {2}", keyValuePair.Key, loggerElement.Name, "rules");
          continue;
      }
    }
    if (string.IsNullOrEmpty(ruleName) && string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(writeTargets) && !flag2)
    {
      InternalLogger.Debug("Logging rule without name or filter or targets is ignored");
      return (LoggingRule) null;
    }
    string str2 = str1 ?? "*";
    if (!flag1)
    {
      InternalLogger.Debug<string, string>("Logging rule {0} with filter `{1}` is disabled", ruleName, str2);
      return (LoggingRule) null;
    }
    LoggingRule rule = new LoggingRule(ruleName)
    {
      LoggerNamePattern = str2,
      Final = flag2
    };
    this.EnableLevelsForRule(rule, enableLevels, minLevel, maxLevel);
    this.ParseLoggingRuleTargets(writeTargets, rule);
    this.ParseLoggingRuleChildren(loggerElement, rule, filterDefaultAction);
    return rule;
  }

  private void EnableLevelsForRule(
    LoggingRule rule,
    string enableLevels,
    string minLevel,
    string maxLevel)
  {
    if (enableLevels != null)
    {
      enableLevels = this.ExpandSimpleVariables(enableLevels);
      if (enableLevels.IndexOf('{') >= 0)
      {
        SimpleLayout levelLayout = this.ParseLevelLayout(enableLevels);
        rule.EnableLoggingForLevels(levelLayout);
      }
      else if (enableLevels.IndexOf(',') >= 0)
      {
        foreach (NLog.LogLevel level in this.ParseLevels(enableLevels))
          rule.EnableLoggingForLevel(level);
      }
      else
        rule.EnableLoggingForLevel(this.LogLevelFromString(enableLevels));
    }
    else
    {
      minLevel = minLevel != null ? this.ExpandSimpleVariables(minLevel) : minLevel;
      maxLevel = maxLevel != null ? this.ExpandSimpleVariables(maxLevel) : maxLevel;
      if (minLevel != null && minLevel.IndexOf('{') >= 0 || maxLevel != null && maxLevel.IndexOf('{') >= 0)
      {
        SimpleLayout levelLayout1 = this.ParseLevelLayout(minLevel);
        SimpleLayout levelLayout2 = this.ParseLevelLayout(maxLevel);
        rule.EnableLoggingForRange(levelLayout1, levelLayout2);
      }
      else
      {
        NLog.LogLevel minLevel1 = minLevel != null ? this.LogLevelFromString(minLevel) : NLog.LogLevel.MinLevel;
        NLog.LogLevel maxLevel1 = maxLevel != null ? this.LogLevelFromString(maxLevel) : NLog.LogLevel.MaxLevel;
        rule.SetLoggingLevels(minLevel1, maxLevel1);
      }
    }
  }

  private SimpleLayout ParseLevelLayout(string levelLayout)
  {
    SimpleLayout levelLayout1 = !StringHelpers.IsNullOrWhiteSpace(levelLayout) ? new SimpleLayout(levelLayout, this._configurationItemFactory) : (SimpleLayout) null;
    if (levelLayout1 == null)
      return levelLayout1;
    levelLayout1.Initialize((LoggingConfiguration) this);
    return levelLayout1;
  }

  private IEnumerable<NLog.LogLevel> ParseLevels(string enableLevels)
  {
    return ((IEnumerable<string>) enableLevels.SplitAndTrimTokens(',')).Select<string, NLog.LogLevel>(new Func<string, NLog.LogLevel>(this.LogLevelFromString));
  }

  private void ParseLoggingRuleTargets(string writeTargets, LoggingRule rule)
  {
    if (string.IsNullOrEmpty(writeTargets))
      return;
    foreach (string splitAndTrimToken in writeTargets.SplitAndTrimTokens(','))
    {
      Target targetByName = this.FindTargetByName(splitAndTrimToken);
      if (targetByName != null)
      {
        rule.Targets.Add(targetByName);
      }
      else
      {
        NLogConfigurationException configException = new NLogConfigurationException($"Target '{splitAndTrimToken}' not found for logging rule: {(string.IsNullOrEmpty(rule.RuleName) ? rule.LoggerNamePattern : rule.RuleName)}.");
        if (this.MustThrowConfigException(configException))
          throw configException;
      }
    }
  }

  private void ParseLoggingRuleChildren(
    ILoggingConfigurationElement loggerElement,
    LoggingRule rule,
    string filterDefaultAction = null)
  {
    foreach (ILoggingConfigurationElement child in loggerElement.Children)
    {
      LoggingRule loggingRule = (LoggingRule) null;
      if (child.MatchesName("filters"))
        this.ParseFilters(rule, child, filterDefaultAction);
      else if (child.MatchesName("logger") && loggerElement.MatchesName("logger"))
        loggingRule = this.ParseRuleElement(child);
      else if (child.MatchesName(nameof (rule)) && loggerElement.MatchesName(nameof (rule)))
        loggingRule = this.ParseRuleElement(child);
      else
        InternalLogger.Debug<string, string, string>("Skipping unknown child {0} for element {1} in section {2}", child.Name, loggerElement.Name, "rules");
      if (loggingRule != null)
      {
        lock (rule.ChildRules)
          rule.ChildRules.Add(loggingRule);
      }
    }
  }

  private void ParseFilters(
    LoggingRule rule,
    ILoggingConfigurationElement filtersElement,
    string filterDefaultAction = null)
  {
    filtersElement.AssertName("filters");
    filterDefaultAction = filtersElement.GetOptionalValue("defaultAction", (string) null) ?? filtersElement.GetOptionalValue(nameof (filterDefaultAction), (string) null) ?? filterDefaultAction;
    if (filterDefaultAction != null)
      PropertyHelper.SetPropertyFromString((object) rule, "DefaultFilterResult", filterDefaultAction, this._configurationItemFactory);
    foreach (ILoggingConfigurationElement child in filtersElement.Children)
    {
      Filter instance = this._configurationItemFactory.Filters.CreateInstance(child.GetOptionalValue("type", (string) null) ?? child.Name);
      this.ConfigureObjectFromAttributes((object) instance, child, true);
      rule.Filters.Add(instance);
    }
  }

  private void ParseTargetsElement(ILoggingConfigurationElement targetsElement)
  {
    targetsElement.AssertName("targets", "appenders");
    KeyValuePair<string, string> keyValuePair = targetsElement.Values.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (configItem => LoggingConfigurationParser.MatchesName(configItem.Key, "async")));
    bool flag = !string.IsNullOrEmpty(keyValuePair.Value) && this.ParseBooleanValue(keyValuePair.Key, keyValuePair.Value, false);
    ILoggingConfigurationElement defaultParameters = (ILoggingConfigurationElement) null;
    Dictionary<string, ILoggingConfigurationElement> typeNameToDefaultTargetParameters = new Dictionary<string, ILoggingConfigurationElement>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    foreach (ILoggingConfigurationElement child in targetsElement.Children)
    {
      string itemTypeAttribute = child.GetConfigItemTypeAttribute();
      string optionalValue = child.GetOptionalValue("name", (string) null);
      Target target = (Target) null;
      string elementName = string.IsNullOrEmpty(optionalValue) ? child.Name : $"{child.Name}(Name={optionalValue})";
      switch (child.Name?.Trim().ToUpperInvariant())
      {
        case "APPENDER":
        case "COMPOUND-TARGET":
        case "TARGET":
        case "WRAPPER":
        case "WRAPPER-TARGET":
          if (LoggingConfigurationParser.AssertNonEmptyValue(itemTypeAttribute, "type", elementName, targetsElement.Name))
          {
            target = this.CreateTargetType(itemTypeAttribute);
            if (target != null)
            {
              this.ParseTargetElement(target, child, typeNameToDefaultTargetParameters);
              break;
            }
            break;
          }
          break;
        case "DEFAULT-TARGET-PARAMETERS":
        case "TARGETDEFAULTPARAMETERS":
          if (LoggingConfigurationParser.AssertNonEmptyValue(itemTypeAttribute, "type", elementName, targetsElement.Name))
          {
            this.ParseDefaultTargetParameters(child, itemTypeAttribute, typeNameToDefaultTargetParameters);
            break;
          }
          break;
        case "DEFAULT-WRAPPER":
        case "TARGETDEFAULTWRAPPER":
          if (LoggingConfigurationParser.AssertNonEmptyValue(itemTypeAttribute, "type", elementName, targetsElement.Name))
          {
            defaultParameters = child;
            break;
          }
          break;
        default:
          InternalLogger.Debug<string, string>("Skipping unknown element {0} in section {1}", elementName, targetsElement.Name);
          break;
      }
      if (target != null)
      {
        if (flag)
          target = LoggingConfigurationParser.WrapWithAsyncTargetWrapper(target);
        if (defaultParameters != null)
          target = this.WrapWithDefaultWrapper(target, defaultParameters);
        InternalLogger.Info<string, string>("Adding target {0}(Name={1})", target.GetType().Name, target.Name);
        this.AddTarget(target.Name, target);
      }
    }
  }

  private Target CreateTargetType(string targetTypeName)
  {
    Target targetType = (Target) null;
    try
    {
      targetType = this._configurationItemFactory.Targets.CreateInstance(targetTypeName);
      if (targetType == null)
        throw new NLogConfigurationException("Factory returned null for target type: " + targetTypeName);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      NLogConfigurationException configException = new NLogConfigurationException("Failed to create target type: " + targetTypeName, ex);
      if (this.MustThrowConfigException(configException))
        throw configException;
    }
    return targetType;
  }

  private void ParseDefaultTargetParameters(
    ILoggingConfigurationElement defaultTargetElement,
    string targetType,
    Dictionary<string, ILoggingConfigurationElement> typeNameToDefaultTargetParameters)
  {
    typeNameToDefaultTargetParameters[targetType.Trim()] = defaultTargetElement;
  }

  private void ParseTargetElement(
    Target target,
    ILoggingConfigurationElement targetElement,
    Dictionary<string, ILoggingConfigurationElement> typeNameToDefaultTargetParameters = null)
  {
    string itemTypeAttribute = targetElement.GetConfigItemTypeAttribute("targets");
    ILoggingConfigurationElement targetElement1;
    if (typeNameToDefaultTargetParameters != null && typeNameToDefaultTargetParameters.TryGetValue(itemTypeAttribute, out targetElement1))
      this.ParseTargetElement(target, targetElement1);
    CompoundTargetBase compound = target as CompoundTargetBase;
    WrapperTargetBase wrapper = target as WrapperTargetBase;
    this.ConfigureObjectFromAttributes((object) target, targetElement, true);
    foreach (ILoggingConfigurationElement child in targetElement.Children)
    {
      string name = child.Name;
      if ((compound == null || !this.ParseCompoundTarget(typeNameToDefaultTargetParameters, name, child, compound, (string) null)) && (wrapper == null || !this.ParseTargetWrapper(typeNameToDefaultTargetParameters, name, child, wrapper)))
        this.SetPropertyFromElement((object) target, child, targetElement);
    }
  }

  private bool ParseTargetWrapper(
    Dictionary<string, ILoggingConfigurationElement> typeNameToDefaultTargetParameters,
    string name,
    ILoggingConfigurationElement childElement,
    WrapperTargetBase wrapper)
  {
    if (LoggingConfigurationParser.IsTargetRefElement(name))
    {
      string requiredValue = childElement.GetRequiredValue(nameof (name), LoggingConfigurationParser.GetName((Target) wrapper));
      Target targetByName = this.FindTargetByName(requiredValue);
      if (targetByName == null)
      {
        NLogConfigurationException configException = new NLogConfigurationException($"Referenced target '{requiredValue}' not found.");
        if (this.MustThrowConfigException(configException))
          throw configException;
      }
      wrapper.WrappedTarget = targetByName;
      return true;
    }
    if (!LoggingConfigurationParser.IsTargetElement(name))
      return false;
    string itemTypeAttribute = childElement.GetConfigItemTypeAttribute(LoggingConfigurationParser.GetName((Target) wrapper));
    Target targetType = this.CreateTargetType(itemTypeAttribute);
    if (targetType != null)
    {
      this.ParseTargetElement(targetType, childElement, typeNameToDefaultTargetParameters);
      if (targetType.Name != null)
        this.AddTarget(targetType.Name, targetType);
      if (wrapper.WrappedTarget != null)
      {
        NLogConfigurationException configException = new NLogConfigurationException($"Failed to assign wrapped target {itemTypeAttribute}, because target {wrapper.Name} already has one.");
        if (this.MustThrowConfigException(configException))
          throw configException;
      }
    }
    wrapper.WrappedTarget = targetType;
    return true;
  }

  private bool ParseCompoundTarget(
    Dictionary<string, ILoggingConfigurationElement> typeNameToDefaultTargetParameters,
    string name,
    ILoggingConfigurationElement childElement,
    CompoundTargetBase compound,
    string targetName)
  {
    if (LoggingConfigurationParser.MatchesName(name, "targets") || LoggingConfigurationParser.MatchesName(name, "appenders"))
    {
      foreach (ILoggingConfigurationElement child in childElement.Children)
        this.ParseCompoundTarget(typeNameToDefaultTargetParameters, child.Name, child, compound, (string) null);
      return true;
    }
    if (LoggingConfigurationParser.IsTargetRefElement(name))
    {
      targetName = childElement.GetRequiredValue(nameof (name), LoggingConfigurationParser.GetName((Target) compound));
      compound.Targets.Add(this.FindTargetByName(targetName) ?? throw new NLogConfigurationException($"Referenced target '{targetName}' not found."));
      return true;
    }
    if (!LoggingConfigurationParser.IsTargetElement(name))
      return false;
    Target targetType = this.CreateTargetType(childElement.GetConfigItemTypeAttribute(LoggingConfigurationParser.GetName((Target) compound)));
    if (targetType != null)
    {
      if (targetName != null)
        targetType.Name = targetName;
      this.ParseTargetElement(targetType, childElement, typeNameToDefaultTargetParameters);
      if (targetType.Name != null)
        this.AddTarget(targetType.Name, targetType);
      compound.Targets.Add(targetType);
    }
    return true;
  }

  private void ConfigureObjectFromAttributes(
    object targetObject,
    ILoggingConfigurationElement element,
    bool ignoreType)
  {
    foreach (KeyValuePair<string, string> keyValuePair in element.Values)
    {
      string key = keyValuePair.Key;
      string input = keyValuePair.Value;
      if (!ignoreType || !LoggingConfigurationParser.MatchesName(key, "type"))
      {
        try
        {
          PropertyHelper.SetPropertyFromString(targetObject, key, this.ExpandSimpleVariables(input), this._configurationItemFactory);
        }
        catch (NLogConfigurationException ex)
        {
          if (this.MustThrowConfigException(ex))
            throw;
        }
        catch (Exception ex)
        {
          if (ex.MustBeRethrownImmediately())
            throw;
          string message = $"Error when setting value '{input}' for property '{key}' on element '{element}'";
          object[] objArray = new object[0];
          if (this.MustThrowConfigException(new NLogConfigurationException(ex, message, objArray)))
            throw;
        }
      }
    }
  }

  private void SetPropertyFromElement(
    object o,
    ILoggingConfigurationElement childElement,
    ILoggingConfigurationElement parentElement)
  {
    PropertyInfo result;
    if (!PropertyHelper.TryGetPropertyInfo(o, childElement.Name, out result))
    {
      InternalLogger.Debug("Skipping unknown element {0} in section {1}. Not matching any property on {2} - {3}", (object) childElement.Name, (object) parentElement.Name, o, (object) o?.GetType());
    }
    else
    {
      if (this.AddArrayItemFromElement(o, result, childElement) || this.SetLayoutFromElement(o, result, childElement) || this.SetFilterFromElement(o, result, childElement))
        return;
      this.SetItemFromElement(o, result, childElement);
    }
  }

  private bool AddArrayItemFromElement(
    object o,
    PropertyInfo propInfo,
    ILoggingConfigurationElement element)
  {
    Type arrayItemType = PropertyHelper.GetArrayItemType(propInfo);
    if (!(arrayItemType != (Type) null))
      return false;
    IList list1 = (IList) propInfo.GetValue(o, (object[]) null);
    if (string.Equals(propInfo.Name, element.Name, StringComparison.OrdinalIgnoreCase))
    {
      List<ILoggingConfigurationElement> list2 = element.Children.ToList<ILoggingConfigurationElement>();
      if (list2.Count > 0)
      {
        foreach (ILoggingConfigurationElement element1 in list2)
          list1.Add(this.ParseArrayItemFromElement(arrayItemType, element1));
        return true;
      }
    }
    object arrayItemFromElement = this.ParseArrayItemFromElement(arrayItemType, element);
    list1.Add(arrayItemFromElement);
    return true;
  }

  private object ParseArrayItemFromElement(Type elementType, ILoggingConfigurationElement element)
  {
    object targetObject = (object) this.TryCreateLayoutInstance(element, elementType) ?? FactoryHelper.CreateInstance(elementType);
    this.ConfigureObjectFromAttributes(targetObject, element, true);
    this.ConfigureObjectFromElement(targetObject, element);
    return targetObject;
  }

  private bool SetLayoutFromElement(
    object o,
    PropertyInfo propInfo,
    ILoggingConfigurationElement element)
  {
    Layout layoutInstance = this.TryCreateLayoutInstance(element, propInfo.PropertyType);
    if (layoutInstance == null)
      return false;
    this.SetItemOnProperty(o, propInfo, element, (object) layoutInstance);
    return true;
  }

  private bool SetFilterFromElement(
    object o,
    PropertyInfo propInfo,
    ILoggingConfigurationElement element)
  {
    Type propertyType = propInfo.PropertyType;
    Filter filterInstance = this.TryCreateFilterInstance(element, propertyType);
    if (filterInstance == null)
      return false;
    this.SetItemOnProperty(o, propInfo, element, (object) filterInstance);
    return true;
  }

  private Layout TryCreateLayoutInstance(ILoggingConfigurationElement element, Type type)
  {
    return this.TryCreateInstance<Layout>(element, type, this._configurationItemFactory.Layouts);
  }

  private Filter TryCreateFilterInstance(ILoggingConfigurationElement element, Type type)
  {
    return this.TryCreateInstance<Filter>(element, type, this._configurationItemFactory.Filters);
  }

  private T TryCreateInstance<T>(
    ILoggingConfigurationElement element,
    Type type,
    INamedItemFactory<T, Type> factory)
    where T : class
  {
    if (!LoggingConfigurationParser.IsAssignableFrom<T>(type))
      return default (T);
    string itemTypeAttribute = element.GetConfigItemTypeAttribute();
    return itemTypeAttribute == null ? default (T) : factory.CreateInstance(this.ExpandSimpleVariables(itemTypeAttribute));
  }

  private static bool IsAssignableFrom<T>(Type type) => typeof (T).IsAssignableFrom(type);

  private void SetItemOnProperty(
    object o,
    PropertyInfo propInfo,
    ILoggingConfigurationElement element,
    object properyValue)
  {
    this.ConfigureObjectFromAttributes(properyValue, element, true);
    this.ConfigureObjectFromElement(properyValue, element);
    propInfo.SetValue(o, properyValue, (object[]) null);
  }

  private void SetItemFromElement(
    object o,
    PropertyInfo propInfo,
    ILoggingConfigurationElement element)
  {
    object targetObject = propInfo.GetValue(o, (object[]) null);
    this.ConfigureObjectFromAttributes(targetObject, element, true);
    this.ConfigureObjectFromElement(targetObject, element);
  }

  private void ConfigureObjectFromElement(object targetObject, ILoggingConfigurationElement element)
  {
    foreach (ILoggingConfigurationElement child in element.Children)
      this.SetPropertyFromElement(targetObject, child, element);
  }

  private static Target WrapWithAsyncTargetWrapper(Target target)
  {
    if (target is AsyncTaskTarget)
    {
      InternalLogger.Debug<string>("Skip wrapping target '{0}' with AsyncTargetWrapper", target.Name);
      return target;
    }
    AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper();
    asyncTargetWrapper.WrappedTarget = target;
    asyncTargetWrapper.Name = target.Name;
    target.Name += "_wrapped";
    InternalLogger.Debug<string, string>("Wrapping target '{0}' with AsyncTargetWrapper and renaming to '{1}", asyncTargetWrapper.Name, target.Name);
    target = (Target) asyncTargetWrapper;
    return target;
  }

  private Target WrapWithDefaultWrapper(
    Target target,
    ILoggingConfigurationElement defaultParameters)
  {
    Target targetType = this.CreateTargetType(defaultParameters.GetConfigItemTypeAttribute("targets"));
    if (!(targetType is WrapperTargetBase wrapperTargetBase2))
      throw new NLogConfigurationException("Target type specified on <default-wrapper /> is not a wrapper.");
    this.ParseTargetElement(targetType, defaultParameters);
    while (wrapperTargetBase2.WrappedTarget != null)
    {
      if (!(wrapperTargetBase2.WrappedTarget is WrapperTargetBase wrapperTargetBase2))
        throw new NLogConfigurationException("Child target type specified on <default-wrapper /> is not a wrapper.");
    }
    if (target is AsyncTaskTarget && targetType is AsyncTargetWrapper && targetType == wrapperTargetBase2)
    {
      InternalLogger.Debug<string>("Skip wrapping target '{0}' with AsyncTargetWrapper", target.Name);
      return target;
    }
    wrapperTargetBase2.WrappedTarget = target;
    targetType.Name = target.Name;
    target.Name += "_wrapped";
    InternalLogger.Debug<string, string, string>("Wrapping target '{0}' with '{1}' and renaming to '{2}", targetType.Name, targetType.GetType().Name, target.Name);
    return targetType;
  }

  private bool ParseBooleanValue(string propertyName, string value, bool defaultValue)
  {
    try
    {
      return Convert.ToBoolean(value?.Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      string message = $"'{propertyName}' hasn't a valid boolean value '{value}'. {defaultValue} will be used";
      object[] objArray = new object[0];
      NLogConfigurationException configException = new NLogConfigurationException(ex, message, objArray);
      if (this.MustThrowConfigException(configException))
        throw configException;
      return defaultValue;
    }
  }

  private bool? ParseNullableBooleanValue(string propertyName, string value, bool defaultValue)
  {
    return !StringHelpers.IsNullOrWhiteSpace(value) ? new bool?(this.ParseBooleanValue(propertyName, value, defaultValue)) : new bool?();
  }

  private bool MustThrowConfigException(NLogConfigurationException configException)
  {
    return configException.MustBeRethrown() || ((int) this.LogFactory.ThrowConfigExceptions ?? (this.LogFactory.ThrowExceptions ? 1 : 0)) != 0;
  }

  private static bool MatchesName(string key, string expectedKey)
  {
    return string.Equals(key?.Trim(), expectedKey, StringComparison.OrdinalIgnoreCase);
  }

  private static bool IsTargetElement(string name)
  {
    return name.Equals("target", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper-target", StringComparison.OrdinalIgnoreCase) || name.Equals("compound-target", StringComparison.OrdinalIgnoreCase);
  }

  private static bool IsTargetRefElement(string name)
  {
    return name.Equals("target-ref", StringComparison.OrdinalIgnoreCase) || name.Equals("wrapper-target-ref", StringComparison.OrdinalIgnoreCase) || name.Equals("compound-target-ref", StringComparison.OrdinalIgnoreCase);
  }

  private static string GetName(Target target)
  {
    return !string.IsNullOrEmpty(target.Name) ? target.Name : target.GetType().Name;
  }
}

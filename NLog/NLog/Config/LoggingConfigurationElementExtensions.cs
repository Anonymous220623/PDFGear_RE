// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationElementExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace NLog.Config;

internal static class LoggingConfigurationElementExtensions
{
  public static bool MatchesName(this ILoggingConfigurationElement section, string expectedName)
  {
    return string.Equals(section?.Name?.Trim(), expectedName, StringComparison.OrdinalIgnoreCase);
  }

  public static void AssertName(
    this ILoggingConfigurationElement section,
    params string[] allowedNames)
  {
    foreach (string allowedName in allowedNames)
    {
      if (section.MatchesName(allowedName))
        return;
    }
    throw new InvalidOperationException($"Assertion failed. Expected element name '{string.Join("|", allowedNames)}', actual: '{section?.Name}'.");
  }

  public static string GetRequiredValue(
    this ILoggingConfigurationElement element,
    string attributeName,
    string section)
  {
    string optionalValue = element.GetOptionalValue(attributeName, (string) null);
    if (optionalValue == null)
      throw new NLogConfigurationException($"Expected {attributeName} on {element.Name} in {section}");
    return !StringHelpers.IsNullOrWhiteSpace(optionalValue) ? optionalValue : throw new NLogConfigurationException($"Expected non-empty {attributeName} on {element.Name} in {section}");
  }

  public static string GetOptionalValue(
    this ILoggingConfigurationElement element,
    string attributeName,
    string defaultValue)
  {
    return element.Values.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (configItem => string.Equals(configItem.Key, attributeName, StringComparison.OrdinalIgnoreCase))).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (configItem => configItem.Value)).FirstOrDefault<string>() ?? defaultValue;
  }

  public static bool GetOptionalBooleanValue(
    this ILoggingConfigurationElement element,
    string attributeName,
    bool defaultValue)
  {
    string optionalValue = element.GetOptionalValue(attributeName, (string) null);
    if (string.IsNullOrEmpty(optionalValue))
      return defaultValue;
    try
    {
      return Convert.ToBoolean(optionalValue.Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch (Exception ex)
    {
      NLogConfigurationException exception = new NLogConfigurationException(ex, $"'{attributeName}' hasn't a valid boolean value '{optionalValue}'. {defaultValue} will be used", new object[0]);
      if (exception.MustBeRethrown())
        throw exception;
      InternalLogger.Error(ex, exception.Message);
      return defaultValue;
    }
  }

  public static string GetConfigItemTypeAttribute(
    this ILoggingConfigurationElement element,
    string sectionNameForRequiredValue = null)
  {
    return LoggingConfigurationElementExtensions.StripOptionalNamespacePrefix(sectionNameForRequiredValue != null ? element.GetRequiredValue("type", sectionNameForRequiredValue) : element.GetOptionalValue("type", (string) null));
  }

  private static string StripOptionalNamespacePrefix(string attributeValue)
  {
    if (attributeValue == null)
      return (string) null;
    int num = attributeValue.IndexOf(':');
    return num < 0 ? attributeValue : attributeValue.Substring(num + 1);
  }
}

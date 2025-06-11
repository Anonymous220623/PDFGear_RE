// Decompiled with JetBrains decompiler
// Type: NLog.Config.NLogXmlElement
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace NLog.Config;

internal class NLogXmlElement : ILoggingConfigurationElement
{
  private readonly List<string> _parsingErrors;

  public NLogXmlElement(XmlReader reader)
    : this(reader, false)
  {
  }

  public NLogXmlElement(XmlReader reader, bool nestedElement)
    : this()
  {
    this.Parse(reader, nestedElement);
  }

  private NLogXmlElement()
  {
    this.AttributeValues = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    this.Children = (IList<NLogXmlElement>) new List<NLogXmlElement>();
    this._parsingErrors = new List<string>();
  }

  public string LocalName { get; private set; }

  public Dictionary<string, string> AttributeValues { get; }

  public IList<NLogXmlElement> Children { get; }

  public string Value { get; private set; }

  public string Name => this.LocalName;

  public IEnumerable<KeyValuePair<string, string>> Values
  {
    get
    {
      for (int index = 0; index < this.Children.Count; ++index)
      {
        if (NLogXmlElement.SingleValueElement(this.Children[index]))
          return this.Children.Where<NLogXmlElement>((Func<NLogXmlElement, bool>) (item => NLogXmlElement.SingleValueElement(item))).Select<NLogXmlElement, KeyValuePair<string, string>>((Func<NLogXmlElement, KeyValuePair<string, string>>) (item => new KeyValuePair<string, string>(item.Name, item.Value))).Concat<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>) this.AttributeValues);
      }
      return (IEnumerable<KeyValuePair<string, string>>) this.AttributeValues;
    }
  }

  private static bool SingleValueElement(NLogXmlElement child)
  {
    return child.Children.Count == 0 && child.AttributeValues.Count == 0 && child.Value != null;
  }

  IEnumerable<ILoggingConfigurationElement> ILoggingConfigurationElement.Children
  {
    get
    {
      for (int index = 0; index < this.Children.Count; ++index)
      {
        if (!NLogXmlElement.SingleValueElement(this.Children[index]))
          return this.Children.Where<NLogXmlElement>((Func<NLogXmlElement, bool>) (item => !NLogXmlElement.SingleValueElement(item))).Cast<ILoggingConfigurationElement>();
      }
      return (IEnumerable<ILoggingConfigurationElement>) ArrayHelper.Empty<ILoggingConfigurationElement>();
    }
  }

  public IEnumerable<NLogXmlElement> Elements(string elementName)
  {
    List<NLogXmlElement> nlogXmlElementList = new List<NLogXmlElement>();
    foreach (NLogXmlElement child in (IEnumerable<NLogXmlElement>) this.Children)
    {
      if (child.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase))
        nlogXmlElementList.Add(child);
    }
    return (IEnumerable<NLogXmlElement>) nlogXmlElementList;
  }

  public void AssertName(params string[] allowedNames)
  {
    foreach (string allowedName in allowedNames)
    {
      if (this.LocalName.Equals(allowedName, StringComparison.OrdinalIgnoreCase))
        return;
    }
    throw new InvalidOperationException($"Assertion failed. Expected element name '{string.Join("|", allowedNames)}', actual: '{this.LocalName}'.");
  }

  public IEnumerable<string> GetParsingErrors()
  {
    foreach (string parsingError in this._parsingErrors)
      yield return parsingError;
    foreach (NLogXmlElement child in (IEnumerable<NLogXmlElement>) this.Children)
    {
      foreach (string parsingError in child.GetParsingErrors())
        yield return parsingError;
    }
  }

  private void Parse(XmlReader reader, bool nestedElement)
  {
    this.ParseAttributes(reader, nestedElement);
    this.LocalName = reader.LocalName;
    if (reader.IsEmptyElement)
      return;
    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
        this.Value += reader.Value;
      else if (reader.NodeType == XmlNodeType.Element)
        this.Children.Add(new NLogXmlElement(reader, true));
    }
  }

  private void ParseAttributes(XmlReader reader, bool nestedElement)
  {
    if (!reader.MoveToFirstAttribute())
      return;
    do
    {
      if (nestedElement || !NLogXmlElement.IsSpecialXmlAttribute(reader))
      {
        if (!this.AttributeValues.ContainsKey(reader.LocalName))
          this.AttributeValues.Add(reader.LocalName, reader.Value);
        else
          this._parsingErrors.Add($"Duplicate attribute detected. Attribute name: [{reader.LocalName}]. Duplicate value:[{reader.Value}], Current value:[{this.AttributeValues[reader.LocalName]}]");
      }
    }
    while (reader.MoveToNextAttribute());
    reader.MoveToElement();
  }

  private static bool IsSpecialXmlAttribute(XmlReader reader)
  {
    string localName = reader.LocalName;
    if ((localName != null ? (localName.Equals("xmlns", StringComparison.OrdinalIgnoreCase) ? 1 : 0) : 0) != 0)
      return true;
    string prefix1 = reader.Prefix;
    if ((prefix1 != null ? (prefix1.Equals("xsi", StringComparison.OrdinalIgnoreCase) ? 1 : 0) : 0) != 0)
      return true;
    string prefix2 = reader.Prefix;
    return (prefix2 != null ? (prefix2.Equals("xmlns", StringComparison.OrdinalIgnoreCase) ? 1 : 0) : 0) != 0;
  }
}

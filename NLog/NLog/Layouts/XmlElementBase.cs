// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.XmlElementBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers.Wrappers;
using NLog.MessageTemplates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
public abstract class XmlElementBase : Layout
{
  private const string DefaultPropertyName = "property";
  private const string DefaultPropertyKeyAttribute = "key";
  private const string DefaultCollectionItemName = "item";
  private string _elementName;
  internal readonly XmlEncodeLayoutRendererWrapper LayoutWrapper = new XmlEncodeLayoutRendererWrapper();
  private string _propertiesElementName = "property";
  private bool _propertiesElementNameHasFormat;
  private ObjectReflectionCache _objectReflectionCache;
  private static readonly IEqualityComparer<object> _referenceEqualsComparer = (IEqualityComparer<object>) SingleItemOptimizedHashSet<object>.ReferenceEqualityComparer.Default;
  private const int MaxXmlLength = 524288 /*0x080000*/;

  protected XmlElementBase(string elementName, Layout elementValue)
  {
    this.ElementNameInternal = elementName;
    this.LayoutWrapper.Inner = elementValue;
    this.Attributes = (IList<XmlAttribute>) new List<XmlAttribute>();
    this.Elements = (IList<XmlElement>) new List<XmlElement>();
    this.ExcludeProperties = (ISet<string>) new HashSet<string>();
  }

  internal string ElementNameInternal
  {
    get => this._elementName;
    set => this._elementName = XmlHelper.XmlConvertToElementName(value?.Trim(), true);
  }

  [DefaultValue(false)]
  public bool IndentXml { get; set; }

  [ArrayParameter(typeof (XmlElement), "element")]
  public IList<XmlElement> Elements { get; private set; }

  [ArrayParameter(typeof (XmlAttribute), "attribute")]
  public IList<XmlAttribute> Attributes { get; private set; }

  [DefaultValue(false)]
  public bool IncludeEmptyValue { get; set; }

  [DefaultValue(false)]
  public bool IncludeMdc { get; set; }

  [DefaultValue(false)]
  public bool IncludeMdlc { get; set; }

  [DefaultValue(false)]
  public bool IncludeAllProperties { get; set; }

  public ISet<string> ExcludeProperties { get; set; }

  public string PropertiesElementName
  {
    get => this._propertiesElementName;
    set
    {
      this._propertiesElementName = value;
      this._propertiesElementNameHasFormat = value != null && value.IndexOf('{') >= 0;
      if (this._propertiesElementNameHasFormat)
        return;
      this._propertiesElementName = XmlHelper.XmlConvertToElementName(value?.Trim(), true);
    }
  }

  public string PropertiesElementKeyAttribute { get; set; } = "key";

  public string PropertiesElementValueAttribute { get; set; }

  public string PropertiesCollectionItemName { get; set; } = "item";

  public int MaxRecursionLimit { get; set; } = 1;

  private ObjectReflectionCache ObjectReflectionCache
  {
    get
    {
      return this._objectReflectionCache ?? (this._objectReflectionCache = new ObjectReflectionCache());
    }
  }

  protected override void InitializeLayout()
  {
    base.InitializeLayout();
    if (this.IncludeMdc)
      this.ThreadAgnostic = false;
    if (this.IncludeMdlc)
      this.ThreadAgnostic = false;
    if (this.IncludeAllProperties)
      this.MutableUnsafe = true;
    if (this.Attributes.Count <= 1)
      return;
    HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    foreach (XmlAttribute attribute in (IEnumerable<XmlAttribute>) this.Attributes)
    {
      if (string.IsNullOrEmpty(attribute.Name))
        InternalLogger.Warn("XmlElement(ElementName={0}): Contains attribute with missing name (Ignored)");
      else if (stringSet.Contains(attribute.Name))
        InternalLogger.Warn<string, string>("XmlElement(ElementName={0}): Contains duplicate attribute name: {1} (Invalid xml)", this.ElementNameInternal, attribute.Name);
      else
        stringSet.Add(attribute.Name);
    }
  }

  internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.PrecalculateBuilderInternal(logEvent, target);
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    int length = target.Length;
    this.RenderXmlFormattedMessage(logEvent, target);
    if (target.Length != length || !this.IncludeEmptyValue || string.IsNullOrEmpty(this.ElementNameInternal))
      return;
    XmlElementBase.RenderSelfClosingElement(target, this.ElementNameInternal);
  }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.RenderAllocateBuilder(logEvent);
  }

  private void RenderXmlFormattedMessage(LogEventInfo logEvent, StringBuilder sb)
  {
    int length1 = sb.Length;
    if (!string.IsNullOrEmpty(this.ElementNameInternal))
    {
      for (int index = 0; index < this.Attributes.Count; ++index)
      {
        XmlAttribute attribute = this.Attributes[index];
        int length2 = sb.Length;
        if (!this.RenderAppendXmlAttributeValue(attribute, logEvent, sb, sb.Length == length1))
          sb.Length = length2;
      }
      if (sb.Length != length1)
      {
        if (!this.HasNestedXmlElements(logEvent))
        {
          sb.Append("/>");
          return;
        }
        sb.Append('>');
      }
      if (this.LayoutWrapper.Inner != null)
      {
        int length3 = sb.Length;
        if (sb.Length == length1)
          XmlElementBase.RenderStartElement(sb, this.ElementNameInternal);
        int length4 = sb.Length;
        this.LayoutWrapper.RenderAppendBuilder(logEvent, sb);
        int length5 = sb.Length;
        if (length4 == length5 && !this.IncludeEmptyValue)
          sb.Length = length3;
      }
      if (this.IndentXml && sb.Length != length1)
        sb.AppendLine();
    }
    for (int index = 0; index < this.Elements.Count; ++index)
    {
      XmlElement element = this.Elements[index];
      int length6 = sb.Length;
      if (!this.RenderAppendXmlElementValue((XmlElementBase) element, logEvent, sb, sb.Length == length1))
        sb.Length = length6;
    }
    this.AppendLogEventXmlProperties(logEvent, sb, length1);
    if (sb.Length <= length1 || string.IsNullOrEmpty(this.ElementNameInternal))
      return;
    this.EndXmlDocument(sb, this.ElementNameInternal);
  }

  private bool HasNestedXmlElements(LogEventInfo logEvent)
  {
    return this.LayoutWrapper.Inner != null || this.Elements.Count > 0 || this.IncludeMdc || this.IncludeMdlc || this.IncludeAllProperties && logEvent.HasProperties;
  }

  private void AppendLogEventXmlProperties(
    LogEventInfo logEventInfo,
    StringBuilder sb,
    int orgLength)
  {
    if (this.IncludeMdc)
    {
      foreach (string name in (IEnumerable<string>) MappedDiagnosticsContext.GetNames())
      {
        if (!string.IsNullOrEmpty(name))
        {
          object propertyValue = MappedDiagnosticsContext.GetObject(name);
          this.AppendXmlPropertyValue(name, propertyValue, sb, orgLength);
        }
      }
    }
    if (this.IncludeMdlc)
    {
      foreach (string name in (IEnumerable<string>) MappedDiagnosticsLogicalContext.GetNames())
      {
        if (!string.IsNullOrEmpty(name))
        {
          object propertyValue = MappedDiagnosticsLogicalContext.GetObject(name);
          this.AppendXmlPropertyValue(name, propertyValue, sb, orgLength);
        }
      }
    }
    if (!this.IncludeAllProperties || !logEventInfo.HasProperties)
      return;
    this.AppendLogEventProperties(logEventInfo, sb, orgLength);
  }

  private void AppendLogEventProperties(LogEventInfo logEventInfo, StringBuilder sb, int orgLength)
  {
    foreach (MessageTemplateParameter templateParameter in (IEnumerable<MessageTemplateParameter>) logEventInfo.CreateOrUpdatePropertiesInternal())
    {
      if (!string.IsNullOrEmpty(templateParameter.Name) && !this.ExcludeProperties.Contains(templateParameter.Name))
      {
        object propertyValue = templateParameter.Value;
        if (!string.IsNullOrEmpty(templateParameter.Format) && propertyValue is IFormattable formattable)
        {
          string format = templateParameter.Format;
          IFormatProvider formatProvider = logEventInfo.FormatProvider;
          if (formatProvider == null)
          {
            LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
            formatProvider = loggingConfiguration != null ? (IFormatProvider) loggingConfiguration.DefaultCultureInfo : (IFormatProvider) null;
          }
          propertyValue = (object) formattable.ToString(format, formatProvider);
        }
        else if (templateParameter.CaptureType == CaptureType.Stringify)
        {
          object obj = templateParameter.Value ?? (object) string.Empty;
          IFormatProvider provider = logEventInfo.FormatProvider;
          if (provider == null)
          {
            LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
            provider = loggingConfiguration != null ? (IFormatProvider) loggingConfiguration.DefaultCultureInfo : (IFormatProvider) null;
          }
          propertyValue = (object) Convert.ToString(obj, provider);
        }
        this.AppendXmlPropertyObjectValue(templateParameter.Name, propertyValue, sb, orgLength, new SingleItemOptimizedHashSet<object>(), 0);
      }
    }
  }

  private bool AppendXmlPropertyObjectValue(
    string propName,
    object propertyValue,
    StringBuilder sb,
    int orgLength,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth,
    bool ignorePropertiesElementName = false)
  {
    IConvertible convertible = propertyValue as IConvertible;
    TypeCode objTypeCode = propertyValue == null ? TypeCode.Empty : (convertible != null ? convertible.GetTypeCode() : TypeCode.Object);
    if (objTypeCode != TypeCode.Object)
    {
      string xmlValueString = XmlHelper.XmlConvertToString(convertible, objTypeCode, true);
      this.AppendXmlPropertyStringValue(propName, xmlValueString, sb, orgLength, ignorePropertiesElementName: ignorePropertiesElementName);
    }
    else
    {
      if (sb.Length > 524288 /*0x080000*/)
        return false;
      int depth1 = objectsInPath.Count == 0 ? depth : depth + 1;
      if (depth1 > this.MaxRecursionLimit || objectsInPath.Contains(propertyValue))
        return false;
      switch (propertyValue)
      {
        case IDictionary dictionary:
          using (XmlElementBase.StartCollectionScope(ref objectsInPath, (object) dictionary))
          {
            this.AppendXmlDictionaryObject(propName, dictionary, sb, orgLength, objectsInPath, depth1, ignorePropertiesElementName);
            break;
          }
        case IEnumerable collection:
          ObjectReflectionCache.ObjectPropertyList objectPropertyList;
          if (this.ObjectReflectionCache.TryLookupExpandoObject(propertyValue, out objectPropertyList))
          {
            using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(propertyValue, ref objectsInPath, false, XmlElementBase._referenceEqualsComparer))
            {
              this.AppendXmlObjectPropertyValues(propName, ref objectPropertyList, sb, orgLength, ref objectsInPath, depth1, ignorePropertiesElementName);
              break;
            }
          }
          using (XmlElementBase.StartCollectionScope(ref objectsInPath, (object) collection))
          {
            this.AppendXmlCollectionObject(propName, collection, sb, orgLength, objectsInPath, depth1, ignorePropertiesElementName);
            break;
          }
        default:
          using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(propertyValue, ref objectsInPath, false, XmlElementBase._referenceEqualsComparer))
          {
            ObjectReflectionCache.ObjectPropertyList propertyValues = this.ObjectReflectionCache.LookupObjectProperties(propertyValue);
            this.AppendXmlObjectPropertyValues(propName, ref propertyValues, sb, orgLength, ref objectsInPath, depth1, ignorePropertiesElementName);
            break;
          }
      }
    }
    return true;
  }

  private static SingleItemOptimizedHashSet<object>.SingleItemScopedInsert StartCollectionScope(
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    object value)
  {
    return new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, true, XmlElementBase._referenceEqualsComparer);
  }

  private void AppendXmlCollectionObject(
    string propName,
    IEnumerable collection,
    StringBuilder sb,
    int orgLength,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth,
    bool ignorePropertiesElementName)
  {
    string propNameElement = this.AppendXmlPropertyValue(propName, (object) string.Empty, sb, orgLength, true);
    if (string.IsNullOrEmpty(propNameElement))
      return;
    foreach (object propertyValue in collection)
    {
      int length = sb.Length;
      if (length <= 524288 /*0x080000*/)
      {
        if (!this.AppendXmlPropertyObjectValue(this.PropertiesCollectionItemName, propertyValue, sb, orgLength, objectsInPath, depth, true))
          sb.Length = length;
      }
      else
        break;
    }
    this.AppendClosingPropertyTag(propNameElement, sb, ignorePropertiesElementName);
  }

  private void AppendXmlDictionaryObject(
    string propName,
    IDictionary dictionary,
    StringBuilder sb,
    int orgLength,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth,
    bool ignorePropertiesElementName)
  {
    string propNameElement = this.AppendXmlPropertyValue(propName, (object) string.Empty, sb, orgLength, true, ignorePropertiesElementName);
    if (string.IsNullOrEmpty(propNameElement))
      return;
    foreach (DictionaryEntry dictionaryEntry in new DictionaryEntryEnumerable(dictionary))
    {
      int length = sb.Length;
      if (length <= 524288 /*0x080000*/)
      {
        if (!this.AppendXmlPropertyObjectValue(dictionaryEntry.Key?.ToString(), dictionaryEntry.Value, sb, orgLength, objectsInPath, depth))
          sb.Length = length;
      }
      else
        break;
    }
    this.AppendClosingPropertyTag(propNameElement, sb, ignorePropertiesElementName);
  }

  private void AppendXmlObjectPropertyValues(
    string propName,
    ref ObjectReflectionCache.ObjectPropertyList propertyValues,
    StringBuilder sb,
    int orgLength,
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    int depth,
    bool ignorePropertiesElementName = false)
  {
    if (propertyValues.ConvertToString)
    {
      this.AppendXmlPropertyValue(propName, (object) propertyValues.ToString(), sb, orgLength, ignorePropertiesElementName: ignorePropertiesElementName);
    }
    else
    {
      string propNameElement = this.AppendXmlPropertyValue(propName, (object) string.Empty, sb, orgLength, true, ignorePropertiesElementName);
      if (string.IsNullOrEmpty(propNameElement))
        return;
      foreach (ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue in propertyValues)
      {
        int length = sb.Length;
        if (length <= 524288 /*0x080000*/)
        {
          TypeCode typeCode = propertyValue.TypeCode;
          if (typeCode != TypeCode.Object)
          {
            string xmlValueString = XmlHelper.XmlConvertToString((IConvertible) propertyValue.Value, typeCode, true);
            this.AppendXmlPropertyStringValue(propertyValue.Name, xmlValueString, sb, orgLength, ignorePropertiesElementName: ignorePropertiesElementName);
          }
          else if (!this.AppendXmlPropertyObjectValue(propertyValue.Name, propertyValue.Value, sb, orgLength, objectsInPath, depth))
            sb.Length = length;
        }
        else
          break;
      }
      this.AppendClosingPropertyTag(propNameElement, sb, ignorePropertiesElementName);
    }
  }

  private string AppendXmlPropertyValue(
    string propName,
    object propertyValue,
    StringBuilder sb,
    int orgLength,
    bool ignoreValue = false,
    bool ignorePropertiesElementName = false)
  {
    string xmlValueString = ignoreValue ? string.Empty : XmlHelper.XmlConvertToStringSafe(propertyValue);
    return this.AppendXmlPropertyStringValue(propName, xmlValueString, sb, orgLength, ignoreValue, ignorePropertiesElementName);
  }

  private string AppendXmlPropertyStringValue(
    string propName,
    string xmlValueString,
    StringBuilder sb,
    int orgLength,
    bool ignoreValue = false,
    bool ignorePropertiesElementName = false)
  {
    if (string.IsNullOrEmpty(this.PropertiesElementName))
      return string.Empty;
    propName = propName?.Trim();
    if (string.IsNullOrEmpty(propName))
      return string.Empty;
    if (sb.Length == orgLength && !string.IsNullOrEmpty(this.ElementNameInternal))
      this.BeginXmlDocument(sb, this.ElementNameInternal);
    if (this.IndentXml && !string.IsNullOrEmpty(this.ElementNameInternal))
      sb.Append("  ");
    sb.Append('<');
    string propNameElement;
    if (ignorePropertiesElementName)
    {
      propNameElement = XmlHelper.XmlConvertToElementName(propName, true);
      sb.Append(propNameElement);
    }
    else
    {
      if (this._propertiesElementNameHasFormat)
      {
        propNameElement = XmlHelper.XmlConvertToElementName(propName, true);
        sb.AppendFormat(this.PropertiesElementName, (object) propNameElement);
      }
      else
      {
        propNameElement = this.PropertiesElementName;
        sb.Append(this.PropertiesElementName);
      }
      XmlElementBase.RenderAttribute(sb, this.PropertiesElementKeyAttribute, propName);
    }
    if (!ignoreValue)
    {
      if (XmlElementBase.RenderAttribute(sb, this.PropertiesElementValueAttribute, xmlValueString))
      {
        sb.Append("/>");
        if (this.IndentXml)
          sb.AppendLine();
      }
      else
      {
        sb.Append('>');
        XmlHelper.EscapeXmlString(xmlValueString, false, sb);
        this.AppendClosingPropertyTag(propNameElement, sb, ignorePropertiesElementName);
      }
    }
    else
    {
      sb.Append('>');
      if (this.IndentXml)
        sb.AppendLine();
    }
    return propNameElement;
  }

  private void AppendClosingPropertyTag(
    string propNameElement,
    StringBuilder sb,
    bool ignorePropertiesElementName = false)
  {
    sb.Append("</");
    if (ignorePropertiesElementName)
      sb.Append(propNameElement);
    else
      sb.AppendFormat(this.PropertiesElementName, (object) propNameElement);
    sb.Append('>');
    if (!this.IndentXml)
      return;
    sb.AppendLine();
  }

  private static bool RenderAttribute(StringBuilder sb, string attributeName, string value)
  {
    if (string.IsNullOrEmpty(attributeName))
      return false;
    sb.Append(' ');
    sb.Append(attributeName);
    sb.Append("=\"");
    XmlHelper.EscapeXmlString(value, true, sb);
    sb.Append('"');
    return true;
  }

  private bool RenderAppendXmlElementValue(
    XmlElementBase xmlElement,
    LogEventInfo logEvent,
    StringBuilder sb,
    bool beginXmlDocument)
  {
    if (string.IsNullOrEmpty(xmlElement.ElementNameInternal))
      return false;
    if (beginXmlDocument && !string.IsNullOrEmpty(this.ElementNameInternal))
      this.BeginXmlDocument(sb, this.ElementNameInternal);
    if (this.IndentXml && !string.IsNullOrEmpty(this.ElementNameInternal))
      sb.Append("  ");
    int length = sb.Length;
    xmlElement.RenderAppendBuilder(logEvent, sb);
    if (sb.Length == length && !xmlElement.IncludeEmptyValue)
      return false;
    if (this.IndentXml)
      sb.AppendLine();
    return true;
  }

  private bool RenderAppendXmlAttributeValue(
    XmlAttribute xmlAttribute,
    LogEventInfo logEvent,
    StringBuilder sb,
    bool beginXmlDocument)
  {
    string name = xmlAttribute.Name;
    if (string.IsNullOrEmpty(name))
      return false;
    if (beginXmlDocument)
    {
      sb.Append('<');
      sb.Append(this.ElementNameInternal);
    }
    sb.Append(' ');
    sb.Append(name);
    sb.Append("=\"");
    int length = sb.Length;
    xmlAttribute.LayoutWrapper.RenderAppendBuilder(logEvent, sb);
    if (sb.Length == length && !xmlAttribute.IncludeEmptyValue)
      return false;
    sb.Append('"');
    return true;
  }

  private void BeginXmlDocument(StringBuilder sb, string elementName)
  {
    XmlElementBase.RenderStartElement(sb, elementName);
    if (!this.IndentXml)
      return;
    sb.AppendLine();
  }

  private void EndXmlDocument(StringBuilder sb, string elementName)
  {
    XmlElementBase.RenderEndElement(sb, elementName);
  }

  public override string ToString()
  {
    if (this.Elements.Count > 0)
      return this.ToStringWithNestedItems<XmlElement>(this.Elements, (Func<XmlElement, string>) (l => l.ToString()));
    if (this.Attributes.Count > 0)
      return this.ToStringWithNestedItems<XmlAttribute>(this.Attributes, (Func<XmlAttribute, string>) (a => "Attributes:" + a.Name));
    if (this.ElementNameInternal == null)
      return this.GetType().Name;
    return this.ToStringWithNestedItems<XmlElementBase>((IList<XmlElementBase>) new XmlElementBase[1]
    {
      this
    }, (Func<XmlElementBase, string>) (n => "Element:" + n.ElementNameInternal));
  }

  private static void RenderSelfClosingElement(StringBuilder target, string elementName)
  {
    target.Append('<');
    target.Append(elementName);
    target.Append("/>");
  }

  private static void RenderStartElement(StringBuilder sb, string elementName)
  {
    sb.Append('<');
    sb.Append(elementName);
    sb.Append('>');
  }

  private static void RenderEndElement(StringBuilder sb, string elementName)
  {
    sb.Append("</");
    sb.Append(elementName);
    sb.Append('>');
  }
}

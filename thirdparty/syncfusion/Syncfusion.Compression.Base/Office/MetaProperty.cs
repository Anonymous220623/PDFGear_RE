// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MetaProperty
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Office;

public class MetaProperty
{
  private string m_id;
  private string m_name;
  private byte m_bFlags;
  private object m_value;
  private MetaPropertyType m_MetaPropertyType;
  private object m_ownerBase;

  public string Id => this.m_id;

  public bool IsReadOnly
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    internal set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public bool IsRequired
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    internal set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public string DisplayName => this.m_name;

  public MetaPropertyType Type => this.m_MetaPropertyType;

  public object Value
  {
    get => this.m_value;
    set
    {
      if (!this.ChecksValidMetaValue(ref value))
        return;
      this.m_value = value;
      this.SetValueInXmlDocument();
    }
  }

  internal MetaProperties Parent
  {
    get => (MetaProperties) this.m_ownerBase;
    set => this.m_ownerBase = (object) value;
  }

  private void MapContentTypeProperties(
    string id,
    string nillable,
    string isreadonly,
    string displayName,
    string type,
    object value,
    MetaProperty metaProperty)
  {
    metaProperty.m_id = id;
    metaProperty.m_name = displayName;
    metaProperty.IsRequired = !bool.Parse(nillable);
    metaProperty.IsReadOnly = bool.Parse(isreadonly);
    metaProperty.m_name = displayName;
    foreach (MetaPropertyType metaPropertyType in Enum.GetValues(typeof (MetaPropertyType)))
    {
      if (type == "URL")
        type = "Url";
      if (type == metaPropertyType.ToString((IFormatProvider) CultureInfo.InvariantCulture))
      {
        metaProperty.m_MetaPropertyType = metaPropertyType;
        break;
      }
    }
    metaProperty.m_value = this.GetValueAsPerMetaPropertyType(metaProperty.m_MetaPropertyType, value);
  }

  private void ParseMetaProperty(
    XmlNodeList nodes,
    string id,
    object value,
    MetaProperty property)
  {
    string nillable = "";
    string displayName = "";
    string isreadonly = "";
    string type = "";
    bool flag = false;
    for (int index = 0; index < nodes.Count; ++index)
    {
      foreach (XmlNode childNode1 in nodes.Item(index).ChildNodes)
      {
        if (childNode1.LocalName == "element")
        {
          XmlAttributeCollection attributes = childNode1.Attributes;
          if (attributes["name"] != null && attributes["name"].Value == id)
          {
            if (attributes["nillable"] != null)
              nillable = attributes["nillable"].Value;
            if (attributes["ma:displayName"] != null)
              displayName = attributes["ma:displayName"].Value;
            if (attributes["ma:readOnly"] != null)
              isreadonly = attributes["ma:readOnly"].Value;
            foreach (XmlNode childNode2 in childNode1.ChildNodes)
            {
              if (childNode2.LocalName == "simpleType" || childNode2.LocalName == "complexType")
              {
                foreach (XmlNode childNode3 in childNode2.ChildNodes)
                {
                  if (childNode3.LocalName == "restriction")
                  {
                    if (childNode3.Attributes["base"] != null)
                      type = childNode3.Attributes["base"].Value;
                    type = type.Substring(4);
                  }
                  else if (childNode3.LocalName == "complexContent")
                  {
                    foreach (XmlNode childNode4 in childNode3.ChildNodes)
                    {
                      if (childNode4.LocalName == "extension")
                      {
                        if (childNode4.Attributes["base"] != null)
                          type = childNode4.Attributes["base"].Value;
                        type = type.Substring(4);
                      }
                    }
                  }
                }
              }
            }
            if (string.IsNullOrEmpty(nillable))
              nillable = "false";
            if (string.IsNullOrEmpty(isreadonly))
              isreadonly = "false";
            this.MapContentTypeProperties(id, nillable, isreadonly, displayName, type, value, property);
            flag = true;
          }
        }
        if (flag)
          break;
      }
      if (flag)
        break;
    }
  }

  internal MetaProperties ParseMetaProperty(
    XmlElement contentTypeSchema,
    XmlDocument contentTypeSchemaProperties)
  {
    MetaProperties metaProperty1 = new MetaProperties();
    metaProperty1.m_contentTypeSchemaProperties = contentTypeSchemaProperties;
    if (contentTypeSchemaProperties.DocumentElement != null && contentTypeSchemaProperties.DocumentElement.FirstChild != null && contentTypeSchemaProperties.DocumentElement.FirstChild.LocalName == "documentManagement")
    {
      foreach (XmlNode childNode in contentTypeSchemaProperties.DocumentElement.FirstChild.ChildNodes)
      {
        MetaProperty metaProperty2 = new MetaProperty();
        object obj;
        if (childNode.HasChildNodes && childNode.ChildNodes.Count > 1)
        {
          XmlNodeList childNodes = childNode.ChildNodes;
          List<string> stringList = new List<string>();
          foreach (XmlNode xmlNode in childNodes)
            stringList.Add(xmlNode.InnerText);
          obj = (object) stringList.ToArray();
        }
        else if (childNode.HasChildNodes && childNode.ChildNodes.Count == 1 && childNode.FirstChild.NodeType != XmlNodeType.Text)
        {
          XmlNodeList childNodes = childNode.FirstChild.ChildNodes;
          if (childNodes.Count == 0 || childNodes.Count == 1 && childNodes.Item(0).NodeType == XmlNodeType.Text)
          {
            obj = (object) childNode.FirstChild.InnerText;
          }
          else
          {
            List<string> stringList = new List<string>();
            foreach (XmlNode xmlNode in childNodes)
            {
              if (xmlNode is XmlElement && !((XmlElement) xmlNode).IsEmpty)
                stringList.Add(xmlNode.InnerText);
            }
            obj = (object) stringList.ToArray();
          }
        }
        else
          obj = (object) childNode.InnerText;
        this.ParseMetaProperty(contentTypeSchema.ChildNodes, childNode.Name, obj, metaProperty2);
        metaProperty1.Add(metaProperty2);
      }
    }
    return metaProperty1;
  }

  private bool ChecksValidMetaValue(ref object value)
  {
    if (this.m_MetaPropertyType == MetaPropertyType.Unknown || this.m_MetaPropertyType == MetaPropertyType.Choice || this.m_MetaPropertyType == MetaPropertyType.Text || this.m_MetaPropertyType == MetaPropertyType.Note)
      return this.ValidateTextNoteChoiceUnknownMetaType(ref value);
    if (this.m_MetaPropertyType == MetaPropertyType.Boolean)
      return this.ValidateBooleanMetaType(ref value);
    if (this.m_MetaPropertyType == MetaPropertyType.DateTime)
      return this.ValidateDateTimeMetaType(ref value);
    if (this.m_MetaPropertyType == MetaPropertyType.Number || this.m_MetaPropertyType == MetaPropertyType.Currency)
      return this.ValidateNumberAndCurrencyMetaType(ref value);
    if (this.m_MetaPropertyType == MetaPropertyType.User)
      return this.ValidateUserMetaType(ref value);
    if (this.m_MetaPropertyType == MetaPropertyType.Url)
      return this.ValidateUrlMetaType(ref value);
    return this.m_MetaPropertyType == MetaPropertyType.Lookup && this.ValidateLookupMetaType(ref value);
  }

  private object GetValueAsPerMetaPropertyType(MetaPropertyType type, object value)
  {
    switch (type)
    {
      case MetaPropertyType.Unknown:
      case MetaPropertyType.Choice:
      case MetaPropertyType.Lookup:
      case MetaPropertyType.Note:
      case MetaPropertyType.Text:
      case MetaPropertyType.Url:
      case MetaPropertyType.User:
        return value != (object) "" ? value : (object) null;
      case MetaPropertyType.Boolean:
        if (!string.IsNullOrEmpty(value.ToString()))
          return (object) Convert.ToBoolean(value);
        break;
      case MetaPropertyType.Currency:
      case MetaPropertyType.Number:
        int result1;
        if (int.TryParse(value.ToString(), out result1) && !string.IsNullOrEmpty(value.ToString()))
          return (object) result1;
        long result2;
        if (long.TryParse(value.ToString(), out result2) && !string.IsNullOrEmpty(value.ToString()))
          return (object) result2;
        double result3;
        if (double.TryParse(value.ToString(), out result3) && !string.IsNullOrEmpty(value.ToString()))
          return (object) result3;
        return value != (object) "" ? value : (object) null;
      case MetaPropertyType.DateTime:
        if (!string.IsNullOrEmpty(value.ToString()))
          return (object) Convert.ToDateTime(value);
        break;
    }
    return (object) null;
  }

  private void SetValueInXmlDocument()
  {
    if (this.Parent.m_contentTypeSchemaProperties == null || this.Parent.m_contentTypeSchemaProperties.DocumentElement == null || this.Parent.m_contentTypeSchemaProperties.DocumentElement.FirstChild == null || !(this.Parent.m_contentTypeSchemaProperties.DocumentElement.FirstChild.LocalName == "documentManagement"))
      return;
    foreach (XmlNode childNode in this.Parent.m_contentTypeSchemaProperties.DocumentElement.FirstChild.ChildNodes)
    {
      if (!(childNode.Name != this.m_id))
      {
        if (this.m_MetaPropertyType == MetaPropertyType.DateTime)
          childNode.InnerText = this.m_value == null ? Convert.ToString(this.m_value) : ((DateTime) this.m_value).ToUniversalTime().ToString("s", (IFormatProvider) CultureInfo.InvariantCulture);
        else if (this.m_MetaPropertyType == MetaPropertyType.Url)
        {
          if (this.m_value != null)
          {
            int index = 0;
            foreach (string str in (string[]) this.m_value)
            {
              childNode.ChildNodes.Item(index).InnerText = str;
              ++index;
            }
          }
        }
        else if (this.m_MetaPropertyType == MetaPropertyType.User)
        {
          if (this.m_value != null)
          {
            int index = 0;
            string[] strArray = (string[]) this.m_value;
            if (childNode.HasChildNodes && childNode.FirstChild != null)
            {
              XmlNodeList childNodes = childNode.FirstChild.ChildNodes;
              foreach (string str in strArray)
              {
                childNodes.Item(index).InnerText = str;
                ++index;
              }
            }
          }
        }
        else if (this.m_MetaPropertyType == MetaPropertyType.Unknown || this.m_MetaPropertyType == MetaPropertyType.Choice || this.m_MetaPropertyType == MetaPropertyType.Text || this.m_MetaPropertyType == MetaPropertyType.Note || this.m_MetaPropertyType == MetaPropertyType.Number || this.m_MetaPropertyType == MetaPropertyType.Currency || this.m_MetaPropertyType == MetaPropertyType.Lookup)
          childNode.InnerText = Convert.ToString(this.m_value, (IFormatProvider) CultureInfo.InvariantCulture);
        else if (this.m_MetaPropertyType == MetaPropertyType.Boolean)
          childNode.InnerText = Convert.ToString(this.m_value, (IFormatProvider) CultureInfo.InvariantCulture).ToLower();
      }
    }
  }

  private bool ValidateBooleanMetaType(ref object value)
  {
    if ((value == null || value.GetType() == typeof (bool)) && !this.IsReadOnly)
      return true;
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    throw new Exception("Data is invalid");
  }

  private bool ValidateLookupMetaType(ref object value)
  {
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    if (value == null || value.GetType() == typeof (bool) || value.GetType() == typeof (DateTime))
    {
      if (value != null)
        value = (object) Convert.ToString(-1, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    else if (value != null && value.GetType() == typeof (char))
      value = (object) ((int) (char) value).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    else if (value == (object) "")
      value = (object) null;
    else if (value.GetType() != typeof (double))
      value = (object) Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.GetType() == typeof (double))
      value = (object) ((int) Math.Round((double) value)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    return true;
  }

  private bool ValidateUserMetaType(ref object value)
  {
    if (value != null && value.GetType() == typeof (string[]) && ((string[]) value).Length <= 3 && !this.IsReadOnly)
    {
      if (((string[]) value).Length == 0)
        value = (object) null;
      else if (((string[]) value).Length == 1)
        value = (object) new string[2]
        {
          ((string[]) value)[0],
          ""
        };
      return true;
    }
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    if (value != null && value != (object) "")
      throw new Exception("Type mismatch");
    return false;
  }

  private bool ValidateUrlMetaType(ref object value)
  {
    if (value != null && value.GetType() == typeof (string) && !this.IsReadOnly)
    {
      if (!((string) value).Contains("http://") && !((string) value).Contains("https://"))
        value = (object) Uri.EscapeUriString(value.ToString());
      if (value.ToString().Contains("http://") || value.ToString().Contains("https://"))
      {
        value = (object) new string[2]
        {
          value.ToString(),
          value.ToString()
        };
        return true;
      }
      value = (object) new string[2]
      {
        "http://" + value,
        "http://" + value
      };
      return true;
    }
    if (value != null && value.GetType() == typeof (string[]) && !this.IsReadOnly && ((string[]) value).Length == 2)
    {
      string[] strArray = (string[]) value;
      if (strArray[0] != null && !strArray[0].Contains("http://") && !strArray[0].Contains("https://"))
      {
        strArray[0] = Uri.EscapeUriString(strArray[0].ToString());
        value = (object) new string[2]
        {
          "http://" + strArray[0],
          strArray[1]
        };
        return true;
      }
      value = (object) new string[2]
      {
        strArray[0],
        strArray[1]
      };
      return true;
    }
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    if (value != null)
      throw new Exception("Type mismatch");
    return false;
  }

  private bool ValidateDateTimeMetaType(ref object value)
  {
    if ((value == null || value.GetType() == typeof (DateTime) || value == (object) "") && !this.IsReadOnly)
    {
      if (value == (object) "")
        value = (object) null;
      return true;
    }
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    throw new Exception("Data is invalid");
  }

  private bool ValidateNumberAndCurrencyMetaType(ref object value)
  {
    if (!this.IsReadOnly)
    {
      if (value != null && value.GetType() == typeof (DateTime))
        value = (object) ((DateTime) value).ToUniversalTime().ToString("s", (IFormatProvider) CultureInfo.InvariantCulture);
      else if (value != null && value.GetType() == typeof (bool))
        value = (object) value.ToString().ToLower();
      else if (value != null && value.GetType() == typeof (char))
        value = (object) (int) (char) value;
      else if (value == (object) "")
        value = (object) null;
      return true;
    }
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    return false;
  }

  private bool ValidateTextNoteChoiceUnknownMetaType(ref object value)
  {
    if (this.IsReadOnly)
      throw new Exception("The specified file is read only.");
    if (value != null && value.GetType() == typeof (DateTime))
      value = (object) ((DateTime) value).ToUniversalTime().ToString("s", (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value != null && value.GetType() == typeof (bool))
      value = (object) value.ToString().ToLower();
    else if (value != null && value.GetType() == typeof (char))
      value = (object) ((int) (char) value).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    else if (value != null && value.GetType() != typeof (string))
      value = (object) Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value == (object) "")
      value = (object) null;
    return true;
  }
}

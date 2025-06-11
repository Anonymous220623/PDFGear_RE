// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpUtils
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

internal class XmpUtils
{
  private const string c_False = "False";
  private const string c_True = "True";
  private const string c_realPattern = "^[+-]?[\\d]+([.]?[\\d])*$";
  private const string c_dateFormat = "yyyy-MM-dd'T'HH:mm:ss.ffzzz";

  private XmpUtils() => throw new NotImplementedException();

  public static void SetTextValue(XmlElement parent, string value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    XmpUtils.ClearChildren((XmlNode) parent);
    XmlText textNode = parent.OwnerDocument.CreateTextNode(value);
    parent.AppendChild((XmlNode) textNode);
  }

  public static void SetBoolValue(XmlElement parent, bool value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    string str = value ? "True" : "False";
    XmpUtils.SetTextValue(parent, str);
  }

  public static bool GetBoolValue(string value)
  {
    int num;
    switch (value)
    {
      case null:
        throw new ArgumentNullException(nameof (value));
      case "True":
        num = 1;
        break;
      default:
        num = 0;
        break;
    }
    return num != 0;
  }

  public static void SetRealValue(XmlElement parent, float value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    string str = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    XmpUtils.SetTextValue(parent, str);
  }

  public static float GetRealValue(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    double result = 0.0;
    double.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    return (float) result;
  }

  public static void SetIntValue(XmlElement parent, int value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    string str = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    XmpUtils.SetTextValue(parent, str);
  }

  public static int GetIntValue(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    double result = 0.0;
    double.TryParse(value, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    return (int) result;
  }

  public static void SetUriValue(XmlElement parent, Uri value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    string str = value.ToString();
    XmpUtils.SetTextValue(parent, str);
  }

  public static Uri GetUriValue(string value)
  {
    return value != null ? new Uri(value) : throw new ArgumentNullException(nameof (value));
  }

  public static void SetDateTimeValue(XmlElement parent, DateTime value)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    string str = value.ToString("yyyy-MM-dd'T'HH:mm:ss.ffzzz");
    XmpUtils.SetTextValue(parent, str);
  }

  public static DateTime GetDateTimeValue(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    DateTime result = DateTime.Now;
    if (value != string.Empty)
    {
      try
      {
        string format = "yyyyMMddHHmmss";
        if (value.Length > 14)
        {
          string[] strArray = new string[4]
          {
            "-",
            "T",
            ":",
            "Z"
          };
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (value.Contains(strArray[index]))
              value = value.Replace(strArray[index], "");
          }
        }
        if (value.Length < 10)
        {
          format = "yyyyMMdd";
          value = value.Substring(0, 8);
        }
        else if (value.Length > 9 && value.Length < 12)
        {
          format = "yyyyMMddHH";
          value = value.Substring(0, 10);
        }
        else if (value.Length > 11 && value.Length < 14)
        {
          format = "yyyyMMddHHmm";
          value = value.Substring(0, 12);
        }
        else if (value.Length > 14)
          value = value.Substring(0, 14);
        DateTime.TryParseExact(value, format, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result);
      }
      catch
      {
      }
    }
    return result;
  }

  public static void SetXmlValue(XmlElement parent, XmlElement child)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (child == null)
      throw new ArgumentNullException(nameof (child));
    XmpUtils.ClearChildren((XmlNode) parent);
    parent.AppendChild((XmlNode) child);
  }

  private static void ClearChildren(XmlNode node)
  {
    XmlNodeList xmlNodeList = node != null ? node.ChildNodes : throw new ArgumentNullException(nameof (node));
    int num = 0;
    for (int count = xmlNodeList.Count; num < count; ++num)
    {
      XmlNode oldChild = xmlNodeList[0];
      node.RemoveChild(oldChild);
    }
  }
}

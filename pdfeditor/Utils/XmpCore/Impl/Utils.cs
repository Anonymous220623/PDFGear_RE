// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.Utils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace XmpCore.Impl;

public static class Utils
{
  public const int UuidSegmentCount = 4;
  public const int UuidLength = 36;
  private static readonly bool[] _xmlNameStartChars;
  private static readonly bool[] _xmlNameChars;
  private static readonly HashSet<string> EXTERNAL_XMPDM_PROPS = new HashSet<string>()
  {
    "xmpDM:album",
    "xmpDM:altTapeName",
    "xmpDM:altTimecode",
    "xmpDM:artist",
    "xmpDM:cameraAngle",
    "xmpDM:cameraLabel",
    "xmpDM:cameraModel",
    "xmpDM:cameraMove",
    "xmpDM:client",
    "xmpDM:comment",
    "xmpDM:composer",
    "xmpDM:director",
    "xmpDM:directorPhotography",
    "xmpDM:engineer",
    "xmpDM:genre",
    "xmpDM:good",
    "xmpDM:instrument",
    "xmpDM:logComment",
    "xmpDM:projectName",
    "xmpDM:releaseDate",
    "xmpDM:scene",
    "xmpDM:shotDate",
    "xmpDM:shotDay",
    "xmpDM:shotLocation",
    "xmpDM:shotName",
    "xmpDM:shotNumber",
    "xmpDM:shotSize",
    "xmpDM:speakerPlacement",
    "xmpDM:takeNumber",
    "xmpDM:tapeName",
    "xmpDM:trackNumber",
    "xmpDM:videoAlphaMode",
    "xmpDM:videoAlphaPremultipleColor"
  };
  private static readonly char[] BasicEscapeCharacters = new char[3]
  {
    '<',
    '>',
    '&'
  };
  private static readonly char[] WhiteSpaceEscapeCharacters = new char[3]
  {
    '\t',
    '\n',
    '\r'
  };

  static Utils()
  {
    Utils._xmlNameChars = new bool[256 /*0x0100*/];
    Utils._xmlNameStartChars = new bool[256 /*0x0100*/];
    for (char minValue = char.MinValue; (int) minValue < Utils._xmlNameChars.Length; ++minValue)
    {
      bool flag = minValue == ':' || 'A' <= minValue && minValue <= 'Z' || minValue == '_' || 'a' <= minValue && minValue <= 'z' || 'À' <= minValue && minValue <= 'Ö' || 'Ø' <= minValue && minValue <= 'ö' || 'ø' <= minValue && minValue <= 'ÿ';
      Utils._xmlNameStartChars[(int) minValue] = flag;
      Utils._xmlNameChars[(int) minValue] = flag || minValue == '-' || minValue == '.' || '0' <= minValue && minValue <= '9' || minValue == '·';
    }
  }

  public static string NormalizeLangValue(string value)
  {
    if (value == "x-default")
      return value;
    int num = 1;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (char c in value)
    {
      switch (c)
      {
        case ' ':
          continue;
        case '-':
        case '_':
          stringBuilder.Append('-');
          ++num;
          continue;
        default:
          stringBuilder.Append(num != 2 ? char.ToLower(c) : char.ToUpper(c));
          continue;
      }
    }
    return stringBuilder.ToString();
  }

  internal static void SplitNameAndValue(string selector, out string name, out string value)
  {
    int num1 = selector.IndexOf('=');
    int num2 = 1;
    if (selector[num2] == '?')
      ++num2;
    name = selector.Substring(num2, num1 - num2);
    int index1 = num1 + 1;
    char ch = selector[index1];
    int index2 = index1 + 1;
    int num3 = selector.Length - 2;
    StringBuilder stringBuilder = new StringBuilder(num3 - num1);
    while (index2 < num3)
    {
      stringBuilder.Append(selector[index2]);
      ++index2;
      if ((int) selector[index2] == (int) ch)
        ++index2;
    }
    value = stringBuilder.ToString();
  }

  internal static bool IsInternalProperty(string schema, string prop)
  {
    if (schema != null)
    {
      switch (schema.Length)
      {
        case 28:
          switch (schema[20])
          {
            case 'p':
              if (schema == "http://ns.adobe.com/pdf/1.3/")
                return prop == "pdf:BaseURL" || prop == "pdf:Creator" || prop == "pdf:ModDate" || prop == "pdf:PDFVersion" || prop == "pdf:Producer";
              goto label_45;
            case 'x':
              if (schema == "http://ns.adobe.com/xap/1.0/")
                return prop == "xmp:BaseURL" || prop == "xmp:CreatorTool" || prop == "xmp:Format" || prop == "xmp:Locale" || prop == "xmp:MetadataDate" || "xmp:ModifyDate" == prop;
              goto label_45;
            default:
              goto label_45;
          }
        case 29:
          switch (schema[20])
          {
            case 'e':
              if (schema == "http://ns.adobe.com/exif/1.0/")
                return prop != "exif:UserComment";
              goto label_45;
            case 't':
              if (schema == "http://ns.adobe.com/tiff/1.0/" && prop != "tiff:ImageDescription" && prop != "tiff:Artist")
                return prop != "tiff:Copyright";
              goto label_45;
            default:
              goto label_45;
          }
        case 30:
          switch (schema[28])
          {
            case 'g':
              if (schema == "http://ns.adobe.com/xap/1.0/g/")
                break;
              goto label_45;
            case 't':
              if (schema == "http://ns.adobe.com/xap/1.0/t/")
                break;
              goto label_45;
            default:
              goto label_45;
          }
          break;
        case 31 /*0x1F*/:
          if (schema == "http://ns.adobe.com/xap/1.0/mm/")
            break;
          goto label_45;
        case 32 /*0x20*/:
          if (schema == "http://purl.org/dc/elements/1.1/")
            return prop == "dc:format" || prop == "dc:language";
          goto label_45;
        case 33:
          switch (schema[20])
          {
            case 'b':
              if (schema == "http://ns.adobe.com/bwf/bext/1.0/")
                return prop == "bext:version";
              goto label_45;
            case 'e':
              if (schema == "http://ns.adobe.com/exif/1.0/aux/")
                break;
              goto label_45;
            case 'x':
              if (schema == "http://ns.adobe.com/xap/1.0/t/pg/")
                break;
              goto label_45;
            default:
              goto label_45;
          }
          break;
        case 34:
          switch (schema[20])
          {
            case 'p':
              if (schema == "http://ns.adobe.com/photoshop/1.0/")
                return prop == "photoshop:ICCProfile" || prop == "photoshop:TextLayers";
              goto label_45;
            case 'x':
              if (schema == "http://ns.adobe.com/xap/1.0/g/img/")
                break;
              goto label_45;
            default:
              goto label_45;
          }
          break;
        case 35:
          switch (schema[20])
          {
            case 'S':
              if (schema == "http://ns.adobe.com/StockPhoto/1.0/")
                break;
              goto label_45;
            case 'x':
              if (schema == "http://ns.adobe.com/xmp/1.0/Script/" && prop != "xmpScript:action" && prop != "xmpScript:character" && prop != "xmpScript:dialog" && prop != "xmpScript:sceneSetting")
                return prop != "xmpScript:sceneTimeOfDay";
              goto label_45;
            default:
              goto label_45;
          }
          break;
        case 39:
          if (schema == "http://ns.adobe.com/xap/1.0/sType/Font#")
            break;
          goto label_45;
        case 41:
          if (schema == "http://ns.adobe.com/xmp/1.0/DynamicMedia/")
            return !Utils.EXTERNAL_XMPDM_PROPS.Contains(prop);
          goto label_45;
        case 44:
          if (schema == "http://ns.adobe.com/camera-raw-settings/1.0/")
            return true;
          goto label_45;
        default:
          goto label_45;
      }
      return true;
    }
label_45:
    return false;
  }

  internal static bool CheckUuidFormat(string uuid)
  {
    if (uuid == null)
      return false;
    bool flag = true;
    int num = 0;
    int index;
    for (index = 0; index < uuid.Length; ++index)
    {
      if (uuid[index] == '-')
      {
        ++num;
        flag = flag && (index == 8 || index == 13 || index == 18 || index == 23);
      }
    }
    return flag && 4 == num && 36 == index;
  }

  public static bool IsXmlName(string name)
  {
    if (name.Length > 0 && !Utils.IsNameStartChar(name[0]))
      return false;
    for (int index = 1; index < name.Length; ++index)
    {
      if (!Utils.IsNameChar(name[index]))
        return false;
    }
    return true;
  }

  public static bool IsXmlNameNs(string name)
  {
    if (name.Length > 0 && (!Utils.IsNameStartChar(name[0]) || name[0] == ':'))
      return false;
    for (int index = 1; index < name.Length; ++index)
    {
      if (!Utils.IsNameChar(name[index]) || name[index] == ':')
        return false;
    }
    return true;
  }

  internal static bool IsControlChar(char c)
  {
    return (c <= '\u001F' || c == '\u007F') && c != '\t' && c != '\n' && c != '\r';
  }

  public static string EscapeXml(string value, bool forAttribute, bool escapeWhitespaces)
  {
    bool flag = value.IndexOfAny(Utils.BasicEscapeCharacters) != -1;
    if (escapeWhitespaces)
      flag |= value.IndexOfAny(Utils.WhiteSpaceEscapeCharacters) != -1;
    if (forAttribute)
      flag |= value.IndexOf('"') != -1;
    if (!flag)
      return value;
    StringBuilder stringBuilder = new StringBuilder(value.Length * 4 / 3);
    foreach (char ch in value)
    {
      if (!escapeWhitespaces || ch != '\t' && ch != '\n' && ch != '\r')
      {
        switch (ch)
        {
          case '"':
            stringBuilder.Append(forAttribute ? "&quot;" : "\"");
            continue;
          case '&':
            stringBuilder.Append("&amp;");
            continue;
          case '<':
            stringBuilder.Append("&lt;");
            continue;
          case '>':
            stringBuilder.Append("&gt;");
            continue;
          default:
            stringBuilder.Append(ch);
            continue;
        }
      }
      else
        stringBuilder.AppendFormat("&#x{0:X};", (object) (int) ch);
    }
    return stringBuilder.ToString();
  }

  internal static string RemoveControlChars(string value)
  {
    StringBuilder stringBuilder = new StringBuilder(value);
    for (int index = 0; index < stringBuilder.Length; ++index)
    {
      if (Utils.IsControlChar(stringBuilder[index]))
        stringBuilder[index] = ' ';
    }
    return stringBuilder.ToString();
  }

  private static bool IsNameStartChar(char ch)
  {
    if (ch <= 'ÿ' && Utils._xmlNameStartChars[(int) ch] || ch >= 'Ā' && ch <= '˿' || ch >= 'Ͱ' && ch <= 'ͽ' || ch >= 'Ϳ' && ch <= '\u1FFF' || ch >= '\u200C' && ch <= '\u200D' || ch >= '\u2070' && ch <= '\u218F' || ch >= 'Ⰰ' && ch <= '\u2FEF' || ch >= '、' && ch <= '\uD7FF' || ch >= '豈' && ch <= '\uFDCF')
      return true;
    return ch >= 'ﷰ' && ch <= '�';
  }

  private static bool IsNameChar(char ch)
  {
    if (ch <= 'ÿ' && Utils._xmlNameChars[(int) ch] || Utils.IsNameStartChar(ch) || ch >= '̀' && ch <= 'ͯ')
      return true;
    return ch >= '‿' && ch <= '⁀';
  }

  public static bool IsNullOrWhiteSpace(string value) => string.IsNullOrWhiteSpace(value);
}

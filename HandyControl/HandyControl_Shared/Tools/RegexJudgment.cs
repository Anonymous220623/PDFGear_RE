// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.RegexJudgment
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace HandyControl.Tools;

public static class RegexJudgment
{
  private static readonly RegexPatterns RegexPatterns = new RegexPatterns();

  public static bool IsKindOf(this string str, string pattern) => Regex.IsMatch(str, pattern);

  public static bool IsKindOf(this string text, TextType textType)
  {
    return textType == TextType.Common || Regex.IsMatch(text, RegexJudgment.RegexPatterns.GetValue(Enum.GetName(typeof (TextType), (object) textType) + "Pattern").ToString());
  }

  public static bool IsEmail(this string email)
  {
    return Regex.IsMatch(email, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
  }

  public static bool IsIp(this string ip, IpType ipType)
  {
    bool flag;
    switch (ipType)
    {
      case IpType.A:
        flag = Regex.IsMatch(ip, "^(12[0-6]|1[0-1]\\d|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
        break;
      case IpType.B:
        flag = Regex.IsMatch(ip, "^(19[0-1]|12[8-9]|1[3-8]\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
        break;
      case IpType.C:
        flag = Regex.IsMatch(ip, "^(19[2-9]|22[0-3]|2[0-1]\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
        break;
      case IpType.D:
        flag = Regex.IsMatch(ip, "^(22[4-9]|23\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
        break;
      case IpType.E:
        flag = Regex.IsMatch(ip, "^(25[0-5]|24\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public static bool IsIp(this string ip)
  {
    return Regex.IsMatch(ip, "^(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)$");
  }

  public static bool IsChinese(this string str) => Regex.IsMatch(str, "^[\\u4e00-\\u9fa5]$");

  public static bool IsUrl(this string str)
  {
    return Regex.IsMatch(str, "((http|ftp|https)://)(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\\&%_\\./-~-]*)?");
  }
}

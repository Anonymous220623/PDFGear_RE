// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Localization.SR
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Resources;

#nullable disable
namespace Syncfusion.XlsIO.Localization;

internal sealed class SR
{
  internal const string Testing = "Testing";
  private ResourceManager resources;
  private static SR loader;

  private SR() => this.resources = new ResourceManager(this.GetType());

  private static SR GetLoader()
  {
    lock (typeof (SR))
    {
      if (SR.loader == null)
        SR.loader = new SR();
      return SR.loader;
    }
  }

  public static string GetString(CultureInfo culture, string name, params object[] args)
  {
    SR loader = SR.GetLoader();
    if (loader == null)
      return (string) null;
    try
    {
      string format = loader.resources.GetString(name, culture);
      return format != null && args != null && args.Length > 0 ? string.Format(format, args) : format;
    }
    catch (Exception ex)
    {
      return name;
    }
  }

  public static string GetString(string name) => SR.GetString((CultureInfo) null, name);

  public static string GetString(string name, params object[] args)
  {
    return SR.GetString((CultureInfo) null, name, args);
  }

  public static string GetString(CultureInfo culture, string name)
  {
    return SR.GetLoader()?.resources.GetString(name, culture);
  }

  public static object GetObject(CultureInfo culture, string name)
  {
    return SR.GetLoader()?.resources.GetObject(name, culture);
  }

  public static object GetObject(string name) => SR.GetObject((CultureInfo) null, name);

  public static bool GetBoolean(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    boolean = false;
    if (loader == null || !(loader.resources.GetObject(name, culture) is bool boolean))
      ;
    return boolean;
  }

  public static bool GetBoolean(string name) => SR.GetBoolean(name);

  public static byte GetByte(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = (byte) 0;
    if (loader == null || !(loader.resources.GetObject(name, culture) is byte num))
      ;
    return num;
  }

  public static byte GetByte(string name) => SR.GetByte((CultureInfo) null, name);

  public static char GetChar(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    minValue = char.MinValue;
    if (loader == null || !(loader.resources.GetObject(name, culture) is char minValue))
      ;
    return minValue;
  }

  public static char GetChar(string name) => SR.GetChar((CultureInfo) null, name);

  public static double GetDouble(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = 0.0;
    if (loader == null || !(loader.resources.GetObject(name, culture) is double num))
      ;
    return num;
  }

  public static double GetDouble(string name) => SR.GetDouble((CultureInfo) null, name);

  public static float GetFloat(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = 0.0f;
    if (loader == null || !(loader.resources.GetObject(name, culture) is float num))
      ;
    return num;
  }

  public static float GetFloat(string name) => SR.GetFloat((CultureInfo) null, name);

  public static int GetInt(string name) => SR.GetInt((CultureInfo) null, name);

  public static int GetInt(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = 0;
    if (loader == null || !(loader.resources.GetObject(name, culture) is int num))
      ;
    return num;
  }

  public static long GetLong(string name) => SR.GetLong((CultureInfo) null, name);

  public static long GetLong(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = 0L;
    if (loader == null || !(loader.resources.GetObject(name, culture) is long num))
      ;
    return num;
  }

  public static short GetShort(CultureInfo culture, string name)
  {
    SR loader = SR.GetLoader();
    num = (short) 0;
    if (loader == null || !(loader.resources.GetObject(name, culture) is short num))
      ;
    return num;
  }

  public static short GetShort(string name) => SR.GetShort((CultureInfo) null, name);
}

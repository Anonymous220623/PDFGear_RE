// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SR
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal sealed class SR
{
  private SR()
  {
  }

  public static void ReleaseResources()
  {
    ChartLocalizationResourceAccessor.Instance.Resources.ReleaseAllResources();
  }

  public static string GetString(CultureInfo culture, string name, params object[] args)
  {
    return ChartLocalizationResourceAccessor.Instance.GetString(culture, name);
  }

  public static string GetString(string name)
  {
    return ChartLocalizationResourceAccessor.Instance.GetString((CultureInfo) null, name);
  }

  public static string GetString(string name, params object[] args)
  {
    return ChartLocalizationResourceAccessor.Instance.GetString((string) null, (object) name, (object) args);
  }

  public static string GetString(CultureInfo culture, string name)
  {
    return ChartLocalizationResourceAccessor.Instance.GetString(name, (object) culture);
  }

  public static object GetObject(CultureInfo culture, string name)
  {
    return ChartLocalizationResourceAccessor.Instance != null ? ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) : (object) null;
  }

  public static object GetObject(string name) => SR.GetObject((CultureInfo) null, name);

  public static bool GetBoolean(CultureInfo culture, string name)
  {
    boolean = false;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is bool boolean))
      ;
    return boolean;
  }

  public static bool GetBoolean(string name) => SR.GetBoolean(name);

  public static byte GetByte(CultureInfo culture, string name)
  {
    num = (byte) 0;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is byte num))
      ;
    return num;
  }

  public static byte GetByte(string name) => SR.GetByte((CultureInfo) null, name);

  public static char GetChar(CultureInfo culture, string name)
  {
    minValue = char.MinValue;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is char minValue))
      ;
    return minValue;
  }

  public static char GetChar(string name) => SR.GetChar((CultureInfo) null, name);

  public static double GetDouble(CultureInfo culture, string name)
  {
    num = 0.0;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is double num))
      ;
    return num;
  }

  public static double GetDouble(string name) => SR.GetDouble((CultureInfo) null, name);

  public static float GetFloat(CultureInfo culture, string name)
  {
    num = 0.0f;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is float num))
      ;
    return num;
  }

  public static float GetFloat(string name) => SR.GetFloat((CultureInfo) null, name);

  public static int GetInt(string name) => SR.GetInt((CultureInfo) null, name);

  public static int GetInt(CultureInfo culture, string name)
  {
    num = 0;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is int num))
      ;
    return num;
  }

  public static long GetLong(string name) => SR.GetLong((CultureInfo) null, name);

  public static long GetLong(CultureInfo culture, string name)
  {
    num = 0L;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is long num))
      ;
    return num;
  }

  public static short GetShort(CultureInfo culture, string name)
  {
    num = (short) 0;
    if (ChartLocalizationResourceAccessor.Instance == null || !(ChartLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is short num))
      ;
    return num;
  }

  public static short GetShort(string name) => SR.GetShort((CultureInfo) null, name);
}

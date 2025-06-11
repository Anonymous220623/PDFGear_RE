// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Resources.SR
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Globalization;
using System.Reflection;

#nullable disable
namespace Syncfusion.Windows.Shared.Resources;

public sealed class SR
{
  private SR()
  {
  }

  public static void ReleaseResources()
  {
    if (SharedLocalizationResourceAccessor.Instance == null)
      return;
    SharedLocalizationResourceAccessor.Instance.Resources.ReleaseAllResources();
  }

  public static string GetString(CultureInfo culture, string name, params object[] args)
  {
    return SharedLocalizationResourceAccessor.Instance.GetString(culture, name);
  }

  public static string GetString(string name) => SR.GetString((CultureInfo) null, name);

  public static string GetString(string name, params object[] args)
  {
    return SR.GetString((CultureInfo) null, name, args);
  }

  public static string GetString(CultureInfo culture, string name)
  {
    return SharedLocalizationResourceAccessor.Instance.GetString(culture, name);
  }

  public static object GetObject(CultureInfo culture, string name)
  {
    if (SharedLocalizationResourceAccessor.Instance != null)
      SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture);
    return (object) null;
  }

  public static object GetObject(string name) => SR.GetObject((CultureInfo) null, name);

  public static bool GetBoolean(CultureInfo culture, string name)
  {
    boolean = false;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is bool boolean))
      ;
    return boolean;
  }

  public static bool GetBoolean(string name) => SR.GetBoolean((CultureInfo) null, name);

  public static byte GetByte(CultureInfo culture, string name)
  {
    num = (byte) 0;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is byte num))
      ;
    return num;
  }

  public static byte GetByte(string name) => SR.GetByte((CultureInfo) null, name);

  public static char GetChar(CultureInfo culture, string name)
  {
    minValue = char.MinValue;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is char minValue))
      ;
    return minValue;
  }

  public static char GetChar(string name) => SR.GetChar((CultureInfo) null, name);

  public static double GetDouble(CultureInfo culture, string name)
  {
    num = 0.0;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is double num))
      ;
    return num;
  }

  public static double GetDouble(string name) => SR.GetDouble((CultureInfo) null, name);

  public static float GetFloat(CultureInfo culture, string name)
  {
    num = 0.0f;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is float num))
      ;
    return num;
  }

  public static float GetFloat(string name) => SR.GetFloat((CultureInfo) null, name);

  public static int GetInt(string name) => SR.GetInt((CultureInfo) null, name);

  public static int GetInt(CultureInfo culture, string name)
  {
    num = 0;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is int num))
      ;
    return num;
  }

  public static long GetLong(string name) => SR.GetLong((CultureInfo) null, name);

  public static long GetLong(CultureInfo culture, string name)
  {
    num = 0L;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is long num))
      ;
    return num;
  }

  public static short GetShort(CultureInfo culture, string name)
  {
    num = (short) 0;
    if (SharedLocalizationResourceAccessor.Instance == null || !(SharedLocalizationResourceAccessor.Instance.Resources.GetObject(name, culture) is short num))
      ;
    return num;
  }

  public static short GetShort(string name) => SR.GetShort((CultureInfo) null, name);

  public static void SetDefaultNamespace(string nameSpace)
  {
  }

  public static void SetResources(Assembly assembly, string _namespace)
  {
    SharedLocalizationResourceAccessor.Instance.SetResources(assembly, _namespace);
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Utils.DataUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Windows.Shared.Utils;

[DesignTimeVisible(false)]
public static class DataUtils
{
  public static object GetObjectByPath(object obj, string path)
  {
    if (!string.IsNullOrEmpty(path))
    {
      switch (obj)
      {
        case XmlElement _:
          obj = (object) ((XmlElement) obj).GetAttribute(path);
          break;
        case DataRow _:
          obj = ((DataRow) obj)[path];
          break;
        default:
          obj = TypeDescriptor.GetProperties(obj)[path].GetValue(obj);
          break;
      }
    }
    return obj;
  }

  public static double ConvertToDouble(object obj)
  {
    double num;
    try
    {
      num = !(obj is DateTime dateTime) ? Convert.ToDouble(obj, (IFormatProvider) CultureInfo.InvariantCulture) : dateTime.ToOADate();
    }
    catch
    {
      num = double.NaN;
    }
    return num;
  }

  public static double GetDoubleByPath(object obj, string path)
  {
    return DataUtils.ConvertToDouble(DataUtils.GetObjectByPath(obj, path));
  }
}

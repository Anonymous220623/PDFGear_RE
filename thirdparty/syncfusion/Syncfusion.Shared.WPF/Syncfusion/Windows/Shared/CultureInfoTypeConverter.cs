// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CultureInfoTypeConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Syncfusion.Windows.Shared;

public sealed class CultureInfoTypeConverter : TypeConverter
{
  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    string str = value.ToString();
    CultureInfo cultureInfo1 = culture;
    CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
    int index = 0;
    for (int length = cultures.Length; index < length; ++index)
    {
      CultureInfo cultureInfo2 = cultures[index];
      if (cultureInfo2.EnglishName == str || cultureInfo2.Name == str)
      {
        cultureInfo1 = cultureInfo2;
        break;
      }
    }
    return (object) cultureInfo1;
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    return base.ConvertTo(context, culture, value, destinationType);
  }
}

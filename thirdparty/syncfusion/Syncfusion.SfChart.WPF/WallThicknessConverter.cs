// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.WallThicknessConverter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class WallThicknessConverter : TypeConverter
{
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object source)
  {
    if (source == null)
      throw this.GetConvertFromException((object) null);
    return source is string s ? (object) WallThicknessConverter.FromString(s, culture) : (object) new WallThickness(Convert.ToDouble(source, (IFormatProvider) culture));
  }

  internal static WallThickness FromString(string s, CultureInfo cultureInfo)
  {
    string[] array = ((IEnumerable<string>) s.Split(',')).ToArray<string>();
    if (((IEnumerable<string>) array).Count<string>() == 1)
      return !string.IsNullOrEmpty(array[0]) ? new WallThickness(Convert.ToDouble(array[0], (IFormatProvider) CultureInfo.InvariantCulture)) : throw new FormatException("Invalid WallThickness value");
    return ((IEnumerable<string>) array).Count<string>() == 3 ? new WallThickness(Convert.ToDouble(array[0], (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToDouble(array[1], (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToDouble(array[2], (IFormatProvider) CultureInfo.InvariantCulture)) : throw new FormatException("Invalid WallThickness value");
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (destinationType == (Type) null)
      throw new ArgumentNullException(nameof (destinationType));
    return base.ConvertTo(context, culture, value, destinationType);
  }
}

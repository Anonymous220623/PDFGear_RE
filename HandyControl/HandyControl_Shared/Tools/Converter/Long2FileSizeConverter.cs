// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.Long2FileSizeConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Properties.Langs;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class Long2FileSizeConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) Lang.UnknownSize;
    if (!(value is long num))
      return (object) Lang.Unknown;
    if (num < 0L)
      return (object) Lang.UnknownSize;
    if (num < 1024L /*0x0400*/)
      return (object) $"{num} B";
    if (num < 1048576L /*0x100000*/)
      return (object) $"{(double) num / 1024.0:0.00} KB";
    if (num < 1073741824L /*0x40000000*/)
      return (object) $"{(double) num / 1048576.0:0.00} MB";
    if (num < 1099511627776L /*0x010000000000*/)
      return (object) $"{(double) num / 1073741824.0:0.00} GB";
    if (num < 1125899906842624L /*0x04000000000000*/)
      return (object) $"{(double) num / 1099511627776.0:0.00} TB";
    return num < 1152921504606847000L /*0x1000000000000018*/ ? (object) $"{(double) num / 1.12589990684262E+15:0.00} PB" : (object) Lang.TooLarge;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}

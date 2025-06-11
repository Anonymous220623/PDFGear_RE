// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.StringToImageTypeConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.Windows.Shared;

public sealed class StringToImageTypeConverter : TypeConverter
{
  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    BitmapImage bitmapImage = new BitmapImage();
    bitmapImage.BeginInit();
    bitmapImage.UriSource = new Uri((string) value, UriKind.Relative);
    bitmapImage.EndInit();
    return (object) new Image()
    {
      Source = (ImageSource) bitmapImage
    };
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

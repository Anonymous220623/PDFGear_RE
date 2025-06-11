// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.RGBToStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

public class RGBToStringConverter : IMultiValueConverter
{
  private Color color;

  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    float num = 0.0f;
    this.color = (Color) values[0];
    if ((bool) values[1])
    {
      switch (parameter.ToString())
      {
        case "R":
          num = this.color.ScR;
          break;
        case "G":
          num = this.color.ScG;
          break;
        case "B":
          num = this.color.ScB;
          break;
        case "A":
          num = this.color.ScA;
          break;
      }
      return (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicRGB || (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicHSV ? (object) num.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture) : (object) (double) num;
    }
    switch (parameter.ToString())
    {
      case "R":
        num = (float) this.color.R;
        break;
      case "G":
        num = (float) this.color.G;
        break;
      case "B":
        num = (float) this.color.B;
        break;
      case "A":
        num = (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicRGB || (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicHSV ? (float) this.color.A : (float) ((double) this.color.A / (double) byte.MaxValue * 100.0);
        break;
    }
    return (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicRGB || (ColorSelectionMode) values[2] == ColorSelectionMode.ClassicHSV ? (object) ((int) num).ToString() : (object) (double) num;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    object[] objArray = new object[2];
    float result = -1f;
    if (float.TryParse(value.ToString(), out result))
    {
      float num = (double) result > (double) byte.MaxValue ? (float) byte.MaxValue : result;
      switch (parameter.ToString())
      {
        case "R":
          this.color.R = (byte) num;
          break;
        case "G":
          this.color.G = (byte) num;
          break;
        case "B":
          this.color.B = (byte) num;
          break;
        case "A":
          this.color.A = (byte) ((double) num / 100.0 * (double) byte.MaxValue);
          break;
      }
    }
    objArray[0] = (object) this.color;
    objArray[1] = (object) false;
    return objArray;
  }
}

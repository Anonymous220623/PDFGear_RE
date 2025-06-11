// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.ColorToStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

public class ColorToStringConverter : IValueConverter
{
  private Color m_currentColorValue;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    Color color = (Color) value;
    color = Color.FromArgb(color.A, color.R, color.G, color.B);
    return (object) color.ToString();
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string str = (string) value;
    Color color;
    try
    {
      color = (Color) ColorConverter.ConvertFromString(str);
    }
    catch (Exception ex)
    {
      return (object) this.m_currentColorValue;
    }
    this.m_currentColorValue = color;
    return (object) color;
  }
}

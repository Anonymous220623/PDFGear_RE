// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Helper.TextHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Globalization;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Helper;

internal class TextHelper
{
  public static FormattedText CreateFormattedText(
    string text,
    FlowDirection flowDirection,
    Typeface typeface,
    double fontSize)
  {
    return new FormattedText(text, CultureInfo.CurrentUICulture, flowDirection, typeface, fontSize, (Brush) Brushes.Black, DpiHelper.DeviceDpiX);
  }
}

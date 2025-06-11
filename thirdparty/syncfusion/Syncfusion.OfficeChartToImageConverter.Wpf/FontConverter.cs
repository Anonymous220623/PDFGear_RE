// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.FontConverter
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class FontConverter : IValueConverter
{
  internal Dictionary<int, string> Names;
  internal Dictionary<int, double> Sizes;
  internal Dictionary<int, bool> Bolds;
  internal Dictionary<int, bool> Italics;
  internal Dictionary<int, bool> Underlines;
  internal Dictionary<int, bool> Strikethroughs;
  internal string DefaultName;
  internal double DefaultSize;
  internal bool DefaultBold;
  internal bool DefaultItalic;
  internal bool DefaultUnderline;
  internal bool DefaultStrikethrough;
  private int m_index = -1;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    ++this.m_index;
    if (this.Names != null)
      return (object) new FontFamily(this.Names.ContainsKey(this.m_index) ? this.Names[this.m_index] : this.DefaultName);
    if (this.Sizes != null)
      return (object) (this.Sizes.ContainsKey(this.m_index) ? this.Sizes[this.m_index] : this.DefaultSize);
    if (this.Bolds != null)
      return (object) (this.Bolds.ContainsKey(this.m_index) ? (this.Bolds[this.m_index] ? FontWeights.Bold : FontWeights.Normal) : (this.DefaultBold ? FontWeights.Bold : FontWeights.Normal));
    if (this.Italics != null)
      return (object) (this.Italics.ContainsKey(this.m_index) ? (this.Italics[this.m_index] ? FontStyles.Italic : FontStyles.Normal) : (this.DefaultItalic ? FontStyles.Italic : FontStyles.Normal));
    if (this.Underlines != null)
      return !this.Underlines.ContainsKey(this.m_index) ? (!this.DefaultItalic ? (object) null : (object) TextDecorations.Underline) : (!this.Underlines[this.m_index] ? (object) null : (object) TextDecorations.Underline);
    if (this.Strikethroughs == null)
      return (object) null;
    return !this.Strikethroughs.ContainsKey(this.m_index) ? (!this.DefaultItalic ? (object) null : (object) TextDecorations.Strikethrough) : (!this.Strikethroughs[this.m_index] ? (object) null : (object) TextDecorations.Strikethrough);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }
}

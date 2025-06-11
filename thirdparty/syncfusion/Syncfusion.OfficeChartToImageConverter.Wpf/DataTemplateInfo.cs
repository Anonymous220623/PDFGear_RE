// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.DataTemplateInfo
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using System.Windows.Media;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal struct DataTemplateInfo
{
  internal string FontName { get; set; }

  internal double Size { get; set; }

  internal bool Bold { get; set; }

  internal bool Italic { get; set; }

  internal bool Underline { get; set; }

  internal bool Strikethrough { get; set; }

  internal Color Color { get; set; }

  internal void SetValues(
    string fontName,
    double size,
    bool bold,
    bool italic,
    bool underline,
    bool strikethrough,
    Color color)
  {
    this.FontName = fontName;
    this.Size = size;
    this.Bold = bold;
    this.Italic = italic;
    this.Underline = underline;
    this.Strikethrough = strikethrough;
    this.Color = color;
  }
}

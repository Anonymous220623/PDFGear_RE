// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HeaderFooterFontColorSettings
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class HeaderFooterFontColorSettings : ICloneable
{
  private Font font;
  private Color fontColor;
  private bool hasUnderline;
  private bool hasStrikeThrough;
  private bool hasSuperscript;
  private bool hasSubscript;

  public HeaderFooterFontColorSettings()
  {
    this.font = new Font("Calibri", 8f);
    this.fontColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
  }

  internal Font Font
  {
    get => this.font;
    set => this.font = value;
  }

  internal bool HasUnderline
  {
    get => this.hasUnderline;
    set => this.hasUnderline = value;
  }

  internal bool HasStrikeThrough
  {
    get => this.hasStrikeThrough;
    set => this.hasStrikeThrough = value;
  }

  internal bool HasSuperscript
  {
    get => this.hasSuperscript;
    set => this.hasSuperscript = value;
  }

  internal bool HasSubscript
  {
    get => this.hasSubscript;
    set => this.hasSubscript = value;
  }

  internal Color FontColor
  {
    get => this.fontColor;
    set => this.fontColor = value;
  }

  public object Clone() => (object) (this.MemberwiseClone() as HeaderFooterFontColorSettings);
}

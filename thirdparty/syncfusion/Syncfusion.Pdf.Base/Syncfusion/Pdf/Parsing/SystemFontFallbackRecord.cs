// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFallbackRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFallbackRecord
{
  public SystemFontFallbackRange Range { get; set; }

  public SystemFontFontProperties FontDescriptor { get; set; }

  public SystemFontFallbackRecord(SystemFontFallbackRange range, SystemFontFontProperties descr)
  {
    this.Range = range;
    this.FontDescriptor = descr;
  }

  public override int GetHashCode()
  {
    return (17 * 23 + (this.Range != null ? this.Range.GetHashCode() : 0)) * 23 + (this.FontDescriptor != null ? this.FontDescriptor.GetHashCode() : 0);
  }

  public override bool Equals(object obj)
  {
    return obj is SystemFontFallbackRecord fontFallbackRecord && this.Range == fontFallbackRecord.Range && this.FontDescriptor == fontFallbackRecord.FontDescriptor;
  }
}

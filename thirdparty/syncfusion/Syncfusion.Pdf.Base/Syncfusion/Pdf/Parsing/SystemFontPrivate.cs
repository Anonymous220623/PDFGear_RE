// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPrivate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPrivate : SystemFontDict
{
  private readonly SystemFontTop top;
  private SystemFontSubrsIndex subrs;

  public static SystemFontOperatorDescriptor SubrsOperator { get; private set; }

  public SystemFontSubrsIndex Subrs
  {
    get
    {
      if (this.subrs == null)
      {
        this.subrs = new SystemFontSubrsIndex(this.File, this.top.CharstringType, this.Offset + (long) this.GetInt(SystemFontPrivate.SubrsOperator));
        this.File.ReadTable((SystemFontCFFTable) this.subrs);
      }
      return this.subrs;
    }
  }

  static SystemFontPrivate()
  {
    SystemFontPrivate.SubrsOperator = new SystemFontOperatorDescriptor((byte) 19);
  }

  public SystemFontPrivate(SystemFontTop top, long offset, int length)
    : base(top.File, offset, length)
  {
    this.top = top;
  }
}

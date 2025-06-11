// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Segments
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Segments
{
  private ushort startCode;
  private ushort endCode;
  private short idDelta;
  private ushort[] map;

  internal Segments()
  {
  }

  public Segments(ushort startCode, ushort endCode, short idDelta)
  {
    this.startCode = startCode;
    this.endCode = endCode;
    this.idDelta = idDelta;
  }

  public Segments(ushort startCode, ushort endCode, short idDelta, ushort[] mapval)
  {
    this.startCode = startCode;
    this.endCode = endCode;
    this.idDelta = idDelta;
    this.map = mapval;
  }

  public bool IsContain(ushort charCode) => (int) this.endCode >= (int) charCode;

  public ushort GetGlyphId(ushort charCode)
  {
    if ((int) charCode < (int) this.startCode || (int) charCode > (int) this.endCode)
      return 0;
    if (this.map == null)
      return (ushort) ((uint) charCode + (uint) (ushort) this.idDelta);
    int index = (int) charCode - (int) this.startCode;
    return index > this.map.Length || this.map[index] == (ushort) 0 ? (ushort) 0 : (ushort) ((uint) this.map[index] + (uint) (ushort) this.idDelta);
  }
}

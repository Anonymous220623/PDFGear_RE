// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IndexLocation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class IndexLocation(FontFile2 fontsource) : TableBase(fontsource)
{
  private int m_id = 3;
  private uint[] m_offset;
  private int p;

  internal override int Id => this.m_id;

  public uint[] Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  public long GetOffset(ushort index)
  {
    return this.Offset == null || (int) index >= this.Offset.Length || (int) index < this.Offset.Length - 1 && (int) this.Offset[(int) index + 1] == (int) this.Offset[(int) index] ? -1L : (long) this.Offset[(int) index];
  }

  public override void Read(ReadFontArray reader)
  {
    this.p = reader.Pointer;
    this.m_offset = new uint[this.FontSource.NumGlyphs + 1];
    reader.Pointer = this.p;
    for (int index = 0; index < this.m_offset.Length; ++index)
      this.m_offset[index] = this.FontSource.Header.IndexToLocFormat != (short) 0 ? (uint) reader.getnextULong() : (uint) reader.getnextUshort() * 2U;
  }
}

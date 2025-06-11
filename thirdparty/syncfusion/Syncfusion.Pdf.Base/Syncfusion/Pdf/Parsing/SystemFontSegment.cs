// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSegment
{
  private ushort startCode;
  private ushort endCode;
  private short idDelta;
  private ushort[] map;

  internal SystemFontSegment()
  {
  }

  public SystemFontSegment(ushort startCode, ushort endCode, short idDelta)
  {
    this.startCode = startCode;
    this.endCode = endCode;
    this.idDelta = idDelta;
  }

  public SystemFontSegment(ushort startCode, ushort endCode, short idDelta, ushort[] map)
    : this(startCode, endCode, idDelta)
  {
    this.map = map;
  }

  public bool IsInside(ushort charCode) => (int) this.endCode >= (int) charCode;

  public ushort GetGlyphId(ushort charCode)
  {
    if ((int) charCode < (int) this.startCode || (int) charCode > (int) this.endCode)
      return 0;
    if (this.map == null)
      return (ushort) ((uint) charCode + (uint) (ushort) this.idDelta);
    int index = (int) charCode - (int) this.startCode;
    return index > this.map.Length || this.map[index] == (ushort) 0 ? (ushort) 0 : (ushort) ((uint) this.map[index] + (uint) (ushort) this.idDelta);
  }

  public void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort(this.startCode);
    writer.WriteUShort(this.endCode);
    writer.WriteShort(this.idDelta);
    if (this.map != null)
    {
      writer.WriteUShort((ushort) this.map.Length);
      for (int index = 0; index < this.map.Length; ++index)
        writer.WriteUShort(this.map[index]);
    }
    else
      writer.WriteUShort((ushort) 0);
  }

  public void Import(SystemFontOpenTypeFontReader reader)
  {
    this.startCode = reader.ReadUShort();
    this.endCode = reader.ReadUShort();
    this.idDelta = reader.ReadShort();
    ushort length = reader.ReadUShort();
    if (length <= (ushort) 0)
      return;
    this.map = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.map[index] = reader.ReadUShort();
  }

  public override int GetHashCode()
  {
    return (17 * 23 + this.startCode.GetHashCode()) * 23 + this.endCode.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj is SystemFontSegment systemFontSegment && (int) this.startCode == (int) systemFontSegment.startCode && (int) this.endCode == (int) systemFontSegment.endCode;
  }
}

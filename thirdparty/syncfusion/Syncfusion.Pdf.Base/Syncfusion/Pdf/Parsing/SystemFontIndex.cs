// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontIndex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontIndex(SystemFontCFFFontFile file, long offset) : SystemFontCFFTable(file, offset)
{
  public long SkipOffset
  {
    get
    {
      return this.Offsets == null ? this.DataOffset : this.DataOffset + (long) SystemFontEnumerable.Last<uint>((IEnumerable<uint>) this.Offsets);
    }
  }

  public ushort Count { get; private set; }

  protected uint[] Offsets { get; private set; }

  protected long DataOffset { get; private set; }

  protected int GetDataLength(int index)
  {
    return (int) this.Offsets[index + 1] - (int) this.Offsets[index];
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    this.Count = reader.ReadCard16();
    if (this.Count == (ushort) 0)
    {
      this.DataOffset = reader.Position;
    }
    else
    {
      byte offsetSize = reader.ReadOffSize();
      ushort length = (ushort) ((uint) this.Count + 1U);
      this.Offsets = new uint[(int) length];
      for (int index = 0; index < (int) length; ++index)
        this.Offsets[index] = reader.ReadOffset(offsetSize);
      this.DataOffset = reader.Position - 1L;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GSUBTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GSUBTable : OtfTable
{
  internal GSUBTable(
    BigEndianReader reader,
    int gsubTableLocation,
    GDEFTable gdef,
    TtfReader ttfReader)
    : base(reader, gsubTableLocation, gdef, ttfReader)
  {
    this.Initialize();
  }

  internal override LookupTable ReadLookupTable(int type, int flag, int[] offset)
  {
    if (type == 7)
    {
      for (int index = 0; index < offset.Length; ++index)
      {
        int position = offset[index];
        this.Reader.Seek((long) position);
        int num1 = (int) this.Reader.ReadUInt16();
        type = (int) this.Reader.ReadUInt16();
        int num2 = position + this.Reader.ReadInt32();
        offset[index] = num2;
      }
    }
    LookupTable lookupTable = (LookupTable) null;
    switch (type - 1)
    {
      case 0:
        lookupTable = (LookupTable) new LookupSubTable1((OtfTable) this, flag, offset);
        break;
      case 1:
        lookupTable = (LookupTable) new LookupSubTable2((OtfTable) this, flag, offset);
        break;
      case 2:
        lookupTable = (LookupTable) new LookupSubTable3((OtfTable) this, flag, offset);
        break;
      case 3:
        lookupTable = (LookupTable) new LookupSubTable4((OtfTable) this, flag, offset);
        break;
      case 4:
        lookupTable = (LookupTable) new LookupSubTable5((OtfTable) this, flag, offset);
        break;
      case 5:
        lookupTable = (LookupTable) new LookupSubTable6((OtfTable) this, flag, offset);
        break;
    }
    return lookupTable;
  }
}

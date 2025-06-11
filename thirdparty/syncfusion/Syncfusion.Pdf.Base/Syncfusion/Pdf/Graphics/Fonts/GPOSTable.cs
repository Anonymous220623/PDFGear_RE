// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GPOSTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GPOSTable : OtfTable
{
  internal GPOSTable(BigEndianReader reader, int offset, GDEFTable gdef, TtfReader ttfReader)
    : base(reader, offset, gdef, ttfReader)
  {
    this.Initialize();
  }

  internal override LookupTable ReadLookupTable(int type, int flag, int[] offsets)
  {
    if (type == 9)
    {
      for (int index = 0; index < offsets.Length; ++index)
      {
        int offset = offsets[index];
        this.Reader.Seek((long) offset);
        int num1 = (int) this.Reader.ReadUInt16();
        type = (int) this.Reader.ReadInt16();
        int num2 = offset + this.Reader.ReadInt32();
        offsets[index] = num2;
      }
    }
    LookupTable lookupTable = (LookupTable) null;
    switch (type - 1)
    {
      case 0:
        lookupTable = (LookupTable) new LookupTable1((OtfTable) this, flag, offsets);
        break;
      case 1:
        lookupTable = (LookupTable) new LookupTable2((OtfTable) this, flag, offsets);
        break;
      case 3:
        lookupTable = (LookupTable) new LookupTable4((OtfTable) this, flag, offsets);
        break;
      case 4:
        lookupTable = (LookupTable) new LookupTable5((OtfTable) this, flag, offsets);
        break;
      case 5:
        lookupTable = (LookupTable) new LookupTable6((OtfTable) this, flag, offsets);
        break;
    }
    return lookupTable;
  }
}

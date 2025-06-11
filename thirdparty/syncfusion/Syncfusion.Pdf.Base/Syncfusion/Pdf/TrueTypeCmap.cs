// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TrueTypeCmap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class TrueTypeCmap(FontFile2 fontsource) : TableBase(fontsource)
{
  private int m_id = 2;
  public FontEncoding[] encodings;
  private Dictionary<FontEncoding, CmapTables> encodingtable;
  private ushort noofSubtable;
  private uint subOffset;

  internal override int Id => this.m_id;

  public override void Read(ReadFontArray reader)
  {
    int num = (int) reader.getnextUshort();
    this.noofSubtable = reader.getnextUshort();
    this.encodings = new FontEncoding[(int) this.noofSubtable];
    this.encodingtable = new Dictionary<FontEncoding, CmapTables>((int) this.noofSubtable);
    for (int index = 0; index < (int) this.noofSubtable; ++index)
    {
      FontEncoding fontEncoding = new FontEncoding();
      fontEncoding.ReadEncodingDeatils(reader);
      this.encodings[index] = fontEncoding;
    }
  }

  public CmapTables GetCmaptable(ushort platformid, ushort encodingid)
  {
    FontEncoding encode = (FontEncoding) null;
    for (int index = 0; index < (int) this.noofSubtable; ++index)
    {
      if ((int) this.encodings[index].PlatformId == (int) platformid && (int) this.encodings[index].EncodingId == (int) encodingid)
        encode = this.encodings[index];
    }
    return encode == null ? (CmapTables) null : this.GetCmapTable(encode, this.Reader);
  }

  public CmapTables GetCmapTable(FontEncoding encode, ReadFontArray reader)
  {
    CmapTables cmapTable;
    if (!this.encodingtable.TryGetValue(encode, out cmapTable))
    {
      reader.Pointer = (int) encode.Offset + this.Offset;
      cmapTable = CmapTables.ReadCmapTable(reader);
      this.encodingtable[encode] = cmapTable;
    }
    return cmapTable;
  }
}

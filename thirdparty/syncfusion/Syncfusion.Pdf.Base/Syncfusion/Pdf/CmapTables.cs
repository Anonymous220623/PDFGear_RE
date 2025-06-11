// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CmapTables
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class CmapTables
{
  private ushort m_firstcode;

  public abstract ushort FirstCode { get; }

  public static CmapTables ReadCmapTable(ReadFontArray reader)
  {
    switch (reader.getnextUint16())
    {
      case 4:
        CmapTables cmapTables1 = (CmapTables) new Cmap4();
        cmapTables1.Read(reader);
        return cmapTables1;
      case 6:
        CmapTables cmapTables2 = (CmapTables) new Cmap6();
        cmapTables2.Read(reader);
        return cmapTables2;
      default:
        CmapTables cmapTables3 = (CmapTables) new Cmap0();
        cmapTables3.Read(reader);
        return cmapTables3;
    }
  }

  public abstract ushort GetGlyphId(ushort charCode);

  public abstract void Read(ReadFontArray reader);
}

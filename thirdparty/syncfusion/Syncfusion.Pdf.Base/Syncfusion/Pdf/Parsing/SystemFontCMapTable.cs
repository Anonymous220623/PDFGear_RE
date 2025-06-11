// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCMapTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontCMapTable
{
  public abstract ushort FirstCode { get; }

  internal static SystemFontCMapTable ReadCMapTable(SystemFontOpenTypeFontReader reader)
  {
    switch (reader.ReadUShort())
    {
      case 0:
        SystemFontCMapTable systemFontCmapTable1 = (SystemFontCMapTable) new SystemFontCMapFormat0Table();
        systemFontCmapTable1.Read(reader);
        return systemFontCmapTable1;
      case 4:
        SystemFontCMapTable systemFontCmapTable2 = (SystemFontCMapTable) new SystemFontCMapFormat4Table();
        systemFontCmapTable2.Read(reader);
        return systemFontCmapTable2;
      case 6:
        SystemFontCMapTable systemFontCmapTable3 = (SystemFontCMapTable) new SystemFontCMapFormat6Table();
        systemFontCmapTable3.Read(reader);
        return systemFontCmapTable3;
      default:
        return (SystemFontCMapTable) null;
    }
  }

  internal static SystemFontCMapTable ImportCMapTable(SystemFontOpenTypeFontReader reader)
  {
    SystemFontCMapTable systemFontCmapTable;
    switch (reader.ReadUShort())
    {
      case 0:
        systemFontCmapTable = (SystemFontCMapTable) new SystemFontCMapFormat0Table();
        break;
      case 4:
        systemFontCmapTable = (SystemFontCMapTable) new SystemFontCMapFormat4Table();
        break;
      default:
        return (SystemFontCMapTable) null;
    }
    systemFontCmapTable.Import(reader);
    return systemFontCmapTable;
  }

  public abstract ushort GetGlyphId(ushort charCode);

  public abstract void Read(SystemFontOpenTypeFontReader reader);

  public abstract void Write(SystemFontFontWriter writer);

  public abstract void Import(SystemFontOpenTypeFontReader reader);
}

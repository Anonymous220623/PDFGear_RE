// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GDEFTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GDEFTable
{
  private CDEFTable m_glyphCdefTable;
  private CDEFTable m_markAttachmentCdefTable;

  internal GDEFTable(BigEndianReader reader, TtfTableInfo tableInfo)
  {
    if (tableInfo.Offset <= 0)
      return;
    reader.Seek((long) tableInfo.Offset);
    int num1 = (int) reader.ReadUInt16();
    int num2 = (int) reader.ReadUInt16();
    int num3 = (int) reader.ReadUInt16();
    int num4 = (int) reader.ReadUInt16();
    int num5 = (int) reader.ReadUInt16();
    int num6 = (int) reader.ReadUInt16();
    if (num3 > 0)
      this.m_glyphCdefTable = this.GetTable(reader, num3 + tableInfo.Offset);
    if (num6 <= 0)
      return;
    this.m_markAttachmentCdefTable = this.GetTable(reader, num6 + tableInfo.Offset);
  }

  internal CDEFTable GlyphCdefTable
  {
    get => this.m_glyphCdefTable;
    set => this.m_glyphCdefTable = value;
  }

  internal CDEFTable MarkAttachmentCdefTable
  {
    get => this.m_markAttachmentCdefTable;
    set => this.m_markAttachmentCdefTable = value;
  }

  private CDEFTable GetTable(BigEndianReader reader, int offset)
  {
    CDEFTable table = new CDEFTable();
    reader.Seek((long) offset);
    switch (reader.ReadUInt16())
    {
      case 1:
        int num1 = (int) reader.ReadUInt16();
        int num2 = (int) reader.ReadInt16();
        int num3 = num1 + num2;
        for (int key = num1; key < num3; ++key)
        {
          int num4 = (int) reader.ReadInt16();
          table.Records.Add(key, num4);
        }
        break;
      case 2:
        int num5 = (int) reader.ReadUInt16();
        for (int index = 0; index < num5; ++index)
        {
          int num6 = (int) reader.ReadUInt16();
          int num7 = (int) reader.ReadUInt16();
          int num8 = (int) reader.ReadUInt16();
          for (int key = num6; key <= num7; ++key)
          {
            if (table.Records.ContainsKey(key))
              table.Records[key] = num8;
            else
              table.Records.Add(key, num8);
          }
        }
        break;
      default:
        table = (CDEFTable) null;
        break;
    }
    return table;
  }

  internal bool IsSkip(int glyph, int flag)
  {
    if (this.m_glyphCdefTable != null && (flag & 14) != 0)
    {
      int num = this.m_glyphCdefTable.GetValue(glyph);
      if (num == 1 && (flag & 2) != 0 || num == 3 && (flag & 8) != 0 || num == 2 && (flag & 4) != 0)
        return true;
    }
    return this.m_markAttachmentCdefTable != null && this.m_markAttachmentCdefTable.GetValue(glyph) > 0 && flag >> 8 > 0 && this.m_markAttachmentCdefTable.GetValue(glyph) != flag >> 8;
  }
}

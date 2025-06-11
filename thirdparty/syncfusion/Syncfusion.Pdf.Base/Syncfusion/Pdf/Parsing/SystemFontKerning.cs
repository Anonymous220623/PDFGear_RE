// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontKerning
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontKerning(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  private List<SystemFontKerningSubTable> subTables;

  internal override uint Tag => SystemFontTags.KERN_TABLE;

  public SystemFontKerningInfo GetKerning(ushort leftGlyphIndex, ushort rightGlyphIndex)
  {
    double x1 = 0.0;
    double x2 = 0.0;
    double y1 = 0.0;
    double y2 = 0.0;
    short? nullable1 = new short?();
    short? nullable2 = new short?();
    short? nullable3 = new short?();
    short? nullable4 = new short?();
    foreach (SystemFontKerningSubTable subTable in this.subTables)
    {
      short num = subTable.GetValue(leftGlyphIndex, rightGlyphIndex);
      if (subTable.IsHorizontal)
      {
        if (subTable.HasMinimumValues)
        {
          if (subTable.IsCrossStream)
            nullable2 = new short?(num);
          else
            nullable1 = new short?(num);
        }
        else if (subTable.IsCrossStream)
        {
          if (subTable.Override)
            y1 = (double) num;
          else
            y1 += (double) num;
        }
        else if (subTable.Override)
          x1 += (double) num;
        else
          x1 = (double) num;
      }
      else if (subTable.HasMinimumValues)
      {
        if (subTable.IsCrossStream)
          nullable4 = new short?(num);
        else
          nullable3 = new short?(num);
      }
      else if (subTable.IsCrossStream)
      {
        if (subTable.Override)
          y2 = (double) num;
        else
          y2 += (double) num;
      }
      else if (subTable.Override)
        x2 = (double) num;
      else
        x2 += (double) num;
    }
    if (nullable1.HasValue)
    {
      short? nullable5 = nullable1;
      double num = x1;
      if ((double) nullable5.GetValueOrDefault() > num && nullable5.HasValue)
        x1 = (double) nullable1.Value;
    }
    if (nullable2.HasValue)
    {
      short? nullable6 = nullable2;
      double num = y1;
      if ((double) nullable6.GetValueOrDefault() > num && nullable6.HasValue)
        y1 = (double) nullable2.Value;
    }
    if (nullable3.HasValue)
    {
      short? nullable7 = nullable3;
      double num = x2;
      if ((double) nullable7.GetValueOrDefault() > num && nullable7.HasValue)
        x2 = (double) nullable3.Value;
    }
    if (nullable4.HasValue)
    {
      short? nullable8 = nullable4;
      double num = y2;
      if ((double) nullable8.GetValueOrDefault() > num && nullable8.HasValue)
        y2 = (double) nullable4.Value;
    }
    return new SystemFontKerningInfo()
    {
      HorizontalKerning = new Point(x1, y1),
      VerticalKerning = new Point(x2, y2)
    };
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    ushort capacity = reader.ReadUShort();
    this.subTables = new List<SystemFontKerningSubTable>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
    {
      SystemFontKerningSubTable fontKerningSubTable = SystemFontKerningSubTable.ReadSubTable(this.FontSource, reader);
      if (fontKerningSubTable != null)
        this.subTables.Add(fontKerningSubTable);
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    if (this.subTables == null)
    {
      writer.WriteUShort((ushort) 0);
    }
    else
    {
      writer.WriteUShort((ushort) this.subTables.Count);
      for (int index = 0; index < this.subTables.Count; ++index)
        this.subTables[index].Write(writer);
    }
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort capacity = reader.ReadUShort();
    if (capacity <= (ushort) 0)
      return;
    this.subTables = new List<SystemFontKerningSubTable>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
      this.subTables.Add(SystemFontKerningSubTable.ImportSubTable(this.FontSource, reader));
  }
}

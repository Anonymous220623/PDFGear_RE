// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOffsetTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontOffsetTable
{
  public ushort NumTables { get; private set; }

  public bool HasOpenTypeOutlines { get; private set; }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    if (reader == null)
      return;
    this.HasOpenTypeOutlines = (int) SystemFontTags.OTTO_TAG == (int) reader.ReadULong();
    this.NumTables = reader.ReadUShort();
    int num1 = (int) reader.ReadUShort();
    int num2 = (int) reader.ReadUShort();
    int num3 = (int) reader.ReadUShort();
  }
}

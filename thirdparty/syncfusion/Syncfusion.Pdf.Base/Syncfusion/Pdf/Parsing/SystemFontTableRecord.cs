// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTableRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTableRecord
{
  public uint Tag { get; set; }

  public uint CheckSum { get; set; }

  public uint Offset { get; set; }

  public uint Length { get; set; }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    this.Tag = reader.ReadULong();
    this.CheckSum = reader.ReadULong();
    this.Offset = reader.ReadULong();
    this.Length = reader.ReadULong();
  }

  public override string ToString() => SystemFontTags.GetStringFromTag(this.Tag);
}

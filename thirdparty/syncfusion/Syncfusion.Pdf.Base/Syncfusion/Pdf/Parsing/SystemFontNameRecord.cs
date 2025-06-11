// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontNameRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontNameRecord
{
  public ushort PlatformID { get; private set; }

  public ushort EncodingID { get; private set; }

  public ushort LanguageID { get; private set; }

  public ushort NameID { get; private set; }

  public ushort Length { get; private set; }

  public ushort Offset { get; private set; }

  public override bool Equals(object obj)
  {
    return obj is SystemFontNameRecord systemFontNameRecord && (int) this.EncodingID == (int) systemFontNameRecord.EncodingID && (int) this.LanguageID == (int) systemFontNameRecord.LanguageID && (int) this.Length == (int) systemFontNameRecord.Length && (int) this.NameID == (int) systemFontNameRecord.NameID && (int) this.PlatformID == (int) systemFontNameRecord.PlatformID;
  }

  public override int GetHashCode()
  {
    return ((((17 * 23 + this.PlatformID.GetHashCode()) * 23 + this.EncodingID.GetHashCode()) * 23 + this.LanguageID.GetHashCode()) * 23 + this.NameID.GetHashCode()) * 23 + this.Length.GetHashCode();
  }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    this.PlatformID = reader.ReadUShort();
    this.EncodingID = reader.ReadUShort();
    this.LanguageID = reader.ReadUShort();
    this.NameID = reader.ReadUShort();
    this.Length = reader.ReadUShort();
    this.Offset = reader.ReadUShort();
  }
}

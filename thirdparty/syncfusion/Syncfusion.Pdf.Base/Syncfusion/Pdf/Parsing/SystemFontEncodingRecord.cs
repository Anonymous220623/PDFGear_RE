// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEncodingRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontEncodingRecord
{
  public ushort PlatformId { get; set; }

  public ushort EncodingId { get; set; }

  public uint Offset { get; set; }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    this.PlatformId = reader.ReadUShort();
    this.EncodingId = reader.ReadUShort();
    this.Offset = reader.ReadULong();
  }
}

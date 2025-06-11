// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FontEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class FontEncoding
{
  private ushort m_platformId;
  private ushort m_encodingId;
  private uint m_offset;

  public uint Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  public ushort PlatformId
  {
    get => this.m_platformId;
    set => this.m_platformId = value;
  }

  public ushort EncodingId
  {
    get => this.m_encodingId;
    set => this.m_encodingId = value;
  }

  public void ReadEncodingDeatils(ReadFontArray reader)
  {
    this.PlatformId = reader.getnextUshort();
    this.EncodingId = reader.getnextUshort();
    this.Offset = reader.getULong();
  }
}

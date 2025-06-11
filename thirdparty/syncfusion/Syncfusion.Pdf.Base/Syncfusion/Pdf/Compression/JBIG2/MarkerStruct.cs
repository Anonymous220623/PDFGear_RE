// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.MarkerStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class MarkerStruct
{
  private byte m_marker;
  private int m_originalLength;
  private byte[] m_data;

  internal MarkerStruct(byte marker, int originalDataLength, int lengthLimit)
  {
    this.m_marker = marker;
    this.m_originalLength = originalDataLength;
    this.m_data = new byte[lengthLimit];
  }

  public byte Marker => this.m_marker;

  public int OriginalLength => this.m_originalLength;

  public byte[] Data => this.m_data;
}

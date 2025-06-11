// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.ComponentBuffer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class ComponentBuffer
{
  private byte[][] m_buffer;
  private int[] m_funnyIndices;
  private int m_funnyOffset;

  public ComponentBuffer()
  {
  }

  public ComponentBuffer(byte[][] buf, int[] funnyIndices, int funnyOffset)
  {
    this.SetBuffer(buf, funnyIndices, funnyOffset);
  }

  public void SetBuffer(byte[][] buf, int[] funnyIndices, int funnyOffset)
  {
    this.m_buffer = buf;
    this.m_funnyIndices = funnyIndices;
    this.m_funnyOffset = funnyOffset;
  }

  public byte[] this[int i]
  {
    get
    {
      return this.m_funnyIndices == null ? this.m_buffer[i] : this.m_buffer[this.m_funnyIndices[i + this.m_funnyOffset]];
    }
  }
}

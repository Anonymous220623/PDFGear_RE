// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ByteStreamRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ByteStreamRenderer
{
  private byte[] data;
  private int position;

  public int Remaining => this.data.Length - this.position;

  public ByteStreamRenderer(byte[] data)
  {
    this.data = data;
    this.position = 0;
  }

  public byte ReadByte()
  {
    byte num = this.data[this.position];
    ++this.position;
    return num;
  }

  public void Read(byte[] buffer)
  {
    for (int index = 0; index < buffer.Length; ++index)
      buffer[index] = this.ReadByte();
  }
}

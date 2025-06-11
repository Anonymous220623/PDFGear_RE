// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.Rect
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class Rect : BaseWordRecord
{
  private long m_left;
  private long m_right;
  private long m_top;
  private long m_bottom;

  public long Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public long Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public long Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public long Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  public void Read(Stream stream)
  {
    this.m_left = (long) stream.ReadByte();
    this.m_top = (long) stream.ReadByte();
    this.m_right = (long) stream.ReadByte();
    this.m_bottom = (long) stream.ReadByte();
  }

  public void Write(Stream stream)
  {
    stream.WriteByte((byte) this.m_left);
    stream.WriteByte((byte) this.m_top);
    stream.WriteByte((byte) this.m_right);
    stream.WriteByte((byte) this.m_bottom);
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.SourceMgr
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal abstract class SourceMgr
{
  private byte[] m_next_input_byte;
  private int m_bytes_in_buffer;
  private int m_position;

  public abstract void init_source();

  public abstract bool fill_input_buffer();

  protected void initInternalBuffer(byte[] buffer, int size)
  {
    this.m_bytes_in_buffer = size;
    this.m_next_input_byte = buffer;
    this.m_position = 0;
  }

  public virtual void skip_input_data(int num_bytes)
  {
    if (num_bytes <= 0)
      return;
    while (num_bytes > this.m_bytes_in_buffer)
    {
      num_bytes -= this.m_bytes_in_buffer;
      this.fill_input_buffer();
    }
    this.m_position += num_bytes;
    this.m_bytes_in_buffer -= num_bytes;
  }

  public virtual bool resync_to_restart(DecompressStruct cinfo, int desired)
  {
    do
    {
      switch (cinfo.m_unread_marker >= 192 /*0xC0*/ ? (cinfo.m_unread_marker < 208 /*0xD0*/ || cinfo.m_unread_marker > 215 ? 3 : (cinfo.m_unread_marker == 208 /*0xD0*/ + (desired + 1 & 7) || cinfo.m_unread_marker == 208 /*0xD0*/ + (desired + 2 & 7) ? 3 : (cinfo.m_unread_marker == 208 /*0xD0*/ + (desired - 1 & 7) || cinfo.m_unread_marker == 208 /*0xD0*/ + (desired - 2 & 7) ? 2 : 1))) : 2)
      {
        case 1:
          cinfo.m_unread_marker = 0;
          return true;
        case 2:
          continue;
        case 3:
          goto label_5;
        default:
          continue;
      }
    }
    while (cinfo.m_marker.next_marker());
    return false;
label_5:
    return true;
  }

  public virtual void term_source()
  {
  }

  public virtual bool GetTwoBytes(out int V)
  {
    if (!this.MakeByteAvailable())
    {
      V = 0;
      return false;
    }
    --this.m_bytes_in_buffer;
    V = (int) this.m_next_input_byte[this.m_position] << 8;
    ++this.m_position;
    if (!this.MakeByteAvailable())
      return false;
    --this.m_bytes_in_buffer;
    V += (int) this.m_next_input_byte[this.m_position];
    ++this.m_position;
    return true;
  }

  public virtual bool GetByte(out int V)
  {
    if (!this.MakeByteAvailable())
    {
      V = 0;
      return false;
    }
    --this.m_bytes_in_buffer;
    V = (int) this.m_next_input_byte[this.m_position];
    ++this.m_position;
    return true;
  }

  public virtual int GetBytes(byte[] dest, int amount)
  {
    int bytes = amount;
    if (bytes > this.m_bytes_in_buffer)
      bytes = this.m_bytes_in_buffer;
    for (int index = 0; index < bytes; ++index)
    {
      dest[index] = this.m_next_input_byte[this.m_position];
      ++this.m_position;
      --this.m_bytes_in_buffer;
    }
    return bytes;
  }

  public virtual bool MakeByteAvailable()
  {
    return this.m_bytes_in_buffer != 0 || this.fill_input_buffer();
  }
}

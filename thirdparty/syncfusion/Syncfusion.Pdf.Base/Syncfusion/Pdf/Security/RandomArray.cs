// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RandomArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RandomArray : IRandom, IDisposable
{
  private byte[] m_array;

  public virtual long Length => (long) this.m_array.Length;

  internal RandomArray(byte[] array)
  {
    this.m_array = array != null ? array : throw new ArgumentNullException();
  }

  public virtual void Close() => this.m_array = (byte[]) null;

  public virtual int Get(long offset)
  {
    return offset >= (long) this.m_array.Length ? -1 : (int) byte.MaxValue & (int) this.m_array[(int) offset];
  }

  public virtual int Get(long offset, byte[] bytes, int off, int length)
  {
    if (this.m_array == null)
      throw new InvalidOperationException("Closed array");
    if (offset >= (long) this.m_array.Length)
      return -1;
    if (offset + (long) length > (long) this.m_array.Length)
      length = (int) ((long) this.m_array.Length - offset);
    Array.Copy((Array) this.m_array, (int) offset, (Array) bytes, off, length);
    return length;
  }

  public virtual void Dispose() => this.Close();
}

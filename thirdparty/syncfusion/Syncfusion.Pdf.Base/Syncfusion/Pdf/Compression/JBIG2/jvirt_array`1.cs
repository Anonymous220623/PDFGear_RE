// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.jvirt_array`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class jvirt_array<T>
{
  private CommonStruct m_cinfo;
  private T[][] m_buffer;

  internal jvirt_array(int width, int height, jvirt_array<T>.Allocator allocator)
  {
    this.m_cinfo = (CommonStruct) null;
    this.m_buffer = allocator(width, height);
  }

  public CommonStruct ErrorProcessor
  {
    get => this.m_cinfo;
    set => this.m_cinfo = value;
  }

  public T[][] Access(int startRow, int numberOfRows)
  {
    if (startRow + numberOfRows > this.m_buffer.Length && this.m_cinfo == null)
      throw new InvalidOperationException("Bogus virtual array access");
    T[][] objArray = new T[numberOfRows][];
    for (int index = 0; index < numberOfRows; ++index)
      objArray[index] = this.m_buffer[startRow + index];
    return objArray;
  }

  internal delegate T[][] Allocator(int width, int height);
}

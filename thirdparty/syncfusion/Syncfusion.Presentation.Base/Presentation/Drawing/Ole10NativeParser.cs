// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Ole10NativeParser
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Ole10NativeParser
{
  private uint m_size;
  private string m_label;
  private string m_fileName;
  private string m_command;
  private uint m_nativeDataSize;
  public byte[] m_nativeData;

  public Ole10NativeParser(byte[] arrBuffer)
  {
    int startIndex = 0;
    this.m_size = BitConverter.ToUInt32(arrBuffer, startIndex);
    int num1 = startIndex + 4;
    if (this.m_size < 4U)
      return;
    int iOffSet1 = num1 + 2;
    this.m_label = this.ReadString(arrBuffer, ref iOffSet1);
    this.m_fileName = this.ReadString(arrBuffer, ref iOffSet1);
    int index = iOffSet1 + 2;
    byte num2 = arrBuffer[index];
    int iOffSet2 = index + 1 + (int) num2 + 3;
    this.m_command = this.ReadString(arrBuffer, ref iOffSet2);
    this.m_nativeDataSize = BitConverter.ToUInt32(arrBuffer, iOffSet2);
    int srcOffset = iOffSet2 + 4;
    if (this.m_nativeDataSize > this.m_size || this.m_nativeDataSize < 0U)
      return;
    this.m_nativeData = new byte[(IntPtr) this.m_nativeDataSize];
    Buffer.BlockCopy((Array) arrBuffer, srcOffset, (Array) this.m_nativeData, 0, (int) this.m_nativeDataSize);
  }

  internal byte[] NativeData => this.m_nativeData;

  internal string FileName => this.m_label;

  private string ReadString(byte[] array, ref int iOffSet)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (array[iOffSet] != (byte) 0)
    {
      stringBuilder.Append((char) array[iOffSet]);
      ++iOffSet;
    }
    ++iOffSet;
    return stringBuilder.ToString();
  }
}

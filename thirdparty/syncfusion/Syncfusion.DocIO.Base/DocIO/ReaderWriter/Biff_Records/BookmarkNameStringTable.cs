// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BookmarkNameStringTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BookmarkNameStringTable
{
  private List<string> m_strArr;
  private int m_bkmkCount;

  internal int BookmarkCount
  {
    get => this.m_bkmkCount;
    set => this.m_bkmkCount = value;
  }

  internal string this[int index] => this.m_strArr[index];

  internal int BookmarkNamesLength => this.m_strArr != null ? this.m_strArr.Count : 0;

  internal BookmarkNameStringTable(Stream stream, int length)
  {
    int count = 6;
    long position = stream.Position;
    byte[] buffer1 = new byte[count];
    stream.Read(buffer1, 0, count);
    this.m_bkmkCount = BitConverter.ToInt32(buffer1, 2);
    this.m_strArr = new List<string>(this.m_bkmkCount);
    for (int index = 0; index < this.m_bkmkCount; ++index)
    {
      byte[] buffer2 = new byte[2];
      stream.Read(buffer2, 0, 2);
      byte[] numArray = new byte[(int) BitConverter.ToInt16(buffer2, 0) * 2];
      stream.Read(numArray, 0, numArray.Length);
      this.m_strArr.Add(Encoding.Unicode.GetString(numArray));
    }
    if (stream.Position - position > (long) length)
      throw new StreamReadException("");
  }

  internal BookmarkNameStringTable()
  {
    this.m_bkmkCount = 0;
    this.m_strArr = new List<string>(this.m_bkmkCount);
  }

  internal void Close()
  {
    if (this.m_strArr == null)
      return;
    this.m_strArr.Clear();
    this.m_strArr = (List<string>) null;
  }

  internal void Save(Stream stream, Fib fib)
  {
    if (this.m_strArr.Count <= 0)
      return;
    int length = 6;
    uint position1 = (uint) stream.Position;
    byte[] buffer = new byte[length];
    BitConverter.GetBytes(this.m_strArr.Count).CopyTo((Array) buffer, 2);
    buffer[0] = buffer[1] = byte.MaxValue;
    stream.Write(buffer, 0, buffer.Length);
    this.m_bkmkCount = this.m_strArr.Count;
    for (int index = 0; index < this.m_bkmkCount; ++index)
    {
      byte[] bytes1 = Encoding.Unicode.GetBytes(this.m_strArr[index]);
      byte[] bytes2 = BitConverter.GetBytes((short) (bytes1.Length / 2));
      stream.Write(bytes2, 0, bytes2.Length);
      stream.Write(bytes1, 0, bytes1.Length);
    }
    uint position2 = (uint) stream.Position;
    if (position2 <= position1)
      return;
    fib.FibRgFcLcb97FcSttbfBkmk = position1;
    fib.FibRgFcLcb97LcbSttbfBkmk = position2 - position1;
  }

  internal void Add(string name) => this.m_strArr.Add(name);

  internal int Find(string name)
  {
    int num = -1;
    for (int index = 0; index < this.m_strArr.Count; ++index)
    {
      if (this[index] == name)
      {
        num = index;
        break;
      }
    }
    return num;
  }
}

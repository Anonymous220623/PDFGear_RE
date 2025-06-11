// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.CharPosTableRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class CharPosTableRecord : BaseWordRecord
{
  private int[] m_arrPositions;

  internal CharPosTableRecord()
  {
  }

  internal CharPosTableRecord(byte[] data)
    : base(data)
  {
  }

  internal CharPosTableRecord(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal CharPosTableRecord(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal CharPosTableRecord(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal int[] Positions
  {
    get => this.m_arrPositions;
    set => this.m_arrPositions = value;
  }

  internal override int Length => this.m_arrPositions.Length * 4;

  internal string GetTextChunk(string text, int position)
  {
    switch (text)
    {
      case null:
        throw new ArgumentNullException(nameof (text));
      case "":
        throw new ArgumentException("text - string can not be empty");
      default:
        int num = this.Positions.Length - 1;
        int startIndex = position >= 0 && position <= num ? this.Positions[position] : throw new ArgumentOutOfRangeException(nameof (position), "Value can not be less 0 and greater " + num.ToString());
        int length = position + 1 >= this.Positions.Length ? text.Length - startIndex : this.Positions[position + 1] - startIndex;
        return text.Substring(startIndex, length);
    }
  }

  internal override void Close()
  {
    base.Close();
    this.m_arrPositions = (int[]) null;
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - 1");
    if (iCount < 0 || iOffset + iCount > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    int length = iCount / 4;
    this.m_arrPositions = new int[length];
    Buffer.BlockCopy((Array) arrData, 0, (Array) this.m_arrPositions, 0, length * 4);
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - 1");
    Buffer.BlockCopy((Array) this.m_arrPositions, 0, (Array) arrData, 0, this.Length);
    return arrData.Length;
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ParagraphExceptionInDiskPage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ParagraphExceptionInDiskPage : ParagraphPropertyException
{
  internal override int Length => (this.IsPad ? 2 : 1) + 2 + this.m_arrSprms.Length;

  protected bool IsPad => this.m_arrSprms.Length % 2 == 0;

  internal ParagraphExceptionInDiskPage()
  {
  }

  internal ParagraphExceptionInDiskPage(ParagraphPropertyException papx)
  {
    this.m_usStyleId = papx.StyleIndex;
    this.m_arrSprms = papx.PropertyModifiers;
  }

  internal int Parse(byte[] arrData, int iOffset)
  {
    int num1 = arrData != null ? arrData.Length : throw new ArgumentNullException(nameof (arrData));
    byte num2 = iOffset >= 0 && iOffset < num1 ? arrData[iOffset] : throw new ArgumentOutOfRangeException(nameof (iOffset));
    ++iOffset;
    if (num2 == (byte) 0)
      num2 = iOffset < num1 ? arrData[iOffset++] : throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num3 = (int) num2 * 2;
    if (iOffset + num3 > num1)
      throw new ArgumentOutOfRangeException("Data array is too short");
    this.m_usStyleId = BitConverter.ToUInt16(arrData, iOffset);
    iOffset += 2;
    int iCount = num3 - 2;
    this.m_arrSprms.Parse(arrData, iOffset, iCount);
    return iOffset;
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    int num1 = iOffset;
    int num2 = this.Length - (this.IsPad ? 2 : 1);
    if (this.IsPad)
      arrData[iOffset++] = (byte) 0;
    byte num3 = (byte) (num2 / 2);
    arrData[iOffset++] = num3;
    BitConverter.GetBytes(this.m_usStyleId).CopyTo((Array) arrData, iOffset);
    iOffset += 2;
    iOffset += this.m_arrSprms.Save(arrData, iOffset);
    return iOffset - num1;
  }

  internal void Save(BinaryWriter writer, Stream stream)
  {
    int num1 = this.Length - (this.IsPad ? 2 : 1);
    if (this.IsPad)
      writer.Write((byte) 0);
    byte num2 = (byte) (num1 / 2);
    if (!this.IsPad)
      ++num2;
    writer.Write(num2);
    writer.Write(this.m_usStyleId);
    if (this.m_arrSprms == null)
      return;
    this.m_arrSprms.Save(writer, stream);
  }
}

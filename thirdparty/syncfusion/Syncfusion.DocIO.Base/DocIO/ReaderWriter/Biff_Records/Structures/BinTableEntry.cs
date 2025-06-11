// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.BinTableEntry
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

internal class BinTableEntry
{
  public const int RECORD_SIZE = 4;
  private int m_iValue;

  internal int Value
  {
    get => this.m_iValue;
    set => this.m_iValue = value;
  }

  internal int Parse(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length");
    this.m_iValue = BitConverter.ToInt32(arrData, iOffset);
    return iOffset + 4;
  }

  internal void Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iOffset + 4 > arrData.Length)
      throw new ArgumentOutOfRangeException("arrData.Length");
    BitConverter.GetBytes(this.m_iValue).CopyTo((Array) arrData, iOffset);
  }
}

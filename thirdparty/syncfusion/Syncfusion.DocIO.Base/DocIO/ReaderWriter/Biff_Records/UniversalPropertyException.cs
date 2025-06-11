// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.UniversalPropertyException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class UniversalPropertyException : BaseWordRecord
{
  private byte[] m_arrData;

  internal UniversalPropertyException()
  {
  }

  internal UniversalPropertyException(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal byte[] Data => this.m_arrData;

  internal override int Length => this.m_arrData.Length;

  internal override void Close()
  {
    base.Close();
    this.m_arrData = (byte[]) null;
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length");
    if (iCount < 0 || iCount + iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    if (this.m_arrData == null || this.m_arrData.Length != iCount)
      this.m_arrData = new byte[iCount];
    Array.Copy((Array) arrData, iOffset, (Array) this.m_arrData, 0, iCount);
  }
}

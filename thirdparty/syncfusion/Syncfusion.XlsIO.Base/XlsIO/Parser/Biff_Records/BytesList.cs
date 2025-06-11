// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BytesList
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class BytesList
{
  private const int DEF_CAPACITY_STEP = 512 /*0x0200*/;
  private const int DEF_DEFAULT_CAPACITY = 512 /*0x0200*/;
  private const int DEF_RECORD_SIZE = 20;
  private byte[] m_arrBuffer;
  private int m_iCurPos;
  private bool m_bExactSize = true;

  public BytesList()
    : this(512 /*0x0200*/)
  {
  }

  public BytesList(bool bExactSize) => this.m_bExactSize = bExactSize;

  public BytesList(int iCapacity) => this.EnsureFreeSpace(iCapacity);

  public BytesList(byte[] arrData)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    this.EnsureFreeSpace(arrData.Length);
    this.AddRange(arrData);
  }

  public void Add(byte bToAdd)
  {
    this.EnsureFreeSpace(1);
    this.m_arrBuffer[this.m_iCurPos] = bToAdd;
    ++this.m_iCurPos;
  }

  public void AddRange(byte[] arrToAdd)
  {
    int num = arrToAdd != null ? arrToAdd.Length : throw new ArgumentNullException(nameof (arrToAdd));
    if (num <= 0)
      return;
    this.EnsureFreeSpace(num);
    Buffer.BlockCopy((Array) arrToAdd, 0, (Array) this.m_arrBuffer, this.m_iCurPos, num);
    this.m_iCurPos += num;
  }

  public void AddRange(BytesList list)
  {
    int num = list != null ? list.Count : throw new ArgumentNullException(nameof (list));
    if (num <= 0)
      return;
    this.EnsureFreeSpace(num);
    Buffer.BlockCopy((Array) list.m_arrBuffer, 0, (Array) this.m_arrBuffer, this.m_iCurPos, num);
    this.m_iCurPos += num;
  }

  public void CopyTo(int iStartIndex, byte[] arrDest, int iDestIndex, int iCount)
  {
    int num = arrDest != null ? arrDest.Length : throw new ArgumentNullException(nameof (arrDest));
    if (iStartIndex < 0 || iStartIndex + iCount > this.m_iCurPos)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    if (iDestIndex < 0 || iDestIndex + iCount > num)
      throw new ArgumentOutOfRangeException(nameof (iDestIndex));
    Buffer.BlockCopy((Array) this.m_arrBuffer, iStartIndex, (Array) arrDest, iDestIndex, iCount);
  }

  public void EnsureFreeSpace(int iSize)
  {
    int length1 = this.m_arrBuffer == null ? 0 : this.m_arrBuffer.Length;
    int num = this.m_iCurPos + iSize;
    if (length1 >= num)
      return;
    int length2 = num;
    if (!this.m_bExactSize)
    {
      length2 = length1 == 0 ? 20 : length1 * 2;
      if (length2 < num)
        length2 = num;
    }
    byte[] dst = new byte[length2];
    if (length1 > 0)
      Buffer.BlockCopy((Array) this.m_arrBuffer, 0, (Array) dst, 0, length1);
    this.m_arrBuffer = dst;
  }

  internal byte[] InnerBuffer => this.m_arrBuffer;

  public int Count => this.m_iCurPos;
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.IntPtrContinueRecordBuilder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public class IntPtrContinueRecordBuilder : IDisposable
{
  protected BiffRecordWithContinue m_parent;
  protected int m_iPos;
  private int m_iContinuePos = -1;
  private int m_iContinueSize;
  private int m_iTotal;
  protected int m_iMax;
  private TBIFFRecord m_continueType = TBIFFRecord.Continue;
  private int m_iContinueCount;
  private int m_iFirstRecordLength = -1;
  private int m_iContinueHeaderSize;

  public int FreeSpace => this.m_iMax - this.m_iContinueSize;

  public int Total
  {
    get => this.m_iTotal;
    set => this.m_iTotal = value;
  }

  public int Position
  {
    get => this.m_iPos;
    set => this.m_iPos = value;
  }

  public int Offset
  {
    get
    {
      return this.m_iContinuePos <= 0 ? this.m_iPos : this.m_iPos - this.m_iContinuePos - this.m_iContinueHeaderSize;
    }
  }

  public int Max => this.m_iMax;

  public TBIFFRecord FirstContinueType => this.m_parent.FirstContinueType;

  public TBIFFRecord ContinueType
  {
    get => this.m_continueType;
    set => this.m_continueType = value;
  }

  public virtual int MaximumSize => 8224 - this.m_iContinueHeaderSize;

  public int FirstRecordLength => this.m_iFirstRecordLength;

  public event EventHandler OnFirstContinue;

  public IntPtrContinueRecordBuilder(BiffRecordWithContinue parent, int continueHeaderSize)
  {
    this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
    this.m_iMax = this.m_parent.MaximumRecordSize;
    this.m_iContinueHeaderSize = continueHeaderSize;
    this.m_iContinueSize = this.m_parent.Length;
    this.m_iPos = this.m_iContinueSize;
    this.m_iTotal = this.m_iContinueSize;
  }

  public void AppendByte(byte value)
  {
    if (this.CheckIfSpaceNeeded(1))
    {
      this.UpdateContinueRecordSize();
      this.StartContinueRecord();
    }
    this.m_parent.SetByte(this.m_iPos, value);
    this.UpdateCounters(1);
  }

  public virtual int AppendBytes(byte[] data, int start, int length)
  {
    int num1 = 0;
    if (this.CheckIfSpaceNeeded(length))
    {
      int num2 = start + length;
      for (int pos = start; pos < num2; pos += this.m_iMax)
      {
        this.UpdateContinueRecordSize();
        this.StartContinueRecord();
        ++num1;
        int num3 = num2 - pos < this.m_iMax ? num2 - pos : this.m_iMax;
        this.m_parent.SetBytes(this.m_iPos, data, pos, num3);
        this.UpdateCounters(num3);
      }
    }
    else
    {
      this.m_parent.SetBytes(this.m_iPos, data, start, length);
      this.UpdateCounters(length);
    }
    this.UpdateContinueRecordSize();
    return num1;
  }

  public void AppendUInt16(ushort value)
  {
    if (this.CheckIfSpaceNeeded(2))
    {
      this.UpdateContinueRecordSize();
      this.StartContinueRecord();
    }
    this.m_parent.SetUInt16(this.m_iPos, value);
    this.UpdateCounters(2);
  }

  public bool CheckIfSpaceNeeded(int length) => this.m_iContinueSize + length > this.m_iMax;

  public void StartContinueRecord()
  {
    if (this.OnFirstContinue != null)
      this.OnFirstContinue((object) this, EventArgs.Empty);
    if (this.m_iContinueCount == 0)
      this.m_iFirstRecordLength = this.m_iPos;
    ++this.m_iContinueCount;
    TBIFFRecord tbiffRecord = this.m_iContinueCount == 1 ? this.FirstContinueType : this.ContinueType;
    this.m_parent.m_arrContinuePos.Add(this.m_iPos);
    this.m_parent.SetUInt16(this.m_iPos, (ushort) tbiffRecord);
    this.m_iPos += 2;
    this.m_iContinuePos = this.m_iPos;
    this.m_iContinueSize = 0;
    this.m_parent.SetUInt16(this.m_iPos, (ushort) this.m_iContinueSize);
    this.m_iPos += 2;
    this.m_iTotal += 4;
    this.m_iMax = this.MaximumSize;
  }

  public void UpdateContinueRecordSize()
  {
    if (this.m_iContinuePos < 0)
      return;
    this.m_parent.SetUInt16(this.m_iContinuePos, (ushort) this.m_iContinueSize);
  }

  protected void UpdateCounters(int iLen)
  {
    this.m_iPos += iLen;
    this.m_iTotal += iLen;
    this.m_iContinueSize += iLen;
  }

  public void Dispose()
  {
    if (this.m_parent == null)
      return;
    this.m_parent = (BiffRecordWithContinue) null;
    GC.SuppressFinalize((object) this);
  }
}

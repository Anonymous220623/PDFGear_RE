// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.HeaderContinueRecordBuilder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public class HeaderContinueRecordBuilder : ContinueRecordBuilder
{
  public HeaderContinueRecordBuilder(BiffContinueRecordRaw parent)
    : base(parent)
  {
    this.ContinueType = TBIFFRecord.HeaderFooterImage;
    this.FirstContinueType = TBIFFRecord.HeaderFooterImage;
  }

  public override int MaximumSize => 8224 - HeaderFooterImageRecord.DEF_DATA_OFFSET;

  public override int AppendBytes(byte[] data, int start, int length)
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
        this.m_parent.SetBytes(this.m_iPos, HeaderFooterImageRecord.DEF_CONTINUE_START, 0, HeaderFooterImageRecord.DEF_DATA_OFFSET);
        this.UpdateCounters(HeaderFooterImageRecord.DEF_DATA_OFFSET);
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
}

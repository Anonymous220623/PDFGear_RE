// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ExternNameRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.ExternName)]
[CLSCompliant(false)]
internal class ExternNameRecord : BiffRecordRawWithArray
{
  [BiffRecordPos(0, 2)]
  private ExternNameRecord.OptionFlags m_options;
  [BiffRecordPos(2, 2)]
  private ushort m_usSheetId;
  [BiffRecordPos(4, 2)]
  private ushort m_usWord2;
  [BiffRecordPos(6, TFieldType.String)]
  private string m_strName = string.Empty;
  private ushort m_usFormulaSize;
  private byte[] m_arrFormulaData;
  private bool m_isAddIn;

  public ushort Options
  {
    get => (ushort) this.m_options;
    set => this.m_options = (ExternNameRecord.OptionFlags) value;
  }

  public ushort SheetId => this.m_usSheetId;

  public ushort Word2 => this.m_usWord2;

  public byte[] FormulaData => this.m_arrFormulaData;

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public override int MinimumRecordSize => 0;

  public ushort FormulaSize
  {
    get => this.m_usFormulaSize;
    set => this.m_usFormulaSize = value;
  }

  public override bool NeedDataArray
  {
    get
    {
      return this.m_options != (ExternNameRecord.OptionFlags) 0 && this.m_options != ExternNameRecord.OptionFlags.BuiltIn;
    }
  }

  public bool BuiltIn
  {
    get
    {
      return (this.m_options & ExternNameRecord.OptionFlags.BuiltIn) != (ExternNameRecord.OptionFlags) 0;
    }
    set => this.SetFlag(ExternNameRecord.OptionFlags.BuiltIn, value);
  }

  public bool WantAdvise
  {
    get
    {
      return (this.m_options & ExternNameRecord.OptionFlags.WantAdvise) != (ExternNameRecord.OptionFlags) 0;
    }
    set => this.SetFlag(ExternNameRecord.OptionFlags.WantAdvise, value);
  }

  public bool WantPicture
  {
    get
    {
      return (this.m_options & ExternNameRecord.OptionFlags.WantPicture) != (ExternNameRecord.OptionFlags) 0;
    }
    set => this.SetFlag(ExternNameRecord.OptionFlags.WantPicture, value);
  }

  public bool Ole
  {
    get => (this.m_options & ExternNameRecord.OptionFlags.Ole) != (ExternNameRecord.OptionFlags) 0;
    set => this.SetFlag(ExternNameRecord.OptionFlags.Ole, value);
  }

  public bool OleLink
  {
    get
    {
      return (this.m_options & ExternNameRecord.OptionFlags.OleLink) != (ExternNameRecord.OptionFlags) 0;
    }
    set => this.SetFlag(ExternNameRecord.OptionFlags.OleLink, value);
  }

  public bool IsAddIn
  {
    get => this.m_isAddIn;
    set => this.m_isAddIn = value;
  }

  private void SetFlag(ExternNameRecord.OptionFlags flag, bool value)
  {
    if (value)
      this.m_options |= flag;
    else
      this.m_options &= ~flag;
  }

  public ExternNameRecord()
  {
  }

  public ExternNameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExternNameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    int offset1 = 0;
    this.m_options = (ExternNameRecord.OptionFlags) this.GetUInt16(offset1);
    int offset2 = offset1 + 2;
    this.m_usSheetId = this.GetUInt16(offset2);
    int offset3 = offset2 + 2;
    this.m_usWord2 = this.GetUInt16(offset3);
    int offset4 = offset3 + 2;
    int iBytes;
    this.m_strName = this.GetStringByteLen(offset4, out iBytes);
    int num1 = offset4 + (iBytes + 2);
    if (this.BuiltIn)
    {
      this.m_usFormulaSize = this.GetUInt16(num1);
      num1 += 2;
    }
    else if (!this.OleLink)
      this.m_usFormulaSize = (ushort) (this.m_iLength - num1);
    this.m_arrFormulaData = new byte[(int) this.m_usFormulaSize];
    Buffer.BlockCopy((Array) this.m_data, num1, (Array) this.m_arrFormulaData, 0, (int) this.m_usFormulaSize);
    int num2 = num1 + (int) this.m_usFormulaSize;
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    if (!this.OleLink && !this.BuiltIn)
    {
      this.InfillDDELink();
    }
    else
    {
      if (this.m_options != (ExternNameRecord.OptionFlags) 0 && (this.m_options != ExternNameRecord.OptionFlags.BuiltIn || this.m_usFormulaSize != (ushort) 0))
        return;
      bool autoGrowData = this.AutoGrowData;
      this.AutoGrowData = true;
      this.SetUInt16(0, (ushort) this.m_options);
      this.SetUInt16(2, this.m_usSheetId);
      this.SetUInt16(4, this.m_usWord2);
      this.m_iLength = 6;
      this.SetString16BitUpdateOffset(ref this.m_iLength, this.m_strName);
      if (this.m_options == (ExternNameRecord.OptionFlags) 0 || this.m_options == ExternNameRecord.OptionFlags.BuiltIn)
      {
        this.SetUInt16(this.m_iLength, this.m_usFormulaSize);
        this.m_iLength += 2;
        if (this.m_usFormulaSize > (ushort) 0)
        {
          this.SetBytes(this.m_iLength, this.m_arrFormulaData, 0, (int) this.m_usFormulaSize);
          this.m_iLength += (int) this.m_usFormulaSize;
        }
      }
      this.AutoGrowData = autoGrowData;
    }
  }

  private void InfillDDELink()
  {
    bool autoGrowData = this.AutoGrowData;
    this.AutoGrowData = true;
    this.m_iLength = 0;
    this.SetUInt16(this.m_iLength, (ushort) this.m_options);
    this.m_iLength += 2;
    this.SetInt32(this.m_iLength, 0);
    this.m_iLength += 4;
    this.m_iLength += this.SetStringByteLen(this.m_iLength, this.m_strName);
    if (this.m_arrFormulaData != null && this.m_arrFormulaData.Length > 0)
    {
      this.SetBytes(this.m_iLength, this.m_arrFormulaData);
      this.m_iLength += this.m_arrFormulaData.Length;
    }
    else if (this.m_isAddIn)
    {
      this.m_arrFormulaData = new byte[4]
      {
        (byte) 2,
        (byte) 0,
        (byte) 28,
        (byte) 23
      };
      this.SetBytes(this.m_iLength, this.m_arrFormulaData);
      this.m_iLength += this.m_arrFormulaData.Length;
    }
    this.AutoGrowData = autoGrowData;
  }

  [Flags]
  private enum OptionFlags
  {
    BuiltIn = 1,
    WantAdvise = 2,
    WantPicture = 4,
    Ole = 8,
    OleLink = 16, // 0x00000010
  }
}

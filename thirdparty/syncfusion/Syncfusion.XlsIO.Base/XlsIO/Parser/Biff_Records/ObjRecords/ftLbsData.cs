// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftLbsData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
public class ftLbsData : ObjSubRecord
{
  private const int DEF_RECORD_SIZE = 20;
  private const int DEF_COLOR_BIT_INDEX = 3;
  private const int DEF_COLOR_BYTE = 10;
  private const int TypeValidMask = 1;
  private const int TypeMask = 65280;
  private const int TypeMaskStartBit = 8;
  private const int ThreeDMask = 8;
  private const int SelectionTypeMask = 48 /*0x30*/;
  private const int SelectionTypeStartBit = 4;
  private static readonly byte[] DEF_SAMPLE_RECORD_DATA = new byte[16 /*0x10*/]
  {
    (byte) 0,
    (byte) 0,
    (byte) 8,
    (byte) 0,
    (byte) 4,
    (byte) 0,
    (byte) 1,
    (byte) 3,
    (byte) 0,
    (byte) 0,
    (byte) 2,
    (byte) 0,
    (byte) 20,
    (byte) 0,
    (byte) 162,
    (byte) 0
  };
  private byte[] m_data;
  private int m_iLinesNumber;
  private Ptg[] m_arrFormula;
  private int m_iSelectedIndex;
  private int m_iOptions;
  private int m_iEditId;
  private LbsDropData m_dropData = new LbsDropData();
  private bool[] m_arrSelections;
  private bool m_bShortVersion;
  private TObjType m_parentObjectType = TObjType.otComboBox;

  public byte[] Data1
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public bool IsSelectedColor
  {
    get => BiffRecordRaw.GetBit(this.m_data, 10, 3);
    set => BiffRecordRaw.SetBit(this.m_data, 10, value, 3);
  }

  public int LinesNumber
  {
    get => this.m_iLinesNumber;
    set
    {
      if (this.m_iLinesNumber == value)
        return;
      this.m_iLinesNumber = value;
      if (this.m_arrSelections == null)
        return;
      bool[] arrSelections = this.m_arrSelections;
      this.m_arrSelections = new bool[value];
      Buffer.BlockCopy((Array) arrSelections, 0, (Array) this.m_arrSelections, 0, arrSelections.Length);
    }
  }

  public Ptg[] Formula
  {
    get => this.m_arrFormula;
    set => this.m_arrFormula = value;
  }

  public int SelectedIndex
  {
    get => this.m_iSelectedIndex;
    set => this.m_iSelectedIndex = value;
  }

  public int Options
  {
    get => this.m_iOptions;
    set => this.m_iOptions = value;
  }

  public int EditId
  {
    get => this.m_iEditId;
    set => this.m_iEditId = value;
  }

  public LbsDropData DropData => this.m_dropData;

  public bool ComboTypeValid
  {
    get => (this.m_iOptions & 1) != 0;
    set => this.m_iOptions = value ? this.m_iOptions | 1 : this.m_iOptions & -2;
  }

  public ExcelComboType ComboType
  {
    get
    {
      return !this.ComboTypeValid ? ExcelComboType.Regular : (ExcelComboType) ((this.m_iOptions & 65280) >> 8);
    }
    set
    {
      int num = (int) value << 8;
      this.m_iOptions &= -65281;
      this.m_iOptions |= num;
      this.ComboTypeValid = value != ExcelComboType.Regular;
    }
  }

  public bool NoThreeD
  {
    get => (this.m_iOptions & 8) != 0;
    set
    {
      if (value)
        this.m_iOptions |= 8;
      else
        this.m_iOptions &= -9;
    }
  }

  public ftLbsData.ExcelListSelectionType SelectionType
  {
    get => (ftLbsData.ExcelListSelectionType) ((this.m_iOptions & 48 /*0x30*/) >> 4);
    set
    {
      if (value == this.SelectionType)
        return;
      int num = (int) value << 4;
      this.m_iOptions &= -49;
      this.m_iOptions |= num;
      if (value == ftLbsData.ExcelListSelectionType.Single)
        this.m_arrSelections = (bool[]) null;
      else
        this.m_arrSelections = new bool[this.LinesNumber];
    }
  }

  public bool IsMultiSelection => this.SelectionType != ftLbsData.ExcelListSelectionType.Single;

  public ftLbsData()
    : base(TObjSubRecordType.ftLbsData)
  {
  }

  public ftLbsData(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  public ftLbsData(TObjSubRecordType type, ushort length, byte[] buffer, TObjType objectType)
    : base(type)
  {
    this.Length = length;
    this.m_parentObjectType = objectType;
    this.Parse(buffer);
  }

  protected override void Parse(byte[] buffer)
  {
    this.m_data = (byte[]) buffer.Clone();
    int startIndex1 = 0;
    int int16_1 = (int) BitConverter.ToInt16(buffer, startIndex1);
    int startIndex2 = startIndex1 + 2;
    if (int16_1 > 0)
    {
      int int16_2 = (int) BitConverter.ToInt16(buffer, startIndex2);
      int startIndex3 = startIndex2 + 2;
      BitConverter.ToInt32(buffer, startIndex3);
      int srcOffset = startIndex3 + 4;
      byte[] numArray = new byte[int16_2];
      Buffer.BlockCopy((Array) this.m_data, srcOffset, (Array) numArray, 0, int16_2);
      this.m_arrFormula = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(numArray), int16_2, ExcelVersion.Excel97to2003);
      startIndex2 = int16_1 + 2;
    }
    this.m_iLinesNumber = (int) BitConverter.ToInt16(buffer, startIndex2);
    int startIndex4 = startIndex2 + 2;
    this.m_iSelectedIndex = (int) BitConverter.ToInt16(buffer, startIndex4);
    int startIndex5 = startIndex4 + 2;
    if (startIndex5 >= buffer.Length)
    {
      this.m_bShortVersion = true;
    }
    else
    {
      this.m_iOptions = (int) BitConverter.ToInt16(buffer, startIndex5);
      int startIndex6 = startIndex5 + 2;
      this.m_iEditId = (int) BitConverter.ToInt16(buffer, startIndex6);
      int num = startIndex6 + 2;
      if (this.m_parentObjectType == TObjType.otComboBox)
        num = this.m_dropData.Parse((DataProvider) new ByteArrayDataProvider(buffer), num);
      if (!this.IsMultiSelection)
        return;
      this.ParseMultiSelection(buffer, num);
    }
  }

  private int ParseMultiSelection(byte[] buffer, int iOffset)
  {
    this.m_arrSelections = new bool[this.m_iLinesNumber];
    for (int index = 0; index < this.m_iLinesNumber && iOffset < buffer.Length; ++iOffset)
    {
      this.m_arrSelections[index] = buffer[iOffset] != (byte) 0;
      ++index;
    }
    return iOffset;
  }

  private void ParseLines(byte[] buffer, int iOffset)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public override object Clone()
  {
    ftLbsData ftLbsData = (ftLbsData) base.Clone();
    ftLbsData.m_data = CloneUtils.CloneByteArray(this.m_data);
    ftLbsData.m_arrFormula = CloneUtils.ClonePtgArray(this.m_arrFormula);
    ftLbsData.m_dropData = this.m_dropData.Clone();
    ftLbsData.m_arrSelections = CloneUtils.CloneBoolArray(this.m_arrSelections);
    return (object) ftLbsData;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int formulaSize = DVRecord.GetFormulaSize(this.m_arrFormula, ExcelVersion.Excel97to2003, false);
    if (formulaSize % 2 != 0)
      ++formulaSize;
    if (formulaSize > 0)
      formulaSize += 8;
    if (!this.m_bShortVersion)
      formulaSize += 6;
    int storeSize = formulaSize + 8;
    if (this.m_parentObjectType == TObjType.otComboBox)
      storeSize += this.m_dropData.GetStoreSize();
    if (this.m_arrSelections != null)
      storeSize += this.m_arrSelections.Length;
    return storeSize;
  }

  protected override void Serialize(DataProvider provider, int iOffset)
  {
    byte[] data = this.m_arrFormula != null ? FormulaUtil.PtgArrayToByteArray(this.m_arrFormula, ExcelVersion.Excel97to2003) : new byte[0];
    int length = data.Length;
    int num = length > 0 ? length + 2 + 4 : 0;
    bool flag = num % 2 != 0;
    if (flag)
      ++num;
    provider.WriteInt16(iOffset, (short) num);
    iOffset += 2;
    if (length > 0)
    {
      provider.WriteInt16(iOffset, (short) length);
      iOffset += 2;
      provider.WriteInt32(iOffset, 0);
      iOffset += 4;
      if (length > 0)
      {
        provider.WriteBytes(iOffset, data);
        iOffset += length;
      }
      if (flag)
      {
        provider.WriteByte(iOffset, (byte) 240 /*0xF0*/);
        ++iOffset;
      }
    }
    provider.WriteInt16(iOffset, (short) this.m_iLinesNumber);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iSelectedIndex);
    iOffset += 2;
    if (this.m_bShortVersion)
      return;
    provider.WriteInt16(iOffset, (short) this.m_iOptions);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iEditId);
    iOffset += 2;
    if (this.m_parentObjectType == TObjType.otComboBox)
    {
      if (this.ComboType == ExcelComboType.AutoFilter)
        this.m_dropData.Options = (short) 2;
      this.m_dropData.Serialize(provider, iOffset);
    }
    if (this.m_arrSelections == null)
      return;
    iOffset = this.SerializeMultiSelection(provider, iOffset);
  }

  private int SerializeMultiSelection(DataProvider provider, int iOffset)
  {
    int index = 0;
    int length = this.m_arrSelections.Length;
    while (index < length)
    {
      byte num = this.m_arrSelections[index] ? (byte) 1 : (byte) 0;
      provider.WriteByte(iOffset, num);
      ++index;
      ++iOffset;
    }
    return iOffset;
  }

  public enum ExcelListSelectionType
  {
    Single,
    MultiClick,
    MultiCtrl,
  }
}

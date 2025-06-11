// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.NameRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Name)]
internal class NameRecord : BiffRecordRaw
{
  public const ushort FunctionGroupBitMask = 4032;
  private const int DEF_FIXED_PART_SIZE = 14;
  private const string XLNM_ExtensionName = "_xlnm.";
  public static readonly string[] PREDEFINED_NAMES = new string[16 /*0x10*/]
  {
    "Consolidate_Area",
    "Auto_Open",
    "Auto_Close",
    "Extract",
    "Database",
    "Criteria",
    "Print_Area",
    "Print_Titles",
    "Recorder",
    "Data_Form",
    "Auto_Activate",
    "Auto_Deactivate",
    "Sheet_Title",
    "_FilterDatabase",
    "_xlnm.Print_Titles",
    "_xlnm.Print_Area"
  };
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bNameHidden;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bNameFunction;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bNameCommand;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bFCMacro;
  [BiffRecordPos(0, 4, TFieldType.Bit)]
  private bool m_bComplexFunction;
  [BiffRecordPos(0, 5, TFieldType.Bit)]
  private bool m_bBuinldInName;
  [BiffRecordPos(1, 4, TFieldType.Bit)]
  private bool m_bBinaryData;
  [BiffRecordPos(2, 1)]
  private byte m_bKeyboardShortcut;
  [BiffRecordPos(3, 1)]
  private byte m_bNameLength;
  [BiffRecordPos(4, 2)]
  private ushort m_usFormulaDataSize;
  [BiffRecordPos(6, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(8, 2)]
  private ushort m_usIndexOrGlobal;
  [BiffRecordPos(10, 1)]
  private byte m_bMenuTextLength;
  [BiffRecordPos(11, 1)]
  private byte m_bDescriptionLength;
  [BiffRecordPos(12, 1)]
  private byte m_bHelpTextLength;
  [BiffRecordPos(13, 1)]
  private byte m_bStatusTextLength;
  private string m_strName = string.Empty;
  private byte[] m_arrFormulaData;
  private string m_strMenuText = string.Empty;
  private string m_strDescription = string.Empty;
  private string m_strHelpText = string.Empty;
  private string m_strStatusText = string.Empty;
  private Ptg[] m_arrToken;

  public bool IsNameHidden
  {
    get => this.m_bNameHidden;
    set => this.m_bNameHidden = value;
  }

  public bool IsNameFunction
  {
    get => this.m_bNameFunction;
    set => this.m_bNameFunction = value;
  }

  public bool IsNameCommand
  {
    get => this.m_bNameCommand;
    set => this.m_bNameCommand = value;
  }

  public bool IsFunctionOrCommandMacro
  {
    get => this.m_bFCMacro;
    set => this.m_bFCMacro = value;
  }

  public bool IsComplexFunction
  {
    get => this.m_bComplexFunction;
    set => this.m_bComplexFunction = value;
  }

  public bool IsBuinldInName
  {
    get => this.m_bBuinldInName;
    set => this.m_bBuinldInName = value;
  }

  public bool HasBinaryData
  {
    get => this.m_bBinaryData;
    set => this.m_bBinaryData = value;
  }

  public ushort FunctionGroupIndex
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 4032) >> 6);
    }
    set
    {
      if (value > (ushort) 63 /*0x3F*/)
        throw new ArgumentOutOfRangeException("FunctionGroupIndex too large.");
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 4032, (ushort) ((uint) value << 6));
    }
  }

  public byte KeyboardShortcut
  {
    get => this.m_bKeyboardShortcut;
    set => this.m_bKeyboardShortcut = value;
  }

  public byte NameLength => this.m_bNameLength;

  public ushort FormulaDataSize => this.m_usFormulaDataSize;

  public ushort IndexOrGlobal
  {
    get => this.m_usIndexOrGlobal;
    set => this.m_usIndexOrGlobal = value;
  }

  public byte MenuTextLength => this.m_bKeyboardShortcut;

  public byte DescriptionLength => this.m_bDescriptionLength;

  public byte HelpTextLength => this.m_bHelpTextLength;

  public byte StatusTextLength => this.m_bStatusTextLength;

  public string Name
  {
    get => this.m_strName;
    set
    {
      this.m_strName = value;
      this.m_bNameLength = NameRecord.IsPredefinedName(this.m_strName) ? (byte) 1 : (this.m_strName != null ? (byte) this.m_strName.Length : (byte) 0);
    }
  }

  public Ptg[] FormulaTokens
  {
    get => this.m_arrToken;
    set
    {
      this.m_arrToken = value;
      if (value != null)
      {
        int formulaLen;
        this.m_arrFormulaData = FormulaUtil.PtgArrayToByteArray(value, out formulaLen, OfficeVersion.Excel2007);
        this.m_usFormulaDataSize = (ushort) formulaLen;
      }
      else
      {
        this.m_usFormulaDataSize = (ushort) 0;
        this.m_arrFormulaData = (byte[]) null;
      }
    }
  }

  public string MenuText
  {
    get => this.m_strMenuText;
    set
    {
      this.m_strMenuText = value;
      this.m_bMenuTextLength = this.m_strMenuText != null ? (byte) this.m_strMenuText.Length : (byte) 0;
    }
  }

  public string Description
  {
    get => this.m_strDescription;
    set
    {
      this.m_strDescription = value;
      this.m_bDescriptionLength = this.m_strDescription != null ? (byte) this.m_strDescription.Length : (byte) 0;
    }
  }

  public string HelpText
  {
    get => this.m_strHelpText;
    set
    {
      this.m_strHelpText = value;
      this.m_bHelpTextLength = this.m_strHelpText != null ? (byte) this.m_strHelpText.Length : (byte) 0;
    }
  }

  public string StatusText
  {
    get => this.m_strStatusText;
    set
    {
      this.m_strStatusText = value;
      this.m_bStatusTextLength = this.m_strStatusText != null ? (byte) this.m_strStatusText.Length : (byte) 0;
    }
  }

  public ushort Reserved => this.m_usReserved;

  public override int MinimumRecordSize => 14;

  public NameRecord()
  {
  }

  public NameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public NameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.ParseFixedPart(provider, iOffset);
    int finalOffset = iOffset + 14;
    if (this.m_bNameLength != (byte) 0)
      this.m_strName = provider.ReadStringUpdateOffset(ref finalOffset, (int) this.m_bNameLength);
    else
      ++finalOffset;
    if (this.IsBuinldInName)
    {
      if (this.m_strName.Length == 1)
        this.m_strName = NameRecord.PREDEFINED_NAMES[(int) this.m_strName[0]];
    }
    try
    {
      this.FormulaTokens = FormulaUtil.ParseExpression(provider, finalOffset, (int) this.m_usFormulaDataSize, out finalOffset, version);
    }
    catch (Exception ex)
    {
      throw;
    }
    this.m_strMenuText = provider.ReadStringUpdateOffset(ref finalOffset, (int) this.m_bMenuTextLength);
    this.m_strDescription = provider.ReadStringUpdateOffset(ref finalOffset, (int) this.m_bDescriptionLength);
    this.m_strHelpText = provider.ReadStringUpdateOffset(ref finalOffset, (int) this.m_bHelpTextLength);
    this.m_strStatusText = provider.ReadStringUpdateOffset(ref finalOffset, (int) this.m_bStatusTextLength);
    if (finalOffset != this.m_iLength)
      throw new WrongBiffRecordDataException(nameof (NameRecord));
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    if (this.m_strName.Equals(NameRecord.PREDEFINED_NAMES[14]))
      this.m_strName = this.m_strName.Substring("_xlnm.".Length);
    bool bUnicode = !BiffRecordRawWithArray.IsAsciiString(this.m_strName);
    if (this.m_arrToken != null && this.m_arrToken.Length > 0)
    {
      int formulaLen;
      this.m_arrFormulaData = FormulaUtil.PtgArrayToByteArray(this.m_arrToken, out formulaLen, version);
      this.m_usFormulaDataSize = (ushort) formulaLen;
    }
    else
    {
      this.m_arrFormulaData = (byte[]) null;
      this.m_usFormulaDataSize = (ushort) 0;
    }
    this.m_iLength = this.GetStoreSize(version);
    this.InfillFixedPart(provider, iOffset);
    iOffset += 14;
    if (this.IsBuinldInName)
    {
      provider.WriteByte(iOffset, (byte) 0);
      int num1 = NameRecord.PredefinedIndex(this.m_strName);
      if (num1 != -1)
      {
        byte num2 = num1 < 0 ? (byte) this.m_strName[0] : (byte) num1;
        provider.WriteByte(iOffset + 1, num2);
        iOffset += 2;
      }
      else
        provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strName, bUnicode);
    }
    else
      provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strName, bUnicode);
    if (this.m_bNameLength == (byte) 0)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++iOffset;
    }
    if (this.m_arrFormulaData != null)
    {
      provider.WriteBytes(iOffset, this.m_arrFormulaData, 0, this.m_arrFormulaData.Length);
      iOffset += this.m_arrFormulaData.Length;
    }
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strMenuText);
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strDescription);
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strHelpText);
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strStatusText);
  }

  private void InfillFixedPart(DataProvider provider, int iOffset)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bNameHidden, 0);
    provider.WriteBit(iOffset, this.m_bNameFunction, 1);
    provider.WriteBit(iOffset, this.m_bNameCommand, 2);
    provider.WriteBit(iOffset, this.m_bFCMacro, 3);
    provider.WriteBit(iOffset, this.m_bComplexFunction, 4);
    provider.WriteBit(iOffset, this.m_bBuinldInName, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bBinaryData, 4);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_bKeyboardShortcut);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_bNameLength);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usFormulaDataSize);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usReserved);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usIndexOrGlobal);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_bMenuTextLength);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_bDescriptionLength);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_bHelpTextLength);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_bStatusTextLength);
    ++iOffset;
  }

  private void ParseFixedPart(DataProvider provider, int iOffset)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bNameHidden = provider.ReadBit(iOffset, 0);
    this.m_bNameFunction = provider.ReadBit(iOffset, 1);
    this.m_bNameCommand = provider.ReadBit(iOffset, 2);
    this.m_bFCMacro = provider.ReadBit(iOffset, 3);
    this.m_bComplexFunction = provider.ReadBit(iOffset, 4);
    this.m_bBuinldInName = provider.ReadBit(iOffset, 5);
    this.m_bBinaryData = provider.ReadBit(iOffset + 1, 4);
    this.m_bKeyboardShortcut = provider.ReadByte(iOffset + 2);
    this.m_bNameLength = provider.ReadByte(iOffset + 3);
    this.m_usFormulaDataSize = provider.ReadUInt16(iOffset + 4);
    this.m_usReserved = provider.ReadUInt16(iOffset + 6);
    this.m_usIndexOrGlobal = provider.ReadUInt16(iOffset + 8);
    this.m_bMenuTextLength = provider.ReadByte(iOffset + 10);
    this.m_bDescriptionLength = provider.ReadByte(iOffset + 11);
    this.m_bHelpTextLength = provider.ReadByte(iOffset + 12);
    this.m_bStatusTextLength = provider.ReadByte(iOffset + 13);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int num = 14;
    Encoding encoding = !BiffRecordRawWithArray.IsAsciiString(this.m_strName) ? Encoding.Unicode : Encoding.UTF8;
    return (!this.IsBuinldInName ? num + (encoding.GetByteCount(this.m_strName) + 1) : (Array.IndexOf<string>(NameRecord.PREDEFINED_NAMES, this.m_strName) == -1 ? num + (encoding.GetByteCount(this.m_strName) + 1) : num + 2)) + DVRecord.GetFormulaSize(this.m_arrToken, version, true) + this.GetByteCount(this.m_strMenuText) + this.GetByteCount(this.m_strDescription) + this.GetByteCount(this.m_strHelpText) + this.GetByteCount(this.m_strStatusText);
  }

  private int GetByteCount(string strValue)
  {
    return strValue == null || strValue.Length <= 0 ? 0 : Encoding.Unicode.GetByteCount(strValue) + 1;
  }

  public static bool IsPredefinedName(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    return NameRecord.PredefinedIndex(value) >= 0;
  }

  public static int PredefinedIndex(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    int num = -1;
    if (value.Length > 0)
    {
      int index = 0;
      for (int length = NameRecord.PREDEFINED_NAMES.Length; index < length; ++index)
      {
        string str = NameRecord.PREDEFINED_NAMES[index];
        if (value.StartsWith(str))
        {
          num = index;
          break;
        }
      }
    }
    return num;
  }

  public override object Clone()
  {
    NameRecord nameRecord = (NameRecord) base.Clone();
    nameRecord.FormulaTokens = CloneUtils.ClonePtgArray(this.m_arrToken);
    return (object) nameRecord;
  }

  public override void ClearData()
  {
    this.m_arrFormulaData = (byte[]) null;
    this.m_arrToken = (Ptg[]) null;
  }

  internal void Delete()
  {
    this.m_usFormulaDataSize = (ushort) 0;
    this.m_arrFormulaData = (byte[]) null;
    this.m_arrToken = (Ptg[]) null;
  }
}

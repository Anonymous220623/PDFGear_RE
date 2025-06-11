// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HLinkRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.HLink)]
[CLSCompliant(false)]
internal class HLinkRecord : BiffRecordRawWithArray
{
  public const int GUID_LENGTH = 16 /*0x10*/;
  public const int STDLINK_START_BYTE = 8;
  public const int URLMONIKER_START_BYTE = 0;
  public const int FILEMONIKER_START_BYTE = 0;
  public static readonly Guid GUID_STDLINK = new Guid("79EAC9D0-BAF9-11CE-8C82-00AA004BA90B");
  public static readonly Guid GUID_URLMONIKER = new Guid("79EAC9E0-BAF9-11CE-8C82-00AA004BA90B");
  public static readonly Guid GUID_FILEMONIKER = new Guid("00000303-0000-0000-C000-000000000046");
  public static readonly byte[] GUID_STDLINK_BYTES = HLinkRecord.GUID_STDLINK.ToByteArray();
  public static readonly byte[] GUID_URLMONIKER_BYTES = HLinkRecord.GUID_URLMONIKER.ToByteArray();
  public static readonly byte[] GUID_FILEMONIKER_BYTES = HLinkRecord.GUID_FILEMONIKER.ToByteArray();
  public static readonly byte[] FILE_UNKNOWN = new byte[24]
  {
    byte.MaxValue,
    byte.MaxValue,
    (byte) 173,
    (byte) 222,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  public static readonly byte[] FILE_UNKNOWN2 = new byte[2]
  {
    (byte) 3,
    (byte) 0
  };
  [BiffRecordPos(0, 2)]
  private uint m_usFirstRow;
  [BiffRecordPos(2, 2)]
  private uint m_usLastRow;
  [BiffRecordPos(4, 2)]
  private uint m_usFirstColumn;
  [BiffRecordPos(6, 2)]
  private uint m_usLastColumn;
  [BiffRecordPos(24, 4)]
  private uint m_uiUnknown = 2;
  [BiffRecordPos(28, 4)]
  private uint m_uiOptions;
  [BiffRecordPos(28, 0, TFieldType.Bit)]
  private bool m_bFileOrUrl;
  [BiffRecordPos(28, 1, TFieldType.Bit)]
  private bool m_bAbsolutePathOrUrl;
  [BiffRecordPos(28, 2, TFieldType.Bit)]
  private bool m_bDescription1;
  [BiffRecordPos(28, 3, TFieldType.Bit)]
  private bool m_bTextMark;
  [BiffRecordPos(28, 4, TFieldType.Bit)]
  private bool m_bDescription2;
  [BiffRecordPos(28, 7, TFieldType.Bit)]
  private bool m_bTargetFrame;
  [BiffRecordPos(29, 0, TFieldType.Bit)]
  private bool m_bUncPath;
  private uint m_uiDescriptionLen;
  private string m_strDescription = string.Empty;
  private uint m_uiTargetFrameLen;
  private string m_strTargetFrame = string.Empty;
  private uint m_uiTextMarkLen;
  private string m_strTextMark = string.Empty;
  private ExcelHyperLinkType m_LinkType;
  private uint m_uiUrlLen;
  private string m_strUrl = string.Empty;
  private ushort m_usDirUpLevel;
  private uint m_uiFileNameLen;
  private string m_strFileName = string.Empty;
  private uint m_uiFollowSize;
  private uint m_uiXFilePathLen;
  private string m_strXFilePath;
  private uint m_uiUncLen;
  private string m_strUnc;

  public uint FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public uint FirstColumn
  {
    get => this.m_usFirstColumn;
    set => this.m_usFirstColumn = value;
  }

  public uint LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public uint LastColumn
  {
    get => this.m_usLastColumn;
    set => this.m_usLastColumn = value;
  }

  public uint Unknown => this.m_uiUnknown;

  public uint Options => this.m_uiOptions;

  public bool IsFileOrUrl
  {
    get => this.m_bFileOrUrl;
    set => this.m_bFileOrUrl = value;
  }

  public bool IsAbsolutePathOrUrl
  {
    get => this.m_bAbsolutePathOrUrl;
    set => this.m_bAbsolutePathOrUrl = value;
  }

  public bool IsDescription
  {
    get => this.m_bDescription1 && this.m_bDescription2;
    set
    {
      this.m_bDescription1 = value;
      this.m_bDescription2 = value;
    }
  }

  public bool IsTextMark
  {
    get => this.m_bTextMark;
    set => this.m_bTextMark = value;
  }

  public bool IsTargetFrame
  {
    get => this.m_bTargetFrame;
    set => this.m_bTargetFrame = value;
  }

  public bool IsUncPath
  {
    get => this.m_bUncPath;
    set => this.m_bUncPath = value;
  }

  public bool CanBeUrl
  {
    get => this.IsFileOrUrl && this.IsAbsolutePathOrUrl && !this.IsUncPath;
    set
    {
      this.IsFileOrUrl = value;
      this.IsAbsolutePathOrUrl = value;
      this.IsUncPath = !value;
    }
  }

  public bool CanBeFile
  {
    get => this.IsFileOrUrl && !this.IsUncPath;
    set
    {
      this.IsFileOrUrl = value;
      this.IsUncPath = !value;
    }
  }

  public bool CanBeUnc
  {
    get => this.IsFileOrUrl && this.IsAbsolutePathOrUrl && this.IsUncPath;
    set
    {
      if (!value)
        return;
      this.IsFileOrUrl = value;
      this.IsAbsolutePathOrUrl = value;
      this.IsUncPath = value;
    }
  }

  public bool CanBeWorkbook
  {
    get => !this.IsFileOrUrl && !this.IsAbsolutePathOrUrl && this.IsTextMark && !this.IsUncPath;
    set
    {
      if (!value)
        return;
      this.IsFileOrUrl = false;
      this.IsAbsolutePathOrUrl = false;
      this.IsTextMark = true;
      this.IsUncPath = false;
    }
  }

  public uint DescriptionLen => this.m_uiDescriptionLen;

  public string Description
  {
    get => this.m_strDescription;
    set
    {
      if (!(this.m_strDescription != value))
        return;
      this.m_strDescription = value != null ? value : string.Empty;
      this.m_uiDescriptionLen = value == null ? 0U : (uint) (this.m_strDescription.Length + 1);
      this.IsDescription = true;
    }
  }

  public uint TargetFrameLen => this.m_uiTargetFrameLen;

  public string TargetFrame
  {
    get => this.m_strTargetFrame;
    set
    {
      if (!(this.m_strTargetFrame != value))
        return;
      this.m_strTargetFrame = value != null ? value : string.Empty;
      this.m_uiTargetFrameLen = value == null ? 0U : (uint) (this.m_strTargetFrame.Length + 1);
    }
  }

  public uint TextMarkLen => this.m_uiTextMarkLen;

  public string TextMark
  {
    get => this.m_strTextMark;
    set
    {
      if (!(this.m_strTextMark != value))
        return;
      this.m_strTextMark = value != null ? value : string.Empty;
      this.m_uiTextMarkLen = value == null ? 0U : (uint) (this.m_strTextMark.Length + 1);
      this.m_bTextMark = true;
    }
  }

  public ExcelHyperLinkType LinkType
  {
    get => this.m_LinkType;
    set
    {
      switch (value)
      {
        case ExcelHyperLinkType.Url:
          this.CanBeUrl = true;
          break;
        case ExcelHyperLinkType.File:
          this.CanBeFile = true;
          break;
        case ExcelHyperLinkType.Unc:
          this.CanBeUnc = true;
          break;
        case ExcelHyperLinkType.Workbook:
          this.CanBeWorkbook = true;
          break;
      }
      this.m_LinkType = value;
    }
  }

  public bool IsUrl
  {
    get => this.m_LinkType == ExcelHyperLinkType.Url;
    set
    {
      if (!value)
        return;
      this.m_LinkType = ExcelHyperLinkType.Url;
      this.CanBeUrl = true;
    }
  }

  public bool IsFileName
  {
    get => this.m_LinkType == ExcelHyperLinkType.File;
    set
    {
      if (!value)
        return;
      this.m_LinkType = ExcelHyperLinkType.File;
      this.CanBeFile = true;
    }
  }

  public uint UrlLen => this.m_uiUrlLen;

  public string Url
  {
    get => this.m_strUrl;
    set
    {
      if (!(this.m_strUrl != value))
        return;
      this.m_strUrl = value;
      this.m_uiUrlLen = value != null ? (uint) (value.Length * 2 + 2) : 2U;
      this.IsUrl = true;
    }
  }

  public ushort DirUpLevel
  {
    get => this.m_usDirUpLevel;
    set => this.m_usDirUpLevel = value;
  }

  public uint FileNameLen => this.m_uiFileNameLen;

  public string FileName
  {
    get => this.m_strFileName;
    set
    {
      this.m_strFileName = value != null ? value : string.Empty;
      this.m_uiFileNameLen = (uint) this.m_strFileName.Length;
      this.IsFileName = true;
    }
  }

  public uint FollowSize => this.m_uiFollowSize;

  public uint XFilePathLen => this.m_uiXFilePathLen;

  public string XFilePath
  {
    get => this.m_strXFilePath;
    set
    {
      this.m_strXFilePath = value != null ? value : string.Empty;
      this.m_uiXFilePathLen = (uint) (this.m_strXFilePath.Length * 2 + 2);
    }
  }

  public uint UncLen => this.m_uiUncLen;

  public string UncPath
  {
    get => this.m_strUnc;
    set
    {
      this.m_strUnc = value;
      this.m_uiUncLen = (uint) (this.m_strUnc.Length + 1);
    }
  }

  public HLinkRecord()
  {
  }

  public HLinkRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HLinkRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_usFirstRow = (uint) this.GetUInt16(0);
    this.m_usLastRow = (uint) this.GetUInt16(2);
    this.m_usFirstColumn = (uint) this.GetUInt16(4);
    this.m_usLastColumn = (uint) this.GetUInt16(6);
    this.m_uiUnknown = this.GetUInt32(24);
    this.m_uiOptions = this.GetUInt32(28);
    this.m_bFileOrUrl = this.GetBit(28, 0);
    this.m_bAbsolutePathOrUrl = this.GetBit(28, 1);
    this.m_bDescription1 = this.GetBit(28, 2);
    this.m_bTextMark = this.GetBit(28, 3);
    this.m_bDescription2 = this.GetBit(28, 4);
    this.m_bTargetFrame = this.GetBit(28, 7);
    this.m_bUncPath = this.GetBit(29, 0);
    int iOffset = 32 /*0x20*/;
    if (this.IsDescription)
      this.ParseDescription(ref iOffset);
    if (this.IsTargetFrame)
      this.ParseTargetFrame(ref iOffset);
    this.ParseSpecialData(ref iOffset);
    if (!this.IsTextMark)
      return;
    this.ParseTextMark(ref iOffset);
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.SetOptionFlags();
    this.m_uiUnknown = 2U;
    this.AutoGrowData = true;
    this.SetUInt16(0, (ushort) this.m_usFirstRow);
    this.SetUInt16(2, (ushort) this.m_usLastRow);
    this.SetUInt16(4, (ushort) this.m_usFirstColumn);
    this.SetUInt16(6, (ushort) this.m_usLastColumn);
    this.SetUInt32(24, this.m_uiUnknown);
    this.SetUInt32(28, this.m_uiOptions);
    this.SetBit(28, this.m_bFileOrUrl, 0);
    this.SetBit(28, this.m_bAbsolutePathOrUrl, 1);
    this.SetBit(28, this.m_bDescription1, 2);
    this.SetBit(28, this.m_bTextMark, 3);
    this.SetBit(28, this.m_bDescription2, 4);
    this.SetBit(28, this.m_bTargetFrame, 7);
    this.SetBit(29, this.m_bUncPath, 0);
    this.m_iLength = 32 /*0x20*/;
    this.SetBytes(8, HLinkRecord.GUID_STDLINK_BYTES, 0, HLinkRecord.GUID_STDLINK_BYTES.Length);
    if (this.IsDescription)
      this.InfillLenAndString(ref this.m_uiDescriptionLen, ref this.m_strDescription, false);
    if (this.IsTargetFrame)
      this.InfillLenAndString(ref this.m_uiTargetFrameLen, ref this.m_strTargetFrame, false);
    this.InfillSpecialData();
    if (!this.IsTextMark)
      return;
    this.InfillLenAndString(ref this.m_uiTextMarkLen, ref this.m_strTextMark, false);
  }

  private void ParseDescription(ref int iOffset)
  {
    if (!this.IsDescription)
      throw new ArgumentException("There is no description.");
    this.m_uiDescriptionLen = this.GetUInt32(iOffset);
    iOffset += 4;
    if ((long) iOffset + (long) (this.m_uiDescriptionLen * 2U) > (long) this.m_iLength)
      throw new WrongBiffRecordDataException("Description");
    this.m_strDescription = Encoding.Unicode.GetString(this.GetBytes(iOffset, (int) this.m_uiDescriptionLen * 2), 0, (int) this.m_uiDescriptionLen * 2 - 2);
    iOffset += (int) this.m_uiDescriptionLen * 2;
  }

  private void ParseTargetFrame(ref int iOffset)
  {
    if (!this.IsTargetFrame)
      throw new ArgumentException("There is no target frame.");
    this.m_uiTargetFrameLen = this.GetUInt32(iOffset);
    iOffset += 4;
    this.m_strTargetFrame = Encoding.Unicode.GetString(this.GetBytes(iOffset, (int) this.m_uiTargetFrameLen * 2), 0, (int) this.m_uiTargetFrameLen * 2 - 2);
    iOffset += (int) this.m_uiTargetFrameLen * 2;
  }

  private void ParseSpecialData(ref int iOffset)
  {
    if (this.CheckUrl(ref iOffset))
    {
      this.LinkType = ExcelHyperLinkType.Url;
      this.ParseUrl(ref iOffset);
    }
    else if (this.CheckLocalFile(ref iOffset))
    {
      this.LinkType = ExcelHyperLinkType.File;
      this.ParseFile(ref iOffset);
    }
    else if (this.CheckUnc(ref iOffset))
    {
      this.LinkType = ExcelHyperLinkType.Unc;
      this.ParseUnc(ref iOffset);
    }
    else
    {
      this.LinkType = ExcelHyperLinkType.Workbook;
      this.ParseWorkbook(ref iOffset);
    }
  }

  private void ParseTextMark(ref int iOffset)
  {
    if (!this.IsTextMark)
      throw new ArgumentException("There is no text mark.");
    if (iOffset >= this.m_iLength)
      return;
    this.m_uiTextMarkLen = this.GetUInt32(iOffset);
    iOffset += 4;
    this.m_strTextMark = Encoding.Unicode.GetString(this.m_data, iOffset, ((int) this.m_uiTextMarkLen - 1) * 2);
    iOffset += (int) this.m_uiTextMarkLen * 2;
  }

  private bool CheckUrl(ref int iOffset)
  {
    bool flag = BiffRecordRaw.CompareArrays(this.m_data, iOffset, HLinkRecord.GUID_URLMONIKER_BYTES, 0, 16 /*0x10*/);
    if (flag)
      iOffset += 16 /*0x10*/;
    return flag;
  }

  private bool CheckLocalFile(ref int iOffset)
  {
    bool flag = this.CanBeFile && BiffRecordRaw.CompareArrays(this.m_data, iOffset, HLinkRecord.GUID_FILEMONIKER_BYTES, 0, 16 /*0x10*/);
    if (flag)
      iOffset += 16 /*0x10*/;
    return flag;
  }

  private bool CheckUnc(ref int iOffset) => this.CanBeUnc;

  private void ParseUrl(ref int iOffset)
  {
    this.m_uiUrlLen = this.GetUInt32(iOffset);
    iOffset += 4;
    this.m_strUrl = Encoding.Unicode.GetString(this.m_data, iOffset, (int) this.m_uiUrlLen);
    int length = this.m_strUrl.IndexOf(char.MinValue);
    if (length != -1)
      this.m_strUrl = this.m_strUrl.Substring(0, length);
    iOffset += (int) this.m_uiUrlLen;
  }

  private void ParseFile(ref int iOffset)
  {
    this.m_usDirUpLevel = this.GetUInt16(iOffset);
    iOffset += 2;
    this.m_uiFileNameLen = this.GetUInt32(iOffset);
    iOffset += 4;
    this.m_strFileName = BiffRecordRaw.LatinEncoding.GetString(this.m_data, iOffset, (int) this.m_uiFileNameLen - 1);
    iOffset += (int) this.m_uiFileNameLen;
    --this.m_uiFileNameLen;
    iOffset += HLinkRecord.FILE_UNKNOWN.Length;
    this.m_uiFollowSize = this.GetUInt32(iOffset);
    iOffset += 4;
    if (this.m_uiFollowSize <= 0U)
      return;
    int int32 = this.GetInt32(iOffset);
    iOffset += 4;
    iOffset += 2;
    this.m_strXFilePath = Encoding.Unicode.GetString(this.m_data, iOffset, int32);
    iOffset += int32;
  }

  private void ParseUnc(ref int iOffset)
  {
    this.m_uiUncLen = this.GetUInt32(iOffset);
    iOffset += 4;
    this.m_strUnc = Encoding.Unicode.GetString(this.m_data, iOffset, (int) this.m_uiUncLen * 2 - 2);
    iOffset += (int) this.m_uiUncLen * 2;
  }

  private void ParseWorkbook(ref int iOffset)
  {
  }

  private void InfillLenAndString(ref uint uiLen, ref string strValue, bool bBytesCount)
  {
    if (strValue == null || strValue.Length == 0)
      strValue = "\0";
    uiLen = (uint) strValue.Length;
    if (strValue[(int) uiLen - 1] != char.MinValue)
    {
      strValue += (string) (object) char.MinValue;
      ++uiLen;
    }
    if (bBytesCount)
      uiLen *= 2U;
    this.SetUInt32(this.m_iLength, uiLen);
    this.m_iLength += 4;
    byte[] bytes = Encoding.Unicode.GetBytes(strValue);
    this.SetBytes(this.m_iLength, bytes);
    this.m_iLength += bytes.Length;
  }

  private void InfillSpecialData()
  {
    switch (this.LinkType)
    {
      case ExcelHyperLinkType.Url:
        this.InfillUrlSpecialData();
        break;
      case ExcelHyperLinkType.File:
        this.InfillFileSpecialData();
        break;
      case ExcelHyperLinkType.Unc:
        this.InfillUncSpecialData();
        break;
      case ExcelHyperLinkType.Workbook:
        this.InfillWorkbookSpecialData();
        break;
    }
  }

  private void InfillFileSpecialData()
  {
    this.SetBytes(this.m_iLength, HLinkRecord.GUID_FILEMONIKER_BYTES);
    this.m_iLength += HLinkRecord.GUID_FILEMONIKER_BYTES.Length;
    this.SetUInt16(this.m_iLength, this.m_usDirUpLevel);
    this.m_iLength += 2;
    if (this.m_strFileName[(int) this.m_uiFileNameLen - 1] != char.MinValue)
    {
      ++this.m_uiFileNameLen;
      this.m_strFileName += (string) (object) char.MinValue;
    }
    this.SetUInt32(this.m_iLength, this.m_uiFileNameLen);
    this.m_iLength += 4;
    byte[] bytes1 = BiffRecordRaw.LatinEncoding.GetBytes(this.m_strFileName);
    this.SetBytes(this.m_iLength, bytes1);
    this.m_iLength += bytes1.Length;
    this.SetBytes(this.m_iLength, HLinkRecord.FILE_UNKNOWN);
    this.m_iLength += HLinkRecord.FILE_UNKNOWN.Length;
    this.m_uiFollowSize = this.m_strXFilePath == null || this.m_strXFilePath.Length == 0 ? 0U : (uint) (6 + this.m_strXFilePath.Length * 2);
    this.SetUInt32(this.m_iLength, this.m_uiFollowSize);
    this.m_iLength += 4;
    if (this.m_uiFollowSize == 0U)
      return;
    this.m_uiXFilePathLen = (uint) (this.m_strXFilePath.Length * 2);
    this.SetUInt32(this.m_iLength, this.m_uiXFilePathLen);
    this.m_iLength += 4;
    this.SetBytes(this.m_iLength, HLinkRecord.FILE_UNKNOWN2);
    this.m_iLength += HLinkRecord.FILE_UNKNOWN2.Length;
    byte[] bytes2 = Encoding.Unicode.GetBytes(this.m_strXFilePath);
    this.SetBytes(this.m_iLength, bytes2);
    this.m_iLength += bytes2.Length;
  }

  private void InfillUncSpecialData()
  {
    this.CanBeUnc = true;
    this.InfillLenAndString(ref this.m_uiUncLen, ref this.m_strUnc, false);
  }

  private void InfillUrlSpecialData()
  {
    this.CanBeUrl = true;
    this.SetBytes(this.m_iLength, HLinkRecord.GUID_URLMONIKER_BYTES);
    this.m_iLength += HLinkRecord.GUID_URLMONIKER_BYTES.Length;
    this.InfillLenAndString(ref this.m_uiUrlLen, ref this.m_strUrl, true);
  }

  private void InfillWorkbookSpecialData() => this.CanBeWorkbook = true;

  private void SetOptionFlags()
  {
    switch (this.LinkType)
    {
      case ExcelHyperLinkType.Url:
        this.CanBeUrl = true;
        break;
      case ExcelHyperLinkType.File:
        this.CanBeFile = true;
        break;
      case ExcelHyperLinkType.Unc:
        this.CanBeUnc = true;
        break;
      case ExcelHyperLinkType.Workbook:
        this.CanBeWorkbook = true;
        break;
    }
  }
}

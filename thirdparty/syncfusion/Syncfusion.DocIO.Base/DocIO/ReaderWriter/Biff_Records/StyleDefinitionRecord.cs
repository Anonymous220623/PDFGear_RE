// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.StyleDefinitionRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class StyleDefinitionRecord : BaseWordRecord
{
  private const int DEF_START_ID = 0;
  private const int DEF_MASK_ID = 4095 /*0x0FFF*/;
  private const int DEF_BIT_SCRATCH = 12;
  private const int DEF_BIT_INVALID_HEIGHT = 13;
  private const int DEF_BIT_HAS_UPE = 14;
  private const int DEF_BIT_MASS_COPY = 15;
  private const int DEF_MASK_TYPE_CODE = 15;
  private const int DEF_START_TYPE_CODE = 0;
  private const int DEF_MASK_BASE_STYLE = 65520;
  private const int DEF_START_BASE_STYLE = 4;
  private const int DEF_MASK_UPX_NUMBER = 15;
  private const int DEF_START_UPX_NUMBER = 0;
  private const int DEF_MASK_NEXT_STYLE = 65520;
  private const int DEF_START_NEXT_STYLE = 4;
  private const int DEF_BIT_AUTO_REDEFINE = 0;
  private const int DEF_BIT_HIDDEN = 1;
  private StyleDefinitionBase m_basePart = new StyleDefinitionBase();
  private string m_strStyleName;
  private string m_aliasesStyleName;
  private UniversalPropertyException[] m_arrUpx;
  private CharacterPropertyException m_chpx;
  private ParagraphPropertyException m_papx;
  private byte[] m_data;
  private byte[] m_data1;
  private byte[] m_data2;
  private byte[] m_data3;
  private byte[] m_tapx;
  private StyleSheetInfoRecord m_shInfo;

  internal byte[] Tapx
  {
    get => this.m_tapx;
    set => this.m_tapx = value;
  }

  internal ushort StyleId
  {
    get => this.m_basePart.StyleId;
    set => this.m_basePart.StyleId = value;
  }

  internal bool IsScratch
  {
    get => this.m_basePart.IsScratch;
    set => this.m_basePart.IsScratch = value;
  }

  internal bool IsInvalidHeight
  {
    get => this.m_basePart.IsInvalidHeight;
    set => this.m_basePart.IsInvalidHeight = value;
  }

  internal bool HasUpe
  {
    get => this.m_basePart.HasUpe;
    set => this.m_basePart.HasUpe = value;
  }

  internal bool IsMassCopy
  {
    get => this.m_basePart.IsMassCopy;
    set => this.m_basePart.IsMassCopy = value;
  }

  internal WordStyleType TypeCode
  {
    get => (WordStyleType) this.m_basePart.TypeCode;
    set
    {
      if (value == WordStyleType.ParagraphStyle)
        this.m_papx = new ParagraphPropertyException();
      else if (this.m_basePart.TypeCode == (ushort) 1)
        this.m_papx = (ParagraphPropertyException) null;
      this.m_basePart.TypeCode = (ushort) value;
    }
  }

  internal ushort BaseStyle
  {
    get => this.m_basePart.BaseStyle;
    set => this.m_basePart.BaseStyle = value;
  }

  internal ushort UPEOffset
  {
    get => this.m_basePart.UPEOffset;
    set => this.m_basePart.UPEOffset = value;
  }

  internal ushort UpxNumber
  {
    get => this.m_basePart.UpxNumber;
    set => this.m_basePart.UpxNumber = value;
  }

  internal ushort NextStyleId
  {
    get => this.m_basePart.NextStyleId;
    set => this.m_basePart.NextStyleId = value;
  }

  internal bool IsAutoRedefine
  {
    get => this.m_basePart.IsAutoRedefine;
    set => this.m_basePart.IsAutoRedefine = value;
  }

  internal bool IsHidden
  {
    get => this.m_basePart.IsHidden;
    set => this.m_basePart.IsHidden = value;
  }

  internal string StyleName
  {
    get => this.m_strStyleName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string can not be empty");
        default:
          this.m_strStyleName = value;
          break;
      }
    }
  }

  internal string AliasesStyleName
  {
    get => this.m_aliasesStyleName;
    set => this.m_aliasesStyleName = value;
  }

  internal UniversalPropertyException[] PropertyExceptions => this.m_arrUpx;

  internal CharacterPropertyException CharacterProperty
  {
    get => this.m_chpx;
    set => this.m_chpx = value;
  }

  internal ParagraphPropertyException ParagraphProperty
  {
    get => this.m_papx;
    set => this.m_papx = value;
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_basePart;

  internal override int Length
  {
    get
    {
      int num = Math.Max((int) this.m_shInfo.STDBaseLength, this.m_basePart.Length);
      if (num % 2 != 0)
        ++num;
      if (this.m_strStyleName != null)
        num += Encoding.Unicode.GetByteCount(this.m_strStyleName);
      int length1 = num + 4;
      if (this.m_tapx != null && this.UpxNumber == (ushort) 3)
      {
        length1 += this.m_tapx.Length;
      }
      else
      {
        if (this.m_arrUpx != null && this.m_arrUpx.Length > 0)
        {
          int index = 0;
          for (int length2 = this.m_arrUpx.Length; index < length2; ++index)
            length1 += this.m_arrUpx[index].Length;
        }
        if (this.m_arrUpx == null)
          length1 += this.UpxLength;
      }
      return length1;
    }
  }

  internal byte[] DBG_data => this.m_data;

  internal byte[] DBG_data1 => this.m_data1;

  internal byte[] DBG_data2 => this.m_data2;

  internal byte[] DBG_data3 => this.m_data3;

  internal ushort LinkStyleId
  {
    get => this.m_basePart.LinkStyleId;
    set => this.m_basePart.LinkStyleId = value;
  }

  internal bool IsQFormat
  {
    get => this.m_basePart.IsQFormat;
    set => this.m_basePart.IsQFormat = value;
  }

  internal bool UnhideWhenUsed
  {
    get => this.m_basePart.UnhideWhenUsed;
    set => this.m_basePart.UnhideWhenUsed = value;
  }

  internal bool IsSemiHidden
  {
    get => this.m_basePart.IsSemiHidden;
    set => this.m_basePart.IsSemiHidden = value;
  }

  internal override void Close()
  {
    base.Close();
    this.m_basePart = (StyleDefinitionBase) null;
    if (this.m_arrUpx != null)
      this.m_arrUpx = (UniversalPropertyException[]) null;
    if (this.m_chpx != null)
      this.m_chpx = (CharacterPropertyException) null;
    if (this.m_papx != null)
      this.m_papx = (ParagraphPropertyException) null;
    this.m_data = (byte[]) null;
    this.m_data1 = (byte[]) null;
    this.m_data2 = (byte[]) null;
    this.m_data3 = (byte[]) null;
    this.m_tapx = (byte[]) null;
    if (this.m_shInfo == null)
      return;
    this.m_shInfo = (StyleSheetInfoRecord) null;
  }

  internal void Parse(Stream stream, int iCount, StyleSheetInfoRecord info)
  {
    this.Clear();
    if (iCount == 0)
      return;
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (info == null)
      throw new ArgumentNullException(nameof (info));
    byte[] buffer = new byte[iCount];
    stream.Read(buffer, 0, iCount);
    stream.Position -= (long) iCount;
    int stdBaseLength = (int) info.STDBaseLength;
    int iCount1 = Math.Max(stdBaseLength, 12);
    byte[] numArray1 = new byte[iCount1];
    stream.Read(numArray1, 0, stdBaseLength);
    this.Parse(numArray1, 0, iCount1);
    int count = iCount - stdBaseLength;
    if (stdBaseLength % 2 != 0)
    {
      ++stream.Position;
      --count;
    }
    byte[] numArray2 = new byte[count];
    stream.Read(numArray2, 0, count);
    int iEndPos;
    string terminatedString = BaseWordRecord.GetZeroTerminatedString(numArray2, 0, out iEndPos);
    if (terminatedString.Contains(","))
    {
      string[] strArray = terminatedString.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries);
      this.m_strStyleName = strArray[0];
      if (strArray.Length > 1)
        this.m_aliasesStyleName = string.Join(",", strArray, 1, strArray.Length - 1);
    }
    else
      this.m_strStyleName = terminatedString;
    this.ParseUpxPart(numArray2, iEndPos);
  }

  internal void Clear()
  {
    this.m_basePart.Clear();
    this.m_arrUpx = (UniversalPropertyException[]) null;
    this.m_strStyleName = (string) null;
  }

  internal override int Save(Stream stream)
  {
    long position = stream.Position;
    this.m_basePart.UpxNumber = this.m_tapx == null || this.TypeCode != WordStyleType.TableStyle ? (this.m_chpx != null ? (ushort) 1 : (ushort) 0) : (ushort) 3;
    if (this.m_strStyleName == "No List")
    {
      this.TypeCode = WordStyleType.ListStyle;
      this.m_basePart.UPEOffset = (ushort) 40;
    }
    else
      this.m_basePart.UPEOffset = (ushort) this.Length;
    if (this.m_basePart.UpxNumber > (ushort) 0)
      this.m_basePart.UpxNumber += this.m_papx != null ? (ushort) 1 : (ushort) 0;
    byte[] numArray = new byte[this.m_basePart.Length];
    this.Save(numArray, 0);
    stream.Write(numArray, 0, numArray.Length);
    if ((int) this.m_shInfo.STDBaseLength > numArray.Length)
    {
      int count = (int) this.m_shInfo.STDBaseLength - numArray.Length;
      stream.Write(new byte[count], 0, count);
    }
    string str = this.m_strStyleName;
    if (!string.IsNullOrEmpty(this.AliasesStyleName))
      str = $"{str},{this.AliasesStyleName}";
    byte[] zeroTerminatedArray = BaseWordRecord.ToZeroTerminatedArray(str);
    stream.Write(zeroTerminatedArray, 0, zeroTerminatedArray.Length);
    if (this.m_basePart.UpxNumber > (ushort) 0)
      this.SaveUpxPart(stream);
    return (int) (stream.Position - position);
  }

  internal StyleDefinitionRecord(string styleName, ushort styleId, StyleSheetInfoRecord info)
  {
    this.m_strStyleName = styleName;
    this.m_basePart.BaseStyle = (ushort) 4095 /*0x0FFF*/;
    this.m_basePart.HasUpe = false;
    this.m_basePart.NextStyleId = (ushort) 0;
    this.m_basePart.StyleId = styleId;
    this.TypeCode = WordStyleType.CharacterStyle;
    this.m_chpx = new CharacterPropertyException();
    this.m_shInfo = info;
  }

  internal StyleDefinitionRecord(
    byte[] arrData,
    int iOffset,
    int iCount,
    StyleSheetInfoRecord info)
    : base(arrData, iOffset, iCount)
  {
    this.m_shInfo = info;
  }

  internal StyleDefinitionRecord(Stream stream, int iCount, StyleSheetInfoRecord info)
  {
    this.m_shInfo = info;
    this.Parse(stream, iCount, info);
  }

  private int UpxLength
  {
    get
    {
      if (this.m_strStyleName == "No List")
        return 4;
      if (this.UpxNumber == (ushort) 3 && this.m_tapx != null)
        return this.m_tapx.Length;
      int upxLength = 2;
      if (this.m_papx != null)
      {
        upxLength += 2 + this.m_papx.Length;
        if (upxLength % 2 != 0)
          ++upxLength;
      }
      if (this.m_chpx != null)
        upxLength += this.m_chpx.PropertyModifiers.Length;
      if (upxLength % 2 != 0)
        ++upxLength;
      return upxLength;
    }
  }

  private void ParseUpxPart(byte[] arrVariable, int iStartPos)
  {
    this.m_arrUpx = new UniversalPropertyException[(int) this.m_basePart.UpxNumber];
    if (this.UpxNumber == (ushort) 3)
    {
      this.m_tapx = new byte[arrVariable.Length - iStartPos];
      this.TypeCode = WordStyleType.TableStyle;
      Buffer.BlockCopy((Array) arrVariable, iStartPos, (Array) this.m_tapx, 0, arrVariable.Length - iStartPos);
    }
    else
    {
      for (int index = 0; index < (int) this.m_basePart.UpxNumber; ++index)
      {
        iStartPos = this.MakeEven(iStartPos);
        ushort uint16 = BitConverter.ToUInt16(arrVariable, iStartPos);
        iStartPos += 2;
        if (uint16 != (ushort) 0)
        {
          this.m_arrUpx[index] = new UniversalPropertyException(arrVariable, iStartPos, (int) uint16);
          if (this.m_basePart.UpxNumber == (ushort) 1 || this.UpxNumber == (ushort) 2 && index == 1)
            this.m_chpx = new CharacterPropertyException(this.m_arrUpx[index]);
          else if (this.m_basePart.UpxNumber == (ushort) 2 && index == 0)
            this.m_papx = new ParagraphPropertyException(this.m_arrUpx[index]);
          iStartPos += (int) uint16;
          iStartPos = this.MakeEven(iStartPos);
        }
      }
    }
  }

  private int MakeEven(int iStartPos) => iStartPos % 2 == 0 ? iStartPos : ++iStartPos;

  private void SaveUpxPart(Stream stream)
  {
    long position = stream.Position;
    if (this.UpxNumber == (ushort) 3 && this.m_tapx != null)
      stream.Write(this.m_tapx, 0, this.m_tapx.Length);
    else if (this.m_strStyleName == "No List")
    {
      stream.Write(new byte[2]{ (byte) 2, (byte) 0 }, 0, 2);
      stream.Write(BitConverter.GetBytes(this.StyleId), 0, 2);
    }
    else
    {
      if (this.m_papx != null)
      {
        ushort length = (ushort) this.m_papx.Length;
        byte[] bytes = BitConverter.GetBytes(length);
        stream.Write(bytes, 0, bytes.Length);
        int num = this.m_papx.Save(stream);
        if ((int) length != num)
          throw new StreamWriteException("Incorrect writing UPX(pap) to file");
        if ((int) length % 2 != 0)
          stream.WriteByte((byte) 0);
      }
      ushort length1 = (ushort) this.m_chpx.PropertyModifiers.Length;
      byte[] bytes1 = BitConverter.GetBytes(length1);
      stream.Write(bytes1, 0, bytes1.Length);
      int num1 = this.m_chpx.PropertyModifiers.Save(stream);
      if ((int) length1 != num1)
        throw new StreamWriteException("Incorrect writing UPX(chp) to file");
      if ((int) length1 % 2 != 0)
        stream.WriteByte((byte) 0);
    }
    if (stream.Position - position != (long) this.UpxLength)
      throw new StreamWriteException("Incorrect writing UPX to file, invalid UPX Length");
  }
}

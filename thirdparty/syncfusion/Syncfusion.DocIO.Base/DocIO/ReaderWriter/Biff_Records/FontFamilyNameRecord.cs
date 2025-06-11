// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.FontFamilyNameRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class FontFamilyNameRecord : BaseWordRecord
{
  private const int DEF_MAX_LENGTH = 130;
  private const int DEF_PITCHREQUEST_MASK = 3;
  private const int DEF_ISTRUETYPE_MASK = 4;
  private const int DEF_ISTRUETYPE_OFFSET = 2;
  private const int DEF_FONTFAMILYID_MASK = 112 /*0x70*/;
  private const int DEF_FONTFAMILYID_OFFSET = 4;
  private byte m_bflags;
  private FFNBaseStructure m_ffnBase = new FFNBaseStructure();
  private string m_strFontName = string.Empty;
  private string m_strAltFontName = string.Empty;
  private byte[] m_dbgFontName;
  private Dictionary<string, Dictionary<string, DictionaryEntry>> m_embedFonts;

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_ffnBase;

  private byte TotalLengthM1 => this.m_ffnBase.TotalLengthM1;

  internal override int Length => (int) this.TotalLengthM1 + 1;

  internal byte PitchRequest
  {
    get => (byte) BaseWordRecord.GetBitsByMask((int) this.m_ffnBase.Options, 3, 0);
    set
    {
      if ((int) this.PitchRequest == (int) value)
        return;
      this.m_ffnBase.Options = (byte) BaseWordRecord.SetBitsByMask((int) this.m_ffnBase.Options, 3, (int) value);
    }
  }

  internal bool IsSubsetted
  {
    get => ((int) this.m_bflags & 4) >> 2 != 0;
    set => this.m_bflags = (byte) ((int) this.m_bflags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool TrueType
  {
    get => BaseWordRecord.GetBit(this.m_ffnBase.Options, 4);
    set
    {
      if (this.TrueType == value)
        return;
      this.m_ffnBase.Options = (byte) BaseWordRecord.SetBit((int) this.m_ffnBase.Options, 2, value);
    }
  }

  internal byte FontFamilyID
  {
    get => (byte) BaseWordRecord.GetBitsByMask((int) this.m_ffnBase.Options, 112 /*0x70*/, 4);
    set
    {
      if ((int) this.FontFamilyID == (int) value)
        return;
      this.m_ffnBase.Options = (byte) BaseWordRecord.SetBitsByMask((int) this.m_ffnBase.Options, 112 /*0x70*/, (int) value << 4);
    }
  }

  internal short Weight
  {
    get => this.m_ffnBase.Weight;
    set
    {
      if ((int) this.m_ffnBase.Weight == (int) value)
        return;
      this.m_ffnBase.Weight = value;
    }
  }

  internal byte[] SigUsb0
  {
    get
    {
      byte[] sigUsb0 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigUsb0[index] = this.m_ffnBase.m_FONTSIGNATURE[index];
      return sigUsb0;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[index] = value[index];
    }
  }

  internal byte[] SigUsb1
  {
    get
    {
      byte[] sigUsb1 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigUsb1[index] = this.m_ffnBase.m_FONTSIGNATURE[4 + index];
      return sigUsb1;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[4 + index] = value[index];
    }
  }

  internal byte[] SigUsb2
  {
    get
    {
      byte[] sigUsb2 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigUsb2[index] = this.m_ffnBase.m_FONTSIGNATURE[8 + index];
      return sigUsb2;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[8 + index] = value[index];
    }
  }

  internal byte[] SigUsb3
  {
    get
    {
      byte[] sigUsb3 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigUsb3[index] = this.m_ffnBase.m_FONTSIGNATURE[12 + index];
      return sigUsb3;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[12 + index] = value[index];
    }
  }

  internal byte[] SigCsb0
  {
    get
    {
      byte[] sigCsb0 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigCsb0[index] = this.m_ffnBase.m_FONTSIGNATURE[16 /*0x10*/ + index];
      return sigCsb0;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[16 /*0x10*/ + index] = value[index];
    }
  }

  internal byte[] SigCsb1
  {
    get
    {
      byte[] sigCsb1 = new byte[4];
      for (int index = 0; index < 4; ++index)
        sigCsb1[index] = this.m_ffnBase.m_FONTSIGNATURE[20 + index];
      return sigCsb1;
    }
    set
    {
      for (int index = 0; index < 4; ++index)
        this.m_ffnBase.m_FONTSIGNATURE[20 + index] = value[index];
    }
  }

  internal byte CharacterSetId
  {
    get => this.m_ffnBase.CharacterSetId;
    set
    {
      if ((int) this.m_ffnBase.CharacterSetId == (int) value)
        return;
      this.m_ffnBase.CharacterSetId = value;
    }
  }

  internal string FontName
  {
    get => this.m_strFontName;
    set
    {
      if (value == null)
        throw new ArgumentNullException("FontName is empty or nullable!");
      if (value.Length + this.m_strAltFontName.Length > 130)
        throw new ArgumentOutOfRangeException($"FontName can not be large ( 65 - {this.m_strAltFontName.ToString()} ) symbols");
      if (!(this.m_strFontName != value) && !(value == string.Empty))
        return;
      Encoding unicode = Encoding.Unicode;
      this.m_strFontName = value == string.Empty ? "\0" : value;
      int byteCount1 = unicode.GetByteCount(this.m_strFontName);
      int byteCount2 = unicode.GetByteCount(this.m_strAltFontName);
      this.m_ffnBase.TotalLengthM1 = (byte) (this.m_ffnBase.Length + byteCount1 + 1 + (byteCount2 > 0 ? byteCount2 + 2 : 0));
    }
  }

  internal string AlternativeFontName
  {
    get => this.m_strAltFontName;
    set => this.m_strAltFontName = value;
  }

  internal Dictionary<string, Dictionary<string, DictionaryEntry>> EmbedFonts
  {
    get
    {
      if (this.m_embedFonts == null)
        this.m_embedFonts = new Dictionary<string, Dictionary<string, DictionaryEntry>>();
      return this.m_embedFonts;
    }
    set => this.m_embedFonts = value;
  }

  internal FontFamilyNameRecord()
  {
  }

  internal int ParseBytes(byte[] arrData, int iOffset, int iCount)
  {
    this.Parse(arrData, iOffset, iCount);
    int length1 = this.m_ffnBase.Length;
    iOffset += length1;
    int length2 = this.Length - length1;
    if (length2 > 130)
      length2 = 130;
    this.m_dbgFontName = new byte[length2];
    Array.Copy((Array) arrData, iOffset, (Array) this.m_dbgFontName, 0, length2);
    this.m_strFontName = BaseWordRecord.ReadString(arrData, iOffset, (ushort) (length2 - 2));
    int length3 = this.Length;
    if (this.m_ffnBase.AlternateFontIndex != (byte) 0 && (int) this.m_ffnBase.AlternateFontIndex < this.m_strFontName.Length)
    {
      this.m_strAltFontName = this.m_strFontName.Substring((int) this.m_ffnBase.AlternateFontIndex);
      this.m_strFontName = this.m_strFontName.Substring(0, (int) this.m_ffnBase.AlternateFontIndex - 1);
      if (this.m_strFontName == string.Empty)
      {
        this.m_strFontName = this.m_strAltFontName;
        this.m_ffnBase.TotalLengthM1 += (byte) (this.m_strFontName.Length * 2);
      }
    }
    else
      this.m_strFontName = this.m_strFontName.Split(new char[1])[0];
    return length3;
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    int num = iOffset;
    if (this.AlternativeFontName != string.Empty)
    {
      this.m_ffnBase.AlternateFontIndex = (byte) (this.m_strFontName.Length + 1);
      FontFamilyNameRecord familyNameRecord = this;
      familyNameRecord.m_strFontName = familyNameRecord.m_strFontName + char.MinValue.ToString() + this.AlternativeFontName;
    }
    base.Save(arrData, iOffset);
    iOffset += this.m_ffnBase.Length;
    BaseWordRecord.WriteString(arrData, this.m_strFontName, ref iOffset);
    arrData[iOffset++] = (byte) 0;
    if (this.Length % 2 == 0)
      arrData[iOffset++] = (byte) 0;
    if (iOffset - num != this.Length)
      throw new Exception("Length of FFN record data is incorrect!");
    return iOffset;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_ffnBase != null)
    {
      this.m_ffnBase.Close();
      this.m_ffnBase = (FFNBaseStructure) null;
    }
    this.m_dbgFontName = (byte[]) null;
    if (this.m_embedFonts == null)
      return;
    foreach (KeyValuePair<string, Dictionary<string, DictionaryEntry>> embedFont in this.m_embedFonts)
    {
      foreach (KeyValuePair<string, DictionaryEntry> keyValuePair in embedFont.Value)
        ((Stream) keyValuePair.Value.Value).Dispose();
      embedFont.Value.Clear();
    }
    this.m_embedFonts.Clear();
    this.m_embedFonts = (Dictionary<string, Dictionary<string, DictionaryEntry>>) null;
  }
}

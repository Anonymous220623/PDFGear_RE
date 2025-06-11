// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListLevel
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListLevel : BaseWordRecord
{
  internal int m_startAt;
  internal ListPatternType m_nfc;
  internal ListNumberAlignment m_jc;
  internal bool m_bLegal;
  internal bool m_bNoRestart;
  internal bool m_bPrev;
  internal bool m_bPrevSpace;
  internal bool m_bWord6;
  internal bool m_unused;
  internal byte[] m_rgbxchNums;
  internal FollowCharacterType m_ixchFollow;
  internal int m_dxaSpace;
  internal int m_dxaIndent;
  internal int m_reserved;
  internal CharacterPropertyException m_chpx;
  internal ParagraphPropertyException m_papx;
  internal string m_str;

  internal ListLevel() => this.m_rgbxchNums = new byte[9];

  internal ListLevel(Stream stream) => this.Read(stream);

  internal void Write(Stream stream)
  {
    long position = stream.Position;
    BaseWordRecord.WriteInt32(stream, this.m_startAt);
    stream.WriteByte((byte) this.m_nfc);
    int num = (int) (ListNumberAlignment.Left | this.m_jc | (this.m_bLegal ? (ListNumberAlignment) 4 : ListNumberAlignment.Left) | (this.m_bNoRestart ? (ListNumberAlignment) 8 : ListNumberAlignment.Left) | (this.m_bPrev ? (ListNumberAlignment) 16 /*0x10*/ : ListNumberAlignment.Left) | (this.m_bPrevSpace ? (ListNumberAlignment) 32 /*0x20*/ : ListNumberAlignment.Left) | (this.m_bWord6 ? (ListNumberAlignment) 64 /*0x40*/ : ListNumberAlignment.Left) | (this.m_unused ? (ListNumberAlignment) 128 /*0x80*/ : ListNumberAlignment.Left));
    stream.WriteByte((byte) num);
    stream.Write(this.m_rgbxchNums, 0, this.m_rgbxchNums.Length);
    stream.WriteByte((byte) this.m_ixchFollow);
    BaseWordRecord.WriteInt32(stream, this.m_dxaSpace);
    BaseWordRecord.WriteInt32(stream, this.m_dxaIndent);
    stream.WriteByte((byte) this.m_chpx.PropertyModifiers.Length);
    stream.WriteByte((byte) this.m_papx.PropertyModifiers.Length);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_reserved);
    this.m_papx.PropertyModifiers.Save(stream);
    this.m_chpx.PropertyModifiers.Save(stream);
    BaseWordRecord.WriteString(stream, this.m_str);
  }

  private void Read(Stream stream)
  {
    long position = stream.Position;
    this.m_rgbxchNums = new byte[9];
    this.m_startAt = (int) BaseWordRecord.ReadUInt32(stream);
    this.m_nfc = (ListPatternType) stream.ReadByte();
    int num = stream.ReadByte();
    this.m_jc = (ListNumberAlignment) (byte) (num & 3);
    this.m_bLegal = (num & 4) != 0;
    this.m_bNoRestart = (num & 8) != 0;
    this.m_bPrev = (num & 16 /*0x10*/) != 0;
    this.m_bPrevSpace = (num & 32 /*0x20*/) != 0;
    this.m_bWord6 = (num & 64 /*0x40*/) != 0;
    this.m_unused = (num & 128 /*0x80*/) != 0;
    this.m_rgbxchNums = this.ReadBytes(stream, 9);
    this.m_ixchFollow = (FollowCharacterType) stream.ReadByte();
    this.m_dxaSpace = (int) BaseWordRecord.ReadUInt32(stream);
    this.m_dxaIndent = (int) BaseWordRecord.ReadUInt32(stream);
    int dataLen1 = stream.ReadByte();
    int dataLen2 = stream.ReadByte();
    this.m_reserved = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_chpx = new CharacterPropertyException();
    this.m_papx = new ParagraphPropertyException();
    this.ReadListSprms(dataLen2, stream, false);
    this.ReadListSprms(dataLen1, stream, true);
    this.m_str = BaseWordRecord.ReadString(stream);
  }

  private void ReadListSprms(int dataLen, Stream stream, bool isChpx)
  {
    int iOffset = 0;
    if (dataLen == 0)
      return;
    SinglePropertyModifierArray propertyModifierArray = isChpx ? this.m_chpx.PropertyModifiers : this.m_papx.PropertyModifiers;
    byte[] numArray = new byte[dataLen];
    stream.Read(numArray, 0, dataLen);
    while (dataLen - iOffset > 1)
    {
      SinglePropertyModifierRecord propertyModifierRecord = new SinglePropertyModifierRecord();
      try
      {
        iOffset = propertyModifierRecord.Parse(numArray, iOffset);
      }
      catch
      {
        iOffset = dataLen;
      }
      if (propertyModifierArray.IsValidCharacterPropertySprm(propertyModifierRecord))
        propertyModifierArray.CheckDuplicateSprms(propertyModifierRecord);
      propertyModifierArray.Add(propertyModifierRecord);
    }
  }
}

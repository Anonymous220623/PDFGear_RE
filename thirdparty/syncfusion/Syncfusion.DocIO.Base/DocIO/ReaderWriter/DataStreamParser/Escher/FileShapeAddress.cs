// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.FileShapeAddress
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

[CLSCompliant(false)]
internal class FileShapeAddress : BaseWordRecord
{
  internal const int DEF_FSPA_LENGTH = 26;
  private int m_spid;
  private int m_xaLeft;
  private int m_yaTop;
  private int m_xaRight;
  private int m_yaBottom;
  private int m_relHrzPos;
  private int m_relVrtPos;
  private int m_wrapStyle;
  private int m_wrapType;
  private int m_cTxbx;
  private byte m_bFlags;

  internal int Spid
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  internal int XaLeft
  {
    get => this.m_xaLeft;
    set => this.m_xaLeft = value;
  }

  internal int YaTop
  {
    get => this.m_yaTop;
    set => this.m_yaTop = value;
  }

  internal int XaRight
  {
    get => this.m_xaRight;
    set => this.m_xaRight = value;
  }

  internal int YaBottom
  {
    get => this.m_yaBottom;
    set => this.m_yaBottom = value;
  }

  internal bool IsHeaderShape
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal HorizontalOrigin RelHrzPos
  {
    get => (HorizontalOrigin) this.m_relHrzPos;
    set => this.m_relHrzPos = (int) value;
  }

  internal VerticalOrigin RelVrtPos
  {
    get => (VerticalOrigin) this.m_relVrtPos;
    set => this.m_relVrtPos = (int) value;
  }

  internal TextWrappingStyle TextWrappingStyle
  {
    get => (TextWrappingStyle) this.m_wrapStyle;
    set => this.m_wrapStyle = (int) value;
  }

  internal TextWrappingType TextWrappingType
  {
    get => (TextWrappingType) this.m_wrapType;
    set => this.m_wrapType = (int) value;
  }

  internal bool IsRcaSimple
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsBelowText
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsAnchorLock
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal int TxbxCount
  {
    get => this.m_cTxbx;
    set => this.m_cTxbx = value;
  }

  internal int Height
  {
    get => this.m_yaBottom - this.m_yaTop;
    set => this.m_yaBottom = this.m_yaTop + value;
  }

  internal int Width
  {
    get => this.m_xaRight - this.m_xaLeft;
    set => this.m_xaRight = this.m_xaLeft + value;
  }

  internal FileShapeAddress(Stream stream) => this.Read(stream);

  internal FileShapeAddress()
  {
  }

  internal void Read(Stream stream)
  {
    this.m_spid = BaseWordRecord.ReadInt32(stream);
    this.m_xaLeft = BaseWordRecord.ReadInt32(stream);
    this.m_yaTop = BaseWordRecord.ReadInt32(stream);
    this.m_xaRight = BaseWordRecord.ReadInt32(stream);
    this.m_yaBottom = BaseWordRecord.ReadInt32(stream);
    int num = (int) BaseWordRecord.ReadInt16(stream);
    this.IsHeaderShape = (num & 1) != 0;
    this.m_relHrzPos = (num & 6) >> 1;
    this.m_relVrtPos = (num & 24) >> 3;
    this.m_wrapStyle = (num & 480) >> 5;
    this.m_wrapType = (num & 7680) >> 9;
    this.IsRcaSimple = (num & 8192 /*0x2000*/) != 0;
    this.IsBelowText = (num & 16384 /*0x4000*/) != 0;
    this.IsAnchorLock = (num & 32768 /*0x8000*/) != 0;
    if (this.IsBelowText && this.TextWrappingStyle == TextWrappingStyle.InFrontOfText)
      this.TextWrappingStyle = TextWrappingStyle.Behind;
    this.m_cTxbx = BaseWordRecord.ReadInt32(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_spid);
    BaseWordRecord.WriteInt32(stream, this.m_xaLeft);
    BaseWordRecord.WriteInt32(stream, this.m_yaTop);
    BaseWordRecord.WriteInt32(stream, this.m_xaRight);
    BaseWordRecord.WriteInt32(stream, this.m_yaBottom);
    if (this.TextWrappingStyle == TextWrappingStyle.Behind && this.IsBelowText)
      this.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
    int num = 0 | (this.IsHeaderShape ? 1 : 0) | this.m_relHrzPos << 1 | this.m_relVrtPos << 3 | this.m_wrapStyle << 5 | this.m_wrapType << 9 | (this.IsRcaSimple ? 8192 /*0x2000*/ : 0) | (this.IsBelowText ? 16384 /*0x4000*/ : 0) | (this.IsAnchorLock ? 32768 /*0x8000*/ : 0);
    BaseWordRecord.WriteInt16(stream, (short) num);
    BaseWordRecord.WriteInt32(stream, this.m_cTxbx);
  }

  internal FileShapeAddress Clone()
  {
    return new FileShapeAddress()
    {
      m_cTxbx = this.m_cTxbx,
      IsAnchorLock = this.IsAnchorLock,
      IsBelowText = this.IsBelowText,
      IsHeaderShape = this.IsHeaderShape,
      IsRcaSimple = this.IsRcaSimple,
      m_relHrzPos = this.m_relHrzPos,
      m_relVrtPos = this.m_relVrtPos,
      m_spid = this.m_spid,
      m_wrapStyle = this.m_wrapStyle,
      m_wrapType = this.m_wrapType,
      m_xaLeft = this.m_xaLeft,
      m_xaRight = this.m_xaRight,
      m_yaBottom = this.m_yaBottom,
      m_yaTop = this.m_yaTop
    };
  }
}

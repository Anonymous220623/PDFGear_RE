// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtBSE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtBSE : BaseEscherRecord
{
  private _FBSE m_fbse;
  private _Blip m_blip;
  private byte m_bFlags;

  internal MsofbtBSE(WordDocument doc)
    : base(MSOFBT.msofbtBSE, 2, doc)
  {
    this.m_fbse = new _FBSE();
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_fbse.Read(stream);
    if (this.Header.Length <= 36)
      return;
    this.IsInlineBlip = true;
    this.m_blip = _MSOFBH.ReadHeaderWithRecord(stream, this.m_doc) as _Blip;
  }

  protected override void WriteRecordData(Stream stream)
  {
    int int32_1 = Convert.ToInt32(stream.Position);
    this.m_fbse.Write(stream);
    if (!this.IsInlineBlip || this.m_blip == null)
      return;
    this.m_fbse.m_size = this.m_blip.WriteMsofbhWithRecord(stream);
    int int32_2 = Convert.ToInt32(stream.Position);
    stream.Position = (long) int32_1;
    this.m_fbse.Write(stream);
    stream.Position = (long) int32_2;
  }

  internal void Read(Stream stream)
  {
    long position = stream.Position;
    stream.Position = (long) this.m_fbse.m_foDelay;
    this.m_blip = _MSOFBH.ReadHeaderWithRecord(stream, this.m_doc) as _Blip;
    stream.Position = position;
  }

  internal void Write(Stream stream)
  {
    this.m_fbse.m_foDelay = (int) stream.Position;
    this.m_fbse.m_size = 0;
    if (this.m_blip == null || this.IsPictureInShapeField)
      return;
    this.m_fbse.m_size = this.m_blip.WriteMsofbhWithRecord(stream);
  }

  internal void Initialize(ImageRecord imageRecord)
  {
    _Blip blip;
    if (imageRecord.IsMetafile)
    {
      blip = (_Blip) new MsofbtMetaFile(imageRecord, this.m_doc);
    }
    else
    {
      bool isBitmap = this.IsBitmap(imageRecord.ImageFormat);
      blip = (_Blip) new MsofbtImage(imageRecord, isBitmap, this.m_doc);
    }
    this.Header.Instance = (int) blip.Type;
    this.Fbse.m_btWin32 = (int) blip.Type;
    this.Fbse.m_btMacOS = (int) blip.Type;
    this.Fbse.m_rgbUid = blip.Uid.ToByteArray();
    this.Fbse.m_tag = (int) byte.MaxValue;
    this.Fbse.m_cRef = 1;
    this.Blip = blip;
  }

  internal _Blip Blip
  {
    get => this.m_blip;
    set => this.m_blip = value;
  }

  internal _FBSE Fbse => this.m_fbse;

  internal bool IsInlineBlip
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsPictureInShapeField
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtBSE msofbtBse = new MsofbtBSE(this.m_doc);
    msofbtBse.IsInlineBlip = this.IsInlineBlip;
    msofbtBse.m_fbse = this.m_fbse.Clone();
    if (this.m_blip != null)
      msofbtBse.m_blip = (_Blip) this.m_blip.Clone();
    msofbtBse.Header = this.Header.Clone();
    msofbtBse.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtBse;
  }

  private bool IsMetafile(ImageFormat imageFormat)
  {
    return imageFormat.Equals((object) ImageFormat.Emf) || imageFormat.Equals((object) ImageFormat.Wmf);
  }

  private bool IsBitmap(ImageFormat imageFormat)
  {
    return imageFormat.Equals((object) ImageFormat.Png) || imageFormat.Equals((object) ImageFormat.Bmp);
  }
}

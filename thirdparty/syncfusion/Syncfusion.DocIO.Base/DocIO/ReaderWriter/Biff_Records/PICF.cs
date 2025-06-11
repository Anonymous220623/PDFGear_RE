// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.PICF
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class PICF
{
  internal const int DEF_PICF_LENGTH = 68;
  internal const int DEF_SCALING_FACTOR = 1000;
  internal int lcb;
  internal short cbHeader;
  internal short mm;
  internal short xExt;
  internal short yExt;
  internal short hMf;
  internal short bm_rcWinMF;
  internal short bm_rcWinMF1;
  internal short bm_rcWinMF2;
  internal short bm_rcWinMF3;
  internal short bm_rcWinMF4;
  internal short bm_rcWinMF5;
  internal short bm_rcWinMF6;
  internal short dxaGoal;
  internal short dyaGoal;
  internal ushort mx;
  internal ushort my;
  internal short dyaCropLeft;
  internal short dyaCropTop;
  internal short dxaCropRight;
  internal short dyaCropBottom;
  internal short brcl;
  internal bool fFrameEmpty;
  internal bool fBitmap;
  internal bool fDrawHatch;
  internal bool fError;
  internal short bpp;
  internal BorderCode brcTop = new BorderCode();
  internal BorderCode brcLeft = new BorderCode();
  internal BorderCode brcBottom = new BorderCode();
  internal BorderCode brcRight = new BorderCode();
  internal short dxaOrigin;
  internal short dyaOrigin;
  internal short cProps;

  internal int ScaleHeight => (int) ((double) this.dyaGoal * this.ScaleY);

  internal int ScaleWidth => (int) ((double) this.dxaGoal * this.ScaleX);

  internal double ScaleX => (double) this.mx / 1000.0;

  internal double ScaleY => (double) this.my / 1000.0;

  internal BorderCode BorderTop => this.brcTop;

  internal BorderCode BorderLeft => this.brcLeft;

  internal BorderCode BorderRight => this.brcRight;

  internal BorderCode BorderBottom => this.brcBottom;

  internal PICF()
  {
    this.cbHeader = (short) 68;
    this.mm = (short) 100;
    this.mx = (ushort) 1000;
    this.my = (ushort) 1000;
  }

  internal PICF(BinaryReader reader) => this.Read(reader);

  internal void Read(BinaryReader reader)
  {
    int position = (int) reader.BaseStream.Position;
    this.lcb = reader.ReadInt32();
    this.cbHeader = reader.ReadInt16();
    this.mm = reader.ReadInt16();
    this.xExt = reader.ReadInt16();
    this.yExt = reader.ReadInt16();
    this.hMf = reader.ReadInt16();
    this.bm_rcWinMF = reader.ReadInt16();
    this.bm_rcWinMF1 = reader.ReadInt16();
    this.bm_rcWinMF2 = reader.ReadInt16();
    this.bm_rcWinMF3 = reader.ReadInt16();
    this.bm_rcWinMF4 = reader.ReadInt16();
    this.bm_rcWinMF5 = reader.ReadInt16();
    this.bm_rcWinMF6 = reader.ReadInt16();
    this.dxaGoal = reader.ReadInt16();
    this.dyaGoal = reader.ReadInt16();
    this.mx = (this.mx = reader.ReadUInt16()) == (ushort) 0 ? (ushort) 1000 : this.mx;
    this.my = (this.my = reader.ReadUInt16()) == (ushort) 0 ? (ushort) 1000 : this.my;
    this.dyaCropLeft = reader.ReadInt16();
    this.dyaCropTop = reader.ReadInt16();
    this.dxaCropRight = reader.ReadInt16();
    this.dyaCropBottom = reader.ReadInt16();
    int num = (int) reader.ReadInt16();
    this.brcl = (short) (num & 15);
    this.fFrameEmpty = (num & 16 /*0x10*/) != 0;
    this.fBitmap = (num & 32 /*0x20*/) != 0;
    this.fDrawHatch = (num & 64 /*0x40*/) != 0;
    this.fError = (num & 128 /*0x80*/) != 0;
    this.bpp = (short) ((num & 65280) >> 8);
    this.brcTop.Read(reader);
    this.brcLeft.Read(reader);
    this.brcBottom.Read(reader);
    this.brcRight.Read(reader);
    this.dxaOrigin = reader.ReadInt16();
    this.dyaOrigin = reader.ReadInt16();
    this.cProps = reader.ReadInt16();
    reader.BaseStream.Position = (long) (position + (int) this.cbHeader);
  }

  internal void Read(Stream stream) => this.Read(new BinaryReader(stream));

  internal void Write(Stream stream)
  {
    BinaryWriter binaryWriter = new BinaryWriter(stream);
    binaryWriter.Write(this.lcb);
    binaryWriter.Write(this.cbHeader);
    binaryWriter.Write(this.mm);
    binaryWriter.Write(this.xExt);
    binaryWriter.Write(this.yExt);
    binaryWriter.Write(this.hMf);
    binaryWriter.Write(this.bm_rcWinMF);
    binaryWriter.Write(this.bm_rcWinMF1);
    binaryWriter.Write(this.bm_rcWinMF2);
    binaryWriter.Write(this.bm_rcWinMF3);
    binaryWriter.Write(this.bm_rcWinMF4);
    binaryWriter.Write(this.bm_rcWinMF5);
    binaryWriter.Write(this.bm_rcWinMF6);
    binaryWriter.Write(this.dxaGoal);
    binaryWriter.Write(this.dyaGoal);
    binaryWriter.Write((short) this.mx);
    binaryWriter.Write((short) this.my);
    binaryWriter.Write(this.dyaCropLeft);
    binaryWriter.Write(this.dyaCropTop);
    binaryWriter.Write(this.dxaCropRight);
    binaryWriter.Write(this.dyaCropBottom);
    int num = (int) this.brcl | (this.fFrameEmpty ? 16 /*0x10*/ : 0) | (this.fBitmap ? 32 /*0x20*/ : 0) | (this.fDrawHatch ? 64 /*0x40*/ : 0) | (this.fError ? 128 /*0x80*/ : 0) | (int) this.bpp << 8;
    binaryWriter.Write((short) num);
    this.brcTop.Write(stream);
    this.brcLeft.Write(stream);
    this.brcBottom.Write(stream);
    this.brcRight.Write(stream);
    binaryWriter.Write(this.dxaOrigin);
    binaryWriter.Write(this.dyaOrigin);
    binaryWriter.Write(this.cProps);
  }

  internal PICF Clone()
  {
    PICF picf = this.MemberwiseClone() as PICF;
    picf.brcBottom = this.brcBottom.Clone();
    picf.brcLeft = this.brcLeft.Clone();
    picf.brcRight = this.brcRight.Clone();
    picf.brcTop = this.brcTop.Clone();
    return picf;
  }

  internal void SetBasePictureOptions(int height, int width, float heightScale, float widthScale)
  {
    this.dxaGoal = width <= (int) short.MaxValue ? (short) width : short.MaxValue;
    this.dyaGoal = height <= (int) short.MaxValue ? (short) height : short.MaxValue;
    this.mx = (ushort) Math.Round((double) widthScale * 10.0);
    this.my = (ushort) Math.Round((double) heightScale * 10.0);
  }
}

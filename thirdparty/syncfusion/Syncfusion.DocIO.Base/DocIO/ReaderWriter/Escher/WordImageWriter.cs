// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.WordImageWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class WordImageWriter
{
  private MemoryStream m_dataStream;
  private PICF m_picData = new PICF();
  private Metafile m_srcMetafile;

  internal WordImageWriter(MemoryStream dataStream) => this.m_dataStream = dataStream;

  internal MemoryStream DataStream => this.m_dataStream;

  internal PICF PictureData => this.m_picData;

  internal int WriteImage(WPicture pict, int height, int width)
  {
    if (!pict.PictureShape.PictureDescriptor.BorderLeft.IsDefault)
      this.m_picData = pict.PictureShape.PictureDescriptor.Clone();
    this.SetPictureSize((float) height, (float) width, pict);
    this.m_picData.cProps = (short) 0;
    this.m_picData.mm = (short) 100;
    this.m_picData.bm_rcWinMF = (short) 8;
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(pict.Document);
    msofbtSpContainer.CreateInlineImageContainer(pict);
    long position1 = this.m_dataStream.Position;
    this.m_dataStream.Position += 68L;
    msofbtSpContainer.WriteContainer((Stream) this.m_dataStream);
    int position2 = (int) this.m_dataStream.Position;
    this.m_picData.lcb = (int) ((long) position2 - position1);
    this.m_picData.cbHeader = (short) 68;
    this.m_dataStream.Position = position1;
    this.m_picData.Write((Stream) this.m_dataStream);
    this.m_dataStream.Position = (long) position2;
    return position2;
  }

  private void SetPictureSize(float height, float width, WPicture pict)
  {
    int num1 = (int) Math.Round((double) pict.Height * 20.0);
    int num2 = (int) Math.Round((double) pict.Width * 20.0);
    if ((double) width < (double) short.MaxValue)
    {
      this.m_picData.dxaGoal = (short) width;
      this.m_picData.mx = (ushort) Math.Round((double) pict.WidthScale * 10.0);
    }
    else if (num2 < (int) short.MaxValue)
    {
      this.m_picData.dxaGoal = (short) num2;
      this.m_picData.mx = (ushort) 1000;
    }
    else
    {
      this.m_picData.dxaGoal = short.MaxValue;
      this.m_picData.mx = (ushort) Math.Round((double) pict.WidthScale * 10.0);
    }
    if ((double) height < (double) short.MaxValue)
    {
      this.m_picData.dyaGoal = (short) height;
      this.m_picData.my = (ushort) Math.Round((double) pict.HeightScale * 10.0);
    }
    else if (num1 < (int) short.MaxValue)
    {
      this.m_picData.dyaGoal = (short) num1;
      this.m_picData.my = (ushort) 1000;
    }
    else
    {
      this.m_picData.dyaGoal = short.MaxValue;
      this.m_picData.my = (ushort) Math.Round((double) pict.HeightScale * 10.0);
    }
  }

  internal int WriteInlineShapeObject(InlineShapeObject shapeObj)
  {
    MsofbtSpContainer shapeContainer = shapeObj.ShapeContainer;
    if (shapeContainer != null)
    {
      if (shapeObj.PictureDescriptor.cbHeader == (short) 68)
      {
        long position1 = this.m_dataStream.Position;
        this.m_dataStream.Position += 68L;
        shapeContainer.WriteContainer((Stream) this.m_dataStream);
        int position2 = (int) this.m_dataStream.Position;
        shapeObj.PictureDescriptor.lcb = (int) ((long) position2 - position1);
        shapeObj.PictureDescriptor.cbHeader = (short) 68;
        this.m_dataStream.Position = position1;
        shapeObj.PictureDescriptor.Write((Stream) this.m_dataStream);
        this.m_dataStream.Position = (long) position2;
      }
      else
      {
        shapeObj.PictureDescriptor.Write((Stream) this.m_dataStream);
        shapeContainer.WriteContainer((Stream) this.m_dataStream);
      }
    }
    else if (shapeObj.PictureDescriptor != null && shapeObj.UnparsedData != null)
    {
      long position3 = this.m_dataStream.Position;
      this.m_dataStream.Position += 68L;
      new BinaryWriter((Stream) this.m_dataStream).Write(shapeObj.UnparsedData);
      int position4 = (int) this.m_dataStream.Position;
      shapeObj.PictureDescriptor.lcb = (int) ((long) position4 - position3);
      shapeObj.PictureDescriptor.cbHeader = (short) 68;
      this.m_dataStream.Position = position3;
      shapeObj.PictureDescriptor.Write((Stream) this.m_dataStream);
      this.m_dataStream.Position = (long) position4;
    }
    return (int) this.m_dataStream.Position;
  }

  internal int WriteInlineTxBxPicture(WTextBoxFormat txbxFormat)
  {
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(txbxFormat.Document);
    msofbtSpContainer.CreateInlineTxbxImageCont();
    PICF picf = new PICF();
    this.m_picData.SetBasePictureOptions((int) Math.Round((double) txbxFormat.Height * 20.0), (int) Math.Round((double) txbxFormat.Width * 20.0), 100f, 100f);
    long position1 = (long) (int) this.m_dataStream.Position;
    this.m_dataStream.Position += 68L;
    msofbtSpContainer.WriteContainer((Stream) this.m_dataStream);
    long position2 = (long) (int) this.m_dataStream.Position;
    this.m_picData.lcb = (int) (position2 - position1);
    this.m_picData.cbHeader = (short) 68;
    this.m_picData.mm = (short) 100;
    this.m_picData.bm_rcWinMF = (short) 2;
    this.m_dataStream.Position = position1;
    this.m_picData.Write((Stream) this.m_dataStream);
    this.m_dataStream.Position = position2;
    return (int) this.m_dataStream.Position;
  }

  internal void Close()
  {
    if (this.m_dataStream != null)
      this.m_dataStream = (MemoryStream) null;
    this.m_picData = (PICF) null;
    if (this.m_srcMetafile == null)
      return;
    this.m_srcMetafile.Dispose();
    this.m_srcMetafile = (Metafile) null;
  }

  private void SavePicf()
  {
    this.m_picData.lcb += 205;
    this.m_picData.cbHeader = (short) 68;
    this.m_picData.Write((Stream) this.m_dataStream);
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.PictureShape
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

[CLSCompliant(false)]
internal class PictureShape : ShapeBase
{
  private ImageRecord m_imageRecord;

  internal ImageRecord ImageRecord => this.m_imageRecord;

  internal PictureShapeProps PictureProps => this.ShapeProps as PictureShapeProps;

  internal PictureShape(ImageRecord imageRecord) => this.m_imageRecord = imageRecord;

  internal PictureShape()
  {
  }

  protected internal override void CreateShapeImpl()
  {
    this.m_shapeProps = (BaseProps) new PictureShapeProps();
  }
}

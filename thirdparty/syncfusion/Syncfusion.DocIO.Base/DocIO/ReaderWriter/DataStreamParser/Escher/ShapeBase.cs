// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

[CLSCompliant(false)]
internal class ShapeBase
{
  protected BaseProps m_shapeProps;

  internal BaseProps ShapeProps => this.m_shapeProps;

  internal ShapeBase() => this.CreateShapeImpl();

  internal virtual void Close() => this.m_shapeProps = (BaseProps) null;

  protected internal virtual void CreateShapeImpl()
  {
    throw new NotImplementedException("Not implemented");
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.TextBoxShape
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

[CLSCompliant(false)]
internal class TextBoxShape : ShapeBase
{
  internal TextBoxShape()
  {
  }

  internal TextBoxProps TextBoxProps => this.ShapeProps as TextBoxProps;

  protected internal override void CreateShapeImpl()
  {
    this.m_shapeProps = (BaseProps) new TextBoxProps();
  }
}

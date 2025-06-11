// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.BaseProps
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser;

[CLSCompliant(false)]
internal class BaseProps : FileShapeAddress
{
  protected ShapeHorizontalAlignment m_horAlignment;
  protected ShapeVerticalAlignment m_vertAlignment;

  internal BaseProps()
  {
  }

  internal ShapeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horAlignment;
    set => this.m_horAlignment = value;
  }

  internal ShapeVerticalAlignment VerticalAlignment
  {
    get => this.m_vertAlignment;
    set => this.m_vertAlignment = value;
  }
}

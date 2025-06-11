// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IShapes
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IShapes : IParentApplication, IEnumerable
{
  IOfficeChartShape AddChart();

  IShape AddCopy(IShape sourceShape);

  IShape AddCopy(
    IShape sourceShape,
    Dictionary<string, string> hashNewNames,
    List<int> arrFontIndexes);

  ITextBoxShapeEx AddTextBox();

  IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int leftColumn,
    int height,
    int width);

  int Count { get; }

  IShape this[int index] { get; }

  IShape this[string strShapeName] { get; }
}

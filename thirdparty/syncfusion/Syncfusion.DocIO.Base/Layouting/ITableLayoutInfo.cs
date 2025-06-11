// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ITableLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.Layouting;

internal interface ITableLayoutInfo : ILayoutSpacingsInfo
{
  float Width { get; set; }

  float Height { get; }

  float[] CellsWidth { get; set; }

  int HeadersRowCount { get; }

  bool[] IsDefaultCells { get; }

  bool IsSplittedTable { get; set; }

  double CellSpacings { get; }

  double CellPaddings { get; }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IWTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IWTable : ICompositeEntity, IEntity
{
  WRowCollection Rows { get; }

  RowFormat TableFormat { get; }

  WTableCell LastCell { get; }

  WTableRow FirstRow { get; }

  WTableRow LastRow { get; }

  WTableCell this[int row, int column] { get; }

  float Width { get; }

  string Title { get; set; }

  string Description { get; set; }

  bool ApplyStyleForHeaderRow { get; set; }

  bool ApplyStyleForLastRow { get; set; }

  bool ApplyStyleForFirstColumn { get; set; }

  bool ApplyStyleForLastColumn { get; set; }

  bool ApplyStyleForBandedRows { get; set; }

  bool ApplyStyleForBandedColumns { get; set; }

  WTableRow AddRow();

  WTableRow AddRow(bool isCopyFormat);

  WTableRow AddRow(bool isCopyFormat, bool autoPopulateCells);

  void ResetCells(int rowsNum, int columnsNum);

  void ResetCells(int rowsNum, int columnsNum, RowFormat format, float cellWidth);

  void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex);

  void ApplyHorizontalMerge(int rowIndex, int startCellIndex, int endCellIndex);

  float IndentFromLeft { get; set; }

  void RemoveAbsPosition();

  void ApplyStyle(BuiltinTableStyle builtinTableStyle);

  IWTableStyle GetStyle();
}

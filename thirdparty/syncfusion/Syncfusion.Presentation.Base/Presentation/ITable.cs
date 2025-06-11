// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ITable
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface ITable : ISlideItem
{
  bool HasFirstColumn { get; set; }

  bool HasHeaderRow { get; set; }

  bool HasBandedRows { get; set; }

  bool HasLastColumn { get; set; }

  bool HasTotalRow { get; set; }

  IRows Rows { get; }

  IColumns Columns { get; }

  BuiltInTableStyle BuiltInStyle { get; set; }

  bool HasBandedColumns { get; set; }

  ICell this[int rowIndex, int columnIndex] { get; }

  int ColumnsCount { get; }

  float GetActualHeight();

  void InsertColumn(int index);
}

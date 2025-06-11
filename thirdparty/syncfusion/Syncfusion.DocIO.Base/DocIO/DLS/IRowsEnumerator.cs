// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IRowsEnumerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IRowsEnumerator
{
  void Reset();

  bool NextRow();

  object GetCellValue(string columnName);

  string[] ColumnNames { get; }

  int RowsCount { get; }

  int CurrentRowIndex { get; }

  string TableName { get; }

  bool IsEnd { get; }

  bool IsLast { get; }
}

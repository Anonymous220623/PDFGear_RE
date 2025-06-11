// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPivotTableOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPivotTableOptions
{
  bool ShowAsteriskTotals { get; set; }

  string ColumnHeaderCaption { get; set; }

  string RowHeaderCaption { get; set; }

  bool ShowCustomSortList { get; set; }

  bool ShowFieldList { get; set; }

  bool IsDataEditable { get; set; }

  bool EnableFieldProperties { get; set; }

  uint Indent { get; set; }

  string ErrorString { get; set; }

  bool DisplayErrorString { get; set; }

  bool MergeLabels { get; set; }

  int PageFieldWrapCount { get; set; }

  PivotPageAreaFieldsOrder PageFieldsOrder { get; set; }

  bool DisplayNullString { get; set; }

  string NullString { get; set; }

  bool PreserveFormatting { get; set; }

  bool ShowTooltips { get; set; }

  bool DisplayFieldCaptions { get; set; }

  bool PrintTitles { get; set; }

  bool IsSaveData { get; set; }

  PivotTableRowLayout RowLayout { get; set; }

  bool ShowDrillIndicators { get; set; }

  void RepeatAllLabels(bool repeat);
}

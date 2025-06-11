// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Constants.Protection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;

internal sealed class Protection
{
  public const string SheetProtectionTag = "sheetProtection";
  public const string PasswordAttribute = "password";
  public const string ContentAttribute = "content";
  public const string ObjectsAttribute = "objects";
  public const string ScenariosAttribute = "scenarios";
  public const string FormatCellsAttribute = "formatCells";
  public const string FormatColumnsAttribute = "formatColumns";
  public const string FormatRowsAttribute = "formatRows";
  public const string InsertColumnsAttribute = "insertColumns";
  public const string InsertRowsAttribute = "insertRows";
  public const string InsertHyperlinksAttribute = "insertHyperlinks";
  public const string DeleteColumnsAttribute = "deleteColumns";
  public const string DeleteRowsAttribute = "deleteRows";
  public const string SelectLockedCells = "selectLockedCells";
  public const string SortAttribute = "sort";
  public const string AutoFilterAttribute = "autoFilter";
  public const string SelectUnlockedCells = "selectUnlockedCells";
  public const string PivotTablesAttribute = "pivotTables";
  public const string SheetAttribute = "sheet";
  public const string AlgorithmName = "algorithmName";
  public const string HashValue = "hashValue";
  public const string SpinCount = "spinCount";
  public const string SaltValue = "saltValue";
  public const string WorkbookAlgorithmName = "workbookAlgorithmName";
  public const string WorkbookHashValue = "workbookHashValue";
  public const string WorkbookSpinCount = "workbookSpinCount";
  public const string WorkbookSaltValue = "workbookSaltValue";
  internal const string UserName = "userName";
  public const string WorkbookProtectionTag = "workbookProtection";
  public const string LockStructureTag = "lockStructure";
  public const string LockWindowsTag = "lockWindows";
  public const string WorkbookPassword = "workbookPassword";
  public static readonly string[] ChartProtectionAttributes = new string[2]
  {
    "content",
    "objects"
  };
  public static readonly bool[] ChartDefaultValues = new bool[2];
  public static readonly string[] ProtectionAttributes = new string[16 /*0x10*/]
  {
    "sheet",
    "objects",
    "scenarios",
    "formatCells",
    "formatColumns",
    "formatRows",
    "insertColumns",
    "insertRows",
    "insertHyperlinks",
    "deleteColumns",
    "deleteRows",
    "selectLockedCells",
    "sort",
    "autoFilter",
    "pivotTables",
    "selectUnlockedCells"
  };
  public static readonly ExcelSheetProtection[] ProtectionFlags = new ExcelSheetProtection[16 /*0x10*/]
  {
    ExcelSheetProtection.Content,
    ExcelSheetProtection.Objects,
    ExcelSheetProtection.Scenarios,
    ExcelSheetProtection.FormattingCells,
    ExcelSheetProtection.FormattingColumns,
    ExcelSheetProtection.FormattingRows,
    ExcelSheetProtection.InsertingColumns,
    ExcelSheetProtection.InsertingRows,
    ExcelSheetProtection.InsertingHyperlinks,
    ExcelSheetProtection.DeletingColumns,
    ExcelSheetProtection.DeletingRows,
    ExcelSheetProtection.LockedCells,
    ExcelSheetProtection.Sorting,
    ExcelSheetProtection.Filtering,
    ExcelSheetProtection.UsingPivotTables,
    ExcelSheetProtection.UnLockedCells
  };
  public static readonly bool[] DefaultValues = new bool[16 /*0x10*/]
  {
    false,
    false,
    false,
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    false,
    true,
    true,
    false,
    false
  };

  private Protection()
  {
  }
}

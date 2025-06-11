// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.SRID
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Controls;

internal struct SRID
{
  private string _string;

  public string String => this._string;

  private SRID(string s) => this._string = s;

  public static SRID DataGrid_SelectAllCommandText
  {
    get => new SRID(nameof (DataGrid_SelectAllCommandText));
  }

  public static SRID DataGrid_SelectAllKey => new SRID(nameof (DataGrid_SelectAllKey));

  public static SRID DataGrid_SelectAllKeyDisplayString
  {
    get => new SRID(nameof (DataGrid_SelectAllKeyDisplayString));
  }

  public static SRID DataGrid_BeginEditCommandText
  {
    get => new SRID(nameof (DataGrid_BeginEditCommandText));
  }

  public static SRID DataGrid_CommitEditCommandText
  {
    get => new SRID(nameof (DataGrid_CommitEditCommandText));
  }

  public static SRID DataGrid_CancelEditCommandText
  {
    get => new SRID(nameof (DataGrid_CancelEditCommandText));
  }

  public static SRID DataGrid_DeleteCommandText => new SRID(nameof (DataGrid_DeleteCommandText));

  public static SRID DataGridCellItemAutomationPeer_NameCoreFormat
  {
    get => new SRID(nameof (DataGridCellItemAutomationPeer_NameCoreFormat));
  }

  public static SRID CalendarAutomationPeer_CalendarButtonLocalizedControlType
  {
    get => new SRID(nameof (CalendarAutomationPeer_CalendarButtonLocalizedControlType));
  }

  internal static SRID DateTimeEditAutomationPeer_DateTimeEditLocalizedControlType
  {
    get => new SRID(nameof (DateTimeEditAutomationPeer_DateTimeEditLocalizedControlType));
  }

  public static SRID CalendarAutomationPeer_DayButtonLocalizedControlType
  {
    get => new SRID(nameof (CalendarAutomationPeer_DayButtonLocalizedControlType));
  }

  public static SRID CalendarAutomationPeer_BlackoutDayHelpText
  {
    get => new SRID(nameof (CalendarAutomationPeer_BlackoutDayHelpText));
  }

  public static SRID Calendar_NextButtonName => new SRID(nameof (Calendar_NextButtonName));

  public static SRID Calendar_PreviousButtonName => new SRID(nameof (Calendar_PreviousButtonName));

  public static SRID DatePickerAutomationPeer_LocalizedControlType
  {
    get => new SRID(nameof (DatePickerAutomationPeer_LocalizedControlType));
  }

  public static SRID DatePickerTextBox_DefaultWatermarkText
  {
    get => new SRID(nameof (DatePickerTextBox_DefaultWatermarkText));
  }

  public static SRID DatePicker_DropDownButtonName
  {
    get => new SRID(nameof (DatePicker_DropDownButtonName));
  }

  public static SRID DataGrid_ColumnIndexOutOfRange
  {
    get => new SRID(nameof (DataGrid_ColumnIndexOutOfRange));
  }

  public static SRID DataGrid_ColumnDisplayIndexOutOfRange
  {
    get => new SRID(nameof (DataGrid_ColumnDisplayIndexOutOfRange));
  }

  public static SRID DataGrid_DisplayIndexOutOfRange
  {
    get => new SRID(nameof (DataGrid_DisplayIndexOutOfRange));
  }

  public static SRID DataGrid_InvalidColumnReuse => new SRID(nameof (DataGrid_InvalidColumnReuse));

  public static SRID DataGrid_DuplicateDisplayIndex
  {
    get => new SRID(nameof (DataGrid_DuplicateDisplayIndex));
  }

  public static SRID DataGrid_NewColumnInvalidDisplayIndex
  {
    get => new SRID(nameof (DataGrid_NewColumnInvalidDisplayIndex));
  }

  public static SRID DataGrid_NullColumn => new SRID(nameof (DataGrid_NullColumn));

  public static SRID DataGrid_ReadonlyCellsItemsSource
  {
    get => new SRID(nameof (DataGrid_ReadonlyCellsItemsSource));
  }

  public static SRID DataGrid_InvalidSortDescription
  {
    get => new SRID(nameof (DataGrid_InvalidSortDescription));
  }

  public static SRID DataGrid_ProbableInvalidSortDescription
  {
    get => new SRID(nameof (DataGrid_ProbableInvalidSortDescription));
  }

  public static SRID DataGridLength_InvalidType => new SRID(nameof (DataGridLength_InvalidType));

  public static SRID DataGridLength_Infinity => new SRID(nameof (DataGridLength_Infinity));

  public static SRID DataGrid_CannotSelectCell => new SRID(nameof (DataGrid_CannotSelectCell));

  public static SRID DataGridRow_CannotSelectRowWhenCells
  {
    get => new SRID(nameof (DataGridRow_CannotSelectRowWhenCells));
  }

  public static SRID DataGrid_AutomationInvokeFailed
  {
    get => new SRID(nameof (DataGrid_AutomationInvokeFailed));
  }

  public static SRID SelectedCellsCollection_InvalidItem
  {
    get => new SRID(nameof (SelectedCellsCollection_InvalidItem));
  }

  public static SRID SelectedCellsCollection_DuplicateItem
  {
    get => new SRID(nameof (SelectedCellsCollection_DuplicateItem));
  }

  public static SRID VirtualizedCellInfoCollection_IsReadOnly
  {
    get => new SRID(nameof (VirtualizedCellInfoCollection_IsReadOnly));
  }

  public static SRID VirtualizedCellInfoCollection_DoesNotSupportIndexChanges
  {
    get => new SRID(nameof (VirtualizedCellInfoCollection_DoesNotSupportIndexChanges));
  }

  public static SRID ClipboardCopyMode_Disabled => new SRID(nameof (ClipboardCopyMode_Disabled));

  public static SRID Calendar_OnDisplayModePropertyChanged_InvalidValue
  {
    get => new SRID(nameof (Calendar_OnDisplayModePropertyChanged_InvalidValue));
  }

  public static SRID Calendar_OnFirstDayOfWeekChanged_InvalidValue
  {
    get => new SRID(nameof (Calendar_OnFirstDayOfWeekChanged_InvalidValue));
  }

  public static SRID Calendar_OnSelectedDateChanged_InvalidValue
  {
    get => new SRID(nameof (Calendar_OnSelectedDateChanged_InvalidValue));
  }

  public static SRID Calendar_OnSelectedDateChanged_InvalidOperation
  {
    get => new SRID(nameof (Calendar_OnSelectedDateChanged_InvalidOperation));
  }

  public static SRID CalendarCollection_MultiThreadedCollectionChangeNotSupported
  {
    get => new SRID(nameof (CalendarCollection_MultiThreadedCollectionChangeNotSupported));
  }

  public static SRID Calendar_CheckSelectionMode_InvalidOperation
  {
    get => new SRID(nameof (Calendar_CheckSelectionMode_InvalidOperation));
  }

  public static SRID Calendar_OnSelectionModeChanged_InvalidValue
  {
    get => new SRID(nameof (Calendar_OnSelectionModeChanged_InvalidValue));
  }

  public static SRID Calendar_UnSelectableDates => new SRID(nameof (Calendar_UnSelectableDates));

  public static SRID DatePickerTextBox_TemplatePartIsOfIncorrectType
  {
    get => new SRID(nameof (DatePickerTextBox_TemplatePartIsOfIncorrectType));
  }

  public static SRID DatePicker_OnSelectedDateFormatChanged_InvalidValue
  {
    get => new SRID(nameof (DatePicker_OnSelectedDateFormatChanged_InvalidValue));
  }

  public static SRID DatePicker_WatermarkText => new SRID(nameof (DatePicker_WatermarkText));

  public static SRID CalendarAutomationPeer_MonthMode
  {
    get => new SRID(nameof (CalendarAutomationPeer_MonthMode));
  }

  public static SRID CalendarAutomationPeer_YearMode
  {
    get => new SRID(nameof (CalendarAutomationPeer_YearMode));
  }

  public static SRID CalendarAutomationPeer_DecadeMode
  {
    get => new SRID(nameof (CalendarAutomationPeer_DecadeMode));
  }

  internal static SRID CalendarEditAutomationPeer_DaysMode
  {
    get => new SRID(nameof (CalendarEditAutomationPeer_DaysMode));
  }

  internal static SRID CalendarEditAutomationPeer_MonthMode
  {
    get => new SRID(nameof (CalendarEditAutomationPeer_MonthMode));
  }

  internal static SRID CalendarEditAutomationPeer_YearMode
  {
    get => new SRID(nameof (CalendarEditAutomationPeer_YearMode));
  }

  internal static SRID CalendarEditAutomationPeer_YearRangeMode
  {
    get => new SRID(nameof (CalendarEditAutomationPeer_YearRangeMode));
  }

  internal static SRID CalendarEditAutomationPeer_WeekNumbersMode
  {
    get => new SRID(nameof (CalendarEditAutomationPeer_WeekNumbersMode));
  }
}

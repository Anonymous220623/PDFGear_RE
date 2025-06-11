// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableOptions : IPivotTableOptions
{
  private const uint DEF_MAX_INDENT = 127 /*0x7F*/;
  private PivotTableRowLayout m_rowLayout;
  private PivotTableImpl m_pivotTable;
  private ViewExtendedInfoRecord m_extInfo;
  private PivotViewDefinitionRecord m_DefinitionInfo;
  private bool m_bShowAsteriskTotals;
  private string m_columnHeaderCaption;
  private string m_rowHeaderCaption;
  private bool m_bIsCompactNewField;
  private bool m_bIsCompactData;
  private byte m_btCreatedVersion;
  private byte m_btUpdatedVersion;
  private byte m_btMiniRefreshVersion;
  private bool m_bShowCustomSortList;
  private string m_dataCaption;
  private bool m_bIsDataEditable;
  private bool m_bIsDefaultSortOrder;
  private bool m_bEnableFieldProperties;
  private bool m_bIsDefaultAutoSort;
  private bool m_bShowCalcMembers;
  private uint m_iIndent;
  private bool m_bOutline;
  private bool m_bOutlineData;
  private bool m_bMultiFieldFilter;
  private bool m_bShowGridDropZone;
  private PivotPageAreaFieldsOrder m_pageFieldsOrder;
  private bool m_bPreserveFormatting;
  private bool m_bShowTooltips;
  private bool m_bShowDrill = true;
  private bool m_bShowHeaders = true;
  private bool m_bPrintTitles;
  private string m_grandTotalCaption;

  public PivotTableOptions(
    PivotTableImpl pivotTable,
    ViewExtendedInfoRecord extInfo,
    PivotViewDefinitionRecord definitionInfo)
  {
    this.m_pivotTable = pivotTable;
    this.m_extInfo = extInfo;
    this.m_DefinitionInfo = definitionInfo;
    this.InitDefault();
  }

  internal void InitDefault()
  {
    this.RowLayout = PivotTableRowLayout.Compact;
    this.IsAutoFormat = false;
    this.Indent = 0U;
    this.Outline = true;
    this.OutlineData = true;
    this.IsMultiFieldFilter = false;
    this.DataCaption = "Values";
    this.MiniRefreshVersion = this.CreatedVersion = this.UpdatedVersion = this.m_pivotTable.GetPivotVersion();
    this.IsMultiFieldFilter = true;
    this.ShowGridDropZone = false;
    this.m_pageFieldsOrder = PivotPageAreaFieldsOrder.DownThenOver;
    this.ErrorString = "";
    this.DisplayErrorString = false;
    this.NullString = "";
    this.DisplayNullString = true;
    this.PreserveFormatting = true;
    this.ShowCustomSortList = true;
    this.ShowTooltips = true;
    this.DisplayFieldCaptions = true;
  }

  public bool IsAlignAutoFormat
  {
    get => this.m_DefinitionInfo.IsAlignAutoFormat;
    set => this.m_DefinitionInfo.IsAlignAutoFormat = value;
  }

  public bool IsBorderAutoFormat
  {
    get => this.m_DefinitionInfo.IsBorderAutoFormat;
    set => this.m_DefinitionInfo.IsBorderAutoFormat = value;
  }

  public bool IsNumberAutoFormat
  {
    get => this.m_DefinitionInfo.IsNumberAutoFormat;
    set => this.m_DefinitionInfo.IsNumberAutoFormat = value;
  }

  public bool IsPatternAutoFormat
  {
    get => this.m_DefinitionInfo.IsPatternAutoFormat;
    set => this.m_DefinitionInfo.IsPatternAutoFormat = value;
  }

  public bool IsWHAutoFormat
  {
    get => this.m_DefinitionInfo.IsWHAutoFormat;
    set => this.m_DefinitionInfo.IsWHAutoFormat = value;
  }

  public bool IsAutoFormat
  {
    get => this.m_DefinitionInfo.IsAutoFormat;
    set => this.m_DefinitionInfo.IsAutoFormat = value;
  }

  public bool IsFontAutoFormat
  {
    get => this.m_DefinitionInfo.IsFontAutoFormat;
    set => this.m_DefinitionInfo.IsFontAutoFormat = value;
  }

  public bool ShowAsteriskTotals
  {
    get => this.m_bShowAsteriskTotals;
    set => this.m_bShowAsteriskTotals = value;
  }

  public string ColumnHeaderCaption
  {
    get => this.m_columnHeaderCaption;
    set
    {
      if (!this.m_pivotTable.Workbook.Loading)
        this.m_pivotTable.SetChanged(false);
      this.m_columnHeaderCaption = value;
    }
  }

  public string RowHeaderCaption
  {
    get => this.m_rowHeaderCaption;
    set
    {
      if (!this.m_pivotTable.Workbook.Loading)
        this.m_pivotTable.SetChanged(false);
      this.m_rowHeaderCaption = value;
    }
  }

  public PivotTableRowLayout RowLayout
  {
    get => this.m_rowLayout;
    set
    {
      PivotTableImpl pivotTable = this.m_pivotTable;
      IPivotFields fields = (IPivotFields) pivotTable.Fields;
      if (!pivotTable.Workbook.Loading && fields != null)
      {
        foreach (PivotFieldImpl pivotFieldImpl in (IEnumerable) fields)
        {
          if (value != PivotTableRowLayout.Compact)
          {
            pivotFieldImpl.Compact = false;
            pivotFieldImpl.ShowOutline = value == PivotTableRowLayout.Outline;
          }
          else
          {
            pivotFieldImpl.ShowOutline = true;
            pivotFieldImpl.Compact = true;
          }
        }
      }
      this.m_rowLayout = value;
    }
  }

  public byte CreatedVersion
  {
    get => this.m_btCreatedVersion;
    set => this.m_btCreatedVersion = value;
  }

  public byte UpdatedVersion
  {
    get => this.m_btUpdatedVersion;
    set => this.m_btUpdatedVersion = value;
  }

  public byte MiniRefreshVersion
  {
    get => this.m_btMiniRefreshVersion;
    set => this.m_btMiniRefreshVersion = value;
  }

  public bool ShowCustomSortList
  {
    get => this.m_bShowCustomSortList;
    set => this.m_bShowCustomSortList = value;
  }

  public string DataCaption
  {
    get => this.m_dataCaption;
    set => this.m_dataCaption = value;
  }

  public ushort DataPosition
  {
    get => this.m_DefinitionInfo.DataPos;
    set
    {
      if (!this.m_pivotTable.Workbook.Loading && this.m_pivotTable.RowFields.Count <= (int) value)
        throw new ArgumentOutOfRangeException("DataPosition must less than are equal to row fields count in the pivot table");
      this.m_DefinitionInfo.DataPos = value;
    }
  }

  public bool ShowFieldList
  {
    get => this.m_pivotTable.Workbook.HidePivotFieldList;
    set => this.m_pivotTable.Workbook.HidePivotFieldList = value;
  }

  public bool IsDataEditable
  {
    get => this.m_bIsDataEditable;
    set => this.m_bIsDataEditable = value;
  }

  public bool EnableFieldProperties
  {
    get => this.m_bEnableFieldProperties;
    set => this.m_bEnableFieldProperties = value;
  }

  public bool IsDefaultAutoSort
  {
    get => this.m_bIsDefaultAutoSort;
    set => this.m_bIsDefaultAutoSort = value;
  }

  public bool ShowCalcMembers
  {
    get => this.m_bShowCalcMembers;
    set => this.m_bShowCalcMembers = value;
  }

  public uint Indent
  {
    get => this.m_iIndent;
    set => this.m_iIndent = value;
  }

  public bool Outline
  {
    get => this.m_bOutline;
    set => this.m_bOutline = value;
  }

  public bool OutlineData
  {
    get => this.m_bOutlineData;
    set => this.m_bOutlineData = value;
  }

  public bool IsMultiFieldFilter
  {
    get => this.m_bMultiFieldFilter;
    set => this.m_bMultiFieldFilter = value;
  }

  public bool ShowGridDropZone
  {
    get => this.m_bShowGridDropZone;
    set => this.m_bShowGridDropZone = value;
  }

  public string ErrorString
  {
    get => this.m_extInfo.ErrorString;
    set
    {
      this.DisplayErrorString = true;
      this.m_extInfo.ErrorString = value;
    }
  }

  public bool DisplayErrorString
  {
    get => this.m_extInfo.IsDisplayErrorString;
    set => this.m_extInfo.IsDisplayErrorString = value;
  }

  public bool DisplayNullString
  {
    get => this.m_extInfo.IsDisplayNullString;
    set => this.m_extInfo.IsDisplayNullString = value;
  }

  public bool MergeLabels
  {
    get => this.m_extInfo.IsMergeLabels;
    set => this.m_extInfo.IsMergeLabels = value;
  }

  public int PageFieldWrapCount
  {
    get => (int) this.m_extInfo.WrapPage;
    set
    {
      this.m_extInfo.WrapPage = value >= 0 && value <= 15 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (PageFieldWrapCount));
    }
  }

  public PivotPageAreaFieldsOrder PageFieldsOrder
  {
    get => this.m_pageFieldsOrder;
    set => this.m_pageFieldsOrder = value;
  }

  public string NullString
  {
    get => this.m_extInfo.NullString;
    set => this.m_extInfo.NullString = value;
  }

  public bool PreserveFormatting
  {
    get => this.m_bPreserveFormatting;
    set => this.m_bPreserveFormatting = value;
  }

  public bool ShowTooltips
  {
    get => this.m_bShowTooltips;
    set => this.m_bShowTooltips = value;
  }

  public bool ShowDrillIndicators
  {
    get => this.m_bShowDrill;
    set => this.m_bShowDrill = value;
  }

  public bool DisplayFieldCaptions
  {
    get => this.m_bShowHeaders;
    set
    {
      if (!this.m_pivotTable.Workbook.Loading)
        this.m_pivotTable.SetChanged(true);
      this.m_bShowHeaders = value;
    }
  }

  public bool PrintTitles
  {
    get => this.m_bPrintTitles;
    set => this.m_bPrintTitles = value;
  }

  public bool IsSaveData
  {
    get => this.m_pivotTable.Cache.IsSaveData;
    set => this.m_pivotTable.Cache.IsSaveData = value;
  }

  internal uint MaxIndent => (uint) sbyte.MaxValue;

  public string GrandTotalCaption
  {
    get => this.m_grandTotalCaption;
    set => this.m_grandTotalCaption = value;
  }

  public void RepeatAllLabels(bool repeat)
  {
    IPivotFields fields = (IPivotFields) this.m_pivotTable.Fields;
    for (int index = 0; index < fields.Count; ++index)
      fields[index].RepeatLabels = repeat;
  }
}

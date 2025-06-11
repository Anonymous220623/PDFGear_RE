// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartCategory
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartCategory : CommonObject, IOfficeChartCategory, IParentApplication
{
  private ChartSeriesCollection m_series;
  private bool m_isfiltered;
  private IRange m_categoryLabel;
  private IRange m_Value;
  private WorkbookImpl m_book;
  private ChartImpl m_chart;
  private ChartSeriesCollection series;
  private ChartCategoryCollection m_categoryColl;
  public bool Filter_customize;
  private string m_categoryName;

  public ChartCategory(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.InitializeCollections();
  }

  private void InitializeCollections()
  {
  }

  private void SetParents()
  {
    this.m_book = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ArgumentNullException("Can't find parent workbook."));
    this.m_chart = (ChartImpl) (this.FindParent(typeof (ChartImpl)) ?? throw new ArgumentNullException("Can't find parent chart."));
    this.m_categoryColl = (ChartCategoryCollection) (this.FindParent(typeof (ChartCategoryCollection)) ?? throw new ArgumentNullException("Can't find parent series collection."));
  }

  public bool IsFiltered
  {
    get => this.m_isfiltered;
    set
    {
      this.m_isfiltered = value;
      if (this.m_book.IsWorkbookOpening)
        return;
      this.Filter_customize = true;
    }
  }

  public IRange CategoryLabelIRange
  {
    get => this.m_categoryLabel;
    set => this.m_categoryLabel = value;
  }

  public IOfficeDataRange CategoryLabel
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.CategoryLabelIRange
      };
    }
    set
    {
      this.CategoryLabelIRange = this.m_chart.Workbook.Worksheets[0][value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public IRange ValuesIRange
  {
    get => this.m_Value;
    set => this.m_Value = value;
  }

  public IOfficeDataRange Values
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.ValuesIRange
      };
    }
    set
    {
      this.ValuesIRange = this.m_chart.Workbook.Worksheets[0][value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  public string Name
  {
    get => this.m_categoryName;
    set => this.m_categoryName = value;
  }
}

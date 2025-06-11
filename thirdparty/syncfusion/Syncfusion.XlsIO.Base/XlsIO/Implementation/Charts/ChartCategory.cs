// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartCategory
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartCategory : CommonObject, IChartCategory, IParentApplication
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
      if (this.m_isfiltered == value)
        return;
      this.m_isfiltered = value;
      if (this.m_book.Loading)
        return;
      this.Filter_customize = true;
    }
  }

  public IRange CategoryLabel
  {
    get => this.m_categoryLabel;
    set => this.m_categoryLabel = value;
  }

  public IRange Values
  {
    get => this.m_Value;
    set => this.m_Value = value;
  }

  public string Name
  {
    get => this.m_categoryName;
    set => this.m_categoryName = value;
  }

  internal object Clone() => this.MemberwiseClone();
}

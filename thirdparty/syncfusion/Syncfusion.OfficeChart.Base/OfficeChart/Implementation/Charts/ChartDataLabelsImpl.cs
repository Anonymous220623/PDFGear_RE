// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartDataLabelsImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Interfaces.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartDataLabelsImpl : 
  CommonObject,
  IOfficeChartDataLabels,
  ISerializable,
  IInternalOfficeChartTextArea,
  IOfficeChartTextArea,
  IInternalFont,
  IOfficeFont,
  IParentApplication,
  IOptimizedUpdate
{
  internal const string DEFAULT_FONTNAME = "Tahoma";
  internal const string DEFAULT_LANGUAGE = "en-US";
  internal const double DEFAULT_FONTSIZE = 10.0;
  private IRange m_valueFromIRange;
  private bool m_conditionCheck;
  private ChartSerieImpl m_serie;
  private ChartImpl m_chart;
  private ChartTextAreaImpl m_textArea;
  private ChartDataPointImpl m_dataPoint;
  private IOfficeChartLayout m_layout;
  private bool m_isDelete;
  private ChartParagraphType m_paraType;
  private bool m_bShowTextProperties = true;
  private bool m_bShowSizeProperties;
  private bool m_bShowBoldProperties;
  internal bool m_bHasValueOption;
  internal bool m_bHasSeriesOption;
  internal bool m_bHasCategoryOption;
  internal bool m_bHasPercentageOption;
  internal bool m_bHasLegendKeyOption;
  internal bool m_bHasBubbleSizeOption;
  private bool m_isSourceLinked = true;
  private string m_numberFormat;
  private bool m_bIsFormula;
  private bool m_bShowLeaderLines;
  private string[] m_stringCache;

  public ChartDataLabelsImpl(IApplication application, object parent, int index)
    : base(application, parent)
  {
    if (((parent as ChartDataPointImpl).Parent as ChartDataPointsCollection).Parent is ChartImpl)
      this.SetParents(true);
    else
      this.SetParents(false);
    this.m_textArea = (ChartTextAreaImpl) new ChartWrappedTextAreaImpl(this.Application, (object) this);
    this.m_textArea.ObjectLink.DataPointNumber = (ushort) index;
    this.m_textArea.TextRecord.IsAutoText = true;
    this.m_textArea.ChartAI.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    this.m_paraType = ChartParagraphType.Default;
    ChartSerieDataFormatImpl innerDataFormat = this.m_dataPoint.InnerDataFormat;
  }

  private void SetParents(bool isChart)
  {
    object[] objArray = new object[2];
    object[] parents;
    if (isChart)
    {
      parents = this.FindParents(new Type[2]
      {
        typeof (ChartImpl),
        typeof (ChartDataPointImpl)
      });
      this.m_chart = parents[0] as ChartImpl;
      this.m_serie = (ChartSerieImpl) null;
    }
    else
    {
      parents = this.FindParents(new Type[2]
      {
        typeof (ChartSerieImpl),
        typeof (ChartDataPointImpl)
      });
      this.m_serie = parents[0] as ChartSerieImpl;
      this.m_chart = (ChartImpl) null;
    }
    if (isChart)
    {
      if (this.m_chart == null)
        throw new ArgumentNullException("parent", "Can't find parent chart.");
    }
    else if (this.m_serie == null)
      throw new ArgumentNullException("parent", "Can't find parent serie.");
    this.m_dataPoint = parents[1] as ChartDataPointImpl;
    if (this.m_dataPoint == null)
      throw new ArgumentNullException("parent", "Can't find data point.");
  }

  public bool IsValueFromCells
  {
    get => this.m_textArea.IsValueFromCells;
    set
    {
      this.m_textArea.IsValueFromCells = value;
      if (this.ParentBook.IsWorkbookOpening || !(this.Parent is ChartDataPointImpl) || this.m_conditionCheck)
        return;
      ChartDataPointsCollection parent = (this.Parent as ChartDataPointImpl).Parent as ChartDataPointsCollection;
      if (parent.m_hashDataPoints == null)
        return;
      for (int key = 0; key < parent.m_hashDataPoints.Count; ++key)
      {
        (parent.m_hashDataPoints[key].DataLabels as ChartDataLabelsImpl).m_conditionCheck = true;
        parent.m_hashDataPoints[key].DataLabels.IsValueFromCells = value;
        this.UpdateDataLabelText(parent.m_hashDataPoints[key].DataLabels as ChartDataLabelsImpl, value);
      }
    }
  }

  public IOfficeDataRange ValueFromCellsRange
  {
    get => this.m_textArea.ValueFromCellsRange;
    set
    {
      this.ValueFromCellsIRange = this.ParentBook.Worksheets[this.m_serie.ParentChart.ActiveSheetIndex][value.FirstRow, value.FirstColumn, value.LastRow, value.LastColumn];
    }
  }

  internal IRange ValueFromCellsIRange
  {
    get => this.m_valueFromIRange;
    set
    {
      this.m_valueFromIRange = value;
      if (!this.ParentBook.IsWorkbookOpening && this.m_serie != null && value != null)
      {
        Dictionary<int, object> dictionary = new Dictionary<int, object>();
        for (int key = 0; key < value.Cells.Length; ++key)
          dictionary.Add(key, (object) value.Cells[key].DisplayText);
        this.m_serie.DataLabelCellsValues = dictionary;
      }
      this.m_textArea.ValueFromCellsRange = (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_serie.ParentChart)
      {
        Range = this.ValueFromCellsIRange
      };
    }
  }

  public bool IsSeriesName
  {
    get
    {
      return !this.m_bHasSeriesOption && !this.ParentBook.Saving && this.Application.DefaultVersion == OfficeVersion.Excel2013 || this.m_textArea.IsSeriesName;
    }
    set
    {
      this.m_textArea.IsSeriesName = value;
      this.m_bHasSeriesOption = true;
    }
  }

  public bool IsCategoryName
  {
    get
    {
      return !this.m_bHasCategoryOption && !this.ParentBook.Saving && this.Application.DefaultVersion == OfficeVersion.Excel2013 || this.m_textArea.IsCategoryName;
    }
    set
    {
      this.m_textArea.IsCategoryName = value;
      this.m_bHasCategoryOption = true;
      ChartSerieDataFormatImpl format = this.Format;
      if (format == null)
        return;
      format.AttachedLabel.ShowCategoryLabel = value;
    }
  }

  public bool IsValue
  {
    get
    {
      return !this.m_bHasValueOption && !this.ParentBook.Saving && this.Application.DefaultVersion == OfficeVersion.Excel2013 || this.m_textArea.IsValue;
    }
    set
    {
      ChartSerieDataFormatImpl format = this.Format;
      this.m_bHasValueOption = true;
      if (format != null)
        format.AttachedLabel.ShowActiveValue = value;
      this.m_textArea.IsValue = value;
    }
  }

  public bool IsPercentage
  {
    get
    {
      return !this.m_bHasPercentageOption && !this.ParentBook.Saving && this.Application.DefaultVersion == OfficeVersion.Excel2013 || this.m_textArea.IsPercentage;
    }
    set
    {
      this.m_textArea.IsPercentage = value;
      this.m_bHasPercentageOption = true;
      ChartSerieDataFormatImpl format = this.Format;
      if (format == null)
        return;
      format.AttachedLabel.ShowPieInPercents = true;
    }
  }

  internal bool IsSourceLinked
  {
    get
    {
      this.m_isSourceLinked = !this.TextArea.ChartAI.IsCustomNumberFormat;
      return this.m_isSourceLinked;
    }
    set
    {
      if (value != this.IsSourceLinked)
        this.TextArea.ChartAI.IsCustomNumberFormat = !value;
      this.m_isSourceLinked = value;
    }
  }

  public bool IsBubbleSize
  {
    get => this.m_textArea.IsBubbleSize;
    set
    {
      this.m_textArea.IsBubbleSize = value;
      this.m_bHasBubbleSizeOption = true;
      ChartSerieDataFormatImpl format = this.Format;
    }
  }

  public string Delimiter
  {
    get => this.m_textArea.Delimiter;
    set => this.m_textArea.Delimiter = value;
  }

  public bool IsLegendKey
  {
    get
    {
      return !this.m_bHasLegendKeyOption && !this.ParentBook.Saving && this.Application.DefaultVersion == OfficeVersion.Excel2013 || this.m_textArea.IsLegendKey;
    }
    set
    {
      this.m_textArea.IsLegendKey = value;
      this.m_bHasLegendKeyOption = true;
    }
  }

  public bool ShowLeaderLines
  {
    get
    {
      if (this.m_bShowLeaderLines)
        return true;
      if (this.m_serie != null && this.m_serie.ParentChart.IsChartPie)
        return this.m_serie.InnerChart.ChartFormat.ShowLeaderLines;
      return this.m_chart != null && this.m_chart.IsChartPie && this.m_chart.ChartFormat.ShowLeaderLines;
    }
    set
    {
      if (this.m_serie != null && this.m_serie.ParentChart.IsChartPie)
      {
        this.m_serie.InnerChart.ChartFormat.ShowLeaderLines = value;
        this.m_serie.SetLeaderLines(value);
      }
      else if (this.m_chart != null && this.m_chart.IsChartPie)
        this.m_chart.ChartFormat.ShowLeaderLines = value;
      this.m_bShowLeaderLines = value;
    }
  }

  public OfficeDataLabelPosition Position
  {
    get => this.m_textArea.Position;
    set => this.m_textArea.Position = value;
  }

  internal OfficeChartBackgroundMode BackgroundMode
  {
    get => this.m_textArea.BackgroundMode;
    set => this.m_textArea.BackgroundMode = value;
  }

  internal bool IsAutoMode
  {
    get => this.m_textArea.IsAutoMode;
    set => this.m_textArea.IsAutoMode = value;
  }

  public string Text
  {
    get => this.m_textArea.Text;
    set => this.m_textArea.Text = value;
  }

  public IOfficeChartRichTextString RichText => this.TextArea.RichText;

  public int TextRotationAngle
  {
    get => this.m_textArea.TextRotationAngle;
    set
    {
      this.m_textArea.TextRotationAngle = value;
      this.ShowTextProperties = true;
    }
  }

  public IOfficeChartFrameFormat FrameFormat => this.m_textArea.FrameFormat;

  public bool Bold
  {
    get => this.m_textArea.Bold;
    set
    {
      this.m_textArea.Bold = value;
      this.ShowTextProperties = true;
    }
  }

  public OfficeKnownColors Color
  {
    get => this.m_textArea.Color;
    set
    {
      this.m_textArea.Color = value;
      this.ShowTextProperties = true;
    }
  }

  public System.Drawing.Color RGBColor
  {
    get => this.m_textArea.RGBColor;
    set
    {
      this.m_textArea.RGBColor = value;
      this.ShowTextProperties = true;
    }
  }

  public bool Italic
  {
    get => this.m_textArea.Italic;
    set
    {
      this.m_textArea.Italic = value;
      this.ShowTextProperties = true;
    }
  }

  public bool MacOSOutlineFont
  {
    get => this.m_textArea.MacOSOutlineFont;
    set
    {
      this.m_textArea.MacOSOutlineFont = value;
      this.ShowTextProperties = true;
    }
  }

  public bool MacOSShadow
  {
    get => this.m_textArea.MacOSShadow;
    set => this.m_textArea.MacOSShadow = value;
  }

  public double Size
  {
    get => this.m_textArea.Size;
    set
    {
      this.m_textArea.Size = value;
      this.ShowTextProperties = true;
    }
  }

  public bool Strikethrough
  {
    get => this.m_textArea.Strikethrough;
    set
    {
      this.m_textArea.Strikethrough = value;
      this.ShowTextProperties = true;
    }
  }

  public bool Subscript
  {
    get => this.m_textArea.Subscript;
    set
    {
      this.m_textArea.Subscript = value;
      this.ShowTextProperties = true;
    }
  }

  public bool Superscript
  {
    get => this.m_textArea.Superscript;
    set
    {
      this.m_textArea.Superscript = value;
      this.ShowTextProperties = true;
    }
  }

  public OfficeUnderline Underline
  {
    get => this.m_textArea.Underline;
    set
    {
      this.m_textArea.Underline = value;
      this.ShowTextProperties = true;
    }
  }

  public string FontName
  {
    get => this.m_textArea.FontName;
    set
    {
      this.m_textArea.FontName = value;
      this.ShowTextProperties = true;
    }
  }

  public OfficeFontVerticalAlignment VerticalAlignment
  {
    get => this.m_textArea.VerticalAlignment;
    set
    {
      this.m_textArea.VerticalAlignment = value;
      this.ShowTextProperties = true;
    }
  }

  public System.Drawing.Font GenerateNativeFont() => this.m_textArea.GenerateNativeFont();

  public bool IsAutoColor => this.m_textArea.IsAutoColor;

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_textArea.ContainDataLabels)
      this.m_textArea.IsShowLabelPercent = this.m_serie != null && this.m_serie.IsPie && this.IsPercentage && this.IsCategoryName && !this.IsValue && !this.IsSeriesName;
    this.SetObjectLink();
    bool flag = !this.IsValue && this.IsCategoryName;
    if (flag)
      this.m_textArea.TextRecord.IsShowLabel = true;
    this.m_textArea.Serialize(records);
    if (!flag)
      return;
    this.m_textArea.TextRecord.IsShowLabel = false;
  }

  private void SetObjectLink()
  {
    ChartObjectLinkRecord objectLink = this.m_textArea.ObjectLink;
    objectLink.DataPointNumber = (ushort) this.m_dataPoint.Index;
    objectLink.LinkObject = ExcelObjectTextLink.DataLabel;
    objectLink.SeriesNumber = this.FindParent(typeof (ChartSerieImpl)) is ChartSerieImpl parent ? (ushort) parent.Index : throw new NotImplementedException("Can't find parent series");
  }

  public ChartTextAreaImpl TextArea
  {
    get => this.m_textArea;
    set
    {
      this.m_textArea = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public ChartSerieDataFormatImpl Format => this.m_dataPoint.InnerDataFormat;

  public IOfficeChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
        this.m_layout = (IOfficeChartLayout) new ChartLayoutImpl(this.Application, (object) this, this.Parent);
      return this.m_layout;
    }
    set => this.m_layout = value;
  }

  internal bool IsDelete
  {
    get => this.m_isDelete;
    set => this.m_isDelete = value;
  }

  public bool HasTextRotation => this.TextArea.HasTextRotation;

  public ChartParagraphType ParagraphType
  {
    get
    {
      if (this.m_paraType != ChartParagraphType.CustomDefault)
      {
        ChartParserCommon.CheckDefaultSettings(this.TextArea);
        this.m_paraType = this.TextArea.ParagraphType;
      }
      return this.m_paraType;
    }
    set => this.m_paraType = value;
  }

  public string NumberFormat
  {
    get
    {
      IOfficeChartDataPoint parent = this.Parent as IOfficeChartDataPoint;
      if (this.m_numberFormat == null && parent.IsDefault && this.TextArea != null)
        this.m_numberFormat = this.TextArea.NumberFormat;
      return this.m_numberFormat;
    }
    set
    {
      this.m_numberFormat = value;
      IOfficeChartDataPoint parent = this.Parent as IOfficeChartDataPoint;
      if (this.IsSourceLinked)
        this.TextArea.ChartAI.IsCustomNumberFormat = true;
      if (!parent.IsDefault || this.TextArea == null)
        return;
      this.TextArea.NumberFormat = value;
    }
  }

  public bool IsFormula
  {
    get => this.m_bIsFormula;
    set => this.m_bIsFormula = value;
  }

  internal bool ShowTextProperties
  {
    get => this.m_bShowTextProperties;
    set => this.m_bShowTextProperties = value;
  }

  internal void SetNumberFormat(string value)
  {
    this.m_numberFormat = value;
    if (!(this.Parent as IOfficeChartDataPoint).IsDefault || this.TextArea == null)
      return;
    this.TextArea.NumberFormat = value;
  }

  internal bool ShowSizeProperties
  {
    get => this.m_bShowSizeProperties;
    set => this.m_bShowSizeProperties = value;
  }

  internal bool ShowBoldProperties
  {
    get => this.m_bShowBoldProperties;
    set => this.m_bShowBoldProperties = value;
  }

  public Excel2007TextRotation TextRotation
  {
    get => this.m_textArea.TextRotation;
    set => this.m_textArea.TextRotation = value;
  }

  internal void UpdateDataLabelText(ChartDataLabelsImpl dataLabelsImpl, bool isValueFromCells)
  {
    if (dataLabelsImpl.Text == null)
      return;
    string[] strArray = dataLabelsImpl.Text.Split(',');
    string text = dataLabelsImpl.Text;
    bool flag = false;
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].Contains("[CELLRANGE]") || strArray[index].Contains("[SERIES NAME]") || strArray[index].Contains("[CATEGORY NAME]") || strArray[index].Contains("[VALUE]") || strArray[index].Contains("[PERCENTAGE]") || strArray[index].Contains("[X VALUE]") || strArray[index].Contains("[Y VALUE]"))
      {
        flag = true;
      }
      else
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    dataLabelsImpl.Text = (string) null;
  }

  public void UpdateSerieIndex()
  {
    if (this.m_serie == null)
      return;
    this.m_textArea.UpdateSerieIndex(this.m_serie.Index);
  }

  public object Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartDataLabelsImpl parent1 = (ChartDataLabelsImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    if (((parent as ChartDataPointImpl).Parent as ChartDataPointsCollection).Parent is ChartImpl)
      parent1.SetParents(true);
    else
      parent1.SetParents(false);
    parent1.m_paraType = this.m_paraType;
    if (this.m_layout != null)
      parent1.m_layout = this.m_layout;
    parent1.m_textArea = (ChartTextAreaImpl) this.m_textArea.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    return (object) parent1;
  }

  internal bool CheckSerieIsPie
  {
    get => this.m_serie != null && ChartImpl.GetIsChartPie(this.m_serie.SerieType);
  }

  private WorkbookImpl ParentBook
  {
    get => this.m_chart == null ? this.m_serie.ParentBook : this.m_chart.ParentWorkbook;
  }

  internal string[] StringCache
  {
    get => this.m_stringCache;
    set => this.m_stringCache = value;
  }

  public void BeginUpdate() => this.m_textArea.BeginUpdate();

  public void EndUpdate() => this.m_textArea.EndUpdate();

  internal ChartSerieImpl Serie
  {
    get => this.m_serie;
    set => this.m_serie = value;
  }

  public ChartColor ColorObject => this.m_textArea.ColorObject;

  public int Index => this.m_textArea.Index;

  public FontImpl Font => this.m_textArea.Font;
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ConditionalFormatWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ConditionalFormatWrapper : 
  CommonWrapper,
  IInternalConditionalFormat,
  IConditionalFormat,
  IParentApplication,
  IOptimizedUpdate
{
  private CondFormatCollectionWrapper m_formats;
  private int m_iIndex;
  private DataBarWrapper m_dataBar;
  private IconSetWrapper m_iconSet;
  private ColorScaleWrapper m_colorScale;
  private IRange m_range;
  private TopBottomWrapper m_topBottom;
  private AboveBelowAverageWrapper m_aboveBelowAverage;

  private ConditionalFormatWrapper()
  {
  }

  public ConditionalFormatWrapper(CondFormatCollectionWrapper formats, int iIndex)
  {
    this.m_formats = formats != null ? formats : throw new ArgumentNullException(nameof (formats));
    if (iIndex < 0 || iIndex >= formats.Count)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    this.m_iIndex = iIndex;
  }

  public ExcelCFType FormatType
  {
    get => this.GetCondition().FormatType;
    set
    {
      this.BeginUpdate();
      this.GetCondition().FormatType = value;
      this.EndUpdate();
    }
  }

  public CFTimePeriods TimePeriodType
  {
    get => this.GetCondition().TimePeriodType;
    set
    {
      this.BeginUpdate();
      this.GetCondition().TimePeriodType = value;
      this.EndUpdate();
    }
  }

  public ExcelComparisonOperator Operator
  {
    get => this.GetCondition().Operator;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Operator = value;
      this.EndUpdate();
    }
  }

  public bool IsBold
  {
    get => this.GetCondition().IsBold;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsBold = value;
      this.EndUpdate();
    }
  }

  public bool IsItalic
  {
    get => this.GetCondition().IsItalic;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsItalic = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors FontColor
  {
    get => this.GetCondition().FontColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsFontColorPresent = true;
      this.GetCondition().FontColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color FontColorRGB
  {
    get => this.GetCondition().FontColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().FontColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelUnderline Underline
  {
    get => this.GetCondition().Underline;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Underline = value;
      this.EndUpdate();
    }
  }

  public bool IsStrikeThrough
  {
    get => this.GetCondition().IsStrikeThrough;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsStrikeThrough = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors LeftBorderColor
  {
    get => this.GetCondition().LeftBorderColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().LeftBorderColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color LeftBorderColorRGB
  {
    get => this.GetCondition().LeftBorderColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().LeftBorderColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle LeftBorderStyle
  {
    get => this.GetCondition().LeftBorderStyle;
    set
    {
      this.BeginUpdate();
      this.GetCondition().LeftBorderStyle = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors RightBorderColor
  {
    get => this.GetCondition().RightBorderColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().RightBorderColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color RightBorderColorRGB
  {
    get => this.GetCondition().RightBorderColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().RightBorderColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle RightBorderStyle
  {
    get => this.GetCondition().RightBorderStyle;
    set
    {
      this.BeginUpdate();
      this.GetCondition().RightBorderStyle = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors TopBorderColor
  {
    get => this.GetCondition().TopBorderColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().TopBorderColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color TopBorderColorRGB
  {
    get => this.GetCondition().TopBorderColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().TopBorderColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle TopBorderStyle
  {
    get => this.GetCondition().TopBorderStyle;
    set
    {
      this.BeginUpdate();
      this.GetCondition().TopBorderStyle = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors BottomBorderColor
  {
    get => this.GetCondition().BottomBorderColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().BottomBorderColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color BottomBorderColorRGB
  {
    get => this.GetCondition().BottomBorderColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().BottomBorderColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle BottomBorderStyle
  {
    get => this.GetCondition().BottomBorderStyle;
    set
    {
      this.BeginUpdate();
      this.GetCondition().BottomBorderStyle = value;
      this.EndUpdate();
    }
  }

  public string FirstFormula
  {
    get
    {
      if (this.FormatType != ExcelCFType.SpecificText)
        this.GetCondition().Range = this.m_range;
      return this.GetCondition().FirstFormula;
    }
    set
    {
      this.BeginUpdate();
      this.GetCondition().FirstFormula = value;
      this.EndUpdate();
    }
  }

  public string FirstFormulaR1C1
  {
    get => this.GetCondition().FirstFormulaR1C1;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Range = this.m_range;
      this.GetCondition().FirstFormulaR1C1 = value;
      this.EndUpdate();
    }
  }

  public string SecondFormula
  {
    get => this.GetCondition().SecondFormula;
    set
    {
      this.BeginUpdate();
      this.GetCondition().SecondFormula = value;
      this.EndUpdate();
    }
  }

  public string SecondFormulaR1C1
  {
    get => this.GetCondition().SecondFormulaR1C1;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Range = this.m_range;
      this.GetCondition().SecondFormulaR1C1 = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors Color
  {
    get => this.GetCondition().Color;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Color = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get => this.GetCondition().ColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().ColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors BackColor
  {
    get => this.GetCondition().BackColor;
    set
    {
      this.BeginUpdate();
      this.GetCondition().BackColor = value;
      this.EndUpdate();
    }
  }

  public System.Drawing.Color BackColorRGB
  {
    get => this.GetCondition().BackColorRGB;
    set
    {
      this.BeginUpdate();
      this.GetCondition().BackColorRGB = value;
      this.EndUpdate();
    }
  }

  public ExcelPattern FillPattern
  {
    get => this.GetCondition().FillPattern;
    set
    {
      this.BeginUpdate();
      this.GetCondition().FillPattern = value;
      this.EndUpdate();
    }
  }

  public bool IsSuperScript
  {
    get => this.GetCondition().IsSuperScript;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsSuperScript = value;
      this.EndUpdate();
    }
  }

  public bool IsSubScript
  {
    get => this.GetCondition().IsSubScript;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsSubScript = value;
      this.EndUpdate();
    }
  }

  public bool IsFontFormatPresent
  {
    get => this.GetCondition().IsFontFormatPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsFontFormatPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsBorderFormatPresent
  {
    get => this.GetCondition().IsBorderFormatPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsBorderFormatPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsPatternFormatPresent
  {
    get => this.GetCondition().IsPatternFormatPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsPatternFormatPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsFontColorPresent
  {
    get => this.GetCondition().IsFontColorPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsFontColorPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsPatternColorPresent
  {
    get => this.GetCondition().IsPatternColorPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsPatternColorPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsBackgroundColorPresent
  {
    get => this.GetCondition().IsBackgroundColorPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsBackgroundColorPresent = value;
      this.EndUpdate();
    }
  }

  public bool HasNumberFormatPresent
  {
    get => this.GetCondition().HasNumberFormatPresent;
    set
    {
      this.BeginUpdate();
      this.GetCondition().HasNumberFormatPresent = value;
      this.EndUpdate();
    }
  }

  public bool IsLeftBorderModified
  {
    get => this.GetCondition().IsLeftBorderModified;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsLeftBorderModified = value;
      this.EndUpdate();
    }
  }

  public bool IsRightBorderModified
  {
    get => this.GetCondition().IsRightBorderModified;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsRightBorderModified = value;
      this.EndUpdate();
    }
  }

  public bool IsTopBorderModified
  {
    get => this.GetCondition().IsTopBorderModified;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsTopBorderModified = value;
      this.EndUpdate();
    }
  }

  public bool IsBottomBorderModified
  {
    get => this.GetCondition().IsBottomBorderModified;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsBottomBorderModified = value;
      this.EndUpdate();
    }
  }

  public ushort NumberFormatIndex
  {
    get => this.GetCondition().NumberFormatIndex;
    set
    {
      this.BeginUpdate();
      this.GetCondition().NumberFormatIndex = value;
      this.EndUpdate();
    }
  }

  public string NumberFormat
  {
    get => this.GetCondition().NumberFormat;
    set
    {
      this.BeginUpdate();
      this.GetCondition().NumberFormat = value;
      this.EndUpdate();
    }
  }

  public string Text
  {
    get => this.GetCondition().Text;
    set
    {
      this.BeginUpdate();
      this.GetCondition().Text = value;
      this.EndUpdate();
    }
  }

  public bool StopIfTrue
  {
    get => this.GetCondition().StopIfTrue;
    set => this.GetCondition().StopIfTrue = value;
  }

  public ConditionalFormatTemplate Template
  {
    get => this.GetCondition().Template;
    set => this.GetCondition().Template = value;
  }

  public IDataBar DataBar
  {
    get
    {
      if (this.FormatType == ExcelCFType.DataBar)
      {
        if (this.m_dataBar == null)
          this.m_dataBar = new DataBarWrapper(this.GetCondition().DataBar as DataBarImpl, this);
      }
      else
        this.m_dataBar = (DataBarWrapper) null;
      return (IDataBar) this.m_dataBar;
    }
  }

  public IIconSet IconSet
  {
    get
    {
      if (this.FormatType == ExcelCFType.IconSet)
      {
        if (this.m_iconSet == null)
          this.m_iconSet = new IconSetWrapper(this);
      }
      else
        this.m_iconSet = (IconSetWrapper) null;
      return (IIconSet) this.m_iconSet;
    }
  }

  public IColorScale ColorScale
  {
    get
    {
      if (this.FormatType == ExcelCFType.ColorScale)
      {
        if (this.m_colorScale == null)
          this.m_colorScale = new ColorScaleWrapper(this);
      }
      else
        this.m_colorScale = (ColorScaleWrapper) null;
      return (IColorScale) this.m_colorScale;
    }
  }

  internal IRange Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  public ITopBottom TopBottom
  {
    get
    {
      if (this.FormatType == ExcelCFType.TopBottom)
      {
        if (this.m_topBottom == null)
          this.m_topBottom = new TopBottomWrapper(this.GetCondition().TopBottom as TopBottomImpl, this);
      }
      else
        this.m_topBottom = (TopBottomWrapper) null;
      return (ITopBottom) this.m_topBottom;
    }
  }

  public IAboveBelowAverage AboveBelowAverage
  {
    get
    {
      if (this.FormatType == ExcelCFType.AboveBelowAverage)
      {
        if (this.m_aboveBelowAverage == null)
          this.m_aboveBelowAverage = new AboveBelowAverageWrapper(this.GetCondition().AboveBelowAverage as AboveBelowAverageImpl, this);
      }
      else
        this.m_aboveBelowAverage = (AboveBelowAverageWrapper) null;
      return (IAboveBelowAverage) this.m_aboveBelowAverage;
    }
  }

  public IApplication Application => this.m_formats.Application;

  public object Parent => (object) this.m_formats;

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
      this.m_formats.BeginUpdate();
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_formats.EndUpdate();
  }

  internal ConditionalFormatImpl GetCondition() => this.m_formats.GetCondition(this.m_iIndex);

  public ColorObject ColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject BackColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject TopBorderColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject BottomBorderColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject LeftBorderColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject RightBorderColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public ColorObject FontColorObject
  {
    get => throw new Exception("The method or operation is not implemented.");
  }

  public bool IsPatternStyleModified
  {
    get => this.GetCondition().IsPatternStyleModified;
    set
    {
      this.BeginUpdate();
      this.GetCondition().IsPatternStyleModified = value;
      this.EndUpdate();
    }
  }

  Ptg[] IInternalConditionalFormat.FirstFormulaPtgs
  {
    get => ((IInternalConditionalFormat) this.GetCondition()).FirstFormulaPtgs;
  }

  Ptg[] IInternalConditionalFormat.SecondFormulaPtgs
  {
    get => ((IInternalConditionalFormat) this.GetCondition()).SecondFormulaPtgs;
  }
}

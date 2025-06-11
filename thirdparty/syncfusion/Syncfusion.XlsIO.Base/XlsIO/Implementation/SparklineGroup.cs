// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SparklineGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class SparklineGroup : 
  List<ISparklines>,
  ISparklineGroup,
  IList<ISparklines>,
  ICollection<ISparklines>,
  IEnumerable<ISparklines>,
  IEnumerable
{
  private bool m_displayAxis;
  private bool m_displayHiddenRC;
  private bool m_plotRightToLeft;
  private bool m_showFirstPoint;
  private bool m_showLastPoint;
  private bool m_showLowPoint;
  private bool m_showHighPoint;
  private bool m_showNegativePoint;
  private bool m_showMarkers;
  private bool m_horizontalDateAxis;
  private Color m_axisColor = ColorExtension.Black;
  private Color m_firstPointColor = ColorExtension.Black;
  private Color m_highPointColor = ColorExtension.Black;
  private Color m_lastPointColor = ColorExtension.Black;
  private double m_lineWeight = 0.75;
  private Color m_lowPointColor = ColorExtension.Black;
  private Color m_markersColor = ColorExtension.Black;
  private Color m_negativePointColor = ColorExtension.Black;
  private Color m_sparklineColor = ColorExtension.Black;
  private ISparklineVerticalAxis m_verticalMaximum;
  private ISparklineVerticalAxis m_verticalMinimum;
  private SparklineType m_sparklineType = SparklineType.Line;
  private SparklineEmptyCells m_displayEmptyCellsAs = SparklineEmptyCells.Zero;
  private IRange m_horizontalDateAxisRange;
  private WorkbookImpl m_book;

  public bool DisplayAxis
  {
    get => this.m_displayAxis;
    set => this.m_displayAxis = value;
  }

  public bool DisplayHiddenRC
  {
    get => this.m_displayHiddenRC;
    set => this.m_displayHiddenRC = value;
  }

  public bool PlotRightToLeft
  {
    get => this.m_plotRightToLeft;
    set => this.m_plotRightToLeft = value;
  }

  public bool ShowFirstPoint
  {
    get => this.m_showFirstPoint;
    set => this.m_showFirstPoint = value;
  }

  public bool ShowLastPoint
  {
    get => this.m_showLastPoint;
    set => this.m_showLastPoint = value;
  }

  public bool ShowLowPoint
  {
    get => this.m_showLowPoint;
    set => this.m_showLowPoint = value;
  }

  public bool ShowHighPoint
  {
    get => this.m_showHighPoint;
    set => this.m_showHighPoint = value;
  }

  public bool ShowNegativePoint
  {
    get => this.m_showNegativePoint;
    set => this.m_showNegativePoint = value;
  }

  public bool ShowMarkers
  {
    get => this.m_showMarkers;
    set
    {
      if (this.m_sparklineType != SparklineType.Line && !this.m_book.Loading)
        throw new NotSupportedException("It is not supported for this Sparkline type.");
      this.m_showMarkers = value;
    }
  }

  public ISparklineVerticalAxis VerticalAxisMaximum
  {
    get
    {
      if (this.m_verticalMaximum == null)
        this.m_verticalMaximum = (ISparklineVerticalAxis) new SparklineVerticalAxis();
      return this.m_verticalMaximum;
    }
    set
    {
      if (value == null)
        return;
      this.m_verticalMaximum = value;
    }
  }

  public ISparklineVerticalAxis VerticalAxisMinimum
  {
    get
    {
      if (this.m_verticalMinimum == null)
        this.m_verticalMinimum = (ISparklineVerticalAxis) new SparklineVerticalAxis();
      return this.m_verticalMinimum;
    }
    set
    {
      if (value == null)
        return;
      this.m_verticalMinimum = value;
    }
  }

  public SparklineType SparklineType
  {
    get => this.m_sparklineType;
    set => this.m_sparklineType = value;
  }

  public bool HorizontalDateAxis
  {
    get => this.m_horizontalDateAxis;
    set => this.m_horizontalDateAxis = value;
  }

  public SparklineEmptyCells DisplayEmptyCellsAs
  {
    get => this.m_displayEmptyCellsAs;
    set => this.m_displayEmptyCellsAs = value;
  }

  public IRange HorizontalDateAxisRange
  {
    get => this.m_horizontalDateAxisRange;
    set
    {
      if (!this.HorizontalDateAxis || value.Rows.Length != 1 && value.Columns.Length != 1)
        throw new ArgumentOutOfRangeException("DataRange", "Date axis reference is not valid because the cells are not in the same column or row.");
      this.m_horizontalDateAxisRange = value;
    }
  }

  public Color AxisColor
  {
    get => this.m_axisColor;
    set
    {
      if (!this.m_book.Loading)
        this.DisplayAxis = true;
      this.m_axisColor = value;
    }
  }

  public Color FirstPointColor
  {
    get => this.m_firstPointColor;
    set
    {
      if (!this.m_book.Loading)
        this.ShowFirstPoint = true;
      this.m_firstPointColor = value;
    }
  }

  public Color HighPointColor
  {
    get => this.m_highPointColor;
    set
    {
      if (!this.m_book.Loading)
        this.ShowHighPoint = true;
      this.m_highPointColor = value;
    }
  }

  public Color LastPointColor
  {
    get => this.m_lastPointColor;
    set
    {
      if (!this.m_book.Loading)
        this.ShowLastPoint = true;
      this.m_lastPointColor = value;
    }
  }

  public double LineWeight
  {
    get => this.m_lineWeight;
    set
    {
      this.m_lineWeight = value >= 0.0 && value <= 1584.0 || this.SparklineType == SparklineType.Line ? value : throw new ArgumentOutOfRangeException(nameof (LineWeight), "The Values should be between 0 and 1584");
    }
  }

  public Color LowPointColor
  {
    get => this.m_lowPointColor;
    set
    {
      if (!this.m_book.Loading)
        this.ShowLowPoint = true;
      this.m_lowPointColor = value;
    }
  }

  public Color MarkersColor
  {
    get => this.m_markersColor;
    set
    {
      if (this.SparklineType != SparklineType.Line && !this.m_book.Loading)
        throw new NotSupportedException("It is not supported for the Current SparklineType");
      if (!this.m_book.Loading)
        this.ShowMarkers = true;
      this.m_markersColor = value;
    }
  }

  public Color NegativePointColor
  {
    get => this.m_negativePointColor;
    set
    {
      if (!this.m_book.Loading)
        this.ShowNegativePoint = true;
      this.m_negativePointColor = value;
    }
  }

  public Color SparklineColor
  {
    get => this.m_sparklineColor;
    set => this.m_sparklineColor = value;
  }

  public SparklineGroup(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
  }

  public ISparklines Add()
  {
    Sparklines sparklines = new Sparklines();
    sparklines.ParentGroup = this;
    this.Add((ISparklines) sparklines);
    return (ISparklines) sparklines;
  }
}

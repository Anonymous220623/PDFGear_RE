// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartManualLayoutImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartManualLayoutImpl : CommonObject, IChartManualLayout, IParentApplication
{
  protected ChartLayoutImpl m_layout;
  protected object m_Parent;
  private ChartAttachedLabelLayoutRecord m_atachedLabelLayout;
  private ChartPlotAreaLayoutRecord m_plotAreaLayout;
  protected LayoutTargets m_layoutTarget;
  protected LayoutModes m_leftMode;
  protected LayoutModes m_topMode;
  protected double m_left;
  protected double m_top;
  protected double m_dX;
  protected double m_dY;
  protected LayoutModes m_widthMode;
  protected LayoutModes m_heightMode;
  protected double m_width;
  protected double m_height;
  protected int m_xTL;
  protected int m_yTL;
  protected int m_xBR;
  protected int m_yBR;
  private byte m_flagOptions;

  public ChartManualLayoutImpl(IApplication application, object parent)
    : this(application, parent, false, false, true)
  {
  }

  public ChartManualLayoutImpl(IApplication application, object parent, bool bSetDefaults)
    : this(application, parent, false, false, bSetDefaults)
  {
  }

  public ChartManualLayoutImpl(
    IApplication application,
    object parent,
    bool bAutoSize,
    bool bIsInteriorGrey,
    bool bSetDefaults)
    : base(application, parent)
  {
    this.SetParents(parent);
  }

  public ChartManualLayoutImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.SetParents(parent);
  }

  private void SetParents(object parent)
  {
    this.m_layout = this.FindParent(typeof (ChartLayoutImpl)) as ChartLayoutImpl;
    this.m_Parent = parent;
    if (this.m_layout == null)
      throw new ArgumentNullException("Can't find parent chart");
  }

  public new object Parent => this.m_Parent;

  public ChartAttachedLabelLayoutRecord AttachedLabelLayout
  {
    get
    {
      if (this.m_atachedLabelLayout == null)
        this.m_atachedLabelLayout = new ChartAttachedLabelLayoutRecord();
      return this.m_atachedLabelLayout;
    }
    set => this.m_atachedLabelLayout = value;
  }

  public ChartPlotAreaLayoutRecord PlotAreaLayout
  {
    get
    {
      if (this.m_plotAreaLayout == null)
        this.m_plotAreaLayout = new ChartPlotAreaLayoutRecord();
      return this.m_plotAreaLayout;
    }
    set => this.m_plotAreaLayout = value;
  }

  public LayoutTargets LayoutTarget
  {
    get
    {
      if (this.m_plotAreaLayout != null)
        this.m_layoutTarget = this.PlotAreaLayout.LayoutTargetInner ? LayoutTargets.inner : (this.m_layoutTarget != LayoutTargets.auto ? LayoutTargets.outer : LayoutTargets.auto);
      return this.m_layoutTarget;
    }
    set
    {
      this.m_layoutTarget = value;
      this.PlotAreaLayout.LayoutTargetInner = this.m_layoutTarget == LayoutTargets.inner;
      this.m_flagOptions |= (byte) 16 /*0x10*/;
    }
  }

  public LayoutModes LeftMode
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_leftMode = this.AttachedLabelLayout.WXMode;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_leftMode = this.PlotAreaLayout.WXMode;
      }
      return this.m_leftMode;
    }
    set
    {
      this.m_leftMode = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.WXMode = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.WXMode = value;
      }
    }
  }

  public LayoutModes TopMode
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_topMode = this.AttachedLabelLayout.WYMode;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_topMode = this.PlotAreaLayout.WYMode;
      }
      return this.m_topMode;
    }
    set
    {
      this.m_topMode = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.WYMode = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.WYMode = value;
      }
    }
  }

  public double Left
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_left = this.AttachedLabelLayout.X;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_left = this.PlotAreaLayout.X;
      }
      return this.m_left;
    }
    set
    {
      this.m_left = value;
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.AttachedLabelLayout.X = value;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.PlotAreaLayout.X = value;
      }
      this.m_flagOptions |= (byte) 2;
    }
  }

  public double Top
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_top = this.AttachedLabelLayout.Y;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_top = this.PlotAreaLayout.Y;
      }
      return this.m_top;
    }
    set
    {
      this.m_top = value;
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.AttachedLabelLayout.Y = value;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.PlotAreaLayout.Y = value;
      }
      this.m_flagOptions |= (byte) 1;
    }
  }

  public double dX
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_dX = this.AttachedLabelLayout.Dx;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_dX = this.PlotAreaLayout.Dx;
      }
      return this.m_dX;
    }
    set
    {
      this.m_dX = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.Dx = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.Dx = value;
      }
    }
  }

  public double dY
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_dY = this.AttachedLabelLayout.Dy;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_dY = this.PlotAreaLayout.Dy;
      }
      return this.m_dY;
    }
    set
    {
      this.m_dY = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.Dy = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.Dy = value;
      }
    }
  }

  public LayoutModes WidthMode
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_widthMode = this.AttachedLabelLayout.WWidthMode;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_widthMode = this.PlotAreaLayout.WWidthMode;
      }
      return this.m_widthMode;
    }
    set
    {
      this.m_widthMode = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.WWidthMode = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.WWidthMode = value;
      }
    }
  }

  public LayoutModes HeightMode
  {
    get
    {
      if (this.Parent is ChartLayoutImpl)
      {
        if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
          this.m_heightMode = this.AttachedLabelLayout.WHeightMode;
        else if ((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl)
          this.m_heightMode = this.PlotAreaLayout.WHeightMode;
      }
      return this.m_heightMode;
    }
    set
    {
      this.m_heightMode = value;
      if (!(this.Parent is ChartLayoutImpl))
        return;
      if ((this.Parent as ChartLayoutImpl).Parent is ChartTextAreaImpl || (this.Parent as ChartLayoutImpl).Parent is ChartLegendImpl)
      {
        this.AttachedLabelLayout.WHeightMode = value;
      }
      else
      {
        if (!((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
          return;
        this.PlotAreaLayout.WHeightMode = value;
      }
    }
  }

  public double Width
  {
    get => this.dX;
    set
    {
      this.dX = value;
      this.m_flagOptions |= (byte) 8;
    }
  }

  public double Height
  {
    get => this.dY;
    set
    {
      this.dY = value;
      this.m_flagOptions |= (byte) 4;
    }
  }

  public int xTL
  {
    get => this.m_xTL;
    set
    {
      this.m_xTL = value;
      if (!(this.Parent is ChartLayoutImpl) || !((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
        return;
      this.PlotAreaLayout.xTL = value;
    }
  }

  public int yTL
  {
    get => this.m_yTL;
    set
    {
      this.m_yTL = value;
      if (!(this.Parent is ChartLayoutImpl) || !((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
        return;
      this.PlotAreaLayout.yTL = value;
    }
  }

  public int xBR
  {
    get => this.m_xBR;
    set
    {
      this.m_xBR = value;
      if (!(this.Parent is ChartLayoutImpl) || !((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
        return;
      this.PlotAreaLayout.xBR = value;
    }
  }

  public int yBR
  {
    get => this.m_yBR;
    set
    {
      this.m_yBR = value;
      if (!(this.Parent is ChartLayoutImpl) || !((this.Parent as ChartLayoutImpl).Parent is ChartPlotAreaImpl))
        return;
      this.PlotAreaLayout.yBR = value;
    }
  }

  internal byte FlagOptions
  {
    get => this.m_flagOptions;
    set => this.m_flagOptions = value;
  }

  public void SetDefaultValues()
  {
  }

  internal ChartAttachedLabelLayoutRecord GetAttachedLabelLayoutRecord()
  {
    return this.m_atachedLabelLayout;
  }
}

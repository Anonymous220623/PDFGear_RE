// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.InteriorWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class InteriorWrapper : CommonWrapper, IInterior, IOptimizedUpdate
{
  private ExtendedFormatImpl m_xFormat;
  private GradientWrapper m_gradient;

  public InteriorWrapper()
  {
  }

  public InteriorWrapper(ExtendedFormatImpl format)
  {
    this.m_xFormat = format != null ? format : throw new ArgumentNullException(nameof (format));
    if (format.FillPattern != ExcelPattern.Gradient)
      return;
    this.CreateGradientWrapper();
  }

  public ExcelKnownColors PatternColorIndex
  {
    get => this.m_xFormat.PatternColorIndex;
    set
    {
      this.BeginUpdate();
      if (this.m_gradient != null)
        this.FillPattern = ExcelPattern.Solid;
      this.m_xFormat.PatternColorIndex = value;
      this.EndUpdate();
    }
  }

  public Color PatternColor
  {
    get => this.m_xFormat.PatternColor;
    set
    {
      this.BeginUpdate();
      if (this.m_gradient != null)
        this.FillPattern = ExcelPattern.Solid;
      this.m_xFormat.PatternColor = value;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors ColorIndex
  {
    get => this.m_xFormat.ColorIndex;
    set
    {
      this.BeginUpdate();
      if (this.m_gradient != null)
        this.FillPattern = ExcelPattern.Solid;
      this.m_xFormat.ColorIndex = value;
      this.EndUpdate();
    }
  }

  public Color Color
  {
    get => this.m_xFormat.Color;
    set
    {
      this.BeginUpdate();
      if (this.m_gradient != null)
        this.FillPattern = ExcelPattern.Solid;
      this.m_xFormat.Color = value;
      this.EndUpdate();
    }
  }

  public IGradient Gradient => (IGradient) this.m_gradient;

  public ExcelPattern FillPattern
  {
    get => this.m_xFormat.FillPattern;
    set
    {
      if (this.m_xFormat.Workbook.Version == ExcelVersion.Excel97to2003 && value == ExcelPattern.Gradient)
        throw new ArgumentException("Excel97to2003 version does not support gradient fill type.");
      this.BeginUpdate();
      this.m_xFormat.FillPattern = value;
      if (value == ExcelPattern.Gradient)
      {
        this.CreateGradientWrapper();
      }
      else
      {
        this.m_gradient = (GradientWrapper) null;
        this.m_xFormat.Gradient = (IGradient) null;
      }
      this.EndUpdate();
    }
  }

  public event EventHandler AfterChangeEvent;

  private void WrappedGradientAfterChangeEvent(object sender, EventArgs e)
  {
    this.BeginUpdate();
    this.m_xFormat.Gradient = (IGradient) this.m_gradient.Wrapped;
    this.EndUpdate();
  }

  private void CreateGradientWrapper()
  {
    WorkbookImpl workbook = this.m_xFormat.Workbook;
    ShapeFillImpl gradient = (ShapeFillImpl) this.m_xFormat.Gradient;
    if (gradient == null)
    {
      gradient = new ShapeFillImpl(workbook.Application, (object) this.m_xFormat);
      gradient.FillType = ExcelFillType.Gradient;
    }
    this.m_gradient = new GradientWrapper(gradient);
    this.m_gradient.AfterChangeEvent += new EventHandler(this.WrappedGradientAfterChangeEvent);
    this.BeginUpdate();
    this.m_xFormat.Gradient = (IGradient) this.m_gradient.Wrapped;
    this.EndUpdate();
  }

  public ExtendedFormatImpl Wrapped => this.m_xFormat;

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
      this.m_xFormat = (ExtendedFormatImpl) this.Wrapped.Clone();
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_xFormat.Workbook.SetChanged();
    if (this.AfterChangeEvent == null)
      return;
    this.AfterChangeEvent((object) this, EventArgs.Empty);
  }

  internal void Dispose()
  {
    this.m_xFormat.ClearAll();
    this.m_gradient.Dispose();
  }
}

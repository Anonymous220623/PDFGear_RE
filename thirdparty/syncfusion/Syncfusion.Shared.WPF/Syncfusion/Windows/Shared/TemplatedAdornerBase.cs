// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TemplatedAdornerBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TemplatedAdornerBase : Adorner, IDisposable
{
  private TemplatedAdornerInternalControl m_innerControl;
  private double m_offsetX;
  private double m_offsetY;
  public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(nameof (OffsetX), typeof (double), typeof (TemplatedAdornerBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange, new PropertyChangedCallback(TemplatedAdornerBase.OnOffsetXChanged)));
  public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(nameof (OffsetY), typeof (double), typeof (TemplatedAdornerBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange, new PropertyChangedCallback(TemplatedAdornerBase.OnOffsetYChanged)));

  protected TemplatedAdornerInternalControl InnerControl => this.m_innerControl;

  public Size DesiredSizeInternal => this.m_innerControl.DesiredSize;

  public double OffsetX
  {
    get => this.m_offsetX;
    set => this.SetValue(TemplatedAdornerBase.OffsetXProperty, (object) value);
  }

  public double OffsetY
  {
    get => this.m_offsetY;
    set => this.SetValue(TemplatedAdornerBase.OffsetYProperty, (object) value);
  }

  protected override IEnumerator LogicalChildren
  {
    get
    {
      yield return (object) this.m_innerControl;
    }
  }

  static TemplatedAdornerBase()
  {
    FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(typeof (TemplatedAdornerBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Left));
    FrameworkElement.VerticalAlignmentProperty.OverrideMetadata(typeof (TemplatedAdornerBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Top));
  }

  public TemplatedAdornerBase(UIElement adornedElement)
    : base(adornedElement)
  {
    if (adornedElement == null)
      throw new ArgumentNullException(nameof (adornedElement));
    this.m_innerControl = new TemplatedAdornerInternalControl(this);
    this.AddLogicalChild((object) this.m_innerControl);
    this.AddVisualChild((Visual) this.m_innerControl);
  }

  public void Dispose()
  {
    if (this.m_innerControl == null)
      return;
    this.RemoveLogicalChild((object) this.m_innerControl);
    this.RemoveVisualChild((Visual) this.m_innerControl);
    this.m_innerControl = (TemplatedAdornerInternalControl) null;
  }

  public event PropertyChangedCallback OffsetXChanged;

  public event PropertyChangedCallback OffsetYChanged;

  public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
  {
    GeneralTransformGroup desiredTransform = new GeneralTransformGroup();
    if (transform != null)
      desiredTransform.Children.Add(transform);
    desiredTransform.Children.Add((GeneralTransform) new TranslateTransform(this.OffsetX, this.OffsetY));
    return (GeneralTransform) desiredTransform;
  }

  protected override Size MeasureOverride(Size constraint)
  {
    Size availableSize = constraint;
    if (double.IsInfinity(availableSize.Height) || double.IsInfinity(availableSize.Width))
      availableSize = this.AdornedElement.RenderSize;
    if (this.m_innerControl != null)
      this.m_innerControl.Measure(availableSize);
    return this.AdornedElement.RenderSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    if (this.m_innerControl == null)
      return base.ArrangeOverride(finalSize);
    this.m_innerControl.Arrange(new Rect(finalSize));
    return this.m_innerControl.RenderSize;
  }

  protected override Visual GetVisualChild(int index) => (Visual) this.m_innerControl;

  protected override int VisualChildrenCount => 1;

  private static void OnOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TemplatedAdornerBase) d).OnOffsetXChanged(e);
  }

  private static void OnOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TemplatedAdornerBase) d).OnOffsetYChanged(e);
  }

  private void OnOffsetXChanged(DependencyPropertyChangedEventArgs e)
  {
    this.m_offsetX = (double) e.NewValue;
    if (this.OffsetXChanged == null)
      return;
    this.OffsetXChanged((DependencyObject) this, e);
  }

  private void OnOffsetYChanged(DependencyPropertyChangedEventArgs e)
  {
    this.m_offsetY = (double) e.NewValue;
    if (this.OffsetYChanged == null)
      return;
    this.OffsetYChanged((DependencyObject) this, e);
  }
}

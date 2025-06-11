// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.LoadingBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public abstract class LoadingBase : ContentControl
{
  protected Storyboard Storyboard;
  public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(nameof (IsRunning), typeof (bool), typeof (LoadingBase), new PropertyMetadata(ValueBoxes.TrueBox, (PropertyChangedCallback) ((o, args) =>
  {
    LoadingBase loadingBase = (LoadingBase) o;
    if ((bool) args.NewValue)
      loadingBase.Storyboard?.Resume();
    else
      loadingBase.Storyboard?.Pause();
  })));
  public static readonly DependencyProperty DotCountProperty = DependencyProperty.Register(nameof (DotCount), typeof (int), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int5Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotIntervalProperty = DependencyProperty.Register(nameof (DotInterval), typeof (double), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotBorderBrushProperty = DependencyProperty.Register(nameof (DotBorderBrush), typeof (Brush), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotBorderThicknessProperty = DependencyProperty.Register(nameof (DotBorderThickness), typeof (double), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotDiameterProperty = DependencyProperty.Register(nameof (DotDiameter), typeof (double), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 6.0, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotSpeedProperty = DependencyProperty.Register(nameof (DotSpeed), typeof (double), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 4.0, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DotDelayTimeProperty = DependencyProperty.Register(nameof (DotDelayTime), typeof (double), typeof (LoadingBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) 80.0, FrameworkPropertyMetadataOptions.AffectsRender));
  protected readonly Canvas PrivateCanvas;

  public bool IsRunning
  {
    get => (bool) this.GetValue(LoadingBase.IsRunningProperty);
    set => this.SetValue(LoadingBase.IsRunningProperty, ValueBoxes.BooleanBox(value));
  }

  protected LoadingBase()
  {
    Canvas canvas = new Canvas();
    canvas.ClipToBounds = true;
    this.PrivateCanvas = canvas;
    // ISSUE: explicit constructor call
    base.\u002Ector();
    this.Content = (object) this.PrivateCanvas;
  }

  public int DotCount
  {
    get => (int) this.GetValue(LoadingBase.DotCountProperty);
    set => this.SetValue(LoadingBase.DotCountProperty, (object) value);
  }

  public double DotInterval
  {
    get => (double) this.GetValue(LoadingBase.DotIntervalProperty);
    set => this.SetValue(LoadingBase.DotIntervalProperty, (object) value);
  }

  public Brush DotBorderBrush
  {
    get => (Brush) this.GetValue(LoadingBase.DotBorderBrushProperty);
    set => this.SetValue(LoadingBase.DotBorderBrushProperty, (object) value);
  }

  public double DotBorderThickness
  {
    get => (double) this.GetValue(LoadingBase.DotBorderThicknessProperty);
    set => this.SetValue(LoadingBase.DotBorderThicknessProperty, (object) value);
  }

  public double DotDiameter
  {
    get => (double) this.GetValue(LoadingBase.DotDiameterProperty);
    set => this.SetValue(LoadingBase.DotDiameterProperty, (object) value);
  }

  public double DotSpeed
  {
    get => (double) this.GetValue(LoadingBase.DotSpeedProperty);
    set => this.SetValue(LoadingBase.DotSpeedProperty, (object) value);
  }

  public double DotDelayTime
  {
    get => (double) this.GetValue(LoadingBase.DotDelayTimeProperty);
    set => this.SetValue(LoadingBase.DotDelayTimeProperty, (object) value);
  }

  protected abstract void UpdateDots();

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    this.UpdateDots();
  }

  protected virtual Ellipse CreateEllipse(int index)
  {
    Ellipse ellipse = new Ellipse();
    ellipse.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding(LoadingBase.DotDiameterProperty.Name)
    {
      Source = (object) this
    });
    ellipse.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding(LoadingBase.DotDiameterProperty.Name)
    {
      Source = (object) this
    });
    ellipse.SetBinding(Shape.FillProperty, (BindingBase) new Binding(Control.ForegroundProperty.Name)
    {
      Source = (object) this
    });
    ellipse.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding(LoadingBase.DotBorderThicknessProperty.Name)
    {
      Source = (object) this
    });
    ellipse.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding(LoadingBase.DotBorderBrushProperty.Name)
    {
      Source = (object) this
    });
    return ellipse;
  }
}

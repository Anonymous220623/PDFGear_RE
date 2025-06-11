// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class SurfaceBase : Control
{
  private Size? rootPanelDesiredSize;
  internal ChartRootPanel RootPanel;
  internal bool IsPointsGenerated;
  internal bool CanDrawMaterial;
  internal bool IsUpdateDispatched;
  internal ContentControl Container;
  internal bool CanDrawWall;
  internal bool IsUpdateLegend = true;
  internal DoubleRange XRange = DoubleRange.Empty;
  internal DoubleRange YRange = DoubleRange.Empty;
  internal DoubleRange ZRange = DoubleRange.Empty;
  public static readonly DependencyProperty EnableRotationProperty = DependencyProperty.Register(nameof (EnableRotation), typeof (bool), typeof (SurfaceBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ShowLeftWallProperty = DependencyProperty.Register(nameof (ShowLeftWall), typeof (bool), typeof (SurfaceBase), new PropertyMetadata((object) true, new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty ShowBottomWallProperty = DependencyProperty.Register(nameof (ShowBottomWall), typeof (bool), typeof (SurfaceBase), new PropertyMetadata((object) true, new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty ShowBackWallProperty = DependencyProperty.Register(nameof (ShowBackWall), typeof (bool), typeof (SurfaceBase), new PropertyMetadata((object) true, new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty LeftWallBrushProperty = DependencyProperty.Register(nameof (LeftWallBrush), typeof (Brush), typeof (SurfaceBase), new PropertyMetadata((object) new SolidColorBrush(Color.FromRgb((byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty BottomWallBrushProperty = DependencyProperty.Register(nameof (BottomWallBrush), typeof (Brush), typeof (SurfaceBase), new PropertyMetadata((object) new SolidColorBrush(Color.FromRgb((byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty BackWallBrushProperty = DependencyProperty.Register(nameof (BackWallBrush), typeof (Brush), typeof (SurfaceBase), new PropertyMetadata((object) new SolidColorBrush(Color.FromRgb((byte) 211, (byte) 211, (byte) 211)), new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty WallThicknessProperty = DependencyProperty.Register(nameof (WallThickness), typeof (WallThickness), typeof (SurfaceBase), new PropertyMetadata((object) new WallThickness(0.03), new PropertyChangedCallback(SurfaceBase.OnWallPropertyChanged)));
  public static readonly DependencyProperty ColorModelProperty = DependencyProperty.Register(nameof (ColorModel), typeof (ChartColorModel), typeof (SurfaceBase), new PropertyMetadata((object) null, new PropertyChangedCallback(SurfaceBase.OnColorModelChanged)));
  public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(nameof (Palette), typeof (ChartColorPalette), typeof (SurfaceBase), new PropertyMetadata((object) ChartColorPalette.AutumnBrights, new PropertyChangedCallback(SurfaceBase.OnPaletteChanged)));
  public static readonly DependencyProperty RotateProperty = DependencyProperty.Register(nameof (Rotate), typeof (double), typeof (SurfaceBase), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(SurfaceBase.OnRotatePropertyChanged)));
  public static readonly DependencyProperty TiltProperty = DependencyProperty.Register(nameof (Tilt), typeof (double), typeof (SurfaceBase), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(SurfaceBase.OnRotatePropertyChanged)));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (SurfaceBase), new PropertyMetadata((PropertyChangedCallback) null));

  internal SurfaceAxis InterernalXAxis { get; set; }

  internal SurfaceAxis InterernalYAxis { get; set; }

  internal SurfaceAxis InterernalZAxis { get; set; }

  internal ChartDockPanel DockPanel { get; set; }

  internal AxisCollection Axes { get; set; }

  internal Viewport3D Viewport { get; set; }

  public bool EnableRotation
  {
    get => (bool) this.GetValue(SurfaceBase.EnableRotationProperty);
    set => this.SetValue(SurfaceBase.EnableRotationProperty, (object) value);
  }

  public bool ShowLeftWall
  {
    get => (bool) this.GetValue(SurfaceBase.ShowLeftWallProperty);
    set => this.SetValue(SurfaceBase.ShowLeftWallProperty, (object) value);
  }

  public bool ShowBottomWall
  {
    get => (bool) this.GetValue(SurfaceBase.ShowBottomWallProperty);
    set => this.SetValue(SurfaceBase.ShowBottomWallProperty, (object) value);
  }

  public bool ShowBackWall
  {
    get => (bool) this.GetValue(SurfaceBase.ShowBackWallProperty);
    set => this.SetValue(SurfaceBase.ShowBackWallProperty, (object) value);
  }

  public Brush LeftWallBrush
  {
    get => (Brush) this.GetValue(SurfaceBase.LeftWallBrushProperty);
    set => this.SetValue(SurfaceBase.LeftWallBrushProperty, (object) value);
  }

  public Brush BottomWallBrush
  {
    get => (Brush) this.GetValue(SurfaceBase.BottomWallBrushProperty);
    set => this.SetValue(SurfaceBase.BottomWallBrushProperty, (object) value);
  }

  public Brush BackWallBrush
  {
    get => (Brush) this.GetValue(SurfaceBase.BackWallBrushProperty);
    set => this.SetValue(SurfaceBase.BackWallBrushProperty, (object) value);
  }

  public WallThickness WallThickness
  {
    get => (WallThickness) this.GetValue(SurfaceBase.WallThicknessProperty);
    set => this.SetValue(SurfaceBase.WallThicknessProperty, (object) value);
  }

  private static void OnWallPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SurfaceBase surfaceBase))
      return;
    surfaceBase.CanDrawWall = false;
    surfaceBase.ScheduleUpdate();
  }

  public ChartColorModel ColorModel
  {
    get => (ChartColorModel) this.GetValue(SurfaceBase.ColorModelProperty);
    set => this.SetValue(SurfaceBase.ColorModelProperty, (object) value);
  }

  private static void OnColorModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SurfaceBase surfaceBase) || surfaceBase.ColorModel == null)
      return;
    surfaceBase.ColorModel.Palette = surfaceBase.Palette;
  }

  public ChartColorPalette Palette
  {
    get => (ChartColorPalette) this.GetValue(SurfaceBase.PaletteProperty);
    set => this.SetValue(SurfaceBase.PaletteProperty, (object) value);
  }

  private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SurfaceBase surfaceBase))
      return;
    if (surfaceBase.ColorModel != null)
      surfaceBase.ColorModel.Palette = surfaceBase.Palette;
    surfaceBase.ScheduleUpdate();
    surfaceBase.IsPointsGenerated = false;
    surfaceBase.CanDrawMaterial = false;
    surfaceBase.IsUpdateLegend = true;
  }

  public double Rotate
  {
    get => (double) this.GetValue(SurfaceBase.RotateProperty);
    set => this.SetValue(SurfaceBase.RotateProperty, (object) value);
  }

  private static void OnRotatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((SurfaceBase) d).OnRotatePropertyChanged();
  }

  private void OnRotatePropertyChanged()
  {
    if (this.Viewport == null)
      return;
    this.PositionCamera(this.Viewport.Camera as ProjectionCamera);
  }

  public double Tilt
  {
    get => (double) this.GetValue(SurfaceBase.TiltProperty);
    set => this.SetValue(SurfaceBase.TiltProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(SurfaceBase.HeaderProperty);
    set => this.SetValue(SurfaceBase.HeaderProperty, value);
  }

  internal Size? RootPanelDesiredSize
  {
    get => this.rootPanelDesiredSize;
    set
    {
      Size? panelDesiredSize = this.rootPanelDesiredSize;
      Size? nullable = value;
      if ((panelDesiredSize.HasValue != nullable.HasValue ? 0 : (!panelDesiredSize.HasValue ? 1 : (panelDesiredSize.GetValueOrDefault() == nullable.GetValueOrDefault() ? 1 : 0))) != 0)
        return;
      this.rootPanelDesiredSize = value;
      this.OnRootPanelSizeChanged(value.HasValue ? value.Value : new Size());
    }
  }

  private void OnRootPanelSizeChanged(Size size) => this.UpdateArea();

  public void Serialize()
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    File.WriteAllText(Directory.GetParent("../").FullName + "\\surfaceChart.xml", result.ToString());
  }

  public void Serialize(string filePath)
  {
    if (string.IsNullOrEmpty(filePath))
      return;
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    File.WriteAllText(filePath, result.ToString());
  }

  public object Deserialize()
  {
    return XamlReader.Load(new StreamReader(Directory.GetParent("../").FullName + "\\surfaceChart.xml").BaseStream);
  }

  public object Deserialize(string filePath)
  {
    return !string.IsNullOrEmpty(filePath) ? XamlReader.Load(new StreamReader(filePath).BaseStream) : (object) null;
  }

  internal void ScheduleUpdate()
  {
    if (this.IsUpdateDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.UpdateArea));
    this.IsUpdateDispatched = true;
  }

  internal virtual void UpdateArea()
  {
  }

  protected internal virtual void PositionCamera(ProjectionCamera camera)
  {
  }
}

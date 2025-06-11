// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.FlipNumber
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

#nullable disable
namespace HandyControl.Controls;

public class FlipNumber : Viewport3D
{
  private bool _isLoaded;
  private TextBlock _page1TextDown;
  private TextBlock _page2TextUp;
  private TextBlock _page2TextDown;
  private TextBlock _page3TextUp;
  private ContainerUIElement3D _page1;
  private ContainerUIElement3D _page2;
  private ContainerUIElement3D _page3;
  private ContainerUIElement3D _content;
  private readonly AxisAngleRotation3D _pageRotation3D;
  private readonly DoubleAnimation _animation;
  private bool _isAnimating;
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (FlipNumber), new PropertyMetadata((object) new CornerRadius(4.0)));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (FlipNumber), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (FlipNumber), new PropertyMetadata((object) null));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (FlipNumber), new PropertyMetadata((object) 70.0));
  public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(nameof (Number), typeof (int), typeof (FlipNumber), new PropertyMetadata(ValueBoxes.Int0Box, new PropertyChangedCallback(FlipNumber.OnNumberChanged)));

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(FlipNumber.CornerRadiusProperty);
    set => this.SetValue(FlipNumber.CornerRadiusProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(FlipNumber.BackgroundProperty);
    set => this.SetValue(FlipNumber.BackgroundProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(FlipNumber.ForegroundProperty);
    set => this.SetValue(FlipNumber.ForegroundProperty, (object) value);
  }

  [TypeConverter(typeof (FontSizeConverter))]
  public double FontSize
  {
    get => (double) this.GetValue(FlipNumber.FontSizeProperty);
    set => this.SetValue(FlipNumber.FontSizeProperty, (object) value);
  }

  private static void OnNumberChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
  {
    ((FlipNumber) s).OnNumberChanged();
  }

  public int Number
  {
    get => (int) this.GetValue(FlipNumber.NumberProperty);
    set => this.SetValue(FlipNumber.NumberProperty, (object) value);
  }

  public FlipNumber()
  {
    this.Children.Add((Visual3D) new ModelVisual3D()
    {
      Content = (Model3D) new DirectionalLight()
    });
    this._pageRotation3D = new AxisAngleRotation3D()
    {
      Angle = 0.0,
      Axis = new Vector3D(1.0, 0.0, 0.0)
    };
    DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 180.0, new Duration(TimeSpan.FromSeconds(0.8)));
    doubleAnimation.FillBehavior = FillBehavior.Stop;
    this._animation = doubleAnimation;
    this._animation.Completed += new EventHandler(this.Animation_Completed);
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._isLoaded)
        return;
      this._isLoaded = true;
      this.InitNumber();
      this._page2.Transform = (Transform3D) new RotateTransform3D((Rotation3D) this._pageRotation3D);
    });
  }

  private void Animation_Completed(object sender, EventArgs e)
  {
    this._isAnimating = false;
    this.UpdateNumber();
  }

  private void InitNumber()
  {
    this._page1 = new ContainerUIElement3D();
    int num = this.Number > 8 ? 0 : this.Number + 1;
    this._page1.Children.Add((Visual3D) this.CreateNumber(num, false, out this._page1TextDown));
    this._page2 = new ContainerUIElement3D();
    Viewport2DVisual3D number1 = this.CreateNumber(num, true, out this._page2TextUp);
    Viewport2DVisual3D number2 = this.CreateNumber(this.Number, false, out this._page2TextDown);
    this._page2.Children.Add((Visual3D) number1);
    this._page2.Children.Add((Visual3D) number2);
    this._page3 = new ContainerUIElement3D();
    this._page3.Children.Add((Visual3D) this.CreateNumber(this.Number, true, out this._page3TextUp));
    this._page3.Transform = (Transform3D) new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
    {
      Angle = 180.0,
      Axis = new Vector3D(1.0, 0.0, 0.0)
    });
    this._content = new ContainerUIElement3D()
    {
      Children = {
        (Visual3D) this._page1,
        (Visual3D) this._page2,
        (Visual3D) this._page3
      }
    };
    this.Children.Add((Visual3D) this._content);
  }

  private bool CheckNull()
  {
    return this._page1TextDown != null && this._page2TextUp != null && this._page2TextDown != null && this._page3TextUp != null;
  }

  private void OnNumberChanged()
  {
    if (!this.CheckNull())
      return;
    this.InitNewNumber();
    if (this._isAnimating)
    {
      this._isAnimating = false;
      this.UpdateNumber();
    }
    else
    {
      this._isAnimating = true;
      this._pageRotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty, (AnimationTimeline) this._animation);
    }
  }

  private void InitNewNumber()
  {
    string str = this.Number.ToString();
    this._page1TextDown.Text = str;
    this._page2TextUp.Text = str;
  }

  private void UpdateNumber()
  {
    this._pageRotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty, (AnimationTimeline) null);
    this._pageRotation3D.Angle = 0.0;
    string str = this.Number.ToString();
    this._page2TextDown.Text = str;
    this._page3TextUp.Text = str;
    this._isAnimating = false;
  }

  private Viewport2DVisual3D CreateNumber(int num, bool isUp, out TextBlock textBlock)
  {
    RotateTransform rotateTransform = new RotateTransform();
    int num1;
    if (isUp)
    {
      num1 = -1;
      rotateTransform.Angle = 180.0;
    }
    else
      num1 = 1;
    double num2 = this.ActualWidth / 2.0;
    double num3 = this.ActualWidth / 4.0;
    double y = this.ActualHeight / 4.0;
    DiffuseMaterial element = new DiffuseMaterial();
    Viewport2DVisual3D.SetIsVisualHostMaterial((Material) element, true);
    ref TextBlock local = ref textBlock;
    TextBlock textBlock1 = new TextBlock();
    textBlock1.RenderTransformOrigin = new Point(0.5, 0.5);
    textBlock1.Foreground = this.Foreground;
    textBlock1.FontSize = this.FontSize;
    textBlock1.VerticalAlignment = VerticalAlignment.Center;
    textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
    textBlock1.TextAlignment = TextAlignment.Center;
    textBlock1.Text = num.ToString();
    textBlock1.RenderTransform = (Transform) rotateTransform;
    textBlock1.Margin = new Thickness(0.0, 0.0, 0.0, -y);
    textBlock1.FontFamily = new FontFamily("Consolas");
    local = textBlock1;
    Border border1 = new Border();
    border1.ClipToBounds = true;
    border1.CornerRadius = new CornerRadius(this.CornerRadius.TopLeft, this.CornerRadius.TopRight, 0.0, 0.0);
    border1.Background = this.Background;
    border1.Width = num2;
    border1.Height = y;
    border1.Child = (UIElement) textBlock;
    Border border2 = border1;
    Point3DCollection point3Dcollection = new Point3DCollection()
    {
      new Point3D(-num3 * (double) num1, y, 0.0),
      new Point3D(-num3 * (double) num1, 0.0, 0.0),
      new Point3D(num3 * (double) num1, 0.0, 0.0),
      new Point3D(num3 * (double) num1, y, 0.0)
    };
    Int32Collection int32Collection = new Int32Collection()
    {
      0,
      1,
      2,
      0,
      2,
      3
    };
    PointCollection pointCollection = new PointCollection()
    {
      new Point(0.0, 0.0),
      new Point(0.0, 1.0),
      new Point(1.0, 1.0),
      new Point(1.0, 0.0)
    };
    MeshGeometry3D meshGeometry3D = new MeshGeometry3D()
    {
      Positions = point3Dcollection,
      TriangleIndices = int32Collection,
      TextureCoordinates = pointCollection
    };
    return new Viewport2DVisual3D()
    {
      Geometry = (Geometry3D) meshGeometry3D,
      Visual = (Visual) border2,
      Material = (Material) element
    };
  }
}

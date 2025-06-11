// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.TorePaperControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public partial class TorePaperControl : UserControl, IComponentConnector
{
  private const double ToreHeight = 10.0;
  private const double HalfToreWidth = 35.0;
  public static readonly DependencyProperty ToreEdgeProperty = DependencyProperty.Register(nameof (ToreEdge), typeof (ToreEdge), typeof (TorePaperControl), new PropertyMetadata((object) ToreEdge.Bottom, new PropertyChangedCallback(TorePaperControl.OnToreEdgePropertyChanged)));
  public static readonly DependencyProperty ContentBrushProperty = DependencyProperty.Register(nameof (ContentBrush), typeof (Brush), typeof (TorePaperControl), new PropertyMetadata((PropertyChangedCallback) null));
  internal Path ContentPath;
  private bool _contentLoaded;

  public TorePaperControl() => this.InitializeComponent();

  public ToreEdge ToreEdge
  {
    get => (ToreEdge) this.GetValue(TorePaperControl.ToreEdgeProperty);
    set => this.SetValue(TorePaperControl.ToreEdgeProperty, (object) value);
  }

  private static void OnToreEdgePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TorePaperControl torePaperControl))
      return;
    torePaperControl.UpdateContent();
  }

  public Brush ContentBrush
  {
    get => (Brush) this.GetValue(TorePaperControl.ContentBrushProperty);
    set => this.SetValue(TorePaperControl.ContentBrushProperty, (object) value);
  }

  private void UpdateContent() => this.ContentPath.Data = this.BuildContentGeometry();

  private Geometry BuildContentGeometry()
  {
    ToreEdge toreEdge = this.ToreEdge;
    double actualWidth = this.ActualWidth;
    double actualHeight = this.ActualHeight;
    if (actualHeight <= 10.0)
      return (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, actualWidth, actualHeight)
      };
    int num1 = (int) Math.Ceiling(actualWidth / 35.0);
    PathFigure pathFigure = new PathFigure();
    pathFigure.StartPoint = toreEdge != ToreEdge.Bottom ? new Point(0.0, 10.0) : new Point(0.0, actualHeight);
    for (int index = 0; index < num1; ++index)
    {
      double x = 35.0 * (double) (index + 1);
      double y = index % 2 == 0 ? 0.0 : 10.0;
      if (x > actualWidth)
      {
        double num2 = 35.0 - (x - actualWidth);
        double num3 = 2.0 / 7.0;
        y = index % 2 == 0 ? 10.0 - num3 * num2 : num3 * num2;
        x = actualWidth;
      }
      if (toreEdge == ToreEdge.Bottom)
        y += actualHeight - 10.0;
      pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(x, y), true));
    }
    if (toreEdge == ToreEdge.Bottom)
    {
      pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(actualWidth, 0.0), true));
      pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(0.0, 0.0), true));
    }
    else
    {
      pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(actualWidth, actualHeight), true));
      pathFigure.Segments.Add((PathSegment) new LineSegment(new Point(0.0, actualHeight), true));
    }
    pathFigure.IsClosed = true;
    return (Geometry) new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
  }

  private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateContent();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageheaderfooters/torepapercontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
        this.ContentPath = (Path) target;
      else
        this._contentLoaded = true;
    }
    else
      ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
  }
}

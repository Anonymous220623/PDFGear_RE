// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.GradientStopItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
internal class GradientStopItem : Control
{
  internal ColorEdit cedit;
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (color), typeof (Color), typeof (GradientStopItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.White, FrameworkPropertyMetadataOptions.AffectsArrange));
  private Thumb th;

  public Color color
  {
    get => (Color) this.GetValue(GradientStopItem.ColorProperty);
    set => this.SetValue(GradientStopItem.ColorProperty, (object) value);
  }

  internal Canvas gradientitem { get; set; }

  internal bool isselected { get; set; }

  internal double offset { get; set; }

  internal bool isEnabled { get; set; }

  public GradientStopItem(Color col, bool sel, double off, ColorEdit edit)
  {
    this.gradientitem = new Canvas();
    this.color = col;
    this.offset = off;
    this.cedit = edit;
    this.isselected = sel;
    this.createitem();
  }

  private void createitem()
  {
    Geometry geometry = (Geometry) new PathGeometry();
    (geometry as PathGeometry).Figures.Add(new PathFigure()
    {
      StartPoint = new Point(0.0, 0.0),
      Segments = {
        (PathSegment) new LineSegment()
        {
          Point = new Point(0.0, 0.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(-6.0, 6.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(6.0, 6.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(0.0, 0.0)
        }
      }
    });
    Path path1 = new Path();
    Path path2 = new Path();
    path1.SetValue(Path.DataProperty, (object) geometry);
    this.th = new Thumb();
    this.th.Height = 15.0;
    this.th.Width = 7.0;
    path2.Height = 17.0;
    path2.Width = 17.0;
    Control element = new Control();
    element.Template = this.cedit.ThumbTemplate;
    element.Focusable = false;
    this.gradientitem.Children.Add((UIElement) element);
    this.th.MouseLeftButtonDown += new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonDown);
    this.gradientitem.MouseLeftButtonDown += new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonDown);
    this.gradientitem.MouseMove += new MouseEventHandler(this.gradientitem_MouseMove);
    this.gradientitem.MouseLeftButtonUp += new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonUp);
  }

  public static ColorEdit GetBrushEditParentFromChildren(FrameworkElement element)
  {
    parentFromChildren = (ColorEdit) null;
    if (element != null && !(element is ColorEdit parentFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element != null && element.GetType() == VisualUtils.RootPopupType)
        {
          parentFromChildren = GradientStopItem.GetBrushEditParentFromChildren((FrameworkElement) (element.Parent as Popup));
          if (parentFromChildren != null)
            break;
        }
        if (element is ColorEdit)
        {
          parentFromChildren = (ColorEdit) element;
          break;
        }
      }
    }
    return parentFromChildren;
  }

  private void gradientitem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    e.GetPosition((IInputElement) null);
    ((UIElement) sender).CaptureMouse();
    this.cedit = GradientStopItem.GetBrushEditParentFromChildren((FrameworkElement) this.gradientitem);
    this.cedit.RemoveSelection();
    this.cedit.Focus();
    this.cedit.gradientItemCollection.gradientItem = this;
    this.cedit.rgbChanged = true;
    this.cedit.Color = this.color;
    this.cedit.fillGradient(this);
    this.isselected = true;
    (this.gradientitem.Children[0] as Control).SetValue(Panel.ZIndexProperty, (object) 1);
    this.isEnabled = true;
  }

  private void gradientitem_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.isEnabled || this.gradientitem == null)
      return;
    this.cedit = GradientStopItem.GetBrushEditParentFromChildren((FrameworkElement) this.gradientitem);
    Canvas parent = (Canvas) this.gradientitem.Parent;
    Point position = e.GetPosition((IInputElement) parent);
    e.GetPosition((IInputElement) null);
    if (parent == null || position.X >= parent.ActualWidth || position.X <= 0.0)
      return;
    this.gradientitem.SetValue(Canvas.LeftProperty, (object) position.X);
    this.offset = position.X / parent.ActualWidth;
    if (this.cedit == null)
      return;
    this.cedit.fillGradient(this);
  }

  private void gradientitem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.isEnabled = false;
    ((UIElement) sender).ReleaseMouseCapture();
    this.cedit = GradientStopItem.GetBrushEditParentFromChildren((FrameworkElement) this.gradientitem);
    Canvas parent = (Canvas) this.gradientitem.Parent;
    Point position = e.GetPosition((IInputElement) parent);
    e.GetPosition((IInputElement) this.cedit);
    if (position.X >= parent.Width || position.X <= 0.0)
      return;
    if (position.Y > parent.ActualHeight + 20.0 && this.cedit.gradientItemCollection.Items.Count > 2)
    {
      this.cedit.canvasBar.Children.Remove((UIElement) this.cedit.gradientItemCollection.gradientItem.gradientitem);
      int index = this.cedit.gradientItemCollection.Items.IndexOf((object) this);
      if (index > 0)
        --index;
      this.cedit.gradientItemCollection.gradientItem = (GradientStopItem) this.cedit.gradientItemCollection.Items[index];
      this.cedit.gradientItemCollection.Items.Remove((object) this);
      this.cedit.fillGradient(this.cedit.gradientItemCollection.gradientItem);
    }
    else
    {
      this.gradientitem.SetValue(Canvas.LeftProperty, (object) position.X);
      this.offset = position.X / parent.ActualWidth;
      this.cedit.fillGradient(this);
    }
  }

  internal void Dispose()
  {
    if (this.gradientitem != null)
    {
      this.gradientitem.MouseLeftButtonDown -= new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonDown);
      this.gradientitem.MouseMove -= new MouseEventHandler(this.gradientitem_MouseMove);
      this.gradientitem.MouseLeftButtonUp -= new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonUp);
      this.gradientitem = (Canvas) null;
    }
    if (this.th != null)
    {
      this.th.MouseLeftButtonDown -= new MouseButtonEventHandler(this.gradientitem_MouseLeftButtonDown);
      this.th = (Thumb) null;
    }
    if (this.cedit == null)
      return;
    this.cedit = (ColorEdit) null;
  }
}

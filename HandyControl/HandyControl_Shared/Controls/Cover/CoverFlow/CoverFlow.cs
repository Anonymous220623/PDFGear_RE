// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CoverFlow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Viewport3D", Type = typeof (Viewport3D))]
[TemplatePart(Name = "PART_Camera", Type = typeof (ProjectionCamera))]
[TemplatePart(Name = "PART_VisualParent", Type = typeof (ModelVisual3D))]
public class CoverFlow : Control
{
  private const string ElementViewport3D = "PART_Viewport3D";
  private const string ElementCamera = "PART_Camera";
  private const string ElementVisualParent = "PART_VisualParent";
  private const int MaxShowCountHalf = 3;
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (CoverFlow), new PropertyMetadata(ValueBoxes.Int0Box, new PropertyChangedCallback(CoverFlow.OnPageIndexChanged), new CoerceValueCallback(CoverFlow.CoercePageIndex)));
  public static readonly DependencyProperty LoopProperty = DependencyProperty.Register(nameof (Loop), typeof (bool), typeof (CoverFlow), new PropertyMetadata(ValueBoxes.FalseBox));
  private readonly Dictionary<int, object> _contentDic = new Dictionary<int, object>();
  private readonly Dictionary<int, CoverFlowItem> _itemShowDic = new Dictionary<int, CoverFlowItem>();
  private ProjectionCamera _camera;
  private Point3DAnimation _point3DAnimation;
  private Viewport3D _viewport3D;
  private ModelVisual3D _visualParent;
  private int _firstShowIndex;
  private int _lastShowIndex;

  private static object CoercePageIndex(DependencyObject d, object baseValue)
  {
    CoverFlow coverFlow = (CoverFlow) d;
    int num = (int) baseValue;
    if (num < 0)
      return (object) 0;
    return num >= coverFlow._contentDic.Count ? (object) (coverFlow._contentDic.Count - 1) : (object) num;
  }

  private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CoverFlow) d).UpdateIndex((int) e.NewValue);
  }

  public int PageIndex
  {
    get => (int) this.GetValue(CoverFlow.PageIndexProperty);
    set => this.SetValue(CoverFlow.PageIndexProperty, (object) value);
  }

  public bool Loop
  {
    get => (bool) this.GetValue(CoverFlow.LoopProperty);
    set => this.SetValue(CoverFlow.LoopProperty, ValueBoxes.BooleanBox(value));
  }

  public override void OnApplyTemplate()
  {
    if (this._viewport3D != null)
    {
      this._viewport3D.Children.Clear();
      this._itemShowDic.Clear();
      this._viewport3D.MouseLeftButtonDown -= new MouseButtonEventHandler(this.Viewport3D_MouseLeftButtonDown);
    }
    base.OnApplyTemplate();
    this._viewport3D = this.GetTemplateChild("PART_Viewport3D") as Viewport3D;
    if (this._viewport3D != null)
      this._viewport3D.MouseLeftButtonDown += new MouseButtonEventHandler(this.Viewport3D_MouseLeftButtonDown);
    this._camera = this.GetTemplateChild("PART_Camera") as ProjectionCamera;
    this._visualParent = this.GetTemplateChild("PART_VisualParent") as ModelVisual3D;
    this.UpdateShowRange();
    this._point3DAnimation = new Point3DAnimation(new Point3D(CoverFlowItem.Interval * (double) this.PageIndex, this._camera.Position.Y, this._camera.Position.Z), new Duration(TimeSpan.FromMilliseconds(200.0)));
    this._camera.BeginAnimation(ProjectionCamera.PositionProperty, (AnimationTimeline) this._point3DAnimation);
  }

  public void AddRange(IEnumerable<object> contentList)
  {
    foreach (object content in contentList)
      this._contentDic.Add(this._contentDic.Count, content);
  }

  public void Add(string uriString)
  {
    this._contentDic.Add(this._contentDic.Count, (object) new Uri(uriString));
  }

  public void Add(Uri uri) => this._contentDic.Add(this._contentDic.Count, (object) uri);

  public void Next() => ++this.PageIndex;

  public void Prev() => --this.PageIndex;

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (e.Delta < 0)
    {
      int num = this.PageIndex + 1;
      this.PageIndex = num >= this._contentDic.Count ? (this.Loop ? 0 : this._contentDic.Count - 1) : num;
    }
    else
    {
      int num = this.PageIndex - 1;
      this.PageIndex = num < 0 ? (this.Loop ? this._contentDic.Count - 1 : 0) : num;
    }
    e.Handled = true;
  }

  private void Remove(int pos)
  {
    CoverFlowItem coverFlowItem;
    if (!this._itemShowDic.TryGetValue(pos, out coverFlowItem))
      return;
    this._visualParent.Children.Remove((Visual3D) coverFlowItem);
    this._itemShowDic.Remove(pos);
  }

  private void Viewport3D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    RayMeshGeometry3DHitTestResult geometry3DhitTestResult = (RayMeshGeometry3DHitTestResult) VisualTreeHelper.HitTest((Visual) this._viewport3D, e.GetPosition((IInputElement) this._viewport3D));
    if (geometry3DhitTestResult == null)
      return;
    foreach (CoverFlowItem coverFlowItem in this._itemShowDic.Values)
    {
      if (coverFlowItem.HitTest(geometry3DhitTestResult.MeshHit))
      {
        this.PageIndex = coverFlowItem.Index;
        break;
      }
    }
  }

  private void UpdateIndex(int newIndex)
  {
    if (!this.IsLoaded)
      return;
    this.UpdateShowRange();
    this._itemShowDic.Do<KeyValuePair<int, CoverFlowItem>>((Action<KeyValuePair<int, CoverFlowItem>>) (item =>
    {
      if (item.Value.Index < this._firstShowIndex || item.Value.Index > this._lastShowIndex)
        this.Remove(item.Value.Index);
      else
        item.Value.Move(this.PageIndex);
    }));
    this._point3DAnimation = new Point3DAnimation(new Point3D(CoverFlowItem.Interval * (double) newIndex, this._camera.Position.Y, this._camera.Position.Z), new Duration(TimeSpan.FromMilliseconds(200.0)));
    this._camera.BeginAnimation(ProjectionCamera.PositionProperty, (AnimationTimeline) this._point3DAnimation);
  }

  private void UpdateShowRange()
  {
    int num1 = Math.Max(this.PageIndex - 3, 0);
    int num2 = Math.Min(this.PageIndex + 3, this._contentDic.Count - 1);
    for (int index = num1; index <= num2; ++index)
    {
      if (!this._itemShowDic.ContainsKey(index))
      {
        CoverFlowItem coverFlowItem = this.CreateCoverFlowItem(index, this._contentDic[index]);
        this._itemShowDic[index] = coverFlowItem;
        this._visualParent.Children.Add((Visual3D) coverFlowItem);
      }
    }
    this._firstShowIndex = num1;
    this._lastShowIndex = num2;
  }

  private CoverFlowItem CreateCoverFlowItem(int index, object content)
  {
    Uri bitmapUri = content as Uri;
    if ((object) bitmapUri != null)
    {
      try
      {
        return new CoverFlowItem(index, this.PageIndex, (UIElement) new Image()
        {
          Source = (ImageSource) BitmapFrame.Create(bitmapUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default)
        });
      }
      catch
      {
        return new CoverFlowItem(index, this.PageIndex, (UIElement) new ContentControl());
      }
    }
    else
      return new CoverFlowItem(index, this.PageIndex, (UIElement) new ContentControl()
      {
        Content = content
      });
  }
}

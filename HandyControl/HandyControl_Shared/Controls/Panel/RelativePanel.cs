// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RelativePanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Controls;

public class RelativePanel : Panel
{
  private readonly RelativePanel.Graph _childGraph;
  public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached("AlignLeftWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached("AlignTopWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached("AlignRightWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached("AlignBottomWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignLeftWithProperty = DependencyProperty.RegisterAttached("AlignLeftWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignTopWithProperty = DependencyProperty.RegisterAttached("AlignTopWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignRightWithProperty = DependencyProperty.RegisterAttached("AlignRightWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignBottomWithProperty = DependencyProperty.RegisterAttached("AlignBottomWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached("LeftOf", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached("Above", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached("RightOf", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached("Below", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached("AlignHorizontalCenterWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached("AlignVerticalCenterWithPanel", typeof (bool), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached("AlignHorizontalCenterWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached("AlignVerticalCenterWith", typeof (UIElement), typeof (RelativePanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));

  public RelativePanel() => this._childGraph = new RelativePanel.Graph();

  public static void SetAlignLeftWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignLeftWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignLeftWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignLeftWithPanelProperty);
  }

  public static void SetAlignTopWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignTopWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignTopWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignTopWithPanelProperty);
  }

  public static void SetAlignRightWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignRightWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignRightWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignRightWithPanelProperty);
  }

  public static void SetAlignBottomWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignBottomWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignBottomWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignBottomWithPanelProperty);
  }

  public static void SetAlignLeftWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignLeftWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignLeftWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignLeftWithProperty);
  }

  public static void SetAlignTopWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignTopWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignTopWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignTopWithProperty);
  }

  public static void SetAlignRightWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignRightWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignRightWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignRightWithProperty);
  }

  public static void SetAlignBottomWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignBottomWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignBottomWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignBottomWithProperty);
  }

  public static void SetLeftOf(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.LeftOfProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetLeftOf(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.LeftOfProperty);
  }

  public static void SetAbove(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AboveProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAbove(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AboveProperty);
  }

  public static void SetRightOf(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.RightOfProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetRightOf(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.RightOfProperty);
  }

  public static void SetBelow(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.BelowProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetBelow(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.BelowProperty);
  }

  public static void SetAlignHorizontalCenterWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignHorizontalCenterWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignHorizontalCenterWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignHorizontalCenterWithPanelProperty);
  }

  public static void SetAlignVerticalCenterWithPanel(DependencyObject element, bool value)
  {
    element.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAlignVerticalCenterWithPanel(DependencyObject element)
  {
    return (bool) element.GetValue(RelativePanel.AlignVerticalCenterWithPanelProperty);
  }

  public static void SetAlignHorizontalCenterWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignHorizontalCenterWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignHorizontalCenterWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignHorizontalCenterWithProperty);
  }

  public static void SetAlignVerticalCenterWith(DependencyObject element, UIElement value)
  {
    element.SetValue(RelativePanel.AlignVerticalCenterWithProperty, (object) value);
  }

  [TypeConverter(typeof (NameReferenceConverter))]
  public static UIElement GetAlignVerticalCenterWith(DependencyObject element)
  {
    return (UIElement) element.GetValue(RelativePanel.AlignVerticalCenterWithProperty);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this._childGraph.Clear();
    foreach (UIElement internalChild in this.InternalChildren)
    {
      if (internalChild != null)
      {
        RelativePanel.GraphNode from = this._childGraph.AddNode(internalChild);
        from.AlignLeftWithNode = this._childGraph.AddLink(from, RelativePanel.GetAlignLeftWith((DependencyObject) internalChild));
        from.AlignTopWithNode = this._childGraph.AddLink(from, RelativePanel.GetAlignTopWith((DependencyObject) internalChild));
        from.AlignRightWithNode = this._childGraph.AddLink(from, RelativePanel.GetAlignRightWith((DependencyObject) internalChild));
        from.AlignBottomWithNode = this._childGraph.AddLink(from, RelativePanel.GetAlignBottomWith((DependencyObject) internalChild));
        from.LeftOfNode = this._childGraph.AddLink(from, RelativePanel.GetLeftOf((DependencyObject) internalChild));
        from.AboveNode = this._childGraph.AddLink(from, RelativePanel.GetAbove((DependencyObject) internalChild));
        from.RightOfNode = this._childGraph.AddLink(from, RelativePanel.GetRightOf((DependencyObject) internalChild));
        from.BelowNode = this._childGraph.AddLink(from, RelativePanel.GetBelow((DependencyObject) internalChild));
        from.AlignHorizontalCenterWith = this._childGraph.AddLink(from, RelativePanel.GetAlignHorizontalCenterWith((DependencyObject) internalChild));
        from.AlignVerticalCenterWith = this._childGraph.AddLink(from, RelativePanel.GetAlignVerticalCenterWith((DependencyObject) internalChild));
      }
    }
    this._childGraph.Measure(availableSize);
    this._childGraph.Reset(false);
    Size boundingSize = this._childGraph.GetBoundingSize(this.Width.IsNaN() && this.HorizontalAlignment != HorizontalAlignment.Stretch, this.Height.IsNaN() && this.VerticalAlignment != VerticalAlignment.Stretch);
    this._childGraph.Reset();
    this._childGraph.Measure(boundingSize);
    return boundingSize;
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    this._childGraph.GetNodes().Do<RelativePanel.GraphNode>((Action<RelativePanel.GraphNode>) (node => node.Arrange(arrangeSize)));
    return arrangeSize;
  }

  private class GraphNode
  {
    public bool Measured { get; set; }

    public UIElement Element { get; }

    private bool HorizontalOffsetFlag { get; set; }

    private bool VerticalOffsetFlag { get; set; }

    private Size BoundingSize { get; set; }

    public Size OriginDesiredSize { get; set; }

    public double Left { get; set; } = double.NaN;

    public double Top { get; set; } = double.NaN;

    public double Right { get; set; } = double.NaN;

    public double Bottom { get; set; } = double.NaN;

    public HashSet<RelativePanel.GraphNode> OutgoingNodes { get; }

    public RelativePanel.GraphNode AlignLeftWithNode { get; set; }

    public RelativePanel.GraphNode AlignTopWithNode { get; set; }

    public RelativePanel.GraphNode AlignRightWithNode { get; set; }

    public RelativePanel.GraphNode AlignBottomWithNode { get; set; }

    public RelativePanel.GraphNode LeftOfNode { get; set; }

    public RelativePanel.GraphNode AboveNode { get; set; }

    public RelativePanel.GraphNode RightOfNode { get; set; }

    public RelativePanel.GraphNode BelowNode { get; set; }

    public RelativePanel.GraphNode AlignHorizontalCenterWith { get; set; }

    public RelativePanel.GraphNode AlignVerticalCenterWith { get; set; }

    public GraphNode(UIElement element)
    {
      this.OutgoingNodes = new HashSet<RelativePanel.GraphNode>();
      this.Element = element;
    }

    public void Arrange(Size arrangeSize)
    {
      this.Element.Arrange(new Rect(this.Left, this.Top, Math.Max(arrangeSize.Width - this.Left - this.Right, 0.0), Math.Max(arrangeSize.Height - this.Top - this.Bottom, 0.0)));
    }

    public void Reset(bool clearPos)
    {
      if (clearPos)
      {
        this.Left = double.NaN;
        this.Top = double.NaN;
        this.Right = double.NaN;
        this.Bottom = double.NaN;
      }
      this.Measured = false;
    }

    public Size GetBoundingSize()
    {
      if (this.Left < 0.0 || this.Top < 0.0)
        return new Size();
      if (this.Measured)
        return this.BoundingSize;
      if (!this.OutgoingNodes.Any<RelativePanel.GraphNode>())
      {
        this.BoundingSize = this.Element.DesiredSize;
        this.Measured = true;
      }
      else
      {
        this.BoundingSize = RelativePanel.GraphNode.GetBoundingSize(this, this.Element.DesiredSize, (IEnumerable<RelativePanel.GraphNode>) this.OutgoingNodes);
        this.Measured = true;
      }
      return this.BoundingSize;
    }

    private static Size GetBoundingSize(
      RelativePanel.GraphNode prevNode,
      Size prevSize,
      IEnumerable<RelativePanel.GraphNode> nodes)
    {
      Size boundingSize;
      foreach (RelativePanel.GraphNode node in nodes)
      {
        if (node.Measured || !node.OutgoingNodes.Any<RelativePanel.GraphNode>())
        {
          if (prevNode.LeftOfNode != null && prevNode.LeftOfNode == node || prevNode.RightOfNode != null && prevNode.RightOfNode == node)
          {
            ref Size local1 = ref prevSize;
            double width1 = local1.Width;
            boundingSize = node.BoundingSize;
            double width2 = boundingSize.Width;
            local1.Width = width1 + width2;
            if (RelativePanel.GetAlignHorizontalCenterWithPanel((DependencyObject) node.Element) || node.HorizontalOffsetFlag)
            {
              ref Size local2 = ref prevSize;
              double width3 = local2.Width;
              boundingSize = prevNode.OriginDesiredSize;
              double width4 = boundingSize.Width;
              local2.Width = width3 + width4;
              prevNode.HorizontalOffsetFlag = true;
            }
            if (node.VerticalOffsetFlag)
              prevNode.VerticalOffsetFlag = true;
          }
          if (prevNode.AboveNode != null && prevNode.AboveNode == node || prevNode.BelowNode != null && prevNode.BelowNode == node)
          {
            ref Size local3 = ref prevSize;
            double height1 = local3.Height;
            boundingSize = node.BoundingSize;
            double height2 = boundingSize.Height;
            local3.Height = height1 + height2;
            if (RelativePanel.GetAlignVerticalCenterWithPanel((DependencyObject) node.Element) || node.VerticalOffsetFlag)
            {
              ref Size local4 = ref prevSize;
              double height3 = local4.Height;
              boundingSize = prevNode.OriginDesiredSize;
              double height4 = boundingSize.Height;
              local4.Height = height3 + height4;
              prevNode.VerticalOffsetFlag = true;
            }
            if (node.HorizontalOffsetFlag)
              prevNode.HorizontalOffsetFlag = true;
          }
        }
        else
        {
          boundingSize = RelativePanel.GraphNode.GetBoundingSize(node, prevSize, (IEnumerable<RelativePanel.GraphNode>) node.OutgoingNodes);
          return boundingSize;
        }
      }
      return prevSize;
    }
  }

  private class Graph
  {
    private readonly Dictionary<DependencyObject, RelativePanel.GraphNode> _nodeDic;

    private Size AvailableSize { get; set; }

    public Graph() => this._nodeDic = new Dictionary<DependencyObject, RelativePanel.GraphNode>();

    public IEnumerable<RelativePanel.GraphNode> GetNodes()
    {
      return (IEnumerable<RelativePanel.GraphNode>) this._nodeDic.Values;
    }

    public void Clear()
    {
      this.AvailableSize = new Size();
      this._nodeDic.Clear();
    }

    public void Reset(bool clearPos = true)
    {
      this._nodeDic.Values.Do<RelativePanel.GraphNode>((Action<RelativePanel.GraphNode>) (node => node.Reset(clearPos)));
    }

    public RelativePanel.GraphNode AddLink(RelativePanel.GraphNode from, UIElement to)
    {
      if (to == null)
        return (RelativePanel.GraphNode) null;
      RelativePanel.GraphNode graphNode;
      if (this._nodeDic.ContainsKey((DependencyObject) to))
      {
        graphNode = this._nodeDic[(DependencyObject) to];
      }
      else
      {
        graphNode = new RelativePanel.GraphNode(to);
        this._nodeDic[(DependencyObject) to] = graphNode;
      }
      from.OutgoingNodes.Add(graphNode);
      return graphNode;
    }

    public RelativePanel.GraphNode AddNode(UIElement value)
    {
      if (this._nodeDic.ContainsKey((DependencyObject) value))
        return this._nodeDic[(DependencyObject) value];
      RelativePanel.GraphNode graphNode = new RelativePanel.GraphNode(value);
      this._nodeDic.Add((DependencyObject) value, graphNode);
      return graphNode;
    }

    public void Measure(Size availableSize)
    {
      this.AvailableSize = RelativePanel.Graph.EnsureValidSize(availableSize);
      this.Measure((IEnumerable<RelativePanel.GraphNode>) this._nodeDic.Values, (HashSet<DependencyObject>) null);
    }

    private static Size EnsureValidSize(Size size)
    {
      return new Size(double.IsInfinity(size.Width) ? 0.0 : size.Width, double.IsInfinity(size.Height) ? 0.0 : size.Height);
    }

    private void Measure(IEnumerable<RelativePanel.GraphNode> nodes, HashSet<DependencyObject> set)
    {
      if (set == null)
        set = new HashSet<DependencyObject>();
      foreach (RelativePanel.GraphNode node in nodes)
      {
        if (!node.Measured && !node.OutgoingNodes.Any<RelativePanel.GraphNode>())
          this.MeasureChild(node);
        else if (node.OutgoingNodes.All<RelativePanel.GraphNode>((Func<RelativePanel.GraphNode, bool>) (item => item.Measured)))
        {
          this.MeasureChild(node);
        }
        else
        {
          if (!set.Add((DependencyObject) node.Element))
            throw new Exception("RelativePanel error: Circular dependency detected. Layout could not complete.");
          this.Measure((IEnumerable<RelativePanel.GraphNode>) node.OutgoingNodes, set);
          if (!node.Measured)
            this.MeasureChild(node);
        }
      }
    }

    private void MeasureChild(RelativePanel.GraphNode node)
    {
      UIElement element = node.Element;
      element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      node.OriginDesiredSize = element.DesiredSize;
      int num1 = RelativePanel.GetAlignLeftWithPanel((DependencyObject) element) ? 1 : 0;
      bool alignTopWithPanel = RelativePanel.GetAlignTopWithPanel((DependencyObject) element);
      bool alignRightWithPanel = RelativePanel.GetAlignRightWithPanel((DependencyObject) element);
      bool alignBottomWithPanel = RelativePanel.GetAlignBottomWithPanel((DependencyObject) element);
      if (num1 != 0)
        node.Left = 0.0;
      if (alignTopWithPanel)
        node.Top = 0.0;
      if (alignRightWithPanel)
        node.Right = 0.0;
      if (alignBottomWithPanel)
        node.Bottom = 0.0;
      if (node.AlignLeftWithNode != null)
        node.Left = node.Left.IsNaN() ? node.AlignLeftWithNode.Left : node.AlignLeftWithNode.Left * 0.5;
      if (node.AlignTopWithNode != null)
        node.Top = node.Top.IsNaN() ? node.AlignTopWithNode.Top : node.AlignTopWithNode.Top * 0.5;
      if (node.AlignRightWithNode != null)
        node.Right = node.Right.IsNaN() ? node.AlignRightWithNode.Right : node.AlignRightWithNode.Right * 0.5;
      if (node.AlignBottomWithNode != null)
        node.Bottom = node.Bottom.IsNaN() ? node.AlignBottomWithNode.Bottom : node.AlignBottomWithNode.Bottom * 0.5;
      double val1_1 = this.AvailableSize.Height - node.Top - node.Bottom;
      if (val1_1.IsNaN())
      {
        val1_1 = this.AvailableSize.Height;
        if (!node.Top.IsNaN() && node.Bottom.IsNaN())
          val1_1 -= node.Top;
        else if (node.Top.IsNaN() && !node.Bottom.IsNaN())
          val1_1 -= node.Bottom;
      }
      double val1_2 = this.AvailableSize.Width - node.Left - node.Right;
      if (val1_2.IsNaN())
      {
        val1_2 = this.AvailableSize.Width;
        if (!node.Left.IsNaN() && node.Right.IsNaN())
          val1_2 -= node.Left;
        else if (node.Left.IsNaN() && !node.Right.IsNaN())
          val1_2 -= node.Right;
      }
      element.Measure(new Size(Math.Max(val1_2, 0.0), Math.Max(val1_1, 0.0)));
      Size desiredSize = element.DesiredSize;
      if (node.LeftOfNode != null && node.Left.IsNaN())
        node.Left = node.LeftOfNode.Left - desiredSize.Width;
      if (node.AboveNode != null && node.Top.IsNaN())
        node.Top = node.AboveNode.Top - desiredSize.Height;
      if (node.RightOfNode != null)
      {
        if (node.Right.IsNaN())
          node.Right = node.RightOfNode.Right - desiredSize.Width;
        if (node.Left.IsNaN())
          node.Left = this.AvailableSize.Width - node.RightOfNode.Right;
      }
      if (node.BelowNode != null)
      {
        if (node.Bottom.IsNaN())
          node.Bottom = node.BelowNode.Bottom - desiredSize.Height;
        if (node.Top.IsNaN())
          node.Top = this.AvailableSize.Height - node.BelowNode.Bottom;
      }
      if (node.AlignHorizontalCenterWith != null)
      {
        double num2 = (this.AvailableSize.Width + node.AlignHorizontalCenterWith.Left - node.AlignHorizontalCenterWith.Right - desiredSize.Width) * 0.5;
        double num3 = (this.AvailableSize.Width - node.AlignHorizontalCenterWith.Left + node.AlignHorizontalCenterWith.Right - desiredSize.Width) * 0.5;
        node.Left = !node.Left.IsNaN() ? (node.Left + num2) * 0.5 : num2;
        node.Right = !node.Right.IsNaN() ? (node.Right + num3) * 0.5 : num3;
      }
      if (node.AlignVerticalCenterWith != null)
      {
        double num4 = (this.AvailableSize.Height + node.AlignVerticalCenterWith.Top - node.AlignVerticalCenterWith.Bottom - desiredSize.Height) * 0.5;
        double num5 = (this.AvailableSize.Height - node.AlignVerticalCenterWith.Top + node.AlignVerticalCenterWith.Bottom - desiredSize.Height) * 0.5;
        node.Top = !node.Top.IsNaN() ? (node.Top + num4) * 0.5 : num4;
        node.Bottom = !node.Bottom.IsNaN() ? (node.Bottom + num5) * 0.5 : num5;
      }
      if (RelativePanel.GetAlignHorizontalCenterWithPanel((DependencyObject) element))
      {
        double num6 = (this.AvailableSize.Width - desiredSize.Width) * 0.5;
        node.Left = !node.Left.IsNaN() ? (node.Left + num6) * 0.5 : num6;
        node.Right = !node.Right.IsNaN() ? (node.Right + num6) * 0.5 : num6;
      }
      if (RelativePanel.GetAlignVerticalCenterWithPanel((DependencyObject) element))
      {
        double num7 = (this.AvailableSize.Height - desiredSize.Height) * 0.5;
        node.Top = !node.Top.IsNaN() ? (node.Top + num7) * 0.5 : num7;
        node.Bottom = !node.Bottom.IsNaN() ? (node.Bottom + num7) * 0.5 : num7;
      }
      Size availableSize;
      if (node.Left.IsNaN())
      {
        if (!node.Right.IsNaN())
        {
          node.Left = this.AvailableSize.Width - node.Right - desiredSize.Width;
        }
        else
        {
          node.Left = 0.0;
          RelativePanel.GraphNode graphNode = node;
          availableSize = this.AvailableSize;
          double num8 = availableSize.Width - desiredSize.Width;
          graphNode.Right = num8;
        }
      }
      else if (!node.Left.IsNaN() && node.Right.IsNaN())
        node.Right = this.AvailableSize.Width - node.Left - desiredSize.Width;
      if (node.Top.IsNaN())
      {
        if (!node.Bottom.IsNaN())
        {
          RelativePanel.GraphNode graphNode = node;
          availableSize = this.AvailableSize;
          double num9 = availableSize.Height - node.Bottom - desiredSize.Height;
          graphNode.Top = num9;
        }
        else
        {
          node.Top = 0.0;
          RelativePanel.GraphNode graphNode = node;
          availableSize = this.AvailableSize;
          double num10 = availableSize.Height - desiredSize.Height;
          graphNode.Bottom = num10;
        }
      }
      else if (!node.Top.IsNaN() && node.Bottom.IsNaN())
      {
        RelativePanel.GraphNode graphNode = node;
        availableSize = this.AvailableSize;
        double num11 = availableSize.Height - node.Top - desiredSize.Height;
        graphNode.Bottom = num11;
      }
      node.Measured = true;
    }

    public Size GetBoundingSize(bool calcWidth, bool calcHeight)
    {
      Size boundingSize1 = new Size();
      foreach (RelativePanel.GraphNode graphNode in this._nodeDic.Values)
      {
        Size boundingSize2 = graphNode.GetBoundingSize();
        boundingSize1.Width = Math.Max(boundingSize1.Width, boundingSize2.Width);
        boundingSize1.Height = Math.Max(boundingSize1.Height, boundingSize2.Height);
      }
      ref Size local1 = ref boundingSize1;
      Size availableSize;
      double width;
      if (!calcWidth)
      {
        availableSize = this.AvailableSize;
        width = availableSize.Width;
      }
      else
        width = boundingSize1.Width;
      local1.Width = width;
      ref Size local2 = ref boundingSize1;
      double height;
      if (!calcHeight)
      {
        availableSize = this.AvailableSize;
        height = availableSize.Height;
      }
      else
        height = boundingSize1.Height;
      local2.Height = height;
      return boundingSize1;
    }
  }
}

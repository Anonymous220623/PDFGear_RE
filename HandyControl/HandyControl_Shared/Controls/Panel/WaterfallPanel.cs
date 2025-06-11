// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.WaterfallPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class WaterfallPanel : Panel
{
  public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(nameof (Groups), typeof (int), typeof (WaterfallPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int2Box, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WaterfallPanel.IsGroupsValid));
  public static readonly DependencyProperty AutoGroupProperty = DependencyProperty.Register(nameof (AutoGroup), typeof (bool), typeof (WaterfallPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty DesiredLengthProperty = DependencyProperty.Register(nameof (DesiredLength), typeof (double), typeof (WaterfallPanel), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof (WaterfallPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public int Groups
  {
    get => (int) this.GetValue(WaterfallPanel.GroupsProperty);
    set => this.SetValue(WaterfallPanel.GroupsProperty, (object) value);
  }

  private static bool IsGroupsValid(object value) => (int) value >= 1;

  public bool AutoGroup
  {
    get => (bool) this.GetValue(WaterfallPanel.AutoGroupProperty);
    set => this.SetValue(WaterfallPanel.AutoGroupProperty, ValueBoxes.BooleanBox(value));
  }

  public double DesiredLength
  {
    get => (double) this.GetValue(WaterfallPanel.DesiredLengthProperty);
    set => this.SetValue(WaterfallPanel.DesiredLengthProperty, (object) value);
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(WaterfallPanel.OrientationProperty);
    set => this.SetValue(WaterfallPanel.OrientationProperty, (object) value);
  }

  private int CaculateGroupCount(Orientation orientation, PanelUvSize size)
  {
    if (!this.AutoGroup)
      return this.Groups;
    double desiredLength = this.DesiredLength;
    return MathHelper.IsVerySmall(desiredLength) ? this.Groups : (int) (size.U / desiredLength);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    Orientation orientation = this.Orientation;
    PanelUvSize size = new PanelUvSize(orientation, finalSize);
    int length = this.CaculateGroupCount(orientation, size);
    if (length < 1)
      return finalSize;
    List<double> list = ((IEnumerable<double>) new double[length]).ToList<double>();
    double width = size.U / (double) length;
    UIElementCollection internalChildren = this.InternalChildren;
    int index1 = 0;
    for (int count = internalChildren.Count; index1 < count; ++index1)
    {
      UIElement uiElement = internalChildren[index1];
      if (uiElement != null)
      {
        int index2 = list.IndexOf(list.Min());
        double height = list[index2];
        PanelUvSize panelUvSize1 = new PanelUvSize(orientation, uiElement.DesiredSize);
        PanelUvSize panelUvSize2 = new PanelUvSize(orientation, width, panelUvSize1.V);
        PanelUvSize panelUvSize3 = new PanelUvSize(orientation, (double) index2 * width, height);
        uiElement.Arrange(new Rect(new Point(panelUvSize3.U, panelUvSize3.V), panelUvSize2.ScreenSize));
        list[index2] = height + panelUvSize1.V;
      }
    }
    return finalSize;
  }

  protected override Size MeasureOverride(Size constraint)
  {
    Orientation orientation = this.Orientation;
    PanelUvSize size = new PanelUvSize(orientation, constraint);
    int length = this.CaculateGroupCount(orientation, size);
    if (length < 1)
      return constraint;
    List<double> list = ((IEnumerable<double>) new double[length]).ToList<double>();
    double d = size.U / (double) length;
    if (double.IsNaN(d) || double.IsInfinity(d))
      return constraint;
    UIElementCollection internalChildren = this.InternalChildren;
    int index1 = 0;
    for (int count = internalChildren.Count; index1 < count; ++index1)
    {
      UIElement uiElement = internalChildren[index1];
      if (uiElement != null)
      {
        uiElement.Measure(constraint);
        PanelUvSize panelUvSize = new PanelUvSize(orientation, uiElement.DesiredSize);
        int index2 = list.IndexOf(list.Min());
        double num = list[index2];
        list[index2] = num + panelUvSize.V;
      }
    }
    size = new PanelUvSize(orientation, new Size(size.ScreenSize.Width, list.Max()));
    return size.ScreenSize;
  }
}

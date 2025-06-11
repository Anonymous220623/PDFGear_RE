// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.VirtualizingWrapPanelWithItemExpansion
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable enable
namespace WpfToolkit.Controls;

public class VirtualizingWrapPanelWithItemExpansion : VirtualizingWrapPanel
{
  public static readonly DependencyProperty ExpandedItemTemplateProperty = DependencyProperty.Register(nameof (ExpandedItemTemplate), typeof (DataTemplate), typeof (VirtualizingWrapPanelWithItemExpansion), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty ExpandedItemProperty = DependencyProperty.Register(nameof (ExpandedItem), typeof (object), typeof (VirtualizingWrapPanelWithItemExpansion), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure, (PropertyChangedCallback) ((o, a) => ((VirtualizingWrapPanelWithItemExpansion) o).ExpandedItemPropertyChanged(a))));
  private FrameworkElement? expandedItemChild;
  private int itemIndexFollwingExpansion;

  public DataTemplate? ExpandedItemTemplate
  {
    get
    {
      return (DataTemplate) this.GetValue(VirtualizingWrapPanelWithItemExpansion.ExpandedItemTemplateProperty);
    }
    set
    {
      this.SetValue(VirtualizingWrapPanelWithItemExpansion.ExpandedItemTemplateProperty, (object) value);
    }
  }

  public object? ExpandedItem
  {
    get => this.GetValue(VirtualizingWrapPanelWithItemExpansion.ExpandedItemProperty);
    set => this.SetValue(VirtualizingWrapPanelWithItemExpansion.ExpandedItemProperty, value);
  }

  private int ExpandedItemIndex => this.Items.IndexOf(this.ExpandedItem);

  private void ExpandedItemPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue == null)
      return;
    int index = this.InternalChildren.IndexOf((UIElement) this.expandedItemChild);
    if (index == -1)
      return;
    this.expandedItemChild = (FrameworkElement) null;
    this.RemoveInternalChildRange(index, 1);
  }

  protected override Size CalculateExtent(Size availableSize)
  {
    Size extent = base.CalculateExtent(availableSize);
    if (this.expandedItemChild != null)
    {
      if (this.Orientation == Orientation.Vertical)
        extent.Height += this.expandedItemChild.DesiredSize.Height;
      else
        extent.Width += this.expandedItemChild.DesiredSize.Width;
    }
    return extent;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double num1 = 0.0;
    Size childArrangeSize = this.CalculateChildArrangeSize(finalSize);
    double innerSpacing;
    double outerSpacing;
    this.CalculateSpacing(finalSize, out innerSpacing, out outerSpacing);
    for (int index = 0; index < this.InternalChildren.Count; ++index)
    {
      UIElement internalChild = this.InternalChildren[index];
      if (internalChild == this.expandedItemChild)
      {
        int num2 = this.ExpandedItemIndex / this.itemsPerRowCount + 1;
        double num3 = outerSpacing;
        double num4 = (double) num2 * this.GetHeight(childArrangeSize);
        double num5 = this.GetWidth(finalSize) - 2.0 * outerSpacing;
        double height = this.GetHeight(this.expandedItemChild.DesiredSize);
        if (!this.IsSpacingEnabled || this.SpacingMode == SpacingMode.None)
          num5 = (double) this.itemsPerRowCount * this.GetWidth(childArrangeSize);
        if (this.Orientation == Orientation.Vertical)
          this.expandedItemChild.Arrange(this.CreateRect(num3 - this.GetX(this.Offset), num4 - this.GetY(this.Offset), num5, height));
        else
          this.expandedItemChild.Arrange(this.CreateRect(num3 - this.GetX(this.Offset), num4 - this.GetY(this.Offset), height, num5));
        num1 = height;
      }
      else
      {
        int indexFromChildIndex = this.GetItemIndexFromChildIndex(index);
        int num6 = indexFromChildIndex % this.itemsPerRowCount;
        int num7 = indexFromChildIndex / this.itemsPerRowCount;
        double num8 = outerSpacing + (double) num6 * (this.GetWidth(childArrangeSize) + innerSpacing);
        double num9 = (double) num7 * this.GetHeight(childArrangeSize) + num1;
        internalChild.Arrange(this.CreateRect(num8 - this.GetX(this.Offset), num9 - this.GetY(this.Offset), childArrangeSize.Width, childArrangeSize.Height));
      }
    }
    return finalSize;
  }

  protected override void RealizeItems()
  {
    GeneratorPosition position = this.ItemContainerGenerator.GeneratorPositionFromIndex(this.ItemRange.StartIndex);
    int index1 = position.Offset == 0 ? position.Index : position.Index + 1;
    int index2 = this.Items.IndexOf(this.ExpandedItem);
    int num = Math.Min(index2 != -1 ? (index2 / this.itemsPerRowCount + 1) * this.itemsPerRowCount - 1 : -1, this.Items.Count - 1);
    if (num != this.itemIndexFollwingExpansion && this.expandedItemChild != null)
    {
      this.RemoveInternalChildRange(this.InternalChildren.IndexOf((UIElement) this.expandedItemChild), 1);
      this.expandedItemChild = (FrameworkElement) null;
    }
    using (this.ItemContainerGenerator.StartAt(position, GeneratorDirection.Forward, true))
    {
      int startIndex = this.ItemRange.StartIndex;
      while (startIndex <= this.ItemRange.EndIndex)
      {
        bool isNewlyRealized;
        FrameworkElement next = (FrameworkElement) this.ItemContainerGenerator.GenerateNext(out isNewlyRealized);
        if (isNewlyRealized || !this.InternalChildren.Contains((UIElement) next))
        {
          if (index1 >= this.InternalChildren.Count)
            this.AddInternalChild((UIElement) next);
          else
            this.InsertInternalChild(index1, (UIElement) next);
          this.ItemContainerGenerator.PrepareItemContainer((DependencyObject) next);
          if (this.ItemSize == Size.Empty)
            next.Measure(this.CreateSize(this.GetWidth(this.Viewport), double.MaxValue));
          else
            next.Measure(this.ItemSize);
        }
        if (startIndex == num && this.ExpandedItemTemplate != null)
        {
          if (this.expandedItemChild == null)
          {
            this.expandedItemChild = (FrameworkElement) this.ExpandedItemTemplate.LoadContent();
            this.expandedItemChild.DataContext = this.Items[index2];
            this.expandedItemChild.Measure(this.CreateSize(this.GetWidth(this.Viewport), double.MaxValue));
          }
          if (!this.InternalChildren.Contains((UIElement) this.expandedItemChild))
          {
            ++index1;
            if (index1 >= this.InternalChildren.Count)
              this.AddInternalChild((UIElement) this.expandedItemChild);
            else
              this.InsertInternalChild(index1, (UIElement) this.expandedItemChild);
          }
        }
        ++startIndex;
        ++index1;
      }
      this.itemIndexFollwingExpansion = num;
    }
  }

  protected override void OnClearChildren()
  {
    base.OnClearChildren();
    this.expandedItemChild = (FrameworkElement) null;
  }

  protected override GeneratorPosition GetGeneratorPositionFromChildIndex(int childIndex)
  {
    int num = this.InternalChildren.IndexOf((UIElement) this.expandedItemChild);
    return num != -1 && childIndex > num ? new GeneratorPosition(childIndex - 1, 0) : new GeneratorPosition(childIndex, 0);
  }

  protected override void VirtualizeItems()
  {
    for (int index = this.InternalChildren.Count - 1; index >= 0; --index)
    {
      FrameworkElement internalChild = (FrameworkElement) this.InternalChildren[index];
      if (internalChild == this.expandedItemChild)
      {
        if (!this.ItemRange.Contains(this.ExpandedItemIndex))
        {
          this.expandedItemChild = (FrameworkElement) null;
          this.RemoveInternalChildRange(index, 1);
        }
      }
      else
      {
        int itemIndex = this.Items.IndexOf(internalChild.DataContext);
        GeneratorPosition position = this.ItemContainerGenerator.GeneratorPositionFromIndex(itemIndex);
        if (!this.ItemRange.Contains(itemIndex))
        {
          if (this.IsRecycling)
            this.ItemContainerGenerator.Recycle(position, 1);
          else
            this.ItemContainerGenerator.Remove(position, 1);
          this.RemoveInternalChildRange(index, 1);
        }
      }
    }
  }

  protected override void BringIndexIntoView(int index)
  {
    double offset = (double) (index / this.itemsPerRowCount) * this.GetHeight(this.childSize);
    if (this.expandedItemChild != null && index > this.itemIndexFollwingExpansion)
      offset += this.GetHeight(this.expandedItemChild.DesiredSize);
    if (this.Orientation == Orientation.Horizontal)
      this.SetHorizontalOffset(offset);
    else
      this.SetVerticalOffset(offset);
  }
}

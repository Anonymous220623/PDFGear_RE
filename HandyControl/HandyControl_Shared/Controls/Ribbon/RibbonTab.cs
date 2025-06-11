// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RibbonTab
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_RootContainer", Type = typeof (UIElement))]
public class RibbonTab : HeaderedItemsControl
{
  private const string RootContainer = "PART_RootContainer";
  private UIElement _rootContainer;
  public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof (RibbonTab), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RibbonTab.OnIsSelectedChanged)));

  public Ribbon Ribbon => Ribbon.GetRibbon((DependencyObject) this);

  internal RibbonTabHeader RibbonTabHeader
  {
    get
    {
      Ribbon ribbon = this.Ribbon;
      if (ribbon == null)
        return (RibbonTabHeader) null;
      int index = ribbon.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
      return index < 0 ? (RibbonTabHeader) null : ribbon.RibbonTabHeaderItemsControl?.ItemContainerGenerator.ContainerFromIndex(index) as RibbonTabHeader;
    }
  }

  static RibbonTab()
  {
    UIElement.VisibilityProperty.OverrideMetadata(typeof (RibbonTab), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(RibbonTab.OnVisibilityChanged)));
  }

  private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    RibbonTab source = (RibbonTab) d;
    if (source.IsSelected)
    {
      if (source.Ribbon.IsDropDownOpen)
        source.SwitchContentVisibility(true);
      source.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, (object) source));
    }
    else
    {
      source.SwitchContentVisibility(false);
      source.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, (object) source));
    }
    source.RibbonTabHeader?.CoerceValue(RibbonTabHeader.IsSelectedProperty);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(RibbonTab.IsSelectedProperty);
    set => this.SetValue(RibbonTab.IsSelectedProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._rootContainer = this.GetTemplateChild("PART_RootContainer") as UIElement;
    this.SwitchContentVisibility(this.IsSelected);
  }

  internal void SwitchContentVisibility(bool isVisible)
  {
    UIElement rootContainer = this._rootContainer;
    if (rootContainer == null)
      return;
    rootContainer.Show(isVisible);
  }

  protected virtual void OnSelected(RoutedEventArgs e) => this.RaiseEvent(e);

  protected virtual void OnUnselected(RoutedEventArgs e) => this.RaiseEvent(e);

  protected override void OnHeaderChanged(object oldHeader, object newHeader)
  {
    base.OnHeaderChanged(oldHeader, newHeader);
    this.Ribbon?.NotifyTabHeaderChanged();
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new RibbonGroup();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonGroup;

  private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    RibbonTab ribbonTab = (RibbonTab) d;
    ribbonTab.RibbonTabHeader?.CoerceValue(UIElement.VisibilityProperty);
    Ribbon ribbon = ribbonTab.Ribbon;
    if (ribbon == null || !ribbonTab.IsSelected || (Visibility) e.OldValue != Visibility.Visible || (Visibility) e.NewValue == Visibility.Visible)
      return;
    ribbon.ResetSelection();
  }
}

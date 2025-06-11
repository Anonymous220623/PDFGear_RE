// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RibbonTabHeader
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class RibbonTabHeader : ContentControl
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (RibbonTabHeader), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(RibbonTabHeader.OnIsSelectedChanged), new CoerceValueCallback(RibbonTabHeader.CoerceIsSelected)));

  public Ribbon Ribbon => Ribbon.GetRibbon((DependencyObject) this);

  internal RibbonTab RibbonTab
  {
    get
    {
      ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer((DependencyObject) this);
      Ribbon ribbon = this.Ribbon;
      if (itemsControl == null || ribbon == null)
        return (RibbonTab) null;
      int index = itemsControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
      return ribbon.ItemContainerGenerator.ContainerFromIndex(index) as RibbonTab;
    }
  }

  static RibbonTabHeader()
  {
    UIElement.VisibilityProperty.OverrideMetadata(typeof (RibbonTabHeader), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonTabHeader.CoerceVisibility)));
  }

  private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((RibbonTabHeader) d).OnIsSelectedChanged((bool) e.OldValue, (bool) e.NewValue);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(RibbonTabHeader.IsSelectedProperty);
    set => this.SetValue(RibbonTabHeader.IsSelectedProperty, (object) value);
  }

  internal void PrepareRibbonTabHeader()
  {
    this.CoerceValue(RibbonTabHeader.IsSelectedProperty);
    this.CoerceValue(UIElement.VisibilityProperty);
  }

  protected virtual void OnIsSelectedChanged(bool oldIsSelected, bool newIsSelected)
  {
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    Ribbon ribbon = this.Ribbon;
    if (ribbon != null)
    {
      ribbon.NotifyMouseClickedOnTabHeader(this, e);
      e.Handled = true;
    }
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    base.OnGotKeyboardFocus(e);
    RibbonTab ribbonTab = this.RibbonTab;
    if (ribbonTab == null)
      return;
    ribbonTab.IsSelected = true;
  }

  private static object CoerceIsSelected(DependencyObject d, object basevalue)
  {
    RibbonTab ribbonTab = ((RibbonTabHeader) d).RibbonTab;
    return ribbonTab == null ? basevalue : ValueBoxes.BooleanBox(ribbonTab.IsSelected);
  }

  private static object CoerceVisibility(DependencyObject d, object basevalue)
  {
    RibbonTab ribbonTab = ((RibbonTabHeader) d).RibbonTab;
    return ribbonTab == null ? basevalue : ValueBoxes.VisibilityBox(ribbonTab.Visibility);
  }
}

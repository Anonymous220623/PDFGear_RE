// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SideMenu
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class SideMenu : HeaderedSimpleItemsControl
{
  private SideMenuItem _selectedItem;
  private SideMenuItem _selectedHeader;
  private bool _isItemSelected;
  public static readonly DependencyProperty AutoSelectProperty = DependencyProperty.Register(nameof (AutoSelect), typeof (bool), typeof (SideMenu), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty ExpandModeProperty = DependencyProperty.Register(nameof (ExpandMode), typeof (ExpandMode), typeof (SideMenu), new PropertyMetadata((object) ExpandMode.ShowOne, new PropertyChangedCallback(SideMenu.OnExpandModeChanged)));
  public static readonly DependencyProperty PanelAreaLengthProperty = DependencyProperty.Register(nameof (PanelAreaLength), typeof (double), typeof (SideMenu), new PropertyMetadata((object) double.NaN));
  public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<object>>), typeof (SideMenu));

  public SideMenu()
  {
    this.AddHandler(SideMenuItem.SelectedEvent, (Delegate) new RoutedEventHandler(this.SideMenuItemSelected));
    this.Loaded += (RoutedEventHandler) ((s, e) => this.Init());
  }

  protected override void Refresh()
  {
    base.Refresh();
    this.Init();
  }

  private void Init()
  {
    if (this.ItemsHost == null)
      return;
    this.OnExpandModeChanged(this.ExpandMode);
  }

  private void SideMenuItemSelected(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is SideMenuItem originalSource))
      return;
    if (originalSource.Role == SideMenuItemRole.Item)
    {
      this._isItemSelected = true;
      if (object.Equals((object) originalSource, (object) this._selectedItem))
        return;
      if (this._selectedItem != null)
        this._selectedItem.IsSelected = false;
      this._selectedItem = originalSource;
      this._selectedItem.IsSelected = true;
      this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<object>(SideMenu.SelectionChangedEvent, (object) this)
      {
        Info = e.OriginalSource
      });
    }
    else
    {
      if (!object.Equals((object) originalSource, (object) this._selectedHeader))
      {
        if (this._selectedHeader != null)
        {
          if (this.ExpandMode == ExpandMode.Freedom && originalSource.ItemsHost.IsVisible && !this._isItemSelected)
          {
            originalSource.IsSelected = false;
            this.SwitchPanelArea(originalSource);
            return;
          }
          this._selectedHeader.IsSelected = false;
          if (this.ExpandMode != ExpandMode.Freedom)
            this.SwitchPanelArea(this._selectedHeader);
        }
        this._selectedHeader = originalSource;
        this._selectedHeader.IsSelected = true;
        this.SwitchPanelArea(this._selectedHeader);
      }
      else if (this.ExpandMode == ExpandMode.Freedom && !this._isItemSelected)
      {
        this._selectedHeader.IsSelected = false;
        this.SwitchPanelArea(this._selectedHeader);
        this._selectedHeader = (SideMenuItem) null;
      }
      if (this._isItemSelected)
        this._isItemSelected = false;
      else if (this._selectedHeader != null)
      {
        if (this.AutoSelect)
        {
          if (this._selectedItem != null)
          {
            this._selectedItem.IsSelected = false;
            this._selectedItem = (SideMenuItem) null;
          }
          this._selectedHeader.SelectDefaultItem();
        }
        this._isItemSelected = false;
      }
      if (originalSource.HasItems)
        return;
      this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<object>(SideMenu.SelectionChangedEvent, (object) this)
      {
        Info = e.OriginalSource
      });
    }
  }

  private void SwitchPanelArea(SideMenuItem oldItem)
  {
    switch (this.ExpandMode)
    {
      case ExpandMode.ShowOne:
      case ExpandMode.Accordion:
      case ExpandMode.Freedom:
        oldItem.SwitchPanelArea(oldItem.IsSelected);
        break;
    }
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new SideMenuItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

  public bool AutoSelect
  {
    get => (bool) this.GetValue(SideMenu.AutoSelectProperty);
    set => this.SetValue(SideMenu.AutoSelectProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnExpandModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SideMenu sideMenu = (SideMenu) d;
    ExpandMode newValue = (ExpandMode) e.NewValue;
    if (sideMenu.ItemsHost == null)
      return;
    sideMenu.OnExpandModeChanged(newValue);
  }

  private void OnExpandModeChanged(ExpandMode mode)
  {
    switch (mode)
    {
      case ExpandMode.ShowOne:
        SideMenuItem sideMenuItem1 = (SideMenuItem) null;
        using (IEnumerator<SideMenuItem> enumerator = this.ItemsHost.Children.OfType<SideMenuItem>().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            SideMenuItem current = enumerator.Current;
            if (sideMenuItem1 != null)
            {
              current.IsSelected = false;
              if (current.ItemsHost != null)
              {
                foreach (SideMenuItem sideMenuItem2 in current.ItemsHost.Children.OfType<SideMenuItem>())
                  sideMenuItem2.IsSelected = false;
              }
            }
            else if (current.IsSelected)
            {
              switch (current.Role)
              {
                case SideMenuItemRole.Header:
                  this._selectedHeader = current;
                  break;
                case SideMenuItemRole.Item:
                  this._selectedItem = current;
                  break;
              }
              this.ShowSelectedOne(current);
              sideMenuItem1 = current;
              if (current.ItemsHost != null)
              {
                foreach (SideMenuItem sideMenuItem3 in current.ItemsHost.Children.OfType<SideMenuItem>())
                {
                  if (this._selectedItem != null)
                    sideMenuItem3.IsSelected = false;
                  else if (sideMenuItem3.IsSelected)
                    this._selectedItem = sideMenuItem3;
                }
              }
            }
          }
          break;
        }
      case ExpandMode.ShowAll:
        this.ShowAll();
        break;
    }
  }

  public ExpandMode ExpandMode
  {
    get => (ExpandMode) this.GetValue(SideMenu.ExpandModeProperty);
    set => this.SetValue(SideMenu.ExpandModeProperty, (object) value);
  }

  public double PanelAreaLength
  {
    get => (double) this.GetValue(SideMenu.PanelAreaLengthProperty);
    set => this.SetValue(SideMenu.PanelAreaLengthProperty, (object) value);
  }

  private void ShowAll()
  {
    foreach (SideMenuItem sideMenuItem in this.ItemsHost.Children.OfType<SideMenuItem>())
      sideMenuItem.SwitchPanelArea(true);
  }

  private void ShowSelectedOne(SideMenuItem item)
  {
    foreach (SideMenuItem objA in this.ItemsHost.Children.OfType<SideMenuItem>())
      objA.SwitchPanelArea(object.Equals((object) objA, (object) item));
  }

  public event EventHandler<FunctionEventArgs<object>> SelectionChanged
  {
    add => this.AddHandler(SideMenu.SelectionChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(SideMenu.SelectionChangedEvent, (Delegate) value);
  }
}

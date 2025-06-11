// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyExtensions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public static class HotKeyExtensions
{
  private static HotKeyExtensions.UIElementManager elementManager = new HotKeyExtensions.UIElementManager();
  private static Action<ButtonBase> buttonBaseOnClick;
  private static bool isButtonBaseOnClickSupported = true;
  public static readonly DependencyProperty InvokeWhenProperty = DependencyProperty.RegisterAttached("InvokeWhen", typeof (string), typeof (HotKeyExtensions), new PropertyMetadata((object) "", new PropertyChangedCallback(HotKeyExtensions.OnInvokeWhenPropertyChanged)));
  public static readonly DependencyProperty InvokeActionProperty = DependencyProperty.RegisterAttached("InvokeAction", typeof (HotKeyInvokeAction), typeof (HotKeyExtensions), new PropertyMetadata((object) HotKeyInvokeAction.Invoke));
  public static readonly DependencyProperty IsHotKeyDisabledScopeProperty = DependencyProperty.RegisterAttached("IsHotKeyDisabledScope", typeof (bool), typeof (HotKeyExtensions), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty InvokeOnHandledProperty = DependencyProperty.RegisterAttached("InvokeOnHandled", typeof (bool), typeof (HotKeyExtensions), new PropertyMetadata((object) false));

  public static string GetInvokeWhen(UIElement obj)
  {
    return (string) obj.GetValue(HotKeyExtensions.InvokeWhenProperty);
  }

  public static void SetInvokeWhen(UIElement obj, string value)
  {
    obj.SetValue(HotKeyExtensions.InvokeWhenProperty, (object) value);
  }

  public static HotKeyInvokeAction GetInvokeAction(UIElement obj)
  {
    return (HotKeyInvokeAction) obj.GetValue(HotKeyExtensions.InvokeActionProperty);
  }

  public static void SetInvokeAction(UIElement obj, HotKeyInvokeAction value)
  {
    obj.SetValue(HotKeyExtensions.InvokeActionProperty, (object) value);
  }

  public static bool GetIsHotKeyDisabledScope(UIElement obj)
  {
    return (bool) obj.GetValue(HotKeyExtensions.IsHotKeyDisabledScopeProperty);
  }

  public static void SetIsHotKeyDisabledScope(UIElement obj, bool value)
  {
    obj.SetValue(HotKeyExtensions.IsHotKeyDisabledScopeProperty, (object) value);
  }

  public static bool GetInvokeOnHandled(DependencyObject obj)
  {
    return (bool) obj.GetValue(HotKeyExtensions.InvokeOnHandledProperty);
  }

  public static void SetInvokeOnHandled(DependencyObject obj, bool value)
  {
    obj.SetValue(HotKeyExtensions.InvokeOnHandledProperty, (object) value);
  }

  private static void OnInvokeWhenPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ToolTipExtensions.UpdateDefaultToolTip(d);
    if (!(d is UIElement element) || object.Equals(e.NewValue, e.OldValue))
      return;
    string[] strArray1 = HotKeyListener.SplitKeyNameGroup((string) e.OldValue);
    string[] strArray2 = HotKeyListener.SplitKeyNameGroup((string) e.NewValue);
    for (int index1 = 0; index1 < strArray2.Length; ++index1)
    {
      for (int index2 = 0; index2 < strArray1.Length; ++index2)
      {
        if (strArray2[index1] == strArray1[index2])
        {
          strArray2[index1] = "";
          strArray1[index2] = "";
        }
      }
    }
    for (int index = 0; index < strArray1.Length; ++index)
      HotKeyExtensions.elementManager.Remove(strArray1[index], element);
    for (int index = 0; index < strArray2.Length; ++index)
      HotKeyExtensions.elementManager.Add(strArray2[index], element);
    HotKeyListener.HotKeyInvoked -= new EventHandler<HotKeyInvokedEventArgs>(HotKeyExtensions.HotKeyListener_HotKeyInvoked);
    if (HotKeyExtensions.elementManager.IsEmpty())
      return;
    HotKeyListener.HotKeyInvoked += new EventHandler<HotKeyInvokedEventArgs>(HotKeyExtensions.HotKeyListener_HotKeyInvoked);
  }

  private static void HotKeyListener_HotKeyInvoked(object sender, HotKeyInvokedEventArgs e)
  {
    if (Keyboard.FocusedElement is UIElement focusedElement && HotKeyExtensions.GetIsHotKeyDisabledScope(focusedElement) || !HotKeyManager.IsHotKeyModelEnabled(e.HotKeyName, e.IsRepeat))
      return;
    IReadOnlyList<UIElement> uiElements = HotKeyExtensions.elementManager.GetUIElements(e.HotKeyName);
    for (int index = 0; index < uiElements.Count; ++index)
    {
      if (uiElements[index].IsEnabled && HotKeyExtensions.IsWindowEnabled(uiElements[index]) && (HotKeyExtensions.GetInvokeOnHandled((DependencyObject) uiElements[index]) || !e.Handled))
      {
        UIElement child = uiElements[index];
        if (HotKeyExtensions.HasHotKeyName(child, e.HotKeyName))
        {
          do
          {
            switch (HotKeyExtensions.GetInvokeAction(uiElements[index]))
            {
              case HotKeyInvokeAction.None:
                continue;
              case HotKeyInvokeAction.Toggle:
                e.Handled = HotKeyExtensions.TryToggle(child, e.HotKeyName);
                goto case HotKeyInvokeAction.None;
              case HotKeyInvokeAction.ToggleOn:
                e.Handled = HotKeyExtensions.TryToggleTo(child, e.HotKeyName, true);
                goto case HotKeyInvokeAction.None;
              case HotKeyInvokeAction.ToggleOff:
                e.Handled = HotKeyExtensions.TryToggleTo(child, e.HotKeyName, false);
                goto case HotKeyInvokeAction.None;
              default:
                e.Handled = HotKeyExtensions.TryInvoke(child, e.HotKeyName);
                goto case HotKeyInvokeAction.None;
            }
          }
          while (!e.Handled && HotKeyExtensions.TryUnwrapControl(child, out child));
        }
      }
    }
  }

  private static string[] GetInvokeNames(UIElement control)
  {
    return control == null ? Array.Empty<string>() : HotKeyListener.SplitKeyNameGroup(HotKeyExtensions.GetInvokeWhen(control));
  }

  private static bool HasHotKeyName(UIElement control, string hotKeyName)
  {
    return control != null && !string.IsNullOrEmpty(hotKeyName) && ((IEnumerable<string>) HotKeyExtensions.GetInvokeNames(control)).Contains<string>(hotKeyName);
  }

  private static bool TryUnwrapControl(UIElement control, out UIElement child)
  {
    child = (UIElement) null;
    switch (control)
    {
      case ContentPresenter reference1:
        if (VisualTreeHelper.GetChildrenCount((DependencyObject) reference1) == 0)
          reference1.ApplyTemplate();
        if (reference1.Content is UIElement content1)
        {
          child = content1;
          return true;
        }
        if (VisualTreeHelper.GetChildrenCount((DependencyObject) reference1) > 0 && VisualTreeHelper.GetChild((DependencyObject) reference1, 0) is UIElement child1)
        {
          child = child1;
          return true;
        }
        break;
      case ContentControl reference2:
        if (VisualTreeHelper.GetChildrenCount((DependencyObject) reference2) == 0)
          reference2.ApplyTemplate();
        if (reference2.Content is UIElement content2)
        {
          child = content2;
          return true;
        }
        break;
    }
    return false;
  }

  private static bool TryInvoke(UIElement control, string hotKeyName)
  {
    if ((UIElementAutomationPeer.FromElement(control) ?? UIElementAutomationPeer.CreatePeerForElement(control))?.GetPattern(PatternInterface.Invoke) is IInvokeProvider pattern)
    {
      pattern.Invoke();
      return true;
    }
    return HotKeyExtensions.TryRaiseClickEvent(control as ButtonBase);
  }

  private static bool TryRaiseClickEvent(ButtonBase buttonBase)
  {
    if (buttonBase == null)
      return false;
    if (HotKeyExtensions.isButtonBaseOnClickSupported)
    {
      if (HotKeyExtensions.buttonBaseOnClick == null)
      {
        try
        {
          MethodInfo method = typeof (ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);
          if (method != (MethodInfo) null)
            HotKeyExtensions.buttonBaseOnClick = ((Expression<Action<ButtonBase>>) (c => System.Linq.Expressions.Expression.Call(c, method))).Compile();
        }
        catch
        {
        }
        if (HotKeyExtensions.buttonBaseOnClick == null)
          HotKeyExtensions.isButtonBaseOnClickSupported = false;
      }
    }
    if (!HotKeyExtensions.isButtonBaseOnClickSupported || HotKeyExtensions.buttonBaseOnClick == null)
      return false;
    HotKeyExtensions.buttonBaseOnClick(buttonBase);
    return true;
  }

  private static bool TryToggleTo(UIElement control, string hotKeyName, bool state)
  {
    AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(control) ?? UIElementAutomationPeer.CreatePeerForElement(control);
    if (!(automationPeer?.GetPattern(PatternInterface.Toggle) is IToggleProvider toggleProvider1))
      toggleProvider1 = automationPeer as IToggleProvider;
    IToggleProvider toggleProvider2 = toggleProvider1;
    if (toggleProvider2 != null)
    {
      for (int index = 0; index < 2; ++index)
      {
        ToggleState toggleState = state ? ToggleState.On : ToggleState.Off;
        if (toggleProvider2.ToggleState != toggleState)
        {
          toggleProvider2.Toggle();
          if (toggleProvider2.ToggleState == toggleState)
            return true;
        }
      }
    }
    return false;
  }

  private static bool TryToggle(UIElement control, string hotKeyName)
  {
    AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(control) ?? UIElementAutomationPeer.CreatePeerForElement(control);
    if (!(automationPeer?.GetPattern(PatternInterface.Toggle) is IToggleProvider toggleProvider1))
      toggleProvider1 = automationPeer as IToggleProvider;
    IToggleProvider toggleProvider2 = toggleProvider1;
    if (toggleProvider2 == null)
      return false;
    toggleProvider2.Toggle();
    return true;
  }

  public static bool IsWindowEnabled(UIElement control)
  {
    switch (control)
    {
      case null:
        return false;
      case ContextMenu _:
      case MenuItem _:
        return true;
      default:
        Window window = Window.GetWindow((DependencyObject) control);
        return window != null && PresentationSource.FromVisual((Visual) window) is HwndSource hwndSource && window.IsVisible && NativeMethods.IsWindowEnabled(hwndSource.Handle);
    }
  }

  private class UIElementManager
  {
    private Dictionary<string, WeakCollection<UIElement>> names = new Dictionary<string, WeakCollection<UIElement>>();

    public void Add(string name, UIElement element)
    {
      lock (this.names)
        this.GetCollection(name).Add(element);
    }

    public void Remove(string name, UIElement element)
    {
      lock (this.names)
      {
        ICollection<UIElement> collection = this.GetCollection(name, false);
        if (collection == null)
          return;
        collection.Remove(element);
        if (collection.Count != 0)
          return;
        this.names.Remove(name);
      }
    }

    public IReadOnlyList<UIElement> GetUIElements(string name)
    {
      lock (this.names)
      {
        ICollection<UIElement> collection = this.GetCollection(name, false);
        return collection == null ? (IReadOnlyList<UIElement>) Array.Empty<UIElement>() : (IReadOnlyList<UIElement>) collection.ToArray<UIElement>();
      }
    }

    public bool IsEmpty()
    {
      lock (this.names)
      {
        foreach (string str in this.names.Keys.ToArray<string>())
        {
          ICollection<UIElement> collection = this.GetCollection(str, false);
          if (collection != null && collection.Count == 0)
            this.names.Remove(str);
        }
        return this.names.Count == 0;
      }
    }

    private ICollection<UIElement> GetCollection(string name, bool createNew = true)
    {
      lock (this.names)
      {
        WeakCollection<UIElement> collection;
        if (!this.names.TryGetValue(name, out collection) & createNew)
          this.names[name] = collection = new WeakCollection<UIElement>();
        return (ICollection<UIElement>) collection;
      }
    }
  }
}

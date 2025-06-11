// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ToolTipExtensions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.HotKeys;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Controls;

public class ToolTipExtensions
{
  private static readonly RoutedEvent EmptyEvent = EventManager.RegisterRoutedEvent(nameof (EmptyEvent), RoutingStrategy.Direct, typeof (RoutedEventArgs), typeof (ToolTipExtensions));
  [ThreadStatic]
  private static ToolTip defaultToolTip;
  public static readonly DependencyProperty IsDefaultToolTipHitTestableProperty = DependencyProperty.RegisterAttached("IsDefaultToolTipHitTestable", typeof (bool), typeof (ToolTipExtensions), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is FrameworkElement frameworkElement2) || object.Equals(a.NewValue, a.OldValue))
      return;
    frameworkElement2.ToolTipOpening -= new ToolTipEventHandler(ToolTipExtensions.OnToolTipOpening);
    if (a.NewValue == null)
      return;
    frameworkElement2.ToolTipOpening += new ToolTipEventHandler(ToolTipExtensions.OnToolTipOpening);
  })));
  public static readonly DependencyProperty CaptionProperty = DependencyProperty.RegisterAttached("Caption", typeof (string), typeof (ToolTipExtensions), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) => ToolTipExtensions.UpdateDefaultToolTip(s))));

  private static ToolTip DefaultToolTip
  {
    get
    {
      ToolTip defaultToolTip1 = ToolTipExtensions.defaultToolTip;
      if (defaultToolTip1 != null)
        return defaultToolTip1;
      ToolTip defaultToolTip2 = new ToolTip();
      Canvas canvas = new Canvas();
      canvas.Width = 0.0;
      canvas.Height = 0.0;
      canvas.Margin = new Thickness(0.0, -4.0, 0.0, 0.0);
      defaultToolTip2.Content = (object) canvas;
      ToolTipExtensions.defaultToolTip = defaultToolTip2;
      return defaultToolTip2;
    }
  }

  public static bool GetIsDefaultToolTipHitTestable(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolTipExtensions.IsDefaultToolTipHitTestableProperty);
  }

  public static void SetIsDefaultToolTipHitTestable(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolTipExtensions.IsDefaultToolTipHitTestableProperty, (object) value);
  }

  public static string GetCaption(DependencyObject obj)
  {
    return (string) obj.GetValue(ToolTipExtensions.CaptionProperty);
  }

  public static void SetCaption(DependencyObject obj, string value)
  {
    obj.SetValue(ToolTipExtensions.CaptionProperty, (object) value);
  }

  internal static void UpdateDefaultToolTip(DependencyObject obj)
  {
    if (obj == null || obj is MenuItem)
      return;
    DependencyPropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ToolTipService.ToolTipProperty, typeof (DependencyObject));
    propertyDescriptor.RemoveValueChanged((object) obj, new EventHandler(ToolTipExtensions.OnToolTipChanged));
    propertyDescriptor.AddValueChanged((object) obj, new EventHandler(ToolTipExtensions.OnToolTipChanged));
    if (ToolTipService.GetToolTip(obj) == null)
    {
      if (string.IsNullOrEmpty(ToolTipExtensions.GetCaption(obj)) && (!(obj is UIElement uiElement) || string.IsNullOrEmpty(HotKeyExtensions.GetInvokeWhen(uiElement))))
        return;
      ToolTipService.SetToolTip(obj, (object) ToolTipExtensions.DefaultToolTip);
    }
    else
    {
      if (ToolTipExtensions.defaultToolTip == null || obj != ToolTipExtensions.defaultToolTip || !string.IsNullOrEmpty(ToolTipExtensions.GetCaption(obj)) || obj is UIElement uiElement && !string.IsNullOrEmpty(HotKeyExtensions.GetInvokeWhen(uiElement)))
        return;
      ToolTipService.SetToolTip(obj, (object) null);
    }
  }

  public static bool ShowToolTip(DependencyObject obj)
  {
    ToolTipExtensions.PopupControlServiceHelper.SetQuickShow(true);
    return ToolTipExtensions.PopupControlServiceHelper.InspectElementForToolTip(obj, ToolTipExtensions.PopupControlServiceHelper.ToolTipTrigger.Mouse);
  }

  internal static bool IsDefaultToolTipContent(object obj)
  {
    return obj != null && ToolTipExtensions.defaultToolTip != null && obj == ToolTipExtensions.defaultToolTip.Content;
  }

  private static void OnToolTipChanged(object sender, EventArgs e)
  {
    ToolTipExtensions.UpdateDefaultToolTip(sender as DependencyObject);
  }

  private static void OnToolTipOpening(object sender, ToolTipEventArgs e)
  {
    if (!(sender is DependencyObject dependencyObject))
      return;
    dependencyObject.Dispatcher.InvokeAsync((Action) (() =>
    {
      HwndSource hwndSource = PresentationSource.CurrentSources.OfType<HwndSource>().FirstOrDefault<HwndSource>((Func<HwndSource, bool>) (c => c.RootVisual is FrameworkElement rootVisual3 && rootVisual3.Parent is Popup parent3 && parent3.Child is ToolTip child2 && child2.PlacementTarget == sender));
      if (hwndSource?.RootVisual is FrameworkElement rootVisual4 && rootVisual4.Parent is Popup parent4 && parent4.Child is ToolTip)
      {
        rootVisual4.IsHitTestVisible = true;
        long windowLong = NativeMethods.GetWindowLong(hwndSource.Handle, -20);
        if ((windowLong & 32L /*0x20*/) != 0L)
          NativeMethods.SetWindowLong(hwndSource.Handle, -20, windowLong & -33L);
      }
      ToolTipExtensions.PopupControlServiceEventHook.EnsureHook();
    }), DispatcherPriority.Loaded);
  }

  private static class PopupControlServiceHelper
  {
    private static bool support = true;
    private static object popupControlService;
    private static Type popupControlServiceType;
    private static Func<object, DependencyObject, int, bool> inspectElementForToolTipFunc;
    private static Action<object, bool> setQuickShowField;

    public static object PopupControlService
    {
      get
      {
        if (!ToolTipExtensions.PopupControlServiceHelper.support)
          return (object) null;
        if (ToolTipExtensions.PopupControlServiceHelper.support && ToolTipExtensions.PopupControlServiceHelper.popupControlService == null)
        {
          PropertyInfo property = typeof (FrameworkElement).GetProperty(nameof (PopupControlService), BindingFlags.Static | BindingFlags.NonPublic);
          if (property != (PropertyInfo) null)
          {
            ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType = property.PropertyType;
            ToolTipExtensions.PopupControlServiceHelper.popupControlService = property.GetValue((object) null);
          }
        }
        if (ToolTipExtensions.PopupControlServiceHelper.popupControlService == null)
          ToolTipExtensions.PopupControlServiceHelper.support = false;
        return ToolTipExtensions.PopupControlServiceHelper.popupControlService;
      }
    }

    public static void SetQuickShow(bool quickShow)
    {
      if (ToolTipExtensions.PopupControlServiceHelper.setQuickShowField == null)
      {
        if (ToolTipExtensions.PopupControlServiceHelper.PopupControlService != null)
        {
          try
          {
            FieldInfo field = ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType.GetField("_quickShow", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != (FieldInfo) null)
            {
              ParameterExpression parameterExpression;
              ToolTipExtensions.PopupControlServiceHelper.setQuickShowField = ((Expression<Action<object, bool>>) ((p1, p2) => System.Linq.Expressions.Expression.Assign((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression, ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType), field), p2))).Compile();
            }
          }
          catch
          {
          }
        }
        if (ToolTipExtensions.PopupControlServiceHelper.setQuickShowField == null)
          ToolTipExtensions.PopupControlServiceHelper.setQuickShowField = (Action<object, bool>) ((a, b) => { });
      }
      ToolTipExtensions.PopupControlServiceHelper.setQuickShowField(ToolTipExtensions.PopupControlServiceHelper.PopupControlService, quickShow);
    }

    public static bool InspectElementForToolTip(
      DependencyObject element,
      ToolTipExtensions.PopupControlServiceHelper.ToolTipTrigger triggerAction)
    {
      if (ToolTipExtensions.PopupControlServiceHelper.inspectElementForToolTipFunc == null)
      {
        if (ToolTipExtensions.PopupControlServiceHelper.PopupControlService != null)
        {
          try
          {
            MethodInfo method1 = ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType.GetMethod(nameof (InspectElementForToolTip), BindingFlags.Instance | BindingFlags.NonPublic);
            if (method1 != (MethodInfo) null)
            {
              Type parameterType = method1.GetParameters()[1].ParameterType;
              ParameterExpression parameterExpression4 = System.Linq.Expressions.Expression.Parameter(typeof (object), "p1");
              ParameterExpression parameterExpression5 = System.Linq.Expressions.Expression.Parameter(typeof (DependencyObject), "p2");
              ParameterExpression parameterExpression6 = System.Linq.Expressions.Expression.Parameter(typeof (int), "p3");
              UnaryExpression instance = System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression4, ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType);
              UnaryExpression unaryExpression1 = System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression6, parameterType);
              MethodInfo method2 = method1;
              ParameterExpression parameterExpression7 = parameterExpression5;
              UnaryExpression unaryExpression2 = unaryExpression1;
              ToolTipExtensions.PopupControlServiceHelper.inspectElementForToolTipFunc = ((Expression<Func<object, DependencyObject, int, bool>>) ((parameterExpression1, parameterExpression2, parameterExpression3) => System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) instance, method2, (System.Linq.Expressions.Expression) parameterExpression7, (System.Linq.Expressions.Expression) unaryExpression2))).Compile();
            }
          }
          catch
          {
          }
        }
        if (ToolTipExtensions.PopupControlServiceHelper.popupControlServiceType == (Type) null)
          ToolTipExtensions.PopupControlServiceHelper.inspectElementForToolTipFunc = (Func<object, DependencyObject, int, bool>) ((a, b, c) => false);
      }
      return ToolTipExtensions.PopupControlServiceHelper.inspectElementForToolTipFunc(ToolTipExtensions.PopupControlServiceHelper.PopupControlService, element, (int) triggerAction);
    }

    public enum ToolTipTrigger
    {
      Mouse,
      KeyboardFocus,
      KeyboardShortcut,
    }
  }

  private static class PopupControlServiceEventHook
  {
    private static int popupEventRedirected;
    private static ProcessInputEventHandler originalOnPostProcessInput;

    internal static void EnsureHook()
    {
      if (Interlocked.Exchange(ref ToolTipExtensions.PopupControlServiceEventHook.popupEventRedirected, 1) != 0)
        return;
      object popupControlService = ToolTipExtensions.PopupControlServiceHelper.PopupControlService;
      if (popupControlService == null)
        return;
      MethodInfo method = popupControlService.GetType().GetMethod("OnPostProcessInput", BindingFlags.Instance | BindingFlags.NonPublic);
      if (!(method != (MethodInfo) null))
        return;
      ToolTipExtensions.PopupControlServiceEventHook.originalOnPostProcessInput = (ProcessInputEventHandler) method.CreateDelegate(typeof (ProcessInputEventHandler), popupControlService);
      if (ToolTipExtensions.PopupControlServiceEventHook.originalOnPostProcessInput == null)
        return;
      InputManager.Current.PostProcessInput -= ToolTipExtensions.PopupControlServiceEventHook.originalOnPostProcessInput;
      InputManager.Current.PostProcessInput += new ProcessInputEventHandler(ToolTipExtensions.PopupControlServiceEventHook.OnPostProcessInput);
    }

    private static void OnPostProcessInput(object sender, ProcessInputEventArgs e)
    {
      bool flag = false;
      InputEventArgs input = e.StagingItem.Input;
      if (input.RoutedEvent == Mouse.MouseDownEvent || input.RoutedEvent == Mouse.MouseUpEvent)
        flag = true;
      else if (input.RoutedEvent.Name == "InputReport" && (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed))
        flag = true;
      FrameworkElement _popupRoot;
      if (flag && GetElements(e.StagingItem.Input.OriginalSource, out HwndSource _, out _popupRoot, out Popup _, out ToolTip _) && _popupRoot.IsHitTestVisible)
        return;
      ProcessInputEventHandler postProcessInput = ToolTipExtensions.PopupControlServiceEventHook.originalOnPostProcessInput;
      if (postProcessInput == null)
        return;
      postProcessInput(sender, e);

      static bool GetElements(
        object _originalSource,
        out HwndSource _hwndSource,
        out FrameworkElement _popupRoot,
        out Popup _popup,
        out ToolTip _toolTip)
      {
        _hwndSource = (HwndSource) null;
        _popupRoot = (FrameworkElement) null;
        _popup = (Popup) null;
        _toolTip = (ToolTip) null;
        if (!(_originalSource is DependencyObject dependencyObject) || !(PresentationSource.FromDependencyObject(dependencyObject) is HwndSource hwndSource) || !(hwndSource?.RootVisual is FrameworkElement rootVisual) || !(rootVisual.Parent is Popup parent) || !(parent.Child is ToolTip child))
          return false;
        _hwndSource = hwndSource;
        _popupRoot = rootVisual;
        _popup = parent;
        _toolTip = child;
        return true;
      }
    }
  }
}

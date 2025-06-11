// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.MouseMiddleButtonScrollExtensions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace CommomLib.Controls;

public class MouseMiddleButtonScrollExtensions
{
  public static readonly DependencyProperty ShowCursorAtStartPointProperty = DependencyProperty.RegisterAttached("ShowCursorAtStartPoint", typeof (bool), typeof (MouseMiddleButtonScrollExtensions), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ScrollViewer scrollViewer2) || object.Equals(a.NewValue, a.OldValue))
      return;
    MouseMiddleButtonScrollHelper buttonScrollHelper = (MouseMiddleButtonScrollHelper) scrollViewer2.GetValue(MouseMiddleButtonScrollExtensions.MouseMiddleButtonScrollHelperProperty);
    if (buttonScrollHelper == null)
      return;
    buttonScrollHelper.ShowCursorAtStartPoint = a.NewValue is bool newValue2 && newValue2;
  })));
  public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (MouseMiddleButtonScrollExtensions), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ScrollViewer scrollViewer4) || object.Equals(a.NewValue, a.OldValue))
      return;
    scrollViewer4.SetValue(MouseMiddleButtonScrollExtensions.MouseMiddleButtonScrollHelperProperty, (object) null);
    if (!(a.NewValue is bool newValue4) || !newValue4)
      return;
    scrollViewer4.SetValue(MouseMiddleButtonScrollExtensions.MouseMiddleButtonScrollHelperProperty, (object) new MouseMiddleButtonScrollHelper(scrollViewer4)
    {
      ShowCursorAtStartPoint = MouseMiddleButtonScrollExtensions.GetShowCursorAtStartPoint(scrollViewer4)
    });
  })));
  private static readonly DependencyProperty MouseMiddleButtonScrollHelperProperty = DependencyProperty.RegisterAttached("MouseMiddleButtonScrollHelper", typeof (MouseMiddleButtonScrollHelper), typeof (MouseMiddleButtonScrollExtensions), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(a.OldValue is MouseMiddleButtonScrollHelper oldValue2))
      return;
    oldValue2.Dispose();
  })));

  public static bool GetShowCursorAtStartPoint(ScrollViewer obj)
  {
    return (bool) obj.GetValue(MouseMiddleButtonScrollExtensions.ShowCursorAtStartPointProperty);
  }

  public static void SetShowCursorAtStartPoint(ScrollViewer obj, bool value)
  {
    obj.SetValue(MouseMiddleButtonScrollExtensions.ShowCursorAtStartPointProperty, (object) value);
  }

  public static bool GetIsEnabled(ScrollViewer obj)
  {
    return (bool) obj.GetValue(MouseMiddleButtonScrollExtensions.IsEnabledProperty);
  }

  public static void SetIsEnabled(ScrollViewer obj, bool value)
  {
    obj.SetValue(MouseMiddleButtonScrollExtensions.IsEnabledProperty, (object) value);
  }

  public static bool TryEnterScrollMode(ScrollViewer scrollViewer)
  {
    MouseMiddleButtonScrollHelper buttonScrollHelper = (MouseMiddleButtonScrollHelper) scrollViewer.GetValue(MouseMiddleButtonScrollExtensions.MouseMiddleButtonScrollHelperProperty);
    return buttonScrollHelper != null && buttonScrollHelper.EnterScrollMode();
  }

  public static void ExitScrollMode(ScrollViewer scrollViewer)
  {
    ((MouseMiddleButtonScrollHelper) scrollViewer.GetValue(MouseMiddleButtonScrollExtensions.MouseMiddleButtonScrollHelperProperty))?.ExitScrollMode();
  }
}

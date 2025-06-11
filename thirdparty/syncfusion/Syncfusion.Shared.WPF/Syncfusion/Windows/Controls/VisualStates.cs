// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.VisualStates
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal static class VisualStates
{
  public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";
  public const string StateCalendarButtonFocused = "CalendarButtonFocused";
  public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";
  public const string StateNormal = "Normal";
  public const string StateMouseOver = "MouseOver";
  public const string StatePressed = "Pressed";
  public const string StateDisabled = "Disabled";
  public const string GroupCommon = "CommonStates";
  public const string StateUnfocused = "Unfocused";
  public const string StateFocused = "Focused";
  public const string GroupFocus = "FocusStates";
  public const string StateSelected = "Selected";
  public const string StateUnselected = "Unselected";
  public const string GroupSelection = "SelectionStates";
  public const string StateActive = "Active";
  public const string StateInactive = "Inactive";
  public const string GroupActive = "ActiveStates";
  public const string StateValid = "Valid";
  public const string StateInvalidFocused = "InvalidFocused";
  public const string StateInvalidUnfocused = "InvalidUnfocused";
  public const string GroupValidation = "ValidationStates";
  public const string StateUnwatermarked = "Unwatermarked";
  public const string StateWatermarked = "Watermarked";
  public const string GroupWatermark = "WatermarkStates";

  public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
  {
    if (control == null)
      throw new ArgumentNullException(nameof (control));
    if (stateNames == null)
      return;
    foreach (string stateName in stateNames)
    {
      if (VisualStateManager.GoToState(control, stateName, useTransitions))
        break;
    }
  }
}

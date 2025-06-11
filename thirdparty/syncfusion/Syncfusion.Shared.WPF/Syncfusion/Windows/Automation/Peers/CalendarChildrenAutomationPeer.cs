// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Automation.Peers.CalendarChildrenAutomationPeer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Automation.Peers;

internal sealed class CalendarChildrenAutomationPeer : AutomationPeer
{
  internal CalendarChildrenAutomationPeer(
    DateTime date,
    Syncfusion.Windows.Controls.Calendar owningCalendar,
    Syncfusion.Windows.Controls.CalendarMode displayMode)
  {
    this.Date = date;
    this.DisplayMode = displayMode;
    this.OwningCalendar = owningCalendar;
  }

  internal Syncfusion.Windows.Controls.Calendar OwningCalendar { get; set; }

  internal DateTime Date { get; private set; }

  internal Syncfusion.Windows.Controls.CalendarMode DisplayMode { get; private set; }

  private bool IsCalendarDayButton => this.DisplayMode == Syncfusion.Windows.Controls.CalendarMode.Month;

  private Button OwningButton
  {
    get
    {
      if (this.OwningCalendar.DisplayMode != this.DisplayMode)
        return (Button) null;
      return this.IsCalendarDayButton ? (Button) this.OwningCalendar.MonthControl.GetCalendarDayButton(this.Date) : (Button) this.OwningCalendar.MonthControl.GetCalendarButton(this.Date, this.DisplayMode);
    }
  }

  private FrameworkElementAutomationPeer CalendarChildrenPeer
  {
    get
    {
      Button owningButton = this.OwningButton;
      return owningButton != null ? UIElementAutomationPeer.CreatePeerForElement((UIElement) owningButton) as FrameworkElementAutomationPeer : (FrameworkElementAutomationPeer) null;
    }
  }

  public override object GetPattern(PatternInterface patternInterface)
  {
    object pattern = (object) null;
    Button owningButton = this.OwningButton;
    switch (patternInterface)
    {
      case PatternInterface.Invoke:
      case PatternInterface.GridItem:
        if (owningButton != null)
        {
          pattern = (object) this;
          break;
        }
        break;
      case PatternInterface.SelectionItem:
        pattern = (object) this;
        break;
      case PatternInterface.TableItem:
        if (this.IsCalendarDayButton && owningButton != null)
        {
          pattern = (object) this;
          break;
        }
        break;
    }
    return pattern;
  }

  protected override string GetAcceleratorKeyCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetAcceleratorKey() : string.Empty;
  }

  protected override string GetAccessKeyCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetAccessKey() : string.Empty;
  }

  protected override AutomationControlType GetAutomationControlTypeCore()
  {
    return AutomationControlType.Button;
  }

  protected override string GetAutomationIdCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetAutomationId() : string.Empty;
  }

  protected override Rect GetBoundingRectangleCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetBoundingRectangle() : new Rect();
  }

  protected override List<AutomationPeer> GetChildrenCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetChildren() : (List<AutomationPeer>) null;
  }

  protected override string GetClassNameCore()
  {
    if (this.CalendarChildrenPeer != null)
      return this.CalendarChildrenPeer.GetClassName();
    return !this.IsCalendarDayButton ? "CalendarButton" : "CalendarDayButton";
  }

  protected override Point GetClickablePointCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetClickablePoint() : new Point(double.NaN, double.NaN);
  }

  protected override string GetHelpTextCore()
  {
    string longDateString = Syncfusion.Windows.Controls.DateTimeHelper.ToLongDateString(new DateTime?(this.Date), Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendar));
    return this.IsCalendarDayButton && this.OwningCalendar.BlackoutDates.Contains(this.Date) ? string.Format((IFormatProvider) Syncfusion.Windows.Controls.DateTimeHelper.GetCurrentDateFormat((object) this.OwningCalendar.Culture), Syncfusion.Windows.Controls.SR.Get(Syncfusion.Windows.Controls.SRID.CalendarAutomationPeer_BlackoutDayHelpText), (object) longDateString) : longDateString;
  }

  protected override string GetItemStatusCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetItemStatus() : string.Empty;
  }

  protected override string GetItemTypeCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetItemType() : string.Empty;
  }

  protected override AutomationPeer GetLabeledByCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetLabeledBy() : (AutomationPeer) null;
  }

  protected override bool HasKeyboardFocusCore()
  {
    return this.CalendarChildrenPeer != null && this.CalendarChildrenPeer.HasKeyboardFocus();
  }

  protected override bool IsContentElementCore()
  {
    return this.CalendarChildrenPeer == null || this.CalendarChildrenPeer.IsContentElement();
  }

  protected override bool IsControlElementCore()
  {
    return this.CalendarChildrenPeer == null || this.CalendarChildrenPeer.IsControlElement();
  }

  protected override bool IsEnabledCore()
  {
    return this.CalendarChildrenPeer != null && this.CalendarChildrenPeer.IsEnabled();
  }

  protected override bool IsKeyboardFocusableCore()
  {
    return this.CalendarChildrenPeer != null && this.CalendarChildrenPeer.IsKeyboardFocusable();
  }

  protected override bool IsOffscreenCore()
  {
    return this.CalendarChildrenPeer == null || this.CalendarChildrenPeer.IsOffscreen();
  }

  protected override bool IsPasswordCore()
  {
    return this.CalendarChildrenPeer != null && this.CalendarChildrenPeer.IsPassword();
  }

  protected override bool IsRequiredForFormCore()
  {
    return this.CalendarChildrenPeer != null && this.CalendarChildrenPeer.IsRequiredForForm();
  }

  protected override void SetFocusCore()
  {
    if (this.CalendarChildrenPeer == null)
      return;
    this.CalendarChildrenPeer.SetFocus();
  }

  protected override AutomationOrientation GetOrientationCore()
  {
    return this.CalendarChildrenPeer != null ? this.CalendarChildrenPeer.GetOrientation() : AutomationOrientation.None;
  }

  protected override string GetNameCore()
  {
    string nameCore = "";
    if (this.OwningCalendar != null)
    {
      switch (this.DisplayMode)
      {
        case Syncfusion.Windows.Controls.CalendarMode.Month:
          nameCore = Syncfusion.Windows.Controls.DateTimeHelper.ToLongDateString(new DateTime?(this.Date), Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendar));
          break;
        case Syncfusion.Windows.Controls.CalendarMode.Year:
          nameCore = Syncfusion.Windows.Controls.DateTimeHelper.ToYearMonthPatternString(new DateTime?(this.Date), Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendar), this.OwningCalendar.AbbreviatedMonthNames, this.OwningCalendar.FormatCalendar);
          break;
        case Syncfusion.Windows.Controls.CalendarMode.Decade:
          nameCore = Syncfusion.Windows.Controls.DateTimeHelper.ToYearString(new DateTime?(this.Date), Syncfusion.Windows.Controls.DateTimeHelper.GetCulture((FrameworkElement) this.OwningCalendar), this.OwningCalendar.FormatCalendar);
          break;
      }
    }
    return nameCore;
  }
}

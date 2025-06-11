// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DayNameCell
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DayNameCell : ContentControl
{
  private CornerRadius mcornerRadius;

  static DayNameCell()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DayNameCell), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DayNameCell)));
    UIElement.FocusableProperty.OverrideMetadata(typeof (DayNameCell), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    Border.CornerRadiusProperty.AddOwner(typeof (DayNameCell));
  }

  public CornerRadius CornerRadius
  {
    get => this.mcornerRadius;
    set => this.mcornerRadius = value;
  }

  protected internal void UpdateCellTemplateAndSelector(
    DataTemplate template,
    DataTemplateSelector selector)
  {
    if (selector != null)
      this.ContentTemplateSelector = selector;
    else
      this.ClearValue(ContentControl.ContentTemplateSelectorProperty);
    if (template != null)
      this.ContentTemplate = template;
    else if (selector == null)
      this.ClearValue(ContentControl.ContentTemplateProperty);
    else
      this.ContentTemplate = (DataTemplate) null;
  }

  protected internal void SetStyle(Style style) => this.Style = style;

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditDayNamesAutomationPeer(this);
  }

  internal void Dispose()
  {
    this.Content = (object) null;
    this.ContentTemplate = (DataTemplate) null;
    this.ContentTemplateSelector = (DataTemplateSelector) null;
    this.Style = (Style) null;
    this.Template = (ControlTemplate) null;
  }
}

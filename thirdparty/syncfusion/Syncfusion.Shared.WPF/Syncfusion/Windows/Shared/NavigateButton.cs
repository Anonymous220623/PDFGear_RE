// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.NavigateButton
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
public class NavigateButton : ContentControl
{
  public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(nameof (Enabled), typeof (bool), typeof (NavigateButton), (PropertyMetadata) new UIPropertyMetadata((object) true));

  static NavigateButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NavigateButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NavigateButton)));
  }

  public bool Enabled
  {
    get => (bool) this.GetValue(NavigateButton.EnabledProperty);
    set => this.SetValue(NavigateButton.EnabledProperty, (object) value);
  }

  protected internal void UpdateCellTemplate(ControlTemplate template)
  {
    if (template == null)
      return;
    this.Template = template;
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new CalendarEditNavigatorAutomationPeer(this);
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

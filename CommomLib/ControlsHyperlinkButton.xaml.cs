// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.HyperlinkButton
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace CommomLib.Controls;

[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
public partial class HyperlinkButton : ButtonBase
{
  public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(nameof (NavigateUri), typeof (Uri), typeof (HyperlinkButton), (PropertyMetadata) null);

  public HyperlinkButton()
  {
    this.DefaultStyleKey = (object) typeof (HyperlinkButton);
    if (HyperlinkButton.IsPermissionGranted())
      return;
    this.IsEnabled = false;
  }

  public Uri NavigateUri
  {
    get => this.GetValue(HyperlinkButton.NavigateUriProperty) as Uri;
    set => this.SetValue(HyperlinkButton.NavigateUriProperty, (object) value);
  }

  protected override void OnClick()
  {
    base.OnClick();
    if (!(this.NavigateUri != (Uri) null))
      return;
    if (!this.NavigateUri.IsAbsoluteUri)
      return;
    try
    {
      Process.Start(new ProcessStartInfo(this.NavigateUri.AbsoluteUri)
      {
        UseShellExecute = true
      });
    }
    catch (Win32Exception ex)
    {
    }
  }

  private static bool IsPermissionGranted()
  {
    try
    {
      new UIPermission(UIPermissionWindow.AllWindows).Demand();
      return true;
    }
    catch
    {
      return false;
    }
  }
}

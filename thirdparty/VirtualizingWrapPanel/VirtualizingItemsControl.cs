// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.VirtualizingItemsControl
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace WpfToolkit.Controls;

public class VirtualizingItemsControl : ItemsControl
{
  public VirtualizingItemsControl()
  {
    this.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof (VirtualizingStackPanel)));
    this.Template = (ControlTemplate) XamlReader.Parse("\r\n            <ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>\r\n                <Border\r\n                    BorderThickness='{TemplateBinding Border.BorderThickness}'\r\n                    Padding='{TemplateBinding Control.Padding}'\r\n                    BorderBrush='{TemplateBinding Border.BorderBrush}'\r\n                    Background='{TemplateBinding Panel.Background}'\r\n                    SnapsToDevicePixels='True'>\r\n                    <ScrollViewer\r\n                        Padding='{TemplateBinding Control.Padding}'\r\n                        Focusable='False'>\r\n                        <ItemsPresenter\r\n                            SnapsToDevicePixels='{TemplateBinding UIElement.SnapsToDevicePixels}'/>\r\n                    </ScrollViewer>\r\n                </Border>\r\n            </ControlTemplate>");
    ScrollViewer.SetCanContentScroll((DependencyObject) this, true);
    ScrollViewer.SetVerticalScrollBarVisibility((DependencyObject) this, ScrollBarVisibility.Auto);
    ScrollViewer.SetHorizontalScrollBarVisibility((DependencyObject) this, ScrollBarVisibility.Auto);
    VirtualizingPanel.SetCacheLengthUnit((DependencyObject) this, VirtualizationCacheLengthUnit.Page);
    VirtualizingPanel.SetCacheLength((DependencyObject) this, new VirtualizationCacheLength(1.0));
    VirtualizingPanel.SetIsVirtualizingWhenGrouping((DependencyObject) this, true);
  }
}

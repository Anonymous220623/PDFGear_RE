// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCloning
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class ChartCloning
{
  internal static void CloneControl(Control origianl, Control copy)
  {
    if (origianl == null || copy == null)
      return;
    copy.DataContext = origianl.DataContext;
    copy.HorizontalAlignment = origianl.HorizontalAlignment;
    copy.Language = origianl.Language;
    copy.Background = origianl.Background;
    copy.BorderBrush = origianl.BorderBrush;
    copy.BorderThickness = origianl.BorderThickness;
    copy.FlowDirection = origianl.FlowDirection;
    copy.FontFamily = origianl.FontFamily;
    copy.FontSize = origianl.FontSize;
    copy.FontStretch = origianl.FontStretch;
    copy.FontStyle = origianl.FontStyle;
    copy.FontWeight = origianl.FontWeight;
    copy.Foreground = origianl.Foreground;
    copy.HorizontalContentAlignment = origianl.HorizontalContentAlignment;
    copy.IsEnabled = origianl.IsEnabled;
    copy.IsHitTestVisible = origianl.IsHitTestVisible;
    copy.IsTabStop = origianl.IsTabStop;
    copy.Language = origianl.Language;
    copy.Margin = origianl.Margin;
    copy.MaxHeight = origianl.MaxHeight;
    copy.MaxWidth = origianl.MaxWidth;
    copy.MinHeight = origianl.MinHeight;
    copy.MinWidth = origianl.MinWidth;
    copy.Opacity = origianl.Opacity;
    copy.Padding = origianl.Padding;
    copy.RenderTransform = origianl.RenderTransform;
    copy.RenderTransformOrigin = origianl.RenderTransformOrigin;
    copy.Style = origianl.Style;
    copy.TabIndex = origianl.TabIndex;
    copy.Tag = origianl.Tag;
    copy.Template = origianl.Template;
    copy.UseLayoutRounding = origianl.UseLayoutRounding;
    copy.VerticalAlignment = origianl.VerticalAlignment;
    copy.VerticalContentAlignment = origianl.VerticalContentAlignment;
    copy.Visibility = origianl.Visibility;
  }
}

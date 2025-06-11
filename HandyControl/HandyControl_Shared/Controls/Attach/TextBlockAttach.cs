// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TextBlockAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Helper;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class TextBlockAttach
{
  public static readonly DependencyProperty AutoTooltipProperty = DependencyProperty.RegisterAttached("AutoTooltip", typeof (bool), typeof (TextBlockAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(TextBlockAttach.OnAutoTooltipChanged)));

  private static void OnAutoTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TextBlock textBlock))
      return;
    if ((bool) e.NewValue)
    {
      TextBlockAttach.UpdateTooltip(textBlock);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      textBlock.SizeChanged += TextBlockAttach.\u003C\u003EO.\u003C0\u003E__TextBlock_SizeChanged ?? (TextBlockAttach.\u003C\u003EO.\u003C0\u003E__TextBlock_SizeChanged = new SizeChangedEventHandler(TextBlockAttach.TextBlock_SizeChanged));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      textBlock.SizeChanged -= TextBlockAttach.\u003C\u003EO.\u003C0\u003E__TextBlock_SizeChanged ?? (TextBlockAttach.\u003C\u003EO.\u003C0\u003E__TextBlock_SizeChanged = new SizeChangedEventHandler(TextBlockAttach.TextBlock_SizeChanged));
    }
  }

  public static void SetAutoTooltip(DependencyObject element, bool value)
  {
    element.SetValue(TextBlockAttach.AutoTooltipProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAutoTooltip(DependencyObject element)
  {
    return (bool) element.GetValue(TextBlockAttach.AutoTooltipProperty);
  }

  private static void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (!(sender is TextBlock textBlock))
      return;
    TextBlockAttach.UpdateTooltip(textBlock);
  }

  private static void UpdateTooltip(TextBlock textBlock)
  {
    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    double num = textBlock.DesiredSize.Width - textBlock.Margin.Left - textBlock.Margin.Right;
    if (textBlock.RenderSize.Width > num || textBlock.ActualWidth < num || Math.Abs(TextBlockAttach.CalcTextWidth(textBlock) - num) > 1.0)
      ToolTipService.SetToolTip((DependencyObject) textBlock, (object) textBlock.Text);
    else
      ToolTipService.SetToolTip((DependencyObject) textBlock, (object) null);
  }

  private static double CalcTextWidth(TextBlock textBlock)
  {
    return TextHelper.CreateFormattedText(textBlock.Text, textBlock.FlowDirection, new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch), textBlock.FontSize).WidthIncludingTrailingWhitespace;
  }
}

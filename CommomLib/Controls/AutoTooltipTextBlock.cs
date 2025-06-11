// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.AutoTooltipTextBlock
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public class AutoTooltipTextBlock : TextBlock
{
  private static DependencyPropertyKey isTextTrimmedPropertyKey;
  public static readonly DependencyProperty IsTextTrimmedProperty = AutoTooltipTextBlock.IsTextTrimmedPropertyKey.DependencyProperty;

  public AutoTooltipTextBlock()
  {
    this.Loaded += new RoutedEventHandler(this.AutoTooltipTextBlock_Loaded);
    this.Unloaded += new RoutedEventHandler(this.AutoTooltipTextBlock_Unloaded);
    this.ToolTipOpening += new ToolTipEventHandler(this.AutoTooltipTextBlock_ToolTipOpening);
    this.TextTrimming = TextTrimming.CharacterEllipsis;
  }

  private void AutoTooltipTextBlock_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateIsTextTrimmedProperty();
    this.AttachTextBlockEvents();
  }

  private void AutoTooltipTextBlock_Unloaded(object sender, RoutedEventArgs e)
  {
    this.DetachTextBlockEvents();
    this.IsTextTrimmed = false;
  }

  private void AutoTooltipTextBlock_ToolTipOpening(object sender, ToolTipEventArgs e)
  {
    Stack<FrameworkElement> frameworkElementStack = new Stack<FrameworkElement>();
    FrameworkElement frameworkElement = (FrameworkElement) null;
    do
    {
      if (frameworkElement != null)
        frameworkElementStack.Push(frameworkElement);
      if (!((frameworkElement ?? (FrameworkElement) this).Parent is FrameworkElement parent))
        parent = VisualTreeHelper.GetParent((DependencyObject) (frameworkElement ?? (FrameworkElement) this)) as FrameworkElement;
      frameworkElement = parent;
    }
    while (frameworkElement != null);
    while (frameworkElementStack.Count > 0)
    {
      if (ToolTipExtensions.ShowToolTip((DependencyObject) frameworkElementStack.Pop()))
      {
        e.Handled = true;
        break;
      }
    }
  }

  public bool IsTextTrimmed
  {
    get => (bool) this.GetValue(AutoTooltipTextBlock.IsTextTrimmedProperty);
    private set => this.SetValue(AutoTooltipTextBlock.IsTextTrimmedPropertyKey, (object) value);
  }

  private static DependencyPropertyKey IsTextTrimmedPropertyKey
  {
    get
    {
      return AutoTooltipTextBlock.isTextTrimmedPropertyKey ?? (AutoTooltipTextBlock.isTextTrimmedPropertyKey = DependencyProperty.RegisterReadOnly("IsTextTrimmed", typeof (bool), typeof (AutoTooltipTextBlock), new PropertyMetadata((object) false, new PropertyChangedCallback(AutoTooltipTextBlock.OnIsTextTrimmedPropertyChanged))));
    }
  }

  private static void OnIsTextTrimmedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TextBlock textBlock))
      return;
    if (e.NewValue is bool newValue && newValue)
    {
      textBlock.SetBinding(ToolTipService.ToolTipProperty, (BindingBase) new Binding("Text")
      {
        Source = (object) textBlock
      });
    }
    else
    {
      BindingOperations.ClearBinding((DependencyObject) textBlock, ToolTipService.ToolTipProperty);
      ToolTipService.SetToolTip((DependencyObject) textBlock, (object) null);
    }
  }

  private void AttachTextBlockEvents()
  {
    this.SizeChanged += new SizeChangedEventHandler(this.TextBlock_SizeChanged);
    this.RegisterPropertyChangedCallback(TextBlock.FontFamilyProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.FontStyleProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.FontWeightProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.FontStretchProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.TextProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(FrameworkElement.FlowDirectionProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.FontSizeProperty, new EventHandler(this.OnTextPropertyChanged));
    this.RegisterPropertyChangedCallback(TextBlock.ForegroundProperty, new EventHandler(this.OnTextPropertyChanged));
  }

  private void DetachTextBlockEvents()
  {
    this.SizeChanged -= new SizeChangedEventHandler(this.TextBlock_SizeChanged);
    this.UnregisterPropertyChangedCallback(TextBlock.FontFamilyProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.FontStyleProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.FontWeightProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.FontStretchProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.TextProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(FrameworkElement.FlowDirectionProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.FontSizeProperty, new EventHandler(this.OnTextPropertyChanged));
    this.UnregisterPropertyChangedCallback(TextBlock.ForegroundProperty, new EventHandler(this.OnTextPropertyChanged));
  }

  private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.OnTextPropertyChanged(sender, (EventArgs) e);
  }

  private void OnTextPropertyChanged(object sender, EventArgs args)
  {
    if (!(sender is TextBlock))
      return;
    this.UpdateIsTextTrimmedProperty();
  }

  private void UpdateIsTextTrimmedProperty()
  {
    if (this.TextWrapping != TextWrapping.NoWrap)
      this.IsTextTrimmed = false;
    else
      this.IsTextTrimmed = new FormattedText(this.Text, Thread.CurrentThread.CurrentUICulture, this.FlowDirection, new Typeface(this.FontFamily ?? SystemFonts.MessageFontFamily, this.FontStyle, this.FontWeight, this.FontStretch), this.FontSize, this.Foreground, VisualTreeHelper.GetDpi((Visual) this).PixelsPerDip).Width - 2.0 > this.ActualWidth;
  }

  private void RegisterPropertyChangedCallback(DependencyProperty dp, EventHandler handler)
  {
    if (dp == null || handler == null)
      return;
    DependencyPropertyDescriptor.FromProperty(dp, typeof (TextBlock))?.AddValueChanged((object) this, handler);
  }

  private void UnregisterPropertyChangedCallback(DependencyProperty dp, EventHandler handler)
  {
    if (dp == null || handler == null)
      return;
    DependencyPropertyDescriptor.FromProperty(dp, typeof (TextBlock))?.RemoveValueChanged((object) this, handler);
  }
}

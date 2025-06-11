// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SimpleText
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Helper;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class SimpleText : FrameworkElement
{
  private FormattedText _formattedText;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(SimpleText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof (TextAlignment), typeof (TextAlignment), typeof (SimpleText), new PropertyMetadata((object) TextAlignment.Left, new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(nameof (TextTrimming), typeof (TextTrimming), typeof (SimpleText), new PropertyMetadata((object) TextTrimming.None, new PropertyChangedCallback(SimpleText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(nameof (TextWrapping), typeof (TextWrapping), typeof (SimpleText), new PropertyMetadata((object) TextWrapping.NoWrap, new PropertyChangedCallback(SimpleText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (SimpleText), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(SimpleText.OnFormattedTextUpdated)));

  static SimpleText()
  {
    UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
    FrameworkElement.UseLayoutRoundingProperty.OverrideMetadata(typeof (SimpleText), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
  }

  public string Text
  {
    get => (string) this.GetValue(SimpleText.TextProperty);
    set => this.SetValue(SimpleText.TextProperty, (object) value);
  }

  public TextAlignment TextAlignment
  {
    get => (TextAlignment) this.GetValue(SimpleText.TextAlignmentProperty);
    set => this.SetValue(SimpleText.TextAlignmentProperty, (object) value);
  }

  public TextTrimming TextTrimming
  {
    get => (TextTrimming) this.GetValue(SimpleText.TextTrimmingProperty);
    set => this.SetValue(SimpleText.TextTrimmingProperty, (object) value);
  }

  public TextWrapping TextWrapping
  {
    get => (TextWrapping) this.GetValue(SimpleText.TextWrappingProperty);
    set => this.SetValue(SimpleText.TextWrappingProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(SimpleText.ForegroundProperty);
    set => this.SetValue(SimpleText.ForegroundProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(SimpleText.FontFamilyProperty);
    set => this.SetValue(SimpleText.FontFamilyProperty, (object) value);
  }

  [TypeConverter(typeof (FontSizeConverter))]
  public double FontSize
  {
    get => (double) this.GetValue(SimpleText.FontSizeProperty);
    set => this.SetValue(SimpleText.FontSizeProperty, (object) value);
  }

  public FontStretch FontStretch
  {
    get => (FontStretch) this.GetValue(SimpleText.FontStretchProperty);
    set => this.SetValue(SimpleText.FontStretchProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(SimpleText.FontStyleProperty);
    set => this.SetValue(SimpleText.FontStyleProperty, (object) value);
  }

  public FontWeight FontWeight
  {
    get => (FontWeight) this.GetValue(SimpleText.FontWeightProperty);
    set => this.SetValue(SimpleText.FontWeightProperty, (object) value);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    drawingContext.DrawText(this._formattedText, new Point());
  }

  private void EnsureFormattedText()
  {
    if (this._formattedText != null || this.Text == null)
      return;
    this._formattedText = TextHelper.CreateFormattedText(this.Text, this.FlowDirection, new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch), this.FontSize);
    this.UpdateFormattedText();
  }

  private void UpdateFormattedText()
  {
    if (this._formattedText == null)
      return;
    this._formattedText.MaxLineCount = this.TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
    this._formattedText.TextAlignment = this.TextAlignment;
    this._formattedText.Trimming = this.TextTrimming;
    this._formattedText.SetFontSize(this.FontSize);
    this._formattedText.SetFontStyle(this.FontStyle);
    this._formattedText.SetFontWeight(this.FontWeight);
    this._formattedText.SetFontFamily(this.FontFamily);
    this._formattedText.SetFontStretch(this.FontStretch);
    this._formattedText.SetForegroundBrush(this.Foreground);
  }

  private static void OnFormattedTextUpdated(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SimpleText simpleText = (SimpleText) d;
    simpleText.UpdateFormattedText();
    simpleText.InvalidateMeasure();
    simpleText.InvalidateVisual();
  }

  private static void OnFormattedTextInvalidated(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SimpleText simpleText = (SimpleText) d;
    simpleText._formattedText = (FormattedText) null;
    simpleText.InvalidateMeasure();
    simpleText.InvalidateVisual();
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.EnsureFormattedText();
    this._formattedText.MaxTextWidth = Math.Min(3579139.0, availableSize.Width);
    this._formattedText.MaxTextHeight = Math.Max(0.0001, availableSize.Height);
    return new Size(this._formattedText.Width, this._formattedText.Height);
  }
}

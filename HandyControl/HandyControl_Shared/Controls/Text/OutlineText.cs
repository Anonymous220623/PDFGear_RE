// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.OutlineText
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

public class OutlineText : FrameworkElement
{
  private Pen _pen;
  private FormattedText _formattedText;
  private Geometry _textGeometry;
  private PathGeometry _clipGeometry;
  public static readonly DependencyProperty StrokePositionProperty = DependencyProperty.Register(nameof (StrokePosition), typeof (StrokePosition), typeof (OutlineText), new PropertyMetadata((object) StrokePosition.Center));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OutlineText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof (TextAlignment), typeof (TextAlignment), typeof (OutlineText), new PropertyMetadata((object) TextAlignment.Left, new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(nameof (TextTrimming), typeof (TextTrimming), typeof (OutlineText), new PropertyMetadata((object) TextTrimming.None, new PropertyChangedCallback(OutlineText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(nameof (TextWrapping), typeof (TextWrapping), typeof (OutlineText), new PropertyMetadata((object) TextWrapping.NoWrap, new PropertyChangedCallback(OutlineText.OnFormattedTextInvalidated)));
  public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof (Fill), typeof (Brush), typeof (OutlineText), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (OutlineText), new PropertyMetadata((object) Brushes.Black, new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (OutlineText), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));
  public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlineText.OnFormattedTextUpdated)));

  static OutlineText()
  {
    UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
    FrameworkElement.UseLayoutRoundingProperty.OverrideMetadata(typeof (OutlineText), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
  }

  public StrokePosition StrokePosition
  {
    get => (StrokePosition) this.GetValue(OutlineText.StrokePositionProperty);
    set => this.SetValue(OutlineText.StrokePositionProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(OutlineText.TextProperty);
    set => this.SetValue(OutlineText.TextProperty, (object) value);
  }

  public TextAlignment TextAlignment
  {
    get => (TextAlignment) this.GetValue(OutlineText.TextAlignmentProperty);
    set => this.SetValue(OutlineText.TextAlignmentProperty, (object) value);
  }

  public TextTrimming TextTrimming
  {
    get => (TextTrimming) this.GetValue(OutlineText.TextTrimmingProperty);
    set => this.SetValue(OutlineText.TextTrimmingProperty, (object) value);
  }

  public TextWrapping TextWrapping
  {
    get => (TextWrapping) this.GetValue(OutlineText.TextWrappingProperty);
    set => this.SetValue(OutlineText.TextWrappingProperty, (object) value);
  }

  public Brush Fill
  {
    get => (Brush) this.GetValue(OutlineText.FillProperty);
    set => this.SetValue(OutlineText.FillProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(OutlineText.StrokeProperty);
    set => this.SetValue(OutlineText.StrokeProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(OutlineText.StrokeThicknessProperty);
    set => this.SetValue(OutlineText.StrokeThicknessProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(OutlineText.FontFamilyProperty);
    set => this.SetValue(OutlineText.FontFamilyProperty, (object) value);
  }

  [TypeConverter(typeof (FontSizeConverter))]
  public double FontSize
  {
    get => (double) this.GetValue(OutlineText.FontSizeProperty);
    set => this.SetValue(OutlineText.FontSizeProperty, (object) value);
  }

  public FontStretch FontStretch
  {
    get => (FontStretch) this.GetValue(OutlineText.FontStretchProperty);
    set => this.SetValue(OutlineText.FontStretchProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(OutlineText.FontStyleProperty);
    set => this.SetValue(OutlineText.FontStyleProperty, (object) value);
  }

  public FontWeight FontWeight
  {
    get => (FontWeight) this.GetValue(OutlineText.FontWeightProperty);
    set => this.SetValue(OutlineText.FontWeightProperty, (object) value);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    if (this.StrokeThickness > 0.0)
    {
      this.EnsureGeometry();
      drawingContext.DrawGeometry(this.Fill, (Pen) null, this._textGeometry);
      if (this.StrokePosition == StrokePosition.Outside)
        drawingContext.PushClip((Geometry) this._clipGeometry);
      else if (this.StrokePosition == StrokePosition.Inside)
        drawingContext.PushClip(this._textGeometry);
      drawingContext.DrawGeometry((Brush) null, this._pen, this._textGeometry);
      if (this.StrokePosition != StrokePosition.Outside && this.StrokePosition != StrokePosition.Inside)
        return;
      drawingContext.Pop();
    }
    else
    {
      this.UpdateFormattedText();
      drawingContext.DrawText(this._formattedText, new Point());
    }
  }

  private void UpdatePen()
  {
    this._pen = new Pen(this.Stroke, this.StrokeThickness);
    if (this.StrokePosition != StrokePosition.Outside && this.StrokePosition != StrokePosition.Inside)
      return;
    this._pen.Thickness = this.StrokeThickness * 2.0;
  }

  private void EnsureFormattedText()
  {
    if (this._formattedText != null || this.Text == null)
      return;
    this._formattedText = TextHelper.CreateFormattedText(this.Text, this.FlowDirection, new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch), this.FontSize);
    this.UpdateFormattedText();
  }

  private void EnsureGeometry()
  {
    if (this._textGeometry != null)
      return;
    this.EnsureFormattedText();
    this._textGeometry = this._formattedText.BuildGeometry(new Point(0.0, 0.0));
    if (this.StrokePosition != StrokePosition.Outside)
      return;
    this._clipGeometry = Geometry.Combine((Geometry) new RectangleGeometry(new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)), this._textGeometry, GeometryCombineMode.Exclude, (Transform) null);
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
    this._formattedText.SetForegroundBrush(this.Fill);
  }

  private static void OnFormattedTextUpdated(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    OutlineText outlineText = (OutlineText) d;
    outlineText.UpdateFormattedText();
    outlineText._textGeometry = (Geometry) null;
    outlineText.InvalidateMeasure();
    outlineText.InvalidateVisual();
  }

  private static void OnFormattedTextInvalidated(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    OutlineText outlineText = (OutlineText) d;
    outlineText._formattedText = (FormattedText) null;
    outlineText._textGeometry = (Geometry) null;
    outlineText.InvalidateMeasure();
    outlineText.InvalidateVisual();
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.EnsureFormattedText();
    this._formattedText.MaxTextWidth = Math.Min(3579139.0, availableSize.Width);
    this._formattedText.MaxTextHeight = Math.Max(0.0001, availableSize.Height);
    this.UpdatePen();
    return new Size(this._formattedText.Width, this._formattedText.Height);
  }
}

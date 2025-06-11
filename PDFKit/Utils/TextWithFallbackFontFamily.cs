// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.TextWithFallbackFontFamily
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils;

public struct TextWithFallbackFontFamily(
  string text,
  FontFamily fallbackFontFamily,
  System.Windows.FontWeight fontWeight,
  FontStyle fontStyle,
  float fontSize,
  float scaledFontSize,
  Rect bounds,
  float baseline,
  FontCharSet charSet)
{
  public string Text { get; } = text;

  public FontFamily FallbackFontFamily { get; } = fallbackFontFamily;

  public System.Windows.FontWeight FontWeight { get; } = fontWeight;

  public FontStyle FontStyle { get; } = fontStyle;

  public float FontSize { get; } = fontSize;

  public float ScaledFontSize { get; } = scaledFontSize;

  public Rect Bounds { get; } = bounds;

  public float Baseline { get; } = baseline;

  public FontCharSet CharSet { get; } = charSet;
}

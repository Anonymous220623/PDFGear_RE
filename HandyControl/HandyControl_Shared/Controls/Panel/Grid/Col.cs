// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Col
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class Col : ContentControl
{
  public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof (Layout), typeof (ColLayout), typeof (Col), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
  public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(nameof (Offset), typeof (int), typeof (Col), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
  public static readonly DependencyProperty SpanProperty = DependencyProperty.Register(nameof (Span), typeof (int), typeof (Col), (PropertyMetadata) new FrameworkPropertyMetadata((object) 24, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Col.OnSpanValidate));
  public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register(nameof (IsFixed), typeof (bool), typeof (Col), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

  public ColLayout Layout
  {
    get => (ColLayout) this.GetValue(Col.LayoutProperty);
    set => this.SetValue(Col.LayoutProperty, (object) value);
  }

  public int Offset
  {
    get => (int) this.GetValue(Col.OffsetProperty);
    set => this.SetValue(Col.OffsetProperty, (object) value);
  }

  private static bool OnSpanValidate(object value)
  {
    int num = (int) value;
    return num >= 1 && num <= 24;
  }

  public int Span
  {
    get => (int) this.GetValue(Col.SpanProperty);
    set => this.SetValue(Col.SpanProperty, (object) value);
  }

  public bool IsFixed
  {
    get => (bool) this.GetValue(Col.IsFixedProperty);
    set => this.SetValue(Col.IsFixedProperty, ValueBoxes.BooleanBox(value));
  }

  internal int GetLayoutCellCount(ColLayoutStatus status)
  {
    if (this.Layout != null)
    {
      switch (status)
      {
        case ColLayoutStatus.Xs:
          return this.Layout.Xs;
        case ColLayoutStatus.Sm:
          return this.Layout.Sm;
        case ColLayoutStatus.Md:
          return this.Layout.Md;
        case ColLayoutStatus.Lg:
          return this.Layout.Lg;
        case ColLayoutStatus.Xl:
          return this.Layout.Xl;
        case ColLayoutStatus.Xxl:
          return this.Layout.Xxl;
        case ColLayoutStatus.Auto:
          return 0;
        default:
          throw new ArgumentOutOfRangeException(nameof (status), (object) status, (string) null);
      }
    }
    else
      return this.IsFixed ? 0 : this.Span;
  }
}

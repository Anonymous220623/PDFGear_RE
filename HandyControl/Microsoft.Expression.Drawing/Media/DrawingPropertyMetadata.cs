// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.DrawingPropertyMetadata
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Media;

internal class DrawingPropertyMetadata : FrameworkPropertyMetadata
{
  private DrawingPropertyMetadataOptions options;
  private PropertyChangedCallback propertyChangedCallback;

  public static event EventHandler<DrawingPropertyChangedEventArgs> DrawingPropertyChanged;

  static DrawingPropertyMetadata()
  {
    DrawingPropertyMetadata.DrawingPropertyChanged += (EventHandler<DrawingPropertyChangedEventArgs>) ((sender, args) =>
    {
      if (!(sender is IShape shape2) || !args.Metadata.AffectsRender)
        return;
      InvalidateGeometryReasons reasons = InvalidateGeometryReasons.PropertyChanged;
      if (args.IsAnimated)
        reasons |= InvalidateGeometryReasons.IsAnimated;
      shape2.InvalidateGeometry(reasons);
    });
  }

  public DrawingPropertyMetadata(object defaultValue)
    : this(defaultValue, DrawingPropertyMetadataOptions.None, (PropertyChangedCallback) null)
  {
  }

  public DrawingPropertyMetadata(PropertyChangedCallback propertyChangedCallback)
    : this(DependencyProperty.UnsetValue, DrawingPropertyMetadataOptions.None, propertyChangedCallback)
  {
  }

  private DrawingPropertyMetadata(DrawingPropertyMetadataOptions options, object defaultValue)
    : base(defaultValue, (FrameworkPropertyMetadataOptions) options)
  {
  }

  public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options)
    : this(defaultValue, options, (PropertyChangedCallback) null)
  {
  }

  public DrawingPropertyMetadata(
    object defaultValue,
    DrawingPropertyMetadataOptions options,
    PropertyChangedCallback propertyChangedCallback)
    : base(defaultValue, (FrameworkPropertyMetadataOptions) options, DrawingPropertyMetadata.AttachCallback(defaultValue, options, propertyChangedCallback))
  {
  }

  private static PropertyChangedCallback AttachCallback(
    object defaultValue,
    DrawingPropertyMetadataOptions options,
    PropertyChangedCallback propertyChangedCallback)
  {
    return new PropertyChangedCallback(new DrawingPropertyMetadata(options, defaultValue)
    {
      options = options,
      propertyChangedCallback = propertyChangedCallback
    }.InternalCallback);
  }

  private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
  {
    if (DrawingPropertyMetadata.DrawingPropertyChanged != null)
    {
      DrawingPropertyChangedEventArgs e1 = new DrawingPropertyChangedEventArgs()
      {
        Metadata = this,
        IsAnimated = DependencyPropertyHelper.GetValueSource(sender, e.Property).IsAnimated
      };
      DrawingPropertyMetadata.DrawingPropertyChanged((object) sender, e1);
    }
    PropertyChangedCallback propertyChangedCallback = this.propertyChangedCallback;
    if (propertyChangedCallback == null)
      return;
    propertyChangedCallback(sender, e);
  }
}

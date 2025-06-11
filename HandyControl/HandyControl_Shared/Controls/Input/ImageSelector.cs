// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImageSelector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Controls;

public class ImageSelector : Control
{
  public static readonly RoutedEvent ImageSelectedEvent = EventManager.RegisterRoutedEvent("ImageSelected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ImageSelector));
  public static readonly RoutedEvent ImageUnselectedEvent = EventManager.RegisterRoutedEvent("ImageUnselected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ImageSelector));
  public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof (Stretch), typeof (Stretch), typeof (ImageSelector), new PropertyMetadata((object) Stretch.None));
  public static readonly DependencyPropertyKey UriPropertyKey = DependencyProperty.RegisterReadOnly(nameof (Uri), typeof (Uri), typeof (ImageSelector), new PropertyMetadata((object) null));
  public static readonly DependencyProperty UriProperty = ImageSelector.UriPropertyKey.DependencyProperty;
  public static readonly DependencyPropertyKey PreviewBrushPropertyKey = DependencyProperty.RegisterReadOnly(nameof (PreviewBrush), typeof (Brush), typeof (ImageSelector), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PreviewBrushProperty = ImageSelector.PreviewBrushPropertyKey.DependencyProperty;
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ImageSelector), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (ImageSelector), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DefaultExtProperty = DependencyProperty.Register(nameof (DefaultExt), typeof (string), typeof (ImageSelector), new PropertyMetadata((object) ".jpg"));
  public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof (Filter), typeof (string), typeof (ImageSelector), new PropertyMetadata((object) "(.jpg)|*.jpg|(.png)|*.png"));
  public static readonly DependencyPropertyKey HasValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasValue), typeof (bool), typeof (ImageSelector), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty HasValueProperty = ImageSelector.HasValuePropertyKey.DependencyProperty;

  public event RoutedEventHandler ImageSelected
  {
    add => this.AddHandler(ImageSelector.ImageSelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(ImageSelector.ImageSelectedEvent, (Delegate) value);
  }

  public event RoutedEventHandler ImageUnselected
  {
    add => this.AddHandler(ImageSelector.ImageUnselectedEvent, (Delegate) value);
    remove => this.RemoveHandler(ImageSelector.ImageUnselectedEvent, (Delegate) value);
  }

  public ImageSelector()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Switch, new ExecutedRoutedEventHandler(this.SwitchImage)));
  }

  private void SwitchImage(object sender, ExecutedRoutedEventArgs e)
  {
    if (!this.HasValue)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.RestoreDirectory = true;
      openFileDialog1.Filter = this.Filter;
      openFileDialog1.DefaultExt = this.DefaultExt;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      bool? nullable = openFileDialog2.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.SetValue(ImageSelector.UriPropertyKey, (object) new Uri(openFileDialog2.FileName, UriKind.RelativeOrAbsolute));
      DependencyPropertyKey brushPropertyKey = ImageSelector.PreviewBrushPropertyKey;
      ImageBrush imageBrush = new ImageBrush((ImageSource) BitmapFrame.Create(this.Uri, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None));
      imageBrush.Stretch = this.Stretch;
      this.SetValue(brushPropertyKey, (object) imageBrush);
      this.SetValue(ImageSelector.HasValuePropertyKey, ValueBoxes.TrueBox);
      this.SetCurrentValue(FrameworkElement.ToolTipProperty, (object) openFileDialog2.FileName);
      this.RaiseEvent(new RoutedEventArgs(ImageSelector.ImageSelectedEvent, (object) this));
    }
    else
    {
      this.SetValue(ImageSelector.UriPropertyKey, (object) null);
      this.SetValue(ImageSelector.PreviewBrushPropertyKey, (object) null);
      this.SetValue(ImageSelector.HasValuePropertyKey, ValueBoxes.FalseBox);
      this.SetCurrentValue(FrameworkElement.ToolTipProperty, (object) null);
      this.RaiseEvent(new RoutedEventArgs(ImageSelector.ImageUnselectedEvent, (object) this));
    }
  }

  public Stretch Stretch
  {
    get => (Stretch) this.GetValue(ImageSelector.StretchProperty);
    set => this.SetValue(ImageSelector.StretchProperty, (object) value);
  }

  public Uri Uri
  {
    get => (Uri) this.GetValue(ImageSelector.UriProperty);
    set => this.SetValue(ImageSelector.UriProperty, (object) value);
  }

  public Brush PreviewBrush
  {
    get => (Brush) this.GetValue(ImageSelector.PreviewBrushProperty);
    set => this.SetValue(ImageSelector.PreviewBrushProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ImageSelector.StrokeThicknessProperty);
    set => this.SetValue(ImageSelector.StrokeThicknessProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(ImageSelector.StrokeDashArrayProperty);
    set => this.SetValue(ImageSelector.StrokeDashArrayProperty, (object) value);
  }

  public string DefaultExt
  {
    get => (string) this.GetValue(ImageSelector.DefaultExtProperty);
    set => this.SetValue(ImageSelector.DefaultExtProperty, (object) value);
  }

  public string Filter
  {
    get => (string) this.GetValue(ImageSelector.FilterProperty);
    set => this.SetValue(ImageSelector.FilterProperty, (object) value);
  }

  public bool HasValue
  {
    get => (bool) this.GetValue(ImageSelector.HasValueProperty);
    set => this.SetValue(ImageSelector.HasValueProperty, ValueBoxes.BooleanBox(value));
  }
}

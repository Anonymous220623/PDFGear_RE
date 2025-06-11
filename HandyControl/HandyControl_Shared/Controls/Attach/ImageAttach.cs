// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImageAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class ImageAttach
{
  public static readonly DependencyProperty SourceFailedProperty = DependencyProperty.RegisterAttached("SourceFailed", typeof (ImageSource), typeof (ImageAttach), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(ImageAttach.OnSourceFailedChanged)));

  private static void OnSourceFailedChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Image image))
      return;
    if (e.NewValue is ImageSource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      image.ImageFailed += ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed ?? (ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed = new EventHandler<ExceptionRoutedEventArgs>(ImageAttach.Image_ImageFailed));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      image.ImageFailed -= ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed ?? (ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed = new EventHandler<ExceptionRoutedEventArgs>(ImageAttach.Image_ImageFailed));
    }
  }

  private static void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
  {
    if (!(sender is Image element))
      return;
    element.SetCurrentValue(Image.SourceProperty, (object) ImageAttach.GetSourceFailed((DependencyObject) element));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.ImageFailed -= ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed ?? (ImageAttach.\u003C\u003EO.\u003C0\u003E__Image_ImageFailed = new EventHandler<ExceptionRoutedEventArgs>(ImageAttach.Image_ImageFailed));
  }

  public static void SetSourceFailed(DependencyObject element, ImageSource value)
  {
    element.SetValue(ImageAttach.SourceFailedProperty, (object) value);
  }

  public static ImageSource GetSourceFailed(DependencyObject element)
  {
    return (ImageSource) element.GetValue(ImageAttach.SourceFailedProperty);
  }
}

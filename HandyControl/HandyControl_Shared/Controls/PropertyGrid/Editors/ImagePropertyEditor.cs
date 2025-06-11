// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImagePropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Controls;

public class ImagePropertyEditor : PropertyEditorBase
{
  internal static readonly DependencyProperty UriProperty = DependencyProperty.Register(nameof (Uri), typeof (Uri), typeof (ImagePropertyEditor), new PropertyMetadata((object) null, new PropertyChangedCallback(ImagePropertyEditor.OnUriChangedCallback)));
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (ImageSource), typeof (ImagePropertyEditor), new PropertyMetadata((object) null));

  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    ImageSelector imageSelector = new ImageSelector();
    imageSelector.IsEnabled = !propertyItem.IsReadOnly;
    imageSelector.Width = 50.0;
    imageSelector.Height = 50.0;
    imageSelector.HorizontalAlignment = HorizontalAlignment.Left;
    ImageSelector element = imageSelector;
    BindingOperations.SetBinding((DependencyObject) this, ImagePropertyEditor.UriProperty, (BindingBase) new Binding(ImageSelector.UriProperty.Name)
    {
      Source = (object) element,
      Mode = BindingMode.OneWay
    });
    return (FrameworkElement) element;
  }

  private static void OnUriChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ImagePropertyEditor imagePropertyEditor = (ImagePropertyEditor) d;
    Uri newValue = e.NewValue as Uri;
    BitmapFrame bitmapFrame = (object) newValue != null ? BitmapFrame.Create(newValue) : (BitmapFrame) null;
    imagePropertyEditor.Source = (ImageSource) bitmapFrame;
  }

  internal Uri Uri
  {
    get => (Uri) this.GetValue(ImagePropertyEditor.UriProperty);
    set => this.SetValue(ImagePropertyEditor.UriProperty, (object) value);
  }

  public ImageSource Source
  {
    get => (ImageSource) this.GetValue(ImagePropertyEditor.SourceProperty);
    set => this.SetValue(ImagePropertyEditor.SourceProperty, (object) value);
  }

  public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
  {
    BindingOperations.SetBinding((DependencyObject) this, this.GetDependencyProperty(), (BindingBase) new Binding($"({propertyItem.PropertyName})")
    {
      Source = propertyItem.Value,
      Mode = this.GetBindingMode(propertyItem),
      UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyItem),
      Converter = this.GetConverter(propertyItem)
    });
  }

  public override DependencyProperty GetDependencyProperty() => ImagePropertyEditor.SourceProperty;
}

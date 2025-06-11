// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Gravatar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class Gravatar : ContentControl
{
  public static readonly DependencyProperty GeneratorProperty = DependencyProperty.Register(nameof (Generator), typeof (IGravatarGenerator), typeof (Gravatar), new PropertyMetadata((object) new GithubGravatarGenerator()));
  public static readonly DependencyProperty IdProperty = DependencyProperty.Register(nameof (Id), typeof (string), typeof (Gravatar), new PropertyMetadata((object) null, new PropertyChangedCallback(Gravatar.OnIdChanged)));
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (ImageSource), typeof (Gravatar), new PropertyMetadata((object) null, new PropertyChangedCallback(Gravatar.OnSourceChanged)));

  public IGravatarGenerator Generator
  {
    get => (IGravatarGenerator) this.GetValue(Gravatar.GeneratorProperty);
    set => this.SetValue(Gravatar.GeneratorProperty, (object) value);
  }

  private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Gravatar gravatar = (Gravatar) d;
    if (gravatar.Source != null)
      return;
    gravatar.Content = gravatar.Generator.GetGravatar((string) e.NewValue);
  }

  public string Id
  {
    get => (string) this.GetValue(Gravatar.IdProperty);
    set => this.SetValue(Gravatar.IdProperty, (object) value);
  }

  private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Gravatar gravatar = (Gravatar) d;
    ImageSource newValue = (ImageSource) e.NewValue;
    Brush brush;
    if (newValue == null)
    {
      brush = ResourceHelper.GetResourceInternal<Brush>("SecondaryRegionBrush");
    }
    else
    {
      brush = (Brush) new ImageBrush(newValue);
      ((TileBrush) brush).Stretch = Stretch.UniformToFill;
    }
    gravatar.Background = brush;
  }

  public ImageSource Source
  {
    get => (ImageSource) this.GetValue(Gravatar.SourceProperty);
    set => this.SetValue(Gravatar.SourceProperty, (object) value);
  }
}

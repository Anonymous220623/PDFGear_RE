// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.VisualState
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows;

[RuntimeNameProperty("Name")]
[ContentProperty("Storyboard")]
public class VisualState : DependencyObject
{
  private static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(nameof (Storyboard), typeof (Storyboard), typeof (VisualState));

  public string Name { get; set; }

  public Storyboard Storyboard
  {
    get => (Storyboard) this.GetValue(VisualState.StoryboardProperty);
    set => this.SetValue(VisualState.StoryboardProperty, (object) value);
  }
}

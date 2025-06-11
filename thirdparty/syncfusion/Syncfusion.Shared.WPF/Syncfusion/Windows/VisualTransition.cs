// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.VisualTransition
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows;

[ContentProperty("Storyboard")]
public class VisualTransition : DependencyObject
{
  private Duration _generatedDuration = new Duration(new TimeSpan());

  public VisualTransition()
  {
    this.DynamicStoryboardCompleted = true;
    this.ExplicitStoryboardCompleted = true;
  }

  public string From { get; set; }

  public string To { get; set; }

  public Storyboard Storyboard { get; set; }

  [TypeConverter(typeof (DurationConverter))]
  public Duration GeneratedDuration
  {
    get => this._generatedDuration;
    set => this._generatedDuration = value;
  }

  internal bool IsDefault => this.From == null && this.To == null;

  internal bool DynamicStoryboardCompleted { get; set; }

  internal bool ExplicitStoryboardCompleted { get; set; }
}

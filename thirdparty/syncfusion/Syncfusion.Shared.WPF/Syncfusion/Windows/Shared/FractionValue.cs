// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.FractionValue
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class FractionValue : DependencyObject
{
  public static readonly DependencyProperty FractionProperty = DependencyProperty.Register(nameof (Fraction), typeof (double), typeof (FractionValue), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (FractionValue), new PropertyMetadata((object) 0.0));
  internal Action PropertyChanged;

  public double Fraction
  {
    get => (double) this.GetValue(FractionValue.FractionProperty);
    set => this.SetValue(FractionValue.FractionProperty, (object) value);
  }

  public double Value
  {
    get => (double) this.GetValue(FractionValue.ValueProperty);
    set => this.SetValue(FractionValue.ValueProperty, (object) value);
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
    if (e.Property != FractionValue.FractionProperty && e.Property != FractionValue.ValueProperty)
      return;
    this.RaisePropertyChanged();
  }

  private void RaisePropertyChanged()
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged();
  }
}

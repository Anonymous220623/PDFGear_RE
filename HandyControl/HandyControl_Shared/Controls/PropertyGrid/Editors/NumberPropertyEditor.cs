// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.NumberPropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class NumberPropertyEditor : PropertyEditorBase
{
  public NumberPropertyEditor()
  {
  }

  public NumberPropertyEditor(double minimum, double maximum)
  {
    this.Minimum = minimum;
    this.Maximum = maximum;
  }

  public double Minimum { get; set; }

  public double Maximum { get; set; }

  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    return (FrameworkElement) new NumericUpDown()
    {
      IsReadOnly = propertyItem.IsReadOnly,
      Minimum = this.Minimum,
      Maximum = this.Maximum
    };
  }

  public override DependencyProperty GetDependencyProperty() => NumericUpDown.ValueProperty;
}

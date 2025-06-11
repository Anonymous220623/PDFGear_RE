// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.DatePicker
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Interactivity;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_TextBox", Type = typeof (DatePickerTextBox))]
public class DatePicker : System.Windows.Controls.DatePicker
{
  private const string ElementTextBox = "PART_TextBox";
  private System.Windows.Controls.TextBox _textBox;
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (DatePicker));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (DatePicker));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (DatePicker));

  public DatePicker()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this.SetCurrentValue(System.Windows.Controls.DatePicker.SelectedDateProperty, (object) null);
      this.SetCurrentValue(System.Windows.Controls.DatePicker.TextProperty, (object) "");
    })));
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._textBox = this.GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
    if (this._textBox == null)
      return;
    this._textBox.SetBinding(DatePicker.SelectionBrushProperty, (BindingBase) new Binding(DatePicker.SelectionBrushProperty.Name)
    {
      Source = (object) this
    });
    this._textBox.SetBinding(DatePicker.SelectionOpacityProperty, (BindingBase) new Binding(DatePicker.SelectionOpacityProperty.Name)
    {
      Source = (object) this
    });
    this._textBox.SetBinding(DatePicker.CaretBrushProperty, (BindingBase) new Binding(DatePicker.CaretBrushProperty.Name)
    {
      Source = (object) this
    });
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(DatePicker.SelectionBrushProperty);
    set => this.SetValue(DatePicker.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(DatePicker.SelectionOpacityProperty);
    set => this.SetValue(DatePicker.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(DatePicker.CaretBrushProperty);
    set => this.SetValue(DatePicker.CaretBrushProperty, (object) value);
  }
}

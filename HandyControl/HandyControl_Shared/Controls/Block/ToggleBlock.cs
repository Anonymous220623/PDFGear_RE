// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ToggleBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Input;
using HandyControl.Interactivity;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Controls;

public class ToggleBlock : Control
{
  public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool?), typeof (ToggleBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));
  public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register(nameof (CheckedContent), typeof (object), typeof (ToggleBlock), new PropertyMetadata((object) null));
  public static readonly DependencyProperty UnCheckedContentProperty = DependencyProperty.Register(nameof (UnCheckedContent), typeof (object), typeof (ToggleBlock), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IndeterminateContentProperty = DependencyProperty.Register(nameof (IndeterminateContent), typeof (object), typeof (ToggleBlock), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ToggleGestureProperty = DependencyProperty.Register(nameof (ToggleGesture), typeof (MouseGesture), typeof (ToggleBlock), (PropertyMetadata) new UIPropertyMetadata((object) new MouseGesture(MouseAction.None), new PropertyChangedCallback(ToggleBlock.OnToggleGestureChanged)));

  [Category("Appearance")]
  [TypeConverter(typeof (NullableBoolConverter))]
  [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
  public bool? IsChecked
  {
    get
    {
      object obj = this.GetValue(ToggleBlock.IsCheckedProperty);
      return obj != null ? new bool?((bool) obj) : new bool?();
    }
    set
    {
      this.SetValue(ToggleBlock.IsCheckedProperty, value.HasValue ? ValueBoxes.BooleanBox(value.Value) : (object) null);
    }
  }

  public object CheckedContent
  {
    get => this.GetValue(ToggleBlock.CheckedContentProperty);
    set => this.SetValue(ToggleBlock.CheckedContentProperty, value);
  }

  public object UnCheckedContent
  {
    get => this.GetValue(ToggleBlock.UnCheckedContentProperty);
    set => this.SetValue(ToggleBlock.UnCheckedContentProperty, value);
  }

  public object IndeterminateContent
  {
    get => this.GetValue(ToggleBlock.IndeterminateContentProperty);
    set => this.SetValue(ToggleBlock.IndeterminateContentProperty, value);
  }

  [ValueSerializer(typeof (MouseGestureValueSerializer))]
  [TypeConverter(typeof (MouseGestureConverter))]
  public MouseGesture ToggleGesture
  {
    get => (MouseGesture) this.GetValue(ToggleBlock.ToggleGestureProperty);
    set => this.SetValue(ToggleBlock.ToggleGestureProperty, (object) value);
  }

  public ToggleBlock()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Toggle, new ExecutedRoutedEventHandler(this.OnToggled)));
    this.OnToggleGestureChanged(this.ToggleGesture);
  }

  private static void OnToggleGestureChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ToggleBlock) d).OnToggleGestureChanged((MouseGesture) e.NewValue);
  }

  private void OnToggleGestureChanged(MouseGesture newValue)
  {
    foreach (InputBinding inputBinding in this.InputBindings.OfType<SimpleMouseBinding>().ToList<SimpleMouseBinding>())
      this.InputBindings.Remove(inputBinding);
    this.InputBindings.Add((InputBinding) new SimpleMouseBinding((ICommand) ControlCommands.Toggle, newValue));
  }

  private void OnToggled(object sender, ExecutedRoutedEventArgs e)
  {
    DependencyProperty isCheckedProperty = ToggleBlock.IsCheckedProperty;
    bool? isChecked = this.IsChecked;
    bool flag = true;
    object obj = isChecked.GetValueOrDefault() == flag & isChecked.HasValue ? ValueBoxes.FalseBox : ValueBoxes.TrueBox;
    this.SetCurrentValue(isCheckedProperty, obj);
  }
}

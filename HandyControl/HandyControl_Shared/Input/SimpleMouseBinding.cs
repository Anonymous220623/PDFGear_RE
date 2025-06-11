// Decompiled with JetBrains decompiler
// Type: HandyControl.Input.SimpleMouseBinding
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Input;

internal sealed class SimpleMouseBinding : InputBinding
{
  public static readonly DependencyProperty MouseActionProperty = DependencyProperty.Register(nameof (MouseAction), typeof (MouseAction), typeof (SimpleMouseBinding), (PropertyMetadata) new UIPropertyMetadata((object) MouseAction.None, new PropertyChangedCallback(SimpleMouseBinding.OnMouseActionPropertyChanged)));
  private bool _settingGesture;

  [TypeConverter(typeof (MouseGestureConverter))]
  [ValueSerializer(typeof (MouseGestureValueSerializer))]
  public override InputGesture Gesture
  {
    get => (InputGesture) (base.Gesture as MouseGesture);
    set
    {
      if (!(value is MouseGesture mouseGesture))
        return;
      base.Gesture = (InputGesture) mouseGesture;
      this.SynchronizePropertiesFromGesture(mouseGesture);
    }
  }

  public MouseAction MouseAction
  {
    get => (MouseAction) this.GetValue(SimpleMouseBinding.MouseActionProperty);
    set => this.SetValue(SimpleMouseBinding.MouseActionProperty, (object) value);
  }

  public SimpleMouseBinding()
  {
  }

  internal SimpleMouseBinding(ICommand command, MouseAction mouseAction)
    : this(command, new MouseGesture(mouseAction))
  {
  }

  public SimpleMouseBinding(ICommand command, MouseGesture gesture)
    : base(command, (InputGesture) gesture)
  {
    this.SynchronizePropertiesFromGesture(gesture);
  }

  private static void OnMouseActionPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((SimpleMouseBinding) d).SynchronizeGestureFromProperties((MouseAction) e.NewValue);
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new SimpleMouseBinding();

  private void SynchronizePropertiesFromGesture(MouseGesture mouseGesture)
  {
    if (this._settingGesture)
      return;
    this._settingGesture = true;
    try
    {
      this.MouseAction = mouseGesture.MouseAction;
    }
    finally
    {
      this._settingGesture = false;
    }
  }

  private void SynchronizeGestureFromProperties(MouseAction mouseAction)
  {
    if (this._settingGesture)
      return;
    this._settingGesture = true;
    try
    {
      if (this.Gesture == null)
        this.Gesture = (InputGesture) new MouseGesture(mouseAction);
      else
        ((MouseGesture) this.Gesture).MouseAction = mouseAction;
    }
    finally
    {
      this._settingGesture = false;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.KeyedRadioButton
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public class KeyedRadioButton : RadioButton
{
  public static readonly DependencyProperty KeyInGroupProperty = DependencyProperty.Register(nameof (KeyInGroup), typeof (object), typeof (KeyedRadioButton), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is KeyedRadioButton keyedRadioButton2) || object.Equals(a.NewValue, a.OldValue) || !keyedRadioButton2.IsLoaded)
      return;
    keyedRadioButton2.UpdateRadioButtonGroup();
  })));
  private static Func<RadioButton, Hashtable> _groupNameToElementsGetter;

  static KeyedRadioButton()
  {
    RadioButton.GroupNameProperty.OverrideMetadata(typeof (KeyedRadioButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(KeyedRadioButton.OnGroupNameChanged)));
  }

  public KeyedRadioButton() => this.Loaded += new RoutedEventHandler(this.KeyedRadioButton_Loaded);

  public object KeyInGroup
  {
    get => this.GetValue(KeyedRadioButton.KeyInGroupProperty);
    set => this.SetValue(KeyedRadioButton.KeyInGroupProperty, value);
  }

  private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is KeyedRadioButton keyedRadioButton) || object.Equals(e.NewValue, e.OldValue) || !keyedRadioButton.IsLoaded)
      return;
    keyedRadioButton.UpdateRadioButtonGroup();
  }

  private void UpdateRadioButtonGroup()
  {
    string groupName = this.GroupName;
    Hashtable hashtable = KeyedRadioButton.GetGroupNameToElements((RadioButton) this);
    if (!string.IsNullOrEmpty(groupName))
    {
      Visual navigationVisualRoot = this.GetKeyboardNavigationVisualRoot((DependencyObject) this);
      if (hashtable == null)
        hashtable = new Hashtable(1);
      lock (hashtable)
      {
        ArrayList arrayList = (ArrayList) hashtable[(object) groupName];
        int index = 0;
        while (index < arrayList.Count)
        {
          if (!(((WeakReference) arrayList[index]).Target is RadioButton target))
          {
            arrayList.RemoveAt(index);
          }
          else
          {
            if (target != this && navigationVisualRoot == this.GetKeyboardNavigationVisualRoot((DependencyObject) target))
            {
              if (this.KeyInGroup != null && target is KeyedRadioButton keyedRadioButton && object.Equals(keyedRadioButton.KeyInGroup, this.KeyInGroup))
              {
                keyedRadioButton.SetCurrentValue(ToggleButton.IsCheckedProperty, (object) this.IsChecked);
              }
              else
              {
                bool? isChecked1 = this.IsChecked;
                if (isChecked1.HasValue && isChecked1.GetValueOrDefault())
                {
                  bool? isChecked2 = target.IsChecked;
                  if (isChecked2.HasValue && isChecked2.GetValueOrDefault())
                    KeyedRadioButton.UncheckRadioButton(target);
                }
              }
            }
            ++index;
          }
        }
      }
    }
    else
    {
      DependencyObject parent = this.Parent;
      if (parent == null)
        return;
      foreach (object child in LogicalTreeHelper.GetChildren(parent))
      {
        if (child is RadioButton radioButton && radioButton != this && string.IsNullOrEmpty(radioButton.GroupName) && radioButton.IsChecked.GetValueOrDefault())
        {
          if (this.KeyInGroup != null && radioButton is KeyedRadioButton keyedRadioButton && object.Equals(keyedRadioButton.KeyInGroup, this.KeyInGroup))
          {
            keyedRadioButton.SetCurrentValue(ToggleButton.IsCheckedProperty, (object) this.IsChecked);
          }
          else
          {
            bool? isChecked3 = this.IsChecked;
            if (isChecked3.HasValue && isChecked3.GetValueOrDefault())
            {
              bool? isChecked4 = radioButton.IsChecked;
              if (isChecked4.HasValue && isChecked4.GetValueOrDefault())
                KeyedRadioButton.UncheckRadioButton(radioButton);
            }
          }
        }
      }
    }
  }

  private void KeyedRadioButton_Loaded(object sender, RoutedEventArgs e)
  {
    bool flag = true;
    if (this.GetBindingExpression(ToggleButton.IsCheckedProperty) == null && this.ReadLocalValue(ToggleButton.IsCheckedProperty) == DependencyProperty.UnsetValue)
      flag = false;
    if (flag)
    {
      this.UpdateRadioButtonGroup();
    }
    else
    {
      string groupName = this.GroupName;
      Hashtable hashtable = KeyedRadioButton.GetGroupNameToElements((RadioButton) this);
      if (string.IsNullOrEmpty(groupName))
        return;
      Visual navigationVisualRoot = this.GetKeyboardNavigationVisualRoot((DependencyObject) this);
      if (hashtable == null)
        hashtable = new Hashtable(1);
      lock (hashtable)
      {
        ArrayList arrayList = (ArrayList) hashtable[(object) groupName];
        int index = 0;
        while (index < arrayList.Count)
        {
          if (!(((WeakReference) arrayList[index]).Target is RadioButton target))
          {
            arrayList.RemoveAt(index);
          }
          else
          {
            if (target != this && navigationVisualRoot == this.GetKeyboardNavigationVisualRoot((DependencyObject) target) && this.KeyInGroup != null && target is KeyedRadioButton keyedRadioButton && object.Equals(keyedRadioButton.KeyInGroup, this.KeyInGroup))
              this.SetCurrentValue(ToggleButton.IsCheckedProperty, (object) keyedRadioButton.IsChecked);
            ++index;
          }
        }
      }
    }
  }

  protected override void OnChecked(RoutedEventArgs e)
  {
    if (this.IsLoaded)
      this.UpdateRadioButtonGroup();
    this.RaiseEvent(e);
  }

  protected override void OnUnchecked(RoutedEventArgs e)
  {
    if (this.IsLoaded)
      this.UpdateRadioButtonGroup();
    this.RaiseEvent(e);
  }

  private static void UncheckRadioButton(RadioButton radioButton)
  {
    radioButton?.SetCurrentValue(ToggleButton.IsCheckedProperty, (object) false);
  }

  private static Hashtable GetGroupNameToElements(RadioButton radioButton)
  {
    if (KeyedRadioButton._groupNameToElementsGetter == null)
      KeyedRadioButton._groupNameToElementsGetter = TypeHelper.CreateFieldOrPropertyGetter<RadioButton, Hashtable>("_groupNameToElements", BindingFlags.Static | BindingFlags.NonPublic);
    if (KeyedRadioButton._groupNameToElementsGetter == null)
      KeyedRadioButton._groupNameToElementsGetter = (Func<RadioButton, Hashtable>) (r => (Hashtable) null);
    return KeyedRadioButton._groupNameToElementsGetter(radioButton);
  }

  private Visual GetKeyboardNavigationVisualRoot(DependencyObject d)
  {
    switch (d)
    {
      case Visual _:
        PresentationSource presentationSource = PresentationSource.FromVisual((Visual) d);
        if (presentationSource != null)
          return presentationSource.RootVisual;
        break;
      case FrameworkContentElement frameworkContentElement:
        return this.GetKeyboardNavigationVisualRoot(frameworkContentElement.Parent);
    }
    return (Visual) null;
  }
}

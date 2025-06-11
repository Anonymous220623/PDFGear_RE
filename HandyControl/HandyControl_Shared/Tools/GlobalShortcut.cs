// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.GlobalShortcut
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Tools;

public class GlobalShortcut
{
  private static ObservableCollection<KeyBinding> KeyBindingCollection;
  private static readonly Dictionary<string, KeyBinding> CommandDic = new Dictionary<string, KeyBinding>();
  public static readonly DependencyProperty HostProperty = DependencyProperty.RegisterAttached("Host", typeof (bool), typeof (GlobalShortcut), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(GlobalShortcut.OnHostChanged)));
  public static readonly DependencyProperty KeyBindingsProperty = DependencyProperty.RegisterAttached("KeyBindings", typeof (ObservableCollection<KeyBinding>), typeof (GlobalShortcut), new PropertyMetadata((object) new ObservableCollection<KeyBinding>()));

  private static void KeyboardHook_KeyDown(object sender, KeyboardHookEventArgs e)
  {
    GlobalShortcut.HitTest(e.Key);
  }

  static GlobalShortcut()
  {
    KeyboardHook.KeyDown += new EventHandler<KeyboardHookEventArgs>(GlobalShortcut.KeyboardHook_KeyDown);
  }

  private static void HitTest(Key key)
  {
    if (GlobalShortcut.IsModifierKey(key))
      return;
    ModifierKeys modifiers = Keyboard.Modifiers;
    GlobalShortcut.ExecuteCommand(modifiers != ModifierKeys.None ? $"{modifiers.ToString()}, {key.ToString()}" : key.ToString());
  }

  private static void ExecuteCommand(string key)
  {
    if (string.IsNullOrEmpty(key) || !GlobalShortcut.CommandDic.ContainsKey(key))
      return;
    KeyBinding keyBinding = GlobalShortcut.CommandDic[key];
    ICommand command = keyBinding.Command;
    if (command == null || !command.CanExecute(keyBinding.CommandParameter))
      return;
    command.Execute(keyBinding.CommandParameter);
  }

  private static bool IsModifierKey(Key key)
  {
    return key == Key.LeftCtrl || key == Key.LeftAlt || key == Key.LeftShift || key == Key.LWin || key == Key.RightCtrl || key == Key.RightAlt || key == Key.RightShift || key == Key.RWin;
  }

  public static void Init(List<KeyBinding> list)
  {
    GlobalShortcut.CommandDic.Clear();
    if (list == null)
      return;
    GlobalShortcut.KeyBindingCollection = new ObservableCollection<KeyBinding>(list);
    if (GlobalShortcut.KeyBindingCollection.Count == 0)
      return;
    GlobalShortcut.AddKeyBindings((IEnumerable<KeyBinding>) GlobalShortcut.KeyBindingCollection);
    KeyboardHook.Start();
  }

  private static void AddKeyBindings(IEnumerable<KeyBinding> keyBindings)
  {
    foreach (KeyBinding keyBinding in keyBindings)
    {
      if (keyBinding.Key != Key.None)
      {
        if (keyBinding.Modifiers == ModifierKeys.None)
        {
          GlobalShortcut.CommandDic[keyBinding.Key.ToString()] = keyBinding;
        }
        else
        {
          string key = $"{keyBinding.Modifiers.ToString()}, {keyBinding.Key.ToString()}";
          GlobalShortcut.CommandDic[key] = keyBinding;
        }
      }
    }
  }

  private static void OnHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (DesignerHelper.IsInDesignMode)
      return;
    if (GlobalShortcut.KeyBindingCollection != null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GlobalShortcut.KeyBindingCollection.CollectionChanged -= GlobalShortcut.\u003C\u003EO.\u003C0\u003E__KeyBindingCollection_CollectionChanged ?? (GlobalShortcut.\u003C\u003EO.\u003C0\u003E__KeyBindingCollection_CollectionChanged = new NotifyCollectionChangedEventHandler(GlobalShortcut.KeyBindingCollection_CollectionChanged));
    }
    GlobalShortcut.KeyBindingCollection = GlobalShortcut.GetKeyBindings(d);
    if (GlobalShortcut.KeyBindingCollection == null)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    GlobalShortcut.KeyBindingCollection.CollectionChanged += GlobalShortcut.\u003C\u003EO.\u003C0\u003E__KeyBindingCollection_CollectionChanged ?? (GlobalShortcut.\u003C\u003EO.\u003C0\u003E__KeyBindingCollection_CollectionChanged = new NotifyCollectionChangedEventHandler(GlobalShortcut.KeyBindingCollection_CollectionChanged));
  }

  private static void KeyBindingCollection_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    GlobalShortcut.AddKeyBindings((IEnumerable<KeyBinding>) GlobalShortcut.KeyBindingCollection);
    KeyboardHook.Start();
  }

  public static void SetHost(DependencyObject element, bool value)
  {
    element.SetValue(GlobalShortcut.HostProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetHost(DependencyObject element)
  {
    return (bool) element.GetValue(GlobalShortcut.HostProperty);
  }

  public static void SetKeyBindings(
    DependencyObject element,
    ObservableCollection<KeyBinding> value)
  {
    element.SetValue(GlobalShortcut.KeyBindingsProperty, (object) value);
  }

  public static ObservableCollection<KeyBinding> GetKeyBindings(DependencyObject element)
  {
    return (ObservableCollection<KeyBinding>) element.GetValue(GlobalShortcut.KeyBindingsProperty);
  }
}

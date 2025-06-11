// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyItem
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Text;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public struct HotKeyItem : IEquatable<HotKeyItem>
{
  private readonly int virtualKey;
  private readonly ModifierKeys modifiers;

  public HotKeyItem(Key key)
    : this(key, ModifierKeys.None)
  {
  }

  public HotKeyItem(int virtualKey)
    : this(virtualKey, ModifierKeys.None)
  {
  }

  public HotKeyItem(Key key, ModifierKeys modifiers)
    : this(KeyInterop.VirtualKeyFromKey(key), modifiers)
  {
  }

  public HotKeyItem(int virtualKey, ModifierKeys modifiers)
  {
    this.virtualKey = KeyboardHelper.NormalizeKey(virtualKey);
    this.modifiers = modifiers;
  }

  public int VirtualKey => this.virtualKey;

  public Key Key => KeyInterop.KeyFromVirtualKey(this.VirtualKey);

  public ModifierKeys Modifiers => this.modifiers;

  public bool IsSupported
  {
    get
    {
      return !KeyboardHelper.IsModifierKey(this.VirtualKey) && KeyboardHelper.HasName(this.VirtualKey);
    }
  }

  public string[] GetKeyNames()
  {
    if (!this.IsSupported)
      return Array.Empty<string>();
    HotKeyItem.NameInlineStack keyNamesCore = this.GetKeyNamesCore();
    if (keyNamesCore.Count == 0)
      return Array.Empty<string>();
    string[] keyNames = new string[keyNamesCore.Count];
    for (int index = 0; index < keyNamesCore.Count; ++index)
      keyNames[index] = keyNamesCore[index];
    return keyNames;
  }

  private HotKeyItem.NameInlineStack GetKeyNamesCore()
  {
    if (!this.IsSupported)
      return new HotKeyItem.NameInlineStack();
    HotKeyItem.NameInlineStack keyNamesCore = new HotKeyItem.NameInlineStack();
    if ((this.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
      keyNamesCore.Push(KeyboardHelper.GetName(ModifierKeys.Control));
    if ((this.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
      keyNamesCore.Push(KeyboardHelper.GetName(ModifierKeys.Alt));
    if ((this.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
      keyNamesCore.Push(KeyboardHelper.GetName(ModifierKeys.Shift));
    if ((this.Modifiers & ModifierKeys.Windows) != ModifierKeys.None)
      keyNamesCore.Push(KeyboardHelper.GetName(ModifierKeys.Windows));
    keyNamesCore.Push(KeyboardHelper.GetName(this.VirtualKey));
    return keyNamesCore;
  }

  public override int GetHashCode()
  {
    return HashCode.Combine<int, ModifierKeys>(this.virtualKey, this.modifiers);
  }

  public override bool Equals(object obj) => obj is HotKeyItem other && this.Equals(other);

  public bool Equals(HotKeyItem other)
  {
    return this.virtualKey == other.virtualKey && this.modifiers == other.modifiers;
  }

  public static bool operator ==(HotKeyItem left, HotKeyItem right) => left.Equals(right);

  public static bool operator !=(HotKeyItem left, HotKeyItem right) => !left.Equals(right);

  public override string ToString() => this.ToString(HotKeyItem.FormatType.Normal);

  public string ToString(HotKeyItem.FormatType formatType)
  {
    if (!this.IsSupported)
      return string.Empty;
    HotKeyItem.NameInlineStack names = this.GetKeyNamesCore();
    if (names.Count == 0)
      return string.Empty;
    if (names.Count == 1)
      return names[0];
    StringBuilder stringBuilder = new StringBuilder();
    this.AppendToStringBuilderCore(in names, stringBuilder, formatType);
    return stringBuilder.ToString();
  }

  internal StringBuilder AppendToStringBuilder(
    StringBuilder stringBuilder,
    HotKeyItem.FormatType formatType)
  {
    if (!this.IsSupported)
      return stringBuilder;
    this.AppendToStringBuilderCore(this.GetKeyNamesCore(), stringBuilder, formatType);
    return stringBuilder;
  }

  private void AppendToStringBuilderCore(
    in HotKeyItem.NameInlineStack names,
    StringBuilder stringBuilder,
    HotKeyItem.FormatType formatType)
  {
    if (names.Count == 0)
      return;
    int index = 0;
    while (true)
    {
      int num1 = index;
      HotKeyItem.NameInlineStack nameInlineStack = names;
      int count = nameInlineStack.Count;
      if (num1 < count)
      {
        StringBuilder stringBuilder1 = stringBuilder;
        nameInlineStack = names;
        string str = nameInlineStack[index];
        stringBuilder1.Append(str);
        int num2 = index;
        nameInlineStack = names;
        int num3 = nameInlineStack.Count - 1;
        if (num2 < num3)
        {
          if (formatType == HotKeyItem.FormatType.Compact)
            stringBuilder.Append("+");
          else
            stringBuilder.Append(" + ");
        }
        ++index;
      }
      else
        break;
    }
  }

  public enum FormatType
  {
    Normal,
    Compact,
  }

  private struct NameInlineStack
  {
    public const int Capacity = 10;
    private int count;
    private string E0;
    private string E1;
    private string E2;
    private string E3;
    private string E4;
    private string E5;
    private string E6;
    private string E7;
    private string E8;
    private string E9;

    public int Count => this.count;

    public void Push(string value)
    {
      if (this.count >= 10)
        HotKeyItem.NameInlineStack.ThrowCountOutOfCapacityException();
      this[this.count++] = value;
    }

    public string Pop()
    {
      this.TryThrowIndexOutOfRangeException(this.count - 1);
      return this[--this.count];
    }

    public string Peek()
    {
      this.TryThrowIndexOutOfRangeException(this.count - 1);
      return this[this.count - 1];
    }

    public string this[int index]
    {
      get
      {
        this.TryThrowIndexOutOfRangeException(index);
        switch (index)
        {
          case 0:
            return this.E0;
          case 1:
            return this.E1;
          case 2:
            return this.E2;
          case 3:
            return this.E3;
          case 4:
            return this.E4;
          case 5:
            return this.E5;
          case 6:
            return this.E6;
          case 7:
            return this.E7;
          case 8:
            return this.E8;
          case 9:
            return this.E9;
          default:
            return (string) null;
        }
      }
      set
      {
        this.TryThrowIndexOutOfRangeException(index);
        switch (index)
        {
          case 0:
            this.E0 = value;
            break;
          case 1:
            this.E1 = value;
            break;
          case 2:
            this.E2 = value;
            break;
          case 3:
            this.E3 = value;
            break;
          case 4:
            this.E4 = value;
            break;
          case 5:
            this.E5 = value;
            break;
          case 6:
            this.E6 = value;
            break;
          case 7:
            this.E7 = value;
            break;
          case 8:
            this.E8 = value;
            break;
          case 9:
            this.E9 = value;
            break;
        }
      }
    }

    private static void ThrowCountOutOfCapacityException() => throw new IndexOutOfRangeException();

    private void TryThrowIndexOutOfRangeException(int index)
    {
      if (index >= this.count)
        throw new IndexOutOfRangeException();
    }
  }
}

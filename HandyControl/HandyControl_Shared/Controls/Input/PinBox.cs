// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PinBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Panel", Type = typeof (Panel))]
public class PinBox : Control
{
  private const string ElementPanel = "PART_Panel";
  private static readonly object MinLength = (object) 4;
  private Panel _panel;
  private int _inputIndex;
  private bool _changed;
  private bool _isInternalAction;
  private List<SecureString> _passwordList;
  private RoutedEventHandler _passwordBoxsGotFocusEventHandler;
  private RoutedEventHandler _passwordBoxsPasswordChangedEventHandler;
  public static readonly DependencyProperty PasswordCharProperty = System.Windows.Controls.PasswordBox.PasswordCharProperty.AddOwner(typeof (PinBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) '●'));
  public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof (Length), typeof (int), typeof (PinBox), new PropertyMetadata(PinBox.MinLength, new PropertyChangedCallback(PinBox.OnLengthChanged), new CoerceValueCallback(PinBox.CoerceLength)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosInt));
  public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof (ItemMargin), typeof (Thickness), typeof (PinBox), new PropertyMetadata((object) new Thickness()));
  public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (PinBox), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (PinBox), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (PinBox));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (PinBox));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (PinBox));
  public static readonly DependencyProperty IsSafeEnabledProperty = PasswordBox.IsSafeEnabledProperty.AddOwner(typeof (PinBox), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(PinBox.OnIsSafeEnabledChanged)));
  public static readonly DependencyProperty UnsafePasswordProperty = PasswordBox.UnsafePasswordProperty.AddOwner(typeof (PinBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(PinBox.OnUnsafePasswordChanged)));
  public static readonly RoutedEvent CompletedEvent = EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PinBox));

  public PinBox()
  {
    this.Loaded += new RoutedEventHandler(this.PinBox_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PinBox_Unloaded);
  }

  private void PinBox_Unloaded(object sender, RoutedEventArgs e)
  {
    this.RemoveHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, (Delegate) this._passwordBoxsPasswordChangedEventHandler);
    this.RemoveHandler(UIElement.GotFocusEvent, (Delegate) this._passwordBoxsGotFocusEventHandler);
    this.Loaded -= new RoutedEventHandler(this.PinBox_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.PinBox_Unloaded);
  }

  private void PinBox_Loaded(object sender, RoutedEventArgs e)
  {
    this._passwordBoxsPasswordChangedEventHandler = new RoutedEventHandler(this.PasswordBoxsPasswordChanged);
    this.AddHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, (Delegate) this._passwordBoxsPasswordChangedEventHandler);
    this._passwordBoxsGotFocusEventHandler = new RoutedEventHandler(this.PasswordBoxsGotFocus);
    this.AddHandler(UIElement.GotFocusEvent, (Delegate) this._passwordBoxsGotFocusEventHandler);
    this.FocusPasswordBox();
  }

  private void FocusPasswordBox()
  {
    if (!this.IsFocused || this._panel.Children.Count == 0 || this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().Any<System.Windows.Controls.PasswordBox>((Func<System.Windows.Controls.PasswordBox, bool>) (box => box.IsFocused)))
      return;
    FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) this._panel.Children[0]);
  }

  private void PasswordBoxsPasswordChanged(object sender, RoutedEventArgs e)
  {
    if (this._isInternalAction || !(e.OriginalSource is System.Windows.Controls.PasswordBox originalSource))
      return;
    if (!this.IsSafeEnabled)
      this.SetCurrentValue(PinBox.UnsafePasswordProperty, (object) this.Password);
    if (originalSource.Password.Length > 0)
    {
      if (++this._inputIndex >= this.Length)
      {
        this._inputIndex = this.Length - 1;
        if (!this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().All<System.Windows.Controls.PasswordBox>((Func<System.Windows.Controls.PasswordBox, bool>) (item => item.Password.Any<char>())))
          return;
        FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) null);
        Keyboard.ClearFocus();
        this.RaiseEvent(new RoutedEventArgs(PinBox.CompletedEvent, (object) this));
        return;
      }
    }
    else if (--this._inputIndex < 0)
    {
      this._inputIndex = 0;
      return;
    }
    this._changed = true;
    FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) this._panel.Children[this._inputIndex]);
  }

  private void PasswordBoxsGotFocus(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is System.Windows.Controls.PasswordBox originalSource))
      return;
    this._inputIndex = this._panel.Children.IndexOf((UIElement) originalSource);
    originalSource.SelectAll();
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyUp(e);
    if (e.Key == Key.Left)
    {
      if (--this._inputIndex < 0)
      {
        this._inputIndex = 0;
      }
      else
      {
        if (this._panel.Children[this._inputIndex] is System.Windows.Controls.PasswordBox child)
          child.SelectAll();
        FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) child);
      }
    }
    else
    {
      if (e.Key != Key.Right)
        return;
      if (++this._inputIndex >= this.Length)
      {
        this._inputIndex = this.Length - 1;
      }
      else
      {
        if (this._panel.Children[this._inputIndex] is System.Windows.Controls.PasswordBox child)
          child.SelectAll();
        FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) child);
      }
    }
  }

  protected override void OnPreviewKeyUp(KeyEventArgs e)
  {
    base.OnPreviewKeyUp(e);
    if (this._changed)
    {
      this._changed = false;
    }
    else
    {
      if (e.Key != Key.Delete && e.Key != Key.Back)
        return;
      if (this._panel.Children[this._inputIndex] is System.Windows.Controls.PasswordBox child)
      {
        this._isInternalAction = true;
        child.Clear();
        this._isInternalAction = false;
      }
      if (--this._inputIndex < 0)
        this._inputIndex = 0;
      else
        FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) this._panel.Children[this._inputIndex]);
    }
  }

  public string Password
  {
    get
    {
      return this._panel != null ? string.Join(string.Empty, this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().Select<System.Windows.Controls.PasswordBox, string>((Func<System.Windows.Controls.PasswordBox, string>) (item => item.Password))) : string.Empty;
    }
    set
    {
      if (this._panel == null)
      {
        this._passwordList = new List<SecureString>();
        if (value == null)
          value = string.Empty;
        foreach (char c in value)
        {
          SecureString secureString = new SecureString();
          secureString.AppendChar(c);
          this._passwordList.Add(secureString);
        }
      }
      else
      {
        this._isInternalAction = true;
        if (string.IsNullOrEmpty(value))
        {
          this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().Do<System.Windows.Controls.PasswordBox>((Action<System.Windows.Controls.PasswordBox>) (item => item.Clear()));
        }
        else
        {
          this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().Take<System.Windows.Controls.PasswordBox>(Math.Min(this.Length, value.Length)).DoWithIndex<System.Windows.Controls.PasswordBox>((Action<System.Windows.Controls.PasswordBox, int>) ((item, index) => item.Password = value[index].ToString()));
          this._panel.Children.OfType<System.Windows.Controls.PasswordBox>().Skip<System.Windows.Controls.PasswordBox>(value.Length).Take<System.Windows.Controls.PasswordBox>(this.Length - value.Length).Do<System.Windows.Controls.PasswordBox>((Action<System.Windows.Controls.PasswordBox>) (item => item.Clear()));
        }
        this._isInternalAction = false;
        if (this.IsSafeEnabled)
          return;
        this.SetCurrentValue(PinBox.UnsafePasswordProperty, (object) this.Password);
      }
    }
  }

  public char PasswordChar
  {
    get => (char) this.GetValue(PinBox.PasswordCharProperty);
    set => this.SetValue(PinBox.PasswordCharProperty, (object) value);
  }

  private static void OnLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((PinBox) d).UpdateItems();
  }

  private static object CoerceLength(DependencyObject d, object basevalue)
  {
    return (int) basevalue >= 4 ? basevalue : PinBox.MinLength;
  }

  public int Length
  {
    get => (int) this.GetValue(PinBox.LengthProperty);
    set => this.SetValue(PinBox.LengthProperty, (object) value);
  }

  public Thickness ItemMargin
  {
    get => (Thickness) this.GetValue(PinBox.ItemMarginProperty);
    set => this.SetValue(PinBox.ItemMarginProperty, (object) value);
  }

  public double ItemWidth
  {
    get => (double) this.GetValue(PinBox.ItemWidthProperty);
    set => this.SetValue(PinBox.ItemWidthProperty, (object) value);
  }

  public double ItemHeight
  {
    get => (double) this.GetValue(PinBox.ItemHeightProperty);
    set => this.SetValue(PinBox.ItemHeightProperty, (object) value);
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(PinBox.SelectionBrushProperty);
    set => this.SetValue(PinBox.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(PinBox.SelectionOpacityProperty);
    set => this.SetValue(PinBox.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(PinBox.CaretBrushProperty);
    set => this.SetValue(PinBox.CaretBrushProperty, (object) value);
  }

  private static void OnIsSafeEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((PinBox) d).OnIsSafeEnabledChanged((bool) e.NewValue);
  }

  private void OnIsSafeEnabledChanged(bool newValue)
  {
    if (this._panel == null)
      return;
    this.SetCurrentValue(PinBox.UnsafePasswordProperty, !newValue ? (object) this.Password : (object) string.Empty);
  }

  public bool IsSafeEnabled
  {
    get => (bool) this.GetValue(PinBox.IsSafeEnabledProperty);
    set => this.SetValue(PinBox.IsSafeEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnUnsafePasswordChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PinBox pinBox = (PinBox) d;
    if (pinBox.IsSafeEnabled)
      return;
    pinBox.Password = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
  }

  public string UnsafePassword
  {
    get => (string) this.GetValue(PinBox.UnsafePasswordProperty);
    set => this.SetValue(PinBox.UnsafePasswordProperty, (object) value);
  }

  public event RoutedEventHandler Completed
  {
    add => this.AddHandler(PinBox.CompletedEvent, (Delegate) value);
    remove => this.RemoveHandler(PinBox.CompletedEvent, (Delegate) value);
  }

  private void UpdateItems()
  {
    if (this._panel == null)
      return;
    this._panel.Children.Clear();
    int length = this.Length;
    for (int index = 0; index < length; ++index)
      this._panel.Children.Add((UIElement) this.CreatePasswordBox());
  }

  private System.Windows.Controls.PasswordBox CreatePasswordBox()
  {
    System.Windows.Controls.PasswordBox passwordBox = new System.Windows.Controls.PasswordBox();
    passwordBox.MaxLength = 1;
    passwordBox.HorizontalContentAlignment = HorizontalAlignment.Center;
    passwordBox.VerticalAlignment = VerticalAlignment.Center;
    passwordBox.Margin = this.ItemMargin;
    passwordBox.Width = this.ItemWidth;
    passwordBox.Height = this.ItemHeight;
    passwordBox.Padding = new Thickness();
    passwordBox.PasswordChar = this.PasswordChar;
    passwordBox.Foreground = this.Foreground;
    passwordBox.SetBinding(PinBox.SelectionBrushProperty, (BindingBase) new Binding(PinBox.SelectionBrushProperty.Name)
    {
      Source = (object) this
    });
    passwordBox.SetBinding(PinBox.SelectionOpacityProperty, (BindingBase) new Binding(PinBox.SelectionOpacityProperty.Name)
    {
      Source = (object) this
    });
    passwordBox.SetBinding(PinBox.CaretBrushProperty, (BindingBase) new Binding(PinBox.CaretBrushProperty.Name)
    {
      Source = (object) this
    });
    return passwordBox;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._panel = this.GetTemplateChild("PART_Panel") as Panel;
    if (this._panel == null)
      return;
    this.UpdateItems();
    int length = this.Length;
    if (this._passwordList != null && this._passwordList.Count == length && this._panel.Children.Count == length)
    {
      for (int index = 0; index < length; ++index)
      {
        SecureString password = this._passwordList[index];
        if (password.Length > 0)
        {
          IntPtr num = IntPtr.Zero;
          try
          {
            num = Marshal.SecureStringToGlobalAllocUnicode(password);
            if (this._panel.Children[index] is System.Windows.Controls.PasswordBox child)
              child.Password = Marshal.PtrToStringUni(num) ?? throw new InvalidOperationException();
          }
          finally
          {
            Marshal.ZeroFreeGlobalAllocUnicode(num);
            password.Clear();
          }
        }
      }
      this._passwordList.Clear();
    }
    this.OnIsSafeEnabledChanged(this.IsSafeEnabled);
  }
}

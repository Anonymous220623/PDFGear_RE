// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PasswordBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.ComponentModel;
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

[TemplatePart(Name = "PART_PasswordBox", Type = typeof (System.Windows.Controls.PasswordBox))]
[TemplatePart(Name = "PART_TextBox", Type = typeof (System.Windows.Controls.TextBox))]
public class PasswordBox : Control
{
  private const string ElementPasswordBox = "PART_PasswordBox";
  private const string ElementTextBox = "PART_TextBox";
  private SecureString _password;
  private System.Windows.Controls.TextBox _textBox;
  public static readonly DependencyProperty PasswordCharProperty = System.Windows.Controls.PasswordBox.PasswordCharProperty.AddOwner(typeof (PasswordBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) '●'));
  public static readonly DependencyProperty ShowEyeButtonProperty = DependencyProperty.Register(nameof (ShowEyeButton), typeof (bool), typeof (PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ShowPasswordProperty = DependencyProperty.Register(nameof (ShowPassword), typeof (bool), typeof (PasswordBox), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(PasswordBox.OnShowPasswordChanged)));
  public static readonly DependencyProperty IsSafeEnabledProperty = DependencyProperty.Register(nameof (IsSafeEnabled), typeof (bool), typeof (PasswordBox), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(PasswordBox.OnIsSafeEnabledChanged)));
  public static readonly DependencyProperty UnsafePasswordProperty = DependencyProperty.Register(nameof (UnsafePassword), typeof (string), typeof (PasswordBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(PasswordBox.OnUnsafePasswordChanged)));
  public static readonly DependencyProperty MaxLengthProperty = System.Windows.Controls.TextBox.MaxLengthProperty.AddOwner(typeof (PasswordBox));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (PasswordBox));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (PasswordBox));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (PasswordBox));
  public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof (PasswordBox));

  public char PasswordChar
  {
    get => (char) this.GetValue(PasswordBox.PasswordCharProperty);
    set => this.SetValue(PasswordBox.PasswordCharProperty, (object) value);
  }

  public bool ShowEyeButton
  {
    get => (bool) this.GetValue(PasswordBox.ShowEyeButtonProperty);
    set => this.SetValue(PasswordBox.ShowEyeButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowPassword
  {
    get => (bool) this.GetValue(PasswordBox.ShowPasswordProperty);
    set => this.SetValue(PasswordBox.ShowPasswordProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnIsSafeEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PasswordBox passwordBox = (PasswordBox) d;
    passwordBox.SetCurrentValue(PasswordBox.UnsafePasswordProperty, !(bool) e.NewValue ? (object) passwordBox.Password : (object) string.Empty);
  }

  public bool IsSafeEnabled
  {
    get => (bool) this.GetValue(PasswordBox.IsSafeEnabledProperty);
    set => this.SetValue(PasswordBox.IsSafeEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnUnsafePasswordChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PasswordBox passwordBox = (PasswordBox) d;
    if (passwordBox.IsSafeEnabled)
      return;
    passwordBox.Password = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
  }

  public string UnsafePassword
  {
    get => (string) this.GetValue(PasswordBox.UnsafePasswordProperty);
    set => this.SetValue(PasswordBox.UnsafePasswordProperty, (object) value);
  }

  public int MaxLength
  {
    get => (int) this.GetValue(PasswordBox.MaxLengthProperty);
    set => this.SetValue(PasswordBox.MaxLengthProperty, (object) value);
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(PasswordBox.SelectionBrushProperty);
    set => this.SetValue(PasswordBox.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(PasswordBox.SelectionOpacityProperty);
    set => this.SetValue(PasswordBox.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(PasswordBox.CaretBrushProperty);
    set => this.SetValue(PasswordBox.CaretBrushProperty, (object) value);
  }

  public bool IsSelectionActive
  {
    get
    {
      return this.ActualPasswordBox != null && (bool) this.ActualPasswordBox.GetValue(PasswordBox.IsSelectionActiveProperty);
    }
  }

  public PasswordBox()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) => this.Clear())));
  }

  public System.Windows.Controls.PasswordBox ActualPasswordBox { get; set; }

  [DefaultValue("")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public string Password
  {
    get
    {
      if (this.ShowEyeButton && this.ShowPassword)
        return this._textBox.Text;
      return this.ActualPasswordBox?.Password;
    }
    set
    {
      if (this.ActualPasswordBox == null)
      {
        this._password = new SecureString();
        if (value == null)
          value = string.Empty;
        foreach (char c in value)
          this._password.AppendChar(c);
      }
      else
      {
        if (object.Equals((object) this.ActualPasswordBox.Password, (object) value))
          return;
        this.ActualPasswordBox.Password = value;
      }
    }
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public SecureString SecurePassword => this.ActualPasswordBox?.SecurePassword;

  private static void OnShowPasswordChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PasswordBox passwordBox = (PasswordBox) d;
    if (!passwordBox.ShowEyeButton)
      return;
    if ((bool) e.NewValue)
    {
      passwordBox._textBox.Text = passwordBox.ActualPasswordBox.Password;
      passwordBox._textBox.Select(string.IsNullOrEmpty(passwordBox._textBox.Text) ? 0 : passwordBox._textBox.Text.Length, 0);
    }
    else
    {
      passwordBox.ActualPasswordBox.Password = passwordBox._textBox.Text;
      passwordBox._textBox.Clear();
    }
  }

  public override void OnApplyTemplate()
  {
    if (this.ActualPasswordBox != null)
      this.ActualPasswordBox.PasswordChanged -= new RoutedEventHandler(this.PasswordBox_PasswordChanged);
    if (this._textBox != null)
      this._textBox.TextChanged -= new TextChangedEventHandler(this.TextBox_TextChanged);
    base.OnApplyTemplate();
    this.ActualPasswordBox = this.GetTemplateChild("PART_PasswordBox") as System.Windows.Controls.PasswordBox;
    this._textBox = this.GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
    if (this.ActualPasswordBox != null)
    {
      this.ActualPasswordBox.PasswordChanged += new RoutedEventHandler(this.PasswordBox_PasswordChanged);
      this.ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.MaxLengthProperty, (BindingBase) new Binding(PasswordBox.MaxLengthProperty.Name)
      {
        Source = (object) this
      });
      this.ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.SelectionBrushProperty, (BindingBase) new Binding(PasswordBox.SelectionBrushProperty.Name)
      {
        Source = (object) this
      });
      this.ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.SelectionOpacityProperty, (BindingBase) new Binding(PasswordBox.SelectionOpacityProperty.Name)
      {
        Source = (object) this
      });
      this.ActualPasswordBox.SetBinding(System.Windows.Controls.PasswordBox.CaretBrushProperty, (BindingBase) new Binding(PasswordBox.CaretBrushProperty.Name)
      {
        Source = (object) this
      });
      SecureString password = this._password;
      if (password != null && password.Length > 0)
      {
        IntPtr num = IntPtr.Zero;
        try
        {
          num = Marshal.SecureStringToGlobalAllocUnicode(this._password);
          System.Windows.Controls.PasswordBox actualPasswordBox = this.ActualPasswordBox;
          actualPasswordBox.Password = Marshal.PtrToStringUni(num) ?? throw new InvalidOperationException();
        }
        finally
        {
          Marshal.ZeroFreeGlobalAllocUnicode(num);
          this._password.Clear();
        }
      }
    }
    if (this._textBox == null)
      return;
    this._textBox.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
  }

  public void Paste()
  {
    this.ActualPasswordBox.Paste();
    if (!this.ShowEyeButton || !this.ShowPassword)
      return;
    this._textBox.Text = this.ActualPasswordBox.Password;
  }

  public void SelectAll()
  {
    this.ActualPasswordBox.SelectAll();
    if (!this.ShowEyeButton || !this.ShowPassword)
      return;
    this._textBox.SelectAll();
  }

  public void Clear()
  {
    this.ActualPasswordBox.Clear();
    this._textBox.Clear();
  }

  private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
  {
    if (this.IsSafeEnabled)
      return;
    this.SetCurrentValue(PasswordBox.UnsafePasswordProperty, (object) this.ActualPasswordBox.Password);
    if (!this.ShowPassword)
      return;
    this._textBox.Text = this.ActualPasswordBox.Password;
  }

  private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (this.IsSafeEnabled || !this.ShowPassword)
      return;
    this.Password = this._textBox.Text;
    this.SetCurrentValue(PasswordBox.UnsafePasswordProperty, (object) this.Password);
  }
}

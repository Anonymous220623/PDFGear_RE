// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageEditor.PageRangeTextBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.PageEditor;

public partial class PageRangeTextBox : TextBox
{
  private static HashSet<Key> pageRangeKeys;
  protected static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasError), typeof (bool), typeof (PageRangeTextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(PageRangeTextBox.OnHasErrorPropertyChanged)));
  public static readonly DependencyProperty HasErrorProperty = PageRangeTextBox.HasErrorPropertyKey.DependencyProperty;
  protected static readonly DependencyPropertyKey PageIndexesPropertyKey = DependencyProperty.RegisterReadOnly(nameof (PageIndexes), typeof (IReadOnlyList<int>), typeof (PageRangeTextBox), new PropertyMetadata((object) Array.Empty<int>()));
  public static readonly DependencyProperty PageIndexesProperty = PageRangeTextBox.PageIndexesPropertyKey.DependencyProperty;

  private static HashSet<Key> PageRangeKeys
  {
    get
    {
      PageRangeTextBox.InitPageRangeKeys();
      return PageRangeTextBox.pageRangeKeys;
    }
  }

  static PageRangeTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PageRangeTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PageRangeTextBox)));
  }

  public PageRangeTextBox() => InputMethod.SetIsInputMethodEnabled((DependencyObject) this, false);

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (PageRangeTextBox.PageRangeKeys.Contains(e.Key))
      base.OnPreviewKeyDown(e);
    else
      e.Handled = true;
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    base.OnKeyDown(e);
    if (e.Key != Key.Return && e.Key != Key.Escape)
      return;
    Keyboard.ClearFocus();
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    this.HasError = false;
    this.PageIndexes = (IReadOnlyList<int>) Array.Empty<int>();
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    this.UpdateState();
    e.Handled = this.HasError;
    base.OnLostFocus(e);
  }

  protected override void OnTextChanged(TextChangedEventArgs e)
  {
    if (!this.IsFocused)
      this.UpdateState();
    base.OnTextChanged(e);
  }

  private void UpdateState()
  {
    string text = this.Text;
    if (string.IsNullOrWhiteSpace(text))
      return;
    int[] pageIndexes;
    int errorCharIndex;
    if (PdfObjectExtensions.TryParsePageRange(text, out pageIndexes, out errorCharIndex))
    {
      this.HasError = false;
      this.PageIndexes = (IReadOnlyList<int>) pageIndexes;
    }
    else
    {
      if (errorCharIndex == -1)
        this.SelectAll();
      else
        this.Select(errorCharIndex, text.Length - errorCharIndex);
      this.HasError = true;
    }
  }

  public bool HasError
  {
    get => (bool) this.GetValue(PageRangeTextBox.HasErrorProperty);
    protected set => this.SetValue(PageRangeTextBox.HasErrorPropertyKey, (object) value);
  }

  private static void OnHasErrorPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PageRangeTextBox sender))
      return;
    EventHandler hasErrorChanged = sender.HasErrorChanged;
    if (hasErrorChanged == null)
      return;
    hasErrorChanged((object) sender, EventArgs.Empty);
  }

  public IReadOnlyList<int> PageIndexes
  {
    get => (IReadOnlyList<int>) this.GetValue(PageRangeTextBox.PageIndexesProperty);
    protected set => this.SetValue(PageRangeTextBox.PageIndexesPropertyKey, (object) value);
  }

  public event EventHandler HasErrorChanged;

  private static void InitPageRangeKeys()
  {
    if (PageRangeTextBox.pageRangeKeys != null)
      return;
    lock (typeof (PageRangeTextBox))
    {
      if (PageRangeTextBox.pageRangeKeys != null)
        return;
      PageRangeTextBox.pageRangeKeys = new HashSet<Key>()
      {
        Key.Space,
        Key.Return,
        Key.Escape,
        Key.Left,
        Key.Up,
        Key.Right,
        Key.Down,
        Key.Tab,
        Key.Subtract,
        Key.OemMinus,
        Key.OemComma,
        Key.Back,
        Key.Delete
      };
      for (Key key = Key.D0; key <= Key.D9; ++key)
        PageRangeTextBox.pageRangeKeys.Add(key);
      for (Key key = Key.NumPad0; key <= Key.NumPad9; ++key)
        PageRangeTextBox.pageRangeKeys.Add(key);
    }
  }
}

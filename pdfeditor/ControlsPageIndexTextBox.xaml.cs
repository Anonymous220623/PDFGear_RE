// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageIndexTextBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls;

public partial class PageIndexTextBox : Control
{
  private Run PageCountRun;
  private TextBoxEditBehavior pageIndexTextBoxBehavior;
  private TextBox _PageIndexTextBox;
  private TextBlock pageCountTextBlock;
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PageIndexTextBox), new PropertyMetadata((object) 0, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PageIndexTextBox pageIndexTextBox2) || object.Equals(a.NewValue, a.OldValue))
      return;
    pageIndexTextBox2.UpdatePageIndex();
  })));
  public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(nameof (PageCount), typeof (int), typeof (PageIndexTextBox), new PropertyMetadata((object) 0, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PageIndexTextBox pageIndexTextBox4) || object.Equals(a.NewValue, a.OldValue))
      return;
    pageIndexTextBox4.UpdatePageCount();
  })));

  static PageIndexTextBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PageIndexTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PageIndexTextBox)));
  }

  private TextBoxEditBehavior PageIndexTextBoxBehavior
  {
    get => this.pageIndexTextBoxBehavior;
    set
    {
      if (this.pageIndexTextBoxBehavior == value)
        return;
      if (this.pageIndexTextBoxBehavior != null)
        this.pageIndexTextBoxBehavior.TextChanged -= new EventHandler(this.PageIndexTextBoxBehavior_TextChanged);
      this.pageIndexTextBoxBehavior = value;
      if (this.pageIndexTextBoxBehavior != null)
        this.pageIndexTextBoxBehavior.TextChanged += new EventHandler(this.PageIndexTextBoxBehavior_TextChanged);
      this.UpdatePageIndex();
    }
  }

  private TextBlock PageCountTextBlock
  {
    get => this.pageCountTextBlock;
    set
    {
      if (this.pageCountTextBlock == value)
        return;
      if (this.pageCountTextBlock != null)
        this.pageCountTextBlock.SizeChanged += new SizeChangedEventHandler(this.PageCountTextBlock_SizeChanged);
      this.pageCountTextBlock = value;
      if (this.pageCountTextBlock != null)
        this.pageCountTextBlock.SizeChanged += new SizeChangedEventHandler(this.PageCountTextBlock_SizeChanged);
      this.UpdateTextBoxSize();
    }
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PageIndexTextBox.PageIndexProperty);
    set => this.SetValue(PageIndexTextBox.PageIndexProperty, (object) value);
  }

  public int PageCount
  {
    get => (int) this.GetValue(PageIndexTextBox.PageCountProperty);
    set => this.SetValue(PageIndexTextBox.PageCountProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.PageCountRun = this.GetTemplateChild("PageCountRun") as Run;
    this.PageIndexTextBoxBehavior = this.GetTemplateChild("PageIndexTextBoxBehavior") as TextBoxEditBehavior;
    this._PageIndexTextBox = this.GetTemplateChild(nameof (PageIndexTextBox)) as TextBox;
    this.PageCountTextBlock = this.GetTemplateChild("PageCountTextBlock") as TextBlock;
    this.UpdatePageCount();
    this.UpdateTextBoxSize();
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    if (this._PageIndexTextBox == null || e.Source == this._PageIndexTextBox)
      return;
    this._PageIndexTextBox.Focus();
    Keyboard.Focus((IInputElement) this._PageIndexTextBox);
    this._PageIndexTextBox.SelectAll();
  }

  private void PageIndexTextBoxBehavior_TextChanged(object sender, EventArgs e)
  {
    int result;
    if (int.TryParse(this.PageIndexTextBoxBehavior.Text, out result))
      this.PageIndex = result;
    this.UpdatePageIndex();
  }

  private void PageCountTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateTextBoxSize();
  }

  private void UpdatePageCount()
  {
    if (this.PageCountRun == null)
      return;
    this.PageCountRun.Text = $"{this.PageCount}";
  }

  private void UpdatePageIndex()
  {
    if (this.PageIndexTextBoxBehavior == null)
      return;
    this.PageIndexTextBoxBehavior.Text = $"{this.PageIndex}";
  }

  private void UpdateTextBoxSize()
  {
    if (this.PageCountTextBlock == null || this._PageIndexTextBox == null)
      return;
    this._PageIndexTextBox.MaxWidth = this.PageCountTextBlock.ActualWidth;
  }
}

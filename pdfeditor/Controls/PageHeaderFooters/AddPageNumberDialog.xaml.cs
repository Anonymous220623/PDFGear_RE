// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.AddPageNumberDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using Patagames.Pdf.Net;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public partial class AddPageNumberDialog : Window, IComponentConnector
{
  private readonly PdfDocument pdfDocument;
  private readonly string initFormat;
  internal NumberBox PageNumberOffsetBox;
  internal ComboBox StyleComboBox;
  private bool _contentLoaded;

  public AddPageNumberDialog(PdfDocument pdfDocument, string format, int offset)
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.PageNumberDialog_Loaded);
    this.pdfDocument = pdfDocument;
    this.PageNumberOffsetBox.Maximum = (double) int.MaxValue;
    this.PageNumberOffsetBox.Value = (double) offset;
    this.initFormat = format;
  }

  private void PageNumberDialog_Loaded(object sender, RoutedEventArgs e)
  {
    this.StyleComboBox.ItemsSource = (IEnumerable) PagePlaceholderFormatter.AllSupportedPageNumberFormats.ToList<string>();
    if (string.IsNullOrEmpty(this.initFormat) || !PagePlaceholderFormatter.AllSupportedPageNumberFormats.Contains<string>(this.initFormat))
      this.StyleComboBox.SelectedIndex = 0;
    else
      this.StyleComboBox.SelectedItem = (object) this.initFormat;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (!this.DialogResult.GetValueOrDefault())
      return;
    this.PageNumberFormats = (string) this.StyleComboBox.SelectedItem;
    this.PageNumberOffset = (int) (this.PageNumberOffsetBox.Value + 0.5);
  }

  public string PageNumberFormats { get; private set; }

  public int PageNumberOffset { get; private set; }

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    Keyboard.ClearFocus();
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.DialogResult = new bool?(true)));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageheaderfooters/addpagenumberdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.PageNumberOffsetBox = (NumberBox) target;
        break;
      case 2:
        this.StyleComboBox = (ComboBox) target;
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}

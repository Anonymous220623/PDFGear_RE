// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Signature.SignatureApplyPageWin
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.Controls.PageEditor;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Signature;

public partial class SignatureApplyPageWin : Window, IComponentConnector
{
  public int[] ApplyPageIndex;
  private int CurrentPageIdx;
  internal RadioButton AllPagesRadioButton;
  internal RadioButton SelectedPagesRadioButton;
  internal PageRangeTextBox RangeBox;
  internal ComboBox applyToComboBox;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  public SignatureApplyPageWin(int currentPageIndex)
  {
    this.InitializeComponent();
    this.CurrentPageIdx = currentPageIndex;
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  private void btnOk_Click(object sender, RoutedEventArgs e)
  {
    int[] source = this.GetImportPageRange();
    if (source == null)
      return;
    if (!((IEnumerable<int>) source).Contains<int>(this.CurrentPageIdx))
    {
      List<int> list = ((IEnumerable<int>) source).ToList<int>();
      list.Add(this.CurrentPageIdx);
      source = list.ToArray();
    }
    this.ApplyPageIndex = source;
    this.DialogResult = new bool?(true);
  }

  private bool CheckPageRange()
  {
    PdfDocument document = this.VM.Document;
    int[] array = this.RangeBox.PageIndexes.ToArray<int>();
    bool? isChecked = this.AllPagesRadioButton.IsChecked;
    bool flag = false;
    if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue) || array.Length != 0)
      return true;
    int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
    return false;
  }

  private int[] GetImportPageRange()
  {
    PdfDocument doc = this.VM.Document;
    int[] array;
    if (this.AllPagesRadioButton.IsChecked.GetValueOrDefault())
    {
      if (doc.Pages.Count == 0)
        return (int[]) null;
      array = Enumerable.Range(0, doc.Pages.Count).ToArray<int>();
    }
    else
      array = this.RangeBox.PageIndexes.ToArray<int>();
    if (((IEnumerable<int>) array).Any<int>((Func<int, bool>) (c => c < 0 || c >= doc.Pages.Count)))
    {
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
      return (int[]) null;
    }
    if (this.applyToComboBox.SelectedIndex == 1)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
    else if (this.applyToComboBox.SelectedIndex == 2)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
    if (array.Length != 0)
      return array;
    int num1 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
    return (int[]) null;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/signature/signatureapplypagewin.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.AllPagesRadioButton = (RadioButton) target;
        break;
      case 2:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 3:
        this.RangeBox = (PageRangeTextBox) target;
        break;
      case 4:
        this.applyToComboBox = (ComboBox) target;
        break;
      case 5:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 6:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}

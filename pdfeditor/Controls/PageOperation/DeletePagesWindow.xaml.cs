// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageOperation.DeletePagesWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
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
namespace pdfeditor.Controls.PageOperation;

public partial class DeletePagesWindow : Window, IComponentConnector
{
  private MainViewModel vm;
  private int[] Pages;
  private string Range;
  private int[] origalPages;
  private DeletePagesWindow.PageOpeations pageOpeations;
  internal RadioButton SelectedPages;
  internal TextBlock range;
  internal RadioButton AllPages;
  internal RadioButton CustomPages;
  internal TextBox CustomTextBox;
  internal TextBlock CustomTextBolck;
  internal ComboBox SubsetComboBox;
  internal ComboBoxItem AllpagesSubset;
  internal ComboBoxItem EvenpagesSubset;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public DeletePagesWindow(MainViewModel mainViewModel, int[] pages)
  {
    this.InitializeComponent();
    this.vm = mainViewModel;
    this.Pages = pages;
    this.origalPages = pages;
    this.SelectedPages.IsChecked = new bool?(true);
    this.Range = ((IEnumerable<int>) pages).ConvertToRange();
    if (this.Range.Length >= 30)
    {
      string[] strArray = this.Range.Split(',');
      this.range.Text = $"{pages.Length}{pdfeditor.Properties.Resources.PageWinSelectedPages.Replace("\"XXX\"", " ")} ({strArray[0]}, {strArray[1]}, {strArray[2]}, {strArray[3]}......{strArray[strArray.Length - 2]}, {strArray[strArray.Length - 1]})";
    }
    else
      this.range.Text = $" {pages.Length}{pdfeditor.Properties.Resources.PageWinSelectedPages.Replace("\"XXX\"", " ")} ({this.Range.Replace(",", ", ")})";
    this.pageOpeations = DeletePagesWindow.PageOpeations.SelectedPages;
    this.InitMenu();
  }

  public DeletePagesWindow(MainViewModel mainViewModel)
  {
    this.InitializeComponent();
    this.vm = mainViewModel;
    this.SelectedPages.IsEnabled = false;
    this.CustomPages.IsChecked = new bool?(true);
    this.pageOpeations = DeletePagesWindow.PageOpeations.CustomPages;
    this.CustomTextBox.IsEnabled = true;
    this.CustomTextBox.Focus();
    this.InitMenu();
  }

  private void SelectedPages_Click(object sender, RoutedEventArgs e)
  {
    RadioButton radioButton = sender as RadioButton;
    if (!radioButton.IsChecked.Value)
      return;
    switch (radioButton.Name)
    {
      case "SelectedPages":
        this.CustomTextBox.IsEnabled = false;
        this.pageOpeations = DeletePagesWindow.PageOpeations.SelectedPages;
        this.AllpagesSubset.Visibility = Visibility.Visible;
        this.AllpagesSubset.IsSelected = true;
        break;
      case "AllPages":
        this.CustomTextBox.IsEnabled = false;
        this.pageOpeations = DeletePagesWindow.PageOpeations.AllPages;
        this.AllpagesSubset.Visibility = Visibility.Collapsed;
        this.EvenpagesSubset.IsSelected = true;
        break;
      case "CustomPages":
        this.CustomTextBox.IsEnabled = true;
        this.CustomTextBox.Focus();
        this.pageOpeations = DeletePagesWindow.PageOpeations.CustomPages;
        this.AllpagesSubset.Visibility = Visibility.Visible;
        this.AllpagesSubset.IsSelected = true;
        break;
      default:
        this.pageOpeations = DeletePagesWindow.PageOpeations.AllPages;
        break;
    }
  }

  private void InitMenu()
  {
    this.btnOk.Click += (RoutedEventHandler) (async (o, e) =>
    {
      DeletePagesWindow deletePagesWindow1 = this;
      DeletePagesWindow deletePagesWindow = deletePagesWindow1;
      if (!deletePagesWindow1.Check())
        return;
      ComboBoxItem selectedItem = deletePagesWindow1.SubsetComboBox.SelectedItem as ComboBoxItem;
      if (!deletePagesWindow1.OddEvenRange(selectedItem.Content.ToString()))
        return;
      int[] pageIndexes;
      PdfObjectExtensions.TryParsePageRange(deletePagesWindow1.Range, out pageIndexes, out int _);
      if (deletePagesWindow1.vm.Document.Pages.Count - pageIndexes.Length <= 0)
      {
        int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.DeletePageCheckMsg, UtilManager.GetProductName());
      }
      else
      {
        if (pageIndexes.Length == 1)
        {
          if (ModernMessageBox.Show(pdfeditor.Properties.Resources.DeleteSinglePageAskMsg, "PDFgear", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        }
        else if (ModernMessageBox.Show(pdfeditor.Properties.Resources.DeletePageAskMsg, "PDFgear", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
          return;
        PdfDocument tmpDoc = PdfDocument.CreateNew();
        tmpDoc.Pages.ImportPages(deletePagesWindow1.vm.Document, deletePagesWindow1.Range, 0);
        int[] arr;
        ((IEnumerable<int>) pageIndexes).ConvertToRange(out arr);
        PageDisposeHelper.TryFixResource(deletePagesWindow1.vm.Document, ((IEnumerable<int>) pageIndexes).Min(), ((IEnumerable<int>) pageIndexes).Max());
        if (!((IEnumerable<int>) arr).Contains<int>(deletePagesWindow1.vm.CurrnetPageIndex - 1))
          deletePagesWindow1.vm.LastViewPage = deletePagesWindow1.vm.Document.Pages[deletePagesWindow1.vm.CurrnetPageIndex - 1];
        else if (((IEnumerable<int>) arr).Count<int>() == 1)
        {
          if (deletePagesWindow1.vm.CurrnetPageIndex == deletePagesWindow1.vm.Document.Pages.Count<PdfPage>())
          {
            int index = Math.Max(0, deletePagesWindow1.vm.CurrnetPageIndex - 2);
            deletePagesWindow1.vm.LastViewPage = deletePagesWindow1.vm.Document.Pages[index];
          }
          else
            deletePagesWindow1.vm.LastViewPage = deletePagesWindow1.vm.Document.Pages[deletePagesWindow1.vm.CurrnetPageIndex];
        }
        else
          deletePagesWindow1.vm.LastViewPage = (PdfPage) null;
        for (int index = arr.Length - 1; index >= 0; --index)
          deletePagesWindow1.vm.Document.Pages.DeleteAt(arr[index]);
        PDFKit.PdfControl pdfControl1 = PDFKit.PdfControl.GetPdfControl(deletePagesWindow1.vm.Document);
        if (pdfControl1 != null && pdfControl1.Document != null)
          pdfControl1.UpdateDocLayout();
        deletePagesWindow1.vm.UpdateDocumentCore();
        deletePagesWindow1.Close();
        await deletePagesWindow1.vm.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
        {
          for (int index = 0; index < arr.Length; ++index)
            doc.Pages.ImportPages(tmpDoc, $"{index + 1}", arr[index]);
          PDFKit.PdfControl pdfControl2 = PDFKit.PdfControl.GetPdfControl(doc);
          if (pdfControl2 != null && pdfControl2.Document != null)
            pdfControl2.UpdateDocLayout();
          deletePagesWindow.vm.UpdateDocumentCore();
        }), (Action<PdfDocument>) (doc =>
        {
          for (int index = arr.Length - 1; index >= 0; --index)
            doc.Pages.DeleteAt(arr[index]);
          PDFKit.PdfControl pdfControl3 = PDFKit.PdfControl.GetPdfControl(doc);
          if (pdfControl3 != null && pdfControl3.Document != null)
            pdfControl3.UpdateDocLayout();
          deletePagesWindow.vm.UpdateDocumentCore();
        }));
      }
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) => this.Close());
  }

  private bool OddEvenRange(string ranges)
  {
    List<int> pageIndexes = new List<int>();
    if (ranges == pdfeditor.Properties.Resources.SelectPageAllEvenPagesItem)
    {
      for (int index = 0; index < this.Pages.Length; ++index)
      {
        if ((this.Pages[index] + 1) % 2 == 0)
          pageIndexes.Add(this.Pages[index]);
      }
      pageIndexes.ToArray();
      this.Range = pageIndexes.ConvertToRange();
    }
    else if (ranges == pdfeditor.Properties.Resources.SelectPageAllOddPagesItem)
    {
      for (int index = 0; index < this.Pages.Length; ++index)
      {
        if ((this.Pages[index] + 1) % 2 != 0)
          pageIndexes.Add(this.Pages[index]);
      }
      pageIndexes.ToArray();
      this.Range = pageIndexes.ConvertToRange();
    }
    else
    {
      pageIndexes = ((IEnumerable<int>) this.Pages).Select<int, int>((Func<int, int>) (i => i)).ToList<int>();
      this.Range = ((IEnumerable<int>) this.Pages).ConvertToRange();
    }
    if (pageIndexes.Count > 0)
      return true;
    int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError);
    return false;
  }

  private bool Check()
  {
    try
    {
      if (this.pageOpeations == DeletePagesWindow.PageOpeations.AllPages)
      {
        int count = this.vm.Document.Pages.Count;
        List<int> intList = new List<int>();
        for (int index = 0; index < count; ++index)
          intList.Add(index);
        this.Pages = intList.ToArray();
      }
      else if (this.pageOpeations == DeletePagesWindow.PageOpeations.CustomPages)
      {
        int[] pageIndexes;
        PdfObjectExtensions.TryParsePageRange(this.CustomTextBox.Text, out pageIndexes, out int _);
        if (pageIndexes != null)
        {
          this.Pages = pageIndexes;
        }
        else
        {
          int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError);
          return false;
        }
      }
      else
        this.Pages = this.origalPages;
      if (((IEnumerable<int>) this.Pages).Max() < this.vm.Document.Pages.Count && ((IEnumerable<int>) this.Pages).Min() >= 0)
        return true;
      int num1 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError);
      return false;
    }
    catch (Exception ex)
    {
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError);
      return false;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageoperation/deletepageswindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.SelectedPages = (RadioButton) target;
        this.SelectedPages.Click += new RoutedEventHandler(this.SelectedPages_Click);
        break;
      case 2:
        this.range = (TextBlock) target;
        break;
      case 3:
        this.AllPages = (RadioButton) target;
        this.AllPages.Click += new RoutedEventHandler(this.SelectedPages_Click);
        break;
      case 4:
        this.CustomPages = (RadioButton) target;
        this.CustomPages.Click += new RoutedEventHandler(this.SelectedPages_Click);
        break;
      case 5:
        this.CustomTextBox = (TextBox) target;
        break;
      case 6:
        this.CustomTextBolck = (TextBlock) target;
        break;
      case 7:
        this.SubsetComboBox = (ComboBox) target;
        break;
      case 8:
        this.AllpagesSubset = (ComboBoxItem) target;
        break;
      case 9:
        this.EvenpagesSubset = (ComboBoxItem) target;
        break;
      case 10:
        this.btnCancel = (Button) target;
        break;
      case 11:
        this.btnOk = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private enum PageOpeations
  {
    SelectedPages,
    AllPages,
    CustomPages,
  }
}

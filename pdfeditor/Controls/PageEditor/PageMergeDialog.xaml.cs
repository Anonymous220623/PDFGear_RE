// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageEditor.PageMergeDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using CommomLib.Models;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Exceptions;
using pdfeditor.Utils;
using pdfeditor.Views;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageEditor;

public partial class PageMergeDialog : Window, IComponentConnector
{
  private string sourceFile;
  private readonly PdfDocument doc;
  private int firstSelectedPage = -1;
  private int lastSelectedPage = -1;
  private string password;
  private InsertSourceFileType sourceFileType;
  internal Grid ContentGrid;
  internal OpenFileTextBox LocationTextBox;
  internal RadioButton AllPagesRadioButton;
  internal RadioButton SelectedPagesRadioButton;
  internal PageRangeTextBox RangeBox;
  internal ComboBox PageRangeTypeComboBox;
  internal RadioButton BeginingRadioButton;
  internal RadioButton EndRadioButton;
  internal RadioButton PageRadioButton;
  internal TextBox PageindexNumbox;
  internal TextBlock PageNumber;
  internal ComboBox InsertPosition;
  internal Button btnOk;
  internal Border ProcessingDismissBorder;
  internal ProgressRing ProcessingRing;
  private bool _contentLoaded;

  public PageMergeDialog(
    string sourceFile,
    PdfDocument doc,
    IEnumerable<int> selectedPages,
    bool fromSinglePageCmd = false,
    InsertSourceFileType type = InsertSourceFileType.PDF)
  {
    PageMergeDialog pageMergeDialog = this;
    this.InitializeComponent();
    this.sourceFile = sourceFile;
    this.doc = doc;
    this.sourceFileType = type;
    if (!string.IsNullOrEmpty(sourceFile))
    {
      try
      {
        this.LocationTextBox.Text = new FileInfo(sourceFile).FullName;
      }
      catch
      {
      }
    }
    List<int> list = selectedPages.ToList<int>();
    list.Sort();
    this.PageindexNumbox.Text = list.Count <= 0 ? "" : selectedPages.ConvertToRange();
    if (doc != null)
      this.PageNumber.Text = doc.Pages.Count.ToString();
    if (doc != null)
      this.PageNumber.Text = doc.Pages.Count.ToString();
    this.InitInsertPositionRadioButtons(selectedPages, fromSinglePageCmd);
    if (this.sourceFileType != InsertSourceFileType.PDF)
    {
      this.SetProcessingState(true);
      if (this.sourceFileType != InsertSourceFileType.Doc)
        return;
      Task.Run((Func<Task>) (async () =>
      {
        CancellationToken token;
        string tempFile = await CommomLib.ConvertUtils.ConvertUtils.GetTempPDF(pageMergeDialog.sourceFileType, sourceFile, token);
        if (!string.IsNullOrWhiteSpace(tempFile))
        {
          pageMergeDialog.Dispatcher.Invoke((Action) (() =>
          {
            pageMergeDialog.SetProcessingState(false);
            try
            {
              pageMergeDialog.sourceFile = tempFile;
              sourceFile = tempFile;
              using (FileStream fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
              {
                PdfDocument pdfDocument = pageMergeDialog.OpenDocument((Stream) fileStream);
                if (pdfDocument == null)
                  return;
                using (pdfDocument)
                {
                  if (pdfDocument.Pages.Count == 0)
                    pageMergeDialog.RangeBox.Text = "";
                  else if (pdfDocument.Pages.Count == 1)
                    pageMergeDialog.RangeBox.Text = "1";
                  else
                    pageMergeDialog.RangeBox.Text = $"1-{pdfDocument.Pages.Count}";
                }
              }
            }
            catch
            {
            }
          }));
        }
        else
        {
          int num = (int) MessageBox.Show("File loading failed. Please select another file and try again.");
          pageMergeDialog.Dispatcher.Invoke((Action) (() => pageMergeDialog.Close()));
        }
      }));
    }
    else
    {
      try
      {
        using (FileStream fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          PdfDocument pdfDocument = this.OpenDocument((Stream) fileStream);
          if (pdfDocument == null)
            return;
          using (pdfDocument)
          {
            if (pdfDocument.Pages.Count == 0)
              this.RangeBox.Text = "";
            else if (pdfDocument.Pages.Count == 1)
              this.RangeBox.Text = "1";
            else
              this.RangeBox.Text = $"1-{pdfDocument.Pages.Count}";
          }
        }
      }
      catch
      {
      }
    }
  }

  private void SetProcessingState(bool processing)
  {
    if (processing)
    {
      this.ProcessingDismissBorder.Visibility = Visibility.Visible;
      this.ProcessingRing.IsActive = true;
    }
    else
    {
      this.ProcessingDismissBorder.Visibility = Visibility.Collapsed;
      this.ProcessingRing.IsActive = false;
    }
  }

  public string MergeTempFilePath { get; private set; }

  public int MergePageCount { get; private set; }

  public int InsertPageIndex { get; private set; }

  public bool InsertBefore { get; private set; }

  private void InitInsertPositionRadioButtons(
    IEnumerable<int> selectedPages,
    bool fromSinglePageCmd)
  {
    if (fromSinglePageCmd)
      this.InitInsertPositionRadioButtonsFromSinglePage(selectedPages);
    else
      this.InitInsertPositionRadioButtonsFromMultiPages(selectedPages);
  }

  private void InitInsertPositionRadioButtonsFromSinglePage(IEnumerable<int> selectedPages)
  {
    int count = this.doc.Pages.Count;
    int[] array = selectedPages != null ? selectedPages.ToArray<int>() : (int[]) null;
    if (array != null && array.Length == 1 && array[0] >= 0 && array[0] < count)
    {
      this.firstSelectedPage = array[0];
      this.lastSelectedPage = array[0];
      this.PageRadioButton.IsEnabled = true;
      this.PageRadioButton.IsChecked = new bool?(true);
      this.PageNumber.Text = count.ToString();
    }
    else
    {
      this.EndRadioButton.IsChecked = new bool?(true);
      this.InitInsertPositionRadioButtonsFromMultiPages(selectedPages);
    }
  }

  private void InitInsertPositionRadioButtonsFromMultiPages(IEnumerable<int> selectedPages)
  {
    int count = this.doc.Pages.Count;
    int[] array = selectedPages != null ? selectedPages.ToArray<int>() : (int[]) null;
    if (array != null)
    {
      int val2_1 = int.MaxValue;
      int val2_2 = int.MinValue;
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] >= 0 && array[index] < count)
        {
          val2_1 = Math.Min(array[index], val2_1);
          val2_2 = Math.Max(array[index], val2_2);
        }
      }
      if (val2_1 != int.MaxValue && val2_1 >= 0)
        this.firstSelectedPage = val2_1;
      if (val2_2 != int.MinValue && val2_2 <= count - 1)
        this.lastSelectedPage = val2_2;
    }
    if (this.firstSelectedPage != -1)
    {
      this.PageRadioButton.IsEnabled = true;
      this.PageRadioButton.IsChecked = new bool?(true);
      this.InsertPosition.SelectedIndex = 1;
      this.PageNumber.Text = count.ToString();
    }
    if (this.lastSelectedPage == -1)
      return;
    this.PageRadioButton.IsEnabled = true;
    this.PageRadioButton.IsChecked = new bool?(true);
    this.InsertPosition.SelectedIndex = 0;
    this.PageNumber.Text = count.ToString();
  }

  private async void OKButton_Click(object sender, RoutedEventArgs e)
  {
    PageMergeDialog pageMergeDialog = this;
    if (pageMergeDialog.RangeBox.IsFocused)
      pageMergeDialog.btnOk.Focus();
    if (!IAPUtils.IsPaidUserWrapper())
    {
      if (pageMergeDialog.sourceFileType == InsertSourceFileType.PDF)
        IAPUtils.ShowPurchaseWindows("insertpages", ".pdf");
      else
        IAPUtils.ShowPurchaseWindows("insertword", ".pdf");
    }
    else
    {
      pageMergeDialog.IsEnabled = false;
      bool? isChecked = pageMergeDialog.BeginingRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
      {
        pageMergeDialog.InsertPageIndex = 0;
        pageMergeDialog.InsertBefore = true;
      }
      else
      {
        isChecked = pageMergeDialog.EndRadioButton.IsChecked;
        if (isChecked.GetValueOrDefault())
        {
          pageMergeDialog.InsertPageIndex = pageMergeDialog.doc.Pages.Count - 1;
          pageMergeDialog.InsertBefore = false;
        }
        else
        {
          isChecked = pageMergeDialog.PageRadioButton.IsChecked;
          if (isChecked.GetValueOrDefault())
          {
            pageMergeDialog.InsertBefore = pageMergeDialog.InsertPosition.SelectedIndex != 0;
            pageMergeDialog.InsertPageIndex = pageMergeDialog.GetInsertPageIndex(pageMergeDialog.InsertBefore);
            if (pageMergeDialog.InsertPageIndex == -1)
            {
              int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError, UtilManager.GetProductName());
              pageMergeDialog.IsEnabled = true;
              return;
            }
          }
        }
      }
      try
      {
        FileInfo fileInfo = pageMergeDialog.TryGetFileInfo();
        if (fileInfo != null)
        {
          using (FileStream stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
          {
            PdfDocument pdfDocument = pageMergeDialog.OpenDocument((Stream) stream);
            if (pdfDocument != null)
            {
              using (pdfDocument)
              {
                int[] range = pageMergeDialog.GetImportPageRange(pdfDocument);
                if (range != null && range.Length != 0)
                {
                  string str = await TempFileUtils.SaveMergeSourceFile(pdfDocument, range);
                  if (!string.IsNullOrEmpty(str))
                  {
                    pageMergeDialog.MergeTempFilePath = str;
                    pageMergeDialog.MergePageCount = range.Length;
                    pageMergeDialog.IsEnabled = true;
                    pageMergeDialog.DialogResult = new bool?(true);
                  }
                  else
                  {
                    int num = (int) MessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeInsertfailedMsg, UtilManager.GetProductName());
                  }
                }
                range = (int[]) null;
              }
            }
            else
            {
              int num1 = (int) MessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeOpenfailedMsg, UtilManager.GetProductName());
            }
          }
        }
        else
        {
          int num2 = (int) MessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeOpenfailedMsg, UtilManager.GetProductName());
        }
      }
      catch
      {
      }
      pageMergeDialog.IsEnabled = true;
    }
  }

  private int GetInsertPageIndex(bool InsertBefore)
  {
    int count = this.doc.Pages.Count;
    int[] importPageRange = this.GetImportPageRange();
    if (importPageRange == null)
      return -1;
    int val2_1 = int.MaxValue;
    int val2_2 = int.MinValue;
    for (int index = 0; index < importPageRange.Length; ++index)
    {
      if (importPageRange[index] >= 0 && importPageRange[index] < count)
      {
        val2_1 = Math.Min(importPageRange[index], val2_1);
        val2_2 = Math.Max(importPageRange[index], val2_2);
      }
    }
    if (!InsertBefore)
    {
      if (val2_2 != int.MinValue && val2_2 <= count - 1)
        return val2_2;
    }
    else if (val2_1 != int.MaxValue && val2_1 >= 0)
      return val2_1;
    return this.InsertPageIndex;
  }

  private int[] GetImportPageRange()
  {
    int[] source = (int[]) null;
    if (string.IsNullOrEmpty(this.PageindexNumbox.Text))
      return (int[]) null;
    int[] pageIndexes;
    PdfObjectExtensions.TryParsePageRange(this.PageindexNumbox.Text, out pageIndexes, out int _);
    if (pageIndexes == null)
      return (int[]) null;
    if (pageIndexes.Length != 0)
      source = pageIndexes;
    if (((IEnumerable<int>) source).Any<int>((Func<int, bool>) (c => c < 0 || c >= this.doc.Pages.Count)))
      return (int[]) null;
    return source.Length == 0 ? (int[]) null : source;
  }

  private PdfDocument OpenDocument(Stream stream)
  {
    PdfDocument pdfDocument = (PdfDocument) null;
    string password = this.password;
    bool flag = false;
    do
    {
      try
      {
        pdfDocument = PdfDocument.Load(stream, password: password);
        this.password = password;
      }
      catch (InvalidPasswordException ex)
      {
        if (flag)
        {
          int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.OpenDocByIncorrectPwdMsg, "PDFgear");
        }
        EnterPasswordDialog enterPasswordDialog = new EnterPasswordDialog();
        enterPasswordDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
        enterPasswordDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        flag = enterPasswordDialog.ShowDialog().GetValueOrDefault();
        password = enterPasswordDialog.Password;
        if (password == "")
          password = (string) null;
        stream.Seek(0L, SeekOrigin.Begin);
      }
    }
    while (flag && pdfDocument == null);
    return pdfDocument;
  }

  private FileInfo TryGetFileInfo()
  {
    try
    {
      return new FileInfo(this.sourceFile);
    }
    catch
    {
    }
    return (FileInfo) null;
  }

  private int[] GetImportPageRange(PdfDocument doc)
  {
    int[] array;
    if (this.AllPagesRadioButton.IsChecked.GetValueOrDefault())
    {
      if (doc.Pages.Count == 0)
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeCorruptedDocMsg, UtilManager.GetProductName());
        return (int[]) null;
      }
      array = Enumerable.Range(0, doc.Pages.Count).ToArray<int>();
    }
    else
      array = this.RangeBox.PageIndexes.ToArray<int>();
    if (array == null || ((IEnumerable<int>) array).Any<int>((Func<int, bool>) (c => c < 0 || c >= doc.Pages.Count)))
    {
      int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeOpenfailedMsg, UtilManager.GetProductName());
      return (int[]) null;
    }
    if (this.PageRangeTypeComboBox.SelectedIndex == 1)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
    else if (this.PageRangeTypeComboBox.SelectedIndex == 2)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
    if (array.Length != 0)
      return array;
    int num1 = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeCorrectPageMsg, UtilManager.GetProductName());
    return (int[]) null;
  }

  private int[] GetImportPageRange(int docMaxPage)
  {
    int[] array;
    if (this.AllPagesRadioButton.IsChecked.GetValueOrDefault())
    {
      if (docMaxPage == 0)
      {
        int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeCorruptedDocMsg, UtilManager.GetProductName());
        return (int[]) null;
      }
      array = Enumerable.Range(0, docMaxPage).ToArray<int>();
    }
    else
      array = this.RangeBox.PageIndexes.ToArray<int>();
    if (array == null || ((IEnumerable<int>) array).Any<int>((Func<int, bool>) (c => c < 0 || c >= docMaxPage)))
    {
      int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeOpenfailedMsg, UtilManager.GetProductName());
      return (int[]) null;
    }
    if (this.PageRangeTypeComboBox.SelectedIndex == 1)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
    else if (this.PageRangeTypeComboBox.SelectedIndex == 2)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
    if (array.Length != 0)
      return array;
    int num1 = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinPageMergeCorrectPageMsg, UtilManager.GetProductName());
    return (int[]) null;
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

  protected override void OnClosing(CancelEventArgs e)
  {
    e.Cancel = !this.IsEnabled;
    base.OnClosing(e);
  }

  private void PageindexNumbox_TextChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    if (this.doc == null)
      return;
    int num = (int) (e.NewValue - 1.0);
    if (num < 0)
    {
      this.PageindexNumbox.Text = "0";
      num = 0;
    }
    if (num > this.doc.Pages.Count - 1)
    {
      this.PageindexNumbox.Text = $"{this.doc.Pages.Count - 1}";
      num = this.doc.Pages.Count - 1;
    }
    this.InsertPageIndex = num;
  }

  private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageeditor/pagemergedialog.xaml", UriKind.Relative));
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
        this.ContentGrid = (Grid) target;
        break;
      case 2:
        this.LocationTextBox = (OpenFileTextBox) target;
        break;
      case 3:
        this.AllPagesRadioButton = (RadioButton) target;
        break;
      case 4:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 5:
        this.RangeBox = (PageRangeTextBox) target;
        break;
      case 6:
        this.PageRangeTypeComboBox = (ComboBox) target;
        break;
      case 7:
        this.BeginingRadioButton = (RadioButton) target;
        break;
      case 8:
        this.EndRadioButton = (RadioButton) target;
        break;
      case 9:
        this.PageRadioButton = (RadioButton) target;
        break;
      case 10:
        this.PageindexNumbox = (TextBox) target;
        this.PageindexNumbox.LostFocus += new RoutedEventHandler(this.CustomTextBox_LostFocus);
        break;
      case 11:
        this.PageNumber = (TextBlock) target;
        break;
      case 12:
        this.InsertPosition = (ComboBox) target;
        break;
      case 13:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 14:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      case 15:
        this.ProcessingDismissBorder = (Border) target;
        break;
      case 16 /*0x10*/:
        this.ProcessingRing = (ProgressRing) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}

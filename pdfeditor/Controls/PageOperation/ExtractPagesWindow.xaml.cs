// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageOperation.ExtractPagesWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageOperation;

public partial class ExtractPagesWindow : Window, IComponentConnector
{
  private int[] Pages;
  private string FilePath;
  private string Range;
  private bool IsSplit;
  private bool IsDelete;
  private ExtractPagesWindow.PageOpeations pageOpeations;
  private MainViewModel vm;
  internal RadioButton SelectedPages;
  internal TextBlock range;
  internal RadioButton AllPages;
  internal RadioButton CustomPages;
  internal TextBox CustomTextBox;
  internal TextBlock CustomTextBolck;
  internal ComboBox SubsetComboBox;
  internal ComboBoxItem AllpagesSubset;
  internal ComboBoxItem EvenpagesSubset;
  internal RadioButton OnePDF;
  internal RadioButton EveryPDF;
  internal CheckBox DeletePages;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public ExtractPagesWindow(string path, MainViewModel mainViewModel)
  {
    this.InitializeComponent();
    this.SelectedPages.IsEnabled = false;
    this.AllPages.IsChecked = new bool?(true);
    this.FilePath = path;
    this.FilePath = path;
    this.CustomPages.IsChecked = new bool?(true);
    this.pageOpeations = ExtractPagesWindow.PageOpeations.CustomPages;
    this.CustomTextBox.IsEnabled = true;
    this.CustomTextBox.Focus();
    this.vm = mainViewModel;
    this.InitMenu();
  }

  public ExtractPagesWindow(int[] pages, string range, string path, MainViewModel mainViewModel)
  {
    this.InitializeComponent();
    this.Pages = pages;
    this.SelectedPages.IsChecked = new bool?(true);
    if (range.Length >= 30)
    {
      string[] strArray = range.Split(',');
      this.range.Text = $"{pages.Length}{pdfeditor.Properties.Resources.PageWinSelectedPages.Replace("\"XXX\"", " ")} ({strArray[0]}, {strArray[1]}, {strArray[2]}, {strArray[3]}......{strArray[strArray.Length - 2]}, {strArray[strArray.Length - 1]})";
    }
    else
      this.range.Text = $" {pages.Length}{pdfeditor.Properties.Resources.PageWinSelectedPages.Replace("\"XXX\"", " ")} ({range.Replace(",", ", ")})";
    this.Range = range;
    this.FilePath = path;
    this.pageOpeations = ExtractPagesWindow.PageOpeations.SelectedPages;
    this.vm = mainViewModel;
    this.InitMenu();
  }

  private void InitMenu()
  {
    this.btnOk.Click += (RoutedEventHandler) (async (o, e) =>
    {
      ExtractPagesWindow extractPagesWindow1 = this;
      if (!extractPagesWindow1.Check())
        return;
      string filePath = extractPagesWindow1.FilePath;
      if (string.IsNullOrEmpty(extractPagesWindow1.vm.DocumentWrapper?.DocumentPath))
        return;
      ExtractPagesWindow extractPagesWindow = extractPagesWindow1;
      FileInfo fileInfo = new FileInfo(extractPagesWindow1.vm.DocumentWrapper?.DocumentPath);
      string directoryName = fileInfo.DirectoryName;
      string str1 = fileInfo.Name;
      ComboBoxItem selectedItem = extractPagesWindow1.SubsetComboBox.SelectedItem as ComboBoxItem;
      if (!extractPagesWindow1.OddEvenRange(selectedItem.Content.ToString()))
        return;
      int[] Ranges;
      PdfObjectExtensions.TryParsePageRange(extractPagesWindow1.Range, out Ranges, out int _);
      if (extractPagesWindow1.IsDelete && Ranges.Length == extractPagesWindow1.vm.Document.Pages.Count)
      {
        int num1 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.DeletePageCheckMsg, UtilManager.GetProductName());
      }
      else
      {
        if (!string.IsNullOrEmpty(fileInfo.Extension))
          str1 = str1.Substring(0, str1.Length - fileInfo.Extension.Length);
        string str2 = str1.Length <= 48 /*0x30*/ ? $"{str1} Extract[{extractPagesWindow1.Range}].pdf" : $"{str1} [{extractPagesWindow1.Range}].pdf";
        if (str2.Length > 128 /*0x80*/)
        {
          string str3 = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
          string path2 = str3 + " Extract.pdf";
          int num2 = 0;
          try
          {
            for (; File.Exists(Path.Combine(directoryName, path2)); path2 = str3 + $" Extract ({num2}).pdf")
              ++num2;
            str2 = path2;
          }
          catch
          {
            str2 = str3 + " Extract.pdf";
          }
        }
        SaveFileDialog saveFileDialog = new SaveFileDialog()
        {
          Filter = "pdf|*.pdf",
          CreatePrompt = false,
          OverwritePrompt = true,
          InitialDirectory = directoryName,
          FileName = str2
        };
        bool result = false;
        if (extractPagesWindow1.IsSplit)
        {
          CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog()
          {
            IsFolderPicker = true,
            InitialDirectory = saveFileDialog.InitialDirectory
          };
          if (commonOpenFileDialog.ShowDialog((Window) extractPagesWindow1) == CommonFileDialogResult.Ok)
          {
            string str4 = $"{commonOpenFileDialog.FileName}/Extract {Path.GetFileNameWithoutExtension(fileInfo.Name)}";
            int num3 = 0;
            while (Directory.Exists(str4))
            {
              str4 = num3 != 0 ? str4 + $"{num3}" : str4 + " Copy";
              ++num3;
            }
            Directory.CreateDirectory(str4);
            saveFileDialog.InitialDirectory = str4;
            saveFileDialog.FileName = Path.Combine(str4, "Extract.pdf");
            result = true;
          }
        }
        else
          result = saveFileDialog.ShowDialog().Value;
        PdfDocument document = extractPagesWindow1.vm.DocumentWrapper.Document;
        await Task.Run(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
        {
          if (!result)
            return;
          if (string.IsNullOrEmpty(saveFileDialog.FileName))
            return;
          try
          {
            if (File.Exists(saveFileDialog.FileName))
            {
              if (!extractPagesWindow.IsSplit)
                File.Delete(saveFileDialog.FileName);
            }
          }
          catch
          {
            int num4 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.ViewToolbarSaveFailed, "PDFgear");
            result = false;
            return;
          }
          PageDisposeHelper.TryFixResource(extractPagesWindow.vm.Document, ((IEnumerable<int>) Ranges).Min(), ((IEnumerable<int>) Ranges).Max());
          using (PdfDocument sourceDoc = PdfDocument.CreateNew())
          {
            sourceDoc.Pages.ImportPages(extractPagesWindow.vm.Document, extractPagesWindow.Range, 0);
            if (extractPagesWindow.IsSplit)
            {
              for (int index = 1; index <= sourceDoc.Pages.Count; ++index)
              {
                using (PdfDocument pdfDocument = PdfDocument.CreateNew())
                {
                  string pagerange = index.ToString();
                  pdfDocument.Pages.ImportPages(sourceDoc, pagerange, 0);
                  string path = $"{saveFileDialog.FileName.Remove(saveFileDialog.FileName.Length - 4, 4)}[{pagerange}].pdf";
                  if (File.Exists(path))
                    File.Delete(path);
                  using (FileStream fileStream = File.OpenWrite(path))
                    pdfDocument.Save((Stream) fileStream, SaveFlags.NoIncremental);
                }
              }
            }
            else
            {
              try
              {
                using (FileStream fileStream = File.OpenWrite(saveFileDialog.FileName))
                {
                  fileStream.Seek(0L, SeekOrigin.Begin);
                  sourceDoc.Save((Stream) fileStream, SaveFlags.NoIncremental);
                  fileStream.SetLength(fileStream.Position);
                }
              }
              catch (Exception ex)
              {
                int num5 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.ViewToolbarSaveFailed);
              }
            }
          }
        })));
        if (extractPagesWindow1.IsDelete && result)
        {
          PdfDocument tmpDoc = PdfDocument.CreateNew();
          tmpDoc.Pages.ImportPages(extractPagesWindow1.vm.Document, extractPagesWindow1.Range, 0);
          int[] arr;
          ((IEnumerable<int>) Ranges).ConvertToRange(out arr);
          PageDisposeHelper.TryFixResource(extractPagesWindow1.vm.Document, ((IEnumerable<int>) Ranges).Min(), ((IEnumerable<int>) Ranges).Max());
          for (int index = arr.Length - 1; index >= 0; --index)
            extractPagesWindow1.vm.Document.Pages.DeleteAt(arr[index]);
          PDFKit.PdfControl pdfControl1 = PDFKit.PdfControl.GetPdfControl(extractPagesWindow1.vm.Document);
          if (pdfControl1 != null && pdfControl1.Document != null)
            pdfControl1.UpdateDocLayout();
          extractPagesWindow1.vm.UpdateDocumentCore();
          await extractPagesWindow1.vm.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
          {
            for (int index = 0; index < arr.Length; ++index)
              doc.Pages.ImportPages(tmpDoc, $"{index + 1}", arr[index]);
            PDFKit.PdfControl pdfControl2 = PDFKit.PdfControl.GetPdfControl(doc);
            if (pdfControl2 != null && pdfControl2.Document != null)
              pdfControl2.UpdateDocLayout();
            closure_3.vm.UpdateDocumentCore();
          }), (Action<PdfDocument>) (doc =>
          {
            for (int index = arr.Length - 1; index >= 0; --index)
              doc.Pages.DeleteAt(arr[index]);
            PDFKit.PdfControl pdfControl3 = PDFKit.PdfControl.GetPdfControl(doc);
            if (pdfControl3 != null && pdfControl3.Document != null)
              pdfControl3.UpdateDocLayout();
            closure_3.vm.UpdateDocumentCore();
          }));
        }
        if (result)
        {
          int num6 = await new FileInfo(saveFileDialog.FileName).ShowInExplorerAsync() ? 1 : 0;
          extractPagesWindow1.Close();
        }
      }
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) => this.Close());
  }

  private bool Check()
  {
    try
    {
      if (this.pageOpeations == ExtractPagesWindow.PageOpeations.AllPages)
      {
        int count = this.vm.Document.Pages.Count;
        List<int> intList = new List<int>();
        for (int index = 0; index < count; ++index)
          intList.Add(index);
        this.Pages = intList.ToArray();
      }
      else if (this.pageOpeations == ExtractPagesWindow.PageOpeations.CustomPages)
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
      this.IsDelete = this.DeletePages.IsChecked.Value;
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

  private void SelectedPages_Click(object sender, RoutedEventArgs e)
  {
    RadioButton radioButton = sender as RadioButton;
    if (!radioButton.IsChecked.Value)
      return;
    switch (radioButton.Name)
    {
      case "SelectedPages":
        this.CustomTextBox.IsEnabled = false;
        this.pageOpeations = ExtractPagesWindow.PageOpeations.SelectedPages;
        this.AllpagesSubset.Visibility = Visibility.Visible;
        this.AllpagesSubset.IsSelected = true;
        break;
      case "AllPages":
        this.CustomTextBox.IsEnabled = false;
        this.pageOpeations = ExtractPagesWindow.PageOpeations.AllPages;
        this.AllpagesSubset.Visibility = Visibility.Collapsed;
        this.EvenpagesSubset.IsSelected = true;
        break;
      case "CustomPages":
        this.CustomTextBox.IsEnabled = true;
        this.CustomTextBox.Focus();
        this.pageOpeations = ExtractPagesWindow.PageOpeations.CustomPages;
        this.AllpagesSubset.Visibility = Visibility.Visible;
        this.AllpagesSubset.IsSelected = true;
        break;
      default:
        this.pageOpeations = ExtractPagesWindow.PageOpeations.AllPages;
        break;
    }
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

  private void OnePDF_Click(object sender, RoutedEventArgs e)
  {
    RadioButton radioButton = sender as RadioButton;
    if (!radioButton.IsChecked.Value)
      return;
    switch (radioButton.Name)
    {
      case "OnePDF":
        this.IsSplit = false;
        break;
      case "EveryPDF":
        this.IsSplit = true;
        break;
    }
  }

  private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageoperation/extractpageswindow.xaml", UriKind.Relative));
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
        this.CustomTextBox.TextChanged += new TextChangedEventHandler(this.CustomTextBox_TextChanged);
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
        this.OnePDF = (RadioButton) target;
        this.OnePDF.Click += new RoutedEventHandler(this.OnePDF_Click);
        break;
      case 11:
        this.EveryPDF = (RadioButton) target;
        this.EveryPDF.Click += new RoutedEventHandler(this.OnePDF_Click);
        break;
      case 12:
        this.DeletePages = (CheckBox) target;
        break;
      case 13:
        this.btnCancel = (Button) target;
        break;
      case 14:
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

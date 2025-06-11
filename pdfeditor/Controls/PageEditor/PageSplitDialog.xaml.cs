// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageEditor.PageSplitDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.Utils.Behaviors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageEditor;

public partial class PageSplitDialog : Window, IComponentConnector
{
  private int maxPageCount = 5;
  private readonly string docPath;
  private readonly PdfDocument document;
  internal Grid ContentLayout;
  internal RadioButton PageRangeRadioButton;
  internal PageRangeTextBox PageRangeTextBox;
  internal RadioButton MaxPageCountRadioButton;
  internal TextBox MaxPageCountTextBox;
  internal TextBoxEditBehavior MaxPageCountTextBehavior;
  internal SaveFolderTextBox SavePath;
  private bool _contentLoaded;

  public PageSplitDialog(string docPath, PdfDocument document)
  {
    this.InitializeComponent();
    this.SavePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    this.MaxPageCountTextBehavior.Text = $"{this.maxPageCount}";
    this.docPath = docPath;
    this.document = document ?? throw new ArgumentNullException(nameof (document));
    if (document.Pages.Count == 1)
      this.PageRangeTextBox.Text = "1";
    else if (document.Pages.Count == 2)
    {
      this.PageRangeTextBox.Text = "1,2";
    }
    else
    {
      int num = document.Pages.Count / 2;
      this.PageRangeTextBox.Text = $"{(num - 1 != 0 ? $"1-{num}" : "1")},{$"{num + 1}-{document.Pages.Count}"}";
    }
  }

  private void MaxPageCountTextBehavior_TextChanged(object sender, EventArgs e)
  {
    int result;
    if (int.TryParse(this.MaxPageCountTextBehavior.Text, out result) && result > 0)
      this.maxPageCount = result;
    else
      this.MaxPageCountTextBehavior.Text = $"{this.maxPageCount}";
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

  private async void OKButton_Click(object sender, RoutedEventArgs e)
  {
    PageSplitDialog pageSplitDialog = this;
    pageSplitDialog.IsEnabled = false;
    try
    {
      string docName;
      string saveDirectoryName = pageSplitDialog.CreateSaveDirectoryName(out docName);
      if (string.IsNullOrEmpty(saveDirectoryName))
      {
        pageSplitDialog.IsEnabled = true;
        return;
      }
      if (pageSplitDialog.MaxPageCountRadioButton.IsChecked.GetValueOrDefault())
      {
        if (pageSplitDialog.maxPageCount < 0)
        {
          pageSplitDialog.IsEnabled = true;
          return;
        }
        if (!pageSplitDialog.CreateSaveDirectory(saveDirectoryName))
        {
          pageSplitDialog.IsEnabled = true;
          return;
        }
        for (int index = 0; index < pageSplitDialog.document.Pages.Count; index += pageSplitDialog.maxPageCount)
        {
          string range = $"{index + 1}-{Math.Min(index + pageSplitDialog.maxPageCount, pageSplitDialog.document.Pages.Count - 1) + 1}";
          pageSplitDialog.CreateRangeDocument(range, docName, saveDirectoryName);
        }
      }
      else if (pageSplitDialog.PageRangeRadioButton.IsChecked.GetValueOrDefault())
      {
        if (pageSplitDialog.PageRangeTextBox.HasError || pageSplitDialog.PageRangeTextBox.PageIndexes.Count == 0 || pageSplitDialog.PageRangeTextBox.PageIndexes.Last<int>() >= pageSplitDialog.document.Pages.Count)
        {
          pageSplitDialog.IsEnabled = true;
          return;
        }
        int[][] pageIndexes1;
        if (!PdfObjectExtensions.TryParsePageRange2(pageSplitDialog.PageRangeTextBox.Text, out pageIndexes1, out int _))
        {
          pageSplitDialog.IsEnabled = true;
          return;
        }
        if (!pageSplitDialog.CreateSaveDirectory(saveDirectoryName))
        {
          pageSplitDialog.IsEnabled = true;
          return;
        }
        foreach (IEnumerable<int> pageIndexes2 in pageIndexes1)
        {
          string range = pageIndexes2.ConvertToRange();
          pageSplitDialog.CreateRangeDocument(range, docName, saveDirectoryName);
        }
      }
      if (Directory.Exists(saveDirectoryName))
      {
        int num = await ExplorerUtils.OpenFolderAsync(saveDirectoryName, new CancellationToken()) ? 1 : 0;
        pageSplitDialog.IsEnabled = true;
        pageSplitDialog.DialogResult = new bool?(true);
      }
    }
    catch
    {
    }
    pageSplitDialog.IsEnabled = true;
  }

  private bool CreateSaveDirectory(string saveFolderPath)
  {
    try
    {
      Directory.CreateDirectory(saveFolderPath);
      return true;
    }
    catch
    {
      this.IsEnabled = true;
    }
    return false;
  }

  private void CreateRangeDocument(string range, string docName, string savePath)
  {
    string path2 = $"{docName} [{range}].pdf";
    string path = Path.Combine(savePath, path2);
    if (File.Exists(path))
      return;
    try
    {
      using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
      {
        using (PdfDocument pdfDocument = PdfDocument.CreateNew())
        {
          pdfDocument.Pages.ImportPages(this.document, range, 0);
          pdfDocument.Save((Stream) fileStream, SaveFlags.NoIncremental);
        }
      }
    }
    catch
    {
    }
  }

  private string CreateSaveDirectoryName(out string docName)
  {
    docName = string.Empty;
    string text = this.SavePath.Text;
    if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
      return string.Empty;
    FileInfo fileInfo = new FileInfo(this.docPath);
    docName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
    if (string.IsNullOrEmpty(docName))
      docName = "Pdf Split";
    string path = Path.Combine(text, docName);
    int num = 1;
    while (Directory.Exists(path))
    {
      path = Path.Combine(text, $"{docName} ({num})");
      ++num;
    }
    return path;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    e.Cancel = !this.IsEnabled;
    base.OnClosing(e);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageeditor/pagesplitdialog.xaml", UriKind.Relative));
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
        this.ContentLayout = (Grid) target;
        break;
      case 2:
        this.PageRangeRadioButton = (RadioButton) target;
        break;
      case 3:
        this.PageRangeTextBox = (PageRangeTextBox) target;
        break;
      case 4:
        this.MaxPageCountRadioButton = (RadioButton) target;
        break;
      case 5:
        this.MaxPageCountTextBox = (TextBox) target;
        break;
      case 6:
        this.MaxPageCountTextBehavior = (TextBoxEditBehavior) target;
        break;
      case 7:
        this.SavePath = (SaveFolderTextBox) target;
        break;
      case 8:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 9:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}

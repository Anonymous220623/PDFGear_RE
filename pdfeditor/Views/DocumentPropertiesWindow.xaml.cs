// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.DocumentPropertiesWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.ViewModels;
using PDFKit.Utils.PageHeaderFooters;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Views;

public partial class DocumentPropertiesWindow : Window, IComponentConnector
{
  private PdfDocument pdfDocument;
  private string titleBackup;
  private string[] authorBackup;
  private string subjectBackup;
  private string keywordBackup;
  private int pageSeletedIndex = -1;
  internal TextBlock Filename;
  internal TextBlock TotalPages;
  internal TextBlock PageSize;
  internal ComboBox Unit;
  internal TextBox Title;
  internal TextBox Author;
  internal TextBox Subject;
  internal TextBox Keyword;
  internal TextBlock Creator;
  internal TextBlock Producer;
  internal TextBlock Version;
  internal Run Location;
  internal TextBlock FileSize;
  internal TextBlock Created;
  internal TextBlock Modified;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public DocumentPropertiesWindow(PdfDocument document, int PageIndex, string FilePath)
  {
    if (document == null)
      return;
    this.pdfDocument = document;
    this.pageSeletedIndex = PageIndex;
    this.InitializeComponent();
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    this.Filename.Text = Path.GetFileName(FilePath);
    this.TotalPages.Text = document.Pages.Count.ToString();
    if (ConfigManager.GetDocumentPropertiesUnit() == 0)
    {
      this.Unit.SelectedIndex = 0;
      this.PageSize.Text = $"{PageHeaderFooterUtils.PdfPointToCm((double) document.Pages[PageIndex - 1].Width).ToString("0.000")} x {PageHeaderFooterUtils.PdfPointToCm((double) document.Pages[PageIndex - 1].Height).ToString("0.000")}";
    }
    else
    {
      this.Unit.SelectedIndex = 1;
      TextBlock pageSize = this.PageSize;
      float num = this.pdfDocument.Pages[this.pageSeletedIndex - 1].Width / 72f;
      string str1 = num.ToString("0.000");
      num = this.pdfDocument.Pages[this.pageSeletedIndex - 1].Height / 72f;
      string str2 = num.ToString("0.000");
      string str3 = $"{str1} x {str2}";
      pageSize.Text = str3;
    }
    this.Title.Text = requiredService.DocumentWrapper.Metadata.Title;
    this.Author.Text = requiredService.DocumentWrapper.Metadata.Author.Length == 0 ? "" : requiredService.DocumentWrapper.Metadata.Author[0];
    this.authorBackup = requiredService.DocumentWrapper.Metadata.Author;
    this.subjectBackup = requiredService.DocumentWrapper.Metadata.Subject;
    this.keywordBackup = requiredService.DocumentWrapper.Metadata.Keywords;
    this.titleBackup = requiredService.DocumentWrapper.Metadata.Title;
    this.Subject.Text = requiredService.DocumentWrapper.Metadata.Subject;
    this.Keyword.Text = requiredService.DocumentWrapper.Metadata.Keywords;
    this.Creator.Text = requiredService.DocumentWrapper.Metadata.Creator;
    this.Producer.Text = requiredService.DocumentWrapper.Metadata.Producer;
    if (this.Producer.Text.Length > 100)
      this.Producer.ToolTip = (object) this.Producer.Text;
    if (this.Creator.Text.Length > 100)
      this.Creator.ToolTip = (object) this.Creator.Text;
    if (this.Filename.Text.Length > 50)
    {
      this.Filename.ToolTip = (object) this.Filename.Text;
      string str = this.Filename.Text;
      for (int startIndex = 30; startIndex < this.Filename.Text.Length; startIndex += 30)
        str = str.Insert(startIndex, "\n");
      this.Filename.ToolTip = (object) str;
    }
    int pdfFileVersion = requiredService.DocumentWrapper.Metadata.PdfFileVersion;
    this.Version.Text = $"PDF-{pdfFileVersion / 10}.{pdfFileVersion % 10}";
    this.Location.Text = FilePath;
    this.FileSize.Text = DocumentPropertiesWindow.FormatFileSize(DocumentPropertiesWindow.GetFileSize(FilePath));
    this.Created.Text = !(requiredService.DocumentWrapper.Metadata.CreationDate == new DateTimeOffset()) ? requiredService.DocumentWrapper.Metadata.CreationDate.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : "";
    this.Modified.Text = !(requiredService.DocumentWrapper.Metadata.ModificationDate == new DateTimeOffset()) ? requiredService.DocumentWrapper.Metadata.ModificationDate.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : "";
    CommomLib.Commom.GAManager.SendEvent(nameof (DocumentPropertiesWindow), "Show", "Count", 1L);
  }

  public static long GetFileSize(string filePath) => new FileInfo(filePath).Length;

  public static string FormatFileSize(long fileSize)
  {
    string[] strArray = new string[9]
    {
      "B",
      "KB",
      "MB",
      "GB",
      "TB",
      "PB",
      "EB",
      "ZB",
      "YB"
    };
    if (fileSize == 0L)
      return "0" + strArray[0];
    int y = (int) Math.Floor(Math.Log((double) fileSize, 1024.0));
    return $"{Math.Round((double) fileSize / Math.Pow(1024.0, (double) y), 2)} {strArray[y]}";
  }

  private async void btnOk_ClickAsync(object sender, RoutedEventArgs e)
  {
    DocumentPropertiesWindow propertiesWindow = this;
    bool flag = false;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    string title = propertiesWindow.Title.Text;
    string[] author = new string[1]
    {
      propertiesWindow.Author.Text
    };
    string subject = propertiesWindow.Subject.Text;
    string keyword = propertiesWindow.Keyword.Text;
    if (propertiesWindow.Title.Text != requiredService.DocumentWrapper.Metadata.Title)
    {
      requiredService.DocumentWrapper.Metadata.Title = propertiesWindow.Title.Text;
      flag = true;
    }
    if ((requiredService.DocumentWrapper.Metadata.Author == null || requiredService.DocumentWrapper.Metadata.Author.Length == 0) && !string.IsNullOrEmpty(propertiesWindow.Author.Text) || requiredService.DocumentWrapper.Metadata.Author.Length != 0 && propertiesWindow.Author.Text != requiredService.DocumentWrapper.Metadata.Author[0])
    {
      requiredService.DocumentWrapper.Metadata.Author = new string[1]
      {
        propertiesWindow.Author.Text
      };
      flag = true;
    }
    if (propertiesWindow.Subject.Text != requiredService.DocumentWrapper.Metadata.Subject)
    {
      requiredService.DocumentWrapper.Metadata.Subject = propertiesWindow.Subject.Text;
      flag = true;
    }
    if (propertiesWindow.Keyword.Text != requiredService.DocumentWrapper.Metadata.Keywords)
    {
      requiredService.DocumentWrapper.Metadata.Keywords = propertiesWindow.Keyword.Text;
      flag = true;
    }
    if (flag)
    {
      CommomLib.Commom.GAManager.SendEvent(nameof (DocumentPropertiesWindow), "OKBtn", "Update", 1L);
      await requiredService.OperationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
      {
        MainViewModel dataContext = PDFKit.PdfControl.GetPdfControl(doc).DataContext as MainViewModel;
        dataContext.DocumentWrapper.Metadata.Title = this.titleBackup;
        dataContext.DocumentWrapper.Metadata.Author = this.authorBackup;
        dataContext.DocumentWrapper.Metadata.Subject = this.subjectBackup;
        dataContext.DocumentWrapper.Metadata.Keywords = this.keywordBackup;
      }), (Action<PdfDocument>) (doc =>
      {
        MainViewModel dataContext = PDFKit.PdfControl.GetPdfControl(doc).DataContext as MainViewModel;
        dataContext.DocumentWrapper.Metadata.Title = title;
        dataContext.DocumentWrapper.Metadata.Author = author;
        dataContext.DocumentWrapper.Metadata.Subject = subject;
        dataContext.DocumentWrapper.Metadata.Keywords = keyword;
      }));
    }
    else
      CommomLib.Commom.GAManager.SendEvent(nameof (DocumentPropertiesWindow), "OKBtn", "NoUpdate", 1L);
    propertiesWindow.Close();
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent(nameof (DocumentPropertiesWindow), "CancelBtn", "Count", 1L);
    this.Close();
  }

  private void Hyperlink_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent(nameof (DocumentPropertiesWindow), "OpenLocation", "Count", 1L);
    if (string.IsNullOrEmpty(this.Location.Text))
      return;
    Process.Start("explorer.exe", $"/select,\"{this.Location.Text}\"");
  }

  private void Unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.pageSeletedIndex == -1 || this.pdfDocument == null)
      return;
    ComboBox comboBox = sender as ComboBox;
    if (comboBox.SelectedIndex == 0)
    {
      TextBlock pageSize = this.PageSize;
      double cm = PageHeaderFooterUtils.PdfPointToCm((double) this.pdfDocument.Pages[this.pageSeletedIndex - 1].Width);
      string str1 = cm.ToString("0.000");
      cm = PageHeaderFooterUtils.PdfPointToCm((double) this.pdfDocument.Pages[this.pageSeletedIndex - 1].Height);
      string str2 = cm.ToString("0.000");
      string str3 = $"{str1} x {str2}";
      pageSize.Text = str3;
    }
    else
    {
      TextBlock pageSize = this.PageSize;
      float num = this.pdfDocument.Pages[this.pageSeletedIndex - 1].Width / 72f;
      string str4 = num.ToString("0.000");
      num = this.pdfDocument.Pages[this.pageSeletedIndex - 1].Height / 72f;
      string str5 = num.ToString("0.000");
      string str6 = $"{str4} x {str5}";
      pageSize.Text = str6;
    }
    ConfigManager.SetDocumentPropertiesUnit(comboBox.SelectedIndex);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/documentpropertieswindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.Filename = (TextBlock) target;
        break;
      case 2:
        this.TotalPages = (TextBlock) target;
        break;
      case 3:
        this.PageSize = (TextBlock) target;
        break;
      case 4:
        this.Unit = (ComboBox) target;
        this.Unit.SelectionChanged += new SelectionChangedEventHandler(this.Unit_SelectionChanged);
        break;
      case 5:
        this.Title = (TextBox) target;
        break;
      case 6:
        this.Author = (TextBox) target;
        break;
      case 7:
        this.Subject = (TextBox) target;
        break;
      case 8:
        this.Keyword = (TextBox) target;
        break;
      case 9:
        this.Creator = (TextBlock) target;
        break;
      case 10:
        this.Producer = (TextBlock) target;
        break;
      case 11:
        this.Version = (TextBlock) target;
        break;
      case 12:
        ((Hyperlink) target).Click += new RoutedEventHandler(this.Hyperlink_Click);
        break;
      case 13:
        this.Location = (Run) target;
        break;
      case 14:
        this.FileSize = (TextBlock) target;
        break;
      case 15:
        this.Created = (TextBlock) target;
        break;
      case 16 /*0x10*/:
        this.Modified = (TextBlock) target;
        break;
      case 17:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 18:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_ClickAsync);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}

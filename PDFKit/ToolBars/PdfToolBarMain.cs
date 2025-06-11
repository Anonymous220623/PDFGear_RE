// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarMain
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Forms;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarMain : PdfToolBar
{
  public static readonly DependencyProperty WindowProperty = DependencyProperty.Register(nameof (Window), typeof (Window), typeof (PdfToolBarMain), new PropertyMetadata((PropertyChangedCallback) null));

  public event EventHandler<EventArgs<string>> PasswordRequired = null;

  public event EventHandler<EventArgs<PdfPrintDocument>> PdfPrintDocumentCreated = null;

  public Window Window
  {
    get => (Window) this.GetValue(PdfToolBarMain.WindowProperty);
    set => this.SetValue(PdfToolBarMain.WindowProperty, (object) value);
  }

  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnOpenDoc", PDFKit.Properties.Resources.btnOpenText, PDFKit.Properties.Resources.btnOpenToolTipText, this.CreateUriToResource("docOpen.png"), new RoutedEventHandler(this.btn_OpenDocClick)));
    this.Items.Add((object) this.CreateButton("btnPrintDoc", PDFKit.Properties.Resources.btnPrintText, PDFKit.Properties.Resources.btnPrintToolTipText, this.CreateUriToResource("docPrint.png"), new RoutedEventHandler(this.btn_PrintDocClick)));
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is System.Windows.Controls.Button button1)
      button1.IsEnabled = this.PdfViewer != null;
    if (!(this.Items[1] is System.Windows.Controls.Button button2))
      return;
    int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
    button2.IsEnabled = num != 0;
  }

  protected override void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    base.OnPdfViewerChanging(oldValue, newValue);
    if (oldValue != null)
      this.UnsubscribePdfViewEvents(oldValue);
    if (newValue != null)
      this.SubscribePdfViewEvents(newValue);
    if (oldValue != null && oldValue.Document != null)
      this.PdfViewer_DocumentClosed((object) this, EventArgs.Empty);
    if (newValue == null || newValue.Document == null)
      return;
    this.PdfViewer_DocumentLoaded((object) this, EventArgs.Empty);
  }

  protected virtual void OnOpenClick(System.Windows.Controls.Button item)
  {
    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
    openFileDialog.Multiselect = false;
    openFileDialog.Filter = PDFKit.Properties.Resources.OpenDialogFilter;
    if (!openFileDialog.ShowDialog().GetValueOrDefault())
      return;
    try
    {
      GAManager.SendEvent(nameof (PdfToolBarMain), "LoadDocument", "Begin", 1L);
      this.PdfViewer.LoadDocument(openFileDialog.FileName);
      GAManager.SendEvent(nameof (PdfToolBarMain), "LoadDocument", "Done", 1L);
    }
    catch (InvalidPasswordException ex1)
    {
      string password = this.OnPasswordRequired();
      try
      {
        GAManager.SendEvent(nameof (PdfToolBarMain), "LoadDocumentWithPassword", "Begin", 1L);
        this.PdfViewer.LoadDocument(openFileDialog.FileName, password);
        GAManager.SendEvent(nameof (PdfToolBarMain), "LoadDocumentWithPassword", "Done", 1L);
      }
      catch (Exception ex2)
      {
        int num = (int) System.Windows.MessageBox.Show(ex2.Message, PDFKit.Properties.Resources.ErrorHeader, MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }
  }

  protected virtual string OnPasswordRequired()
  {
    EventArgs<string> e = new EventArgs<string>((string) null);
    if (this.PasswordRequired != null)
      this.PasswordRequired((object) this, e);
    return e.Value;
  }

  protected virtual void OnPrintClick(System.Windows.Controls.Button item)
  {
    if (this.PdfViewer.Document.FormFill != null)
      this.PdfViewer.Document.FormFill.ForceToKillFocus();
    PdfPrintDocument pdfPrintDocument = new PdfPrintDocument(this.PdfViewer.Document);
    System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
    printDialog.AllowCurrentPage = true;
    printDialog.AllowSomePages = true;
    printDialog.UseEXDialog = true;
    printDialog.Document = (PrintDocument) pdfPrintDocument;
    this.OnPdfPrinDocumentCreaded(new EventArgs<PdfPrintDocument>(pdfPrintDocument));
    this.Dispatcher.BeginInvoke((Delegate) new PdfToolBarMain.ShowPrintDialogDelegate(PdfToolBarMain.ShowPrintDialog), (object) printDialog);
  }

  protected virtual void OnPdfPrinDocumentCreaded(EventArgs<PdfPrintDocument> e)
  {
    if (this.PdfPrintDocumentCreated == null)
      return;
    this.PdfPrintDocumentCreated((object) this, e);
  }

  private void PdfViewer_DocumentClosed(object sender, EventArgs e) => this.UpdateButtons();

  private void PdfViewer_DocumentLoaded(object sender, EventArgs e) => this.UpdateButtons();

  private void btn_OpenDocClick(object sender, EventArgs e)
  {
    this.OnOpenClick(this.Items[0] as System.Windows.Controls.Button);
  }

  private void btn_PrintDocClick(object sender, EventArgs e)
  {
    this.OnPrintClick(this.Items[1] as System.Windows.Controls.Button);
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_DocumentLoaded);
    oldValue.DocumentClosing -= new EventHandler<DocumentClosingEventArgs>(this.PdfViewer_DocumentClosed);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_DocumentLoaded);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_DocumentClosed);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_DocumentLoaded);
    newValue.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(this.PdfViewer_DocumentClosed);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_DocumentLoaded);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_DocumentClosed);
  }

  private static void ShowPrintDialog(System.Windows.Forms.PrintDialog dlg)
  {
    if (dlg.ShowDialog() != DialogResult.OK)
      return;
    try
    {
      dlg.Document.Print();
    }
    catch (Win32Exception ex)
    {
    }
  }

  private delegate void ShowPrintDialogDelegate(System.Windows.Forms.PrintDialog dlg);
}

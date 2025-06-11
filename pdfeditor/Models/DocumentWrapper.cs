// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.DocumentWrapper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Exceptions;
using pdfeditor.Controls.Protection;
using pdfeditor.Models.Protection;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfeditor.Models;

public class DocumentWrapper : ObservableObject, IDisposable
{
  private FileStream sourceFileStream;
  private Stream openDocStream;
  private PdfDocument document;
  private CancellationTokenSource cts;
  private bool disposedValue;
  private string documentPath;
  private EncryptManage encryptManage;
  private PdfDocumentMetadata pdfMetadata;
  private bool isUntitledFile;
  private Queue<IDisposable> disposeQueue = new Queue<IDisposable>();

  public EncryptManage EncryptManage
  {
    get => this.encryptManage ?? (this.encryptManage = new EncryptManage());
  }

  public PdfDocument Document
  {
    get
    {
      this.ThrowIfDisposed();
      return this.document;
    }
    private set => this.SetProperty<PdfDocument>(ref this.document, value, nameof (Document));
  }

  public string DocumentPath
  {
    get => this.documentPath;
    private set => this.SetProperty<string>(ref this.documentPath, value, nameof (DocumentPath));
  }

  public PdfDocumentMetadata Metadata => this.pdfMetadata;

  public bool IsOpening => this.cts != null;

  public bool IsUntitledFile
  {
    get => this.isUntitledFile;
    private set => this.SetProperty<bool>(ref this.isUntitledFile, value, nameof (IsUntitledFile));
  }

  public async Task<bool> OpenAsync(string filePath)
  {
    DocumentWrapper documentWrapper = this;
    documentWrapper.TryCancel();
    documentWrapper.ThrowIfDisposed();
    documentWrapper.cts = new CancellationTokenSource();
    documentWrapper.OnPropertyChanged("IsOpening");
    documentWrapper.EncryptManage.Init();
    try
    {
      (PdfDocument document1, FileStream fileStream, Stream stream) = await documentWrapper.OpenCoreAsync(filePath, documentWrapper.cts.Token);
      if (document1 != null)
      {
        PdfDocument document2 = documentWrapper.document;
        FileStream sourceFileStream = documentWrapper.sourceFileStream;
        Stream openDocStream = documentWrapper.openDocStream;
        try
        {
          documentWrapper.Document = document1;
          documentWrapper.sourceFileStream = fileStream;
          documentWrapper.openDocStream = stream;
          documentWrapper.DocumentPath = filePath;
          documentWrapper.IsUntitledFile = false;
          documentWrapper.pdfMetadata = new PdfDocumentMetadata(documentWrapper.document);
          documentWrapper.EncryptManage.IsHaveOwerPassword = documentWrapper.Document.SecurityRevision > 0;
          documentWrapper.EncryptManage.IsRequiredOwerPassword = !Pdfium.FPDF_IsOwnerPasswordIsUsed(documentWrapper.Document.Handle);
          if (!documentWrapper.EncryptManage.IsRequiredOwerPassword)
            documentWrapper.EncryptManage.UpdateOwerPassword(documentWrapper.EncryptManage.UserPassword);
          documentWrapper.SendFormData(document1);
          return true;
        }
        finally
        {
          documentWrapper.disposeQueue.Enqueue((IDisposable) document2);
          documentWrapper.disposeQueue.Enqueue((IDisposable) openDocStream);
          documentWrapper.disposeQueue.Enqueue((IDisposable) sourceFileStream);
        }
      }
    }
    catch (OperationCanceledException ex)
    {
    }
    finally
    {
      documentWrapper.cts?.Dispose();
      documentWrapper.cts = (CancellationTokenSource) null;
      documentWrapper.OnPropertyChanged("IsOpening");
    }
    return false;
  }

  public void TryCancel()
  {
    this.cts?.Cancel();
    this.cts = (CancellationTokenSource) null;
    this.OnPropertyChanged("IsOpening");
  }

  public void CloseDocument()
  {
    this.TryCancel();
    PdfDocument document = this.document;
    FileStream sourceFileStream = this.sourceFileStream;
    Stream openDocStream = this.openDocStream;
    if (document != null)
      this.disposeQueue.Enqueue((IDisposable) document);
    if (openDocStream != null)
      this.disposeQueue.Enqueue((IDisposable) openDocStream);
    if (sourceFileStream != null)
      this.disposeQueue.Enqueue((IDisposable) sourceFileStream);
    this.Document = (PdfDocument) null;
    this.sourceFileStream = (FileStream) null;
    this.openDocStream = (Stream) null;
    this.pdfMetadata = (PdfDocumentMetadata) null;
  }

  public void ShowEncryptWindow()
  {
    if (this.document == null)
      return;
    EncryptWindow encryptWindow = new EncryptWindow(this);
    if (encryptWindow == null)
      return;
    encryptWindow.Owner = Application.Current.MainWindow;
    encryptWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    encryptWindow.ShowDialog();
  }

  public void ShowDecryptWindow()
  {
    if (this.document == null)
      return;
    if (!this.EncryptManage.IsHaveUserPassword && !this.EncryptManage.IsHaveOwerPassword || this.EncryptManage.IsRemoveAllPassword)
    {
      int num = (int) ModernMessageBox.Show(Resources.WinPwdRemoveCheckContent, UtilManager.GetProductName());
    }
    else
    {
      if (ModernMessageBox.Show(Resources.WinPwdRemoveConfirmContent, UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        return;
      GAManager.SendEvent("Password", "RemovePassword", "Count", 1L);
      this.EncryptManage.RemoveAllPassword();
      this.EncryptManage.IsHaveOwerPassword = false;
      this.EncryptManage.IsHaveUserPassword = false;
      Ioc.Default.GetRequiredService<MainViewModel>().CanSave = true;
    }
  }

  public void SetUntitledFile() => this.IsUntitledFile = true;

  public void TrimMemory()
  {
    if (this.IsOpening)
      return;
    while (this.disposeQueue.Count > 0)
    {
      IDisposable disposable = this.disposeQueue.Dequeue();
      try
      {
        if (disposable is PdfDocument pdfDocument)
        {
          if (!pdfDocument.IsDisposed)
          {
            try
            {
              if (!pdfDocument.IsDisposed)
              {
                for (int index = pdfDocument.Pages.Count - 1; index >= 0; --index)
                {
                  try
                  {
                    PageDisposeHelper.DisposePage(pdfDocument.Pages[index]);
                  }
                  catch
                  {
                  }
                }
              }
            }
            catch
            {
            }
            pdfDocument.Dispose();
            continue;
          }
        }
        disposable?.Dispose();
      }
      catch
      {
      }
    }
  }

  public async Task<bool> SaveAsync()
  {
    DocumentWrapper documentWrapper = this;
    if (documentWrapper.IsOpening || documentWrapper.Document == null || documentWrapper.sourceFileStream == null)
      return false;
    long length = documentWrapper.sourceFileStream.Length;
    // ISSUE: reference to a compiler-generated method
    return await Task.Run<bool>(TaskExceptionHelper.ExceptionBoundary<bool>(new Func<bool>(documentWrapper.\u003CSaveAsync\u003Eb__32_0))).ConfigureAwait(false);
  }

  private async Task<(PdfDocument document, FileStream sourceFileStream, Stream openDocStream)> OpenCoreAsync(
    string filePath,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(filePath))
      throw new ArgumentException(nameof (filePath));
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.Run<(PdfDocument, FileStream, Stream)>(TaskExceptionHelper.ExceptionBoundary<(PdfDocument, FileStream, Stream)>((Func<Task<(PdfDocument, FileStream, Stream)>>) (async () =>
    {
      FileInfo fileInfo = new FileInfo(filePath);
      FileStream sourceFileStream = fileInfo.Exists ? this.CreateFileStream(fileInfo) : throw new FileNotFoundException(filePath);
      if (sourceFileStream.Length == 0L)
        return ();
      Stream openDocStream = (Stream) null;
      object obj;
      int num1;
      try
      {
        openDocStream = await TempFileUtils.CreateStreamAsync(filePath, sourceFileStream, cancellationToken);
        sourceFileStream.Seek(0L, SeekOrigin.Begin);
        string password = (string) null;
        bool pwdResult = false;
        do
        {
          try
          {
            PdfDocument pdfDocument = await this.OpenCoreAsync(openDocStream, password, cancellationToken);
            this.EncryptManage.UpdateUserPassword(password);
            return (pdfDocument, sourceFileStream, openDocStream);
          }
          catch (InvalidPasswordException ex)
          {
            GAManager.SendEvent("MainWindow", "OpenDocumentCore", "PasswordAsked", 1L);
            if (pwdResult)
            {
              GAManager.SendEvent("MainWindow", "OpenDocumentCore", "WrongPassword", 1L);
              int num2;
              DispatcherHelper.UIDispatcher.Invoke((Action) (() => num2 = (int) ModernMessageBox.Show(Resources.OpenDocByIncorrectPwdMsg, "PDFgear")));
            }
            pwdResult = this.OnPasswordRequested(out password, filePath);
            if (pwdResult)
            {
              if (password == "")
                password = (string) null;
              this.EncryptManage.UpdateUserPassword(password);
              sourceFileStream.Seek(0L, SeekOrigin.Begin);
              openDocStream.Seek(0L, SeekOrigin.Begin);
            }
          }
        }
        while (pwdResult);
        await this.SendPdfFileVersion(openDocStream, cancellationToken, "Password");
        return ();
      }
      catch (object ex)
      {
        obj = ex;
        num1 = 1;
      }
      if (num1 == 1)
      {
        await this.SendPdfFileVersion(openDocStream, cancellationToken, "Exception");
        sourceFileStream?.Dispose();
        sourceFileStream = (FileStream) null;
        openDocStream?.Dispose();
        openDocStream = (Stream) null;
        if (!(obj is Exception source2))
          throw obj;
        ExceptionDispatchInfo.Capture(source2).Throw();
      }
      obj = (object) null;
      sourceFileStream = (FileStream) null;
      openDocStream = (Stream) null;
      (PdfDocument, FileStream, Stream) valueTuple;
      return valueTuple;
    })), cancellationToken).ConfigureAwait(false);
  }

  private FileStream CreateFileStream(FileInfo fileInfo)
  {
    return fileInfo != null ? fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read) : throw new ArgumentNullException(nameof (fileInfo));
  }

  private async Task<PdfDocument> OpenCoreAsync(
    Stream stream,
    string password,
    CancellationToken cancellationToken)
  {
    SynchronizationContext synchronizationContext = await this.GetUIThreadContext().WaitAsync<SynchronizationContext>(cancellationToken).ConfigureAwait(false);
    PdfForms forms = new PdfForms()
    {
      SynchronizationContext = synchronizationContext
    };
    PdfDocument pdfDocument = (PdfDocument) null;
    try
    {
      pdfDocument = PdfDocument.Load(stream, forms, password);
    }
    finally
    {
      try
      {
        if (pdfDocument == null)
        {
          if (forms != null)
          {
            if (!forms.IsDisposed)
              forms.Dispose();
          }
        }
      }
      catch
      {
      }
    }
    return pdfDocument;
  }

  private async Task<SynchronizationContext> GetUIThreadContext()
  {
    TaskCompletionSource<SynchronizationContext> tcs = new TaskCompletionSource<SynchronizationContext>();
    await DispatcherHelper.RunAsync((Action) (() => tcs.SetResult(SynchronizationContext.Current)));
    return await tcs.Task;
  }

  public event EventHandler<DocumentPasswordRequestedEventArgs> PasswordRequested;

  public event EventHandler FileError;

  private bool OnPasswordRequested(out string password, string filepath)
  {
    password = string.Empty;
    DocumentPasswordRequestedEventArgs args = new DocumentPasswordRequestedEventArgs();
    args.FileName = filepath;
    this.EncryptManage.IsHaveUserPassword = true;
    DispatcherHelper.UIDispatcher.Invoke((Action) (() =>
    {
      EventHandler<DocumentPasswordRequestedEventArgs> passwordRequested = this.PasswordRequested;
      if (passwordRequested == null)
        return;
      passwordRequested((object) this, args);
    }));
    password = args.Password;
    return !args.Cancel;
  }

  private void SendFormData(PdfDocument document)
  {
    try
    {
      if (document?.FormFill?.InterForm != null)
      {
        if (document.FormFill.InterForm.HasXFAForm)
          GAManager.SendEvent("MainWindow", "OpenDocumentPDFForm", "XFA", 1L);
        PdfControlCollections controls = document.FormFill.InterForm.Controls;
        // ISSUE: explicit non-virtual call
        if ((controls != null ? (__nonvirtual (controls.Count) > 0 ? 1 : 0) : 0) == 0)
        {
          PdfFieldCollections fields = document.FormFill.InterForm.Fields;
          // ISSUE: explicit non-virtual call
          if ((fields != null ? (__nonvirtual (fields.Count) > 0 ? 1 : 0) : 0) == 0)
            goto label_8;
        }
        GAManager.SendEvent("MainWindow", "OpenDocumentPDFForm", "PDFForm", 1L);
      }
    }
    catch
    {
    }
label_8:
    try
    {
      string str1 = string.Empty;
      if (!string.IsNullOrEmpty(document?.Creator))
        str1 = $"{str1}Creator: {document?.Creator}";
      if (!string.IsNullOrEmpty(document?.Producer))
      {
        if (!string.IsNullOrEmpty(str1))
          str1 += "; ";
        string str2 = $"{str1}Producer: {document.Producer}";
      }
    }
    catch
    {
    }
    if (!this.EncryptManage.IsHaveUserPassword && !this.EncryptManage.IsHaveOwerPassword)
      return;
    if (this.EncryptManage.IsHaveUserPassword && !this.EncryptManage.IsHaveOwerPassword)
      GAManager.SendEvent("MainWindow", "Password", "OnlyUserPassword", 1L);
    else if (!this.EncryptManage.IsHaveUserPassword && this.EncryptManage.IsHaveOwerPassword)
    {
      GAManager.SendEvent("MainWindow", "Password", "OnlyOwerPassword", 1L);
    }
    else
    {
      if (!this.EncryptManage.IsHaveUserPassword || !this.EncryptManage.IsHaveOwerPassword)
        return;
      GAManager.SendEvent("MainWindow", "Password", "UserOwerPassword", 1L);
    }
  }

  private void SendAnnotationData(PdfDocument document, string filePath, bool traceAllData)
  {
    try
    {
      this.SendAnnotationDataCore(document);
    }
    catch
    {
    }
  }

  private void SendAnnotationDataCore(PdfDocument document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    foreach ((string type, int count) traceObjectAllPage in new AnnotationTrace(document).GetAnnotationTypeTraceObjectAllPages())
      GAManager.SendEvent("DocumentAnnotations", traceObjectAllPage.type, "Count", (long) traceObjectAllPage.count);
  }

  private void SendAnnotationDataCore(PdfDocument document, string filePath, bool traceAllData)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    string str = "";
    try
    {
      str = document.Title;
    }
    catch
    {
    }
    if (string.IsNullOrEmpty(str))
    {
      try
      {
        str = new FileInfo(filePath).Name;
      }
      catch
      {
      }
    }
    if (string.IsNullOrEmpty(str))
      str = "no title";
    AnnotationTrace annotationTrace = new AnnotationTrace(document);
    object[] objArray = !traceAllData ? annotationTrace.GetAnnotationTypeTraceObject() : annotationTrace.GetAnnotationModelTraceObject();
    if (objArray == null || objArray.Length == 0)
      JsonConvert.SerializeObject((object) new
      {
        pdfTitle = str,
        annotTracePageCount = 0,
        maxAnnotTracePageCount = 20,
        annots = "empty"
      });
    else
      JsonConvert.SerializeObject((object) new
      {
        pdfTitle = str,
        annotTracePageCount = objArray.Length,
        maxAnnotTracePageCount = 20,
        annots = objArray
      });
  }

  private async Task SendPdfFileVersion(
    Stream stream,
    CancellationToken stopToken,
    string sourceFrom)
  {
    if (stream == null)
      return;
    try
    {
      PdfFileInformationHelper.PdfVersion? pdfVersionAsync = await PdfFileInformationHelper.GetPdfVersionAsync(stream, stopToken);
      if (pdfVersionAsync.HasValue)
        GAManager.SendEvent("OpenPdfFileFail", "PdfFileVersion" + sourceFrom, pdfVersionAsync.Value.ToString(), 1L);
      else
        GAManager.SendEvent("OpenPdfFileFail", "PdfFileVersion" + sourceFrom, "NotPDF", 1L);
    }
    catch
    {
    }
  }

  protected void ThrowIfDisposed()
  {
    if (this.disposedValue)
      throw new ObjectDisposedException(nameof (DocumentWrapper));
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      try
      {
        this.TryCancel();
        this.TrimMemory();
        PdfDocument document = this.document;
        FileStream sourceFileStream = this.sourceFileStream;
        this.Document = (PdfDocument) null;
        document?.Dispose();
        sourceFileStream?.Dispose();
      }
      catch
      {
      }
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}

// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfForms
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the PDF forms in doucument</summary>
public class PdfForms : IDisposable
{
  private PdfInteractiveForms _interForm;
  private static object _syncTimerId = new object();
  private static int __timerid = 0;
  private Dictionary<int, TimerEx> _timers = new Dictionary<int, TimerEx>();
  private PdfDocument _doc;
  private FPDF_FORMFILLINFO _formInfo = new FPDF_FORMFILLINFO();
  private IPDF_JSPLATFORM _jsPlatform = new IPDF_JSPLATFORM();
  private FPDF_FORMFILLNOTIFY _notify = new FPDF_FORMFILLNOTIFY();
  private FS_COLOR[] _highlightColor = new FS_COLOR[8];
  private byte[] _response;
  private byte[] _docPath;
  private byte[] _filePath;

  private static int _timerid
  {
    get
    {
      lock (PdfForms._syncTimerId)
        return ++PdfForms.__timerid;
    }
  }

  /// <summary>
  /// Gets PDF Document associated with this <see cref="T:Patagames.Pdf.Net.PdfForms" />.
  /// </summary>
  public PdfDocument Document => this._doc;

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets the Pdfium SDK handle that the forms is bound to</summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets Interactive Forms object for current PDF document.
  /// </summary>
  public PdfInteractiveForms InterForm
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfInteractiveForms) null;
      if (this._interForm == null)
        this._interForm = new PdfInteractiveForms(this);
      return this._interForm;
    }
  }

  /// <summary>
  /// Gets or sets the object used to marshal event-handler calls that are issued from JS engine.
  /// </summary>
  public SynchronizationContext SynchronizationContext { get; set; }

  /// <summary>
  /// Gets or sets the object used to marshal event-handler calls that are issued from JS engine.
  /// </summary>
  [Obsolete("This property is obsolete. Please use SynchronizationContext instead", true)]
  public ISynchronizeInvoke SynchronizingObject { get; set; }

  /// <summary>
  /// Gets the text in the widget that is currently processing the input queue.
  /// </summary>
  public string FocusedText => Pdfium.FORM_GetFocusedText(this.Handle);

  /// <summary>
  /// Gets the selected text in the widget that is currently processing the input queue.
  /// </summary>
  public string SelectedText
  {
    get
    {
      int len;
      int startIndex = len = 0;
      Pdfium.FORM_GetSelectedText(this.Handle, out startIndex, out len);
      if (startIndex >= 0 && len > 0)
      {
        string str = this.FocusedText ?? "";
        if (str.Length >= startIndex + len)
          return str.Substring(startIndex, len);
      }
      return "";
    }
  }

  /// <summary>
  /// Give implementation a chance to release any data after the forms is no longer used
  /// </summary>
  /// <remarks>
  /// <para>Required: No.</para>
  /// <para>Called by SDK during the final cleanup process.</para></remarks>
  public event EventHandler Release;

  /// <summary>
  /// SDK will fire this event when the page need to be repainted.
  /// </summary>
  /// <remarks>
  /// <para>Required: Yes.</para>
  /// <para>All positions are measured in PDF "user space". Implementation should call <see cref="O:Patagames.Pdf.Net.PdfPage.Render" /> for repainting a specified page area.</para></remarks>
  public event EventHandler<InvalidatePageEventArgs> Invalidate;

  /// <summary>
  /// SDK fire this event when user is taking the mouse to select texts on a form field.
  /// </summary>
  /// <remarks>
  /// <para>Required: No.</para>
  /// <para>This event is useful for implementing special text selection effect. Implementation should
  /// first records the returned rectangles, then draw them one by one at the painting period, last,remove all
  /// the recorded rectangles when finish painting.</para></remarks>
  public event EventHandler<InvalidatePageEventArgs> OutputSelectedRect;

  /// <summary>
  /// Application shoud use this event for sets the specified cursor for the entire application.
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<SetCursorEventArgs> SetCursor;

  /// <summary>
  /// With this event SDK receives the current local time on the system.
  /// </summary>
  /// <remarks>Required: No.</remarks>
  public event EventHandler<LocalTimeEventArgs> LocalTime;

  /// <summary>
  /// This event will be invoked to notify implementation when the value of any FormField on the document had been changed.
  /// </summary>
  /// <remarks>Required: No.</remarks>
  public event EventHandler FieldChanged;

  /// <summary>
  /// This action resolves to a uniform resource identifier.
  /// </summary>
  /// <remarks>
  /// <para>required: No.</para>
  /// <para>See the URI actions description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> for more details.</para></remarks>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public event EventHandler<EventArgs<string>> DoURIAction;

  /// <summary>This event will execute an named action.</summary>
  /// <remarks>See the named actions description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> for more details.
  /// Required: Yes.</remarks>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public event EventHandler<EventArgs<string>> DoNamedAction;

  /// <summary>
  /// This event will be fired when a text field is getting or losing a focus.
  /// </summary>
  /// <remarks><para>Required: No.</para>
  /// <para>Currently,only support text field and combobox field.</para></remarks>
  public event EventHandler<FocusChangedEventArgs> FocusChanged;

  /// <summary>
  /// While processing this event pplication must be changes the view to a specified destination.
  /// </summary>
  /// <remarks><para>Required: No.</para>
  /// <para>See the Destinations description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> in 8.2.1 for more details.</para></remarks>
  /// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
  public event EventHandler<DoGotoActionEventArgs> DoGotoAction;

  /// <summary>pop up a dialog to show warning or hint.</summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<AppAlertEventEventArgs> AppAlert;

  /// <summary>Causes the system to play a sound.</summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<EventArgs<BeepTypes>> AppBeep;

  /// <summary>
  /// Displays a dialog box containing a question and an entry field for the user to reply to the question.
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<AppResponseEventArgs> AppResponse;

  /// <summary>Get the file path of the current document.</summary>
  public event EventHandler<EventArgs<string>> GetDocumentPath;

  /// <summary>
  /// Mails the data buffer as an attachment to all recipients, with or without user interaction.
  /// </summary>
  /// <remarks><para>Required: Yes.</para>
  /// <para>If the parameter mailData is NULL, the current document will be mailed as an attachment to all recipients.</para></remarks>
  public event EventHandler<SendMailEventArgs> SendMail;

  /// <summary>
  /// Prints all or a specific number of pages of the document.
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<PrintEventArgs> Print;

  /// <summary>Send the form data to a specified URL.</summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<SubmitFormEventArgs> SubmitForm;

  /// <summary>Jump to a specified page.</summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<EventArgs<int>> GotoPage;

  /// <summary>
  /// Show a file selection dialog, and return the selected file path.
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  public event EventHandler<BrowseFileEventArgs> BrowseFile;

  /// <summary>
  /// Alerts listeners when a text box or combo box value is about to change.
  /// </summary>
  public event EventHandler<FillFormNotifyBeforeEventArgs> BeforeValueChange;

  /// <summary>
  /// Occurs when the value of a text box or combo box is changed.
  /// </summary>
  public event EventHandler<FillFormNotifyAfterEventArgs> AfterValueChange;

  /// <summary>
  /// Alerts listeners when a selected index for a list box is about to change.
  /// </summary>
  public event EventHandler<FillFormNotifyBeforeEventArgs> BeforeSelectionChange;

  /// <summary>
  /// Occurs when the selected index for list box is changed.
  /// </summary>
  public event EventHandler<FillFormNotifyAfterEventArgs> AfterSelectionChange;

  /// <summary>
  /// Occurs when the checked statuses for check boxes or radion buttons are changed.
  /// </summary>
  public event EventHandler<FillFormNotifyAfterEventArgs> AfterCheckedStatusChange;

  /// <summary>
  /// Occurs when the <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ResetForm" /> or <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ResetForm(System.IntPtr,System.Boolean)" /> is calling.
  /// </summary>
  public event EventHandler<InterFormNotifyBeforeEventArgs> BeforeFormReset;

  /// <summary>
  /// Occurs when the <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ResetForm" /> or the <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ResetForm(System.IntPtr,System.Boolean)" /> is calling.
  /// </summary>
  public event EventHandler AfterFormReset;

  /// <summary>
  /// Occurs when the <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ImportFromFdf(Patagames.Pdf.Net.FdfDocument)" /> or the <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ImportFromFDF(System.IntPtr,System.IntPtr,System.Boolean)" /> is calling.
  /// </summary>
  public event EventHandler<InterFormNotifyBeforeEventArgs> BeforeFormImportData;

  /// <summary>
  /// Occurs when the <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ImportFromFdf(Patagames.Pdf.Net.FdfDocument)" /> or the <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ImportFromFDF(System.IntPtr,System.IntPtr,System.Boolean)" /> is calling.
  /// </summary>
  public event EventHandler AfterFormImportData;

  private void ReleaseCallback(FPDF_FORMFILLINFO pThis)
  {
    if (this.Release == null)
      return;
    this.Release((object) this, EventArgs.Empty);
  }

  private void FFI_InvalidateCallback(
    FPDF_FORMFILLINFO pThis,
    IntPtr page,
    double left,
    double top,
    double right,
    double bottom)
  {
    FS_RECTF rect = new FS_RECTF()
    {
      bottom = (float) bottom,
      left = (float) left,
      right = (float) right,
      top = (float) top
    };
    PdfPage byHandle = this._doc.Pages.GetByHandle(page);
    if (this.Invalidate == null)
      return;
    this.Invalidate((object) this, new InvalidatePageEventArgs(byHandle, rect));
  }

  private void FFI_OutputSelectedRectCallback(
    FPDF_FORMFILLINFO pThis,
    IntPtr page,
    double left,
    double top,
    double right,
    double bottom)
  {
    FS_RECTF rect = new FS_RECTF()
    {
      bottom = (float) bottom,
      left = (float) left,
      right = (float) right,
      top = (float) top
    };
    PdfPage byHandle = this._doc.Pages.GetByHandle(page);
    if (this.OutputSelectedRect == null)
      return;
    this.OutputSelectedRect((object) this, new InvalidatePageEventArgs(byHandle, rect));
  }

  private void FFI_SetCursorCallback(FPDF_FORMFILLINFO pThis, CursorTypes nCursorType)
  {
    if (this.SetCursor == null)
      return;
    this.SetCursor((object) this, new SetCursorEventArgs(nCursorType));
  }

  private int FFI_SetTimerCallback(FPDF_FORMFILLINFO pThis, int uElapse, Patagames.Pdf.TimerCallback lpTimerFunc)
  {
    TimerEx timerEx = new TimerEx(this.SynchronizationContext, uElapse, PdfForms._timerid, lpTimerFunc);
    this._timers.Add(timerEx.TimerId, timerEx);
    timerEx.Start();
    return timerEx.TimerId;
  }

  private void FFI_KillTimerCallback(FPDF_FORMFILLINFO pThis, int nTimerID)
  {
    if (!this._timers.ContainsKey(nTimerID))
      return;
    this._timers[nTimerID].Stop();
    this._timers.Remove(nTimerID);
  }

  private FPDF_SYSTEMTIME FFI_GetLocalTimeCallback(FPDF_FORMFILLINFO pThis)
  {
    DateTime dateTime;
    if (this.LocalTime != null)
    {
      LocalTimeEventArgs e = new LocalTimeEventArgs();
      this.LocalTime((object) this, e);
      dateTime = e.Time;
    }
    else
      dateTime = DateTime.Now;
    return new FPDF_SYSTEMTIME()
    {
      wYear = (ushort) dateTime.Year,
      wDay = (ushort) dateTime.Day,
      wDayOfWeek = (ushort) dateTime.DayOfWeek,
      wHour = (ushort) dateTime.Hour,
      wMinute = (ushort) dateTime.Minute,
      wMonth = (ushort) dateTime.Month,
      wSecond = (ushort) dateTime.Second,
      wMilliseconds = dateTime.Millisecond > 999 ? (ushort) 999 : (ushort) dateTime.Millisecond
    };
  }

  private void FFI_OnChangeCallback(FPDF_FORMFILLINFO pThis)
  {
    if (this.FieldChanged == null)
      return;
    this.FieldChanged((object) this, EventArgs.Empty);
  }

  /// <summary>
  /// This method receives the page pointer associated with a specified page index.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself.</param>
  /// <param name="document">Handle to document. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadDocument(System.String,System.String)" /> function.</param>
  /// <param name="nPageIndex">Index number of the page. 0 for the first page.</param>
  /// <returns>Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</returns>
  /// <remarks>Required: Yes. In some cases, the document-level JavaScript action may refer to a page which hadn't been loaded yet.
  /// To successfully run the javascript action, implementation need to load the page for SDK.</remarks>
  private IntPtr FFI_GetPageCallback(FPDF_FORMFILLINFO pThis, IntPtr document, int nPageIndex)
  {
    return nPageIndex >= this._doc.Pages.Count || nPageIndex < 0 ? IntPtr.Zero : this._doc.Pages[nPageIndex].Handle;
  }

  /// <summary>This method receives the current page pointer.</summary>
  /// <param name="pThis">Pointer to the interface structure itself.</param>
  /// <param name="document">Handle to document. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadDocument(System.String,System.String)" /> function.</param>
  /// <returns>Handle to the page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</returns>
  /// <remarks>Required: Yes.</remarks>
  private IntPtr FFI_GetCurrentPageCallback(FPDF_FORMFILLINFO pThis, IntPtr document)
  {
    return this._doc.Pages.CurrentPage == null ? IntPtr.Zero : this._doc.Pages.CurrentPage.Handle;
  }

  /// <summary>
  /// This method receives currently rotation of the page view.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself.</param>
  /// <param name="page">Handle to page. Returned by <see cref="M:Patagames.Pdf.Pdfium.FPDF_LoadPage(System.IntPtr,System.Int32)" /> function.</param>
  /// <returns>The page rotation. Should be 0(0 degree),1(90 degree),2(180 degree),3(270 degree), in a clockwise direction.</returns>
  /// <remarks>Required: Yes.</remarks>
  private PageRotate FFI_GetRotationCallback(FPDF_FORMFILLINFO pThis, IntPtr page)
  {
    PdfPage byHandle = this._doc.Pages.GetByHandle(page);
    return byHandle != null ? byHandle.Rotation : PageRotate.Normal;
  }

  private void FFI_DoURIActionCallback(FPDF_FORMFILLINFO pThis, string bsURI)
  {
    if (this.DoURIAction == null)
      return;
    this.DoURIAction((object) this, new EventArgs<string>(bsURI));
  }

  private void FFI_ExecuteNamedActionCallback(FPDF_FORMFILLINFO pThis, string namedAction)
  {
    if (this.DoNamedAction == null)
      return;
    this.DoNamedAction((object) this, new EventArgs<string>(namedAction));
  }

  private void FFI_SetTextFieldFocusCallback(
    FPDF_FORMFILLINFO pThis,
    string value,
    int valueLen,
    bool is_focus)
  {
    if (this.FocusChanged == null)
      return;
    this.FocusChanged((object) this, new FocusChangedEventArgs(value, is_focus));
  }

  private void FFI_DoGoToActionCallback(
    FPDF_FORMFILLINFO pThis,
    int nPageIndex,
    ZoomTypes zoomMode,
    float[] fPosArray,
    int sizeofArray)
  {
    if (this.DoGotoAction == null)
      return;
    this.DoGotoAction((object) this, new DoGotoActionEventArgs(nPageIndex, zoomMode, fPosArray));
  }

  private DialogResults app_alert_callback(
    IPDF_JSPLATFORM pThis,
    string Msg,
    string Title,
    ButtonTypes Type,
    IconTypes Icon)
  {
    if (this.AppAlert == null)
      return DialogResults.Ok;
    AppAlertEventEventArgs e = new AppAlertEventEventArgs(Msg, Title, Type, Icon);
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Send((SendOrPostCallback) (o => this.AppAlert((o as object[])[0], (o as object[])[1] as AppAlertEventEventArgs)), (object) new object[2]
      {
        (object) this,
        (object) e
      });
    else
      this.AppAlert((object) this, e);
    return e.DialogResult;
  }

  private void app_beep_callback(IPDF_JSPLATFORM pThis, BeepTypes nType)
  {
    if (this.AppBeep == null)
      return;
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Post((SendOrPostCallback) (o => this.AppBeep((o as object[])[0], (o as object[])[1] as EventArgs<BeepTypes>)), (object) new object[2]
      {
        (object) this,
        (object) new EventArgs<BeepTypes>(nType)
      });
    else
      this.AppBeep((object) this, new EventArgs<BeepTypes>(nType));
  }

  private int app_response_callback(
    IPDF_JSPLATFORM pThis,
    string Question,
    string Title,
    string Default,
    string cLabel,
    bool Password,
    IntPtr buffer,
    int buflen)
  {
    if (buffer == IntPtr.Zero || buflen == 0)
    {
      if (this.AppResponse == null)
        return 0;
      AppResponseEventArgs e = new AppResponseEventArgs(Question, Title, Default, cLabel, Password);
      if (this.SynchronizationContext != null)
        this.SynchronizationContext.Send((SendOrPostCallback) (o => this.AppResponse((o as object[])[0], (o as object[])[1] as AppResponseEventArgs)), (object) new object[2]
        {
          (object) this,
          (object) e
        });
      else
        this.AppResponse((object) this, e);
      this._response = Encoding.Unicode.GetBytes(e.UserResponse ?? "");
      return this._response.Length;
    }
    if (this._response == null)
      return 0;
    int length = Math.Min(buflen, this._response.Length);
    Marshal.Copy(this._response, 0, buffer, length);
    return this._response.Length;
  }

  private int Doc_getFilePath_callback(IPDF_JSPLATFORM pThis, IntPtr buffer, int buflen)
  {
    if (buffer == IntPtr.Zero || buflen == 0)
    {
      if (this.GetDocumentPath == null)
        return 0;
      EventArgs<string> e = new EventArgs<string>((string) null);
      if (this.SynchronizationContext != null)
        this.SynchronizationContext.Send((SendOrPostCallback) (o => this.GetDocumentPath((o as object[])[0], (o as object[])[1] as EventArgs<string>)), (object) new object[2]
        {
          (object) this,
          (object) e
        });
      else
        this.GetDocumentPath((object) this, e);
      byte[] bytes = Encoding.Unicode.GetBytes(e.Value ?? "");
      this._docPath = new byte[bytes.Length + 2];
      Array.Copy((Array) bytes, (Array) this._docPath, bytes.Length);
      return this._docPath.Length;
    }
    if (this._docPath == null)
      return 0;
    int length = Math.Min(buflen, this._docPath.Length);
    Marshal.Copy(this._docPath, 0, buffer, length);
    return this._docPath.Length;
  }

  private void Doc_mail_callback(
    IPDF_JSPLATFORM pThis,
    byte[] mailData,
    int length,
    bool bUI,
    string To,
    string Subject,
    string Cc,
    string Bcc,
    string Msg)
  {
    if (this.SendMail == null)
      return;
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Post((SendOrPostCallback) (o => this.SendMail((o as object[])[0], (o as object[])[1] as SendMailEventArgs)), (object) new object[2]
      {
        (object) this,
        (object) new SendMailEventArgs(To, Cc, Bcc, Subject, Msg, mailData, bUI)
      });
    else
      this.SendMail((object) this, new SendMailEventArgs(To, Cc, Bcc, Subject, Msg, mailData, bUI));
  }

  private void Doc_print_callback(
    IPDF_JSPLATFORM pThis,
    bool bUI,
    int nStart,
    int nEnd,
    bool bSilent,
    bool bShrinkToFit,
    bool bPrintAsImage,
    bool bReverse,
    bool bAnnotations)
  {
    if (this.Print == null)
      return;
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Post((SendOrPostCallback) (o => this.Print((o as object[])[0], (o as object[])[1] as PrintEventArgs)), (object) new object[2]
      {
        (object) this,
        (object) new PrintEventArgs(bUI, nStart, nEnd, bSilent, bShrinkToFit, bPrintAsImage, bReverse, bAnnotations)
      });
    else
      this.Print((object) this, new PrintEventArgs(bUI, nStart, nEnd, bSilent, bShrinkToFit, bPrintAsImage, bReverse, bAnnotations));
  }

  private void Doc_submitForm_callback(
    IPDF_JSPLATFORM pThis,
    byte[] formData,
    int length,
    string Url)
  {
    if (this.SubmitForm == null)
      return;
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Post((SendOrPostCallback) (o => this.SubmitForm((o as object[])[0], (o as object[])[1] as SubmitFormEventArgs)), (object) new object[2]
      {
        (object) this,
        (object) new SubmitFormEventArgs(formData, Url)
      });
    else
      this.SubmitForm((object) this, new SubmitFormEventArgs(formData, Url));
  }

  private void Doc_gotoPage_callback(IPDF_JSPLATFORM pThis, int nPageNum)
  {
    if (this.GotoPage == null)
      return;
    if (this.SynchronizationContext != null)
      this.SynchronizationContext.Post((SendOrPostCallback) (o => this.GotoPage((o as object[])[0], (o as object[])[1] as EventArgs<int>)), (object) new object[2]
      {
        (object) this,
        (object) new EventArgs<int>(nPageNum)
      });
    else
      this.GotoPage((object) this, new EventArgs<int>(nPageNum));
  }

  private int Field_browse_callback(IPDF_JSPLATFORM pThis, IntPtr filePath, int length)
  {
    if (filePath == IntPtr.Zero || length == 0)
    {
      if (this.BrowseFile == null)
        return 0;
      BrowseFileEventArgs e = new BrowseFileEventArgs();
      if (this.SynchronizationContext != null)
        this.SynchronizationContext.Send((SendOrPostCallback) (o => this.BrowseFile((o as object[])[0], (o as object[])[1] as BrowseFileEventArgs)), (object) new object[2]
        {
          (object) this,
          (object) e
        });
      else
        this.BrowseFile((object) this, e);
      byte[] bytes = Encoding.Unicode.GetBytes(e.FilePath ?? "");
      this._filePath = new byte[bytes.Length + 2];
      Array.Copy((Array) bytes, (Array) this._filePath, bytes.Length);
      return this._filePath.Length;
    }
    if (this._filePath == null)
      return 0;
    int length1 = Math.Min(length, this._filePath.Length);
    Marshal.Copy(this._filePath, 0, filePath, length1);
    return this._docPath.Length;
  }

  /// <summary>This method invoked by SDK before field is changed.</summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="field">Handle to Field object</param>
  /// <param name="value">Field's value</param>
  /// <returns>0 to accept changes, -1 to cancel</returns>
  /// <remarks>
  /// <para>Method is called by the text boxes and the combo boxes when they lose focus.</para>
  /// </remarks>
  protected virtual int OnBeforeValueChangeCallback(
    FPDF_FORMFILLNOTIFY pThis,
    IntPtr field,
    string value)
  {
    PdfField byHandle = this._interForm.Fields.GetByHandle(field);
    if (this.BeforeValueChange == null)
      return 0;
    FillFormNotifyBeforeEventArgs e = new FillFormNotifyBeforeEventArgs(byHandle, value);
    this.BeforeValueChange((object) this, e);
    return !e.IsCancel ? 0 : -1;
  }

  /// <summary>This method invoked by SDK after field is changed.</summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="field">Handle to Field object</param>
  /// <remarks>
  /// <para>Method is called by the text boxes and the combo boxes when they lose focus.</para>
  /// </remarks>
  protected virtual void OnAfterValueChangeCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr field)
  {
    PdfField byHandle = this._interForm.Fields.GetByHandle(field);
    if (this.AfterValueChange == null)
      return;
    this.AfterValueChange((object) this, new FillFormNotifyAfterEventArgs(byHandle));
  }

  /// <summary>
  /// This method invoked by SDK before field's selection is changed.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="field">Handle to Field object</param>
  /// <param name="value">Field's value</param>
  /// <returns>0 to accept changes, -1 to cancel</returns>
  /// <remarks>
  /// <para>Method is called by the list boxes when it lose focus.</para>
  /// </remarks>
  protected virtual int OnBeforeSelectionChangeCallback(
    FPDF_FORMFILLNOTIFY pThis,
    IntPtr field,
    string value)
  {
    PdfField byHandle = this._interForm.Fields.GetByHandle(field);
    if (this.BeforeSelectionChange == null)
      return 0;
    FillFormNotifyBeforeEventArgs e = new FillFormNotifyBeforeEventArgs(byHandle, value);
    this.BeforeSelectionChange((object) this, e);
    return !e.IsCancel ? 0 : -1;
  }

  /// <summary>
  /// This method invoked by SDK after field's selection is changed.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="field">Handle to Field object</param>
  /// <remarks>
  /// <para>Method is called by the list boxes when it lose focus.</para>
  /// </remarks>
  protected virtual void OnAfterSelectionChangeCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr field)
  {
    PdfField byHandle = this._interForm.Fields.GetByHandle(field);
    if (this.AfterSelectionChange == null)
      return;
    this.AfterSelectionChange((object) this, new FillFormNotifyAfterEventArgs(byHandle));
  }

  /// <summary>
  /// This method invoked by SDK after checked statuses is changed.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="field">Handle to Field object</param>
  /// <remarks>
  /// <para>Method is called by the check boxes and radio buttons when they lose focus.</para>
  /// </remarks>
  protected virtual void OnAfterCheckedStatusChangeCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr field)
  {
    PdfField byHandle = this._interForm.Fields.GetByHandle(field);
    if (this.AfterCheckedStatusChange == null)
      return;
    this.AfterCheckedStatusChange((object) this, new FillFormNotifyAfterEventArgs(byHandle));
  }

  /// <summary>This method invoked by SDK before forms is reseted.</summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="intForm">Handle to Interactive Forms object</param>
  /// <returns>0 to accept changes, -1 to cancel</returns>
  /// <remarks>
  /// <para>Method is called by <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ResetForm" /> and by <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ResetForm(System.IntPtr,System.Boolean)" /></para>
  /// </remarks>
  protected virtual int OnBeforeFormResetCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr intForm)
  {
    if (this.BeforeFormReset == null)
      return 0;
    InterFormNotifyBeforeEventArgs e = new InterFormNotifyBeforeEventArgs();
    this.BeforeFormReset((object) this, e);
    return !e.IsCancel ? 0 : -1;
  }

  /// <summary>This method invoked by SDK after forms is reseted.</summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="intForm">Handle to Interactive Forms object</param>
  /// <remarks>
  /// <para>Method is called by <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ResetForm" /> and by <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ResetForm(System.IntPtr,System.Boolean)" /></para>
  /// </remarks>
  protected virtual void OnAfterFormResetCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr intForm)
  {
    if (this.AfterFormReset == null)
      return;
    this.AfterFormReset((object) this, EventArgs.Empty);
  }

  /// <summary>
  /// This method invoked by SDK before forms data is imported.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="intForm">Handle to Interactive Forms object</param>
  /// <returns>0 to accept changes, -1 to cancel</returns>
  /// <remarks>
  /// <para>Method is called by <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ImportFromFdf(Patagames.Pdf.Net.FdfDocument)" /> and by <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ImportFromFDF(System.IntPtr,System.IntPtr,System.Boolean)" /></para>
  /// </remarks>
  protected virtual int OnBeforeFormImportDataCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr intForm)
  {
    if (this.BeforeFormImportData == null)
      return 0;
    InterFormNotifyBeforeEventArgs e = new InterFormNotifyBeforeEventArgs();
    this.BeforeFormImportData((object) this, e);
    return !e.IsCancel ? 0 : -1;
  }

  /// <summary>
  /// This method invoked by SDK after forms data is imported.
  /// </summary>
  /// <param name="pThis">Pointer to the interface structure itself</param>
  /// <param name="intForm">Handle to Interactive Forms object</param>
  /// <remarks>
  /// <para>Method is called by <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ImportFromFdf(Patagames.Pdf.Net.FdfDocument)" /> and by <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ImportFromFDF(System.IntPtr,System.IntPtr,System.Boolean)" /></para>
  /// </remarks>
  protected virtual void OnAfterFormImportDataCallback(FPDF_FORMFILLNOTIFY pThis, IntPtr intForm)
  {
    if (this.AfterFormImportData == null)
      return;
    this.AfterFormImportData((object) this, EventArgs.Empty);
  }

  /// <summary>Initializes a new instance of the PdfForms class.</summary>
  public PdfForms()
  {
    this._formInfo.FFI_DoGoToAction = new Patagames.Pdf.FFI_DoGoToActionCallback(this.FFI_DoGoToActionCallback);
    this._formInfo.FFI_DoURIAction = new Patagames.Pdf.FFI_DoURIActionCallback(this.FFI_DoURIActionCallback);
    this._formInfo.FFI_ExecuteNamedAction = new Patagames.Pdf.FFI_ExecuteNamedActionCallback(this.FFI_ExecuteNamedActionCallback);
    this._formInfo.FFI_GetCurrentPage = new Patagames.Pdf.FFI_GetCurrentPageCallback(this.FFI_GetCurrentPageCallback);
    this._formInfo.FFI_GetLocalTime = new Patagames.Pdf.FFI_GetLocalTimeCallback(this.FFI_GetLocalTimeCallback);
    this._formInfo.FFI_GetPage = new Patagames.Pdf.FFI_GetPageCallback(this.FFI_GetPageCallback);
    this._formInfo.FFI_GetRotation = new Patagames.Pdf.FFI_GetRotationCallback(this.FFI_GetRotationCallback);
    this._formInfo.FFI_Invalidate = new Patagames.Pdf.FFI_InvalidateCallback(this.FFI_InvalidateCallback);
    this._formInfo.FFI_KillTimer = new Patagames.Pdf.FFI_KillTimerCallback(this.FFI_KillTimerCallback);
    this._formInfo.FFI_OnChange = new Patagames.Pdf.FFI_OnChangeCallback(this.FFI_OnChangeCallback);
    this._formInfo.FFI_OutputSelectedRect = new Patagames.Pdf.FFI_OutputSelectedRectCallback(this.FFI_OutputSelectedRectCallback);
    this._formInfo.FFI_SetCursor = new Patagames.Pdf.FFI_SetCursorCallback(this.FFI_SetCursorCallback);
    this._formInfo.FFI_SetTextFieldFocus = new Patagames.Pdf.FFI_SetTextFieldFocusCallback(this.FFI_SetTextFieldFocusCallback);
    this._formInfo.FFI_SetTimer = new Patagames.Pdf.FFI_SetTimerCallback(this.FFI_SetTimerCallback);
    this._formInfo.Release = new Patagames.Pdf.ReleaseCallback(this.ReleaseCallback);
    this._jsPlatform.app_alert = new Patagames.Pdf.app_alert_callback(this.app_alert_callback);
    this._jsPlatform.app_beep = new Patagames.Pdf.app_beep_callback(this.app_beep_callback);
    this._jsPlatform.app_response = new Patagames.Pdf.app_response_callback(this.app_response_callback);
    this._jsPlatform.Doc_getFilePath = new Patagames.Pdf.Doc_getFilePath_callback(this.Doc_getFilePath_callback);
    this._jsPlatform.Doc_gotoPage = new Patagames.Pdf.Doc_gotoPage_callback(this.Doc_gotoPage_callback);
    this._jsPlatform.Doc_mail = new Patagames.Pdf.Doc_mail_callback(this.Doc_mail_callback);
    this._jsPlatform.Doc_print = new Patagames.Pdf.Doc_print_callback(this.Doc_print_callback);
    this._jsPlatform.Doc_submitForm = new Patagames.Pdf.Doc_submitForm_callback(this.Doc_submitForm_callback);
    this._jsPlatform.Field_browse = new Patagames.Pdf.Field_browse_callback(this.Field_browse_callback);
    for (int index = 0; index < 8; ++index)
      this._highlightColor[index] = new FS_COLOR(100, (int) byte.MaxValue, 228, 221);
    this._notify.BeforeValueChange = new BeforeValueChangeCallback(this.OnBeforeValueChangeCallback);
    this._notify.AfterValueChange = new AfterValueChangeCallback(this.OnAfterValueChangeCallback);
    this._notify.BeforeSelectionChange = new BeforeSelectionChangeCallback(this.OnBeforeSelectionChangeCallback);
    this._notify.AfterSelectionChange = new AfterSelectionChangeCallback(this.OnAfterSelectionChangeCallback);
    this._notify.AfterCheckedStatusChange = new AfterCheckedStatusChangeCallback(this.OnAfterCheckedStatusChangeCallback);
    this._notify.BeforeFormReset = new BeforeFormResetCallback(this.OnBeforeFormResetCallback);
    this._notify.AfterFormReset = new AfterFormResetCallback(this.OnAfterFormResetCallback);
    this._notify.BeforeFormImportData = new BeforeFormImportDataCallback(this.OnBeforeFormImportDataCallback);
    this._notify.AfterFormImportData = new AfterFormImportDataCallback(this.OnAfterFormImportDataCallback);
  }

  /// <summary>Release all resources used by PdfForms</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this._interForm != null)
      this._interForm = (PdfInteractiveForms) null;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFDOC_ExitFormFillEnvironment(this.Handle);
    this._doc = (PdfDocument) null;
    this.Handle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfForms()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfForms));
  }

  /// <summary>
  /// Init fillforms and execute javascript and OnOpen document's actions
  /// </summary>
  /// <param name="document">Instance of <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class that represents the current document</param>
  internal void Init(PdfDocument document)
  {
    this.IsDisposed = false;
    this._doc = this._doc == null ? document : throw new Exception(Error.err0008);
    this.Handle = Pdfium.FPDFDOC_InitFormFillEnvironment(this._doc.Handle, this._formInfo, this._jsPlatform);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this.SetHighlightColor(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, this._highlightColor[0]);
    this._interForm = new PdfInteractiveForms(this);
    Pdfium.FPDFInterForm_SetFormNotify(this._interForm.Handle, this._notify);
    Pdfium.FORM_DoDocumentJSAction(this.Handle);
    Pdfium.FORM_DoDocumentOpenAction(this.Handle);
  }

  /// <summary>
  /// Select text in the widget that is currently processing the input queue.
  /// </summary>
  /// <param name="startIndex">The character index from which to select the text.</param>
  /// <param name="length">The length of the text in characters to select.</param>
  public void SelectText(int startIndex, int length)
  {
    Pdfium.FORM_SetSelectedText(this.Handle, startIndex, length);
  }

  /// <summary>
  /// Set the highlight color of specified or all the form fields in the document.
  /// </summary>
  /// <param name="field">From field</param>
  /// <param name="color">color</param>
  /// <returns>Previouse color</returns>
  /// <remarks>
  /// <para>When the parameter fieldType is set to FPDF_FORMFIELD_UNKNOWN, the highlight color will be applied to all the form fields in the document.
  /// Please refresh the client window to show the highlight immediately if necessary.</para>
  /// <para><note type="note">Alpha channel is applied to all the form fields in the document even if field was specified.</note></para>
  /// </remarks>
  public FS_COLOR SetHighlightColor(FormFieldTypes field, FS_COLOR color)
  {
    int field_type = (int) field;
    if (field_type < 0)
      field_type = 0;
    if (field_type >= 8)
      field_type = 7;
    FS_COLOR fsColor = this._highlightColor[field_type];
    if (field_type == 0)
    {
      for (int index = 0; index < 8; ++index)
        this._highlightColor[field_type] = color;
    }
    else
    {
      this._highlightColor[field_type] = color;
      for (int index = 0; index < 8; ++index)
        this._highlightColor[field_type] = new FS_COLOR(color.A, this._highlightColor[field_type].R, this._highlightColor[field_type].G, this._highlightColor[field_type].B);
    }
    if (this.Handle == IntPtr.Zero)
      return fsColor;
    Pdfium.FPDF_SetFormFieldHighlightColor(this.Handle, (FormFieldTypes) field_type, color.ToArgb());
    Pdfium.FPDF_SetFormFieldHighlightAlpha(this.Handle, (byte) color.A);
    return fsColor;
  }

  /// <summary>
  /// Set the highlight color of specified or all the form fields in the document.
  /// </summary>
  /// <param name="field">From field</param>
  /// <param name="argb">Color in ARGB format</param>
  /// <returns>Previouse color</returns>
  /// <remarks>
  /// <para>When the parameter fieldType is set to FPDF_FORMFIELD_UNKNOWN, the highlight color will be applied to all the form fields in the document.
  /// Please refresh the client window to show the highlight immediately if necessary.</para>
  /// <para><note type="note">Alpha channel is applied to all the form fields in the document even if field was specified.</note></para>
  /// </remarks>
  public int SetHighlightColorEx(FormFieldTypes field, int argb)
  {
    return this.SetHighlightColor(field, new FS_COLOR(argb)).ToArgb();
  }

  /// <summary>
  /// Remove the form field highlight color in the document.
  /// </summary>
  /// <remarks>Please refresh the client window to remove the highlight immediately if necessary.</remarks>
  public void RemoveHighlightColor() => Pdfium.FPDF_RemoveFormFieldHighlight(this.Handle);

  /// <summary>
  /// You can call this member function to force to kill the focus of the form field which got focus.
  /// It would kill the focus on the form field, save the value of form field if it's changed by user.
  /// </summary>
  /// <returns>TRUE indicates success; otherwise false.</returns>
  public bool ForceToKillFocus() => Pdfium.FORM_ForceToKillFocus(this.Handle);
}

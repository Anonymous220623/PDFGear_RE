// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.SendMailEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.SendMail" /> event
/// </summary>
/// <remarks>Application should mails the data buffer as an attachment to all recipients, with or without user interaction. </remarks>
public class SendMailEventArgs : EventArgs
{
  /// <summary>Pointer to the data buffer to be sent.Can be NULL.</summary>
  public byte[] Data { get; private set; }

  /// <summary>
  /// If true, the rest of the parameters are used in a compose-new-message window that is displayed to the user. If false, the cTo parameter is required and all others are optional.
  /// </summary>
  public bool IsGUI { get; private set; }

  /// <summary>
  /// A semicolon-delimited list of recipients for the message.
  /// </summary>
  public string To { get; private set; }

  /// <summary>
  /// The subject of the message. The length limit is 64 KB.
  /// </summary>
  public string Subject { get; private set; }

  /// <summary>
  /// A semicolon-delimited list of CC recipients for the message.
  /// </summary>
  public string Cc { get; private set; }

  /// <summary>
  /// A semicolon-delimited list of BCC recipients for the message.
  /// </summary>
  public string Bcc { get; private set; }

  /// <summary>
  /// The content of the message. The length limit is 64 KB.
  /// </summary>
  public string Msg { get; private set; }

  /// <summary>Construct SendMailEventArgs object</summary>
  /// <param name="To">A semicolon-delimited list of recipients for the message.</param>
  /// <param name="Cc">A semicolon-delimited list of CC recipients for the message.</param>
  /// <param name="Bcc">A semicolon-delimited list of BCC recipients for the message.</param>
  /// <param name="Subject">Subject</param>
  /// <param name="Msg">The content of the message. The length limit is 64 KB.</param>
  /// <param name="Data">Pointer to the data buffer to be sent. Can be NULL.</param>
  /// <param name="IsGUI">If true, the rest of the parameters are used in a compose-new-message window that is displayed to the user. If false, the cTo parameter is required and all others are optional.</param>
  public SendMailEventArgs(
    string To,
    string Cc,
    string Bcc,
    string Subject,
    string Msg,
    byte[] Data,
    bool IsGUI)
  {
    this.To = To;
    this.Cc = Cc;
    this.Bcc = Bcc;
    this.Subject = Subject;
    this.Msg = Msg;
    this.Data = Data;
    this.IsGUI = IsGUI;
  }
}

// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.EmailMessage
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Utils.Email;

public class EmailMessage
{
  private List<string> _attachmentFilePaths;
  private List<string> _to;
  private List<string> _bcc;
  private List<string> _cc;

  public string Subject { get; set; }

  public IList<string> AttachmentFilePath => (IList<string>) this._attachmentFilePaths;

  public IList<string> To => (IList<string>) this._to;

  public IList<string> Cc => (IList<string>) this._cc;

  public IList<string> Bcc => (IList<string>) this._bcc;

  public string Body { get; set; }

  public EmailMessage()
  {
    this._to = new List<string>();
    this._cc = new List<string>();
    this._bcc = new List<string>();
    this._attachmentFilePaths = new List<string>();
    this.Subject = "";
    this.Body = "";
  }

  public bool Send() => MailApiProvider.SendMessage(this);
}

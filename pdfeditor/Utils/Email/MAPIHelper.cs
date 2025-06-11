// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.MAPIHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils.Email;

internal class MAPIHelper
{
  private bool _useUnicode = Environment.OSVersion.Version >= new Version(6, 2);
  private readonly string[] Errors = new string[27]
  {
    "OK [0]",
    "User abort [1]",
    "General MAPI failure [2]",
    "MAPI login failure [3]",
    "Disk full [4]",
    "Insufficient memory [5]",
    "Access denied [6]",
    "-unknown- [7]",
    "Too many sessions [8]",
    "Too many files were specified [9]",
    "Too many recipients were specified [10]",
    "A specified attachment was not found [11]",
    "Attachment open failure [12]",
    "Attachment write failure [13]",
    "Unknown recipient [14]",
    "Bad recipient type [15]",
    "No messages [16]",
    "Invalid message [17]",
    "Text too large [18]",
    "Invalid session [19]",
    "Type not supported [20]",
    "A recipient was specified ambiguously [21]",
    "Message in use [22]",
    "Network failure [23]",
    "Invalid edit fields [24]",
    "Invalid recipients [25]",
    "Not supported [26]"
  };
  private readonly List<MapiRecipDesc> m_recipients = new List<MapiRecipDesc>();
  private readonly List<string> m_attachments = new List<string>();
  private int m_lastError;
  private const int MAPI_LOGON_UI = 1;
  private const int MAPI_DIALOG = 8;
  private const int maxAttachments = 20;

  public bool AddRecipientTo(string email) => this.AddRecipient(email, MAPIHelper.HowTo.MAPI_TO);

  public bool AddRecipientCc(string email) => this.AddRecipient(email, MAPIHelper.HowTo.MAPI_CC);

  public bool AddRecipientBcc(string email) => this.AddRecipient(email, MAPIHelper.HowTo.MAPI_BCC);

  public void AddAttachment(string strAttachmentFileName)
  {
    this.m_attachments.Add(strAttachmentFileName);
  }

  public bool SendMailPopup(string strSubject, string strBody)
  {
    return this.SendMail(strSubject, strBody, 9);
  }

  public bool SendMailDirect(string strSubject, string strBody)
  {
    return this.SendMail(strSubject, strBody, 1);
  }

  [DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
  private static extern int MAPISendMail(
    IntPtr sess,
    IntPtr hwnd,
    MapiMessage message,
    int flg,
    int rsv);

  [DllImport("MAPI32.DLL", CharSet = CharSet.Unicode)]
  private static extern int MAPISendMailW(
    IntPtr sess,
    IntPtr hwnd,
    MapiMessageW message,
    int flg,
    int rsv);

  private bool SendMail(string strSubject, string strBody, int how)
  {
    MapiMessage msg = new MapiMessage()
    {
      subject = strSubject,
      noteText = strBody
    };
    msg.recips = this.GetRecipients(out msg.recipCount);
    msg.files = this.GetAttachments(out msg.fileCount);
    this.m_lastError = this._useUnicode ? MAPIHelper.MAPISendMailW(new IntPtr(0), new IntPtr(0), new MapiMessageW(msg), how, 0) : MAPIHelper.MAPISendMail(new IntPtr(0), new IntPtr(0), msg, how, 0);
    int num = this.m_lastError == 0 ? 1 : 0;
    this.Cleanup(ref msg);
    return num != 0;
  }

  private bool AddRecipient(string email, MAPIHelper.HowTo howTo)
  {
    this.m_recipients.Add(new MapiRecipDesc()
    {
      recipClass = (int) howTo,
      name = email
    });
    return true;
  }

  private IntPtr GetRecipients(out int recipCount)
  {
    recipCount = 0;
    if (this.m_recipients.Count == 0)
      return IntPtr.Zero;
    int offset = Marshal.SizeOf(this._useUnicode ? typeof (MapiRecipDescW) : typeof (MapiRecipDesc));
    IntPtr recipients = Marshal.AllocHGlobal(this.m_recipients.Count * offset);
    IntPtr num = recipients;
    foreach (MapiRecipDesc recipient in this.m_recipients)
    {
      if (this._useUnicode)
        Marshal.StructureToPtr<MapiRecipDescW>(new MapiRecipDescW(recipient), num, false);
      else
        Marshal.StructureToPtr<MapiRecipDesc>(recipient, num, false);
      IntPtr.Add(num, offset);
    }
    recipCount = this.m_recipients.Count;
    return recipients;
  }

  private IntPtr GetAttachments(out int fileCount)
  {
    fileCount = 0;
    if (this.m_attachments == null || this.m_attachments.Count <= 0 || this.m_attachments.Count > 20)
      return IntPtr.Zero;
    int offset = Marshal.SizeOf(this._useUnicode ? typeof (MapiFileDescW) : typeof (MapiFileDesc));
    IntPtr attachments = Marshal.AllocHGlobal(this.m_attachments.Count * offset);
    MapiFileDesc mapiFileDesc = new MapiFileDesc()
    {
      position = -1
    };
    IntPtr num = attachments;
    foreach (string attachment in this.m_attachments)
    {
      mapiFileDesc.name = Path.GetFileName(attachment);
      mapiFileDesc.path = attachment;
      if (this._useUnicode)
        Marshal.StructureToPtr<MapiFileDescW>(new MapiFileDescW(mapiFileDesc), num, false);
      else
        Marshal.StructureToPtr<MapiFileDesc>(mapiFileDesc, num, false);
      IntPtr.Add(num, offset);
    }
    fileCount = this.m_attachments.Count;
    return attachments;
  }

  private void Cleanup(ref MapiMessage msg)
  {
    FreeStruct(this._useUnicode ? typeof (MapiRecipDescW) : typeof (MapiRecipDesc), msg.recips, msg.recipCount);
    FreeStruct(this._useUnicode ? typeof (MapiFileDescW) : typeof (MapiFileDesc), msg.files, msg.fileCount);
    this.m_recipients.Clear();
    this.m_attachments.Clear();
    this.m_lastError = 0;

    static void FreeStruct(Type type, IntPtr structPtr, int count)
    {
      int offset = Marshal.SizeOf(type);
      if (structPtr == IntPtr.Zero)
        return;
      IntPtr num = structPtr;
      for (int index = 0; index < count; ++index)
      {
        Marshal.DestroyStructure(num, type);
        IntPtr.Add(num, offset);
      }
      Marshal.FreeHGlobal(structPtr);
    }
  }

  public string GetLastError()
  {
    return this.m_lastError >= 0 && this.m_lastError <= 26 ? this.Errors[this.m_lastError] : $"MAPI error [{this.m_lastError.ToString()}]";
  }

  private enum HowTo
  {
    MAPI_ORIG,
    MAPI_TO,
    MAPI_CC,
    MAPI_BCC,
  }
}

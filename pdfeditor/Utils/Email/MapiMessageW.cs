// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.MapiMessageW
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils.Email;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal class MapiMessageW
{
  public int reserved;
  public string subject;
  public string noteText;
  public string messageType;
  public string dateReceived;
  public string conversationID;
  public int flags;
  public IntPtr originator;
  public int recipCount;
  public IntPtr recips;
  public int fileCount;
  public IntPtr files;

  public MapiMessageW(MapiMessage message)
  {
    this.reserved = message.reserved;
    this.subject = message.subject;
    this.noteText = message.noteText;
    this.messageType = message.messageType;
    this.dateReceived = message.dateReceived;
    this.conversationID = message.conversationID;
    this.flags = message.flags;
    this.originator = message.originator;
    this.recipCount = message.recipCount;
    this.recips = message.recips;
    this.fileCount = message.fileCount;
    this.files = message.files;
  }
}

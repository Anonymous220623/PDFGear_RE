// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ChatTextHyperlinkClickEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Copilot;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Copilot;

public class ChatTextHyperlinkClickEventArgs : RoutedEventArgs
{
  public ChatTextHyperlinkClickEventArgs(
    RoutedEvent routedEvent,
    object source,
    ChatTextHyperlinkClickAction action,
    string text,
    CopilotHelper.AppActionModel appAction)
    : base(routedEvent, source)
  {
    this.Action = action;
    this.Text = text;
    this.AppAction = appAction;
  }

  public string Text { get; }

  public CopilotHelper.AppActionModel AppAction { get; }

  public ChatTextHyperlinkClickAction Action { get; }
}

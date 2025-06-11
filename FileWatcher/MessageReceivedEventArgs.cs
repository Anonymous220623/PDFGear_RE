// Decompiled with JetBrains decompiler
// Type: FileWatcher.MessageReceivedEventArgs
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

#nullable disable
namespace FileWatcher;

internal class MessageReceivedEventArgs
{
  public MessageReceivedEventArgs(MessageData messageData) => this.MessageData = messageData;

  public MessageData MessageData { get; }
}

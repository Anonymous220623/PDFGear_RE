// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.ImeReceiverTextReceivedEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit.Contents;

public class ImeReceiverTextReceivedEventArgs : EventArgs
{
  internal ImeReceiverTextReceivedEventArgs(string text) => this.Text = text;

  public string Text { get; }
}

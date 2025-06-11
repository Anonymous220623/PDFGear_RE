// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.DrawControlOperation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class DrawControlOperation : DrawOperation
{
  public DrawControlOperation(OperationType type, UIElement element)
  {
    this.Type = type;
    this.Element = element;
  }
}

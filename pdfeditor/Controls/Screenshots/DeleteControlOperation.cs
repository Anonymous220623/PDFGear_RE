// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.DeleteControlOperation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class DeleteControlOperation : DrawOperation
{
  public double Left { get; private set; }

  public double Top { get; private set; }

  public Rect Location { get; private set; }

  public DeleteControlOperation(UIElement element, double left, double top)
  {
    this.Type = OperationType.DeleteControl;
    this.Element = element;
    this.Left = left;
    this.Top = top;
  }
}

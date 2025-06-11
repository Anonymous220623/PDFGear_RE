// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.MoveControlOperation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class MoveControlOperation : DrawOperation
{
  public TransformGroup OriginalTransformGroup { get; private set; }

  public MoveControlOperation(UIElement element, TransformGroup originalTransformGroup)
  {
    this.Type = OperationType.MoveControl;
    this.Element = element;
    this.OriginalTransformGroup = originalTransformGroup;
  }
}

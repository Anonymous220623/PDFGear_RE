// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TransformationStack
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class TransformationStack
{
  private Matrix currentTransform = Matrix.Identity;
  private Matrix initialTransform = Matrix.Identity;
  private Stack<Matrix> transformStack = new Stack<Matrix>();

  internal Matrix CurrentTransform
  {
    get
    {
      return this.transformStack.Count == 0 ? this.initialTransform : this.currentTransform * this.initialTransform;
    }
  }

  internal TransformationStack() => this.initialTransform = Matrix.Identity;

  internal TransformationStack(Matrix initialTransform) => this.initialTransform = initialTransform;

  internal void PushTransform(Matrix transformMatrix)
  {
    this.transformStack.Push(transformMatrix);
    Matrix identity = Matrix.Identity;
    foreach (Matrix transform in this.transformStack)
      identity *= transform;
    this.currentTransform = identity;
  }

  internal void PopTransform()
  {
    this.transformStack.Pop();
    Matrix identity = Matrix.Identity;
    foreach (Matrix transform in this.transformStack)
      identity *= transform;
    this.currentTransform = identity;
  }

  internal void Clear() => this.transformStack.Clear();
}

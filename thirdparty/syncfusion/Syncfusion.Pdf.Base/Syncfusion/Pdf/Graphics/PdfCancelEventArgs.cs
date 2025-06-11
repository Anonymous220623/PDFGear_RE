// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfCancelEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfCancelEventArgs : EventArgs
{
  private bool m_cancel;

  public bool Cancel
  {
    get => this.m_cancel;
    set => this.m_cancel = value;
  }
}

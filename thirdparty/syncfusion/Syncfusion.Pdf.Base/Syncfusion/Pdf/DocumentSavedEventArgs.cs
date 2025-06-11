// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DocumentSavedEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class DocumentSavedEventArgs : EventArgs
{
  private PdfWriter m_writer;

  internal DocumentSavedEventArgs(PdfWriter writer)
  {
    this.m_writer = writer != null ? writer : throw new ArgumentNullException(nameof (writer));
  }

  internal PdfWriter Writer => this.m_writer;
}

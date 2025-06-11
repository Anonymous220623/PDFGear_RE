// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignatureEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfSignatureEventArgs : EventArgs
{
  private byte[] m_data;
  private byte[] m_signedData;

  public byte[] Data => this.m_data;

  public byte[] SignedData
  {
    get => this.m_signedData;
    set
    {
      if (value == null)
        return;
      this.m_signedData = value;
    }
  }

  internal PdfSignatureEventArgs(byte[] documentData) => this.m_data = documentData;
}

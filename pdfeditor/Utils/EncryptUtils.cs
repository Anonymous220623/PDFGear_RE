// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.EncryptUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;

#nullable disable
namespace pdfeditor.Utils;

public static class EncryptUtils
{
  public static bool VerifyOwerpassword(string pdfpath, string password)
  {
    PdfDocument pdfDocument = (PdfDocument) null;
    try
    {
      pdfDocument = PdfDocument.Load(pdfpath, password: password);
      return Pdfium.FPDF_IsOwnerPasswordIsUsed(pdfDocument.Handle);
    }
    catch
    {
      return false;
    }
    finally
    {
      switch (pdfDocument)
      {
        case null:
        case null:
        default:
          // ISSUE: explicit non-virtual call
          __nonvirtual (pdfDocument.Dispose());
          goto case null;
      }
    }
  }
}

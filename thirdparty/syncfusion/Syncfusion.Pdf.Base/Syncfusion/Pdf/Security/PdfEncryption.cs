// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfEncryption
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfEncryption
{
  internal static long Sequence = DateTime.Now.Ticks + (long) Environment.TickCount;

  internal static byte[] CreateDocumentId()
  {
    return new MessageDigestAlgorithms().Digest("MD5", Encoding.ASCII.GetBytes($"{(object) (DateTime.Now.Ticks + (long) Environment.TickCount)}+{(object) GC.GetTotalMemory(false)}+{(object) PdfEncryption.Sequence++}"));
  }
}

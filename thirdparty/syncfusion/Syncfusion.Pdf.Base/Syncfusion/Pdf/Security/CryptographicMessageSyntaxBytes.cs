// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CryptographicMessageSyntaxBytes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CryptographicMessageSyntaxBytes
{
  private readonly byte[] m_bytes;

  internal CryptographicMessageSyntaxBytes(byte[] bytes) => this.m_bytes = bytes;

  internal virtual void Write(Stream stream) => stream.Write(this.m_bytes, 0, this.m_bytes.Length);
}

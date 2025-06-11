// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyParameter : ICipherParam
{
  private readonly byte[] m_bytes;

  internal byte[] Keys => (byte[]) this.m_bytes.Clone();

  internal KeyParameter(byte[] bytes)
  {
    this.m_bytes = bytes != null ? (byte[]) bytes.Clone() : throw new ArgumentNullException(nameof (bytes));
  }

  internal KeyParameter(byte[] bytes, int offset, int length)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    if (offset < 0 || offset > bytes.Length)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length < 0 || offset + length > bytes.Length)
      throw new ArgumentOutOfRangeException(nameof (length));
    this.m_bytes = new byte[length];
    Array.Copy((Array) bytes, offset, (Array) this.m_bytes, 0, length);
  }
}

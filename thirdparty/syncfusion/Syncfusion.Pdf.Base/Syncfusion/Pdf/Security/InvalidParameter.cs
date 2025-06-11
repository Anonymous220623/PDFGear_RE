// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.InvalidParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class InvalidParameter : ICipherParam
{
  private readonly ICipherParam m_parameters;
  private readonly byte[] m_bytes;

  internal InvalidParameter(ICipherParam parameters, byte[] bytes)
    : this(parameters, bytes, 0, bytes.Length)
  {
  }

  internal InvalidParameter(ICipherParam parameters, byte[] bytes, int offset, int length)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    this.m_parameters = parameters;
    this.m_bytes = new byte[length];
    Array.Copy((Array) bytes, offset, (Array) this.m_bytes, 0, length);
  }

  internal byte[] InvalidBytes => (byte[]) this.m_bytes.Clone();

  internal ICipherParam Parameters => this.m_parameters;
}

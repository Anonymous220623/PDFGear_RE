// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PasswordGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class PasswordGenerator
{
  protected byte[] m_password;
  protected byte[] m_value;
  protected int m_count;

  internal abstract ICipherParam GenerateParam(string algorithm, int keySize);

  internal abstract ICipherParam GenerateParam(string algorithm, int keySize, int size);

  internal abstract ICipherParam GenerateParam(int keySize);

  internal virtual void Init(byte[] password, byte[] value, int count)
  {
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    this.m_password = Asn1Constants.Clone(password);
    this.m_value = Asn1Constants.Clone(value);
    this.m_count = count;
  }

  internal static byte[] ToBytes(char[] password, bool isWrong)
  {
    if (password == null || password.Length < 1)
      return new byte[isWrong ? 2 : 0];
    byte[] bytes = new byte[(password.Length + 1) * 2];
    Encoding.BigEndianUnicode.GetBytes(password, 0, password.Length, bytes, 0);
    return bytes;
  }
}

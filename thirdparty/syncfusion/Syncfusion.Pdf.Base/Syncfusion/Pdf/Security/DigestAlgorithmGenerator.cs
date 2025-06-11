// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DigestAlgorithmGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DigestAlgorithmGenerator : IAlgorithmGenerator
{
  private const long m_count = 10;
  private long m_stateCount;
  private long m_seedCount;
  private IMessageDigest m_digest;
  private byte[] m_state;
  private byte[] m_bytes;

  internal DigestAlgorithmGenerator(IMessageDigest digest)
  {
    this.m_digest = digest;
    this.m_bytes = new byte[digest.MessageDigestSize];
    this.m_seedCount = 1L;
    this.m_state = new byte[digest.MessageDigestSize];
    this.m_stateCount = 1L;
  }

  public void AddMaterial(byte[] bytes)
  {
    lock (this)
    {
      this.DigestUpdate(bytes);
      this.DigestUpdate(this.m_bytes);
      this.DigestDoFinal(this.m_bytes);
    }
  }

  public void AddMaterial(long value)
  {
    lock (this)
    {
      this.AddToCounter(value);
      this.DigestUpdate(this.m_bytes);
      this.DigestDoFinal(this.m_bytes);
    }
  }

  public void FillNextBytes(byte[] bytes) => this.FillNextBytes(bytes, 0, bytes.Length);

  public void FillNextBytes(byte[] bytes, int index, int length)
  {
    lock (this)
    {
      int num1 = 0;
      this.GenerateState();
      int num2 = index + length;
      for (int index1 = index; index1 < num2; ++index1)
      {
        if (num1 == this.m_state.Length)
        {
          this.GenerateState();
          num1 = 0;
        }
        bytes[index1] = this.m_state[num1++];
      }
    }
  }

  private void GenerateState()
  {
    this.AddToCounter(this.m_stateCount++);
    this.DigestUpdate(this.m_state);
    this.DigestUpdate(this.m_bytes);
    this.DigestDoFinal(this.m_state);
    if (this.m_stateCount % 10L != 0L)
      return;
    this.DigestUpdate(this.m_bytes);
    this.AddToCounter(this.m_seedCount++);
    this.DigestDoFinal(this.m_bytes);
  }

  private void AddToCounter(long value)
  {
    ulong input = (ulong) value;
    for (int index = 0; index != 8; ++index)
    {
      this.m_digest.Update((byte) input);
      input >>= 8;
    }
  }

  private void DigestUpdate(byte[] bytes) => this.m_digest.Update(bytes, 0, bytes.Length);

  private void DigestDoFinal(byte[] bytes) => this.m_digest.DoFinal(bytes, 0);
}

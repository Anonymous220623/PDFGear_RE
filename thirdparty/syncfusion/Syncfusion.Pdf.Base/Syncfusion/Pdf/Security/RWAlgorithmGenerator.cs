// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RWAlgorithmGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RWAlgorithmGenerator : IAlgorithmGenerator
{
  private IAlgorithmGenerator m_generator;
  private byte[] m_window;
  private int m_windowCount;

  internal RWAlgorithmGenerator(IAlgorithmGenerator generator, int windowSize)
  {
    if (generator == null)
      throw new ArgumentNullException(nameof (generator));
    if (windowSize < 2)
      throw new ArgumentException("Invalid size. Window size must be at least 2");
    this.m_generator = generator;
    this.m_window = new byte[windowSize];
  }

  public virtual void AddMaterial(byte[] bytes)
  {
    lock (this)
    {
      this.m_windowCount = 0;
      this.m_generator.AddMaterial(bytes);
    }
  }

  public virtual void AddMaterial(long value)
  {
    lock (this)
    {
      this.m_windowCount = 0;
      this.m_generator.AddMaterial(value);
    }
  }

  public virtual void FillNextBytes(byte[] bytes) => this.doNextBytes(bytes, 0, bytes.Length);

  public virtual void FillNextBytes(byte[] bytes, int start, int length)
  {
    this.doNextBytes(bytes, start, length);
  }

  private void doNextBytes(byte[] bytes, int start, int length)
  {
    lock (this)
    {
      for (int index = 0; index < length; bytes[start + index++] = this.m_window[--this.m_windowCount])
      {
        if (this.m_windowCount < 1)
        {
          this.m_generator.FillNextBytes(this.m_window, 0, this.m_window.Length);
          this.m_windowCount = this.m_window.Length;
        }
      }
    }
  }
}

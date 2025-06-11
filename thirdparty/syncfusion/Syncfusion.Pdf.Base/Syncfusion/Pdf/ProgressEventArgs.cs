// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ProgressEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

public class ProgressEventArgs
{
  private int m_total;
  private int m_current;
  private int m_pageProcessed = -1;
  private int m_changedPages;

  internal ProgressEventArgs(int current, int total)
  {
    if (total <= 0)
      throw new ArgumentOutOfRangeException(nameof (total), "Total is less then or equal to zero.");
    this.m_current = current >= 0 ? current : throw new ArgumentOutOfRangeException(nameof (current), "Current can't be less then zero.");
    this.m_total = total;
  }

  internal ProgressEventArgs(int current, int total, int processed)
    : this(current, total)
  {
    this.m_pageProcessed = processed;
  }

  internal ProgressEventArgs(int current, int total, int processed, int changed)
    : this(current, total, processed)
  {
    this.m_changedPages = changed;
  }

  private ProgressEventArgs()
  {
  }

  public int Total => this.m_total;

  public int Current => this.m_current;

  public float Progress
  {
    get
    {
      return (float) ((this.m_pageProcessed != -1 ? (double) this.m_pageProcessed : (double) this.Current) / (this.m_changedPages > 0 ? (double) this.m_changedPages : (double) this.Total));
    }
  }
}

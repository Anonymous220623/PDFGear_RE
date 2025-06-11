// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ArithmeticDecoderStats
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ArithmeticDecoderStats
{
  private int m_contextSize;
  private int[] m_codingContextTable;

  internal int ContextSize => this.m_contextSize;

  internal ArithmeticDecoderStats(int contextSize)
  {
    this.m_contextSize = contextSize;
    this.m_codingContextTable = new int[contextSize];
    this.reset();
  }

  internal void reset()
  {
    for (int index = 0; index < this.m_contextSize; ++index)
      this.m_codingContextTable[index] = 0;
  }

  internal void setEntry(int codingContext, int i, int moreProbableSymbol)
  {
    this.m_codingContextTable[codingContext] = (i << i) + moreProbableSymbol;
  }

  internal int getContextCodingTableValue(int index) => this.m_codingContextTable[index];

  internal void setContextCodingTableValue(int index, int value)
  {
    this.m_codingContextTable[index] = value;
  }

  internal void overwrite(ArithmeticDecoderStats stats)
  {
    Array.Copy((Array) stats.m_codingContextTable, 0, (Array) this.m_codingContextTable, 0, this.m_contextSize);
  }

  internal ArithmeticDecoderStats copy()
  {
    ArithmeticDecoderStats arithmeticDecoderStats = new ArithmeticDecoderStats(this.m_contextSize);
    Array.Copy((Array) this.m_codingContextTable, 0, (Array) arithmeticDecoderStats.m_codingContextTable, 0, this.m_contextSize);
    return arithmeticDecoderStats;
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.QueryRowCountEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class QueryRowCountEventArgs : EventArgs
{
  private int m_rowCount;

  public int RowCount
  {
    get => this.m_rowCount;
    set => this.m_rowCount = value > 0 ? value : throw new ArgumentOutOfRangeException("RowNumber");
  }

  internal QueryRowCountEventArgs()
  {
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.ProgressMgr
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class ProgressMgr
{
  private int m_passCounter;
  private int m_passLimit;
  private int m_completedPasses;
  private int m_totalPasses;

  public event EventHandler OnProgress;

  public int Pass_counter
  {
    get => this.m_passCounter;
    set => this.m_passCounter = value;
  }

  public int Pass_limit
  {
    get => this.m_passLimit;
    set => this.m_passLimit = value;
  }

  public int Completed_passes
  {
    get => this.m_completedPasses;
    set => this.m_completedPasses = value;
  }

  public int Total_passes
  {
    get => this.m_totalPasses;
    set => this.m_totalPasses = value;
  }

  public void Updated()
  {
    if (this.OnProgress == null)
      return;
    this.OnProgress((object) this, new EventArgs());
  }
}

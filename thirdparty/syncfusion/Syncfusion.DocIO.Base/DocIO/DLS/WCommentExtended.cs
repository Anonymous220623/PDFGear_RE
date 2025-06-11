// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCommentExtended
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WCommentExtended
{
  private string m_paraId;
  private string m_parentParaId;
  private bool m_isResolved;

  internal string ParaId
  {
    get => this.m_paraId;
    set => this.m_paraId = value;
  }

  internal string ParentParaId
  {
    get => this.m_parentParaId;
    set => this.m_parentParaId = value;
  }

  internal bool IsResolved
  {
    get => this.m_isResolved;
    set => this.m_isResolved = value;
  }
}

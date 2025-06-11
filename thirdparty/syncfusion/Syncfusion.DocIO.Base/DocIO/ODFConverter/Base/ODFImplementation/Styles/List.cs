// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.Styles.List
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.Styles;

internal class List
{
  private string m_continueList;
  private bool m_isContinueNumbering;
  private ListHeader m_listHeader;
  private ListItem m_listItem;

  internal ListItem ListItem
  {
    get => this.m_listItem;
    set => this.m_listItem = value;
  }

  internal ListHeader ListHeader
  {
    get => this.m_listHeader;
    set => this.m_listHeader = value;
  }

  internal bool IsContinueNumbering
  {
    get => this.m_isContinueNumbering;
    set => this.m_isContinueNumbering = value;
  }

  internal string ContinueList
  {
    get => this.m_continueList;
    set => this.m_continueList = value;
  }
}

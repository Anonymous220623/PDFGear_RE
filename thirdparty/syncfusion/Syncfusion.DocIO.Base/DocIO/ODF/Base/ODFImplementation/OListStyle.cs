// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OListStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OListStyle
{
  private List<ListLevelProperties> m_listLevels;
  private string m_name;
  private string m_currentStyleName;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string CurrentStyleName
  {
    get => this.m_currentStyleName;
    set => this.m_currentStyleName = value;
  }

  internal List<ListLevelProperties> ListLevels
  {
    get
    {
      if (this.m_listLevels == null)
        this.m_listLevels = new List<ListLevelProperties>();
      return this.m_listLevels;
    }
  }

  internal void Close()
  {
    if (this.m_listLevels == null)
      return;
    foreach (ListLevelProperties listLevel in this.m_listLevels)
      listLevel.Close();
    this.m_listLevels.Clear();
    this.m_listLevels = (List<ListLevelProperties>) null;
  }
}

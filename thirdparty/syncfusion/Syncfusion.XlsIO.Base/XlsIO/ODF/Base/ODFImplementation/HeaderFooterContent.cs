// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.HeaderFooterContent
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class HeaderFooterContent
{
  private bool m_display;
  private HeaderSection m_regionCenter;
  private HeaderSection m_regionLeft;
  private HeaderSection m_regionRight;
  private OTable m_table;
  private string m_alphabeticalIndex;
  private string m_alphabeticalIndexAutoMarkFile;
  private string m_bibiliography;
  private string m_change;
  private string m_changeEnd;
  private string m_changeStart;
  private string m_DDEConnectionDecls;
  private string m_heading;
  private string m_illustrationIndex;
  private string m_list;
  private string m_objectIndex;
  private string m_Para;
  private string m_section;
  private string m_sequenceDecls;
  private int m_tableindex;
  private string m_TableOfContent;
  private string m_trackedChanges;
  private string m_userFileds;
  private string m_userIndex;
  private string m_variableDecls;
  private System.Collections.Generic.List<OTextBodyItem> m_bodyItems;

  internal System.Collections.Generic.List<OTextBodyItem> ChildItems
  {
    get
    {
      if (this.m_bodyItems == null)
        this.m_bodyItems = new System.Collections.Generic.List<OTextBodyItem>();
      return this.m_bodyItems;
    }
  }

  internal bool Display
  {
    get => this.m_display;
    set => this.m_display = value;
  }

  internal HeaderSection RegionCenter
  {
    get
    {
      if (this.m_regionCenter == null)
        this.m_regionCenter = new HeaderSection();
      return this.m_regionCenter;
    }
    set => this.m_regionCenter = value;
  }

  internal HeaderSection RegionLeft
  {
    get
    {
      if (this.m_regionLeft == null)
        this.m_regionLeft = new HeaderSection();
      return this.m_regionLeft;
    }
    set => this.m_regionLeft = value;
  }

  internal HeaderSection RegionRight
  {
    get
    {
      if (this.m_regionRight == null)
        this.m_regionRight = new HeaderSection();
      return this.m_regionRight;
    }
    set => this.m_regionRight = value;
  }

  internal OTable Table
  {
    get => this.m_table;
    set => this.m_table = value;
  }

  internal string AlphabeticalIndex
  {
    get => this.m_alphabeticalIndex;
    set => this.m_alphabeticalIndex = value;
  }

  internal string AlphabeticalIndexAutoMarkFile
  {
    get => this.m_alphabeticalIndexAutoMarkFile;
    set => this.m_alphabeticalIndexAutoMarkFile = value;
  }

  internal string Bibiliography
  {
    get => this.m_bibiliography;
    set => this.m_bibiliography = value;
  }

  internal string Change
  {
    get => this.m_change;
    set => this.m_change = value;
  }

  internal string ChangeEnd
  {
    get => this.m_changeEnd;
    set => this.m_changeEnd = value;
  }

  internal string ChangeStart
  {
    get => this.m_changeStart;
    set => this.m_changeStart = value;
  }

  internal string DDEConnectionDecls
  {
    get => this.m_DDEConnectionDecls;
    set => this.m_DDEConnectionDecls = value;
  }

  internal string Heading
  {
    get => this.m_heading;
    set => this.m_heading = value;
  }

  internal string IllustrationIndex
  {
    get => this.m_illustrationIndex;
    set => this.m_illustrationIndex = value;
  }

  internal string List
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  internal string ObjectIndex
  {
    get => this.m_objectIndex;
    set => this.m_objectIndex = value;
  }

  internal string Para
  {
    get => this.m_Para;
    set => this.m_Para = value;
  }

  internal string Section
  {
    get => this.m_section;
    set => this.m_section = value;
  }

  internal string SequenceDecls
  {
    get => this.m_sequenceDecls;
    set => this.m_sequenceDecls = value;
  }

  internal int Tableindex
  {
    get => this.m_tableindex;
    set => this.m_tableindex = value;
  }

  internal string TableOfContent
  {
    get => this.m_TableOfContent;
    set => this.m_TableOfContent = value;
  }

  internal string TrackedChanges
  {
    get => this.m_trackedChanges;
    set => this.m_trackedChanges = value;
  }

  internal string UserFileds
  {
    get => this.m_userFileds;
    set => this.m_userFileds = value;
  }

  internal string UserIndex
  {
    get => this.m_userIndex;
    set => this.m_userIndex = value;
  }

  internal string VariableDecls
  {
    get => this.m_variableDecls;
    set => this.m_variableDecls = value;
  }

  internal void Dispose()
  {
    if (this.m_regionCenter != null)
    {
      this.m_regionCenter.Dispose();
      this.m_regionCenter = (HeaderSection) null;
    }
    if (this.m_regionLeft != null)
    {
      this.m_regionLeft.Dispose();
      this.m_regionLeft = (HeaderSection) null;
    }
    if (this.m_regionRight != null)
    {
      this.m_regionRight.Dispose();
      this.m_regionRight = (HeaderSection) null;
    }
    if (this.m_table == null)
      return;
    this.m_table.Dispose();
    this.m_table = (OTable) null;
  }
}

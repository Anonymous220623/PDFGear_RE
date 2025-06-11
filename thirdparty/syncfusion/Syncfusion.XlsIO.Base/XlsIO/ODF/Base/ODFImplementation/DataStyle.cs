// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.DataStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class DataStyle : NumberFormat
{
  private string m_name;
  private string m_displayName;
  private bool m_volatile;
  private List<string> m_text;
  private List<MapStyle> m_map;
  private TextProperties m_textProperties;
  private bool m_hasSections;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string DisplayName
  {
    get => this.m_displayName;
    set => this.m_displayName = value;
  }

  internal bool Volatile
  {
    get => this.m_volatile;
    set => this.m_volatile = value;
  }

  internal List<string> Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal List<MapStyle> Map
  {
    get
    {
      if (this.m_map == null)
        this.m_map = new List<MapStyle>();
      return this.m_map;
    }
    set => this.m_map = value;
  }

  internal TextProperties TextProperties
  {
    get => this.m_textProperties;
    set => this.m_textProperties = value;
  }

  internal bool HasSections
  {
    get => this.m_hasSections;
    set => this.m_hasSections = value;
  }

  internal void Dispose1()
  {
    if (this.m_text != null)
    {
      this.m_text.Clear();
      this.m_text = (List<string>) null;
    }
    if (this.m_map != null)
    {
      this.m_map.Clear();
      this.m_map = (List<MapStyle>) null;
    }
    if (this.m_textProperties == null)
      return;
    this.m_textProperties = (TextProperties) null;
  }
}

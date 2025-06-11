// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.DefaultStyleCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class DefaultStyleCollection : CollectionBase<PageLayout>
{
  private Dictionary<string, DefaultStyle> m_defaultStyles;

  internal Dictionary<string, DefaultStyle> DefaultStyles
  {
    get
    {
      if (this.m_defaultStyles == null)
        this.m_defaultStyles = new Dictionary<string, DefaultStyle>();
      return this.m_defaultStyles;
    }
    set => this.m_defaultStyles = value;
  }

  internal string Add(DefaultStyle style)
  {
    string key = style.Name;
    if (string.IsNullOrEmpty(style.Name))
      key = CollectionBase<PageLayout>.GenerateDefaultName(this.MapName(style), (ICollection) this.DefaultStyles.Values);
    if (!this.m_defaultStyles.ContainsKey(key))
    {
      string str = this.ContainsValue(style);
      if (str != null)
      {
        key = str;
      }
      else
      {
        style.Name = key;
        this.DefaultStyles.Add(key, style);
      }
    }
    return key;
  }

  private string ContainsValue(DefaultStyle style)
  {
    string str = (string) null;
    foreach (DefaultStyle defaultStyle in this.DefaultStyles.Values)
    {
      if (defaultStyle.Equals((object) style))
      {
        str = defaultStyle.Name;
        break;
      }
    }
    return str;
  }

  private string MapName(DefaultStyle style)
  {
    string str = "ce";
    switch (style.Family)
    {
      case ODFFontFamily.Paragraph:
        return "P";
      case ODFFontFamily.Text:
        return "T";
      case ODFFontFamily.Table:
        return "ta";
      case ODFFontFamily.Table_Column:
        return "co";
      case ODFFontFamily.Table_Row:
        return "ro";
      default:
        return str;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.ODFStyleCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class ODFStyleCollection : CollectionBase<ODFStyle>
{
  private Dictionary<string, ODFStyle> m_dictStyles;

  internal Dictionary<string, ODFStyle> DictStyles
  {
    get
    {
      if (this.m_dictStyles == null)
        this.m_dictStyles = new Dictionary<string, ODFStyle>();
      return this.m_dictStyles;
    }
    set => this.m_dictStyles = value;
  }

  internal string Add(ODFStyle style)
  {
    string key = style.Name;
    if (string.IsNullOrEmpty(style.Name))
      key = CollectionBase<ODFStyle>.GenerateDefaultName(this.MapName(style), (ICollection) this.DictStyles.Values);
    if (!this.DictStyles.ContainsKey(key))
    {
      string str = this.ContainsValue(style);
      if (str != null)
      {
        key = str;
      }
      else
      {
        style.Name = key;
        this.DictStyles.Add(key, style);
      }
    }
    return key;
  }

  internal string Add(ODFStyle style, int index)
  {
    string key = style.Name;
    if (string.IsNullOrEmpty(style.Name))
      key = CollectionBase<ODFStyle>.GenerateDefaultName(this.MapName(style), (ICollection) this.DictStyles.Values);
    if (!this.DictStyles.ContainsKey(key))
    {
      string str = this.ContainsValue(style);
      if (str != null)
      {
        key = str;
      }
      else
      {
        style.Name = key;
        this.DictStyles.Add(index.ToString(), style);
      }
    }
    return key;
  }

  private string ContainsValue(ODFStyle style)
  {
    string str = (string) null;
    foreach (ODFStyle odfStyle in this.DictStyles.Values)
    {
      if (odfStyle.Family == style.Family && odfStyle.Equals((object) style))
      {
        str = odfStyle.Name;
        break;
      }
    }
    return str;
  }

  private string MapName(ODFStyle style)
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
      case ODFFontFamily.Section:
        return "S";
      case ODFFontFamily.Graphic:
        return "A";
      default:
        return str;
    }
  }

  internal void Dispose()
  {
    if (this.m_dictStyles == null)
      return;
    foreach (DefaultStyle defaultStyle in this.m_dictStyles.Values)
      defaultStyle.Dispose();
    this.m_dictStyles.Clear();
    this.m_dictStyles = (Dictionary<string, ODFStyle>) null;
  }
}

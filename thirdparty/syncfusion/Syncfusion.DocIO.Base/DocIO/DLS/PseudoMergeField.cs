// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PseudoMergeField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class PseudoMergeField
{
  private bool m_fitMailMerge;
  private string m_name;
  private string m_value;

  internal string Name => this.m_name;

  internal string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal bool FitMailMerge => this.m_fitMailMerge;

  internal PseudoMergeField(string fieldText)
  {
    if (fieldText == null)
      return;
    if (fieldText.IndexOf("MERGEFIELD") == -1)
    {
      char[] chArray = new char[1]{ '"' };
      string[] strArray = fieldText.Split(chArray);
      if (strArray.Length == 1)
        this.m_value = fieldText.Trim();
      else if (strArray.Length == 3)
        this.m_value = strArray[1];
      else
        this.m_value = string.Empty;
    }
    else
    {
      Match match = new Regex("MERGEFIELD\\s+\"?([^\"]+)\"").Match(fieldText);
      if (match.Groups.Count <= 1)
        return;
      this.m_name = match.Groups[1].Value;
      this.m_fitMailMerge = true;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSPathDataReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XPS;

internal class OXPSPathDataReader
{
  private const string ST_Double = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)";
  private const string ST_Point = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)";
  private const string ST_EventArrayPos = "(\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-859 9]+)?) (\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( (\\+?(([0-9]+(\\.[0-860 9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?) (\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-861 |\\+)?[0-9]+)?))*";
  private const string ST_Points = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)(((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?))*";
  private const string ST_PointsM3 = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)){2}(( ((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)){3})*";
  private char[] m_commaSeparator = new char[1]{ ',' };
  private char[] m_seperator = new char[1]{ ' ' };
  private string m_text;
  private int m_position;
  private char[] m_symbols = new char[20]
  {
    'F',
    'f',
    'm',
    'M',
    'l',
    'L',
    'h',
    'H',
    'v',
    'V',
    'c',
    'C',
    'q',
    'Q',
    's',
    'S',
    'a',
    'A',
    'z',
    'Z'
  };
  private char m_currentSymbol;

  internal bool EOF
  {
    get
    {
      return this.m_position == this.m_text.Length || this.m_text.IndexOfAny(this.m_symbols, this.m_position) == -1;
    }
  }

  internal int Length => this.m_text.Length;

  internal int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal OXPSPathDataReader(string text)
  {
    this.m_text = text;
    this.m_position = 0;
  }

  internal char ReadSymbol()
  {
    int index = this.m_text.IndexOfAny(this.m_symbols, this.m_position);
    if (index == -1)
      return char.MinValue;
    this.m_currentSymbol = this.m_text[index];
    this.m_position = index + 1;
    return this.m_currentSymbol;
  }

  internal char GetNextSymbol()
  {
    int index = this.m_text.IndexOfAny(this.m_symbols, this.m_position);
    return index == -1 ? char.MinValue : this.m_text[index];
  }

  internal void UpdateCurrentPosition(int length) => this.m_position += length;

  internal bool TryReadFloat(out float value)
  {
    value = 0.0f;
    Match match = Regex.Match(this.m_text.Substring(this.m_position), "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)");
    if (!match.Success)
      return false;
    this.UpdateCurrentPosition(match.Index + match.Captures[0].Value.Length);
    value = OXPSRenderer.ParseFloat(match.Captures[0].Value);
    return true;
  }

  internal bool TryReadPoint(out PointF val)
  {
    val = PointF.Empty;
    Match match = Regex.Match(this.m_text.Substring(this.m_position), "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)(((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?))*");
    if (!match.Success || char.IsLetter(this.m_text[this.m_position]) && this.m_text[this.m_position] != 'L' || !((object) this.m_text[this.m_position + 1] is ' ') && char.IsLetter(this.m_text, this.m_position + 1) && this.m_text[this.m_position + 1] != 'L')
      return false;
    string str = match.Captures[0].Value;
    this.UpdateCurrentPosition(match.Index + str.Length);
    string[] strArray = str.Split(this.m_commaSeparator);
    val = new PointF(OXPSRenderer.ParseFloat(strArray[0]), OXPSRenderer.ParseFloat(strArray[1]));
    return true;
  }

  internal bool TryReadPointM3(out PointF[] val)
  {
    val = (PointF[]) null;
    for (int position = this.m_position; position < this.m_text.Length; ++position)
    {
      switch (this.m_text[position])
      {
        case ' ':
          continue;
        case 'L':
        case 'l':
          return false;
        default:
          goto label_5;
      }
    }
label_5:
    Match match = Regex.Match(this.m_text.Substring(this.m_position), "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)){2}(( ((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)){3})*");
    List<PointF> pointFList = new List<PointF>();
    if (!match.Success || char.IsLetter(this.m_text[this.m_position]) && this.m_text[this.m_position] != 'C' || !((object) this.m_text[this.m_position + 1] is ' ') && char.IsLetter(this.m_text, this.m_position + 1))
      return false;
    string str = match.Captures[0].Value;
    this.UpdateCurrentPosition(match.Index + str.Length);
    string[] strArray = str.Replace(", ", ",").Split(this.m_seperator);
    for (int index = 0; index < strArray.Length; ++index)
      pointFList.Add(new PointF(OXPSRenderer.ParseFloat(strArray[index].Split(this.m_commaSeparator)[0]), OXPSRenderer.ParseFloat(strArray[index].Split(this.m_commaSeparator)[1])));
    val = pointFList.ToArray();
    return true;
  }

  internal bool TryReadPositionArray(out string[] val)
  {
    val = (string[]) null;
    Match match = Regex.Match(this.m_text.Substring(this.m_position), "(\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-859 9]+)?) (\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( (\\+?(([0-9]+(\\.[0-860 9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?) (\\+?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-861 |\\+)?[0-9]+)?))*");
    if (!match.Success)
      return false;
    val = match.Captures[0].Value.Split(this.m_seperator);
    return true;
  }

  internal PointF[] ReadPoints()
  {
    List<PointF> pointFList = new List<PointF>();
    PointF val;
    while (!this.CheckIfCurrentCharIsSymbol() && this.TryReadPoint(out val))
      pointFList.Add(val);
    return pointFList.ToArray();
  }

  private bool CheckIfCurrentCharIsSymbol()
  {
    return this.m_text.IndexOfAny(this.m_symbols, this.m_position) == this.m_position + 1;
  }
}

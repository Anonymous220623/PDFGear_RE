// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.XPSDataReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XPS;

internal class XPSDataReader
{
  private const string ST_Name = "(\\p{Lu}|\\p{Ll}|\\p{Lt}|\\p{Lo}|\\p{Nl}|_)(\\p{Lu}|\\p{Ll}|\\p{Lt}|\\p{Lo}|\\p{Nl}|\\p{Mn}|\\p{Mc}|\\p{Nd}|_)*";
  private const string ST_Boolean = "true|false";
  private const string ST_Double = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)";
  private const string ST_Point = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)";
  private const string ST_Matrix = "((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)";
  private char[] m_commaSeparator = new char[1]{ ',' };

  public string ReadName(string data, ref int position)
  {
    string str = "";
    Match match = new Regex("(\\p{Lu}|\\p{Ll}|\\p{Lt}|\\p{Lo}|\\p{Nl}|_)(\\p{Lu}|\\p{Ll}|\\p{Lt}|\\p{Lo}|\\p{Nl}|\\p{Mn}|\\p{Mc}|\\p{Nd}|_)*").Match(data, position);
    ++position;
    return !match.Success ? str : match.Captures[0].Value;
  }

  public bool ReadBoolean(string data, ref int position)
  {
    bool result = false;
    Match match = new Regex("true|false").Match(data, position);
    ++position;
    return match.Success ? bool.TryParse(match.Captures[0].Value, out result) : result;
  }

  public float ReadDouble(string data, ref int position)
  {
    float result = 0.0f;
    Match match = new Regex("((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)").Match(data, position);
    ++position;
    if (!match.Success)
      return result;
    float.TryParse(match.Captures[0].Value, out result);
    return result;
  }

  public PointF ReadPoint(string data, ref int position)
  {
    PointF pointF = PointF.Empty;
    Match match = new Regex("((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)").Match(data, position);
    ++position;
    if (!match.Success)
      return pointF;
    string[] strArray = match.Captures[0].Value.Split(this.m_commaSeparator);
    pointF = new PointF(float.Parse(strArray[0]), float.Parse(strArray[1]));
    return pointF;
  }

  public Matrix ReadMatrix(string data, ref int position)
  {
    new Regex("((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)( ?, ?)((\\-|\\+)?(([0-9]+(\\.[0-9]+)?)|(\\.[0-9]+))((e|E)(\\-|\\+)?[0-9]+)?)").Match(data, position);
    ++position;
    return (Matrix) null;
  }
}

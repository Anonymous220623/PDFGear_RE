// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ControlChar
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO;

public class ControlChar
{
  internal const char CellChar = '\a';
  internal const char ColumnBreakChar = '\u000E';
  internal const char FieldEndChar = '\u0015';
  internal const char FieldSeparatorChar = '\u0014';
  internal const char FieldStartChar = '\u0013';
  internal const char PageBreakChar = '\f';
  internal const char ParagraphBreakChar = '\r';
  internal const char SectionBreakChar = '\f';
  public static readonly string CarriegeReturn = '\r'.ToString();
  public static readonly string CrLf = "\r\n";
  public static readonly string DefaultTextInput = ' '.ToString();
  public static readonly char DefaultTextInputChar = ' ';
  public static readonly string LineBreak = "\v";
  public static readonly char LineBreakChar = '\v';
  public static readonly string LineFeed = '\n'.ToString();
  public static readonly char LineFeedChar = '\n';
  public static readonly string NonBreakingSpace = ' '.ToString();
  public static readonly char NonBreakingSpaceChar = ' ';
  public static readonly string Tab = "\t";
  public static readonly char TabChar = '\t';
  public static readonly string Hyphen = '\u001F'.ToString();
  public static readonly char HyphenChar = '\u001F';
  public static readonly string Space = ' '.ToString();
  public static readonly char SpaceChar = ' ';
  public static readonly char DoubleQuote = '"';
  public static readonly char LeftDoubleQuote = '“';
  public static readonly char RightDoubleQuote = '”';
  public static readonly char DoubleLowQuote = '„';
  internal static readonly string DoubleQuoteString = '"'.ToString();
  internal static readonly string LeftDoubleQuoteString = '“'.ToString();
  internal static readonly string RightDoubleQuoteString = '”'.ToString();
  internal static readonly string DoubleLowQuoteString = '„'.ToString();
  public static readonly string NonBreakingHyphen = '\u001E'.ToString();
  public static readonly char NonBreakingHyphenChar = '\u001E';
  internal static readonly string Cell = '\a'.ToString();
  internal static readonly string ColumnBreak = '\u000E'.ToString();
  internal static readonly string PageBreak = '\f'.ToString();
  internal static readonly string ParagraphBreak = '\r'.ToString();
  internal static readonly string SectionBreak = '\f'.ToString();
}

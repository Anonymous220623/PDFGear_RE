// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HtmlStringParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class HtmlStringParser
{
  private Stack<TextFormat> styleStack = new Stack<TextFormat>();
  private static TextFormat m_formatText;
  private bool isPreserveBreakForInvalidStyles;

  protected TextFormat CurrentFormat
  {
    get
    {
      if (this.styleStack.Count > 0)
        return this.styleStack.Peek();
      if (HtmlStringParser.m_formatText == null)
        HtmlStringParser.m_formatText = new TextFormat();
      return HtmlStringParser.m_formatText;
    }
  }

  private string LoadXHtml(string html)
  {
    html = this.PrepareHtml(html);
    html = this.InsertHtmlElement(html);
    html = this.RemoveunWantedQuotes(html);
    return html;
  }

  internal string RemoveunWantedQuotes(string html)
  {
    Regex regex1 = new Regex("(?<=font-family:)(.*?)(?=;)");
    html = this.ReplaceUnwantedQuotes(html, regex1);
    Regex regex2 = new Regex("(?<=color:)(.*?)(?=;)");
    html = this.ReplaceUnwantedQuotes(html, regex2);
    Regex regex3 = new Regex("(?<=font-size:)(.*?)(?=;)");
    html = this.ReplaceUnwantedQuotes(html, regex3);
    return html;
  }

  private string ReplaceUnwantedQuotes(string html, Regex regex)
  {
    foreach (Match match in regex.Matches(html))
    {
      string[] strArray = match.Value.Split(',');
      string empty = string.Empty;
      foreach (string str in strArray)
        empty += str.Replace("\"", ",");
      string newValue = empty.Trim(',');
      html = html.Replace(match.Value, newValue);
    }
    return html;
  }

  internal string PrepareHtml(string html)
  {
    html = this.ReplaceHtmlConstantByUnicodeChar(html);
    return html;
  }

  private string ReplaceHtmlConstantByUnicodeChar(string html)
  {
    html = this.ReplaceHtmlSpecialCharacters(html);
    html = this.ReplaceHtmlSymbols(html);
    html = this.ReplaceHtmlCharacters(html);
    html = this.ReplaceHtmlMathSymbols(html);
    html = this.ReplaceHtmlGreekLetters(html);
    html = this.ReplaceHtmlOtherEntities(html);
    html = this.ReplaceHtmlTags(html);
    return html;
  }

  private string ReplaceHtmlTags(string html)
  {
    MatchCollection matchCollection = new Regex("(\\<[\\t \\n]*\\/{0,1}[ \\t]*){1}[a-zA-Z0-9]*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant).Matches(html);
    Regex regex = new Regex("\\t|\\n| ");
    StringBuilder stringBuilder = new StringBuilder(html);
    int num = 0;
    foreach (Match match in matchCollection)
    {
      stringBuilder.Remove(match.Index - num, match.Length);
      if (regex.Match(match.Value).Success)
      {
        string str = Regex.Replace(match.Value, "\\t|\\n| ", "", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
        stringBuilder.Insert(match.Index - num, str.ToLower());
        num += match.Value.Length - str.Length;
      }
      else
        stringBuilder.Insert(match.Index - num, match.Value.ToLower());
    }
    return stringBuilder.ToString();
  }

  private string ReplaceHtmlBreakTags(string html)
  {
    html = Regex.Replace(html, "(<br />|< br />|</ br >|</br >|</br>|<br>|<br >|< br>)", "<br/>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
    return html;
  }

  private string ReplaceHtmlSpecialCharacters(string html)
  {
    html = html.Replace("&", string.Concat((object) '&'));
    html = html.Replace("&quot;", string.Concat((object) '"'));
    html = html.Replace("&apos;", string.Concat((object) '\''));
    html = html.Replace("&gt;", string.Concat((object) '>'));
    html = html.Replace("Â", string.Concat((object) 'Â'));
    html = html.Replace("«", string.Concat((object) '«'));
    html = html.Replace("»", string.Concat((object) '»'));
    return html;
  }

  internal string ReplaceHtmlSymbols(string html)
  {
    html = html.Replace("&nbsp;", string.Concat((object) ' '));
    html = html.Replace("&iexcl;", string.Concat((object) '¡'));
    html = html.Replace("&cent;", string.Concat((object) '¢'));
    html = html.Replace("&pound;", string.Concat((object) '£'));
    html = html.Replace("&curren;", string.Concat((object) '¤'));
    html = html.Replace("&yen;", string.Concat((object) '¥'));
    html = html.Replace("&brvbar;", string.Concat((object) '¦'));
    html = html.Replace("&sect;", string.Concat((object) '§'));
    html = html.Replace("&uml;", string.Concat((object) '¨'));
    html = html.Replace("&copy;", string.Concat((object) '©'));
    html = html.Replace("&ordf;", string.Concat((object) 'ª'));
    html = html.Replace("&laquo;", string.Concat((object) '«'));
    html = html.Replace("&not;", string.Concat((object) '¬'));
    html = html.Replace("&shy;", string.Concat((object) '\u00AD'));
    html = html.Replace("&reg;", string.Concat((object) '®'));
    html = html.Replace("&macr;", string.Concat((object) '¯'));
    html = html.Replace("&deg;", string.Concat((object) '°'));
    html = html.Replace("&plusmn;", string.Concat((object) '±'));
    html = html.Replace("&sup2;", string.Concat((object) '\u00B2'));
    html = html.Replace("&sup3;", string.Concat((object) '\u00B3'));
    html = html.Replace("&acute;", string.Concat((object) '´'));
    html = html.Replace("&micro;", string.Concat((object) 'µ'));
    html = html.Replace("&para;", string.Concat((object) '¶'));
    html = html.Replace("&middot;", string.Concat((object) '·'));
    html = html.Replace("&cedil;", string.Concat((object) '¸'));
    html = html.Replace("&sup1;", string.Concat((object) '\u00B9'));
    html = html.Replace("&ordm;", string.Concat((object) 'º'));
    html = html.Replace("&raquo;", string.Concat((object) '»'));
    html = html.Replace("&frac14;", string.Concat((object) '\u00BC'));
    html = html.Replace("&frac12;", string.Concat((object) '\u00BD'));
    html = html.Replace("&frac34;", string.Concat((object) '\u00BE'));
    html = html.Replace("&iquest;", string.Concat((object) '¿'));
    html = html.Replace("&times;", string.Concat((object) '×'));
    html = html.Replace("&divide;", string.Concat((object) '÷'));
    return html;
  }

  private string ReplaceHtmlCharacters(string html)
  {
    html = html.Replace("&Agrave;", string.Concat((object) 'À'));
    html = html.Replace("&Aacute;", string.Concat((object) 'Á'));
    html = html.Replace("&Acirc;", string.Concat((object) 'Â'));
    html = html.Replace("&Atilde;", string.Concat((object) 'Ã'));
    html = html.Replace("&Auml;", string.Concat((object) 'Ä'));
    html = html.Replace("&Aring;", string.Concat((object) 'Å'));
    html = html.Replace("&AElig;", string.Concat((object) 'Æ'));
    html = html.Replace("&Ccedil;", string.Concat((object) 'Ç'));
    html = html.Replace("&Egrave;", string.Concat((object) 'È'));
    html = html.Replace("&Eacute;", string.Concat((object) 'É'));
    html = html.Replace("&Ecirc;", string.Concat((object) 'Ê'));
    html = html.Replace("&Euml;", string.Concat((object) 'Ë'));
    html = html.Replace("&Igrave;", string.Concat((object) 'Ì'));
    html = html.Replace("&Iacute;", string.Concat((object) 'Í'));
    html = html.Replace("&Icirc;", string.Concat((object) 'Î'));
    html = html.Replace("&Iuml;", string.Concat((object) 'Ï'));
    html = html.Replace("&ETH;", string.Concat((object) 'Ð'));
    html = html.Replace("&Ntilde;", string.Concat((object) 'Ñ'));
    html = html.Replace("&Ograve;", string.Concat((object) 'Ò'));
    html = html.Replace("&Oacute;", string.Concat((object) 'Ó'));
    html = html.Replace("&Ocirc;", string.Concat((object) 'Ô'));
    html = html.Replace("&Otilde;", string.Concat((object) 'Õ'));
    html = html.Replace("&Ouml;", string.Concat((object) 'Ö'));
    html = html.Replace("&Oslash;", string.Concat((object) 'Ø'));
    html = html.Replace("&Ugrave;", string.Concat((object) 'Ù'));
    html = html.Replace("&Uacute;", string.Concat((object) 'Ú'));
    html = html.Replace("&Ucirc;", string.Concat((object) 'Û'));
    html = html.Replace("&Uuml;", string.Concat((object) 'Ü'));
    html = html.Replace("&Yacute;", string.Concat((object) 'Ý'));
    html = html.Replace("&THORN;", string.Concat((object) 'Þ'));
    html = html.Replace("&szlig;", string.Concat((object) 'ß'));
    html = html.Replace("&agrave;", string.Concat((object) 'à'));
    html = html.Replace("&aacute;", string.Concat((object) 'á'));
    html = html.Replace("&acirc;", string.Concat((object) 'â'));
    html = html.Replace("&atilde;", string.Concat((object) 'ã'));
    html = html.Replace("&auml;", string.Concat((object) 'ä'));
    html = html.Replace("&aring;", string.Concat((object) 'å'));
    html = html.Replace("&aelig;", string.Concat((object) 'æ'));
    html = html.Replace("&ccedil;", string.Concat((object) 'ç'));
    html = html.Replace("&egrave;", string.Concat((object) 'è'));
    html = html.Replace("&eacute;", string.Concat((object) 'é'));
    html = html.Replace("&ecirc;", string.Concat((object) 'ê'));
    html = html.Replace("&euml;", string.Concat((object) 'ë'));
    html = html.Replace("&igrave;", string.Concat((object) 'ì'));
    html = html.Replace("&iacute;", string.Concat((object) 'í'));
    html = html.Replace("&icirc;", string.Concat((object) 'î'));
    html = html.Replace("&iuml;", string.Concat((object) 'ï'));
    html = html.Replace("&eth;", string.Concat((object) 'ð'));
    html = html.Replace("&ntilde;", string.Concat((object) 'ñ'));
    html = html.Replace("&ograve;", string.Concat((object) 'ò'));
    html = html.Replace("&oacute;", string.Concat((object) 'ó'));
    html = html.Replace("&ocirc;", string.Concat((object) 'ô'));
    html = html.Replace("&otilde;", string.Concat((object) 'õ'));
    html = html.Replace("&ouml;", string.Concat((object) 'ö'));
    html = html.Replace("&oslash;", string.Concat((object) 'ø'));
    html = html.Replace("&ugrave;", string.Concat((object) 'ù'));
    html = html.Replace("&uacute;", string.Concat((object) 'ú'));
    html = html.Replace("&ucirc;", string.Concat((object) 'û'));
    html = html.Replace("&uuml;", string.Concat((object) 'ü'));
    html = html.Replace("&yacute;", string.Concat((object) 'ý'));
    html = html.Replace("&thorn;", string.Concat((object) 'þ'));
    html = html.Replace("&yuml;", string.Concat((object) 'ÿ'));
    return html;
  }

  private string ReplaceHtmlMathSymbols(string html)
  {
    html = html.Replace("&forall;", string.Concat((object) '∀'));
    html = html.Replace("&part;", string.Concat((object) '∂'));
    html = html.Replace("&exist;", string.Concat((object) '∃'));
    html = html.Replace("&empty;", string.Concat((object) '∅'));
    html = html.Replace("&nabla;", string.Concat((object) '∇'));
    html = html.Replace("&isin;", string.Concat((object) '∈'));
    html = html.Replace("&notin;", string.Concat((object) '∉'));
    html = html.Replace("&ni;", string.Concat((object) '∋'));
    html = html.Replace("&prod;", string.Concat((object) '∏'));
    html = html.Replace("&sum;", string.Concat((object) '∑'));
    html = html.Replace("&minus;", string.Concat((object) '−'));
    html = html.Replace("&lowast;", string.Concat((object) '∗'));
    html = html.Replace("&radic;", string.Concat((object) '√'));
    html = html.Replace("&prop;", string.Concat((object) '∝'));
    html = html.Replace("&infin;", string.Concat((object) '∞'));
    html = html.Replace("&ang;", string.Concat((object) '∠'));
    html = html.Replace("&and;", string.Concat((object) '∧'));
    html = html.Replace("&or;", string.Concat((object) '∨'));
    html = html.Replace("&cap;", string.Concat((object) '∩'));
    html = html.Replace("&cup;", string.Concat((object) '∪'));
    html = html.Replace("&int;", string.Concat((object) '∫'));
    html = html.Replace("&there4;", string.Concat((object) '∴'));
    html = html.Replace("&sim;", string.Concat((object) '∼'));
    html = html.Replace("&cong;", string.Concat((object) '≅'));
    html = html.Replace("&asymp;", string.Concat((object) '≈'));
    html = html.Replace("&ne;", string.Concat((object) '≠'));
    html = html.Replace("&equiv;", string.Concat((object) '≡'));
    html = html.Replace("&le;", string.Concat((object) '≤'));
    html = html.Replace("&ge;", string.Concat((object) '≥'));
    html = html.Replace("&sub;", string.Concat((object) '⊂'));
    html = html.Replace("&sup;", string.Concat((object) '⊃'));
    html = html.Replace("&nsub;", string.Concat((object) '⊄'));
    html = html.Replace("&sube;", string.Concat((object) '⊆'));
    html = html.Replace("&supe;", string.Concat((object) '⊇'));
    html = html.Replace("&oplus;", string.Concat((object) '⊕'));
    html = html.Replace("&otimes;", string.Concat((object) '⊗'));
    html = html.Replace("&perp;", string.Concat((object) '⊥'));
    html = html.Replace("&sdot;", string.Concat((object) '⋅'));
    html = html.Replace("&frasl;", string.Concat((object) '⁄'));
    return html;
  }

  private string ReplaceHtmlGreekLetters(string html)
  {
    html = html.Replace("&Alpha;", string.Concat((object) 'Α'));
    html = html.Replace("&Beta;", string.Concat((object) 'Β'));
    html = html.Replace("&Gamma;", string.Concat((object) 'Γ'));
    html = html.Replace("&Delta;", string.Concat((object) 'Δ'));
    html = html.Replace("&Epsilon;", string.Concat((object) 'Ε'));
    html = html.Replace("&Zeta;", string.Concat((object) 'Ζ'));
    html = html.Replace("&Eta;", string.Concat((object) 'Η'));
    html = html.Replace("&Theta;", string.Concat((object) 'Θ'));
    html = html.Replace("&Iota;", string.Concat((object) 'Ι'));
    html = html.Replace("&Kappa;", string.Concat((object) 'Κ'));
    html = html.Replace("&Lambda;", string.Concat((object) 'Λ'));
    html = html.Replace("&Mu;", string.Concat((object) 'Μ'));
    html = html.Replace("&Nu;", string.Concat((object) 'Ν'));
    html = html.Replace("&Xi;", string.Concat((object) 'Ξ'));
    html = html.Replace("&Omicron;", string.Concat((object) 'Ο'));
    html = html.Replace("&Pi;", string.Concat((object) 'Π'));
    html = html.Replace("&Rho;", string.Concat((object) 'Ρ'));
    html = html.Replace("&Sigma;", string.Concat((object) 'Σ'));
    html = html.Replace("&Tau;", string.Concat((object) 'Τ'));
    html = html.Replace("&Upsilon;", string.Concat((object) 'Υ'));
    html = html.Replace("&Phi;", string.Concat((object) 'Φ'));
    html = html.Replace("&Chi;", string.Concat((object) 'Χ'));
    html = html.Replace("&Psi;", string.Concat((object) 'Ψ'));
    html = html.Replace("&Omega;", string.Concat((object) 'Ω'));
    html = html.Replace("&alpha;", string.Concat((object) 'α'));
    html = html.Replace("&beta;", string.Concat((object) 'β'));
    html = html.Replace("&gamma;", string.Concat((object) 'γ'));
    html = html.Replace("&delta;", string.Concat((object) 'δ'));
    html = html.Replace("&epsilon;", string.Concat((object) 'ε'));
    html = html.Replace("&zeta;", string.Concat((object) 'ζ'));
    html = html.Replace("&eta;", string.Concat((object) 'η'));
    html = html.Replace("&theta;", string.Concat((object) 'θ'));
    html = html.Replace("&iota;", string.Concat((object) 'ι'));
    html = html.Replace("&kappa;", string.Concat((object) 'κ'));
    html = html.Replace("&lambda;", string.Concat((object) 'λ'));
    html = html.Replace("&mu;", string.Concat((object) 'μ'));
    html = html.Replace("&nu;", string.Concat((object) 'ν'));
    html = html.Replace("&xi;", string.Concat((object) 'ξ'));
    html = html.Replace("&omicron;", string.Concat((object) 'ο'));
    html = html.Replace("&pi;", string.Concat((object) 'π'));
    html = html.Replace("&rho;", string.Concat((object) 'ρ'));
    html = html.Replace("&sigmaf;", string.Concat((object) 'ς'));
    html = html.Replace("&sigma;", string.Concat((object) 'σ'));
    html = html.Replace("&tau;", string.Concat((object) 'τ'));
    html = html.Replace("&upsilon;", string.Concat((object) 'υ'));
    html = html.Replace("&phi;", string.Concat((object) 'φ'));
    html = html.Replace("&chi;", string.Concat((object) 'χ'));
    html = html.Replace("&psi;", string.Concat((object) 'ψ'));
    html = html.Replace("&omega;", string.Concat((object) 'ω'));
    html = html.Replace("&thetasym;", string.Concat((object) 'ϑ'));
    html = html.Replace("&upsih;", string.Concat((object) 'ϒ'));
    html = html.Replace("&piv;", string.Concat((object) 'ϖ'));
    return html;
  }

  private string ReplaceHtmlOtherEntities(string html)
  {
    html = html.Replace("&OElig;", string.Concat((object) 'Œ'));
    html = html.Replace("&oelig;", string.Concat((object) 'œ'));
    html = html.Replace("&Scaron;", string.Concat((object) 'Š'));
    html = html.Replace("&scaron;", string.Concat((object) 'š'));
    html = html.Replace("&Yuml;", string.Concat((object) 'Ÿ'));
    html = html.Replace("&fnof;", string.Concat((object) 'ƒ'));
    html = html.Replace("&circ;", string.Concat((object) 'ˆ'));
    html = html.Replace("&tilde;", string.Concat((object) '˜'));
    html = html.Replace("&ensp;", string.Concat((object) ' '));
    html = html.Replace("&emsp;", string.Concat((object) ' '));
    html = html.Replace("&thinsp;", string.Concat((object) ' '));
    html = html.Replace("&zwnj;", string.Concat((object) '\u200C'));
    html = html.Replace("&zwj;", string.Concat((object) '\u200D'));
    html = html.Replace("&lrm;", string.Concat((object) '\u200E'));
    html = html.Replace("&rlm;", string.Concat((object) '\u200F'));
    html = html.Replace("&ndash;", string.Concat((object) '–'));
    html = html.Replace("&mdash;", string.Concat((object) '—'));
    html = html.Replace("&lsquo;", string.Concat((object) '‘'));
    html = html.Replace("&rsquo;", string.Concat((object) '’'));
    html = html.Replace("&sbquo;", string.Concat((object) '‚'));
    html = html.Replace("&ldquo;", string.Concat((object) '“'));
    html = html.Replace("&rdquo;", string.Concat((object) '”'));
    html = html.Replace("&bdquo;", string.Concat((object) '„'));
    html = html.Replace("&dagger;", string.Concat((object) '†'));
    html = html.Replace("&Dagger;", string.Concat((object) '‡'));
    html = html.Replace("&bull;", string.Concat((object) '•'));
    html = html.Replace("&hellip;", string.Concat((object) '…'));
    html = html.Replace("&permil;", string.Concat((object) '‰'));
    html = html.Replace("&prime;", string.Concat((object) '′'));
    html = html.Replace("&Prime;", string.Concat((object) '″'));
    html = html.Replace("&lsaquo;", string.Concat((object) '‹'));
    html = html.Replace("&rsaquo;", string.Concat((object) '›'));
    html = html.Replace("&oline;", string.Concat((object) '‾'));
    html = html.Replace("&euro;", string.Concat((object) '€'));
    html = html.Replace("&trade;", string.Concat((object) '™'));
    html = html.Replace("&larr;", string.Concat((object) '←'));
    html = html.Replace("&uarr;", string.Concat((object) '↑'));
    html = html.Replace("&rarr;", string.Concat((object) '→'));
    html = html.Replace("&darr;", string.Concat((object) '↓'));
    html = html.Replace("&harr;", string.Concat((object) '↔'));
    html = html.Replace("&crarr;", string.Concat((object) '↵'));
    html = html.Replace("&lArr;", string.Concat((object) '⇐'));
    html = html.Replace("&uArr;", string.Concat((object) '⇑'));
    html = html.Replace("&rArr;", string.Concat((object) '⇒'));
    html = html.Replace("&dArr;", string.Concat((object) '⇓'));
    html = html.Replace("&hArr;", string.Concat((object) '⇔'));
    html = html.Replace("&lceil;", string.Concat((object) '⌈'));
    html = html.Replace("&rceil;", string.Concat((object) '⌉'));
    html = html.Replace("&lfloor;", string.Concat((object) '⌊'));
    html = html.Replace("&rfloor;", string.Concat((object) '⌋'));
    html = html.Replace("&loz;", string.Concat((object) '◊'));
    html = html.Replace("&spades;", string.Concat((object) '♠'));
    html = html.Replace("&clubs;", string.Concat((object) '♣'));
    html = html.Replace("&hearts;", string.Concat((object) '♥'));
    html = html.Replace("&diams;", string.Concat((object) '♦'));
    html = html.Replace("&lang;", string.Concat((object) '〈'));
    html = html.Replace("&rang;", string.Concat((object) '〉'));
    return html;
  }

  internal string InsertHtmlElement(string html)
  {
    string str = "<html>";
    if (html.ToLower().StartsWith("<html>"))
    {
      int count = html.ToLower().IndexOf("<body");
      html = str + html.Remove(0, count);
    }
    else
      html = !html.ToLower().StartsWith("<head") ? (!html.ToLower().StartsWith("<body") ? $"{str}<body>{html}</body></html>" : $"{str}{html}</html>") : $"{str}{html}</html>";
    return html;
  }

  internal void ParseHtml(string html, IRichTextString range)
  {
    range.Clear();
    html = this.ReplaceHtmlBreakTags(html);
    html = this.LoadXHtml(html);
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(html);
    XmlNode xmlNode = (XmlNode) xmlDocument.DocumentElement;
    if (xmlNode.LocalName.ToLower() == nameof (html))
    {
      foreach (XmlNode childNode in xmlNode.ChildNodes)
      {
        if (childNode.LocalName.ToLower() == "body")
        {
          xmlNode = childNode;
          break;
        }
      }
    }
    this.TraverseChildNodes(xmlNode.ChildNodes, range);
  }

  internal void TraverseChildNodes(XmlNodeList nodes, IRichTextString range)
  {
    foreach (XmlNode node in nodes)
    {
      if (node.NodeType == XmlNodeType.Text)
        this.TraverseTextWithinTag(node, range);
      else if (node.NodeType == XmlNodeType.Element)
        this.ParseTags(node, range);
      else if (node.NodeType != XmlNodeType.Whitespace)
      {
        int nodeType = (int) node.NodeType;
      }
    }
  }

  internal void ParseTags(XmlNode node, IRichTextString range)
  {
    switch (node.Name.ToLower())
    {
      case "p":
      case "span":
        this.EnsureStyle(node);
        this.TraverseChildNodes(node.ChildNodes, range);
        break;
      case "br":
        this.TraverseTextWithinTag(node, range);
        break;
      case "h1":
      case "h2":
      case "h3":
      case "h4":
      case "h5":
      case "h6":
        if (this.IsEmptyNode(node))
          break;
        this.ParseHeadingTag(node, range);
        break;
      case "th":
      case "td":
        this.TraverseTextWithinTag(node, range);
        break;
      default:
        this.ParseFormattingTags(node, range);
        break;
    }
  }

  private void ParseFormattingTags(XmlNode tag, IRichTextString range)
  {
    TextFormat textFormat = this.EnsureStyle(tag);
    switch (tag.Name.ToLower())
    {
      case "b":
        textFormat.Bold = true;
        break;
      case "strong":
        textFormat.Bold = true;
        break;
      case "i":
        textFormat.Italic = true;
        break;
      case "u":
        textFormat.Underline = true;
        break;
      case "strike":
        textFormat.Strike = true;
        break;
      case "a":
        this.ParseHyperLinkTag(tag, range);
        break;
      case "sup":
        textFormat.SuperScript = true;
        break;
      case "sub":
        textFormat.SubScript = true;
        break;
      case "font":
        string attributeValue1 = this.GetAttributeValue(tag, "color");
        string attributeValue2 = this.GetAttributeValue(tag, "face");
        if (attributeValue1.Length > 0)
          textFormat.FontColor = this.GetColor(attributeValue1);
        if (attributeValue2.Length > 0)
          textFormat.FontFamily = attributeValue2;
        string attributeValue3 = this.GetAttributeValue(tag, "size");
        if (attributeValue3.Length > 0)
        {
          textFormat.FontSize = float.Parse(attributeValue3);
          break;
        }
        break;
      default:
        throw new NotSupportedException("XlsIO do not support html tag: " + tag.Name);
    }
    this.TraverseChildNodes(tag.ChildNodes, range);
    this.LeaveStyle(true);
  }

  private bool IsEmptyNode(XmlNode node)
  {
    bool flag = true;
    if (node.LocalName.ToLower() == "img" || node.LocalName.ToLower() == "br" || node.LocalName.ToLower() == "a")
      return false;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType != XmlNodeType.Whitespace && childNode.NodeType != XmlNodeType.SignificantWhitespace)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void ParseHeadingTag(XmlNode tag, IRichTextString range)
  {
    TextFormat textFormat = this.EnsureStyle(tag);
    switch (tag.Name.ToLower())
    {
      case "h1":
        textFormat.FontSize = 24f;
        textFormat.FontFamily = "Times New Roman";
        break;
      case "h2":
        textFormat.FontSize = 18f;
        textFormat.FontFamily = "Times New Roman";
        break;
      case "h3":
        textFormat.FontSize = 13f;
        textFormat.FontFamily = "Times New Roman";
        break;
      case "h4":
        textFormat.FontSize = 12f;
        textFormat.FontFamily = "Times New Roman";
        break;
      case "h5":
        textFormat.FontSize = 10f;
        textFormat.FontFamily = "Times New Roman";
        break;
      case "h6":
        textFormat.FontSize = 7f;
        textFormat.FontFamily = "Times New Roman";
        break;
      default:
        throw new NotSupportedException("XlsIO do not support html tag: " + tag.Name);
    }
    this.TraverseChildNodes(tag.ChildNodes, range);
    this.LeaveStyle(true);
  }

  internal Color GetColor(string attValue)
  {
    if (!attValue.StartsWith("rgb"))
      return ColorTranslator.FromHtml(attValue);
    string[] strArray = attValue.Replace("rgb", string.Empty).Trim('(', ')', ' ').Split(new char[1]
    {
      ','
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length != 3)
      return Color.Empty;
    int result1;
    int.TryParse(strArray[0], out result1);
    int result2;
    int.TryParse(strArray[1], out result2);
    int result3;
    int.TryParse(strArray[2], out result3);
    return Color.FromArgb(result1, result2, result3);
  }

  private string GetFontName(string paramValue)
  {
    string fontName = paramValue;
    if (paramValue.Trim().Contains(","))
    {
      int length = paramValue.Trim().IndexOf(',');
      fontName = paramValue.Trim().Substring(0, length);
    }
    return fontName;
  }

  internal string GetAttributeValue(XmlNode node, string attrName)
  {
    attrName = attrName.ToLower();
    for (int i = 0; i < node.Attributes.Count; ++i)
    {
      XmlAttribute attribute = node.Attributes[i];
      if (attribute.LocalName.ToLower() == attrName)
        return attribute.Value;
    }
    return string.Empty;
  }

  private void TraverseTextWithinTag(XmlNode node, IRichTextString rtfRange)
  {
    string empty = string.Empty;
    IFont font1;
    string innerText;
    if (rtfRange is RichTextString)
    {
      font1 = (rtfRange as RichTextString).Workbook.CreateFont();
      if (node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase))
      {
        innerText = '\n'.ToString();
        if (rtfRange is RangeRichTextString)
          (rtfRange as RangeRichTextString).Worksheet[RangeImpl.GetRowFromCellIndex((rtfRange as RichTextString).RtfCellIndex), RangeImpl.GetColumnFromCellIndex((rtfRange as RichTextString).RtfCellIndex)].WrapText = true;
      }
      else
        innerText = node.InnerText;
    }
    else
    {
      font1 = (rtfRange as RTFStringArray).RtfRange.Worksheet.Workbook.CreateFont();
      if (node.LocalName.Equals("br", StringComparison.OrdinalIgnoreCase))
      {
        innerText = '\n'.ToString();
        if (rtfRange is RTFStringArray)
          (rtfRange as RTFStringArray).RtfRange.WrapText = true;
      }
      else
        innerText = node.InnerText;
    }
    IFont font2 = this.ApplyTextFormatting(font1, this.CurrentFormat);
    rtfRange.Append(innerText, font2);
  }

  internal IFont ApplyTextFormatting(IFont font, TextFormat format)
  {
    if (format.HasValue(2))
      font.Bold = format.Bold;
    if (format.HasValue(4))
      font.Italic = format.Italic;
    if (format.HasValue(3) && format.Underline)
      font.Underline = ExcelUnderline.Single;
    if (format.HasValue(5))
      font.Strikethrough = format.Strike;
    if (format.HasValue(6) && format.FontColor != Color.Empty)
      font.RGBColor = format.FontColor;
    if (format.HasValue(7))
      font.Superscript = format.SuperScript;
    if (format.HasValue(8))
      font.Subscript = format.SubScript;
    if (format.HasValue(0))
      font.Size = (double) format.FontSize;
    if (format.HasValue(1) && format.FontFamily != string.Empty)
      font.FontName = format.FontFamily;
    return font;
  }

  private void ParseHyperLinkTag(XmlNode tag, IRichTextString range)
  {
    TextFormat textFormat = this.EnsureStyle(tag);
    string empty = string.Empty;
    if (tag.Attributes["href"] != null)
      empty = tag.Attributes["href"].Value;
    if (!(range is RichTextString))
      return;
    RichTextString richTextString = range as RichTextString;
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(richTextString.RtfCellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(richTextString.RtfCellIndex);
    RangeRichTextString rangeRichTextString = range as RangeRichTextString;
    IRange range1 = rangeRichTextString.Worksheet.Range[rowFromCellIndex, columnFromCellIndex];
    if (range1.Hyperlinks.Count >= 1)
      return;
    IHyperLink hyperLink = rangeRichTextString.Worksheet.HyperLinks.Add(range1);
    hyperLink.Type = ExcelHyperLinkType.Url;
    hyperLink.Address = empty;
    hyperLink.TextToDisplay = tag.InnerText;
    if (tag.Attributes["title"] != null)
      hyperLink.ScreenTip = tag.Attributes["title"].Value;
    textFormat.Underline = true;
    textFormat.FontColor = Color.Blue;
  }

  private TextFormat EnsureStyle(XmlNode node)
  {
    this.ParseStyle(node);
    return this.CurrentFormat;
  }

  private TextFormat AddStyle()
  {
    TextFormat textFormat = this.styleStack.Count > 0 ? this.styleStack.Peek().Clone() : new TextFormat();
    this.styleStack.Push(textFormat);
    return textFormat;
  }

  private bool ParseStyle(XmlNode node)
  {
    string attributeValue = this.GetAttributeValue(node, "style");
    if (attributeValue.Length < 0)
      return false;
    TextFormat format = this.AddStyle();
    string[] strArray = attributeValue.Split(';', ':');
    int index = 0;
    for (int length = strArray.Length; index < length - 1; index += 2)
    {
      char[] chArray = new char[2]{ '\'', '"' };
      string paramName = strArray[index].ToLower().Trim();
      string paramValue = strArray[index + 1].Trim().Trim(chArray);
      this.GetFormat(format, paramName, paramValue, node);
    }
    return true;
  }

  internal void GetFormat(TextFormat format, string paramName, string paramValue, XmlNode node)
  {
    if (!(paramName == "mso-field-code"))
      paramValue = paramValue.ToLower();
    if (paramValue.ToLower().Contains("inherit") && paramName.ToLower() != "page-break-before" && paramName.ToLower() != "page-break-after" && paramName != "page-break-inside")
      return;
    if (node.LocalName.ToLower() == "span")
    {
      if (this.isPreserveBreakForInvalidStyles)
        this.isPreserveBreakForInvalidStyles = false;
    }
    try
    {
      switch (paramName)
      {
        case "text-align":
          format.TextAlignment = paramValue;
          break;
        case "vertical-align":
          format.VerticalAlignment = paramValue;
          break;
        case "background":
        case "background-color":
          format.BackColor = this.GetColor(paramValue);
          break;
        case "color":
          format.FontColor = this.GetColor(paramValue);
          break;
        case "font-family":
          format.FontFamily = this.GetFontName(paramValue);
          break;
        case "text-decoration":
          switch (paramValue)
          {
            case "underline":
              format.Underline = true;
              return;
            case "strike-through":
              format.Strike = true;
              return;
            case "none":
              format.Underline = false;
              format.Strike = false;
              return;
            case "line-through":
              format.Strike = true;
              return;
            default:
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "font-size":
          switch (paramValue)
          {
            case "smallar":
              format.FontSize = 10f;
              return;
            case "larger":
              format.FontSize = 13.5f;
              return;
            case "small":
              format.FontSize = 12f;
              return;
            case "large":
              format.FontSize = 18f;
              return;
            case "xx-small":
              format.FontSize = 7.5f;
              return;
            case "x-small":
              format.FontSize = 10f;
              return;
            case "x-large":
              format.FontSize = 24f;
              return;
            case "xx-large":
              format.FontSize = 36f;
              return;
            case "medium":
              format.FontSize = 13.5f;
              return;
            default:
              paramValue = paramValue.Remove(paramValue.Length - 2);
              format.FontSize = float.Parse(paramValue);
              return;
          }
        case "font-weight":
          if (paramValue == "normal")
          {
            format.Bold = false;
            break;
          }
          format.Bold = true;
          this.isPreserveBreakForInvalidStyles = true;
          break;
        case "font-style":
          switch (paramValue)
          {
            case "italic":
            case "oblique":
              format.Italic = true;
              return;
            case "strike":
              format.Strike = true;
              return;
            case "normal":
              format.Italic = false;
              format.Strike = false;
              return;
            default:
              this.isPreserveBreakForInvalidStyles = true;
              return;
          }
        case "display":
          if (!("none" == paramValue))
            break;
          format.Display = false;
          break;
      }
    }
    catch
    {
      this.isPreserveBreakForInvalidStyles = true;
    }
  }

  private void LeaveStyle(bool stylePresent)
  {
    if (!stylePresent)
      return;
    this.styleStack.Pop();
  }
}

// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.HtmlToXaml.HtmlSchema
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Collections;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;

internal class HtmlSchema
{
  private static ArrayList _htmlInlineElements;
  private static ArrayList _htmlBlockElements;
  private static ArrayList _htmlOtherOpenableElements;
  private static ArrayList _htmlEmptyElements;
  private static ArrayList _htmlElementsClosingOnParentElementEnd;
  private static ArrayList _htmlElementsClosingColgroup;
  private static ArrayList _htmlElementsClosingDd;
  private static ArrayList _htmlElementsClosingDt;
  private static ArrayList _htmlElementsClosingLi;
  private static ArrayList _htmlElementsClosingTbody;
  private static ArrayList _htmlElementsClosingTd;
  private static ArrayList _htmlElementsClosingTfoot;
  private static ArrayList _htmlElementsClosingThead;
  private static ArrayList _htmlElementsClosingTh;
  private static ArrayList _htmlElementsClosingTr;
  private static Hashtable _htmlCharacterEntities;

  static HtmlSchema()
  {
    HtmlSchema.InitializeInlineElements();
    HtmlSchema.InitializeBlockElements();
    HtmlSchema.InitializeOtherOpenableElements();
    HtmlSchema.InitializeEmptyElements();
    HtmlSchema.InitializeElementsClosingOnParentElementEnd();
    HtmlSchema.InitializeElementsClosingOnNewElementStart();
    HtmlSchema.InitializeHtmlCharacterEntities();
  }

  internal static bool IsEmptyElement(string xmlElementName)
  {
    return HtmlSchema._htmlEmptyElements.Contains((object) xmlElementName.ToLower());
  }

  internal static bool IsBlockElement(string xmlElementName)
  {
    return HtmlSchema._htmlBlockElements.Contains((object) xmlElementName);
  }

  internal static bool IsInlineElement(string xmlElementName)
  {
    return HtmlSchema._htmlInlineElements.Contains((object) xmlElementName);
  }

  internal static bool IsKnownOpenableElement(string xmlElementName)
  {
    return HtmlSchema._htmlOtherOpenableElements.Contains((object) xmlElementName);
  }

  internal static bool ClosesOnParentElementEnd(string xmlElementName)
  {
    return HtmlSchema._htmlElementsClosingOnParentElementEnd.Contains((object) xmlElementName.ToLower());
  }

  internal static bool ClosesOnNextElementStart(string currentElementName, string nextElementName)
  {
    switch (currentElementName)
    {
      case "colgroup":
        return HtmlSchema._htmlElementsClosingColgroup.Contains((object) nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
      case "dd":
        return HtmlSchema._htmlElementsClosingDd.Contains((object) nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
      case "dt":
        return HtmlSchema._htmlElementsClosingDt.Contains((object) nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
      case "li":
        return HtmlSchema._htmlElementsClosingLi.Contains((object) nextElementName);
      case "p":
        return HtmlSchema.IsBlockElement(nextElementName);
      case "tbody":
        return HtmlSchema._htmlElementsClosingTbody.Contains((object) nextElementName);
      case "td":
        return HtmlSchema._htmlElementsClosingTd.Contains((object) nextElementName);
      case "tfoot":
        return HtmlSchema._htmlElementsClosingTfoot.Contains((object) nextElementName);
      case "th":
        return HtmlSchema._htmlElementsClosingTh.Contains((object) nextElementName);
      case "thead":
        return HtmlSchema._htmlElementsClosingThead.Contains((object) nextElementName);
      case "tr":
        return HtmlSchema._htmlElementsClosingTr.Contains((object) nextElementName);
      default:
        return false;
    }
  }

  internal static bool IsEntity(string entityName)
  {
    return HtmlSchema._htmlCharacterEntities.Contains((object) entityName);
  }

  internal static char EntityCharacterValue(string entityName)
  {
    return HtmlSchema._htmlCharacterEntities.Contains((object) entityName) ? (char) HtmlSchema._htmlCharacterEntities[(object) entityName] : char.MinValue;
  }

  private static void InitializeInlineElements()
  {
    HtmlSchema._htmlInlineElements = new ArrayList()
    {
      (object) "a",
      (object) "abbr",
      (object) "acronym",
      (object) "address",
      (object) "b",
      (object) "bdo",
      (object) "big",
      (object) "button",
      (object) "code",
      (object) "del",
      (object) "dfn",
      (object) "em",
      (object) "font",
      (object) "i",
      (object) "ins",
      (object) "kbd",
      (object) "label",
      (object) "legend",
      (object) "q",
      (object) "s",
      (object) "samp",
      (object) "small",
      (object) "span",
      (object) "strike",
      (object) "strong",
      (object) "sub",
      (object) "sup",
      (object) "u",
      (object) "var"
    };
  }

  private static void InitializeBlockElements()
  {
    HtmlSchema._htmlBlockElements = new ArrayList()
    {
      (object) "blockquote",
      (object) "body",
      (object) "caption",
      (object) "center",
      (object) "cite",
      (object) "dd",
      (object) "dir",
      (object) "div",
      (object) "dl",
      (object) "dt",
      (object) "form",
      (object) "h1",
      (object) "h2",
      (object) "h3",
      (object) "h4",
      (object) "h5",
      (object) "h6",
      (object) "html",
      (object) "li",
      (object) "menu",
      (object) "ol",
      (object) "p",
      (object) "pre",
      (object) "table",
      (object) "tbody",
      (object) "td",
      (object) "textarea",
      (object) "tfoot",
      (object) "th",
      (object) "thead",
      (object) "tr",
      (object) "tt",
      (object) "ul"
    };
  }

  private static void InitializeEmptyElements()
  {
    HtmlSchema._htmlEmptyElements = new ArrayList()
    {
      (object) "area",
      (object) "base",
      (object) "basefont",
      (object) "br",
      (object) "col",
      (object) "frame",
      (object) "hr",
      (object) "img",
      (object) "input",
      (object) "isindex",
      (object) "link",
      (object) "meta",
      (object) "param"
    };
  }

  private static void InitializeOtherOpenableElements()
  {
    HtmlSchema._htmlOtherOpenableElements = new ArrayList()
    {
      (object) "applet",
      (object) "base",
      (object) "basefont",
      (object) "colgroup",
      (object) "fieldset",
      (object) "frameset",
      (object) "head",
      (object) "iframe",
      (object) "map",
      (object) "noframes",
      (object) "noscript",
      (object) "object",
      (object) "optgroup",
      (object) "option",
      (object) "script",
      (object) "select",
      (object) "style",
      (object) "title"
    };
  }

  private static void InitializeElementsClosingOnParentElementEnd()
  {
    HtmlSchema._htmlElementsClosingOnParentElementEnd = new ArrayList()
    {
      (object) "body",
      (object) "colgroup",
      (object) "dd",
      (object) "dt",
      (object) "head",
      (object) "html",
      (object) "li",
      (object) "p",
      (object) "tbody",
      (object) "td",
      (object) "tfoot",
      (object) "thead",
      (object) "th",
      (object) "tr"
    };
  }

  private static void InitializeElementsClosingOnNewElementStart()
  {
    HtmlSchema._htmlElementsClosingColgroup = new ArrayList()
    {
      (object) "colgroup",
      (object) "tr",
      (object) "thead",
      (object) "tfoot",
      (object) "tbody"
    };
    HtmlSchema._htmlElementsClosingDd = new ArrayList()
    {
      (object) "dd",
      (object) "dt"
    };
    HtmlSchema._htmlElementsClosingDt = new ArrayList();
    HtmlSchema._htmlElementsClosingDd.Add((object) "dd");
    HtmlSchema._htmlElementsClosingDd.Add((object) "dt");
    HtmlSchema._htmlElementsClosingLi = new ArrayList()
    {
      (object) "li"
    };
    HtmlSchema._htmlElementsClosingTbody = new ArrayList()
    {
      (object) "tbody",
      (object) "thead",
      (object) "tfoot"
    };
    HtmlSchema._htmlElementsClosingTr = new ArrayList()
    {
      (object) "thead",
      (object) "tfoot",
      (object) "tbody",
      (object) "tr"
    };
    HtmlSchema._htmlElementsClosingTd = new ArrayList()
    {
      (object) "td",
      (object) "th",
      (object) "tr",
      (object) "tbody",
      (object) "tfoot",
      (object) "thead"
    };
    HtmlSchema._htmlElementsClosingTh = new ArrayList()
    {
      (object) "td",
      (object) "th",
      (object) "tr",
      (object) "tbody",
      (object) "tfoot",
      (object) "thead"
    };
    HtmlSchema._htmlElementsClosingThead = new ArrayList()
    {
      (object) "tbody",
      (object) "tfoot"
    };
    HtmlSchema._htmlElementsClosingTfoot = new ArrayList()
    {
      (object) "tbody",
      (object) "thead"
    };
  }

  private static void InitializeHtmlCharacterEntities()
  {
    HtmlSchema._htmlCharacterEntities = new Hashtable()
    {
      [(object) "Aacute"] = (object) 'Á',
      [(object) "aacute"] = (object) 'á',
      [(object) "Acirc"] = (object) 'Â',
      [(object) "acirc"] = (object) 'â',
      [(object) "acute"] = (object) '´',
      [(object) "AElig"] = (object) 'Æ',
      [(object) "aelig"] = (object) 'æ',
      [(object) "Agrave"] = (object) 'À',
      [(object) "agrave"] = (object) 'à',
      [(object) "alefsym"] = (object) 'ℵ',
      [(object) "Alpha"] = (object) 'Α',
      [(object) "alpha"] = (object) 'α',
      [(object) "amp"] = (object) '&',
      [(object) "and"] = (object) '∧',
      [(object) "ang"] = (object) '∠',
      [(object) "Aring"] = (object) 'Å',
      [(object) "aring"] = (object) 'å',
      [(object) "asymp"] = (object) '≈',
      [(object) "Atilde"] = (object) 'Ã',
      [(object) "atilde"] = (object) 'ã',
      [(object) "Auml"] = (object) 'Ä',
      [(object) "auml"] = (object) 'ä',
      [(object) "bdquo"] = (object) '„',
      [(object) "Beta"] = (object) 'Β',
      [(object) "beta"] = (object) 'β',
      [(object) "brvbar"] = (object) '¦',
      [(object) "bull"] = (object) '•',
      [(object) "cap"] = (object) '∩',
      [(object) "Ccedil"] = (object) 'Ç',
      [(object) "ccedil"] = (object) 'ç',
      [(object) "cent"] = (object) '¢',
      [(object) "Chi"] = (object) 'Χ',
      [(object) "chi"] = (object) 'χ',
      [(object) "circ"] = (object) 'ˆ',
      [(object) "clubs"] = (object) '♣',
      [(object) "cong"] = (object) '≅',
      [(object) "copy"] = (object) '©',
      [(object) "crarr"] = (object) '↵',
      [(object) "cup"] = (object) '∪',
      [(object) "curren"] = (object) '¤',
      [(object) "dagger"] = (object) '†',
      [(object) "Dagger"] = (object) '‡',
      [(object) "darr"] = (object) '↓',
      [(object) "dArr"] = (object) '⇓',
      [(object) "deg"] = (object) '°',
      [(object) "Delta"] = (object) 'Δ',
      [(object) "delta"] = (object) 'δ',
      [(object) "diams"] = (object) '♦',
      [(object) "divide"] = (object) '÷',
      [(object) "Eacute"] = (object) 'É',
      [(object) "eacute"] = (object) 'é',
      [(object) "Ecirc"] = (object) 'Ê',
      [(object) "ecirc"] = (object) 'ê',
      [(object) "Egrave"] = (object) 'È',
      [(object) "egrave"] = (object) 'è',
      [(object) "empty"] = (object) '∅',
      [(object) "emsp"] = (object) ' ',
      [(object) "ensp"] = (object) ' ',
      [(object) "Epsilon"] = (object) 'Ε',
      [(object) "epsilon"] = (object) 'ε',
      [(object) "equiv"] = (object) '≡',
      [(object) "Eta"] = (object) 'Η',
      [(object) "eta"] = (object) 'η',
      [(object) "ETH"] = (object) 'Ð',
      [(object) "eth"] = (object) 'ð',
      [(object) "Euml"] = (object) 'Ë',
      [(object) "euml"] = (object) 'ë',
      [(object) "euro"] = (object) '€',
      [(object) "exist"] = (object) '∃',
      [(object) "fnof"] = (object) 'ƒ',
      [(object) "forall"] = (object) '∀',
      [(object) "frac12"] = (object) '\u00BD',
      [(object) "frac14"] = (object) '\u00BC',
      [(object) "frac34"] = (object) '\u00BE',
      [(object) "frasl"] = (object) '⁄',
      [(object) "Gamma"] = (object) 'Γ',
      [(object) "gamma"] = (object) 'γ',
      [(object) "ge"] = (object) '≥',
      [(object) "gt"] = (object) '>',
      [(object) "harr"] = (object) '↔',
      [(object) "hArr"] = (object) '⇔',
      [(object) "hearts"] = (object) '♥',
      [(object) "hellip"] = (object) '…',
      [(object) "Iacute"] = (object) 'Í',
      [(object) "iacute"] = (object) 'í',
      [(object) "Icirc"] = (object) 'Î',
      [(object) "icirc"] = (object) 'î',
      [(object) "iexcl"] = (object) '¡',
      [(object) "Igrave"] = (object) 'Ì',
      [(object) "igrave"] = (object) 'ì',
      [(object) "image"] = (object) 'ℑ',
      [(object) "infin"] = (object) '∞',
      [(object) "int"] = (object) '∫',
      [(object) "Iota"] = (object) 'Ι',
      [(object) "iota"] = (object) 'ι',
      [(object) "iquest"] = (object) '¿',
      [(object) "isin"] = (object) '∈',
      [(object) "Iuml"] = (object) 'Ï',
      [(object) "iuml"] = (object) 'ï',
      [(object) "Kappa"] = (object) 'Κ',
      [(object) "kappa"] = (object) 'κ',
      [(object) "Lambda"] = (object) 'Λ',
      [(object) "lambda"] = (object) 'λ',
      [(object) "lang"] = (object) '〈',
      [(object) "laquo"] = (object) '«',
      [(object) "larr"] = (object) '←',
      [(object) "lArr"] = (object) '⇐',
      [(object) "lceil"] = (object) '⌈',
      [(object) "ldquo"] = (object) '“',
      [(object) "le"] = (object) '≤',
      [(object) "lfloor"] = (object) '⌊',
      [(object) "lowast"] = (object) '∗',
      [(object) "loz"] = (object) '◊',
      [(object) "lrm"] = (object) '\u200E',
      [(object) "lsaquo"] = (object) '‹',
      [(object) "lsquo"] = (object) '‘',
      [(object) "lt"] = (object) '<',
      [(object) "macr"] = (object) '¯',
      [(object) "mdash"] = (object) '—',
      [(object) "micro"] = (object) 'µ',
      [(object) "middot"] = (object) '·',
      [(object) "minus"] = (object) '−',
      [(object) "Mu"] = (object) 'Μ',
      [(object) "mu"] = (object) 'μ',
      [(object) "nabla"] = (object) '∇',
      [(object) "nbsp"] = (object) ' ',
      [(object) "ndash"] = (object) '–',
      [(object) "ne"] = (object) '≠',
      [(object) "ni"] = (object) '∋',
      [(object) "not"] = (object) '¬',
      [(object) "notin"] = (object) '∉',
      [(object) "nsub"] = (object) '⊄',
      [(object) "Ntilde"] = (object) 'Ñ',
      [(object) "ntilde"] = (object) 'ñ',
      [(object) "Nu"] = (object) 'Ν',
      [(object) "nu"] = (object) 'ν',
      [(object) "Oacute"] = (object) 'Ó',
      [(object) "ocirc"] = (object) 'ô',
      [(object) "OElig"] = (object) 'Œ',
      [(object) "oelig"] = (object) 'œ',
      [(object) "Ograve"] = (object) 'Ò',
      [(object) "ograve"] = (object) 'ò',
      [(object) "oline"] = (object) '‾',
      [(object) "Omega"] = (object) 'Ω',
      [(object) "omega"] = (object) 'ω',
      [(object) "Omicron"] = (object) 'Ο',
      [(object) "omicron"] = (object) 'ο',
      [(object) "oplus"] = (object) '⊕',
      [(object) "or"] = (object) '∨',
      [(object) "ordf"] = (object) 'ª',
      [(object) "ordm"] = (object) 'º',
      [(object) "Oslash"] = (object) 'Ø',
      [(object) "oslash"] = (object) 'ø',
      [(object) "Otilde"] = (object) 'Õ',
      [(object) "otilde"] = (object) 'õ',
      [(object) "otimes"] = (object) '⊗',
      [(object) "Ouml"] = (object) 'Ö',
      [(object) "ouml"] = (object) 'ö',
      [(object) "para"] = (object) '¶',
      [(object) "part"] = (object) '∂',
      [(object) "permil"] = (object) '‰',
      [(object) "perp"] = (object) '⊥',
      [(object) "Phi"] = (object) 'Φ',
      [(object) "phi"] = (object) 'φ',
      [(object) "pi"] = (object) 'π',
      [(object) "piv"] = (object) 'ϖ',
      [(object) "plusmn"] = (object) '±',
      [(object) "pound"] = (object) '£',
      [(object) "prime"] = (object) '′',
      [(object) "Prime"] = (object) '″',
      [(object) "prod"] = (object) '∏',
      [(object) "prop"] = (object) '∝',
      [(object) "Psi"] = (object) 'Ψ',
      [(object) "psi"] = (object) 'ψ',
      [(object) "quot"] = (object) '"',
      [(object) "radic"] = (object) '√',
      [(object) "rang"] = (object) '〉',
      [(object) "raquo"] = (object) '»',
      [(object) "rarr"] = (object) '→',
      [(object) "rArr"] = (object) '⇒',
      [(object) "rceil"] = (object) '⌉',
      [(object) "rdquo"] = (object) '”',
      [(object) "real"] = (object) 'ℜ',
      [(object) "reg"] = (object) '®',
      [(object) "rfloor"] = (object) '⌋',
      [(object) "Rho"] = (object) 'Ρ',
      [(object) "rho"] = (object) 'ρ',
      [(object) "rlm"] = (object) '\u200F',
      [(object) "rsaquo"] = (object) '›',
      [(object) "rsquo"] = (object) '’',
      [(object) "sbquo"] = (object) '‚',
      [(object) "Scaron"] = (object) 'Š',
      [(object) "scaron"] = (object) 'š',
      [(object) "sdot"] = (object) '⋅',
      [(object) "sect"] = (object) '§',
      [(object) "shy"] = (object) '\u00AD',
      [(object) "Sigma"] = (object) 'Σ',
      [(object) "sigma"] = (object) 'σ',
      [(object) "sigmaf"] = (object) 'ς',
      [(object) "sim"] = (object) '∼',
      [(object) "spades"] = (object) '♠',
      [(object) "sub"] = (object) '⊂',
      [(object) "sube"] = (object) '⊆',
      [(object) "sum"] = (object) '∑',
      [(object) "sup"] = (object) '⊃',
      [(object) "sup1"] = (object) '\u00B9',
      [(object) "sup2"] = (object) '\u00B2',
      [(object) "sup3"] = (object) '\u00B3',
      [(object) "supe"] = (object) '⊇',
      [(object) "szlig"] = (object) 'ß',
      [(object) "Tau"] = (object) 'Τ',
      [(object) "tau"] = (object) 'τ',
      [(object) "there4"] = (object) '∴',
      [(object) "Theta"] = (object) 'Θ',
      [(object) "theta"] = (object) 'θ',
      [(object) "thetasym"] = (object) 'ϑ',
      [(object) "thinsp"] = (object) ' ',
      [(object) "THORN"] = (object) 'Þ',
      [(object) "thorn"] = (object) 'þ',
      [(object) "tilde"] = (object) '˜',
      [(object) "times"] = (object) '×',
      [(object) "trade"] = (object) '™',
      [(object) "Uacute"] = (object) 'Ú',
      [(object) "uacute"] = (object) 'ú',
      [(object) "uarr"] = (object) '↑',
      [(object) "uArr"] = (object) '⇑',
      [(object) "Ucirc"] = (object) 'Û',
      [(object) "ucirc"] = (object) 'û',
      [(object) "Ugrave"] = (object) 'Ù',
      [(object) "ugrave"] = (object) 'ù',
      [(object) "uml"] = (object) '¨',
      [(object) "upsih"] = (object) 'ϒ',
      [(object) "Upsilon"] = (object) 'Υ',
      [(object) "upsilon"] = (object) 'υ',
      [(object) "Uuml"] = (object) 'Ü',
      [(object) "uuml"] = (object) 'ü',
      [(object) "weierp"] = (object) '℘',
      [(object) "Xi"] = (object) 'Ξ',
      [(object) "xi"] = (object) 'ξ',
      [(object) "Yacute"] = (object) 'Ý',
      [(object) "yacute"] = (object) 'ý',
      [(object) "yen"] = (object) '¥',
      [(object) "Yuml"] = (object) 'Ÿ',
      [(object) "yuml"] = (object) 'ÿ',
      [(object) "Zeta"] = (object) 'Ζ',
      [(object) "zeta"] = (object) 'ζ',
      [(object) "zwj"] = (object) '\u200D',
      [(object) "zwnj"] = (object) '\u200C'
    };
  }
}

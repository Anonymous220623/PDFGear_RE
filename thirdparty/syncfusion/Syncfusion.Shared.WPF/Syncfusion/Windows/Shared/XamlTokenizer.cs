// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.XamlTokenizer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class XamlTokenizer
{
  private string input;
  private int position;
  private XamlTokenizerMode mode = XamlTokenizerMode.OutsideElement;

  public static List<XamlToken> Tokenize(string input)
  {
    XamlTokenizerMode x_mode = XamlTokenizerMode.OutsideElement;
    return new XamlTokenizer().Tokenize(input, ref x_mode);
  }

  public List<XamlToken> Tokenize(string input, ref XamlTokenizerMode x_mode)
  {
    this.input = input;
    this.mode = x_mode;
    this.position = 0;
    List<XamlToken> xamlTokenList = this.Tokenize();
    x_mode = this.mode;
    return xamlTokenList;
  }

  private List<XamlToken> Tokenize()
  {
    List<XamlToken> list = new List<XamlToken>();
    try
    {
      XamlToken xamlToken;
      do
      {
        int position = this.position;
        xamlToken = this.NextToken();
        this.input.Substring(position, (int) xamlToken.Length);
        list.Add(xamlToken);
      }
      while (xamlToken.Kind != XamlTokenKind.EOF);
      this.TokensToStrings(list, this.input);
    }
    catch (Exception ex)
    {
    }
    return list;
  }

  private List<string> TokensToStrings(List<XamlToken> list, string input)
  {
    List<string> strings = new List<string>();
    int startIndex = 0;
    foreach (XamlToken xamlToken in list)
    {
      strings.Add(input.Substring(startIndex, (int) xamlToken.Length));
      startIndex += (int) xamlToken.Length;
    }
    return strings;
  }

  public string RemainingText => this.input.Substring(this.position);

  private XamlToken NextToken()
  {
    if (this.position >= this.input.Length)
      return new XamlToken(XamlTokenKind.EOF, 0);
    XamlToken xamlToken;
    switch (this.mode)
    {
      case XamlTokenizerMode.InsideComment:
        xamlToken = this.TokenizeInsideComment();
        break;
      case XamlTokenizerMode.InsideProcessingInstruction:
        xamlToken = this.TokenizeInsideProcessingInstruction();
        break;
      case XamlTokenizerMode.AfterOpen:
        xamlToken = this.TokenizeName(XamlTokenKind.ElementName, XamlTokenizerMode.InsideElement);
        break;
      case XamlTokenizerMode.AfterAttributeName:
        xamlToken = this.TokenizeSimple("=", XamlTokenKind.Equals, XamlTokenizerMode.AfterAttributeEquals);
        break;
      case XamlTokenizerMode.AfterAttributeEquals:
        xamlToken = this.TokenizeAttributeValue();
        break;
      case XamlTokenizerMode.InsideElement:
        xamlToken = this.TokenizeInsideElement();
        break;
      case XamlTokenizerMode.OutsideElement:
        xamlToken = this.TokenizeOutsideElement();
        break;
      case XamlTokenizerMode.InsideCData:
        xamlToken = this.TokenizeInsideCData();
        break;
      default:
        xamlToken = new XamlToken(XamlTokenKind.EOF, 0);
        break;
    }
    return xamlToken;
  }

  private bool IsNameCharacter(char character)
  {
    return char.IsLetterOrDigit(character) || character == '.' | character == '-' | character == '_' | character == ':';
  }

  private XamlToken TokenizeAttributeValue()
  {
    int num = this.input.IndexOf(this.input[this.position], this.position + 1);
    XamlToken xamlToken = new XamlToken(XamlTokenKind.AttributeValue, num + 1 - this.position);
    this.position = num + 1;
    this.mode = XamlTokenizerMode.InsideElement;
    return xamlToken;
  }

  private XamlToken TokenizeName(XamlTokenKind kind, XamlTokenizerMode nextMode)
  {
    int position = this.position;
    while (position < this.input.Length && this.IsNameCharacter(this.input[position]))
      ++position;
    XamlToken xamlToken = new XamlToken(kind, position - this.position);
    this.mode = nextMode;
    this.position = position;
    return xamlToken;
  }

  private XamlToken TokenizeElementWhitespace()
  {
    int position = this.position;
    while (position < this.input.Length && char.IsWhiteSpace(this.input[position]))
      ++position;
    XamlToken xamlToken = new XamlToken(XamlTokenKind.ElementWhitespace, position - this.position);
    this.position = position;
    return xamlToken;
  }

  private bool StartsWith(string text)
  {
    return this.position + text.Length <= this.input.Length && this.input.Substring(this.position, text.Length) == text;
  }

  private XamlToken TokenizeInsideElement()
  {
    if (char.IsWhiteSpace(this.input[this.position]))
      return this.TokenizeElementWhitespace();
    if (this.StartsWith("/>"))
      return this.TokenizeSimple("/>", XamlTokenKind.SelfClose, XamlTokenizerMode.OutsideElement);
    return this.StartsWith(">") ? this.TokenizeSimple(">", XamlTokenKind.Close, XamlTokenizerMode.OutsideElement) : this.TokenizeName(XamlTokenKind.AttributeName, XamlTokenizerMode.AfterAttributeName);
  }

  private XamlToken TokenizeText()
  {
    int position = this.position;
    while (position < this.input.Length && this.input[position] != '<' && this.input[position] != '&')
      ++position;
    XamlToken xamlToken = new XamlToken(XamlTokenKind.TextContent, position - this.position);
    this.position = position;
    return xamlToken;
  }

  private XamlToken TokenizeOutsideElement()
  {
    if (this.position >= this.input.Length)
      return new XamlToken(XamlTokenKind.EOF, 0);
    switch (this.input[this.position])
    {
      case '&':
        return this.TokenizeEntity();
      case '<':
        return this.TokenizeOpen();
      default:
        return this.TokenizeText();
    }
  }

  private XamlToken TokenizeSimple(string text, XamlTokenKind kind, XamlTokenizerMode nextMode)
  {
    XamlToken xamlToken = new XamlToken(kind, text.Length);
    this.position += text.Length;
    this.mode = nextMode;
    return xamlToken;
  }

  private XamlToken TokenizeOpen()
  {
    if (this.StartsWith("<!--"))
      return this.TokenizeSimple("<!--", XamlTokenKind.CommentBegin, XamlTokenizerMode.InsideComment);
    if (this.StartsWith("<![CDATA["))
      return this.TokenizeSimple("<![CDATA[", XamlTokenKind.CDataBegin, XamlTokenizerMode.InsideCData);
    if (this.StartsWith("<?"))
      return this.TokenizeSimple("<?", XamlTokenKind.OpenProcessingInstruction, XamlTokenizerMode.InsideProcessingInstruction);
    return this.StartsWith("</") ? this.TokenizeSimple("</", XamlTokenKind.OpenClose, XamlTokenizerMode.AfterOpen) : this.TokenizeSimple("<", XamlTokenKind.Open, XamlTokenizerMode.AfterOpen);
  }

  private XamlToken TokenizeEntity()
  {
    XamlToken xamlToken = new XamlToken(XamlTokenKind.Entity, this.input.IndexOf(';', this.position) - this.position);
    this.position += (int) xamlToken.Length;
    return xamlToken;
  }

  private XamlToken TokenizeInsideProcessingInstruction()
  {
    int num = this.input.IndexOf("?>", this.position);
    if (this.position == num)
    {
      this.position += "?>".Length;
      this.mode = XamlTokenizerMode.OutsideElement;
      return new XamlToken(XamlTokenKind.CloseProcessingInstruction, "?>".Length);
    }
    XamlToken xamlToken = new XamlToken(XamlTokenKind.TextContent, num - this.position);
    this.position = num;
    return xamlToken;
  }

  private XamlToken TokenizeInsideCData()
  {
    int num = this.input.IndexOf("]]>", this.position);
    if (this.position == num)
    {
      this.position += "]]>".Length;
      this.mode = XamlTokenizerMode.OutsideElement;
      return new XamlToken(XamlTokenKind.CDataEnd, "]]>".Length);
    }
    XamlToken xamlToken = new XamlToken(XamlTokenKind.TextContent, num - this.position);
    this.position = num;
    return xamlToken;
  }

  private XamlToken TokenizeInsideComment()
  {
    int num = this.input.IndexOf("-->", this.position);
    if (this.position == num)
    {
      this.position += "-->".Length;
      this.mode = XamlTokenizerMode.OutsideElement;
      return new XamlToken(XamlTokenKind.CommentEnd, "-->".Length);
    }
    XamlToken xamlToken = new XamlToken(XamlTokenKind.CommentText, num - this.position);
    this.position = num;
    return xamlToken;
  }

  public static Color ColorForToken(XamlToken token, string tokenText)
  {
    Color color = Color.FromRgb((byte) 0, (byte) 0, (byte) 0);
    switch (token.Kind)
    {
      case XamlTokenKind.Open:
      case XamlTokenKind.Close:
      case XamlTokenKind.SelfClose:
      case XamlTokenKind.OpenClose:
      case XamlTokenKind.Equals:
      case XamlTokenKind.AttributeValue:
      case XamlTokenKind.CommentBegin:
      case XamlTokenKind.CommentEnd:
      case XamlTokenKind.OpenProcessingInstruction:
      case XamlTokenKind.CloseProcessingInstruction:
      case XamlTokenKind.CDataBegin:
      case XamlTokenKind.CDataEnd:
        color = Color.FromRgb((byte) 0, (byte) 0, byte.MaxValue);
        break;
      case XamlTokenKind.ElementName:
        color = Color.FromRgb((byte) 163, (byte) 21, (byte) 21);
        break;
      case XamlTokenKind.AttributeName:
      case XamlTokenKind.Entity:
        color = Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0);
        break;
      case XamlTokenKind.CommentText:
        color = Color.FromRgb((byte) 0, (byte) 128 /*0x80*/, (byte) 0);
        break;
      case XamlTokenKind.TextContent:
        color = Color.FromRgb((byte) 0, (byte) 0, (byte) 0);
        break;
    }
    if (token.Kind != XamlTokenKind.ElementWhitespace && token.Kind == XamlTokenKind.TextContent)
    {
      int num = tokenText.Trim() == string.Empty ? 1 : 0;
    }
    return color;
  }
}

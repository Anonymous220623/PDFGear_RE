// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.RichTextBoxExt
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

[ToolboxItem(false)]
internal class RichTextBoxExt : RichTextBox
{
  private int updating;
  private int oldEventMask;
  private string lastTag = " ";
  private bool isNested;
  private Dictionary<string, int> m_htmlDictionary;
  private Font m_Font;
  private PdfColor m_Color;
  private PdfColor colors;
  private int m_pdfFontHeight;
  private int m_htmlFontHeight;

  public TextAlign SelectionAlignment
  {
    get
    {
      PARAFORMAT lp = new PARAFORMAT();
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1085, 1, ref lp);
      return ((int) lp.dwMask & 8) == 0 ? TextAlign.Left : (TextAlign) lp.wAlignment;
    }
    set
    {
      PARAFORMAT lp = new PARAFORMAT();
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      lp.dwMask = 8U;
      lp.wAlignment = (short) value;
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1095, 1, ref lp);
    }
  }

  public PARAFORMAT ParaFormat
  {
    get
    {
      PARAFORMAT lp = new PARAFORMAT();
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1085, 1, ref lp);
      return lp;
    }
    set
    {
      PARAFORMAT lp = value;
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1095, 1, ref lp);
    }
  }

  public PARAFORMAT DefaultParaFormat
  {
    get
    {
      PARAFORMAT lp = new PARAFORMAT();
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1085, 4, ref lp);
      return lp;
    }
    set
    {
      PARAFORMAT lp = value;
      lp.cbSize = Marshal.SizeOf<PARAFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1095, 4, ref lp);
    }
  }

  public CHARFORMAT CharFormat
  {
    get
    {
      CHARFORMAT lp = new CHARFORMAT();
      lp.cbSize = Marshal.SizeOf<CHARFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1082, 1, ref lp);
      return lp;
    }
    set
    {
      CHARFORMAT lp = value;
      lp.cbSize = Marshal.SizeOf<CHARFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1092, 1, ref lp);
    }
  }

  public CHARFORMAT DefaultCharFormat
  {
    get
    {
      CHARFORMAT lp = new CHARFORMAT();
      lp.cbSize = Marshal.SizeOf<CHARFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1082, 4, ref lp);
      return lp;
    }
    set
    {
      CHARFORMAT lp = value;
      lp.cbSize = Marshal.SizeOf<CHARFORMAT>(lp);
      RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1092, 4, ref lp);
    }
  }

  public void BeginUpdate()
  {
    ++this.updating;
    if (this.updating > 1)
      return;
    this.oldEventMask = RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1073, 0, 0);
    RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 11, 0, 0);
  }

  public void EndUpdate()
  {
    --this.updating;
    if (this.updating > 0)
      return;
    RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 11, 1, 0);
    RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1073, 0, this.oldEventMask);
  }

  public bool InternalUpdating => this.updating != 0;

  protected override void OnHandleCreated(EventArgs e)
  {
    base.OnHandleCreated(e);
    RtfApi.SendMessage(new HandleRef((object) this, this.Handle), 1226, 1, 1);
  }

  private Color GetColor(int crColor)
  {
    return Color.FromArgb((int) (byte) crColor, (int) (byte) (crColor >> 8), (int) (byte) (crColor >> 16 /*0x10*/));
  }

  private void Initialize()
  {
    this.m_htmlDictionary = new Dictionary<string, int>(10);
    this.m_htmlDictionary.Add("font", 0);
    this.m_htmlDictionary.Add("b", 1);
    this.m_htmlDictionary.Add("i", 2);
    this.m_htmlDictionary.Add("u", 3);
    this.m_htmlDictionary.Add("st", 4);
    this.m_htmlDictionary.Add("sup", 6);
    this.m_htmlDictionary.Add("sub", 7);
    this.m_htmlDictionary.Add("p", 8);
    this.m_htmlDictionary.Add("li", 9);
  }

  private int GetCOLORREF(int r, int g, int b) => r | g << 8 | b << 16 /*0x10*/;

  private int GetCOLORREF(Color color)
  {
    return this.GetCOLORREF((int) color.R, (int) color.G, (int) color.B);
  }

  public void RenderHTML(string strHTML, PdfFont font, PdfBrush color)
  {
    string familyName = font.Name;
    if (font is PdfStandardFont)
    {
      if (font.Name == "TimesRoman")
        familyName = "Times New Roman";
      else if (font.Name == "Courier")
        familyName = "Courier New";
    }
    this.m_Font = new Font(familyName, font.Size, (FontStyle) font.Style);
    PdfSolidBrush pdfSolidBrush = color as PdfSolidBrush;
    this.m_Color = pdfSolidBrush.Color;
    this.colors = pdfSolidBrush.Color;
    this.ParseHtml(strHTML);
  }

  public void ParseHtml(string strHTML)
  {
    CHARFORMAT defaultCharFormat = this.DefaultCharFormat;
    PARAFORMAT defaultParaFormat = this.DefaultParaFormat;
    if (this.m_pdfFontHeight == 0)
      this.m_pdfFontHeight = (int) ((double) defaultCharFormat.yHeight * (double) this.m_Font.Size / 8.0);
    string str = new string(defaultCharFormat.szFaceName);
    int crTextColor = defaultCharFormat.crTextColor;
    int pdfFontHeight1 = this.m_pdfFontHeight;
    string name = this.m_Font.Name;
    int colorref1 = this.GetCOLORREF((int) this.m_Color.Red, (int) this.m_Color.Green, (int) this.m_Color.Blue);
    int pdfFontHeight2 = this.m_pdfFontHeight;
    defaultCharFormat.szFaceName = new char[32 /*0x20*/];
    name.CopyTo(0, defaultCharFormat.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
    defaultCharFormat.crTextColor = colorref1;
    defaultCharFormat.yHeight = pdfFontHeight2;
    defaultCharFormat.dwMask |= 3758096384U /*0xE0000000*/;
    defaultCharFormat.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
    this.HideSelection = true;
    this.BeginUpdate();
    strHTML = strHTML.Replace("<br/>", "\r\n");
    strHTML = strHTML.Replace("<BR/>", "\r\n");
    strHTML = strHTML.Replace("<br />", "\r\n");
    strHTML = strHTML.Replace("<BR />", "\r\n");
    strHTML = strHTML.Replace("&nbsp;", " ");
    strHTML = strHTML.Replace("&", "&amp;");
    string xml = $"<html>{strHTML}</html>";
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.PreserveWhitespace = true;
    xmlDocument.LoadXml(xml);
    foreach (XmlNode childNode in xmlDocument.ChildNodes[0].ChildNodes)
    {
      this.ParseXmlNode(childNode, ref defaultCharFormat, ref defaultParaFormat);
      this.isNested = false;
      int colorref2 = this.GetCOLORREF((Color) this.colors);
      defaultCharFormat.crTextColor = colorref2;
    }
    this.SelectionStart = this.TextLength + 1;
    this.SelectionLength = 0;
    this.EndUpdate();
    this.HideSelection = false;
  }

  internal void ParseXmlNode(XmlNode node, ref CHARFORMAT cf, ref PARAFORMAT pf)
  {
    if (this.m_htmlDictionary == null)
      this.Initialize();
    if (node.PreviousSibling != null)
    {
      if (node.PreviousSibling.Name == "font")
      {
        this.m_htmlFontHeight = this.m_pdfFontHeight;
        cf.yHeight = this.m_htmlFontHeight;
      }
    }
    else if (node.ParentNode != null)
    {
      if (node.ParentNode.Name == "p")
      {
        this.m_htmlFontHeight = this.m_pdfFontHeight;
        cf.yHeight = this.m_htmlFontHeight;
      }
      else if (node.ParentNode.Name == "div")
      {
        this.m_htmlFontHeight = this.m_pdfFontHeight;
        cf.yHeight = this.m_htmlFontHeight;
      }
    }
    string lower1 = node.Name.ToLower();
    char[] chArray = new char[2]{ ' ', char.MinValue };
    this.HideSelection = true;
    this.BeginUpdate();
    int num1;
    if (lower1 != null && this.m_htmlDictionary.TryGetValue(lower1, out num1))
    {
      switch (num1)
      {
        case 0:
          if (node.Attributes != null)
          {
            string str = new string(cf.szFaceName);
            int num2 = cf.crTextColor;
            int num3 = cf.yHeight;
            foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
            {
              string lower2 = attribute.Name.ToLower();
              str = str.Trim(chArray);
              switch (lower2)
              {
                case "color":
                  if (attribute.Value[0] != '#')
                  {
                    Color color = Color.FromName(attribute.Value);
                    num2 = this.GetCOLORREF(color);
                    this.m_Color = (PdfColor) color;
                    continue;
                  }
                  try
                  {
                    Color color = Color.FromArgb((int) (byte) int.Parse(attribute.Value.Substring(1, 2), NumberStyles.HexNumber), (int) (byte) int.Parse(attribute.Value.Substring(3, 2), NumberStyles.HexNumber), (int) (byte) int.Parse(attribute.Value.Substring(5, 2), NumberStyles.HexNumber));
                    num2 = this.GetCOLORREF(color);
                    this.m_Color = (PdfColor) color;
                    continue;
                  }
                  catch
                  {
                    continue;
                  }
                case "size":
                  num3 = (int) ((double) float.Parse(attribute.Value) * 20.0 * 5.0);
                  this.m_htmlFontHeight = num3;
                  continue;
                case "face":
                  str = attribute.Value;
                  continue;
                default:
                  continue;
              }
            }
            cf.szFaceName = new char[32 /*0x20*/];
            str.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, str.Length));
            cf.crTextColor = num2;
            cf.yHeight = this.m_htmlFontHeight != 0 ? this.m_htmlFontHeight : num3;
            cf.dwMask |= 3758096384U /*0xE0000000*/;
            cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
          }
          this.lastTag += "font";
          break;
        case 1:
          cf.dwMask |= 4194305U /*0x400001*/;
          cf.dwEffects |= 1U;
          cf.wWeight = (short) 700;
          this.lastTag += "b";
          break;
        case 2:
          cf.dwMask |= 2U;
          cf.dwEffects |= 2U;
          this.lastTag += "i";
          break;
        case 3:
          cf.dwMask |= 8388612U /*0x800004*/;
          cf.dwEffects |= 4U;
          cf.bUnderlineType = (byte) 1;
          this.lastTag += "u";
          break;
        case 4:
          cf.dwMask |= 8U;
          cf.dwEffects |= 8U;
          this.lastTag += "s";
          break;
        case 5:
          pf.dwMask = 40U;
          pf.wAlignment = (short) 1;
          pf.wNumbering = (short) 0;
          break;
        case 6:
          cf.dwMask |= 196608U /*0x030000*/;
          cf.dwEffects |= 131072U /*0x020000*/;
          this.lastTag += "sup";
          break;
        case 7:
          cf.dwMask |= 196608U /*0x030000*/;
          cf.dwEffects |= 65536U /*0x010000*/;
          this.lastTag += "sub";
          break;
        case 8:
          if (node.Attributes != null)
          {
            foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
            {
              switch (attribute.Name.ToLower())
              {
                case "align":
                  if (attribute.Value.IndexOf("left") > 0)
                  {
                    pf.dwMask |= 8U;
                    pf.wAlignment = (short) 1;
                    continue;
                  }
                  if (attribute.Value.IndexOf("right") > 0)
                  {
                    pf.dwMask |= 8U;
                    pf.wAlignment = (short) 2;
                    continue;
                  }
                  if (attribute.Value.IndexOf("center") > 0)
                  {
                    pf.dwMask |= 8U;
                    pf.wAlignment = (short) 3;
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
          this.lastTag += "p";
          break;
        case 9:
          if (pf.wNumbering != (short) 1)
          {
            pf.dwMask |= 32U /*0x20*/;
            pf.wNumbering = (short) 1;
          }
          this.lastTag += "li";
          break;
      }
    }
    if (node.Name == "#text")
    {
      int length = node.OuterXml.Length;
      int selectionStart = this.SelectionStart;
      this.SelectedText = this.GetSafeText(node.OuterXml);
      this.SelectionStart = selectionStart;
      this.SelectionLength = node.OuterXml.Length;
      this.ParaFormat = pf;
      this.CharFormat = cf;
      this.SelectionStart = this.TextLength + 1;
      this.SelectionLength = 0;
      if (this.lastTag.IndexOf("b") >= 0)
      {
        cf.dwEffects &= 4294967294U;
        cf.wWeight = (short) 400;
      }
      if (this.lastTag.IndexOf("i") >= 0)
        cf.dwEffects &= 4294967293U;
      if (this.lastTag.IndexOf("u") >= 0)
        cf.dwEffects &= 4294967291U;
      if (this.lastTag.IndexOf("s") >= 0)
        cf.dwEffects &= 4294967287U;
      if (this.lastTag.IndexOf("sup") >= 0)
        cf.dwEffects &= 4294836223U;
      if (this.lastTag.IndexOf("sub") >= 0)
        cf.dwEffects &= 4294901759U;
      if (this.lastTag.IndexOf("font") >= 0)
      {
        string name = this.m_Font.Name;
        cf.szFaceName = new char[32 /*0x20*/];
        name.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
        cf.crTextColor = this.GetCOLORREF((Color) this.m_Color);
        cf.dwMask |= 3758096384U /*0xE0000000*/;
        cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
      }
      this.lastTag = "";
    }
    else if (node.Name == "#whitespace")
    {
      int selectionStart = this.SelectionStart;
      this.SelectedText = node.OuterXml;
      this.SelectionStart = selectionStart;
      this.SelectionLength = node.OuterXml.Length;
      this.SelectionStart = this.TextLength + 1;
      this.SelectionLength = 0;
    }
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.ParentNode.Name == "p" && this.lastTag == "")
      {
        string name = this.m_Font.Name;
        cf.szFaceName = new char[32 /*0x20*/];
        name.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
        cf.crTextColor = this.GetCOLORREF((Color) this.colors);
        cf.dwMask |= 3758096384U /*0xE0000000*/;
        cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
      }
      else if (childNode.ParentNode.Name == "b" && this.lastTag == "")
      {
        string name = this.m_Font.Name;
        cf.szFaceName = new char[32 /*0x20*/];
        name.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
        cf.crTextColor = this.GetCOLORREF((Color) this.colors);
        cf.dwMask |= 3758096384U /*0xE0000000*/;
        cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
      }
      else if (childNode.ParentNode.Name == "i" && this.lastTag == "")
      {
        string name = this.m_Font.Name;
        cf.szFaceName = new char[32 /*0x20*/];
        name.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
        cf.crTextColor = this.GetCOLORREF((Color) this.colors);
        cf.dwMask |= 3758096384U /*0xE0000000*/;
        cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
      }
      else if (childNode.ParentNode.Name == "u" && this.lastTag == "")
      {
        string name = this.m_Font.Name;
        cf.szFaceName = new char[32 /*0x20*/];
        name.CopyTo(0, cf.szFaceName, 0, Math.Min(31 /*0x1F*/, name.Length));
        cf.crTextColor = this.GetCOLORREF((Color) this.colors);
        cf.dwMask |= 3758096384U /*0xE0000000*/;
        cf.dwEffects &= 3221225471U /*0xBFFFFFFF*/;
      }
      this.isNested = true;
      this.lastTag += childNode.Name;
      this.ParseXmlNode(childNode, ref cf, ref pf);
    }
  }

  private string GetSafeText(string text)
  {
    text = text.Replace("&amp;", "&");
    text = text.Replace("&lt;", "<");
    text = text.Replace("&gt;", ">");
    return text;
  }
}

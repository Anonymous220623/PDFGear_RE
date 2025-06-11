// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Redaction.StringMapping
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Syncfusion.Pdf.Redaction;

internal class StringMapping
{
  internal string text;
  internal Glyph[] glyph;
  private Dictionary<char, int> m_macReverseEncodeTable;

  internal string GetText(FontStructure fontStructure, bool isHex)
  {
    if (this.glyph == null)
      return this.text;
    string text1 = string.Empty;
    string text2 = "";
    bool flag1 = this.text.Length >= 2;
    bool flag2 = this.text.StartsWith("(");
    bool flag3 = this.text.EndsWith(")");
    if (flag1 && flag2 && !flag3)
      text2 = this.text.Substring(1, this.text.Length - 1);
    else if (flag1 && !flag2 && flag3)
      text2 = this.text.Substring(0, this.text.Length - 1);
    else if (flag1)
      text2 = this.text.Substring(1, this.text.Length - 2);
    Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
    Dictionary<string, Glyph[]> dictionary2 = new Dictionary<string, Glyph[]>();
    string empty1 = string.Empty;
    List<Glyph> glyphList1 = new List<Glyph>();
    string str1 = string.Empty;
    int num1 = 0;
    int num2 = 0;
    foreach (Glyph glyph in this.glyph)
    {
      if (glyph.IsReplace)
      {
        if (str1 != string.Empty)
        {
          dictionary1.Add("O-" + num2++.ToString(), str1);
          str1 = string.Empty;
        }
        glyphList1.Add(glyph);
        empty1 += glyph.ToUnicode;
      }
      else
      {
        if (empty1 != string.Empty)
        {
          string key = "R-" + num1++.ToString();
          dictionary2.Add(key, glyphList1.ToArray());
          glyphList1.Clear();
          glyphList1 = new List<Glyph>();
          dictionary1.Add(key, empty1);
          empty1 = string.Empty;
        }
        str1 = glyph.ToUnicode == "(" || glyph.ToUnicode == ")" ? $"{str1}\\{glyph.ToUnicode}" : str1 + glyph.ToUnicode;
      }
    }
    if (empty1 != string.Empty)
    {
      int num3 = num1;
      int num4 = num3 + 1;
      string key = "R-" + num3.ToString();
      dictionary2.Add(key, glyphList1.ToArray());
      dictionary1.Add(key, empty1);
      glyphList1.Clear();
      List<Glyph> glyphList2 = new List<Glyph>();
      string empty2 = string.Empty;
    }
    if (str1 != string.Empty)
    {
      Dictionary<string, string> dictionary3 = dictionary1;
      int num5 = num2;
      int num6 = num5 + 1;
      string key = "O-" + num5.ToString();
      string str2 = str1;
      dictionary3.Add(key, str2);
      string empty3 = string.Empty;
    }
    foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
    {
      if (keyValuePair.Key.Contains("O"))
      {
        string empty4 = string.Empty;
        if (fontStructure.FontEncoding == "Identity-H" || fontStructure.FontEncoding == "" && fontStructure.CharacterMapTable != null && fontStructure.CharacterMapTable.Count > 0)
        {
          foreach (char key in keyValuePair.Value)
          {
            if (key.ToString() != "\\")
            {
              string empty5 = string.Empty;
              double num7;
              if (fontStructure.CharacterMapTable.ContainsKey((double) key))
              {
                string str3 = fontStructure.CharacterMapTable[(double) key];
                string str4 = text2.Substring(0, str3.Length);
                text2 = text2.Substring(str3.Length, text2.Length - str3.Length);
                num7 = !(str3 == str4) ? this.GetKey(fontStructure.CharacterMapTable, key.ToString()) : (double) key;
              }
              else
              {
                num7 = this.GetKey(fontStructure.CharacterMapTable, key.ToString());
                text2 = text2.Substring(key.ToString().Length, text2.Length - key.ToString().Length);
              }
              empty4 += (string) (object) (char) num7;
            }
          }
        }
        else if (fontStructure.FontEncoding == "MacRomanEncoding")
        {
          if (this.m_macReverseEncodeTable == null || this.m_macReverseEncodeTable.Count <= 0)
            this.GetReverseMacEncodeTable();
          string str5 = string.Empty;
          foreach (char key in keyValuePair.Value)
            str5 = !this.m_macReverseEncodeTable.ContainsKey(key) || key == ' ' ? str5 + key.ToString() : $"{str5}\\{Convert.ToString(this.m_macReverseEncodeTable[key], 8)}";
          empty4 += str5;
        }
        else
          empty4 += keyValuePair.Value;
        if (fontStructure.FontEncoding == "Identity-H")
        {
          byte[] numArray = this.GetUnicodeString(PdfString.ByteToString(PdfString.ToUnicodeArray(empty4, false))).PdfEncode((PdfDocumentBase) null);
          text1 += new PdfString(numArray).Value;
        }
        else if (!isHex)
        {
          text1 += "(";
          text1 = !(fontStructure.FontName == "ZapfDingbats") || fontStructure.isEmbedded ? text1 + empty4 : text1 + this.ReverseMapZapf(empty4, fontStructure);
          text1 += ")";
        }
        else
        {
          text1 += "<";
          if (fontStructure.FontName == "ZapfDingbats" && !fontStructure.isEmbedded)
          {
            text1 += this.ReverseMapZapf(empty4, fontStructure);
          }
          else
          {
            char[] charArray = empty4.ToCharArray();
            string empty6 = string.Empty;
            foreach (char ch in charArray)
            {
              byte num8 = Convert.ToByte(ch);
              empty6 += num8.ToString("X2");
            }
            text1 += empty6;
          }
          text1 += ">";
        }
      }
      if (keyValuePair.Key.Contains("R"))
      {
        text1 = $"{text1} -{this.GetReplacedChar(dictionary2[keyValuePair.Key], fontStructure).ToString()} ";
        text2 = this.ReplacedText(text2, dictionary2[keyValuePair.Key], fontStructure);
      }
    }
    return text1;
  }

  private string ReplacedText(string text, Glyph[] glyphs, FontStructure structure)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    string str1 = text;
    int startIndex = 0;
    foreach (Glyph glyph in glyphs)
    {
      string toUnicode = glyph.ToUnicode;
      if (toUnicode != null)
      {
        char[] charArray = toUnicode.ToCharArray();
        if (structure.CharacterMapTable.ContainsKey((double) charArray[0]))
        {
          string str2 = structure.CharacterMapTable[(double) charArray[0]];
          startIndex += str2.Length;
        }
        else
          startIndex += toUnicode.Length;
      }
    }
    return str1.Substring(startIndex, str1.Length - startIndex);
  }

  private string ReverseMapZapf(string encodedText, FontStructure structure)
  {
    string str = (string) null;
    foreach (char ch in encodedText)
    {
      switch (ch)
      {
        case ' ':
          str += (string) (object) (char) Convert.ToInt32("20", 16 /*0x10*/);
          break;
        case '→':
          str += (string) (object) (char) Convert.ToInt32("D5", 16 /*0x10*/);
          break;
        case '↔':
          str += (string) (object) (char) Convert.ToInt32("D6", 16 /*0x10*/);
          break;
        case '↕':
          str += (string) (object) (char) Convert.ToInt32("D7", 16 /*0x10*/);
          break;
        case '\u2460':
          str += (string) (object) (char) Convert.ToInt32("AC", 16 /*0x10*/);
          break;
        case '\u2461':
          str += (string) (object) (char) Convert.ToInt32("AD", 16 /*0x10*/);
          break;
        case '\u2462':
          str += (string) (object) (char) Convert.ToInt32("AE", 16 /*0x10*/);
          break;
        case '\u2463':
          str += (string) (object) (char) Convert.ToInt32("AF", 16 /*0x10*/);
          break;
        case '\u2464':
          str += (string) (object) (char) Convert.ToInt32("B0", 16 /*0x10*/);
          break;
        case '\u2465':
          str += (string) (object) (char) Convert.ToInt32("B1", 16 /*0x10*/);
          break;
        case '\u2466':
          str += (string) (object) (char) Convert.ToInt32("B2", 16 /*0x10*/);
          break;
        case '\u2467':
          str += (string) (object) (char) Convert.ToInt32("B3", 16 /*0x10*/);
          break;
        case '\u2468':
          str += (string) (object) (char) Convert.ToInt32("B4", 16 /*0x10*/);
          break;
        case '\u2469':
          str += (string) (object) (char) Convert.ToInt32("B5", 16 /*0x10*/);
          break;
        case '╍':
          str += (string) (object) (char) Convert.ToInt32("6D", 16 /*0x10*/);
          break;
        case '■':
          str += (string) (object) (char) Convert.ToInt32("6E", 16 /*0x10*/);
          break;
        case '▲':
          str += (string) (object) (char) Convert.ToInt32("73", 16 /*0x10*/);
          break;
        case '▼':
          str += (string) (object) (char) Convert.ToInt32("74", 16 /*0x10*/);
          break;
        case '●':
          str += (string) (object) (char) Convert.ToInt32("6C", 16 /*0x10*/);
          break;
        case '◗':
          str += (string) (object) (char) Convert.ToInt32("77", 16 /*0x10*/);
          break;
        case '★':
          str += (string) (object) (char) Convert.ToInt32("48", 16 /*0x10*/);
          break;
        case '☎':
          str += (string) (object) (char) Convert.ToInt32("25", 16 /*0x10*/);
          break;
        case '☛':
          str += (string) (object) (char) Convert.ToInt32("2A", 16 /*0x10*/);
          break;
        case '☞':
          str += (string) (object) (char) Convert.ToInt32("2B", 16 /*0x10*/);
          break;
        case '♠':
          str += (string) (object) (char) Convert.ToInt32("AB", 16 /*0x10*/);
          break;
        case '♣':
          str += (string) (object) (char) Convert.ToInt32("A8", 16 /*0x10*/);
          break;
        case '♥':
          str += (string) (object) (char) Convert.ToInt32("AA", 16 /*0x10*/);
          break;
        case '♦':
          str += (string) (object) (char) Convert.ToInt32("A9", 16 /*0x10*/);
          break;
        case '✁':
          str += (string) (object) (char) Convert.ToInt32("21", 16 /*0x10*/);
          break;
        case '✂':
          str += (string) (object) (char) Convert.ToInt32("22", 16 /*0x10*/);
          break;
        case '✃':
          str += (string) (object) (char) Convert.ToInt32("23", 16 /*0x10*/);
          break;
        case '✄':
          str += (string) (object) (char) Convert.ToInt32("24", 16 /*0x10*/);
          break;
        case '✆':
          str += (string) (object) (char) Convert.ToInt32("26", 16 /*0x10*/);
          break;
        case '✇':
          str += (string) (object) (char) Convert.ToInt32("27", 16 /*0x10*/);
          break;
        case '✈':
          str += (string) (object) (char) Convert.ToInt32("28", 16 /*0x10*/);
          break;
        case '✉':
          str += (string) (object) (char) Convert.ToInt32("29", 16 /*0x10*/);
          break;
        case '✌':
          str += (string) (object) (char) Convert.ToInt32("2C", 16 /*0x10*/);
          break;
        case '✍':
          str += (string) (object) (char) Convert.ToInt32("2D", 16 /*0x10*/);
          break;
        case '✎':
          str += (string) (object) (char) Convert.ToInt32("2E", 16 /*0x10*/);
          break;
        case '✏':
          str += (string) (object) (char) Convert.ToInt32("2F", 16 /*0x10*/);
          break;
        case '✐':
          str += (string) (object) (char) Convert.ToInt32("30", 16 /*0x10*/);
          break;
        case '✑':
          str += (string) (object) (char) Convert.ToInt32("31", 16 /*0x10*/);
          break;
        case '✒':
          str += (string) (object) (char) Convert.ToInt32("32", 16 /*0x10*/);
          break;
        case '✓':
          str += (string) (object) (char) Convert.ToInt32("33", 16 /*0x10*/);
          break;
        case '✔':
          str += (string) (object) (char) Convert.ToInt32("34", 16 /*0x10*/);
          break;
        case '✕':
          str += (string) (object) (char) Convert.ToInt32("35", 16 /*0x10*/);
          break;
        case '✖':
          str += (string) (object) (char) Convert.ToInt32("36", 16 /*0x10*/);
          break;
        case '✗':
          str += (string) (object) (char) Convert.ToInt32("37", 16 /*0x10*/);
          break;
        case '✘':
          str += (string) (object) (char) Convert.ToInt32("38", 16 /*0x10*/);
          break;
        case '✙':
          str += (string) (object) (char) Convert.ToInt32("39", 16 /*0x10*/);
          break;
        case '✚':
          str += (string) (object) (char) Convert.ToInt32("3A", 16 /*0x10*/);
          break;
        case '✛':
          str += (string) (object) (char) Convert.ToInt32("3B", 16 /*0x10*/);
          break;
        case '✜':
          str += (string) (object) (char) Convert.ToInt32("3C", 16 /*0x10*/);
          break;
        case '✝':
          str += (string) (object) (char) Convert.ToInt32("3D", 16 /*0x10*/);
          break;
        case '✞':
          str += (string) (object) (char) Convert.ToInt32("3E", 16 /*0x10*/);
          break;
        case '✟':
          str += (string) (object) (char) Convert.ToInt32("3F", 16 /*0x10*/);
          break;
        case '✠':
          str += (string) (object) (char) Convert.ToInt32("40", 16 /*0x10*/);
          break;
        case '✡':
          str += (string) (object) (char) Convert.ToInt32("41", 16 /*0x10*/);
          break;
        case '✢':
          str += (string) (object) (char) Convert.ToInt32("42", 16 /*0x10*/);
          break;
        case '✣':
          str += (string) (object) (char) Convert.ToInt32("43", 16 /*0x10*/);
          break;
        case '✤':
          str += (string) (object) (char) Convert.ToInt32("44", 16 /*0x10*/);
          break;
        case '✥':
          str += (string) (object) (char) Convert.ToInt32("45", 16 /*0x10*/);
          break;
        case '✦':
          str += (string) (object) (char) Convert.ToInt32("46", 16 /*0x10*/);
          break;
        case '✧':
          str += (string) (object) (char) Convert.ToInt32("47", 16 /*0x10*/);
          break;
        case '✩':
          str += (string) (object) (char) Convert.ToInt32("49", 16 /*0x10*/);
          break;
        case '✪':
          str += (string) (object) (char) Convert.ToInt32("4A", 16 /*0x10*/);
          break;
        case '✫':
          str += (string) (object) (char) Convert.ToInt32("4B", 16 /*0x10*/);
          break;
        case '✬':
          str += (string) (object) (char) Convert.ToInt32("4C", 16 /*0x10*/);
          break;
        case '✭':
          str += (string) (object) (char) Convert.ToInt32("4D", 16 /*0x10*/);
          break;
        case '✮':
          str += (string) (object) (char) Convert.ToInt32("4E", 16 /*0x10*/);
          break;
        case '✯':
          str += (string) (object) (char) Convert.ToInt32("4F", 16 /*0x10*/);
          break;
        case '✰':
          str += (string) (object) (char) Convert.ToInt32("50", 16 /*0x10*/);
          break;
        case '✱':
          str += (string) (object) (char) Convert.ToInt32("51", 16 /*0x10*/);
          break;
        case '✲':
          str += (string) (object) (char) Convert.ToInt32("52", 16 /*0x10*/);
          break;
        case '✳':
          str += (string) (object) (char) Convert.ToInt32("53", 16 /*0x10*/);
          break;
        case '✴':
          str += (string) (object) (char) Convert.ToInt32("54", 16 /*0x10*/);
          break;
        case '✵':
          str += (string) (object) (char) Convert.ToInt32("55", 16 /*0x10*/);
          break;
        case '✶':
          str += (string) (object) (char) Convert.ToInt32("56", 16 /*0x10*/);
          break;
        case '✷':
          str += (string) (object) (char) Convert.ToInt32("57", 16 /*0x10*/);
          break;
        case '✸':
          str += (string) (object) (char) Convert.ToInt32("58", 16 /*0x10*/);
          break;
        case '✹':
          str += (string) (object) (char) Convert.ToInt32("59", 16 /*0x10*/);
          break;
        case '✺':
          str += (string) (object) (char) Convert.ToInt32("5A", 16 /*0x10*/);
          break;
        case '✻':
          str += (string) (object) (char) Convert.ToInt32("5B", 16 /*0x10*/);
          break;
        case '✼':
          str += (string) (object) (char) Convert.ToInt32("5C", 16 /*0x10*/);
          break;
        case '✽':
          str += (string) (object) (char) Convert.ToInt32("5D", 16 /*0x10*/);
          break;
        case '✾':
          str += (string) (object) (char) Convert.ToInt32("5E", 16 /*0x10*/);
          break;
        case '✿':
          str += (string) (object) (char) Convert.ToInt32("5F", 16 /*0x10*/);
          break;
        case '❀':
          str += (string) (object) (char) Convert.ToInt32("60", 16 /*0x10*/);
          break;
        case '❁':
          str += (string) (object) (char) Convert.ToInt32("61", 16 /*0x10*/);
          break;
        case '❂':
          str += (string) (object) (char) Convert.ToInt32("62", 16 /*0x10*/);
          break;
        case '❃':
          str += (string) (object) (char) Convert.ToInt32("63", 16 /*0x10*/);
          break;
        case '❄':
          str += (string) (object) (char) Convert.ToInt32("64", 16 /*0x10*/);
          break;
        case '❅':
          str += (string) (object) (char) Convert.ToInt32("65", 16 /*0x10*/);
          break;
        case '❆':
          str += (string) (object) (char) Convert.ToInt32("66", 16 /*0x10*/);
          break;
        case '❇':
          str += (string) (object) (char) Convert.ToInt32("67", 16 /*0x10*/);
          break;
        case '❈':
          str += (string) (object) (char) Convert.ToInt32("68", 16 /*0x10*/);
          break;
        case '❉':
          str += (string) (object) (char) Convert.ToInt32("69", 16 /*0x10*/);
          break;
        case '❊':
          str += (string) (object) (char) Convert.ToInt32("6A", 16 /*0x10*/);
          break;
        case '❋':
          str += (string) (object) (char) Convert.ToInt32("6B", 16 /*0x10*/);
          break;
        case '❏':
          str += (string) (object) (char) Convert.ToInt32("6F", 16 /*0x10*/);
          break;
        case '❐':
          str += (string) (object) (char) Convert.ToInt32("70", 16 /*0x10*/);
          break;
        case '❑':
          str += (string) (object) (char) Convert.ToInt32("71", 16 /*0x10*/);
          break;
        case '❒':
          str += (string) (object) (char) Convert.ToInt32("72", 16 /*0x10*/);
          break;
        case '❖':
          str += (string) (object) (char) Convert.ToInt32("76", 16 /*0x10*/);
          break;
        case '❘':
          str += (string) (object) (char) Convert.ToInt32("78", 16 /*0x10*/);
          break;
        case '❙':
          str += (string) (object) (char) Convert.ToInt32("79", 16 /*0x10*/);
          break;
        case '❚':
          str += (string) (object) (char) Convert.ToInt32("7A", 16 /*0x10*/);
          break;
        case '❛':
          str += (string) (object) (char) Convert.ToInt32("7B", 16 /*0x10*/);
          break;
        case '❜':
          str += (string) (object) (char) Convert.ToInt32("7C", 16 /*0x10*/);
          break;
        case '❝':
          str += (string) (object) (char) Convert.ToInt32("7D", 16 /*0x10*/);
          break;
        case '❞':
          str += (string) (object) (char) Convert.ToInt32("7E", 16 /*0x10*/);
          break;
        case '❡':
          str += (string) (object) (char) Convert.ToInt32("A1", 16 /*0x10*/);
          break;
        case '❢':
          str += (string) (object) (char) Convert.ToInt32("A2", 16 /*0x10*/);
          break;
        case '❣':
          str += (string) (object) (char) Convert.ToInt32("A3", 16 /*0x10*/);
          break;
        case '❤':
          str += (string) (object) (char) Convert.ToInt32("A4", 16 /*0x10*/);
          break;
        case '❥':
          str += (string) (object) (char) Convert.ToInt32("A5", 16 /*0x10*/);
          break;
        case '❦':
          str += (string) (object) (char) Convert.ToInt32("A6", 16 /*0x10*/);
          break;
        case '❧':
          str += (string) (object) (char) Convert.ToInt32("A7", 16 /*0x10*/);
          break;
        case '\u2776':
          str += (string) (object) (char) Convert.ToInt32("B6", 16 /*0x10*/);
          break;
        case '\u2777':
          str += (string) (object) (char) Convert.ToInt32("B7", 16 /*0x10*/);
          break;
        case '\u2778':
          str += (string) (object) (char) Convert.ToInt32("B8", 16 /*0x10*/);
          break;
        case '\u2779':
          str += (string) (object) (char) Convert.ToInt32("B9", 16 /*0x10*/);
          break;
        case '\u277A':
          str += (string) (object) (char) Convert.ToInt32("BA", 16 /*0x10*/);
          break;
        case '\u277B':
          str += (string) (object) (char) Convert.ToInt32("BB", 16 /*0x10*/);
          break;
        case '\u277C':
          str += (string) (object) (char) Convert.ToInt32("BC", 16 /*0x10*/);
          break;
        case '\u277D':
          str += (string) (object) (char) Convert.ToInt32("BD", 16 /*0x10*/);
          break;
        case '\u277E':
          str += (string) (object) (char) Convert.ToInt32("BE", 16 /*0x10*/);
          break;
        case '\u277F':
          str += (string) (object) (char) Convert.ToInt32("BF", 16 /*0x10*/);
          break;
        case '\u2780':
          str += (string) (object) (char) Convert.ToInt32("C0", 16 /*0x10*/);
          break;
        case '\u2781':
          str += (string) (object) (char) Convert.ToInt32("C1", 16 /*0x10*/);
          break;
        case '\u2782':
          str += (string) (object) (char) Convert.ToInt32("C2", 16 /*0x10*/);
          break;
        case '\u2783':
          str += (string) (object) (char) Convert.ToInt32("C3", 16 /*0x10*/);
          break;
        case '\u2784':
          str += (string) (object) (char) Convert.ToInt32("C4", 16 /*0x10*/);
          break;
        case '\u2785':
          str += (string) (object) (char) Convert.ToInt32("C5", 16 /*0x10*/);
          break;
        case '\u2786':
          str += (string) (object) (char) Convert.ToInt32("C6", 16 /*0x10*/);
          break;
        case '\u2787':
          str += (string) (object) (char) Convert.ToInt32("C7", 16 /*0x10*/);
          break;
        case '\u2788':
          str += (string) (object) (char) Convert.ToInt32("C8", 16 /*0x10*/);
          break;
        case '\u2789':
          str += (string) (object) (char) Convert.ToInt32("C9", 16 /*0x10*/);
          break;
        case '\u278A':
          str += (string) (object) (char) Convert.ToInt32("CA", 16 /*0x10*/);
          break;
        case '\u278B':
          str += (string) (object) (char) Convert.ToInt32("CB", 16 /*0x10*/);
          break;
        case '\u278C':
          str += (string) (object) (char) Convert.ToInt32("CC", 16 /*0x10*/);
          break;
        case '\u278D':
          str += (string) (object) (char) Convert.ToInt32("CD", 16 /*0x10*/);
          break;
        case '\u278E':
          str += (string) (object) (char) Convert.ToInt32("CE", 16 /*0x10*/);
          break;
        case '\u278F':
          str += (string) (object) (char) Convert.ToInt32("CF", 16 /*0x10*/);
          break;
        case '\u2790':
          str += (string) (object) (char) Convert.ToInt32("D0", 16 /*0x10*/);
          break;
        case '\u2791':
          str += (string) (object) (char) Convert.ToInt32("D1", 16 /*0x10*/);
          break;
        case '\u2792':
          str += (string) (object) (char) Convert.ToInt32("D2", 16 /*0x10*/);
          break;
        case '\u2793':
          str += (string) (object) (char) Convert.ToInt32("D3", 16 /*0x10*/);
          break;
        case '➔':
          str += (string) (object) (char) Convert.ToInt32("D4", 16 /*0x10*/);
          break;
        case '➘':
          str += (string) (object) (char) Convert.ToInt32("D8", 16 /*0x10*/);
          break;
        case '➙':
          str += (string) (object) (char) Convert.ToInt32("D9", 16 /*0x10*/);
          break;
        case '➚':
          str += (string) (object) (char) Convert.ToInt32("DA", 16 /*0x10*/);
          break;
        case '➛':
          str += (string) (object) (char) Convert.ToInt32("DB", 16 /*0x10*/);
          break;
        case '➜':
          str += (string) (object) (char) Convert.ToInt32("DC", 16 /*0x10*/);
          break;
        case '➝':
          str += (string) (object) (char) Convert.ToInt32("DD", 16 /*0x10*/);
          break;
        case '➞':
          str += (string) (object) (char) Convert.ToInt32("DE", 16 /*0x10*/);
          break;
        case '➟':
          str += (string) (object) (char) Convert.ToInt32("DF", 16 /*0x10*/);
          break;
        case '➠':
          str += (string) (object) (char) Convert.ToInt32("E0", 16 /*0x10*/);
          break;
        case '➡':
          str += (string) (object) (char) Convert.ToInt32("E1", 16 /*0x10*/);
          break;
        case '➢':
          str += (string) (object) (char) Convert.ToInt32("E2", 16 /*0x10*/);
          break;
        case '➣':
          str += (string) (object) (char) Convert.ToInt32("E3", 16 /*0x10*/);
          break;
        case '➤':
          str += (string) (object) (char) Convert.ToInt32("E4", 16 /*0x10*/);
          break;
        case '➥':
          str += (string) (object) (char) Convert.ToInt32("E5", 16 /*0x10*/);
          break;
        case '➦':
          str += (string) (object) (char) Convert.ToInt32("E6", 16 /*0x10*/);
          break;
        case '➧':
          str += (string) (object) (char) Convert.ToInt32("E7", 16 /*0x10*/);
          break;
        case '➨':
          str += (string) (object) (char) Convert.ToInt32("E8", 16 /*0x10*/);
          break;
        case '➩':
          str += (string) (object) (char) Convert.ToInt32("E9", 16 /*0x10*/);
          break;
        case '➪':
          str += (string) (object) (char) Convert.ToInt32("EA", 16 /*0x10*/);
          break;
        case '➫':
          str += (string) (object) (char) Convert.ToInt32("EB", 16 /*0x10*/);
          break;
        case '➬':
          str += (string) (object) (char) Convert.ToInt32("EC", 16 /*0x10*/);
          break;
        case '➭':
          str += (string) (object) (char) Convert.ToInt32("ED", 16 /*0x10*/);
          break;
        case '➮':
          str += (string) (object) (char) Convert.ToInt32("EE", 16 /*0x10*/);
          break;
        case '➯':
          str += (string) (object) (char) Convert.ToInt32("EF", 16 /*0x10*/);
          break;
        case '➱':
          str += (string) (object) (char) Convert.ToInt32("F1", 16 /*0x10*/);
          break;
        case '➲':
          str += (string) (object) (char) Convert.ToInt32("F2", 16 /*0x10*/);
          break;
        case '➳':
          str += (string) (object) (char) Convert.ToInt32("F3", 16 /*0x10*/);
          break;
        case '➴':
          str += (string) (object) (char) Convert.ToInt32("F4", 16 /*0x10*/);
          break;
        case '➵':
          str += (string) (object) (char) Convert.ToInt32("F5", 16 /*0x10*/);
          break;
        case '➶':
          str += (string) (object) (char) Convert.ToInt32("F6", 16 /*0x10*/);
          break;
        case '➷':
          str += (string) (object) (char) Convert.ToInt32("F7", 16 /*0x10*/);
          break;
        case '➸':
          str += (string) (object) (char) Convert.ToInt32("F8", 16 /*0x10*/);
          break;
        case '➹':
          str += (string) (object) (char) Convert.ToInt32("F9", 16 /*0x10*/);
          break;
        case '➺':
          str += (string) (object) (char) Convert.ToInt32("FA", 16 /*0x10*/);
          break;
        case '➻':
          str += (string) (object) (char) Convert.ToInt32("FB", 16 /*0x10*/);
          break;
        case '➼':
          str += (string) (object) (char) Convert.ToInt32("FC", 16 /*0x10*/);
          break;
        case '➽':
          str += (string) (object) (char) Convert.ToInt32("FD", 16 /*0x10*/);
          break;
        case '➾':
          str += (string) (object) (char) Convert.ToInt32("FE", 16 /*0x10*/);
          break;
        case '⟆':
          str += (string) (object) (char) Convert.ToInt32("75", 16 /*0x10*/);
          break;
        case '\uF8D7':
          str += (string) (object) (char) Convert.ToInt32("80", 16 /*0x10*/);
          break;
        case '\uF8D8':
          str += (string) (object) (char) Convert.ToInt32("81", 16 /*0x10*/);
          break;
        case '\uF8D9':
          str += (string) (object) (char) Convert.ToInt32("82", 16 /*0x10*/);
          break;
        case '\uF8DA':
          str += (string) (object) (char) Convert.ToInt32("83", 16 /*0x10*/);
          break;
        case '\uF8DB':
          str += (string) (object) (char) Convert.ToInt32("84", 16 /*0x10*/);
          break;
        case '\uF8DC':
          str += (string) (object) (char) Convert.ToInt32("85", 16 /*0x10*/);
          break;
        case '\uF8DD':
          str += (string) (object) (char) Convert.ToInt32("86", 16 /*0x10*/);
          break;
        case '\uF8DE':
          str += (string) (object) (char) Convert.ToInt32("87", 16 /*0x10*/);
          break;
        case '\uF8DF':
          str += (string) (object) (char) Convert.ToInt32("88", 16 /*0x10*/);
          break;
        case '\uF8E0':
          str += (string) (object) (char) Convert.ToInt32("89", 16 /*0x10*/);
          break;
        case '\uF8E1':
          str += (string) (object) (char) Convert.ToInt32("8A", 16 /*0x10*/);
          break;
        case '\uF8E2':
          str += (string) (object) (char) Convert.ToInt32("8B", 16 /*0x10*/);
          break;
        case '\uF8E3':
          str += (string) (object) (char) Convert.ToInt32("8C", 16 /*0x10*/);
          break;
        case '\uF8E4':
          str += (string) (object) (char) Convert.ToInt32("8D", 16 /*0x10*/);
          break;
        default:
          str = !structure.ReverseMapTable.ContainsKey(encodedText) ? ((char) Convert.ToInt32("28", 16 /*0x10*/)).ToString() : encodedText;
          break;
      }
    }
    return str;
  }

  private void GetReverseMacEncodeTable()
  {
    this.m_macReverseEncodeTable = new Dictionary<char, int>();
    this.m_macReverseEncodeTable.Add(' ', (int) sbyte.MaxValue);
    this.m_macReverseEncodeTable.Add('Ä', 128 /*0x80*/);
    this.m_macReverseEncodeTable.Add('Å', 129);
    this.m_macReverseEncodeTable.Add('Ç', 130);
    this.m_macReverseEncodeTable.Add('É', 131);
    this.m_macReverseEncodeTable.Add('Ñ', 132);
    this.m_macReverseEncodeTable.Add('Ö', 133);
    this.m_macReverseEncodeTable.Add('Ü', 134);
    this.m_macReverseEncodeTable.Add('á', 135);
    this.m_macReverseEncodeTable.Add('à', 136);
    this.m_macReverseEncodeTable.Add('â', 137);
    this.m_macReverseEncodeTable.Add('ä', 138);
    this.m_macReverseEncodeTable.Add('ã', 139);
    this.m_macReverseEncodeTable.Add('å', 140);
    this.m_macReverseEncodeTable.Add('ç', 141);
    this.m_macReverseEncodeTable.Add('é', 142);
    this.m_macReverseEncodeTable.Add('è', 143);
    this.m_macReverseEncodeTable.Add('ê', 144 /*0x90*/);
    this.m_macReverseEncodeTable.Add('ë', 145);
    this.m_macReverseEncodeTable.Add('í', 146);
    this.m_macReverseEncodeTable.Add('ì', 147);
    this.m_macReverseEncodeTable.Add('î', 148);
    this.m_macReverseEncodeTable.Add('ï', 149);
    this.m_macReverseEncodeTable.Add('ñ', 150);
    this.m_macReverseEncodeTable.Add('ó', 151);
    this.m_macReverseEncodeTable.Add('ò', 152);
    this.m_macReverseEncodeTable.Add('ô', 153);
    this.m_macReverseEncodeTable.Add('ö', 154);
    this.m_macReverseEncodeTable.Add('õ', 155);
    this.m_macReverseEncodeTable.Add('ú', 156);
    this.m_macReverseEncodeTable.Add('ù', 157);
    this.m_macReverseEncodeTable.Add('û', 158);
    this.m_macReverseEncodeTable.Add('ü', 159);
    this.m_macReverseEncodeTable.Add('†', 160 /*0xA0*/);
    this.m_macReverseEncodeTable.Add('°', 161);
    this.m_macReverseEncodeTable.Add('¢', 162);
    this.m_macReverseEncodeTable.Add('£', 163);
    this.m_macReverseEncodeTable.Add('§', 164);
    this.m_macReverseEncodeTable.Add('•', 165);
    this.m_macReverseEncodeTable.Add('¶', 166);
    this.m_macReverseEncodeTable.Add('ß', 167);
    this.m_macReverseEncodeTable.Add('®', 168);
    this.m_macReverseEncodeTable.Add('©', 169);
    this.m_macReverseEncodeTable.Add('™', 170);
    this.m_macReverseEncodeTable.Add('´', 171);
    this.m_macReverseEncodeTable.Add('¨', 172);
    this.m_macReverseEncodeTable.Add('≠', 173);
    this.m_macReverseEncodeTable.Add('Æ', 174);
    this.m_macReverseEncodeTable.Add('Ø', 175);
    this.m_macReverseEncodeTable.Add('∞', 176 /*0xB0*/);
    this.m_macReverseEncodeTable.Add('±', 177);
    this.m_macReverseEncodeTable.Add('≤', 178);
    this.m_macReverseEncodeTable.Add('≥', 179);
    this.m_macReverseEncodeTable.Add('¥', 180);
    this.m_macReverseEncodeTable.Add('µ', 181);
    this.m_macReverseEncodeTable.Add('∂', 182);
    this.m_macReverseEncodeTable.Add('∑', 183);
    this.m_macReverseEncodeTable.Add('∏', 184);
    this.m_macReverseEncodeTable.Add('π', 185);
    this.m_macReverseEncodeTable.Add('∫', 186);
    this.m_macReverseEncodeTable.Add('ª', 187);
    this.m_macReverseEncodeTable.Add('º', 188);
    this.m_macReverseEncodeTable.Add('Ω', 189);
    this.m_macReverseEncodeTable.Add('æ', 190);
    this.m_macReverseEncodeTable.Add('ø', 191);
    this.m_macReverseEncodeTable.Add('¿', 192 /*0xC0*/);
    this.m_macReverseEncodeTable.Add('¡', 193);
    this.m_macReverseEncodeTable.Add('¬', 194);
    this.m_macReverseEncodeTable.Add('√', 195);
    this.m_macReverseEncodeTable.Add('ƒ', 196);
    this.m_macReverseEncodeTable.Add('≈', 197);
    this.m_macReverseEncodeTable.Add('∆', 198);
    this.m_macReverseEncodeTable.Add('«', 199);
    this.m_macReverseEncodeTable.Add('»', 200);
    this.m_macReverseEncodeTable.Add('…', 201);
    this.m_macReverseEncodeTable.Add('À', 203);
    this.m_macReverseEncodeTable.Add('Ã', 204);
    this.m_macReverseEncodeTable.Add('Õ', 205);
    this.m_macReverseEncodeTable.Add('Œ', 206);
    this.m_macReverseEncodeTable.Add('œ', 207);
    this.m_macReverseEncodeTable.Add('–', 208 /*0xD0*/);
    this.m_macReverseEncodeTable.Add('—', 209);
    this.m_macReverseEncodeTable.Add('“', 210);
    this.m_macReverseEncodeTable.Add('”', 211);
    this.m_macReverseEncodeTable.Add('‘', 212);
    this.m_macReverseEncodeTable.Add('’', 213);
    this.m_macReverseEncodeTable.Add('÷', 214);
    this.m_macReverseEncodeTable.Add('◊', 215);
    this.m_macReverseEncodeTable.Add('ÿ', 216);
    this.m_macReverseEncodeTable.Add('Ÿ', 217);
    this.m_macReverseEncodeTable.Add('⁄', 218);
    this.m_macReverseEncodeTable.Add('€', 219);
    this.m_macReverseEncodeTable.Add('‹', 220);
    this.m_macReverseEncodeTable.Add('›', 221);
    this.m_macReverseEncodeTable.Add('ﬁ', 222);
    this.m_macReverseEncodeTable.Add('ﬂ', 223);
    this.m_macReverseEncodeTable.Add('‡', 224 /*0xE0*/);
    this.m_macReverseEncodeTable.Add('·', 225);
    this.m_macReverseEncodeTable.Add(',', 226);
    this.m_macReverseEncodeTable.Add('„', 227);
    this.m_macReverseEncodeTable.Add('‰', 228);
    this.m_macReverseEncodeTable.Add('Â', 229);
    this.m_macReverseEncodeTable.Add('Ê', 230);
    this.m_macReverseEncodeTable.Add('Á', 231);
    this.m_macReverseEncodeTable.Add('Ë', 232);
    this.m_macReverseEncodeTable.Add('È', 233);
    this.m_macReverseEncodeTable.Add('Í', 234);
    this.m_macReverseEncodeTable.Add('Î', 235);
    this.m_macReverseEncodeTable.Add('Ï', 236);
    this.m_macReverseEncodeTable.Add('Ì', 237);
    this.m_macReverseEncodeTable.Add('Ó', 238);
    this.m_macReverseEncodeTable.Add('Ô', 239);
    this.m_macReverseEncodeTable.Add('\uF8FF', 240 /*0xF0*/);
    this.m_macReverseEncodeTable.Add('Ò', 241);
    this.m_macReverseEncodeTable.Add('Ú', 242);
    this.m_macReverseEncodeTable.Add('Û', 243);
    this.m_macReverseEncodeTable.Add('Ù', 244);
    this.m_macReverseEncodeTable.Add('ı', 245);
    this.m_macReverseEncodeTable.Add('ˆ', 246);
    this.m_macReverseEncodeTable.Add('˜', 247);
    this.m_macReverseEncodeTable.Add('¯', 248);
    this.m_macReverseEncodeTable.Add('˘', 249);
    this.m_macReverseEncodeTable.Add('˙', 250);
    this.m_macReverseEncodeTable.Add('˚', 251);
    this.m_macReverseEncodeTable.Add('¸', 252);
    this.m_macReverseEncodeTable.Add('˝', 253);
    this.m_macReverseEncodeTable.Add('˛', 254);
    this.m_macReverseEncodeTable.Add('ˇ', (int) byte.MaxValue);
  }

  private double GetKey(Dictionary<double, string> charMapTable, string val)
  {
    double num = -1.0;
    foreach (KeyValuePair<double, string> keyValuePair in charMapTable)
    {
      if (val == keyValuePair.Value)
      {
        num = keyValuePair.Key;
        break;
      }
    }
    return num == -1.0 ? (double) val[0] : num;
  }

  private double GetKey(Dictionary<double, string> charMapTable)
  {
    string str = this.text.Substring(1, this.text.Length - 2);
    string[] array = charMapTable.Values.ToArray<string>();
    double key = -1.0;
    for (int index = 0; index < array.Length; ++index)
    {
      if (str == array[index])
        key = charMapTable.Keys.ToArray<double>()[index];
    }
    return key;
  }

  private PdfString GetUnicodeString(string token)
  {
    return token != null ? new PdfString(token)
    {
      Converted = true,
      Encode = PdfString.ForceEncoding.ASCII
    } : throw new ArgumentNullException(nameof (token));
  }

  private float GetReplacedChar(Glyph[] glyphs, FontStructure structure)
  {
    float replacedChar = 0.0f;
    if (structure.FontGlyphWidths != null && structure.FontGlyphWidths.Count == 0)
    {
      if (Enum.IsDefined(typeof (PdfFontFamily), (object) structure.FontName))
      {
        PdfFontMetrics metrics = PdfStandardFontMetricsFactory.GetMetrics(this.GetFamily(structure.FontName), (PdfFontStyle) structure.FontStyle, structure.FontSize);
        foreach (Glyph glyph in glyphs)
        {
          int num1 = !(structure.FontName == "ZapfDingbats") || structure.isEmbedded ? glyph.CharId.IntValue - 32 /*0x20*/ : (int) this.ReverseMapZapf(((char) glyph.CharId.IntValue).ToString(), structure)[0] - 32 /*0x20*/;
          int index = num1 < 0 || num1 == 128 /*0x80*/ ? 0 : num1;
          if (index < metrics.WidthTable.ToArray().Count)
          {
            float num2 = (float) metrics.WidthTable[index];
            float num3 = (float) (glyph.Width * glyph.FontSize);
            replacedChar += (float) ((double) num2 + (double) num2 / (double) num3 * glyph.CharSpacing + (double) num2 / (double) num3 * glyph.WordSpacing);
          }
        }
      }
      else if (structure.FontDictionary != null && structure.FontDictionary.ContainsKey("BaseFont"))
      {
        string name = (structure.FontDictionary["BaseFont"] as PdfName).Value;
        if (name.Contains("-"))
          name = name.Replace("-", "");
        if (name.Contains(","))
          name = name.Replace(",", "");
        if (Enum.IsDefined(typeof (PdfFontFamily), (object) name))
        {
          PdfFontMetrics metrics = PdfStandardFontMetricsFactory.GetMetrics(this.GetFamily(name), (PdfFontStyle) structure.FontStyle, structure.FontSize);
          foreach (Glyph glyph in glyphs)
          {
            int num4 = glyph.CharId.IntValue - 32 /*0x20*/;
            int index = num4 < 0 || num4 == 128 /*0x80*/ ? 0 : num4;
            if (index < metrics.WidthTable.ToArray().Count)
            {
              float num5 = (float) metrics.WidthTable[index];
              float num6 = (float) (glyph.Width * glyph.FontSize);
              replacedChar += (float) ((double) num5 + (double) num5 / (double) num6 * glyph.CharSpacing + (double) num5 / (double) num6 * glyph.WordSpacing);
            }
          }
        }
      }
    }
    else if (structure.FontGlyphWidths != null && structure.FontGlyphWidths.Count > 0)
    {
      foreach (Glyph glyph in glyphs)
      {
        if (structure.FontEncoding == "Identity-H" || structure.FontEncoding == "" && structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
        {
          double key = -1.0;
          if (structure.CharacterMapTable.ContainsValue(this.text.Substring(1, this.text.Length - 2)))
            key = this.GetKey(structure.CharacterMapTable);
          if (key == -1.0)
            key = glyph.ToUnicode[0] == char.MinValue || glyph.CharId.IntValue != 0 ? this.GetKey(structure.CharacterMapTable, ((char) glyph.CharId.IntValue).ToString()) : this.GetKey(structure.CharacterMapTable, glyph.ToUnicode[0].ToString());
          int num7;
          structure.FontGlyphWidths.TryGetValue((int) key, out num7);
          float num8 = (float) (glyph.Width * glyph.FontSize);
          replacedChar += (float) ((double) num7 + (double) num7 / (double) num8 * glyph.CharSpacing + (double) num7 / (double) num8 * glyph.WordSpacing);
        }
        else
        {
          int num9 = 0;
          CharCode charId1 = glyph.CharId;
          if (glyph.CharId.BytesCount != 0)
          {
            num9 = glyph.CharId.IntValue;
          }
          else
          {
            CharCode charId2 = glyph.CharId;
            if (glyph.CharId.Bytes != null || glyph.ToUnicode != null && structure.FontEncoding == "WinAnsiEncoding")
              num9 = (int) glyph.ToUnicode[0];
          }
          if (structure.FontGlyphWidths.ContainsKey(num9))
          {
            float fontGlyphWidth = (float) structure.FontGlyphWidths[num9];
            float num10 = (float) (glyph.Width * glyph.FontSize);
            replacedChar += (float) ((double) fontGlyphWidth + (double) fontGlyphWidth / (double) num10 * glyph.CharSpacing + (double) fontGlyphWidth / (double) num10 * glyph.WordSpacing);
          }
          else if (structure.FontEncoding == "Encoding")
          {
            int itFormDifference = this.GetItFormDifference(structure.DifferencesDictionary, (char) num9);
            if (structure.FontGlyphWidths.ContainsKey(itFormDifference))
            {
              float fontGlyphWidth = (float) structure.FontGlyphWidths[itFormDifference];
              float num11 = (float) (glyph.Width * glyph.FontSize);
              replacedChar += (float) ((double) fontGlyphWidth + (double) fontGlyphWidth / (double) num11 * glyph.CharSpacing + (double) fontGlyphWidth / (double) num11 * glyph.WordSpacing);
            }
          }
        }
      }
    }
    return replacedChar;
  }

  private int GetItFormDifference(Dictionary<string, string> diffTable, char val)
  {
    int result = 0;
    foreach (KeyValuePair<string, string> keyValuePair in diffTable)
    {
      if (keyValuePair.Value == val.ToString())
      {
        int.TryParse(keyValuePair.Key, out result);
        break;
      }
    }
    return result;
  }

  private PdfFontFamily GetFamily(string name)
  {
    switch (name)
    {
      case "Helvetica":
        return PdfFontFamily.Helvetica;
      case "Courier":
        return PdfFontFamily.Courier;
      case "TimesRoman":
        return PdfFontFamily.TimesRoman;
      case "Symbol":
        return PdfFontFamily.Symbol;
      case "ZapfDingbats":
        return PdfFontFamily.ZapfDingbats;
      default:
        return PdfFontFamily.Helvetica;
    }
  }
}

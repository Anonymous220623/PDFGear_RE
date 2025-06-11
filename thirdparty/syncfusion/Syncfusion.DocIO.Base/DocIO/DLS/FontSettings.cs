// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FontSettings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class FontSettings
{
  internal Dictionary<string, Stream> FontStreams = new Dictionary<string, Stream>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
  internal Dictionary<string, string> FontNames = new Dictionary<string, string>();
  internal PrivateFontCollection PrivateFonts = new PrivateFontCollection();
  internal Dictionary<string, Font> FontCollection = new Dictionary<string, Font>();

  public event SubstituteFontEventHandler SubstituteFont;

  internal Font GetFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    string key = $"{fontName};{(object) fontStyle};{(object) fontSize}";
    Font font;
    if (this.FontCollection.ContainsKey(key))
    {
      font = this.FontCollection[key];
    }
    else
    {
      font = this.CreateFont(fontName, fontSize, fontStyle);
      if (fontName.Equals(font.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase))
        this.FontCollection.Add(key, font);
    }
    if (this.SubstituteFont != null && (font.Name.ToLower() != fontName.ToLower() || font.Style != fontStyle))
    {
      SubstituteFontEventArgs args = new SubstituteFontEventArgs(fontName, "Microsoft Sans Serif", fontStyle);
      while (this.SubstituteFont != null)
      {
        string alternateFontName = args.AlternateFontName;
        if (!this.FontStreams.ContainsKey($"{fontName.ToLower()}_{fontStyle.ToString().ToLower()}"))
          this.SubstituteFont((object) this, args);
        if (args.AlternateFontStream != null && args.AlternateFontStream.Length > 0L)
        {
          Stream alternateFontStream = args.AlternateFontStream;
          if (!this.FontStreams.ContainsKey($"{fontName.ToLower()}_{fontStyle.ToString().ToLower()}"))
          {
            alternateFontStream.Position = 0L;
            byte[] numArray = new byte[alternateFontStream.Length];
            alternateFontStream.Position = 0L;
            alternateFontStream.Read(numArray, 0, (int) alternateFontStream.Length);
            IntPtr num = Marshal.AllocCoTaskMem((int) alternateFontStream.Length);
            Marshal.Copy(numArray, 0, num, (int) alternateFontStream.Length);
            this.PrivateFonts.AddMemoryFont(num, (int) alternateFontStream.Length);
            alternateFontStream.Position = 0L;
            this.FontStreams.Add($"{args.OriginalFontName.ToLower()}_{args.FontStyle.ToString().ToLower()}", alternateFontStream);
          }
          Font privateFont = this.GetPrivateFont(fontName, fontSize, fontStyle);
          if (privateFont.Name != fontName)
            return this.GetPrivateFont(args.AlternateFontName, fontSize, fontStyle);
          if (!this.FontCollection.ContainsKey(key))
            this.FontCollection.Add(key, privateFont);
          return font;
        }
        font = this.CreateFont(args.AlternateFontName, fontSize, fontStyle);
        if (font.Name == args.AlternateFontName || alternateFontName != "Microsoft Sans Serif" || alternateFontName == args.AlternateFontName)
          break;
      }
    }
    else if (!font.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase) && (this.PrivateFonts.Families.Length > 0 || this.FontNames.ContainsKey(fontName)))
      return this.GetPrivateFont(fontName, fontSize, fontStyle);
    return font;
  }

  private Font GetPrivateFont(string fontName, float fontsize, FontStyle style)
  {
    if (this.FontNames.ContainsKey(fontName))
      fontName = this.FontNames[fontName];
    foreach (FontFamily family in this.PrivateFonts.Families)
    {
      if (family.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
        return new Font(family, fontsize, style);
    }
    return new Font(fontName, fontsize, style);
  }

  private Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    try
    {
      Font font = new Font(fontName, fontSize, fontStyle);
      if (!fontName.Equals(font.Name, StringComparison.OrdinalIgnoreCase) && this.FontStreams.ContainsKey($"{fontName}_{fontStyle.ToString().ToLower()}"))
        font = this.GetPrivateFont(fontName, fontSize, fontStyle);
      return font;
    }
    catch
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      return new Font(fontName, fontSize, fontStyle);
    }
  }

  internal void Close()
  {
    if (this.FontCollection != null)
    {
      foreach (string key in this.FontCollection.Keys)
        this.FontCollection[key]?.Dispose();
      this.FontCollection.Clear();
      this.FontCollection = (Dictionary<string, Font>) null;
    }
    if (this.FontNames != null)
    {
      this.FontNames.Clear();
      this.FontNames = (Dictionary<string, string>) null;
    }
    if (this.PrivateFonts != null)
    {
      this.PrivateFonts.Dispose();
      this.PrivateFonts = (PrivateFontCollection) null;
    }
    if (this.FontStreams != null)
    {
      foreach (string key in this.FontStreams.Keys)
        this.FontStreams[key]?.Dispose();
      this.FontStreams.Clear();
      this.FontStreams = (Dictionary<string, Stream>) null;
    }
    if (this.SubstituteFont == null)
      return;
    this.SubstituteFont = (SubstituteFontEventHandler) null;
  }
}

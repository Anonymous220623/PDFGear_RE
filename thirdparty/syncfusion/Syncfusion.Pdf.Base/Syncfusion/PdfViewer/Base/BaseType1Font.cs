// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.BaseType1Font
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class BaseType1Font : PostScriptObj, IBuildCharacterOwner
{
  private readonly CharacterBuilder buildChar;
  private readonly Dictionary<string, GlyphInfo> glyphOutlines;
  private readonly KeyProperty<PostScriptArray> fontMatrix;
  private readonly KeyProperty<FontData> fontInfo;
  private readonly KeyProperty<PostScriptArray> encoding;
  private readonly KeyProperty<PostScriptDict> charStrings;
  private readonly KeyProperty<KeyPrivate> priv;
  private object SyncObj = new object();

  public PostScriptArray FontMatrix => this.fontMatrix.GetValue();

  public FontData FontInfo => this.fontInfo.GetValue();

  public PostScriptArray Encoding => this.encoding.GetValue();

  public PostScriptDict CharStrings => this.charStrings.GetValue();

  public KeyPrivate Private => this.priv.GetValue();

  public BaseType1Font()
  {
    this.buildChar = new CharacterBuilder((IBuildCharacterOwner) this);
    this.glyphOutlines = new Dictionary<string, GlyphInfo>();
    this.fontMatrix = this.CreateProperty<PostScriptArray>(new KeyPropertyDescriptor()
    {
      Name = nameof (FontMatrix)
    }, PostScriptArray.MatrixIdentity);
    this.fontInfo = this.CreateProperty<FontData>(new KeyPropertyDescriptor()
    {
      Name = nameof (FontInfo)
    }, new FontData());
    this.encoding = this.CreateProperty<PostScriptArray>(new KeyPropertyDescriptor()
    {
      Name = nameof (Encoding)
    }, (IConverter) Type1Converters.EncodingConverter, PresettedEncoding.StandardEncoding.ToArray());
    this.charStrings = this.CreateProperty<PostScriptDict>(new KeyPropertyDescriptor()
    {
      Name = nameof (CharStrings)
    });
    this.priv = this.CreateProperty<KeyPrivate>(new KeyPropertyDescriptor()
    {
      Name = nameof (Private)
    }, (IConverter) Type1Converters.PostScriptObjectConverter);
  }

  public ushort GetAdvancedWidth(Glyph glyph) => this.GetGlyphData(glyph.Name).AdvancedWidth;

  public void GetGlyphOutlines(Glyph glyph, double fontSize)
  {
    Monitor.Enter(this.SyncObj);
    try
    {
      GlyphOutlinesCollection outlinesCollection = this.GetGlyphData(glyph.Name).Oultlines.Clone();
      Matrix matrix = this.FontMatrix.ToMatrix();
      matrix.ScaleAppend(fontSize, -fontSize, 0.0, 0.0);
      outlinesCollection.Transform(matrix);
      glyph.Outlines = outlinesCollection;
    }
    finally
    {
      Monitor.Exit(this.SyncObj);
    }
  }

  public GlyphInfo GetGlyphData(string name)
  {
    GlyphInfo glyphData;
    if (!this.glyphOutlines.TryGetValue(name, out glyphData))
    {
      glyphData = this.ReadGlyphData(name);
      this.glyphOutlines[name] = glyphData;
    }
    return glyphData;
  }

  public byte[] GetSubr(int index) => this.Private.GetSubr(index);

  public byte[] GetGlobalSubr(int index) => throw new NotImplementedException();

  internal string GetGlyphName(ushort cid)
  {
    return this.Encoding == null ? ".notdef" : this.Encoding.GetElementAs<string>((int) cid);
  }

  private GlyphInfo ReadGlyphData(string name)
  {
    PostScriptStrHelper elementAs = this.CharStrings.GetElementAs<PostScriptStrHelper>(name);
    if (elementAs == null)
      return new GlyphInfo(new GlyphOutlinesCollection(), new ushort?((ushort) 0));
    this.buildChar.Execute(elementAs.ToByteArray());
    GlyphOutlinesCollection glyphOutlines = this.buildChar.GlyphOutlines;
    int? width = this.buildChar.Width;
    return new GlyphInfo(glyphOutlines, width.HasValue ? new ushort?((ushort) width.GetValueOrDefault()) : new ushort?());
  }
}

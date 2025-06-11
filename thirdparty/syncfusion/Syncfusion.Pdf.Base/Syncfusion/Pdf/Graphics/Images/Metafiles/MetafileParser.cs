// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.MetafileParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal abstract class MetafileParser
{
  protected const byte PointNumber = 2;
  protected const byte RectNumber = 4;
  protected static readonly int IntSize = Marshal.SizeOf(typeof (int));
  protected static readonly int ShortSize = Marshal.SizeOf(typeof (short));
  protected static readonly int FloatSize = Marshal.SizeOf(typeof (float));
  private System.Drawing.Graphics.EnumerateMetafileProc m_enumerateHandler;
  private PdfEmfRenderer m_renderer;
  private object m_context;
  private object m_imageContext;
  private Metafile m_metaFile;
  private float m_pageScale;
  private GraphicsUnit m_pageUnit;

  public MetafileParser()
  {
  }

  public MetafileParser(PdfEmfRenderer renderer)
  {
    this.m_renderer = renderer != null ? renderer : throw new ArgumentNullException(nameof (renderer));
  }

  public virtual void Dispose()
  {
    this.m_enumerateHandler = (System.Drawing.Graphics.EnumerateMetafileProc) null;
    this.m_renderer = (PdfEmfRenderer) null;
  }

  public System.Drawing.Graphics.EnumerateMetafileProc ParsingHandler
  {
    get
    {
      if (this.m_enumerateHandler == null)
        this.m_enumerateHandler = this.CreateParsingHandler();
      return this.m_enumerateHandler;
    }
  }

  public PdfEmfRenderer Renderer
  {
    get => this.m_renderer;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Renderer));
      if (this.m_renderer == value)
        return;
      this.m_renderer = value;
    }
  }

  public object Context
  {
    get => this.m_context;
    set
    {
      if (this.m_context == value)
        return;
      this.m_context = value;
    }
  }

  public object ImageContext
  {
    get => this.m_imageContext;
    set
    {
      if (this.m_imageContext == value)
        return;
      this.m_imageContext = value;
    }
  }

  public Metafile Metafile
  {
    get => this.m_metaFile;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Metafile));
      if (this.m_metaFile == value)
        return;
      this.m_metaFile = value;
    }
  }

  public float PageScale
  {
    get => this.m_pageScale;
    set => this.m_pageScale = value;
  }

  public GraphicsUnit PageUnit
  {
    get => this.m_pageUnit;
    set => this.m_pageUnit = value;
  }

  protected internal static void CheckResult(bool result)
  {
    if (result)
      return;
    int lastError = (int) KernelApi.GetLastError();
  }

  public abstract MetafileType Type { get; }

  protected abstract System.Drawing.Graphics.EnumerateMetafileProc CreateParsingHandler();

  protected float ReadNumber(byte[] data, int index, int step)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    float num = 0.0f;
    if (step == MetafileParser.ShortSize)
      num = (float) BitConverter.ToInt16(data, index);
    else if (step == MetafileParser.FloatSize)
    {
      num = BitConverter.ToSingle(data, index);
      if ((double) num != (double) num)
      {
        Array.Reverse((Array) data, index * 4, 4);
        num = BitConverter.ToSingle(data, index * 4);
      }
    }
    return num;
  }
}

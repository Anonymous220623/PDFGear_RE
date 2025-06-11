// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TableBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class TableBase
{
  private readonly FontFile2 fontSource;
  private int m_offset;

  internal abstract int Id { get; }

  public int Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  protected ReadFontArray Reader => this.fontSource.FontArrayReader;

  protected FontFile2 FontSource => this.fontSource;

  public TableBase(FontFile2 fontSource) => this.fontSource = fontSource;

  public TableBase()
  {
  }

  public abstract void Read(ReadFontArray reader);
}

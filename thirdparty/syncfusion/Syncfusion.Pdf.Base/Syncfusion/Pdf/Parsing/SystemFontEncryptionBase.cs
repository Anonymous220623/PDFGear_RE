// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEncryptionBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontEncryptionBase
{
  private const ushort C1 = 52845;
  private const ushort C2 = 22719;
  private readonly int randomBytesCount;
  private readonly SystemFontPostScriptReader reader;
  private ushort r;

  public int RandomBytesCount => this.randomBytesCount;

  protected SystemFontPostScriptReader Reader => this.reader;

  public SystemFontEncryptionBase(SystemFontPostScriptReader reader, ushort r, int n)
  {
    this.reader = reader;
    this.r = r;
    this.randomBytesCount = n;
  }

  public abstract void Initialize();

  public byte Decrypt(byte cipher)
  {
    byte num = (byte) ((uint) cipher ^ (uint) this.r >> 8);
    this.r = (ushort) (((int) cipher + (int) this.r) * 52845 + 22719);
    return num;
  }
}

// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEncryptionCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontEncryptionCollection
{
  private readonly List<SystemFontEncryptionBase> encryptionCollection;

  public bool HasEncryption => this.encryptionCollection.Count != 0;

  public SystemFontEncryptionCollection()
  {
    this.encryptionCollection = new List<SystemFontEncryptionBase>();
  }

  public void PushEncryption(SystemFontEncryptionBase encryption)
  {
    this.encryptionCollection.Add(encryption);
  }

  public void PopEncryption()
  {
    this.encryptionCollection.Remove(SystemFontEnumerable.Last<SystemFontEncryptionBase>((IEnumerable<SystemFontEncryptionBase>) this.encryptionCollection));
  }

  public byte Decrypt(byte b)
  {
    byte cipher = this.encryptionCollection[0].Decrypt(b);
    for (int index = 1; index < this.encryptionCollection.Count; ++index)
      cipher = this.encryptionCollection[index].Decrypt(cipher);
    return cipher;
  }
}

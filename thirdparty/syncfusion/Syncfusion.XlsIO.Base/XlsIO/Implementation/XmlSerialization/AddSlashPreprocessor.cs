// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.AddSlashPreprocessor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression.Zip;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class AddSlashPreprocessor : IFileNamePreprocessor
{
  public string PreprocessName(string fullName)
  {
    if (fullName != null && fullName.Length > 0 && fullName[0] != '/')
      fullName = '/'.ToString() + fullName;
    return fullName;
  }
}

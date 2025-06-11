// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.AddSlashPreprocessor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression.Zip;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class AddSlashPreprocessor : IFileNamePreprocessor
{
  public string PreprocessName(string fullName)
  {
    if (fullName != null && fullName.Length > 0 && fullName[0] != '/')
      fullName = '/'.ToString() + fullName;
    return fullName;
  }
}

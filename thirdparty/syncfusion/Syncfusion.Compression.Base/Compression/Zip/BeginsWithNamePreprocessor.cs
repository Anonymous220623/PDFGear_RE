// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.BeginsWithNamePreprocessor
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Compression.Zip;

public class BeginsWithNamePreprocessor : IFileNamePreprocessor
{
  private string m_strStartToRemove;

  public BeginsWithNamePreprocessor(string startToRemove)
  {
    this.m_strStartToRemove = startToRemove;
  }

  public string PreprocessName(string fullName)
  {
    string str = fullName;
    if (this.m_strStartToRemove != null && fullName.StartsWith(this.m_strStartToRemove))
      str = fullName.Remove(0, this.m_strStartToRemove.Length);
    return str;
  }
}

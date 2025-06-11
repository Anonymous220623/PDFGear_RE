// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaAttribute
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class VbaAttribute
{
  private string m_name;
  private string m_value;
  private bool m_isText = true;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal bool IsText
  {
    get => this.m_isText;
    set => this.m_isText = value;
  }

  internal VbaAttribute Clone() => (VbaAttribute) this.MemberwiseClone();
}

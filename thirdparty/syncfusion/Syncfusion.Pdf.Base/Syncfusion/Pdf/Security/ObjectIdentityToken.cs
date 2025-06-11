// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ObjectIdentityToken
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ObjectIdentityToken
{
  private string m_id;
  private int m_index;

  internal ObjectIdentityToken(string id) => this.m_id = id;

  internal bool HasMoreTokens => this.m_index != -1;

  internal string NextToken()
  {
    if (this.m_index == -1)
      return (string) null;
    int num = this.m_id.IndexOf('.', this.m_index);
    if (num == -1)
    {
      string str = this.m_id.Substring(this.m_index);
      this.m_index = -1;
      return str;
    }
    string str1 = this.m_id.Substring(this.m_index, num - this.m_index);
    this.m_index = num + 1;
    return str1;
  }
}

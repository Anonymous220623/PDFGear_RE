// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignerInformationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignerInformationCollection
{
  private readonly ICollection all;
  private readonly IDictionary table = (IDictionary) new Dictionary<SignerId, List<CmsSignerDetails>>();

  internal SignerInformationCollection(ICollection signerInfos)
  {
    foreach (CmsSignerDetails signerInfo in (IEnumerable) signerInfos)
    {
      SignerId id = signerInfo.ID;
      IList list = (IList) this.table[(object) id];
      if (list == null)
        this.table[(object) id] = (object) (List<CmsSignerDetails>) (list = (IList) new List<CmsSignerDetails>(1));
      list.Add((object) signerInfo);
    }
    this.all = signerInfos;
  }

  internal ICollection GetSigners() => this.all;
}

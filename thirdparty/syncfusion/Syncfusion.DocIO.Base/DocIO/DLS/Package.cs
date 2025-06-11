// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Package
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Package : PartContainer
{
  internal PartContainer FindPartContainer(string containerName)
  {
    return this.EnsurePartContainer(containerName.Split('/'), 0);
  }

  internal Part FindPart(string fullPartName)
  {
    int num = fullPartName.LastIndexOf("/");
    PartContainer partContainer = this.FindPartContainer(fullPartName.Substring(0, num + 1));
    if (partContainer != null)
    {
      string key = fullPartName.Substring(num + 1, fullPartName.Length - (num + 1));
      if (partContainer.XmlParts.ContainsKey(key))
        return partContainer.XmlParts[key];
    }
    return (Part) null;
  }

  internal void Load(ZipArchive zipArc)
  {
    int index = 0;
    for (int count = zipArc.Count; index < count; ++index)
      this.LoadPart(zipArc[index]);
  }

  private void LoadPart(ZipArchiveItem item)
  {
    string[] nameParts = item.ItemName.Split('/');
    string str = nameParts[nameParts.Length - 1];
    PartContainer partContainer = this.EnsurePartContainer(nameParts, 0);
    if (nameParts.Length > 1 && nameParts[nameParts.Length - 2].EndsWith("_rels"))
      partContainer.LoadRelations(item);
    else
      partContainer.AddPart(item);
  }

  internal Package Clone()
  {
    Package package = new Package();
    package.Name = this.m_name;
    if (this.m_xmlParts != null && this.m_xmlParts.Count > 0)
    {
      foreach (string key in this.m_xmlParts.Keys)
        package.XmlParts.Add(key, this.m_xmlParts[key].Clone());
    }
    if (this.m_relations != null && this.m_relations.Count > 0)
    {
      foreach (string key in this.m_relations.Keys)
        package.Relations.Add(key, this.m_relations[key].Clone() as Syncfusion.DocIO.DLS.Relations);
    }
    if (this.m_xmlPartContainers != null && this.m_xmlPartContainers.Count > 0)
    {
      foreach (string key in this.m_xmlPartContainers.Keys)
        package.XmlPartContainers.Add(key, this.m_xmlPartContainers[key].Clone());
    }
    return package;
  }

  internal override void Close() => base.Close();
}

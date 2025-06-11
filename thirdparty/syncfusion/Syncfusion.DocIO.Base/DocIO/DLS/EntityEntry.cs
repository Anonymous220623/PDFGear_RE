// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EntityEntry
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class EntityEntry
{
  public Entity Current;
  public int Index;

  public EntityEntry(Entity ent)
  {
    this.Current = ent;
    this.Index = 0;
  }

  public bool Fetch()
  {
    if (this.Current != null && this.Current.Owner != null && this.Current.Owner.IsComposite)
    {
      ICompositeEntity owner = this.Current.Owner as ICompositeEntity;
      if (owner.ChildEntities.Count > this.Index + 1)
      {
        ++this.Index;
        this.Current = owner.ChildEntities[this.Index];
        return true;
      }
    }
    this.Current = (Entity) null;
    this.Index = -1;
    return false;
  }
}

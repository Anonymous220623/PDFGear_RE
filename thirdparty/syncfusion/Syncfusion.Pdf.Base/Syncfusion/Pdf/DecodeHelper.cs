// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DecodeHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class DecodeHelper
{
  public ModuleSpec iccs;
  public MaxShiftSpec rois;
  public QuantTypeSpec qts;
  public QuantStepSizeSpec qsss;
  internal GuardBitsSpec gbs;
  public SynWTFilterSpec wfs;
  public IntegerSpec dls;
  public IntegerSpec nls;
  public IntegerSpec pos;
  public ModuleSpec ecopts;
  public CompTransfSpec cts;
  public ModuleSpec pcs;
  public ModuleSpec ers;
  public PrecinctSizeSpec pss;
  public ModuleSpec sops;
  public ModuleSpec ephs;
  public CBlkSizeSpec cblks;
  public ModuleSpec pphs;

  public virtual DecodeHelper Copy
  {
    get
    {
      DecodeHelper copy = (DecodeHelper) null;
      try
      {
        copy = (DecodeHelper) this.Clone();
      }
      catch (Exception ex)
      {
      }
      copy.qts = (QuantTypeSpec) this.qts.Copy;
      copy.qsss = (QuantStepSizeSpec) this.qsss.Copy;
      copy.gbs = (GuardBitsSpec) this.gbs.Copy;
      copy.wfs = (SynWTFilterSpec) this.wfs.Copy;
      copy.dls = (IntegerSpec) this.dls.Copy;
      copy.cts = (CompTransfSpec) this.cts.Copy;
      if (this.rois != null)
        copy.rois = (MaxShiftSpec) this.rois.Copy;
      return copy;
    }
  }

  public DecodeHelper(int nt, int nc)
  {
    this.qts = new QuantTypeSpec(nt, nc, (byte) 2);
    this.qsss = new QuantStepSizeSpec(nt, nc, (byte) 2);
    this.gbs = new GuardBitsSpec(nt, nc, (byte) 2);
    this.wfs = new SynWTFilterSpec(nt, nc, (byte) 2);
    this.dls = new IntegerSpec(nt, nc, (byte) 2);
    this.cts = new CompTransfSpec(nt, nc, (byte) 2);
    this.ecopts = new ModuleSpec(nt, nc, (byte) 2);
    this.ers = new ModuleSpec(nt, nc, (byte) 2);
    this.cblks = new CBlkSizeSpec(nt, nc, (byte) 2);
    this.pss = new PrecinctSizeSpec(nt, nc, (byte) 2, this.dls);
    this.nls = new IntegerSpec(nt, nc, (byte) 1);
    this.pos = new IntegerSpec(nt, nc, (byte) 1);
    this.pcs = new ModuleSpec(nt, nc, (byte) 1);
    this.sops = new ModuleSpec(nt, nc, (byte) 1);
    this.ephs = new ModuleSpec(nt, nc, (byte) 1);
    this.pphs = new ModuleSpec(nt, nc, (byte) 1);
    this.iccs = new ModuleSpec(nt, nc, (byte) 1);
    this.pphs.setDefault((object) false);
  }

  public virtual object Clone() => (object) null;
}

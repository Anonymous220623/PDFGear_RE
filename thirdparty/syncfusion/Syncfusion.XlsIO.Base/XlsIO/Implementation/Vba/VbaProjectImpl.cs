// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Vba.VbaProjectImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.Office;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Vba;

internal class VbaProjectImpl : VbaProject
{
  internal WorkbookImpl Workbook => this.m_parent as WorkbookImpl;

  internal VbaProjectImpl(WorkbookImpl book)
    : base((object) book)
  {
  }

  internal VbaProjectImpl(WorkbookImpl book, ICompoundStorage rootStorage)
    : base((object) book)
  {
    this.ParseMacroStream(rootStorage);
  }

  private void ParseMacroStream(ICompoundStorage rootStorage)
  {
    using (ICompoundStorage storage = rootStorage.OpenStorage("VBA"))
    {
      this.ParseDirStream(storage);
      this.ParseModuleStream(storage);
      this.ParseProjectStream(rootStorage);
      this.ParseDesignerStream(rootStorage);
    }
  }

  private void ParseDirStream(ICompoundStorage storage)
  {
    using (Stream dirStream = (Stream) storage.OpenStream("dir"))
      this.ParseDirStream(dirStream);
  }

  private void ParseModuleStream(ICompoundStorage storage)
  {
    foreach (VbaModule module in (IEnumerable) this.Modules)
    {
      using (CompoundStream dirStream = storage.OpenStream(module.StreamName))
        this.ParseModuleStream((Stream) dirStream, module);
    }
  }

  private void ParseProjectStream(ICompoundStorage rootStorage)
  {
    using (CompoundStream compoundStream = rootStorage.OpenStream("Project"))
      this.ParseProjectStream((Stream) compoundStream);
  }

  private void ParseDesignerStream(ICompoundStorage rootStorage)
  {
    foreach (VbaModule module in (IEnumerable) this.Modules)
    {
      if (module.Type == VbaModuleType.MsForm)
        module.DesignerStorage = (object) rootStorage.OpenStorage(module.StreamName);
    }
  }

  internal void Save(ICompoundStorage vbaRootStorage) => this.SerializeVbaProject(vbaRootStorage);

  private void SerializeVbaProject(ICompoundStorage vbaRootStorage)
  {
    ICompoundStorage storage = vbaRootStorage.CreateStorage("VBA");
    this.SerializeVbaStream(storage);
    this.SerializeDirStream(storage);
    this.SerializeModuleStream(storage);
    this.SerializeProjectStream(vbaRootStorage);
    this.SerializeProjectWmStream(vbaRootStorage);
    this.SerializeDesignerStream(vbaRootStorage);
  }

  private void SerializeVbaStream(ICompoundStorage vbaStorage)
  {
    using (CompoundStream stream = vbaStorage.CreateStream("_VBA_PROJECT"))
      this.SerializeVbaStream((Stream) stream);
  }

  private void SerializeDirStream(ICompoundStorage vbaStorage)
  {
    using (CompoundStream stream = vbaStorage.CreateStream("dir"))
      this.SerializeDirStream((Stream) stream);
  }

  private void SerializeModuleStream(ICompoundStorage vbaStorage)
  {
    foreach (VbaModule module in (IEnumerable) this.Modules)
    {
      if (module.Type != VbaModuleType.MsForm || module.DesignerStorage != null)
      {
        string streamName = module.StreamName;
        using (CompoundStream stream = vbaStorage.CreateStream(streamName))
          this.SerializeModuleStream(module, (Stream) stream);
      }
    }
  }

  private void SerializeProjectStream(ICompoundStorage vbaRootStorage)
  {
    using (CompoundStream stream = vbaRootStorage.CreateStream("PROJECT"))
      this.SerializeProjectStream((Stream) stream);
  }

  private void SerializeProjectWmStream(ICompoundStorage vbaRootStorage)
  {
    using (CompoundStream stream = vbaRootStorage.CreateStream("PROJECTwm"))
      this.SerializeProjectWmStream((Stream) stream);
  }

  private void SerializeDesignerStream(ICompoundStorage vbaRootStorage)
  {
    foreach (VbaModule module in (IEnumerable) this.Modules)
    {
      if (module.Type == VbaModuleType.MsForm && module.DesignerStorage != null)
        vbaRootStorage.InsertCopy(module.DesignerStorage as ICompoundStorage);
    }
  }

  internal VbaProjectImpl Clone(WorkbookImpl workbook)
  {
    return this.Clone((object) workbook) as VbaProjectImpl;
  }
}

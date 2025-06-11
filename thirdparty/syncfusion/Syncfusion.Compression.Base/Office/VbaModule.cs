// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaModule
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class VbaModule : IVbaModule
{
  private string m_name;
  private string m_description;
  private string m_streamName;
  private uint m_offSet;
  private uint m_helpTopic;
  private VbaAttributesCollection m_attributeCollection;
  private object m_designerStorage;
  private string m_packages;
  private VbaModuleType m_type;
  private string m_code;
  private VbaModulesCollection m_vbaModules;

  internal event NameChangedEventHandler CodeNameChanged;

  internal VbaModule(VbaModulesCollection modules) => this.m_vbaModules = modules;

  public string Name
  {
    get => this.m_name;
    set
    {
      if (this.m_vbaModules[value] != null && this.m_vbaModules[value] != this)
        throw new ArgumentException("Name is already taken");
      if (this.m_name != value && this.CodeNameChanged != null)
        this.CodeNameChanged((object) this, value);
      this.m_name = value;
      this.m_streamName = value;
    }
  }

  public VbaModuleType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public string Code
  {
    get => this.m_code;
    set => this.m_code = value;
  }

  internal uint HelpTopicId
  {
    get => this.m_helpTopic;
    set => this.m_helpTopic = value;
  }

  internal string Description
  {
    get => !string.IsNullOrEmpty(this.m_description) ? this.m_description : string.Empty;
    set => this.m_description = value;
  }

  internal string StreamName
  {
    get => !string.IsNullOrEmpty(this.m_streamName) ? this.m_streamName : this.m_name;
    set => this.m_streamName = value;
  }

  internal uint ModuleOffSet
  {
    get => this.m_offSet;
    set => this.m_offSet = value;
  }

  internal VbaAttributesCollection Attributes
  {
    get
    {
      if (this.m_attributeCollection == null)
        this.m_attributeCollection = new VbaAttributesCollection(this);
      return this.m_attributeCollection;
    }
  }

  public object DesignerStorage
  {
    get
    {
      if (this.Type == VbaModuleType.MsForm)
        return this.m_designerStorage;
      throw new Exception("Not a UserForm type");
    }
    set
    {
      if (this.Type != VbaModuleType.MsForm)
        throw new Exception("Not a UserForm type");
      this.m_designerStorage = value;
    }
  }

  internal string Package
  {
    get
    {
      if (this.Type == VbaModuleType.MsForm)
        return this.m_packages;
      throw new Exception("Not a UserForm type");
    }
    set => this.m_packages = value;
  }

  internal VbaAttributesCollection InitializeAttributes(string name, string clsID)
  {
    this.Attributes.Clear();
    this.Attributes.AddAttribute("VB_NAME", name, true);
    this.Attributes.AddAttribute("VB_Base", clsID, true);
    this.Attributes.AddAttribute("VB_GlobalNameSpace", "False", false);
    this.Attributes.AddAttribute("VB_Creatable", "False", false);
    this.Attributes.AddAttribute("VB_TemplateDerived", "False", false);
    if (this.Type == VbaModuleType.MsForm)
    {
      this.Attributes.RemoveAt(1);
      this.Attributes.AddAttribute("VB_Base", "0{89F37417-10A6-4162-BDD9-9C652CCF21EB}{C4140624-8198-4E53-ACEA-C890085902FD}", true);
      this.Attributes.AddAttribute("VB_PredeclaredId", "True", false);
      this.Attributes.AddAttribute("VB_Exposed", "False", false);
      this.Attributes.AddAttribute("VB_Customizable", "False", false);
    }
    else if (this.Type == VbaModuleType.ClassModule)
    {
      this.Attributes.AddAttribute("VB_PredeclaredId", "False", false);
      this.Attributes.AddAttribute("VB_Exposed", "False", false);
      this.Attributes.AddAttribute("VB_Customizable", "False", false);
    }
    else
    {
      this.Attributes.AddAttribute("VB_PredeclaredId", "True", false);
      this.Attributes.AddAttribute("VB_Exposed", "True", false);
      this.Attributes.AddAttribute("VB_Customizable", "True", false);
    }
    return this.Attributes;
  }

  internal void ParseModuleRecord(Stream moduleStream)
  {
    moduleStream.Position += 2L;
    int count1 = (int) ZipArchive.ReadUInt32(moduleStream);
    byte[] numArray1 = new byte[count1];
    moduleStream.Read(numArray1, 0, count1);
    Encoding encodingType = this.m_vbaModules.Project.EncodingType;
    this.Name = encodingType.GetString(numArray1, 0, numArray1.Length);
    if (ZipArchive.ReadUInt16(moduleStream) == (ushort) 71)
    {
      int count2 = (int) ZipArchive.ReadUInt32(moduleStream);
      byte[] numArray2 = new byte[count2];
      moduleStream.Read(numArray2, 0, count2);
      this.Name = Encoding.Unicode.GetString(numArray2, 0, numArray2.Length);
    }
    moduleStream.Position += 2L;
    int length = (int) ZipArchive.ReadUInt32(moduleStream);
    byte[] numArray3 = new byte[length];
    moduleStream.Position += (long) length;
    moduleStream.Position += 2L;
    int count3 = (int) ZipArchive.ReadUInt32(moduleStream);
    byte[] numArray4 = new byte[count3];
    moduleStream.Read(numArray4, 0, count3);
    this.StreamName = Encoding.Unicode.GetString(numArray4, 0, numArray4.Length);
    moduleStream.Position += 2L;
    int num = (int) ZipArchive.ReadUInt32(moduleStream);
    moduleStream.Position += (long) num;
    moduleStream.Position += 2L;
    int count4 = (int) ZipArchive.ReadUInt32(moduleStream);
    byte[] numArray5 = new byte[count4];
    moduleStream.Read(numArray5, 0, count4);
    this.Description = encodingType.GetString(numArray5, 0, numArray5.Length);
    moduleStream.Position += 6L;
    this.ModuleOffSet = ZipArchive.ReadUInt32(moduleStream);
    moduleStream.Position += 6L;
    this.HelpTopicId = ZipArchive.ReadUInt32(moduleStream);
    moduleStream.Position += 8L;
    this.Type = (VbaModuleType) ZipArchive.ReadUInt16(moduleStream);
    moduleStream.Position += 4L;
    if (ZipArchive.ReadUInt16(moduleStream) == (ushort) 37)
      moduleStream.Position += 4L;
    else
      moduleStream.Position -= 2L;
    if (ZipArchive.ReadUInt16(moduleStream) == (ushort) 40)
      moduleStream.Position += 4L;
    else
      moduleStream.Position -= 2L;
    moduleStream.Position += 6L;
  }

  internal void SerializeModuleRecord(Stream dirStream)
  {
    dirStream.Write(BitConverter.GetBytes(25), 0, 2);
    Encoding encodingType = this.m_vbaModules.Project.EncodingType;
    byte[] bytes1 = encodingType.GetBytes(this.Name);
    dirStream.Write(BitConverter.GetBytes(bytes1.Length), 0, 4);
    dirStream.Write(bytes1, 0, bytes1.Length);
    dirStream.Write(BitConverter.GetBytes(71), 0, 2);
    byte[] bytes2 = Encoding.Unicode.GetBytes(this.Name);
    dirStream.Write(BitConverter.GetBytes(bytes2.Length), 0, 4);
    dirStream.Write(bytes2, 0, bytes2.Length);
    dirStream.Write(BitConverter.GetBytes(26), 0, 2);
    byte[] bytes3 = encodingType.GetBytes(this.StreamName);
    dirStream.Write(BitConverter.GetBytes(bytes3.Length), 0, 4);
    dirStream.Write(bytes3, 0, bytes3.Length);
    dirStream.Write(BitConverter.GetBytes(50), 0, 2);
    byte[] bytes4 = Encoding.Unicode.GetBytes(this.StreamName);
    dirStream.Write(BitConverter.GetBytes(bytes4.Length), 0, 4);
    dirStream.Write(bytes4, 0, bytes4.Length);
    dirStream.Write(BitConverter.GetBytes(28), 0, 2);
    byte[] bytes5 = encodingType.GetBytes(this.Description);
    dirStream.Write(BitConverter.GetBytes(bytes5.Length), 0, 4);
    dirStream.Write(bytes5, 0, bytes5.Length);
    dirStream.Write(BitConverter.GetBytes(72), 0, 2);
    byte[] bytes6 = Encoding.Unicode.GetBytes(this.Description);
    dirStream.Write(BitConverter.GetBytes(bytes6.Length), 0, 4);
    dirStream.Write(bytes6, 0, bytes6.Length);
    dirStream.Write(BitConverter.GetBytes(49), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(0), 0, 4);
    dirStream.Write(BitConverter.GetBytes(30), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(this.HelpTopicId), 0, 4);
    dirStream.Write(BitConverter.GetBytes(44), 0, 2);
    dirStream.Write(BitConverter.GetBytes(2), 0, 4);
    dirStream.Write(BitConverter.GetBytes((int) ushort.MaxValue), 0, 2);
    if (this.Type == VbaModuleType.StdModule)
      dirStream.Write(BitConverter.GetBytes(33), 0, 2);
    else
      dirStream.Write(BitConverter.GetBytes(34), 0, 2);
    dirStream.Write(BitConverter.GetBytes(0), 0, 4);
    dirStream.Write(BitConverter.GetBytes(43), 0, 2);
    dirStream.Write(BitConverter.GetBytes(0), 0, 4);
  }

  internal void Dispose()
  {
    if (this.m_attributeCollection != null)
    {
      this.m_attributeCollection.Clear();
      this.m_attributeCollection = (VbaAttributesCollection) null;
    }
    if (this.m_designerStorage == null)
      return;
    this.m_designerStorage = (object) null;
  }

  internal VbaModule Clone(VbaModulesCollection vbaModules)
  {
    VbaModule parent = (VbaModule) this.MemberwiseClone();
    if (this.m_attributeCollection != null)
      parent.m_attributeCollection = this.m_attributeCollection.Clone(parent);
    if (parent.Type == VbaModuleType.MsForm)
      parent.DesignerStorage = (object) null;
    return parent;
  }
}

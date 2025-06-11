// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpMeta
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System.Collections.Generic;
using XmpCore.Impl.XPath;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpMeta : IXmpMeta
{
  private readonly XmpNode _tree;
  private string _packetHeader;

  public XmpMeta()
    : this(new XmpNode((string) null, (string) null, (PropertyOptions) null))
  {
  }

  public XmpMeta(XmpNode tree) => this._tree = tree;

  public void AppendArrayItem(
    string schemaNs,
    string arrayName,
    PropertyOptions arrayOptions,
    string itemValue,
    PropertyOptions itemOptions)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    if (arrayOptions == null)
      arrayOptions = new PropertyOptions();
    arrayOptions = arrayOptions.IsOnlyArrayOptions ? XmpNodeUtils.VerifySetOptions(arrayOptions, (object) null) : throw new XmpException("Only array form flags allowed for arrayOptions", XmpErrorCode.BadOptions);
    XmpPath xpath = XmpPathParser.ExpandXPath(schemaNs, arrayName);
    XmpNode node = XmpNodeUtils.FindNode(this._tree, xpath, false, (PropertyOptions) null);
    if (node != null)
    {
      if (!node.Options.IsArray)
        throw new XmpException("The named property is not an array", XmpErrorCode.BadXPath);
    }
    else
    {
      if (!arrayOptions.IsArray)
        throw new XmpException("Explicit arrayOptions required to create new array", XmpErrorCode.BadOptions);
      node = XmpNodeUtils.FindNode(this._tree, xpath, true, arrayOptions);
      if (node == null)
        throw new XmpException("Failure creating array node", XmpErrorCode.BadXPath);
    }
    XmpMeta.DoSetArrayItem(node, -1, itemValue, itemOptions, true);
  }

  public void AppendArrayItem(string schemaNs, string arrayName, string itemValue)
  {
    this.AppendArrayItem(schemaNs, arrayName, (PropertyOptions) null, itemValue, (PropertyOptions) null);
  }

  public int CountArrayItems(string schemaNs, string arrayName)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    XmpNode node = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, arrayName), false, (PropertyOptions) null);
    if (node == null)
      return 0;
    if (node.Options.IsArray)
      return node.GetChildrenLength();
    throw new XmpException("The named property is not an array", XmpErrorCode.BadXPath);
  }

  public void DeleteArrayItem(string schemaNs, string arrayName, int itemIndex)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertArrayName(arrayName);
      string propName = XmpPathFactory.ComposeArrayItemPath(arrayName, itemIndex);
      this.DeleteProperty(schemaNs, propName);
    }
    catch (XmpException ex)
    {
    }
  }

  public void DeleteProperty(string schemaNs, string propName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertPropName(propName);
      XmpNode node = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, propName), false, (PropertyOptions) null);
      if (node == null)
        return;
      XmpNodeUtils.DeleteNode(node);
    }
    catch (XmpException ex)
    {
    }
  }

  public void DeleteQualifier(string schemaNs, string propName, string qualNs, string qualName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertPropName(propName);
      string propName1 = propName + XmpPathFactory.ComposeQualifierPath(qualNs, qualName);
      this.DeleteProperty(schemaNs, propName1);
    }
    catch (XmpException ex)
    {
    }
  }

  public void DeleteStructField(
    string schemaNs,
    string structName,
    string fieldNs,
    string fieldName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertStructName(structName);
      string propName = structName + XmpPathFactory.ComposeStructFieldPath(fieldNs, fieldName);
      this.DeleteProperty(schemaNs, propName);
    }
    catch (XmpException ex)
    {
    }
  }

  public bool DoesPropertyExist(string schemaNs, string propName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertPropName(propName);
      return XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, propName), false, (PropertyOptions) null) != null;
    }
    catch (XmpException ex)
    {
      return false;
    }
  }

  public bool DoesArrayItemExist(string schemaNs, string arrayName, int itemIndex)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertArrayName(arrayName);
      string propName = XmpPathFactory.ComposeArrayItemPath(arrayName, itemIndex);
      return this.DoesPropertyExist(schemaNs, propName);
    }
    catch (XmpException ex)
    {
      return false;
    }
  }

  public bool DoesStructFieldExist(
    string schemaNs,
    string structName,
    string fieldNs,
    string fieldName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertStructName(structName);
      string str = XmpPathFactory.ComposeStructFieldPath(fieldNs, fieldName);
      return this.DoesPropertyExist(schemaNs, structName + str);
    }
    catch (XmpException ex)
    {
      return false;
    }
  }

  public bool DoesQualifierExist(string schemaNs, string propName, string qualNs, string qualName)
  {
    try
    {
      ParameterAsserts.AssertSchemaNs(schemaNs);
      ParameterAsserts.AssertPropName(propName);
      string str = XmpPathFactory.ComposeQualifierPath(qualNs, qualName);
      return this.DoesPropertyExist(schemaNs, propName + str);
    }
    catch (XmpException ex)
    {
      return false;
    }
  }

  public IXmpProperty GetArrayItem(string schemaNs, string arrayName, int itemIndex)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    string propName = XmpPathFactory.ComposeArrayItemPath(arrayName, itemIndex);
    return this.GetProperty(schemaNs, propName);
  }

  public IXmpProperty GetLocalizedText(
    string schemaNs,
    string altTextName,
    string genericLang,
    string specificLang)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(altTextName);
    ParameterAsserts.AssertSpecificLang(specificLang);
    genericLang = genericLang != null ? Utils.NormalizeLangValue(genericLang) : (string) null;
    specificLang = Utils.NormalizeLangValue(specificLang);
    XmpNode node = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, altTextName), false, (PropertyOptions) null);
    if (node == null)
      return (IXmpProperty) null;
    object[] objArray = XmpNodeUtils.ChooseLocalizedText(node, genericLang, specificLang);
    int num = (int) objArray[0];
    XmpNode itemNode = (XmpNode) objArray[1];
    return num != 0 ? (IXmpProperty) new XmpMeta.XmpProperty(itemNode) : (IXmpProperty) null;
  }

  public void SetLocalizedText(
    string schemaNs,
    string altTextName,
    string genericLang,
    string specificLang,
    string itemValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(altTextName);
    ParameterAsserts.AssertSpecificLang(specificLang);
    genericLang = genericLang != null ? Utils.NormalizeLangValue(genericLang) : (string) null;
    specificLang = Utils.NormalizeLangValue(specificLang);
    XmpNode node1 = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, altTextName), true, new PropertyOptions(7680));
    if (node1 == null)
      throw new XmpException("Failed to find or create array node", XmpErrorCode.BadXPath);
    if (!node1.Options.IsArrayAltText)
    {
      if (node1.HasChildren || !node1.Options.IsArrayAlternate)
        throw new XmpException("Specified property is no alt-text array", XmpErrorCode.BadXPath);
      node1.Options.IsArrayAltText = true;
    }
    bool flag1 = false;
    XmpNode node2 = (XmpNode) null;
    IIterator iterator1 = node1.IterateChildren();
    while (iterator1.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator1.Next();
      if (!xmpNode.HasQualifier || xmpNode.GetQualifier(1).Name != "xml:lang")
        throw new XmpException("Language qualifier must be first", XmpErrorCode.BadXPath);
      if (xmpNode.GetQualifier(1).Value == "x-default")
      {
        node2 = xmpNode;
        flag1 = true;
        break;
      }
    }
    if (node2 != null && node1.GetChildrenLength() > 1)
    {
      node1.RemoveChild(node2);
      node1.AddChild(1, node2);
    }
    object[] objArray = XmpNodeUtils.ChooseLocalizedText(node1, genericLang, specificLang);
    int num = (int) objArray[0];
    XmpNode xmpNode1 = (XmpNode) objArray[1];
    bool flag2 = specificLang == "x-default";
    switch (num)
    {
      case 0:
        XmpNodeUtils.AppendLangItem(node1, "x-default", itemValue);
        flag1 = true;
        if (!flag2)
        {
          XmpNodeUtils.AppendLangItem(node1, specificLang, itemValue);
          break;
        }
        break;
      case 1:
        if (!flag2)
        {
          if (flag1 && node2 != xmpNode1 && node2 != null && node2.Value == xmpNode1.Value)
            node2.Value = itemValue;
          xmpNode1.Value = itemValue;
          break;
        }
        IIterator iterator2 = node1.IterateChildren();
        while (iterator2.HasNext())
        {
          XmpNode xmpNode2 = (XmpNode) iterator2.Next();
          if (xmpNode2 != node2 && !(xmpNode2.Value != node2?.Value))
            xmpNode2.Value = itemValue;
        }
        if (node2 != null)
        {
          node2.Value = itemValue;
          break;
        }
        break;
      case 2:
        if (flag1 && node2 != xmpNode1 && node2 != null && node2.Value == xmpNode1.Value)
          node2.Value = itemValue;
        xmpNode1.Value = itemValue;
        break;
      case 3:
        XmpNodeUtils.AppendLangItem(node1, specificLang, itemValue);
        if (flag2)
        {
          flag1 = true;
          break;
        }
        break;
      case 4:
        if (node2 != null && node1.GetChildrenLength() == 1)
          node2.Value = itemValue;
        XmpNodeUtils.AppendLangItem(node1, specificLang, itemValue);
        break;
      case 5:
        XmpNodeUtils.AppendLangItem(node1, specificLang, itemValue);
        if (flag2)
        {
          flag1 = true;
          break;
        }
        break;
      default:
        throw new XmpException("Unexpected result from ChooseLocalizedText", XmpErrorCode.InternalFailure);
    }
    if (flag1 || node1.GetChildrenLength() != 1)
      return;
    XmpNodeUtils.AppendLangItem(node1, "x-default", itemValue);
  }

  public void SetLocalizedText(
    string schemaNs,
    string altTextName,
    string genericLang,
    string specificLang,
    string itemValue)
  {
    this.SetLocalizedText(schemaNs, altTextName, genericLang, specificLang, itemValue, (PropertyOptions) null);
  }

  public IXmpProperty GetProperty(string schemaNs, string propName)
  {
    return this.GetProperty(schemaNs, propName, XmpMeta.ValueType.String);
  }

  private IXmpProperty GetProperty(string schemaNs, string propName, XmpMeta.ValueType valueType)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertPropName(propName);
    XmpNode node = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, propName), false, (PropertyOptions) null);
    if (node == null)
      return (IXmpProperty) null;
    if (valueType != XmpMeta.ValueType.String && node.Options.IsCompositeProperty)
      throw new XmpException("Property must be simple when a value type is requested", XmpErrorCode.BadXPath);
    return (IXmpProperty) new XmpMeta.XmpProperty(XmpMeta.EvaluateNodeValue(valueType, node), node);
  }

  private object GetPropertyObject(string schemaNs, string propName, XmpMeta.ValueType valueType)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertPropName(propName);
    XmpNode node = XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, propName), false, (PropertyOptions) null);
    if (node == null)
      return (object) null;
    if (valueType != XmpMeta.ValueType.String && node.Options.IsCompositeProperty)
      throw new XmpException("Property must be simple when a value type is requested", XmpErrorCode.BadXPath);
    return XmpMeta.EvaluateNodeValue(valueType, node);
  }

  public bool GetPropertyBoolean(string schemaNs, string propName)
  {
    return (bool) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Boolean);
  }

  public void SetPropertyBoolean(
    string schemaNs,
    string propName,
    bool propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, propValue ? (object) "True" : (object) "False", options);
  }

  public void SetPropertyBoolean(string schemaNs, string propName, bool propValue)
  {
    this.SetProperty(schemaNs, propName, propValue ? (object) "True" : (object) "False", (PropertyOptions) null);
  }

  public int GetPropertyInteger(string schemaNs, string propName)
  {
    return (int) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Integer);
  }

  public void SetPropertyInteger(
    string schemaNs,
    string propName,
    int propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyInteger(string schemaNs, string propName, int propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public long GetPropertyLong(string schemaNs, string propName)
  {
    return (long) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Long);
  }

  public void SetPropertyLong(
    string schemaNs,
    string propName,
    long propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyLong(string schemaNs, string propName, long propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public double GetPropertyDouble(string schemaNs, string propName)
  {
    return (double) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Double);
  }

  public void SetPropertyDouble(
    string schemaNs,
    string propName,
    double propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyDouble(string schemaNs, string propName, double propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public IXmpDateTime GetPropertyDate(string schemaNs, string propName)
  {
    return (IXmpDateTime) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Date);
  }

  public void SetPropertyDate(
    string schemaNs,
    string propName,
    IXmpDateTime propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyDate(string schemaNs, string propName, IXmpDateTime propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public Calendar GetPropertyCalendar(string schemaNs, string propName)
  {
    return (Calendar) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Calendar);
  }

  public void SetPropertyCalendar(
    string schemaNs,
    string propName,
    Calendar propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyCalendar(string schemaNs, string propName, Calendar propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public byte[] GetPropertyBase64(string schemaNs, string propName)
  {
    return (byte[]) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.Base64);
  }

  public string GetPropertyString(string schemaNs, string propName)
  {
    return (string) this.GetPropertyObject(schemaNs, propName, XmpMeta.ValueType.String);
  }

  public void SetPropertyBase64(
    string schemaNs,
    string propName,
    byte[] propValue,
    PropertyOptions options)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, options);
  }

  public void SetPropertyBase64(string schemaNs, string propName, byte[] propValue)
  {
    this.SetProperty(schemaNs, propName, (object) propValue, (PropertyOptions) null);
  }

  public IXmpProperty GetQualifier(
    string schemaNs,
    string propName,
    string qualNs,
    string qualName)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertPropName(propName);
    string propName1 = propName + XmpPathFactory.ComposeQualifierPath(qualNs, qualName);
    return this.GetProperty(schemaNs, propName1);
  }

  public IXmpProperty GetStructField(
    string schemaNs,
    string structName,
    string fieldNs,
    string fieldName)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertStructName(structName);
    string propName = structName + XmpPathFactory.ComposeStructFieldPath(fieldNs, fieldName);
    return this.GetProperty(schemaNs, propName);
  }

  public IEnumerable<IXmpPropertyInfo> Properties
  {
    get
    {
      XmpIterator iterator = new XmpIterator(this, (string) null, (string) null, (IteratorOptions) null);
      while (iterator.HasNext())
        yield return (IXmpPropertyInfo) iterator.Next();
    }
  }

  public void SetArrayItem(
    string schemaNs,
    string arrayName,
    int itemIndex,
    string itemValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    XmpMeta.DoSetArrayItem(XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, arrayName), false, (PropertyOptions) null) ?? throw new XmpException("Specified array does not exist", XmpErrorCode.BadXPath), itemIndex, itemValue, options, false);
  }

  public void SetArrayItem(string schemaNs, string arrayName, int itemIndex, string itemValue)
  {
    this.SetArrayItem(schemaNs, arrayName, itemIndex, itemValue, (PropertyOptions) null);
  }

  public void InsertArrayItem(
    string schemaNs,
    string arrayName,
    int itemIndex,
    string itemValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    XmpMeta.DoSetArrayItem(XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, arrayName), false, (PropertyOptions) null) ?? throw new XmpException("Specified array does not exist", XmpErrorCode.BadXPath), itemIndex, itemValue, options, true);
  }

  public void InsertArrayItem(string schemaNs, string arrayName, int itemIndex, string itemValue)
  {
    this.InsertArrayItem(schemaNs, arrayName, itemIndex, itemValue, (PropertyOptions) null);
  }

  public void SetProperty(
    string schemaNs,
    string propName,
    object propValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertPropName(propName);
    options = XmpNodeUtils.VerifySetOptions(options, propValue);
    XmpMeta.SetNode(XmpNodeUtils.FindNode(this._tree, XmpPathParser.ExpandXPath(schemaNs, propName), true, options) ?? throw new XmpException("Specified property does not exist", XmpErrorCode.BadXPath), propValue, options, false);
  }

  public void SetProperty(string schemaNs, string propName, object propValue)
  {
    this.SetProperty(schemaNs, propName, propValue, (PropertyOptions) null);
  }

  public void SetQualifier(
    string schemaNs,
    string propName,
    string qualNs,
    string qualName,
    string qualValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertPropName(propName);
    if (!this.DoesPropertyExist(schemaNs, propName))
      throw new XmpException("Specified property does not exist!", XmpErrorCode.BadXPath);
    string propName1 = propName + XmpPathFactory.ComposeQualifierPath(qualNs, qualName);
    this.SetProperty(schemaNs, propName1, (object) qualValue, options);
  }

  public void SetQualifier(
    string schemaNs,
    string propName,
    string qualNs,
    string qualName,
    string qualValue)
  {
    this.SetQualifier(schemaNs, propName, qualNs, qualName, qualValue, (PropertyOptions) null);
  }

  public void SetStructField(
    string schemaNs,
    string structName,
    string fieldNs,
    string fieldName,
    string fieldValue,
    PropertyOptions options)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertStructName(structName);
    string propName = structName + XmpPathFactory.ComposeStructFieldPath(fieldNs, fieldName);
    this.SetProperty(schemaNs, propName, (object) fieldValue, options);
  }

  public void SetStructField(
    string schemaNs,
    string structName,
    string fieldNs,
    string fieldName,
    string fieldValue)
  {
    this.SetStructField(schemaNs, structName, fieldNs, fieldName, fieldValue, (PropertyOptions) null);
  }

  public string GetObjectName() => this._tree.Name ?? string.Empty;

  public void SetObjectName(string name) => this._tree.Name = name;

  public string GetPacketHeader() => this._packetHeader;

  public void SetPacketHeader(string packetHeader) => this._packetHeader = packetHeader;

  public IXmpMeta Clone() => (IXmpMeta) new XmpMeta((XmpNode) this._tree.Clone());

  public string DumpObject() => this.GetRoot().DumpNode(true);

  public void Sort() => this._tree.Sort();

  public void Normalize(ParseOptions options)
  {
    XmpNormalizer.Process(this, options ?? new ParseOptions());
  }

  public XmpNode GetRoot() => this._tree;

  private static void DoSetArrayItem(
    XmpNode arrayNode,
    int itemIndex,
    string itemValue,
    PropertyOptions itemOptions,
    bool insert)
  {
    XmpNode node = new XmpNode("[]", (PropertyOptions) null);
    itemOptions = XmpNodeUtils.VerifySetOptions(itemOptions, (object) itemValue);
    int num = insert ? arrayNode.GetChildrenLength() + 1 : arrayNode.GetChildrenLength();
    if (itemIndex == -1)
      itemIndex = num;
    if (1 > itemIndex || itemIndex > num)
      throw new XmpException("Array index out of bounds", XmpErrorCode.BadIndex);
    if (!insert)
      arrayNode.RemoveChild(itemIndex);
    arrayNode.AddChild(itemIndex, node);
    XmpMeta.SetNode(node, (object) itemValue, itemOptions, false);
  }

  internal static void SetNode(
    XmpNode node,
    object value,
    PropertyOptions newOptions,
    bool deleteExisting)
  {
    int num = 7936;
    if (deleteExisting)
      node.Clear();
    node.Options.MergeWith(newOptions);
    if ((node.Options.GetOptions() & num) == 0)
    {
      XmpNodeUtils.SetNodeValue(node, value);
    }
    else
    {
      if (value != null && value.ToString().Length > 0)
        throw new XmpException("Composite nodes can't have values", XmpErrorCode.BadXPath);
      if ((node.Options.GetOptions() & num) != 0 && (newOptions.GetOptions() & num) != (node.Options.GetOptions() & num))
        throw new XmpException("Requested and existing composite form mismatch", XmpErrorCode.BadXPath);
      node.RemoveChildren();
    }
  }

  private static object EvaluateNodeValue(XmpMeta.ValueType valueType, XmpNode propNode)
  {
    switch (valueType)
    {
      case XmpMeta.ValueType.Boolean:
        return (object) XmpCore.XmpUtils.ConvertToBoolean(propNode.Value);
      case XmpMeta.ValueType.Integer:
        return (object) XmpCore.XmpUtils.ConvertToInteger(propNode.Value);
      case XmpMeta.ValueType.Long:
        return (object) XmpCore.XmpUtils.ConvertToLong(propNode.Value);
      case XmpMeta.ValueType.Double:
        return (object) XmpCore.XmpUtils.ConvertToDouble(propNode.Value);
      case XmpMeta.ValueType.Date:
        return (object) XmpCore.XmpUtils.ConvertToDate(propNode.Value);
      case XmpMeta.ValueType.Calendar:
        return (object) XmpCore.XmpUtils.ConvertToDate(propNode.Value).Calendar;
      case XmpMeta.ValueType.Base64:
        return (object) XmpCore.XmpUtils.DecodeBase64(propNode.Value);
      default:
        return propNode.Value == null && !propNode.Options.IsCompositeProperty ? (object) string.Empty : (object) propNode.Value;
    }
  }

  public enum ValueType
  {
    String,
    Boolean,
    Integer,
    Long,
    Double,
    Date,
    Calendar,
    Base64,
  }

  private sealed class XmpProperty : IXmpProperty
  {
    private readonly XmpMeta.XmpProperty.XmpPropertyType _proptype;
    private readonly object _value;
    private readonly XmpNode _node;

    public XmpProperty(XmpNode itemNode)
    {
      this._node = itemNode;
      this._proptype = XmpMeta.XmpProperty.XmpPropertyType.item;
    }

    public XmpProperty(object value, XmpNode propNode)
    {
      this._value = value;
      this._node = propNode;
      this._proptype = XmpMeta.XmpProperty.XmpPropertyType.prop;
    }

    public string Value
    {
      get
      {
        if (this._proptype == XmpMeta.XmpProperty.XmpPropertyType.item)
          return this._node.Value;
        return this._value?.ToString();
      }
    }

    public PropertyOptions Options => this._node.Options;

    public string Language
    {
      get
      {
        return this._proptype != XmpMeta.XmpProperty.XmpPropertyType.item ? (string) null : this._node.GetQualifier(1).Value;
      }
    }

    public override string ToString()
    {
      return this._proptype != XmpMeta.XmpProperty.XmpPropertyType.item ? this._value.ToString() : this._node.Value;
    }

    private enum XmpPropertyType
    {
      item,
      prop,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PropertyResolver
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Properties.Langs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class PropertyResolver
{
  private static readonly Dictionary<Type, PropertyResolver.EditorTypeCode> TypeCodeDic = new Dictionary<Type, PropertyResolver.EditorTypeCode>()
  {
    [typeof (string)] = PropertyResolver.EditorTypeCode.PlainText,
    [typeof (sbyte)] = PropertyResolver.EditorTypeCode.SByteNumber,
    [typeof (byte)] = PropertyResolver.EditorTypeCode.ByteNumber,
    [typeof (short)] = PropertyResolver.EditorTypeCode.Int16Number,
    [typeof (ushort)] = PropertyResolver.EditorTypeCode.UInt16Number,
    [typeof (int)] = PropertyResolver.EditorTypeCode.Int32Number,
    [typeof (uint)] = PropertyResolver.EditorTypeCode.UInt32Number,
    [typeof (long)] = PropertyResolver.EditorTypeCode.Int64Number,
    [typeof (ulong)] = PropertyResolver.EditorTypeCode.UInt64Number,
    [typeof (float)] = PropertyResolver.EditorTypeCode.SingleNumber,
    [typeof (double)] = PropertyResolver.EditorTypeCode.DoubleNumber,
    [typeof (bool)] = PropertyResolver.EditorTypeCode.Switch,
    [typeof (DateTime)] = PropertyResolver.EditorTypeCode.DateTime,
    [typeof (HorizontalAlignment)] = PropertyResolver.EditorTypeCode.HorizontalAlignment,
    [typeof (VerticalAlignment)] = PropertyResolver.EditorTypeCode.VerticalAlignment,
    [typeof (ImageSource)] = PropertyResolver.EditorTypeCode.ImageSource
  };

  public string ResolveCategory(PropertyDescriptor propertyDescriptor)
  {
    CategoryAttribute categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault<CategoryAttribute>();
    return categoryAttribute != null && !string.IsNullOrEmpty(categoryAttribute.Category) ? categoryAttribute.Category : Lang.Miscellaneous;
  }

  public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
  {
    string str = propertyDescriptor.DisplayName;
    if (string.IsNullOrEmpty(str))
      str = propertyDescriptor.Name;
    return str;
  }

  public string ResolveDescription(PropertyDescriptor propertyDescriptor)
  {
    return propertyDescriptor.Description;
  }

  public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor)
  {
    return propertyDescriptor.IsBrowsable;
  }

  public bool ResolveIsDisplay(PropertyDescriptor propertyDescriptor)
  {
    return propertyDescriptor.IsLocalizable;
  }

  public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor)
  {
    return propertyDescriptor.IsReadOnly;
  }

  public object ResolveDefaultValue(PropertyDescriptor propertyDescriptor)
  {
    return propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault<DefaultValueAttribute>()?.Value;
  }

  public PropertyEditorBase ResolveEditor(PropertyDescriptor propertyDescriptor)
  {
    EditorAttribute editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault<EditorAttribute>();
    return editorAttribute != null && !string.IsNullOrEmpty(editorAttribute.EditorTypeName) ? this.CreateEditor(Type.GetType(editorAttribute.EditorTypeName)) : this.CreateDefaultEditor(propertyDescriptor.PropertyType);
  }

  public virtual PropertyEditorBase CreateDefaultEditor(Type type)
  {
    PropertyResolver.EditorTypeCode editorTypeCode;
    PropertyEditorBase defaultEditor;
    if (PropertyResolver.TypeCodeDic.TryGetValue(type, out editorTypeCode))
    {
      PropertyEditorBase propertyEditorBase;
      switch (editorTypeCode)
      {
        case PropertyResolver.EditorTypeCode.PlainText:
          propertyEditorBase = (PropertyEditorBase) new PlainTextPropertyEditor();
          break;
        case PropertyResolver.EditorTypeCode.SByteNumber:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor((double) sbyte.MinValue, (double) sbyte.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.ByteNumber:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(0.0, (double) byte.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.Int16Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor((double) short.MinValue, (double) short.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.UInt16Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(0.0, (double) ushort.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.Int32Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor((double) int.MinValue, (double) int.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.UInt32Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(0.0, (double) uint.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.Int64Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor((double) long.MinValue, (double) long.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.UInt64Number:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(0.0, 1.8446744073709552E+19);
          break;
        case PropertyResolver.EditorTypeCode.SingleNumber:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(-3.4028234663852886E+38, 3.4028234663852886E+38);
          break;
        case PropertyResolver.EditorTypeCode.DoubleNumber:
          propertyEditorBase = (PropertyEditorBase) new NumberPropertyEditor(double.MinValue, double.MaxValue);
          break;
        case PropertyResolver.EditorTypeCode.Switch:
          propertyEditorBase = (PropertyEditorBase) new SwitchPropertyEditor();
          break;
        case PropertyResolver.EditorTypeCode.DateTime:
          propertyEditorBase = (PropertyEditorBase) new DateTimePropertyEditor();
          break;
        case PropertyResolver.EditorTypeCode.HorizontalAlignment:
          propertyEditorBase = (PropertyEditorBase) new HorizontalAlignmentPropertyEditor();
          break;
        case PropertyResolver.EditorTypeCode.VerticalAlignment:
          propertyEditorBase = (PropertyEditorBase) new VerticalAlignmentPropertyEditor();
          break;
        case PropertyResolver.EditorTypeCode.ImageSource:
          propertyEditorBase = (PropertyEditorBase) new ImagePropertyEditor();
          break;
        default:
          propertyEditorBase = (PropertyEditorBase) new ReadOnlyTextPropertyEditor();
          break;
      }
      defaultEditor = propertyEditorBase;
    }
    else
      defaultEditor = type.IsSubclassOf(typeof (Enum)) ? (PropertyEditorBase) new EnumPropertyEditor() : (PropertyEditorBase) new ReadOnlyTextPropertyEditor();
    return defaultEditor;
  }

  public virtual PropertyEditorBase CreateEditor(Type type)
  {
    return Activator.CreateInstance(type) is PropertyEditorBase instance ? instance : (PropertyEditorBase) new ReadOnlyTextPropertyEditor();
  }

  private enum EditorTypeCode
  {
    PlainText,
    SByteNumber,
    ByteNumber,
    Int16Number,
    UInt16Number,
    Int32Number,
    UInt32Number,
    Int64Number,
    UInt64Number,
    SingleNumber,
    DoubleNumber,
    Switch,
    DateTime,
    HorizontalAlignment,
    VerticalAlignment,
    ImageSource,
  }
}

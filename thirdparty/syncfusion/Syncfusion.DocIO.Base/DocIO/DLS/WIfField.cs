// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WIfField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WIfField(IWordDocument doc) : WField(doc)
{
  private const char FieldTextStart = '\u0013';
  private const char FieldTextEnd = '\u0015';
  private string m_expression1;
  private string m_expression2;
  private string m_operator;
  private string m_trueText;
  private string m_falseText;
  private int m_inc;
  private PseudoMergeField m_expField1;
  private PseudoMergeField m_expField2;
  private List<Entity> m_trueTextField;
  private List<Entity> m_falseTextField;
  private List<PseudoMergeField> m_mergeFields;
  private WFieldMark nestedFieldEnd;

  internal PseudoMergeField Expression1
  {
    get
    {
      if (this.m_expField1 == null)
      {
        this.CheckExpStrings();
        this.m_expField1 = new PseudoMergeField(this.m_expression1);
      }
      return this.m_expField1;
    }
  }

  internal PseudoMergeField Expression2
  {
    get
    {
      if (this.m_expField2 == null)
      {
        this.CheckExpStrings();
        this.m_expField2 = new PseudoMergeField(this.m_expression2);
      }
      return this.m_expField2;
    }
  }

  internal List<Entity> TrueTextField
  {
    get
    {
      if (this.m_trueTextField == null)
        this.m_trueTextField = new List<Entity>();
      return this.m_trueTextField;
    }
  }

  internal List<Entity> FalseTextField
  {
    get
    {
      if (this.m_falseTextField == null)
        this.m_falseTextField = new List<Entity>();
      return this.m_falseTextField;
    }
  }

  internal List<PseudoMergeField> MergeFields
  {
    get
    {
      if (this.m_mergeFields == null)
      {
        this.m_mergeFields = new List<PseudoMergeField>();
        this.UpdateMergeFields();
      }
      return this.m_mergeFields;
    }
  }

  internal void UpdateIfField()
  {
    this.ParseResult();
    string empty = string.Empty;
    string text = this.RemoveText(this.RemoveMergeFormat(this.FieldCode, ref empty), "if");
    List<int> operatorIndexForDoubleQuotes = (List<int>) null;
    string operatorValue = (string) null;
    List<string> stringList = this.SplitIfArguments(text, ref operatorIndexForDoubleQuotes, ref operatorValue);
    if (stringList[0].Contains('\u0013'.ToString()) && stringList[0].Contains('\u0015'.ToString()))
      stringList[0] = stringList[0].Replace('\u0013'.ToString(), "").Replace('\u0015'.ToString(), "");
    bool result = this.UpdateCondition(stringList[0], operatorIndexForDoubleQuotes, operatorValue) == "1";
    List<Entity> entityList = new List<Entity>();
    bool flag = this.HaveAutoNumFieldInResult(result ? this.TrueTextField : this.FalseTextField);
    try
    {
      if (!flag)
        this.UpdateIfFieldResult(result);
      else
        this.RemoveFieldSeparatorAndResultForAutoNumField(this.FieldSeparator);
    }
    catch (Exception ex)
    {
      this.FieldResult = "Error! Unknown op code for conditional.";
    }
  }

  private void RemoveFieldSeparatorAndResultForAutoNumField(WFieldMark fieldMark)
  {
    WField parentField = fieldMark.ParentField;
    WParagraph ownerParagraph = fieldMark.OwnerParagraph;
    WTextRange nextSibling = parentField.FieldSeparator.NextSibling as WTextRange;
    ownerParagraph.Items.Remove((IEntity) nextSibling);
    ownerParagraph.Items.Remove((IEntity) parentField.FieldSeparator);
  }

  private bool HaveAutoNumFieldInResult(List<Entity> fieldResult)
  {
    for (int index = 0; index < fieldResult.Count; ++index)
    {
      if (fieldResult[index] is WField && (fieldResult[index] as WField).FieldType == FieldType.FieldAutoNum)
        return true;
    }
    return false;
  }

  private string GetEntityText(Entity entity, bool isFirstCall)
  {
    if (isFirstCall)
    {
      this.IsFieldSeparator = false;
      this.IsSkip = false;
      this.m_nestedFields.Clear();
    }
    string str = string.Empty;
    switch (entity)
    {
      case ParagraphItem _:
        str = this.UpdateTextForParagraphItem(entity, true);
        break;
      case WParagraph _:
        str = this.UpdateTextForTextBodyItem(entity, true);
        break;
    }
    return str.Replace(ControlChar.CarriegeReturn, string.Empty);
  }

  private string ConvertFieldResultToString(List<Entity> fieldResult)
  {
    string empty = string.Empty;
    bool isFirstCall = true;
    foreach (Entity entity in fieldResult)
    {
      empty += this.GetEntityText(entity, isFirstCall);
      isFirstCall = false;
    }
    return empty;
  }

  private void UpdateIfFieldResult(bool result)
  {
    string str = this.RemoveMergeFormat(this.FieldCode);
    string formattingString = string.Empty;
    int startIndex = -1;
    if (str.Contains("\\*"))
    {
      startIndex = str.LastIndexOf("\\*");
      formattingString = str.Substring(startIndex);
    }
    else if (str.Contains("\\@"))
    {
      startIndex = str.LastIndexOf("\\@");
      formattingString = str.Substring(startIndex);
    }
    List<Entity> entityList = new List<Entity>();
    List<Entity> fieldResult = result ? this.TrueTextField : this.FalseTextField;
    this.RemoveNestedSetField(ref fieldResult);
    this.FieldResult = this.ConvertFieldResultToString(fieldResult);
    double result1 = 0.0;
    bool flag = false;
    if (double.TryParse(this.FieldResult, out result1))
      flag = true;
    if (!(this.Owner is WParagraph) || this.FieldEnd == null)
      return;
    this.CheckFieldSeparator();
    this.RemovePreviousResult();
    if (this.OwnerParagraph == this.FieldEnd.OwnerParagraph)
    {
      if (!string.IsNullOrEmpty(formattingString) && fieldResult.Count > 1 && this.IsAllFieldResultTextRange(fieldResult))
      {
        (fieldResult[0] as WTextRange).Text = this.FieldResult;
        this.RemoveOtherTextRange(fieldResult);
      }
      for (int index = 0; index < fieldResult.Count; ++index)
      {
        int inOwnerCollection = this.FieldEnd.GetIndexInOwnerCollection();
        if (fieldResult[index] is WTextRange)
        {
          if (flag && string.IsNullOrEmpty(formattingString))
          {
            string empty = string.Empty;
            str = this.RemoveMergeFormat(this.FieldCode, ref empty);
            if (!string.IsNullOrEmpty(empty))
              (fieldResult[index] as WTextRange).Text = this.UpdateNumberFormat((fieldResult[index] as WTextRange).Text, empty);
          }
          else if (!string.IsNullOrEmpty(formattingString) && startIndex > str.IndexOf("\\#"))
            (fieldResult[index] as WTextRange).Text = this.UpdateTextFormat((fieldResult[index] as WTextRange).Text, formattingString);
          else if (!string.IsNullOrEmpty(formattingString) && flag)
            (fieldResult[index] as WTextRange).Text = "Error! Picture switch must be first formatting switch.";
        }
        this.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) fieldResult[index]);
        if (this.OwnerParagraph.ChildEntities[inOwnerCollection] is InlineContentControl)
        {
          foreach (IWidget paragraphItem in (CollectionImpl) (this.OwnerParagraph.ChildEntities[inOwnerCollection] as InlineContentControl).ParagraphItems)
            paragraphItem.InitLayoutInfo();
        }
        (this.OwnerParagraph.ChildEntities[inOwnerCollection] as IWidget).InitLayoutInfo();
      }
    }
    else if (this.FieldSeparator.OwnerParagraph != this.FieldEnd.OwnerParagraph)
    {
      WTextBody ownerTextBody = this.FieldEnd.OwnerParagraph.OwnerTextBody;
      if (fieldResult.Count == 0)
      {
        this.MergeFieldSeparatorAndFieldEndParagraph();
      }
      else
      {
        for (int index1 = 0; index1 < fieldResult.Count; ++index1)
        {
          if (fieldResult[index1] is ParagraphItem)
          {
            if (fieldResult[index1].EntityType != EntityType.TextRange || !((fieldResult[index1] as WTextRange).Text == string.Empty))
            {
              this.FieldSeparator.OwnerParagraph.Items.Add((IEntity) fieldResult[index1]);
              ((IWidget) this.FieldSeparator.OwnerParagraph.LastItem).InitLayoutInfo();
            }
            if (index1 == fieldResult.Count - 1)
              this.MergeFieldSeparatorAndFieldEndParagraph();
          }
          else if (index1 == fieldResult.Count - 1 && fieldResult[index1] is WParagraph)
          {
            int count = (fieldResult[index1] as WParagraph).ChildEntities.Count;
            for (int index2 = 0; index2 < count; ++index2)
            {
              this.FieldEnd.OwnerParagraph.ChildEntities.Insert(0, (IEntity) (fieldResult[index1] as WParagraph).ChildEntities[(fieldResult[index1] as WParagraph).ChildEntities.Count - 1]);
              (this.FieldEnd.OwnerParagraph.ChildEntities[0] as IWidget).InitLayoutInfo();
            }
          }
          else
          {
            int inOwnerCollection = this.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection();
            ownerTextBody.Items.Insert(inOwnerCollection, (IEntity) fieldResult[index1]);
            Entity entity = (Entity) ownerTextBody.Items[inOwnerCollection];
            (entity as IWidget).InitLayoutInfo();
            this.InitLayoutInfoOfTextBodyItem(entity);
          }
        }
      }
    }
    else
      this.UpdateIfFieldResult(fieldResult);
    this.IsFieldRangeUpdated = false;
  }

  private bool IsAllFieldResultTextRange(List<Entity> fieldResult)
  {
    foreach (Entity entity in fieldResult)
    {
      switch (entity)
      {
        case WTextRange _:
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        default:
          return false;
      }
    }
    return true;
  }

  private void RemoveOtherTextRange(List<Entity> fieldResult)
  {
    for (int index = 1; index < fieldResult.Count; ++index)
    {
      if (fieldResult[index] is WTextRange)
      {
        fieldResult.Remove(fieldResult[index]);
        --index;
      }
    }
  }

  private void RemoveNestedSetField(ref List<Entity> fieldResult)
  {
    for (int index = 0; index < fieldResult.Count; ++index)
    {
      if (fieldResult[index] is WField && (fieldResult[index] as WField).FieldType == FieldType.FieldSet)
      {
        WField wfield = fieldResult[index] as WField;
        wfield.UpdateSetFields();
        fieldResult.Remove((Entity) wfield);
        --index;
      }
    }
  }

  private void InitLayoutInfoOfTextBodyItem(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        WParagraph wparagraph = entity as WParagraph;
        for (int index = 0; index < wparagraph.Items.Count; ++index)
          ((IWidget) wparagraph.Items[index]).InitLayoutInfo();
        break;
      case WTable _:
        WTable wtable = entity as WTable;
        for (int index1 = 0; index1 < wtable.Rows.Count; ++index1)
        {
          WTableRow row = wtable.Rows[index1];
          ((IWidget) row).InitLayoutInfo();
          for (int index2 = 0; index2 < row.Cells.Count; ++index2)
          {
            WTableCell cell = row.Cells[index2];
            ((IWidget) cell).InitLayoutInfo();
            for (int index3 = 0; index3 < cell.Items.Count; ++index3)
            {
              ((IWidget) cell.Items[index3]).InitLayoutInfo();
              this.InitLayoutInfoOfTextBodyItem((Entity) cell.Items[index3]);
            }
          }
        }
        break;
    }
  }

  private void UpdateIfFieldResult(List<Entity> fieldResult)
  {
    int index1 = this.FieldSeparator.GetIndexInOwnerCollection() + 1;
    WTextBody ownerTextBody = this.FieldSeparator.OwnerParagraph.OwnerTextBody;
    int inOwnerCollection = this.FieldSeparator.OwnerParagraph.GetIndexInOwnerCollection();
    for (int index2 = 0; index2 < fieldResult.Count; ++index2)
    {
      if (fieldResult[index2] is ParagraphItem)
      {
        if (fieldResult[index2].EntityType != EntityType.TextRange || !((fieldResult[index2] as WTextRange).Text == string.Empty))
        {
          this.FieldSeparator.OwnerParagraph.Items.Insert(index1, (IEntity) fieldResult[index2]);
          if (fieldResult[index2] is InlineContentControl)
            this.FieldSeparator.OwnerParagraph.HasSDTInlineItem = true;
          if (this.FieldSeparator.OwnerParagraph.Items[index1] != null)
            ((IWidget) this.FieldSeparator.OwnerParagraph.Items[index1]).InitLayoutInfo();
          ++index1;
        }
      }
      else if (index2 == fieldResult.Count - 1)
      {
        if (fieldResult[index2] is WParagraph)
        {
          this.MergeFieldSeparatorAndFieldEndParagraph(fieldResult[index2] as WParagraph);
          ownerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) fieldResult[index2]);
          if (ownerTextBody.Items[inOwnerCollection + 1] != null)
          {
            ((IWidget) ownerTextBody.Items[inOwnerCollection + 1]).InitLayoutInfo();
            this.InitLayoutInfoOfTextBodyItem((Entity) ownerTextBody.Items[inOwnerCollection + 1]);
          }
        }
        else
        {
          ownerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) fieldResult[index2]);
          if (ownerTextBody.Items[inOwnerCollection + 1] != null)
          {
            ((IWidget) ownerTextBody.Items[inOwnerCollection + 1]).InitLayoutInfo();
            this.InitLayoutInfoOfTextBodyItem((Entity) ownerTextBody.Items[inOwnerCollection + 1]);
          }
          ++inOwnerCollection;
          WParagraph para = new WParagraph((IWordDocument) this.Document);
          this.MergeFieldSeparatorAndFieldEndParagraph(para);
          ownerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) para);
          if (ownerTextBody.Items[inOwnerCollection + 1] != null)
          {
            ((IWidget) ownerTextBody.Items[inOwnerCollection + 1]).InitLayoutInfo();
            this.InitLayoutInfoOfTextBodyItem((Entity) ownerTextBody.Items[inOwnerCollection + 1]);
          }
        }
      }
      else
      {
        ownerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) fieldResult[index2]);
        if (ownerTextBody.Items[inOwnerCollection + 1] != null)
        {
          ((IWidget) ownerTextBody.Items[inOwnerCollection + 1]).InitLayoutInfo();
          this.InitLayoutInfoOfTextBodyItem((Entity) ownerTextBody.Items[inOwnerCollection + 1]);
        }
        ++inOwnerCollection;
      }
    }
  }

  private void MergeFieldSeparatorAndFieldEndParagraph()
  {
    WTextBody ownerTextBody = this.FieldEnd.OwnerParagraph.OwnerTextBody;
    WParagraph ownerParagraph = this.FieldEnd.OwnerParagraph;
    for (int index = 0; index < ownerParagraph.ChildEntities.Count; index = index - 1 + 1)
    {
      WFieldMark wfieldMark1 = (WFieldMark) null;
      WFieldMark wfieldMark2 = (WFieldMark) null;
      if (ownerParagraph.ChildEntities[index] is WField && (ownerParagraph.ChildEntities[index] as WField).FieldEnd != null)
      {
        wfieldMark1 = (ownerParagraph.ChildEntities[index] as WField).FieldEnd;
        wfieldMark1.SetOwner((OwnerHolder) (ownerParagraph.ChildEntities[index] as WField).FieldEnd.OwnerParagraph);
        (ownerParagraph.ChildEntities[index] as WField).FieldEnd = (WFieldMark) null;
      }
      if (ownerParagraph.ChildEntities[index] is WField && (ownerParagraph.ChildEntities[index] as WField).FieldSeparator != null)
      {
        wfieldMark2 = (ownerParagraph.ChildEntities[index] as WField).FieldSeparator;
        wfieldMark2.SetOwner((OwnerHolder) (ownerParagraph.ChildEntities[index] as WField).FieldSeparator.OwnerParagraph);
        (ownerParagraph.ChildEntities[index] as WField).FieldSeparator = (WFieldMark) null;
      }
      this.FieldSeparator.OwnerParagraph.ChildEntities.Add((IEntity) ownerParagraph.ChildEntities[index]);
      if (this.FieldSeparator.OwnerParagraph.ChildEntities[this.FieldSeparator.OwnerParagraph.ChildEntities.Count - 1] is WField)
      {
        WField childEntity = this.FieldSeparator.OwnerParagraph.ChildEntities[this.FieldSeparator.OwnerParagraph.ChildEntities.Count - 1] as WField;
        if (childEntity.FieldEnd == null || !(childEntity.FieldEnd.Owner is WParagraph))
          childEntity.FieldEnd = wfieldMark1;
        if (childEntity.FieldSeparator == null || !(childEntity.FieldSeparator.Owner is WParagraph))
          childEntity.FieldSeparator = wfieldMark2;
      }
    }
    ownerTextBody.Items.Remove((IEntity) ownerParagraph);
  }

  private void MergeFieldSeparatorAndFieldEndParagraph(WParagraph para)
  {
    for (int index = this.FieldEnd.GetIndexInOwnerCollection(); index < this.FieldSeparator.OwnerParagraph.ChildEntities.Count; index = index - 1 + 1)
    {
      WFieldMark wfieldMark = (WFieldMark) null;
      if (this.FieldSeparator.OwnerParagraph.ChildEntities[index] is WField && (this.FieldSeparator.OwnerParagraph.ChildEntities[index] as WField).FieldEnd != null)
      {
        wfieldMark = (this.FieldSeparator.OwnerParagraph.ChildEntities[index] as WField).FieldEnd;
        wfieldMark.SetOwner((OwnerHolder) (this.FieldSeparator.OwnerParagraph.ChildEntities[index] as WField).FieldEnd.OwnerParagraph);
        (this.FieldSeparator.OwnerParagraph.ChildEntities[index] as WField).FieldEnd = (WFieldMark) null;
      }
      para.ChildEntities.Add((IEntity) this.FieldSeparator.OwnerParagraph.ChildEntities[index]);
      if (para.ChildEntities[para.ChildEntities.Count - 1] is WField)
      {
        WField childEntity = para.ChildEntities[para.ChildEntities.Count - 1] as WField;
        if (childEntity.FieldEnd == null || !(childEntity.FieldEnd.Owner is WParagraph))
          childEntity.FieldEnd = wfieldMark;
        if (childEntity.FieldSeparator == null || !(childEntity.FieldSeparator.Owner is WParagraph))
          childEntity.FieldSeparator = (WFieldMark) null;
      }
    }
  }

  internal string ParseResult()
  {
    this.TrueTextField.Clear();
    this.FalseTextField.Clear();
    Entity entity = (Entity) null;
    string empty1 = string.Empty;
    int index = 0;
    string empty2 = string.Empty;
    bool expressionFound = false;
    bool readTrueText = false;
    bool readFalseText = false;
    bool flag = true;
    bool isContinuousAdd = false;
    bool isIfFieldResult = false;
    List<Entity> updatedRange = this.GetUpdatedRange();
    bool isFirstCall = true;
    while (entity != null || flag)
    {
      flag = false;
      if (!expressionFound)
        this.ReadExpression(ref empty1, ref entity, ref readTrueText, ref expressionFound, ref isFirstCall);
      if (readTrueText && entity != null)
        this.ReadTrueResult(ref empty2, ref entity, ref readTrueText, ref isIfFieldResult, ref isFirstCall);
      if (!readTrueText && !readFalseText && expressionFound)
      {
        string text = empty2.TrimStart();
        if (!this.StartsWithExt(text, ControlChar.DoubleQuoteString) && !this.StartsWithExt(text, ControlChar.RightDoubleQuoteString) && !this.StartsWithExt(text, ControlChar.LeftDoubleQuoteString) && !this.StartsWithExt(text, ControlChar.DoubleLowQuoteString) && (text.EndsWith(ControlChar.DoubleQuoteString) || text.EndsWith(ControlChar.RightDoubleQuoteString) || text.EndsWith(ControlChar.LeftDoubleQuoteString)))
          isContinuousAdd = true;
        empty2 = string.Empty;
        readFalseText = true;
      }
      if (readFalseText && entity != null)
        this.ReadFalseResult(ref empty2, ref entity, ref readFalseText, ref isContinuousAdd, ref isIfFieldResult, ref isFirstCall);
      if (entity == null)
      {
        if (updatedRange.Count > index && (readTrueText || readFalseText || !expressionFound))
        {
          entity = updatedRange[index];
          ++index;
        }
        else
          break;
      }
      if (!expressionFound)
      {
        empty1 += this.GetEntityText(entity, isFirstCall);
        isFirstCall = false;
      }
    }
    this.TrimFieldResults(this.TrueTextField);
    this.TrimFieldResults(this.FalseTextField);
    return empty1;
  }

  private List<Entity> GetUpdatedRange()
  {
    bool flag1 = false;
    List<Entity> entityList1 = new List<Entity>();
    List<Entity> entityList2 = new List<Entity>();
    for (int index1 = 0; index1 < this.Range.Items.Count; ++index1)
    {
      Entity entity1 = this.Range.Items[index1] as Entity;
      if (entity1 is ParagraphItem)
      {
        if (this.FieldSeparator == entity1 || this.FieldEnd == entity1)
        {
          flag1 = true;
        }
        else
        {
          if (this.nestedFieldEnd == null)
            this.GetClonedParagraphItem(entity1, ref entityList1);
          if (this.nestedFieldEnd != null && entity1 is WIfField)
          {
            this.Range.Items.Clear();
            this.UpdateFieldRange();
          }
          if (this.nestedFieldEnd != null && entity1 == this.nestedFieldEnd)
            this.nestedFieldEnd = (WFieldMark) null;
        }
      }
      if (entity1 is WParagraph)
      {
        WParagraph wparagraph1 = entity1.Clone() as WParagraph;
        WParagraph wparagraph2 = entity1 as WParagraph;
        int count = wparagraph2.ChildEntities.Count;
        WIfField wifField = (WIfField) null;
        bool flag2 = false;
        bool flag3 = false;
        for (int index2 = 0; index2 < count; ++index2)
        {
          Entity childEntity = wparagraph2.ChildEntities[index2];
          if (this.FieldSeparator == childEntity || this.FieldEnd == childEntity)
          {
            flag1 = true;
            break;
          }
          if (this.nestedFieldEnd == null)
            this.GetClonedParagraphItem(childEntity, ref entityList2);
          if (wifField == null && this.nestedFieldEnd != null && childEntity is WIfField)
          {
            wifField = childEntity as WIfField;
            this.Range.Items.Clear();
            this.UpdateFieldRange();
          }
          if (this.nestedFieldEnd != null && childEntity == this.nestedFieldEnd)
          {
            flag2 = this.nestedFieldEnd.OwnerParagraph != this.nestedFieldEnd.ParentField.OwnerParagraph;
            wifField = (WIfField) null;
            this.nestedFieldEnd = (WFieldMark) null;
          }
          count = wparagraph2.ChildEntities.Count;
        }
        wparagraph1.ClearItems();
        List<Entity> entityList3 = new List<Entity>();
        foreach (Entity entity2 in entityList2)
        {
          if (entity2 is ParagraphItem)
            wparagraph1.ChildEntities.Add((IEntity) entity2);
          else
            entityList3.Add(entity2);
        }
        if (flag2 && entityList1.Count > 0 && entityList1[entityList1.Count - 1] is WParagraph && (entityList1[entityList1.Count - 1] as WParagraph).Items.Count > 0 && (entityList1[entityList1.Count - 1] as WParagraph).LastItem is WFieldMark && ((entityList1[entityList1.Count - 1] as WParagraph).LastItem as WFieldMark).Type == FieldMarkType.FieldEnd)
        {
          WParagraph wparagraph3 = entityList1[entityList1.Count - 1] as WParagraph;
          while (wparagraph1.ChildEntities.Count > 0)
          {
            wparagraph3.ChildEntities.Add((IEntity) wparagraph1.ChildEntities[0]);
            flag3 = true;
          }
        }
        else if ((wparagraph2.ChildEntities.Count == 0 || wparagraph1.ChildEntities.Count > 0) && (this.nestedFieldEnd == null || wifField != null && wifField.FieldEnd == this.nestedFieldEnd))
        {
          entityList1.Add((Entity) wparagraph1);
          flag3 = true;
        }
        foreach (Entity entity3 in entityList3)
        {
          entityList1.Add(entity3);
          flag3 = true;
        }
        if (flag3)
          entityList2.Clear();
      }
      if (this.nestedFieldEnd == null)
        entityList2.Clear();
      if (entity1 is WTable && this.nestedFieldEnd == null)
        entityList1.Add(this.GetClonedTable(entity1));
      if (flag1)
        break;
    }
    entityList2.Clear();
    return entityList1;
  }

  private void GetClonedFieldItem(Entity entity, ref List<Entity> entityList)
  {
    if (entity is WField && ((entity as WField).FieldSeparator != null && (entity as WField).FieldEnd != null || entity is WIfField && (entity as WField).FieldEnd != null) && (entity as WField).FieldType != FieldType.FieldHyperlink)
    {
      (entity as WField).Update();
      this.nestedFieldEnd = (entity as WField).FieldEnd;
      bool flag = false;
      if (entity is WIfField)
      {
        WIfField wifField = (WIfField) entity.Clone();
        wifField.IsSkip = true;
        wifField.IsUpdated = true;
        wifField.FieldEnd = (WFieldMark) null;
        wifField.FieldSeparator = (WFieldMark) null;
        if (entityList.Count == 0 || entityList[entityList.Count - 1] is ParagraphItem)
          entityList.Add((Entity) wifField);
        else if (entityList[entityList.Count - 1] is WParagraph)
          (entityList[entityList.Count - 1] as WParagraph).ChildEntities.Add((IEntity) wifField);
        else
          entityList.Add((Entity) new WParagraph((IWordDocument) entity.Document)
          {
            ChildEntities = {
              (IEntity) wifField
            }
          });
        entityList.Add((Entity) new WTextRange((IWordDocument) entity.Document)
        {
          Text = '\u0013'.ToString()
        });
        flag = true;
      }
      int num = 0;
      if ((entity as WField).FieldSeparator != null && (entity as WField).Range.Items.Contains((object) (entity as WField).FieldSeparator))
        num = (entity as WField).Range.Items.IndexOf((object) (entity as WField).FieldSeparator) + 1;
      else if ((entity as WField).FieldSeparator != null && (entity as WField).Range.Items.Contains((object) (entity as WField).FieldSeparator.OwnerParagraph))
        num = (entity as WField).Range.Items.IndexOf((object) (entity as WField).FieldSeparator.OwnerParagraph);
      for (int index1 = num; index1 < (entity as WField).Range.Items.Count && (entity as WField).Range.Items[index1] != (entity as WField).FieldEnd; ++index1)
      {
        if ((entity as WField).FieldSeparator != null && (entity as WField).Range.Items[index1] == (entity as WField).FieldSeparator.OwnerParagraph)
        {
          if (((entity as WField).Range.Items[index1] as WParagraph).LastItem != (entity as WField).FieldSeparator)
          {
            for (int index2 = (entity as WField).FieldSeparator.GetIndexInOwnerCollection() + 1; index2 < (entity as WField).FieldSeparator.OwnerParagraph.ChildEntities.Count && (entity as WField).FieldEnd != (entity as WField).FieldSeparator.OwnerParagraph.ChildEntities[index2]; ++index2)
              entityList.Add((entity as WField).FieldSeparator.OwnerParagraph.ChildEntities[index2].Clone());
          }
        }
        else if ((entity as WField).Range.Items[index1] == (entity as WField).FieldEnd.OwnerParagraph)
        {
          WParagraph wparagraph1 = (entity as WField).Range.Items[index1] as Entity as WParagraph;
          WParagraph wparagraph2 = wparagraph1.Clone() as WParagraph;
          wparagraph2.ClearItems();
          for (int index3 = 0; index3 < wparagraph1.ChildEntities.Count && (entity as WField).FieldEnd != wparagraph1.ChildEntities[index3]; ++index3)
            wparagraph2.Items.Add((IEntity) wparagraph1.ChildEntities[index3].Clone());
          if ((entity as WField).FieldEnd != wparagraph1.ChildEntities[0])
            entityList.Add((Entity) wparagraph2);
        }
        else
        {
          Entity entity1 = (entity as WField).Range.Items[index1] as Entity;
          if (entity1 is WTextRange)
          {
            string empty = string.Empty;
            if ((entity as WField).FieldType != FieldType.FieldSet)
            {
              string text = (entity1 as WTextRange).Text;
              (entity1 as WTextRange).Text = '\u0013'.ToString() + text + (object) '\u0015';
            }
            else
              continue;
          }
          if (entity1 != null)
            entityList.Add(entity1.Clone());
        }
      }
      if (!(entity is WIfField) || entityList.Count <= 0 || !flag)
        return;
      WTextRange wtextRange = new WTextRange((IWordDocument) entity.Document);
      wtextRange.Text = '\u0015'.ToString();
      WFieldMark wfieldMark = (WFieldMark) (entity as WField).FieldEnd.Clone();
      wfieldMark.SkipDocxItem = true;
      if (entityList[entityList.Count - 1] is ParagraphItem)
      {
        entityList.Add((Entity) wtextRange);
        entityList.Add((Entity) wfieldMark);
      }
      else if (entityList[entityList.Count - 1] is WParagraph)
      {
        (entityList[entityList.Count - 1] as WParagraph).ChildEntities.Add((IEntity) wtextRange);
        (entityList[entityList.Count - 1] as WParagraph).ChildEntities.Add((IEntity) wfieldMark);
      }
      else
        entityList.Add((Entity) new WParagraph((IWordDocument) entity.Document)
        {
          ChildEntities = {
            (IEntity) wtextRange,
            (IEntity) wfieldMark
          }
        });
    }
    else
      entityList.Add(entity.Clone());
  }

  private void GetClonedParagraphItem(Entity entity, ref List<Entity> entityList)
  {
    if (entity is WField)
    {
      if (!(entity as WField).IsFieldWithoutSeparator)
        (entity as WField).CheckFieldSeparator();
      if ((entity as WField).FieldType == FieldType.FieldSet)
        entityList.Add(entity.Clone());
      this.GetClonedFieldItem(entity, ref entityList);
    }
    else
      entityList.Add(entity.Clone());
  }

  private Entity SplitEntity(Entity entity, string remaining, ref bool isFirstCall)
  {
    Entity entity1 = entity.Clone();
    string str = this.GetEntityText(entity, isFirstCall);
    isFirstCall = false;
    if (str.Length - remaining.Length >= 0)
      str = str.Substring(0, str.Length - remaining.Length);
    if (entity is WTextRange)
    {
      (entity as WTextRange).Text = remaining;
      (entity1 as WTextRange).Text = str;
    }
    if (entity is WParagraph)
    {
      WParagraph wparagraph1 = entity1 as WParagraph;
      WParagraph wparagraph2 = entity as WParagraph;
      int count1 = this.GetEntityText((Entity) wparagraph1, false).Length - remaining.Length;
      string empty = string.Empty;
      for (int index1 = 0; index1 < wparagraph1.ChildEntities.Count; ++index1)
      {
        ParagraphItem childEntity1 = wparagraph1.ChildEntities[index1] as ParagraphItem;
        empty += this.GetEntityText((Entity) childEntity1, false);
        if (empty.Length >= count1)
        {
          string remaining1 = empty.Remove(0, count1);
          Entity entity2 = this.SplitEntity(wparagraph2.ChildEntities[index1], remaining1, ref isFirstCall);
          for (int index2 = 0; index2 < index1; ++index2)
          {
            ParagraphItem childEntity2 = wparagraph2.ChildEntities[0] as ParagraphItem;
            if (childEntity2 is WField)
              (childEntity2 as WField).Document.IsSkipFieldDetach = true;
            wparagraph2.ChildEntities.RemoveAt(0);
            if (childEntity2 is WField)
              (childEntity2 as WField).Document.IsSkipFieldDetach = false;
          }
          ParagraphItem childEntity3 = wparagraph1.ChildEntities[index1] as ParagraphItem;
          if (childEntity3 is WField)
            (childEntity3 as WField).Document.IsSkipFieldDetach = true;
          wparagraph1.ChildEntities.RemoveAt(index1);
          if (childEntity3 is WField)
            (childEntity3 as WField).Document.IsSkipFieldDetach = false;
          wparagraph1.ChildEntities.Insert(index1, (IEntity) entity2);
          int count2 = wparagraph1.ChildEntities.Count;
          if (index1 != count2 - 1)
          {
            for (int index3 = index1 + 1; index3 < count2; ++index3)
            {
              ParagraphItem childEntity4 = wparagraph1.ChildEntities[index1 + 1] as ParagraphItem;
              if (childEntity4 is WField)
                (childEntity4 as WField).Document.IsSkipFieldDetach = true;
              wparagraph1.ChildEntities.RemoveAt(index1 + 1);
              if (childEntity4 is WField)
                (childEntity4 as WField).Document.IsSkipFieldDetach = false;
            }
            break;
          }
          break;
        }
      }
    }
    return entity1;
  }

  private void ReadExpression(
    ref string expressionText,
    ref Entity entity,
    ref bool readTrueText,
    ref bool expressionFound,
    ref bool isFirstCall)
  {
    string text1 = expressionText;
    if (this.ContainsOperator(ref text1))
    {
      string text2 = text1;
      if (this.ReachedEndOfExpression(ref text2))
      {
        expressionFound = true;
        expressionText = expressionText.Substring(0, expressionText.Length - text2.Length);
        if (entity == null)
          entity = (Entity) this.GetTextRange(text2);
        else
          this.SplitEntity(entity, text2, ref isFirstCall);
        readTrueText = true;
      }
      else
        entity = (Entity) null;
    }
    else
      entity = (Entity) null;
  }

  private bool ReachedEndOfExpression(ref string text)
  {
    if (this.StartsWithExt(text, " "))
      text = text.TrimStart(' ');
    if (this.StartsWithExt(text, ControlChar.DoubleQuoteString) || this.StartsWithExt(text, ControlChar.RightDoubleQuoteString) || this.StartsWithExt(text, ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(text, ControlChar.DoubleLowQuoteString))
    {
      if (text.Length > 0)
        text = text.Remove(0, 1);
      for (int index = 0; index < text.Length; ++index)
      {
        if (((int) text[index] == (int) ControlChar.DoubleQuote || (int) text[index] == (int) ControlChar.RightDoubleQuote || (int) text[index] == (int) ControlChar.LeftDoubleQuote) && this.IsNeedToSplitText(text, index))
        {
          text = text.Length - 1 == index ? string.Empty : text.Substring(index + 1);
          return true;
        }
      }
    }
    else
    {
      if (this.StartsWithExt(text.TrimStart(), "\\*") && text.Substring(text.IndexOf("*") + 1).Trim().ToUpper() == "MERGEFORMAT")
        return true;
      if (text.Length > 0)
        text = text.Remove(0, 1);
      for (int index = 0; index < text.Length; ++index)
      {
        if ((text[index] == ' ' || (int) text[index] == (int) ControlChar.DoubleQuote || (int) text[index] == (int) ControlChar.RightDoubleQuote || (int) text[index] == (int) ControlChar.LeftDoubleQuote || (int) text[index] == (int) ControlChar.DoubleLowQuote) && this.IsNeedToSplitText(text, index))
        {
          text = text.Length - 1 == index ? string.Empty : text.Substring(index + 1);
          return true;
        }
      }
    }
    return false;
  }

  private bool IsNeedToSplitText(string text, int i)
  {
    int num1 = -1;
    int num2 = -1;
    for (int index = 0; index < text.Length; ++index)
    {
      if (text[index] == '\u0013')
        num1 = index;
      else if (text[index] == '\u0015')
        num2 = index;
      if (num1 < i && i < num2)
        return false;
    }
    return num1 == -1 || num2 != -1 || i == 0;
  }

  private bool ContainsOperator(ref string text)
  {
    string[] strArray = new string[6]
    {
      "<=",
      ">=",
      "<>",
      "=",
      "<",
      ">"
    };
    foreach (string oldValue in strArray)
    {
      if (text.Contains(oldValue))
      {
        int startIndex = text.LastIndexOf(oldValue);
        text = text.Substring(startIndex);
        text = text.Replace(oldValue, string.Empty);
        return true;
      }
    }
    return false;
  }

  private void ReadTrueResult(
    ref string text,
    ref Entity entity,
    ref bool readTrueText,
    ref bool isIfFieldResult,
    ref bool isFirstCall)
  {
    string text1 = text.Trim();
    if (entity is WIfField && (entity as WIfField).nestedFieldEnd == null && !isIfFieldResult)
    {
      entity = (Entity) null;
      if (!(text.Trim() == string.Empty))
        return;
      text += ControlChar.DoubleQuoteString;
      isIfFieldResult = true;
    }
    else
    {
      if (entity is WParagraph && !isIfFieldResult)
      {
        WParagraph para = entity as WParagraph;
        int count = para.ChildEntities.Count;
        this.CheckIfField(ref para, ref text, ref isIfFieldResult);
        if (count == 1 && para.ChildEntities.Count == 0)
        {
          entity = (Entity) null;
          return;
        }
      }
      if (entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd && (entity as WFieldMark).SkipDocxItem)
      {
        if (isIfFieldResult)
        {
          text += ControlChar.DoubleQuoteString;
          isIfFieldResult = false;
        }
        string text2 = text;
        if (this.ReachedEndOfExpression(ref text2))
        {
          readTrueText = false;
          text = text.Substring(0, text.Length - text2.Length);
          entity = (Entity) null;
        }
        else
          entity = (Entity) null;
      }
      else
      {
        if (entity is WParagraph)
        {
          WParagraph para = entity as WParagraph;
          int count = para.ChildEntities.Count;
          this.CheckIfFieldEnd(ref para, ref isIfFieldResult);
          if (count == 1 && para.ChildEntities.Count == 0)
          {
            entity = (Entity) null;
            return;
          }
        }
        if (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet)
          text += this.GetEntityText(entity, isFirstCall);
        isFirstCall = false;
        string text3 = text;
        if (this.ReachedEndOfExpression(ref text3))
        {
          Entity entity1 = this.SplitEntity(entity, text3, ref isFirstCall);
          if (entity1 != null)
            this.TrueTextField.Add(entity1);
          text = text.Substring(0, text.Length - text3.Length);
          readTrueText = false;
        }
        else if (entity is Break)
        {
          string text4 = text.Trim();
          if (this.StartsWithExt(text4, ControlChar.DoubleQuoteString) || this.StartsWithExt(text4, ControlChar.RightDoubleQuoteString) || this.StartsWithExt(text4, ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(text4, ControlChar.DoubleLowQuoteString))
            this.TrueTextField.Add(entity);
          entity = (Entity) null;
        }
        else if (this.TrueTextField.Count > 0 && (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet) && this.StartsWithExt(this.GetEntityText(entity, isFirstCall), " ") && !this.StartsWithExt(text1, ControlChar.DoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.RightDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.LeftDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.DoubleLowQuoteString) && this.HasRenderableParaItem(this.TrueTextField))
        {
          readTrueText = false;
        }
        else
        {
          this.TrueTextField.Add(entity);
          entity = (Entity) null;
        }
      }
    }
  }

  private void ReadFalseResult(
    ref string text,
    ref Entity entity,
    ref bool readFalseText,
    ref bool isContinuousAdd,
    ref bool isIfFieldResult,
    ref bool isFirstCall)
  {
    string text1 = text;
    if (entity is WIfField && (entity as WIfField).nestedFieldEnd == null && !isIfFieldResult)
    {
      entity = (Entity) null;
      if (!(text.Trim() == string.Empty))
        return;
      text += ControlChar.DoubleQuoteString;
      isIfFieldResult = true;
    }
    else
    {
      if (entity is WParagraph && !isIfFieldResult)
      {
        WParagraph para = entity as WParagraph;
        int count = para.ChildEntities.Count;
        this.CheckIfField(ref para, ref text, ref isIfFieldResult);
        if (count == 1 && para.ChildEntities.Count == 0)
        {
          entity = (Entity) null;
          return;
        }
      }
      if (entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd && (entity as WFieldMark).SkipDocxItem)
      {
        if (isIfFieldResult)
        {
          text += ControlChar.DoubleQuoteString;
          isIfFieldResult = false;
        }
        string text2 = text;
        if (this.ReachedEndOfExpression(ref text2))
        {
          readFalseText = false;
          text = string.Empty;
          entity = (Entity) null;
        }
        else
          entity = (Entity) null;
      }
      else
      {
        if (entity is WParagraph)
        {
          WParagraph para = entity as WParagraph;
          int count = para.ChildEntities.Count;
          this.CheckIfFieldEnd(ref para, ref isIfFieldResult);
          if (count == 1 && para.ChildEntities.Count == 0)
          {
            entity = (Entity) null;
            return;
          }
        }
        if (!isContinuousAdd)
        {
          if (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet)
            text += this.GetEntityText(entity, isFirstCall);
          isFirstCall = false;
          string text3 = text;
          if (this.ReachedEndOfExpression(ref text3))
          {
            readFalseText = false;
            Entity entity1 = this.SplitEntity(entity, text3, ref isFirstCall);
            if (entity1 != null)
              this.FalseTextField.Add(entity1);
            text = string.Empty;
          }
          else if (entity is Break)
          {
            string text4 = text.Trim();
            if (this.StartsWithExt(text4, ControlChar.DoubleQuoteString) || this.StartsWithExt(text4, ControlChar.RightDoubleQuoteString) || this.StartsWithExt(text4, ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(text4, ControlChar.DoubleLowQuoteString))
              this.FalseTextField.Add(entity);
          }
          else if (this.FalseTextField.Count > 0 && (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet) && this.StartsWithExt(this.GetEntityText(entity, isFirstCall), " ") && !this.StartsWithExt(text1, ControlChar.DoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.RightDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.LeftDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.DoubleLowQuoteString) && this.HasRenderableParaItem(this.FalseTextField))
            readFalseText = false;
          else
            this.FalseTextField.Add(entity);
        }
        else
        {
          string text5 = string.Empty;
          if (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet)
            text5 = this.GetEntityText(entity, isFirstCall);
          isFirstCall = false;
          if (text5.Contains(ControlChar.DoubleQuoteString) || text5.Contains(ControlChar.LeftDoubleQuoteString) || text5.Contains(ControlChar.RightDoubleQuoteString) || text5.Contains(ControlChar.DoubleLowQuoteString))
          {
            int indexOfDoubleQuote = this.GetIndexOfDoubleQuote(text5);
            string remaining = text5.Substring(0, indexOfDoubleQuote);
            Entity entity2 = this.SplitEntity(entity, remaining, ref isFirstCall);
            if (entity2 != null)
              this.FalseTextField.Add(entity2);
            readFalseText = false;
          }
          else if (entity is Break)
          {
            string text6 = text.Trim();
            if (this.StartsWithExt(text6, ControlChar.DoubleQuoteString) || this.StartsWithExt(text6, ControlChar.RightDoubleQuoteString) || this.StartsWithExt(text6, ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(text6, ControlChar.DoubleLowQuoteString))
              this.FalseTextField.Add(entity);
          }
          else if (this.FalseTextField.Count > 0 && (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldSet) && this.StartsWithExt(this.GetEntityText(entity, isFirstCall), " ") && !this.StartsWithExt(text1, ControlChar.DoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.RightDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.LeftDoubleQuoteString) && !this.StartsWithExt(text1, ControlChar.DoubleLowQuoteString) && this.HasRenderableParaItem(this.FalseTextField))
            readFalseText = false;
          else
            this.FalseTextField.Add(entity);
        }
        entity = (Entity) null;
      }
    }
  }

  private bool HasRenderableParaItem(List<Entity> resultItems)
  {
    for (int index = resultItems.Count - 1; index >= 0; --index)
    {
      Entity resultItem = resultItems[index];
      switch (resultItem)
      {
        case WParagraph _:
        case WTable _:
          return false;
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          continue;
        case InlineContentControl _:
          Entity entity = (resultItem as InlineContentControl).ParagraphItems.LastItem;
          while (true)
          {
            switch (entity)
            {
              case BookmarkStart _:
              case BookmarkEnd _:
              case EditableRangeStart _:
              case EditableRangeEnd _:
                entity = entity.PreviousSibling as Entity;
                continue;
              case WPicture _:
              case WTextBox _:
              case Shape _:
                goto label_7;
              default:
                goto label_6;
            }
          }
label_6:
          return entity is GroupShape;
label_7:
          return true;
        case WPicture _:
        case WTextBox _:
        case Shape _:
          return true;
        default:
          return resultItem is GroupShape;
      }
    }
    return false;
  }

  private void TrimFieldResults(List<Entity> fieldResult)
  {
    this.TrimEmptyText(fieldResult);
    this.TrimDoubleQuotes(fieldResult);
    if (fieldResult.Count <= 0 || !(fieldResult[0] is WParagraph))
      return;
    int num = 0;
    WParagraph wparagraph = fieldResult[0] as WParagraph;
    foreach (Entity childEntity in (CollectionImpl) wparagraph.ChildEntities)
      fieldResult.Insert(num++, childEntity.Clone());
    fieldResult.Remove((Entity) wparagraph);
  }

  private void TrimEmptyText(List<Entity> fieldResult)
  {
    bool flag1 = false;
    for (int index = 0; index < fieldResult.Count; ++index)
    {
      Entity entity = fieldResult[index];
      switch (entity)
      {
        case WTextRange _:
        case WParagraph _:
          if ((entity is WTextRange ? ((entity as WTextRange).Text != null ? ((entity as WTextRange).Text == string.Empty ? 1 : 0) : 0) : 0) != 0 || (entity is WParagraph ? ((entity as WParagraph).Text != null ? ((entity as WParagraph).Text == string.Empty ? 1 : 0) : 0) : 0) != 0 && !flag1)
          {
            fieldResult.RemoveAt(index);
            --index;
            break;
          }
          bool flag2;
          if ((entity is WParagraph ? ((entity is WParagraph ? ((entity as WParagraph).Text.StartsWith("\"") ? 1 : 0) : 0) != 0 || (entity is WParagraph ? ((entity as WParagraph).Text.EndsWith("\"") ? 1 : 0) : 0) != 0 || flag1 ? 1 : (entity is WParagraph ? ((entity as WParagraph).Text.Contains(" ") ? 1 : 0) : 0)) : 0) != 0)
          {
            if ((entity as WParagraph).Text.EndsWith("\""))
            {
              flag2 = false;
              return;
            }
            flag1 = true;
            break;
          }
          if ((entity is WTextRange ? ((entity is WTextRange ? ((entity as WTextRange).Text.StartsWith("\"") ? 1 : 0) : 0) != 0 || (entity is WTextRange ? ((entity as WTextRange).Text.EndsWith("\"") ? 1 : 0) : 0) != 0 || flag1 ? 1 : (entity is WTextRange ? ((entity as WTextRange).Text.Contains(" ") ? 1 : 0) : 0)) : 0) != 0)
          {
            if ((entity as WTextRange).Text.EndsWith("\""))
            {
              flag2 = false;
              return;
            }
            flag1 = true;
            break;
          }
          if (index < fieldResult.Count - 1)
          {
            fieldResult.RemoveRange(index + 1, fieldResult.Count - 1 - index);
            return;
          }
          break;
      }
    }
  }

  private void TrimDoubleQuotes(List<Entity> fieldResult)
  {
    bool isStart = true;
    bool isEnd = false;
    int num = fieldResult.Count;
    for (int entityIndex = 0; entityIndex < fieldResult.Count; ++entityIndex)
    {
      if (fieldResult[entityIndex] is WTextRange)
      {
        WTextRange textRange = fieldResult[entityIndex] as WTextRange;
        if (textRange.Text.Contains('\u0013'.ToString()) || textRange.Text.Contains('\u0015'.ToString()))
          this.RemoveFieldTextStartEndChar(ref textRange, ref fieldResult, ref entityIndex);
        else
          this.TrimDoubleQuotesinTextRange(ref textRange, ref isStart, ref isEnd);
      }
      if (isStart && fieldResult.Count != 0 && fieldResult[entityIndex] is WParagraph)
      {
        WParagraph wparagraph = fieldResult[entityIndex] as WParagraph;
        for (int index = 0; index < wparagraph.ChildEntities.Count; ++index)
        {
          if ((fieldResult[entityIndex] as WParagraph).ChildEntities[index] is WTextRange)
          {
            WTextRange childEntity = wparagraph.ChildEntities[index] as WTextRange;
            if (childEntity.Text.Contains('\u0013'.ToString()) || childEntity.Text.Contains('\u0015'.ToString()))
              this.RemoveFieldTextStartEndChar(ref childEntity);
            else
              this.TrimDoubleQuotesinTextRange(ref childEntity, ref isStart, ref isEnd);
            if (!isStart && childEntity.Text == string.Empty)
              wparagraph.ChildEntities.RemoveAt(index);
          }
        }
      }
      if (!isStart && !isEnd && fieldResult.Count != 0 && fieldResult[entityIndex] is WParagraph)
      {
        WParagraph wparagraph = fieldResult[entityIndex] as WParagraph;
        for (int index = 0; index < wparagraph.ChildEntities.Count; ++index)
        {
          ParagraphItem childEntity1 = wparagraph.ChildEntities[index] as ParagraphItem;
          WField wfield;
          switch (childEntity1)
          {
            case WField _ when (wfield = childEntity1 as WField).FieldSeparator != null && (childEntity1 as WField).FieldEnd != null:
              index = wfield.FieldSeparator.OwnerParagraph != wparagraph ? wparagraph.ChildEntities.Count : wfield.FieldSeparator.Index;
              break;
            case WTextRange _:
              WTextRange childEntity2 = wparagraph.ChildEntities[index] as WTextRange;
              if (childEntity2.Text.Contains('\u0013'.ToString()) || childEntity2.Text.Contains('\u0015'.ToString()))
                this.RemoveFieldTextStartEndChar(ref childEntity2);
              else
                this.TrimDoubleQuotesinTextRange(ref childEntity2, ref isStart, ref isEnd);
              if (isEnd && childEntity2.Text == string.Empty)
              {
                wparagraph.ChildEntities.RemoveAt(index);
                break;
              }
              break;
          }
        }
        if (isEnd)
          num = entityIndex;
      }
      if (isEnd)
        break;
    }
    for (int index = fieldResult.Count - 1; index > num; --index)
      fieldResult.RemoveAt(index);
    this.m_inc = 0;
  }

  private void RemoveFieldTextStartEndChar(
    ref WTextRange textRange,
    ref List<Entity> fieldResult,
    ref int entityIndex)
  {
    if (textRange.Text.StartsWith('\u0013'.ToString()) && textRange.Text.EndsWith('\u0015'.ToString()))
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    else if (textRange.Text.StartsWith('\u0013'.ToString()) && !textRange.Text.EndsWith('\u0015'.ToString()))
    {
      ++this.m_inc;
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    }
    else if (!textRange.Text.StartsWith('\u0013'.ToString()) && textRange.Text.EndsWith('\u0015'.ToString()))
    {
      --this.m_inc;
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    }
    if (textRange.Text == null || !(textRange.Text == string.Empty))
      return;
    fieldResult.RemoveAt(entityIndex);
    --entityIndex;
  }

  private void RemoveFieldTextStartEndChar(ref WTextRange textRange)
  {
    if (textRange.Text.StartsWith('\u0013'.ToString()) && textRange.Text.EndsWith('\u0015'.ToString()))
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    else if (textRange.Text.StartsWith('\u0013'.ToString()) && !textRange.Text.EndsWith('\u0015'.ToString()))
    {
      ++this.m_inc;
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    }
    else
    {
      if (textRange.Text.StartsWith('\u0013'.ToString()) || !textRange.Text.EndsWith('\u0015'.ToString()))
        return;
      --this.m_inc;
      textRange.Text = textRange.Text.Replace('\u0013'.ToString(), string.Empty).Replace('\u0015'.ToString(), string.Empty);
    }
  }

  private void TrimDoubleQuotesinTextRange(
    ref WTextRange textRange,
    ref bool isStart,
    ref bool isEnd)
  {
    if (isStart && this.StartsWithExt(textRange.Text, " "))
      textRange.Text = textRange.Text.TrimStart();
    if (isStart && !textRange.Text.Contains(ControlChar.DoubleLowQuoteString) && !this.StartsWithExt(textRange.Text, ControlChar.DoubleQuoteString) && !this.StartsWithExt(textRange.Text, ControlChar.RightDoubleQuoteString) && !this.StartsWithExt(textRange.Text, ControlChar.LeftDoubleQuoteString) && textRange.Text != string.Empty)
      isStart = false;
    if (isStart && (textRange.Text.Contains(ControlChar.DoubleLowQuoteString) || textRange.Text.Contains(ControlChar.DoubleQuoteString) || textRange.Text.Contains(ControlChar.RightDoubleQuoteString) || textRange.Text.Contains(ControlChar.LeftDoubleQuoteString)) && this.m_inc == 0)
    {
      string text = textRange.Text;
      int startIndex = this.GetIndexOfDoubleQuote(text) + 1;
      textRange.Text = text.Substring(startIndex, text.Length - startIndex);
      isStart = false;
    }
    if (!isStart && !isEnd && (textRange.Text.EndsWith(ControlChar.DoubleLowQuoteString) || textRange.Text.EndsWith(ControlChar.DoubleQuoteString) || textRange.Text.EndsWith(ControlChar.RightDoubleQuoteString) || textRange.Text.EndsWith(ControlChar.LeftDoubleQuoteString)) && this.m_inc == 0)
    {
      string text = textRange.Text;
      int indexOfDoubleQuote = this.GetIndexOfDoubleQuote(text);
      textRange.Text = text.Remove(indexOfDoubleQuote, text.Length - indexOfDoubleQuote);
      isEnd = true;
    }
    else
    {
      if (isStart || isEnd || !textRange.Text.Contains(ControlChar.DoubleLowQuoteString) && !textRange.Text.Contains(ControlChar.DoubleQuoteString) && !textRange.Text.Contains(ControlChar.RightDoubleQuoteString) && !textRange.Text.Contains(ControlChar.LeftDoubleQuoteString) || this.m_inc != 0)
        return;
      string text = textRange.Text;
      int startIndex = this.GetIndexOfDoubleQuote(text) + 1;
      textRange.Text = text.Substring(startIndex, text.Length - startIndex);
      isEnd = true;
    }
  }

  private int GetIndexOfDoubleQuote(string text)
  {
    for (int index = 0; index < text.Length; ++index)
    {
      if ((int) text[index] == (int) ControlChar.DoubleLowQuote || (int) text[index] == (int) ControlChar.DoubleQuote || (int) text[index] == (int) ControlChar.LeftDoubleQuote || (int) text[index] == (int) ControlChar.RightDoubleQuote)
        return index;
    }
    return 0;
  }

  private void CheckIfField(ref WParagraph para, ref string text, ref bool isIfFieldResult)
  {
    for (int index = 0; index < para.ChildEntities.Count; ++index)
    {
      if (para.ChildEntities[index] is WIfField && (para.ChildEntities[index] as WIfField).FieldEnd == null && (para.ChildEntities[index] as WIfField).IsUpdated)
      {
        if (this.StartsWithExt(para.Text.Trim(), ControlChar.DoubleQuoteString) || this.StartsWithExt(para.Text.Trim(), ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(para.Text.Trim(), ControlChar.RightDoubleQuoteString) || this.StartsWithExt(para.Text.Trim(), ControlChar.DoubleLowQuoteString))
          para.ChildEntities.RemoveAt(index);
        else if (text.Trim() != string.Empty)
        {
          para.ChildEntities.RemoveAt(index);
        }
        else
        {
          para.ChildEntities.Insert(index, (IEntity) new WTextRange((IWordDocument) para.Document)
          {
            Text = ControlChar.DoubleQuoteString
          });
          para.ChildEntities.RemoveAt(index + 1);
          isIfFieldResult = true;
        }
      }
    }
  }

  private void CheckIfFieldEnd(ref WParagraph para, ref bool isIfFieldResult)
  {
    for (int index = 0; index < para.ChildEntities.Count; ++index)
    {
      if (para.ChildEntities[index] is WFieldMark && (para.ChildEntities[index] as WFieldMark).SkipDocxItem)
      {
        if (isIfFieldResult)
        {
          para.ChildEntities.Insert(index, (IEntity) new WTextRange((IWordDocument) para.Document)
          {
            Text = ControlChar.DoubleQuoteString
          });
          para.ChildEntities.RemoveAt(index + 1);
          isIfFieldResult = false;
        }
        else
          para.ChildEntities.RemoveAt(index);
      }
    }
  }

  protected internal override void ParseFieldCode(string fieldCode)
  {
    this.UpdateFieldCode(fieldCode);
  }

  protected internal override void UpdateFieldCode(string fieldCode)
  {
    char[] chArray = new char[1]{ '\\' };
    string[] fieldValues = fieldCode.Split(chArray);
    this.ParseFieldFormat(fieldValues);
    this.m_fieldValue = fieldValues[0].Replace("IF", string.Empty);
  }

  private void CheckExpStrings()
  {
    if (this.m_expression1 != null)
      return;
    this.ParseFieldValue();
  }

  private void ParseFieldValue()
  {
    if (this.m_fieldValue == null || this.m_fieldValue == string.Empty)
      return;
    Match match = new Regex("([<>=]+)").Match(this.m_fieldValue);
    this.m_operator = match.Groups[0].Value;
    int index1 = match.Index;
    this.m_expression1 = this.m_fieldValue.Substring(0, index1).Replace("IF", string.Empty);
    int startIndex1 = index1 + this.m_operator.Length;
    string input = this.m_fieldValue.Substring(startIndex1, this.m_fieldValue.Length - startIndex1);
    MatchCollection matchCollection = new Regex("\\s+\"?([^\"]*)\"").Matches(input);
    if (matchCollection.Count != 3)
      return;
    int index2 = matchCollection[0].Index;
    int index3 = matchCollection[1].Index;
    this.m_expression2 = input.Substring(index2, index3 - index2);
    int index4 = matchCollection[1].Index;
    int index5 = matchCollection[2].Index;
    this.m_trueText = input.Substring(index4, index5 - index4);
    int startIndex2 = index5;
    this.m_falseText = input.Substring(startIndex2, input.Length - startIndex2);
  }

  internal void UpdateExpString()
  {
    string str = " ";
    if (this.m_expField1 != null && this.m_expField1.Value != null)
      this.m_expression1 = $"\"{this.m_expField1.Value}\"{str}";
    if (this.m_expField2 != null && this.m_expField2.Value != null)
      this.m_expression2 = $"\"{this.m_expField2.Value}\"{str}";
    if (this.m_expression1 == null || this.m_expression2 == null || this.m_trueText == null || this.m_falseText == null)
      return;
    this.m_fieldValue = this.m_expression1 + this.m_operator + str + this.m_expression2 + this.m_trueText + this.m_falseText;
  }

  internal void UpdateMergeFields()
  {
    if (this.Expression1.FitMailMerge)
      this.m_mergeFields.Add(this.Expression1);
    if (!this.Expression2.FitMailMerge)
      return;
    this.m_mergeFields.Add(this.Expression2);
  }

  protected override object CloneImpl()
  {
    WIfField wifField = base.CloneImpl() as WIfField;
    wifField.m_expField1 = (PseudoMergeField) null;
    wifField.m_expField2 = (PseudoMergeField) null;
    wifField.m_trueTextField = (List<Entity>) null;
    wifField.m_falseTextField = (List<Entity>) null;
    return (object) wifField;
  }

  internal override void Close()
  {
    base.Close();
    this.m_expField1 = (PseudoMergeField) null;
    this.m_expField2 = (PseudoMergeField) null;
    if (this.m_trueTextField != null)
    {
      this.m_trueTextField.Clear();
      this.m_trueTextField = (List<Entity>) null;
    }
    if (this.m_falseTextField != null)
    {
      this.m_falseTextField.Clear();
      this.m_falseTextField = (List<Entity>) null;
    }
    if (this.m_mergeFields == null)
      return;
    this.m_mergeFields.Clear();
    this.m_mergeFields = (List<PseudoMergeField>) null;
  }
}

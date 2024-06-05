import React, { useState, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";

interface SortField {
  fieldName: string;
  fieldValue: string;
}

interface Props {
  onFilter: (queryParams: string) => void;
}

const FilterFields: React.FC<Props> = ({ onFilter }) => {
  const [sortFields, setSortFields] = useState<SortField[]>([
    { fieldName: "", fieldValue: "" },
  ]);
  const { t } = useTranslation();

  const handleAddSortField = () => {
    setSortFields([...sortFields, { fieldName: "", fieldValue: "" }]);
  };

  const handleFieldNameChange = (
    index: number,
    event: ChangeEvent<HTMLInputElement>
  ) => {
    const updatedFields = [...sortFields];
    updatedFields[index].fieldName = event.target.value;
    setSortFields(updatedFields);
  };

  const handleFieldValueChange = (
    index: number,
    event: ChangeEvent<HTMLInputElement>
  ) => {
    const updatedFields = [...sortFields];
    updatedFields[index].fieldValue = event.target.value;
    setSortFields(updatedFields);
  };

  const handleSort = () => {
    const queryParams = sortFields
      .filter((field) => field.fieldName)
      .map((field) => `${field.fieldName}=${field.fieldValue}`)
      .join("&");

    onFilter(queryParams);
  };

  const handleDelete = (index: number) => {
    setSortFields((oldFields) => {
      oldFields.splice(index, 1);
      return oldFields;
    });
  };

  return (
    <div>
      {sortFields.map((field, index) => (
        <div key={index}>
          <label htmlFor={`fieldName${index}`}>{t("Sort by")}</label>
          <input
            type="text"
            id={`fieldName${index}`}
            value={field.fieldName}
            onChange={(event) => handleFieldNameChange(index, event)}
          />
          <label htmlFor={`fieldValue${index}`}>{t("Sort by")}</label>
          <input
            type="text"
            id={`fieldValue${index}`}
            value={field.fieldValue}
            onChange={(event) => handleFieldValueChange(index, event)}
          />
          <button onClick={() => handleDelete(index)}>{t("Delete")}</button>
        </div>
      ))}
      <button onClick={handleAddSortField}>{t("Add Filter Field")}</button>
      <button onClick={handleSort}>{t("Filter")}</button>
    </div>
  );
};

export default FilterFields;

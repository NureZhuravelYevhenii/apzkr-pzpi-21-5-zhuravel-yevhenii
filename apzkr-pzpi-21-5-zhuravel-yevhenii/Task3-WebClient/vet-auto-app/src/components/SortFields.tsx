import React, { useState, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";

interface SortField {
  fieldName: string;
  sortOrder: "asc" | "desc";
}

interface Props {
  onSort: (queryParams: string) => void;
}

const SortFields: React.FC<Props> = ({ onSort }) => {
  const [sortFields, setSortFields] = useState<SortField[]>([
    { fieldName: "", sortOrder: "asc" },
  ]);
  const { t } = useTranslation();

  const handleAddSortField = () => {
    setSortFields([...sortFields, { fieldName: "", sortOrder: "asc" }]);
  };

  const handleSortByChange = (
    index: number,
    event: ChangeEvent<HTMLInputElement>
  ) => {
    const updatedFields = [...sortFields];
    updatedFields[index].fieldName = event.target.value;
    setSortFields(updatedFields);
  };

  const handleSortOrderChange = (
    index: number,
    event: ChangeEvent<HTMLInputElement>
  ) => {
    const updatedFields = [...sortFields];
    updatedFields[index].sortOrder = event.target.checked ? "desc" : "asc";
    setSortFields(updatedFields);
  };

  const handleSort = () => {
    const queryParams = sortFields
      .filter((field) => field.fieldName)
      .map((field) => `${field.fieldName}Sort=${field.sortOrder}`)
      .join("&");

    onSort(queryParams);
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
          <label htmlFor={`sortByField${index}`}>{t("Sort by")}:</label>
          <input
            type="text"
            id={`sortByField${index}`}
            value={field.fieldName}
            onChange={(event) => handleSortByChange(index, event)}
          />
          <input
            type="checkbox"
            id={`sortOrderDescending${index}`}
            checked={field.sortOrder === "desc"}
            onChange={(event) => handleSortOrderChange(index, event)}
          />
          <label htmlFor={`sortOrderDescending${index}`}>
            {t("Descending")}
          </label>
          <button onClick={() => handleDelete(index)}>{t("Delete")}</button>
        </div>
      ))}
      <button onClick={handleAddSortField}>{t("Add Sort Field")}</button>
      <button onClick={handleSort}>{t("Sort")}</button>
    </div>
  );
};

export default SortFields;

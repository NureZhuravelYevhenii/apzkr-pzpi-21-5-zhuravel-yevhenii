import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalUpdateFormValues {
  name: string;
  typeId: string;
}

const AnimalUpdateForm = () => {
  const { animalId } = useParams();
  const [animalUpdateFormValues, setAnimalUpdateFormValues] =
    useState<AnimalUpdateFormValues>({
      name: "",
      typeId: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<AnimalUpdateFormValues>(`api/animals/single?Id=${animalId}`)
      .then((response) => response.data)
      .then(setAnimalUpdateFormValues);
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setAnimalUpdateFormValues({ ...animalUpdateFormValues, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .put(`api/animals`, animalUpdateFormValues)
        .then(() => navigate("/animals"));
      // Redirect or display success message
    } catch (error) {
      console.error("Animal update failed:", error);
      // Handle update failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Update Animal")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">{t("Name")}:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={animalUpdateFormValues.name}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="typeId">{t("Type ID")}:</label>
          <input
            type="text"
            id="typeId"
            name="typeId"
            value={animalUpdateFormValues.typeId}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Update Animal")}</button>
      </form>
    </div>
  );
};

export default AnimalUpdateForm;

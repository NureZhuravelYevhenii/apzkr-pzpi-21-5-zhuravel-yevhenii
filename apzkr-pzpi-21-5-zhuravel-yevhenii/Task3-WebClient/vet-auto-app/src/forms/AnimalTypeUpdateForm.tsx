import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalTypeUpdateFormValues {
  id: string;
  name: string;
}

const AnimalTypeUpdateForm = () => {
  const { typeId } = useParams();
  const [animalTypeUpdateFormValues, setAnimalTypeUpdateFormValues] =
    useState<AnimalTypeUpdateFormValues>({
      id: "",
      name: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<AnimalTypeUpdateFormValues>(`api/animal-types/single?Id=${typeId}`) // Make sure to replace `any` with the correct type for AnimalTypeUpdateFormValues
      .then((response) => response.data)
      .then(setAnimalTypeUpdateFormValues); // Replace `setAnimalTypeUpdateFormValues` with the correct state setter
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setAnimalTypeUpdateFormValues({
      ...animalTypeUpdateFormValues,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .put(`api/animal-types`, animalTypeUpdateFormValues)
        .then(() => navigate("/animal-types"));
      // Redirect or display success message
    } catch (error) {
      console.error("Animal type update failed:", error);
      // Handle update failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Update Animal Type")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">{t("Name")}:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={animalTypeUpdateFormValues.name}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Update Animal Type")}</button>
      </form>
    </div>
  );
};

export default AnimalTypeUpdateForm;

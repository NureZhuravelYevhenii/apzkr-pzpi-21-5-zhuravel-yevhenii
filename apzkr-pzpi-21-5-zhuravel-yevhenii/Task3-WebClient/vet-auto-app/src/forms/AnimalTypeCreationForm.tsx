import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalTypeCreationDtoFormData {
  name: string | null;
}

const AnimalTypeCreationForm = () => {
  const [formData, setFormData] = useState<AnimalTypeCreationDtoFormData>({
    name: null,
  });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .post("/api/animal-types", formData)
        .then(() => navigate("/animal-types"));
      // Handle successful form submission
    } catch (error) {
      // Handle error
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        name="name"
        value={formData.name || ""}
        onChange={handleChange}
        placeholder={t("Animal Type Name")}
      />
      <button type="submit">{t("Submit")}</button>
    </form>
  );
};

export default AnimalTypeCreationForm;

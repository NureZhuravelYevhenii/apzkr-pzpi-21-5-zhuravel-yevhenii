import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalCenterCreationFormData {
  name: string;
  password: string;
  address: string;
  info: string;
}

const AnimalCenterCreationForm: React.FC = () => {
  const [formData, setFormData] = useState<AnimalCenterCreationFormData>({
    name: "",
    password: "",
    address: "",
    info: "",
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
        .post("/api/animalCenter", formData)
        .then(() => navigate("/animal-centers"));
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
        value={formData.name}
        onChange={handleChange}
        placeholder={t("Name")}
      />
      <input
        type="password"
        name="password"
        value={formData.password}
        onChange={handleChange}
        placeholder={t("Password")}
      />
      <input
        type="text"
        name="address"
        value={formData.address}
        onChange={handleChange}
        placeholder={t("Address")}
      />
      <input
        type="text"
        name="info"
        value={formData.info}
        onChange={handleChange}
        placeholder={t("Info")}
      />
      <button type="submit">{t("Submit")}</button>
    </form>
  );
};

export default AnimalCenterCreationForm;

import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalCenterUpdateFormData {
  id: string;
  name: string;
  passwordHash: string;
  address: string;
  info: string;
}

const AnimalCenterUpdateForm = () => {
  const { animalCenterId } = useParams();
  const [formData, setFormData] = useState<AnimalCenterUpdateFormData>({
    id: "",
    name: "",
    passwordHash: "",
    address: "",
    info: "",
  });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<AnimalCenterUpdateFormData>(
        `api/animalCenter/single?Id=${animalCenterId}`
      ) // Make sure to replace `any` with the correct type for AnimalCenterUpdateFormValues
      .then((response) => response.data)
      .then((data) => {
        setFormData(data);
      }); // Replace `setAnimalCenterUpdateFormValues` with the correct state setter
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance.put(`api/animalCenter`, formData);
      navigate("/animal-centers");
    } catch (error) {
      // Handle error
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        name="id"
        value={formData.id}
        onChange={handleChange}
        placeholder={t("ID")}
      />
      <input
        type="text"
        name="name"
        value={formData.name}
        onChange={handleChange}
        placeholder={t("Name")}
      />
      <input
        type="password"
        name="passwordHash"
        value={formData.passwordHash}
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

export default AnimalCenterUpdateForm;

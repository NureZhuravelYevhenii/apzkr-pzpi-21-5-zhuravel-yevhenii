import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalCreationFormData {
  name: string;
  typeId: string;
}

const AnimalCreationForm: React.FC = () => {
  const [formData, setFormData] = useState<AnimalCreationFormData>({
    name: "",
    typeId: "",
  });
  const [animalTypes, setAnimalTypes] = useState<
    { name: string; id: string }[]
  >([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get("api/animal-types")
      .then((res) => res.data)
      .then(setAnimalTypes);
  }, []);

  const handleChange = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .post("/api/animals", formData)
        .then(() => navigate("/animals"));
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
      <select
        name="typeId"
        value={formData.typeId}
        onChange={handleChange}
        defaultValue=""
      >
        <option value="">{t("Select animal type")}</option>
        {animalTypes.map((animalType) => (
          <option value={animalType.id} key={animalType.id}>
            {animalType.name}
          </option>
        ))}
      </select>
      <button type="submit">{t("Submit")}</button>
    </form>
  );
};

export default AnimalCreationForm;

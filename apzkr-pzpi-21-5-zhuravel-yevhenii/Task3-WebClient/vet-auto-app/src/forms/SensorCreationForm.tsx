import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

const SensorCreationForm = () => {
  const [sensorCreationFormValues, setSensorCreationFormValues] = useState({
    animalId: "",
    typeId: "",
  });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleInputChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = event.target;
    setSensorCreationFormValues({
      ...sensorCreationFormValues,
      [name]: value,
    });
  };

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    axiosInstance
      .post("sensors", sensorCreationFormValues)
      .then(() => {
        navigate("/sensors");
      })
      .catch((error) => {
        // Handle error
        console.error("Error:", error);
      });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>
          {t("Animal ID")}:
          <input
            type="text"
            name="animalId"
            value={sensorCreationFormValues.animalId}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <div>
        <label>
          {t("Type ID")}:
          <input
            type="text"
            name="typeId"
            value={sensorCreationFormValues.typeId}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <button type="submit">{t("Create Sensor")}</button>
    </form>
  );
};

export default SensorCreationForm;

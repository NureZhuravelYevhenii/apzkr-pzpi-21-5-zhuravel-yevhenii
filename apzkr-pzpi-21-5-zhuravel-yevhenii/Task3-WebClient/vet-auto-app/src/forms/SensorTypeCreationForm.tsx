import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface SensorTypeCreationFormValues {
  name: string;
}

const SensorTypeCreationForm = () => {
  const [sensorTypeCreationFormValues, setSensorTypeCreationFormValues] =
    useState<SensorTypeCreationFormValues>({
      name: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setSensorTypeCreationFormValues({
      ...sensorTypeCreationFormValues,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .post("/api/sensor-types", sensorTypeCreationFormValues)
        .then(() => navigate("/sensor-types"));
    } catch (error) {
      console.error("Sensor type creation failed:", error);
      // Handle creation failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Create Sensor Type")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">{t("Name")}:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={sensorTypeCreationFormValues.name}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Create Sensor Type")}</button>
      </form>
    </div>
  );
};

export default SensorTypeCreationForm;

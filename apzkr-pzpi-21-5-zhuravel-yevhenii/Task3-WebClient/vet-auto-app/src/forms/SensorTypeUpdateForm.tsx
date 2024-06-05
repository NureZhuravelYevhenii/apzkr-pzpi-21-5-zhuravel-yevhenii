import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface SensorTypeUpdateFormValues {
  name: string;
}

const SensorTypeUpdateForm = () => {
  const { typeId } = useParams();
  const [sensorTypeUpdateFormValues, setSensorTypeUpdateFormValues] =
    useState<SensorTypeUpdateFormValues>({
      name: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<SensorTypeUpdateFormValues>(`api/sensor-types/single?Id=${typeId}`)
      .then((response) => response.data)
      .then(setSensorTypeUpdateFormValues);
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setSensorTypeUpdateFormValues({
      ...sensorTypeUpdateFormValues,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .put(`api/sensor-types`, sensorTypeUpdateFormValues)
        .then(() => navigate("/sensor-types"));
      // Redirect or display success message
    } catch (error) {
      console.error("Sensor type update failed:", error);
      // Handle update failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Update Sensor Type")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">{t("Name")}:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={sensorTypeUpdateFormValues.name}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Update Sensor Type")}</button>
      </form>
    </div>
  );
};

export default SensorTypeUpdateForm;

import React, { useState, useEffect } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface SensorUpdateFormValues {
  animalId: string;
  typeId: string;
}

const SensorUpdateForm = () => {
  const { sensorId } = useParams();
  const [sensorUpdateFormValues, setSensorUpdateFormValues] =
    useState<SensorUpdateFormValues>({
      animalId: "",
      typeId: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<SensorUpdateFormValues>(`api/sensors/single?Id=${sensorId}`)
      .then((response) => response.data)
      .then(setSensorUpdateFormValues)
      .catch((error) => {
        console.error("Error:", error);
      });
  }, [sensorId]);

  const handleInputChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = event.target;
    setSensorUpdateFormValues({
      ...sensorUpdateFormValues,
      [name]: value,
    });
  };

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    axiosInstance
      .put(`api/sensors`, sensorUpdateFormValues)
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
            value={sensorUpdateFormValues.animalId}
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
            value={sensorUpdateFormValues.typeId}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <button type="submit">{t("Update Sensor")}</button>
    </form>
  );
};

export default SensorUpdateForm;

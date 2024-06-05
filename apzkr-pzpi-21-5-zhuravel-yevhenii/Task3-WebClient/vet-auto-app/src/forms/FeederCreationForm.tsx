import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface FeederCreationFormValuesModel {
  location: string;
}

const FeederCreationForm = () => {
  const [feederCreationFormValuesModel, setFeederCreationFormValuesModel] =
    useState<FeederCreationFormValuesModel>({
      location: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFeederCreationFormValuesModel({
      ...feederCreationFormValuesModel,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const feederCreationFormValues = {
      location: {
        coordinates: feederCreationFormValuesModel.location
          .split(" ")
          .map((v) => +v),
      },
    };
    try {
      await axiosInstance
        .post(`/api/feeders`, feederCreationFormValues)
        .then(() => navigate("/feeders"));
      // Redirect or display success message
    } catch (error) {
      console.error("Feeder creation failed:", error);
      // Handle creation failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Create Feeder")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="location">{t("Location")}:</label>
          <input
            type="text"
            id="location"
            name="location"
            value={feederCreationFormValuesModel.location}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Create Feeder")}</button>
      </form>
    </div>
  );
};

export default FeederCreationForm;

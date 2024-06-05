import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface FeederUpdateFormValues {
  location: string;
}

const FeederUpdateForm = () => {
  const { feederId } = useParams();
  const [feederUpdateFormValues, setFeederUpdateFormValues] =
    useState<FeederUpdateFormValues>({
      location: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<FeederUpdateFormValues>(`api/feeders/single?Id=${feederId}`) // Make sure to replace `any` with the correct type for FeederUpdateFormValues
      .then((response) => response.data)
      .then(setFeederUpdateFormValues); // Replace `setFeederUpdateFormValues` with the correct state setter
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFeederUpdateFormValues({ ...feederUpdateFormValues, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .put(`api/feeders`, feederUpdateFormValues)
        .then(() => navigate("/feeders"));
      // Redirect or display success message
    } catch (error) {
      console.error("Feeder update failed:", error);
      // Handle update failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Update Feeder")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="location">{t("Location")}:</label>
          <input
            type="text"
            id="location"
            name="location"
            value={feederUpdateFormValues.location}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Update Feeder")}</button>
      </form>
    </div>
  );
};

export default FeederUpdateForm;

import React, { useState, useEffect } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalFeederUpdateFormValues {
  animalId: string;
  feederId: string;
  feedDate: string;
}

const AnimalFeederUpdateForm = () => {
  const { feederId } = useParams();
  const [animalFeederUpdateFormValues, setAnimalFeederUpdateFormValues] =
    useState<AnimalFeederUpdateFormValues>({
      animalId: "",
      feederId: "",
      feedDate: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<AnimalFeederUpdateFormValues>(
        `api/animalFeeders/single?Id=${feederId}`
      )
      .then((response) => response.data)
      .then(setAnimalFeederUpdateFormValues)
      .catch((error) => {
        console.error("Error:", error);
      });
  }, [feederId]);

  const handleInputChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = event.target;
    setAnimalFeederUpdateFormValues({
      ...animalFeederUpdateFormValues,
      [name]: value,
    });
  };

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    axiosInstance
      .put(`api/animalFeeders`, animalFeederUpdateFormValues)
      .then(() => {
        navigate("/animal-feeders");
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
            value={animalFeederUpdateFormValues.animalId}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <div>
        <label>
          {t("Feeder ID")}:
          <input
            type="text"
            name="feederId"
            value={animalFeederUpdateFormValues.feederId}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <div>
        <label>
          {t("Feed Date")}:
          <input
            type="date"
            name="feedDate"
            value={animalFeederUpdateFormValues.feedDate}
            onChange={handleInputChange}
          />
        </label>
      </div>
      <button type="submit">{t("Update Animal Feeder")}</button>
    </form>
  );
};

export default AnimalFeederUpdateForm;

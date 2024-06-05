import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";

interface AnimalFeeder {
  animalId: string;
  feederId: string;
  feedDate: string; // Assuming it's a string in ISO format for simplicity
}

const AnimalFeederView: React.FC = () => {
  const [animalFeeders, setAnimalFeeders] = useState<AnimalFeeder[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchAnimalFeeders = async () => {
      try {
        const response = await axiosInstance.get("/api/animalFeeders");
        setAnimalFeeders(response.data);
      } catch (error) {
        console.error("Error fetching animal feeders:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchAnimalFeeders();
  }, []);

  const deleteAnimalFeeder = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/animalFeeders`, { data: { id } })
        .then(() => navigate(0));
      // If successful, remove the deleted item from the list
      // Or, fetch the list again to reflect the changes
    } catch (error) {
      console.error("Error deleting animal feeder:", error);
      // Handle error, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Animal Feeders")}</h2>
      <ul>
        {animalFeeders.map((animalFeeder) => (
          <li key={`${animalFeeder.animalId}-${animalFeeder.feederId}`}>
            <h3>
              {t("Animal ID")}: {animalFeeder.animalId}
            </h3>
            <p>
              {t("Feeder ID")}: {animalFeeder.feederId}
            </p>
            <p>
              {t("Feed Date")}: {animalFeeder.feedDate}
            </p>
            <Link to={`/animal-feeders/update/${animalFeeder.animalId}`}>
              {t("Edit")}
            </Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteAnimalFeeder(animalFeeder.animalId);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/animal-feeders/create">{t("Create New Animal Feeder")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default AnimalFeederView;

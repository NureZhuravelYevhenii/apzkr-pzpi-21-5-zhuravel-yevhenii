import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import FilterFields from "../components/FilterFields";
import { useTranslation } from "react-i18next";

interface AnimalType {
  id: string;
  name: string;
}

const AnimalTypeView: React.FC = () => {
  const [animalTypes, setAnimalTypes] = useState<AnimalType[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchAnimalTypes = async () => {
      try {
        const response = await axiosInstance.get("/api/animal-types");
        setAnimalTypes(response.data);
      } catch (error) {
        console.error("Error fetching animal types:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchAnimalTypes();
  }, []);

  const deleteAnimalType = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/animal-types`, { data: { id } })
        .then(() => navigate(0));
      // If successful, remove the deleted item from the list
      // Or, fetch the list again to reflect the changes
    } catch (error) {
      console.error("Error deleting animal type:", error);
      // Handle error, e.g., display error message
    }
  };

  const handleOnFilter = (query: string) => {
    axiosInstance
      .get(`api/animal-types?${query}`)
      .then((res) => res.data)
      .then(setAnimalTypes);
  };

  return (
    <div>
      <h2>{t("Animal Types")}</h2>
      <FilterFields onFilter={handleOnFilter} />
      <ul>
        {animalTypes.map((animalType) => (
          <li key={animalType.id}>
            <h3>{animalType.name}</h3>
            <Link to={`/animal-types/update/${animalType.id}`}>
              {t("Edit")}
            </Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteAnimalType(animalType.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/animal-types/create">{t("Create New Animal Type")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default AnimalTypeView;

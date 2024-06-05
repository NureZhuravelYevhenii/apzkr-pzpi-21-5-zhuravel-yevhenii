import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import SortFields from "../components/SortFields";
import { useTranslation } from "react-i18next";

interface Animal {
  id: string;
  name: string;
  typeId: string;
}

const AnimalView: React.FC = () => {
  const [animals, setAnimals] = useState<Animal[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchAnimals = async () => {
      try {
        const response = await axiosInstance.get("/api/animals");
        setAnimals(response.data);
      } catch (error) {
        console.error("Error fetching animals:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchAnimals();
  }, []);

  const deleteAnimal = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/animals`, { data: { id } })
        .then(() => navigate(0));
    } catch (error) {
      console.error("Error deleting animal:", error);
      // Handle error, e.g., display error message
    }
  };

  const handleSort = (query: string) => {
    axiosInstance
      .get(`/api/animals/?${query}`)
      .then((res) => res.data)
      .then(setAnimals);
  };

  return (
    <div>
      <h2>{t("Animals")}</h2>
      <SortFields onSort={handleSort} />
      <ul>
        {animals.map((animal) => (
          <li key={animal.id} onClick={() => navigate(`/animals/${animal.id}`)}>
            <h3>{animal.name}</h3>
            <p>
              {t("Type")}: {animal.typeId}
            </p>
            <Link to={`/animals/update/${animal.id}`}>{t("Edit")}</Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteAnimal(animal.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/animals/create">{t("Create New Animal")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default AnimalView;

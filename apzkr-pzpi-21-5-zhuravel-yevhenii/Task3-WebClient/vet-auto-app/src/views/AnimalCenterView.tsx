import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";

interface AnimalCenter {
  id: string;
  name: string;
  address: string;
  animals: {
    name: string;
  }[];
}

const AnimalCenterView = () => {
  const [animalCenters, setAnimalCenters] = useState<AnimalCenter[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchAnimalCenters = async () => {
      try {
        const response = await axiosInstance.get("/api/animalCenter");
        setAnimalCenters(response.data);
      } catch (error) {
        console.error("Error fetching animal centers:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchAnimalCenters();
  }, []);

  const deleteAnimalCenter = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/animalCenter/`, { data: { id } })
        .then(() => navigate(0));
    } catch (error) {
      console.error("Error deleting animal center:", error);
    }
  };

  return (
    <div>
      <h2>{t("Animal Centers")}</h2>
      <ul>
        {animalCenters.map((animalCenter) => (
          <li key={animalCenter.id}>
            <h3>{animalCenter.name}</h3>
            <p>{animalCenter.address}</p>
            {animalCenter.animals.map((a) => (
              <p>{a.name}</p>
            ))}
            <Link to={`/animal-centers/update/${animalCenter.id}`}>
              {t("Edit")}
            </Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteAnimalCenter(animalCenter.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/animalCenters/create">{t("Create New Animal Center")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default AnimalCenterView;

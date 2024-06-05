import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";

interface Feeder {
  id: string;
  location: Location;
}

interface Location {
  coordinates: number[];
}

const FeederView: React.FC = () => {
  const [feeders, setFeeders] = useState<Feeder[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchFeeders = async () => {
      try {
        const response = await axiosInstance.get("/api/feeders");
        setFeeders(response.data);
      } catch (error) {
        console.error("Error fetching feeders:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchFeeders();
  }, []);

  const deleteFeeder = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/feeders`, { data: { id } })
        .then(() => navigate(0));
      // If successful, remove the deleted item from the list
      // Or, fetch the list again to reflect the changes
    } catch (error) {
      console.error("Error deleting feeder:", error);
      // Handle error, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Feeders")}</h2>
      <ul>
        {feeders.map((feeder) => (
          <li key={feeder.id} onClick={() => navigate(`/feeders/${feeder.id}`)}>
            <h3>{feeder.location.coordinates.join(" ")}</h3>
            <Link to={`/feeders/update/${feeder.id}`}>{t("Edit")}</Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteFeeder(feeder.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/feeders/create">{t("Create New Feeder")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default FeederView;

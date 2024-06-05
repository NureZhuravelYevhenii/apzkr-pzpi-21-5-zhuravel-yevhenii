import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";

interface SensorType {
  id: string;
  name: string;
}

const SensorTypeView: React.FC = () => {
  const [sensorTypes, setSensorTypes] = useState<SensorType[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchSensorTypes = async () => {
      try {
        const response = await axiosInstance.get("/api/sensorTypes");
        setSensorTypes(response.data);
      } catch (error) {
        console.error("Error fetching sensor types:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchSensorTypes();
  }, []);

  const deleteSensorType = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/sensorTypes`, { data: { id } })
        .then(() => navigate(0));
    } catch (error) {
      console.error("Error deleting sensor type:", error);
      // Handle error, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Sensor Types")}</h2>
      <ul>
        {sensorTypes.map((sensorType) => (
          <li key={sensorType.id}>
            <h3>{sensorType.name}</h3>
            <Link to={`/sensorTypes/update/${sensorType.id}`}>
              {t("Edit")}
            </Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteSensorType(sensorType.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/sensorTypes/create">{t("Create New Sensor Type")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default SensorTypeView;

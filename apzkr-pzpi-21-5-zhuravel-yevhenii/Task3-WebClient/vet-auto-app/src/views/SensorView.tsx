import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";

interface Sensor {
  id: string;
  type: string;
}

const SensorView: React.FC = () => {
  const [sensors, setSensors] = useState<Sensor[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchSensors = async () => {
      try {
        const response = await axiosInstance.get("/api/sensors");
        setSensors(response.data);
      } catch (error) {
        console.error("Error fetching sensors:", error);
        // Handle error, e.g., display error message
      }
    };
    fetchSensors();
  }, []);

  const deleteSensor = async (id: string) => {
    try {
      await axiosInstance
        .delete(`/api/sensors`, { data: { id } })
        .then(() => navigate(0));
    } catch (error) {
      console.error("Error deleting sensor:", error);
      // Handle error, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Sensors")}</h2>
      <ul>
        {sensors.map((sensor) => (
          <li key={sensor.id}>
            <h3>{sensor.type}</h3>
            <Link to={`/sensors/update/${sensor.id}`}>{t("Edit")}</Link>{" "}
            <button
              onClick={(event) => {
                event.stopPropagation();
                deleteSensor(sensor.id);
              }}
            >
              {t("Delete")}
            </button>
          </li>
        ))}
      </ul>
      <Link to="/sensors/create">{t("Create New Sensor")}</Link>
      <Link to="/">{t("Back")}</Link>
    </div>
  );
};

export default SensorView;

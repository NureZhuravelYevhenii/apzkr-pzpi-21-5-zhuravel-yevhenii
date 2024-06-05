import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { useTranslation } from "react-i18next";
import { MapContainer, Marker, TileLayer } from "react-leaflet";

interface Feeder {
  id: string;
  location: Location;
}

interface Location {
  coordinates: number[];
}

const FeederPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [feeder, setFeeder] = useState<Feeder | null>(null);
  const [popularMonth, setPopularMonth] = useState<string | null>(null);
  const [popularSeason, setPopularSeason] = useState<string | null>(null);
  const [popularDayOfWeek, setPopularDayOfWeek] = useState<string | null>(null);
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<Feeder>(`api/feeders/single?Id=${id}`)
      .then((response) => {
        setFeeder(response.data);
        axiosInstance
          .get<string>(`api/feeders/${id}/popular-month`)
          .then((response) => setPopularMonth(response.data))
          .catch((error) =>
            console.error("Error fetching feeding places:", error)
          );
        axiosInstance
          .get<string>(`api/feeders/${id}/popular-season`)
          .then((response) => setPopularSeason(response.data))
          .catch((error) =>
            console.error("Error fetching feeding places:", error)
          );
        axiosInstance
          .get<string>(`api/feeders/${id}/popular-day-of-week`)
          .then((response) => setPopularDayOfWeek(response.data))
          .catch((error) =>
            console.error("Error fetching feeding places:", error)
          );
      })
      .catch((error) => console.error("Error fetching feeder:", error));
  }, [id]);

  if (!feeder) {
    return <div>{t("Loading")}</div>;
  }

  return (
    <div>
      <h2>{t("Feeder Details")}</h2>
      <p>
        {t("ID")}: {feeder.id}
      </p>
      <p>
        {t("Location")}: {feeder.location.coordinates.join(", ")}
      </p>
      <p>
        {t("Popular-day")}: {popularDayOfWeek ?? "None"}
      </p>
      <p>
        {t("Popular-season")}: {popularSeason ?? "None"}
      </p>
      <p>
        {t("Popular-month")}: {popularMonth ?? "None"}
      </p>
      <MapContainer
        center={[
          feeder.location.coordinates[0],
          feeder.location.coordinates[1],
        ]}
        zoom={13}
        scrollWheelZoom={false}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <Marker
          position={[
            feeder.location.coordinates[0],
            feeder.location.coordinates[1],
          ]}
        />
      </MapContainer>
    </div>
  );
};

export default FeederPage;

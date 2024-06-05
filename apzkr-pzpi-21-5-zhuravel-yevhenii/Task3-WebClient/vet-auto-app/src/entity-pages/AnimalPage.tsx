import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import axiosInstance from "../axios-instance";
import { MapContainer, Marker, Polyline, TileLayer } from "react-leaflet";
import { LatLngTuple } from "leaflet";
import "./css/Map.css";
import { useTranslation } from "react-i18next";

interface Animal {
  id: string;
  name: string;
  typeId: string;
  animalCenterId: string;
}

interface FeedingPlace {
  feedingDate: Date;
  coordinates: LatLngTuple;
}

const AnimalPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [animal, setAnimal] = useState<Animal | null>(null);
  const [feedingPlaces, setFeedingPlaces] = useState<FeedingPlace[]>([]);
  const [averageEatenFood, setAverageEatenFood] = useState<number>(0);
  const [noEat, setNoEat] = useState<Date>(new Date(0));
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<Animal>(`api/animals/single?Id=${id}`)
      .then((response) => setAnimal(response.data))
      .catch((error) => console.error("Error fetching animal:", error));

    axiosInstance
      .get<FeedingPlace[]>(`api/animals/feeding-places/${id}`)
      .then((response) => setFeedingPlaces(response.data))
      .catch((error) => console.error("Error fetching animal:", error));

    axiosInstance
      .get<number>(`api/animals/average-eaten-food/${id}`)
      .then((response) => setAverageEatenFood(response.data))
      .catch((error) => console.error("Error fetching animal:", error));

    axiosInstance
      .get<Date>(`api/animals/no-eat/${id}`)
      .then((response) => setNoEat(response.data))
      .catch((error) => console.error("Error fetching animal:", error));
  }, [id]);

  if (!animal) {
    return <div>{t("Loading...")}</div>;
  }

  return (
    <div>
      <h2>{t("Animal Details")}</h2>
      <p>
        {t("ID")}: {animal.id}
      </p>
      <p>
        {t("Name")}: {animal.name}
      </p>
      <p>
        {t("Type ID")}: {animal.typeId}
      </p>
      <p>
        {t("Animal Center ID")}: {animal.animalCenterId}
      </p>
      <p>
        {t("Average eaten food")}: {averageEatenFood}
      </p>
      <p>
        {t("No eat period")}: {noEat.toString()}
      </p>
      {feedingPlaces.length ? (
        <MapContainer
          center={feedingPlaces[0].coordinates}
          zoom={13}
          scrollWheelZoom={false}
        >
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
          {feedingPlaces.map(
            (v, i, arr) =>
              i && (
                <Polyline
                  positions={[v.coordinates, arr[i - 1].coordinates]}
                  color="red"
                />
              )
          )}
          {feedingPlaces.map((v) => (
            <Marker position={v.coordinates} />
          ))}
        </MapContainer>
      ) : (
        <p>{t("There is no feeding places to show(")}</p>
      )}
    </div>
  );
};

export default AnimalPage;

import React, { useEffect, useState } from "react";
import axiosInstance from "../axios-instance";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface AnimalFeederCreationFormData {
  animalId: string;
  feederId: string;
  feedDate: string;
  amountOfFood: number;
}

interface Animal {
  id: string;
  name: string;
}

interface Feeder {
  id: string;
}

const AnimalFeederCreationForm: React.FC = () => {
  const [formData, setFormData] = useState<AnimalFeederCreationFormData>({
    animalId: "",
    feederId: "",
    feedDate: "",
    amountOfFood: 0,
  });

  const [animals, setAnimals] = useState<Animal[]>([]);
  const [feeders, setFeeders] = useState<Feeder[]>([]);
  const navigate = useNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    axiosInstance
      .get<Animal[]>(`api/animals`)
      .then((res) => setAnimals(res.data));

    axiosInstance
      .get<Feeder[]>(`api/feeders`)
      .then((res) => setFeeders(res.data));
  }, []);

  const handleChange = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    formData.feedDate = new Date(Date.now()).toISOString();

    try {
      await axiosInstance
        .post("/api/animals/create-animal-feeder", formData)
        .then(() => navigate("/animal-feeders"));
      // Handle successful form submission
    } catch (error) {
      // Handle error
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <select
        name="animalId"
        value={formData.animalId}
        onChange={handleChange}
        defaultValue=""
      >
        <option value="">{t("Select animal")}</option>
        {animals.map((animal) => (
          <option value={animal.id} key={animal.id}>
            {animal.name}
          </option>
        ))}
      </select>
      <select
        name="feederId"
        value={formData.feederId}
        onChange={handleChange}
        defaultValue=""
      >
        <option value="">{t("Select feeder id")}</option>
        {feeders.map((feeder) => (
          <option value={feeder.id} key={feeder.id}>
            {feeder.id}
          </option>
        ))}
      </select>
      <input
        name="amountOfFood"
        value={formData.amountOfFood}
        onChange={handleChange}
        type="number"
      />
      <button type="submit">{t("Submit")}</button>
    </form>
  );
};

export default AnimalFeederCreationForm;

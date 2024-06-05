import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { accessToken } from "../constants/AuthenticationConstants";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface RegistrationFormValues {
  name: string;
  email: string;
  password: string;
}

const RegistrationForm = () => {
  const [registrationFormValues, setRegistrationFormValues] =
    useState<RegistrationFormValues>({
      name: "",
      email: "",
      password: "",
    });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setRegistrationFormValues({ ...registrationFormValues, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .post<{ accessToken: string }>(
          "/api/animalCenter/register",
          registrationFormValues
        )
        .then((res) => res.data)
        .then((data) => {
          localStorage.setItem(accessToken, data.accessToken);
          navigate("/");
        });
      // Redirect or display success message
    } catch (error) {
      console.error("Registration failed:", error);
      // Handle registration failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Registration")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">{t("Name")}</label>
          <input
            type="text"
            id="name"
            name="name"
            value={registrationFormValues.name}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="email">{t("Email")}</label>
          <input
            type="email"
            id="email"
            name="email"
            value={registrationFormValues.email}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="password">{t("Password")}</label>
          <input
            type="password"
            id="password"
            name="password"
            value={registrationFormValues.password}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Register")}</button>
      </form>
    </div>
  );
};

export default RegistrationForm;

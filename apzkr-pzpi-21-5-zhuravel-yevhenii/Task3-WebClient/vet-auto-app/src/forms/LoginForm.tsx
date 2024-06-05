import React, { useState } from "react";
import axiosInstance from "../axios-instance";
import { accessToken } from "../constants/AuthenticationConstants";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

interface LoginFormValues {
  login: string;
  password: string;
}

const LoginForm = () => {
  const [loginFormValues, setLoginFormValues] = useState<LoginFormValues>({
    login: "",
    password: "",
  });
  const navigate = useNavigate();
  const { t } = useTranslation();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setLoginFormValues({ ...loginFormValues, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await axiosInstance
        .post<{ accessToken: string }>(
          "/api/animalCenter/login",
          loginFormValues
        )
        .then((res) => res.data)
        .then((data) => {
          localStorage.setItem(accessToken, data.accessToken);
          navigate("/");
        });
      // Redirect or display success message
    } catch (error) {
      console.error("Login failed:", error);
      // Handle login failure, e.g., display error message
    }
  };

  return (
    <div>
      <h2>{t("Login")}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="login">{t("Username or Email")}</label>
          <input
            type="text"
            id="login"
            name="login"
            value={loginFormValues.login}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="password">{t("Password")}</label>
          <input
            type="password"
            id="password"
            name="password"
            value={loginFormValues.password}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("Login")}</button>
      </form>
    </div>
  );
};

export default LoginForm;

import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import AnimalsView from "./views/AnimalView.tsx";
import AnimalCenterView from "./views/AnimalCenterView.tsx";
import AnimalCenterUpdateForm from "./forms/AnimalCenterUpdateForm.tsx";
import AnimalCenterCreationForm from "./forms/AnimalCenterCreationForm.tsx";
import AnimalUpdateForm from "./forms/AnimalUpdateForm.tsx";
import AnimalCreationForm from "./forms/AnimalCreationForm.tsx";
import SensorTypeView from "./views/SensorTypeView.tsx";
import SensorTypeUpdateForm from "./forms/SensorTypeUpdateForm.tsx";
import SensorTypeCreationForm from "./forms/SensorTypeCreationForm.tsx";
import SensorView from "./views/SensorView.tsx";
import SensorUpdateForm from "./forms/SensorUpdateForm.tsx";
import SensorCreationForm from "./forms/SensorCreationForm.tsx";
import FeederView from "./views/FeederView.tsx";
import FeederUpdateForm from "./forms/FeederUpdateForm.tsx";
import FeederCreationForm from "./forms/FeederCreationForm.tsx";
import AnimalTypeView from "./views/AnimalTypeView.tsx";
import AnimalTypeUpdateForm from "./forms/AnimalTypeUpdateForm.tsx";
import AnimalTypeCreationForm from "./forms/AnimalTypeCreationForm.tsx";
import AnimalFeederView from "./views/AnimalFeederView.tsx";
import AnimalFeederCreationForm from "./forms/AnimalFeederCreationForm.tsx";
import AnimalFeederUpdateForm from "./forms/AnimalFeederUpdateForm.tsx";
import LoginForm from "./forms/LoginForm.tsx";
import RegistrationForm from "./forms/RegistrationForm.tsx";
import AnimalPage from "./entity-pages/AnimalPage.tsx";
import "./i18n";
import AppPage from "./entity-pages/AppPage.tsx";
import FeederPage from "./entity-pages/FeederPage.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <AppPage />,
      },
      {
        path: "animals/create",
        element: <AnimalCreationForm />,
      },
      {
        path: "animals/update/:animalId",
        element: <AnimalUpdateForm />,
      },
      {
        path: "animals",
        element: <AnimalsView />,
      },
      {
        path: "animals/:id",
        element: <AnimalPage />,
      },
      {
        path: "animal-centers/create",
        element: <AnimalCenterCreationForm />,
      },
      {
        path: "animal-centers/update/:animalCenterId",
        element: <AnimalCenterUpdateForm />,
      },
      {
        path: "animal-centers",
        element: <AnimalCenterView />,
      },
      {
        path: "animal-feeders/create",
        element: <AnimalFeederCreationForm />,
      },
      {
        path: "animal-feeders/update/:animalFeederid",
        element: <AnimalFeederUpdateForm />,
      },
      {
        path: "animal-feeders",
        element: <AnimalFeederView />,
      },
      {
        path: "animal-types/create",
        element: <AnimalTypeCreationForm />,
      },
      {
        path: "animal-types/update/:animalTypeid",
        element: <AnimalTypeUpdateForm />,
      },
      {
        path: "animal-types",
        element: <AnimalTypeView />,
      },
      {
        path: "feeders/create",
        element: <FeederCreationForm />,
      },
      {
        path: "feeders/update/:feederId",
        element: <FeederUpdateForm />,
      },
      {
        path: "feeders",
        element: <FeederView />,
      },
      {
        path: "feeders/:id",
        element: <FeederPage />,
      },
      {
        path: "sensors/create",
        element: <SensorCreationForm />,
      },
      {
        path: "sensors/update/:sensorId",
        element: <SensorUpdateForm />,
      },
      {
        path: "sensors",
        element: <SensorView />,
      },
      {
        path: "sensor-types/create",
        element: <SensorTypeCreationForm />,
      },
      {
        path: "sensor-types/update/:sensorTypeId",
        element: <SensorTypeUpdateForm />,
      },
      {
        path: "sensor-types",
        element: <SensorTypeView />,
      },
      {
        path: "login",
        element: <LoginForm />,
      },
      {
        path: "register",
        element: <RegistrationForm />,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);

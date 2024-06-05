import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";

function AppPage() {
  const { t } = useTranslation();

  return (
    <>
      <Link style={{ display: "block" }} to="/animals">
        {t("Animals")}
      </Link>
      <Link style={{ display: "block" }} to="/animal-centers">
        {t("Animal Centers")}
      </Link>
      <Link style={{ display: "block" }} to="/feeders">
        {t("Feeders")}
      </Link>
      <Link style={{ display: "block" }} to="/sensors">
        {t("Sensors")}
      </Link>
      <Link style={{ display: "block" }} to="/animal-types">
        {t("Animal Types")}
      </Link>
      <Link style={{ display: "block" }} to="/animal-feeders">
        {t("Animal Feeders")}
      </Link>
      <Link style={{ display: "block" }} to="/login">
        {t("Login")}
      </Link>
      <Link style={{ display: "block" }} to="/register">
        {t("Register")}
      </Link>
    </>
  );
}

export default AppPage;

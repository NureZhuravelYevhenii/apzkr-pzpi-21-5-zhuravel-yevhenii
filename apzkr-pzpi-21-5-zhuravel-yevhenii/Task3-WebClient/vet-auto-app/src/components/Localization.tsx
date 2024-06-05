import { useEffect } from "react";
import { useTranslation } from "react-i18next";

const Localization = () => {
  const { i18n } = useTranslation();

  const handle = (lang: string) => {
    i18n.changeLanguage(lang);
    localStorage.setItem("lang", lang);
  };

  useEffect(() => {
    if (localStorage.getItem("lang")) {
      i18n.changeLanguage(localStorage.getItem("lang")!);
    }
  }, []);

  return (
    <div>
      <button onClick={() => handle("en")}>en</button>
      <button onClick={() => handle("uk")}>uk</button>
    </div>
  );
};

export default Localization;

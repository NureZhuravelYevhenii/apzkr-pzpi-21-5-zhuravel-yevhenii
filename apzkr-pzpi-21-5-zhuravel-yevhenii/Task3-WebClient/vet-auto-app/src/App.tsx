import { Outlet } from "react-router-dom";
import Localization from "./components/Localization";

const App = () => {
  return (
    <>
      <Localization />
      <Outlet />
    </>
  );
};

export default App;

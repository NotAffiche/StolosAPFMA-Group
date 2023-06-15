import React from "react";
import { Outlet } from "react-router-dom";
import DriversNavigation from "../components/DriversNavigation";

const DriversRootLayout = () => {
  return (
    <>
      <DriversNavigation />
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default DriversRootLayout;

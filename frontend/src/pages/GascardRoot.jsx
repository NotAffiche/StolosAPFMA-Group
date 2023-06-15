import React from "react";
import { Outlet } from "react-router-dom";
import GascardNavigation from "../components/GascardNavigation";

const GascardRootLayout = () => {
  return (
    <>
      <GascardNavigation />
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default GascardRootLayout;

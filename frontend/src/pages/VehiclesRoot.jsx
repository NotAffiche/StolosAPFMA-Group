import React from 'react';
import { Outlet } from 'react-router-dom';
import VehiclesNavigation from '../components/VehiclesNavigation';

const VehiclesRootLayout = () => {
  return (
    <>
      <VehiclesNavigation />
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default VehiclesRootLayout;

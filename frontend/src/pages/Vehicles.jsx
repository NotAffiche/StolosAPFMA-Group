import React, { Suspense } from 'react';
import { useLoaderData, defer, Await, json } from 'react-router-dom';
import { VehicleList } from '../components/Lists';

const VehiclesPage = () => {
  const { vehicles } = useLoaderData();

  return (
    <Suspense fallback={<p>Laden...</p>}>
      <Await resolve={vehicles}>
        {loadedVehicles => <VehicleList vehicles={loadedVehicles} />}
      </Await>
    </Suspense>
  );
};

export default VehiclesPage;

const loadVehicles = async () => {
  const response = await fetch(process.env.REACT_APP_VEHICLE_URL);
  if (!response.ok) {
    return json({ message: 'Voertuigen ophalen mislukt.' }, { status: 500 });
  } else {
    const res = await response.json();
    return res;
  }
};

export function loader() {
  return defer({
    vehicles: loadVehicles(),
  });
}

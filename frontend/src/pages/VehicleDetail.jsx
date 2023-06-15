import React, { Suspense } from 'react';
import {
  useRouteLoaderData,
  json,
  defer,
  redirect,
  Await,
} from 'react-router-dom';
import VehicleItem from '../components/VehicleItem';
import { VehicleList } from '../components/Lists';

const VehicleDetailPage = () => {
  const { vehicles, vehicle, drivers } = useRouteLoaderData('vehicle-detail');
  return (
    <>
      <Suspense>
        <Await resolve={[vehicle, drivers]}>
          {([loadedVehicle, loadedDrivers]) => (
            <VehicleItem vehicle={loadedVehicle} drivers={loadedDrivers} />
          )}
        </Await>
      </Suspense>
      <Suspense>
        <Await resolve={vehicles}>
          {loadedVehicles => <VehicleList vehicles={loadedVehicles} />}
        </Await>
      </Suspense>
    </>
  );
};

export default VehicleDetailPage;

async function loadVehicle(vin) {
  const response = await fetch(process.env.REACT_APP_VEHICLE_URL + vin);
  if (!response.ok) {
    throw json(
      { message: 'Kon de details van het voertuig niet ophalen' },
      { status: 500 }
    );
  } else {
    const resData = await response.json();
    return resData;
  }
}

async function loadVehicles() {
  const response = await fetch(process.env.REACT_APP_VEHICLE_URL);

  if (!response.ok) {
    return json({ message: 'Voertuigen ophalen mislukt.' }, { status: 500 });
  } else {
    const resData = await response.json();
    return resData.vehicles;
  }
}

async function loadDrivers() {
  const response = await fetch(process.env.REACT_APP_DRIVER_URL);

  if (!response.ok) {
    return json({ message: 'Bestuurders ophalen mislukt.' }, { status: 500 });
  } else {
    const resData = await response.json();
    return resData;
  }
}

export async function loader({ req, params }) {
  const { vin } = params;
  return defer({
    vehicle: await loadVehicle(vin),
    vehicles: loadVehicles(),
    drivers: await loadDrivers(),
  });
}

export async function action({ params, request }) {
  const response = await fetch(process.env.REACT_APP_VEHICLE_URL + params.vin, {
    method: request.method,
  });
  if (!response.ok) {
    throw json({ message: 'Error.' }, { status: 500 });
  }
  return redirect('/vehicles');
}

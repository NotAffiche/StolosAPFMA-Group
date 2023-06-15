import React, { Suspense } from 'react';
import {
  Await,
  useRouteLoaderData,
  json,
  redirect,
  defer,
} from 'react-router-dom';
import VehicleForm from '../components/VehicleForm';

const EditVehiclePage = () => {
  const { vehicle, drivers } = useRouteLoaderData('vehicle-detail');
  return (
    <Suspense>
      <Await resolve={[vehicle, drivers]}>
        {loadVehicle => (
          <VehicleForm
            vehicle={loadVehicle[0]}
            drivers={loadVehicle[1]}
            method="put"
          />
        )}
      </Await>
    </Suspense>
  );
};

export default EditVehiclePage;

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

async function loadDrivers() {
  const response = await fetch(process.env.REACT_APP_DRIVER_URL);

  if (!response.ok) {
    return json({ message: 'Bestuurders ophalen mislukt.' }, { status: 500 });
  } else {
    const resData = await response.json();
    return resData.drivers;
  }
}

export async function loader({ req, params }) {
  const { vin } = params;
  return defer({
    vehicle: await loadVehicle(vin),
    drivers: loadDrivers(),
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

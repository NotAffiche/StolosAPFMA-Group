import React, { Suspense } from 'react';
import {
  useRouteLoaderData,
  json,
  defer,
  Await,
  redirect,
} from 'react-router-dom';
import DriverItem from '../components/DriverItem';
import DriverList from '../components/Lists';

const DriverDetailPage = () => {
  const { drivers, driver, vehicles, gascards } =
    useRouteLoaderData('driver-detail');
  return (
    <>
      <Suspense>
        <Await resolve={[driver, vehicles, gascards]}>
          {([loadedDriver, loadedVehicles, loadedGascards]) => (
            <DriverItem
              driver={loadedDriver}
              vehicles={loadedVehicles}
              gascards={loadedGascards}
            />
          )}
        </Await>
      </Suspense>
      <Suspense>
        <Await resolve={drivers}>
          {loadedDrivers => <DriverList drivers={loadedDrivers} />}
        </Await>
      </Suspense>
    </>
  );
};

export default DriverDetailPage;

async function loadDriver(driverID) {
  const response = await fetch(
    process.env.REACT_APP_DRIVER_URL + driverID.toString()
  );
  if (!response.ok) {
    throw json(
      { message: 'Kon de details van de chauffeur niet ophalen' },
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
    return resData;
  }
}

async function loadGascards() {
  const response = await fetch(process.env.REACT_APP_GASCARD_URL);

  if (!response.ok) {
    return json({ message: 'Tankkaarten ophalen mislukt.' }, { status: 500 });
  } else {
    const resData = await response.json();
    return resData;
  }
}

async function loadDrivers() {
  const response = await fetch(process.env.REACT_APP_DRIVER_URL);

  if (!response.ok) {
    return json({ message: 'Chauffeurs ophalen mislukt.' }, { status: 500 });
  } else {
    const resData = await response.json();
    return resData.drivers;
  }
}

export async function loader({ req, params }) {
  const { driverID } = params;
  return defer({
    driver: await loadDriver(driverID.toString()),
    drivers: loadDrivers(),
    vehicles: await loadVehicles(),
    gascards: await loadGascards(),
  });
}

export async function action({ params, request }) {
  const response = await fetch(
    process.env.REACT_APP_DRIVER_URL + params.driverID.toString(),
    {
      method: request.method,
    }
  );
  if (!response.ok) {
    throw json({ message: 'Kon bestuurder niet verwijderen' }, { status: 500 });
  }
  return redirect('/drivers');
}

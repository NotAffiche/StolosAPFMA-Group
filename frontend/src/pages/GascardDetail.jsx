import React, { Suspense } from 'react';
import {
  useRouteLoaderData,
  json,
  defer,
  redirect,
  Await,
} from 'react-router-dom';
import GascardItem from '../components/GascardItem';
import { GascardList } from '../components/Lists';

const GascardDetail = () => {
  const { gascard, gascards, drivers } = useRouteLoaderData('gascard-detail');
  return (
    <>
      <Suspense>
        <Await resolve={[gascard, drivers]}>
          {([loadedGascard, loadedDrivers]) => (
            <GascardItem gascard={loadedGascard} drivers={loadedDrivers} />
          )}
        </Await>
      </Suspense>
      <Suspense>
        <Await resolve={gascards}>
          {loadedGascards => <GascardList gascards={loadedGascards} />}
        </Await>
      </Suspense>
    </>
  );
};

export default GascardDetail;

async function loadGascard(cardNumber) {
  const response = await fetch(process.env.REACT_APP_GASCARD_URL + cardNumber);
  if (!response.ok) {
    throw json(
      { message: 'Kon de details van de tankkaart niet ophalen' },
      { status: 500 }
    );
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
    return resData.gascards;
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
  const { cardNumber } = params;
  return defer({
    gascard: await loadGascard(cardNumber),
    gascards: loadGascards(),
    drivers: await loadDrivers(),
  });
}

export async function action({ params, request }) {
  const response = await fetch(
    process.env.REACT_APP_GASCARD_URL + params.cardNumber,
    {
      method: request.method,
    }
  );
  if (!response.ok) {
    throw json({ message: 'Error' }, { status: 500 });
  }
  return redirect('/gascards');
}

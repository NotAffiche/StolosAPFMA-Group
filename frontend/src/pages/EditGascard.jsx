import React, { Suspense } from 'react';
import {
  Await,
  useRouteLoaderData,
  json,
  redirect,
  defer,
} from 'react-router-dom';
import GascardForm from '../components/GascardForm';

const EditGascardPage = () => {
  const { gascard, drivers } = useRouteLoaderData('gascard-detail');
  return (
    <Suspense>
      <Await resolve={[gascard, drivers]}>
        {loadGascard => (
          <GascardForm
            gascard={loadGascard[0]}
            drivers={loadGascard[1]}
            method="put"
          />
        )}
      </Await>
    </Suspense>
  );
};

export default EditGascardPage;

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
  const { cardNumber } = params;
  return defer({
    vehicle: await loadGascard(cardNumber),
    drivers: loadDrivers(),
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
    throw json({ message: 'Error.' }, { status: 500 });
  }
  return redirect('/gascards');
}

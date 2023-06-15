import React, { useState } from 'react';
import Select from 'react-select';
import foto from '../images/user.png';
import classes from '../css/Edit.module.css';
import EditCard from './EditCard';
import { Licenses } from './DataLicenses';
import {
  Form,
  json,
  redirect,
  useActionData,
  useNavigate,
  useNavigation,
} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBan, faFloppyDisk } from '@fortawesome/free-solid-svg-icons';

const DriverForm = ({ method, driver, vehicles, gascards }) => {
  const [input, setInput] = useState();
  const data = useActionData();
  const navigate = useNavigate();
  const navigation = useNavigation();

  let currentLicenses = [];
  if (driver) {
    driver.licenses.forEach(license => {
      currentLicenses.push({
        value: license,
        label: license,
      });
    });
  }

  const availableVehicles = [];
  if (method === 'put') {
    if (driver && driver.vehicleVin !== null) {
      const currentVehicle = vehicles.filter(
        vehicle => vehicle.vin === driver.vehicleVin
      )[0];
      availableVehicles.push({
        value: driver.vehicleVin,
        label: `${currentVehicle.brandModel} - ${currentVehicle.licensePlate} - Huidig voertuig`,
      });
      availableVehicles.push({
        value: 0,
        label: 'X Voertuig verwijderen',
      });
    }

    vehicles
      .filter(vehicle => vehicle.driverId === null)
      .forEach(selectedVehicles => {
        availableVehicles.push({
          value: selectedVehicles.vin,
          label: `${selectedVehicles.brandModel} ${selectedVehicles.licensePlate}`,
        });
      });
  }

  const availableGascards = [];
  if (method === 'put') {
    if (driver && driver.gasCardNum !== null) {
      const currentGascard = gascards.filter(
        gascard => gascard.cardNumber === driver.gasCardNum
      )[0];
      availableGascards.push({
        value: driver.gasCardNum,
        label: `${currentGascard.cardNumber} - Huidige kaart`,
      });
      availableGascards.push({
        value: 0,
        label: 'X Kaart verwijderen',
      });
    }

    gascards
      .filter(gascard => gascard.driverId === null)
      .forEach(selectedGascards => {
        availableGascards.push({
          value: selectedGascards.cardNumber,
          label: `${selectedGascards.cardNumber}`,
        });
      });
  }

  const isSubmitting = navigation.state === 'submitting';

  function cancelHandler() {
    navigate('..');
  }

  const changeHandler = e => {
    setInput({ ...input, [e.target.name]: e.target.value });
  };

  return (
    <EditCard>
      <Form method={method} className={classes.table}>
        {data && data.errors && (
          <ul>
            {Object.values(data.errors).map(err => (
              <li key={err}>{err}</li>
            ))}
          </ul>
        )}
        <img className={classes.tablePicture} src={foto} alt="notAvailable" />
        <div className={classes.data}>
          <label htmlFor="firstname">Voornaam:</label>
          <input
            id="firstname"
            type="text"
            name="firstName"
            required
            defaultValue={driver ? driver.firstName : ''}
            onChange={changeHandler}
          />
          <label htmlFor="lastname">Familienaam:</label>
          <input
            id="lastname"
            type="text"
            name="lastName"
            required
            defaultValue={driver ? driver.lastName : ''}
            onChange={changeHandler}
          />
          <label htmlFor="birthdate">Geboortedatum:</label>
          <input
            id="birthdate"
            type="date"
            name="birthDate"
            required
            defaultValue={driver ? driver.birthDate.split('T')[0] : ''}
            onChange={changeHandler}
          />
          <label htmlFor="natregnumber">Rijksregisternummer:</label>
          <input
            id="natregnumber"
            type="text"
            name="natregnumber"
            defaultValue={driver ? driver.natRegNum : ''}
            onChange={changeHandler}
          />
          <label htmlFor="address">Adres:</label>
          <input
            id="address"
            type="text"
            name="address"
            defaultValue={driver ? driver.address : ''}
            onChange={changeHandler}
          />
          <label htmlFor="licenses">Rijbewijzen: </label>
          <Select
            id="licenses"
            name="licenses"
            isMulti
            options={Licenses}
            defaultValue={driver ? currentLicenses : ''}
          />
          <label htmlFor="vehicle">Voertuig:</label>
          <Select
            id="vehicle"
            name="vehicle"
            options={availableVehicles}
            defaultValue={
              driver && driver.vehicleVin
                ? availableVehicles[0]
                : method === 'post'
                ? { value: 0, label: 'Maak eerst de bestuurder aan' }
                : ''
            }
            isDisabled={method === 'post' ? true : false}
          />
          <label htmlFor="gascard">Tankkaart:</label>
          <Select
            id="gascard"
            name="gascard"
            options={availableGascards}
            defaultValue={
              driver && driver.gasCardNum
                ? availableGascards[0]
                : method === 'post'
                ? { value: 0, label: 'Maak eerst de bestuurder aan' }
                : ''
            }
            isDisabled={method === 'post' ? true : false}
          />
        </div>
        <div className={classes.buttons}>
          <p></p>
          <button disabled={isSubmitting} className={classes.save}>
            <FontAwesomeIcon icon={faFloppyDisk} /> Opslaan
          </button>
          <button onClick={cancelHandler} className={classes.cancel}>
            <FontAwesomeIcon icon={faBan} /> Annuleren
          </button>
        </div>
      </Form>
    </EditCard>
  );
};

export default DriverForm;

export async function action({ request, params }) {
  const method = request.method;
  const data = await request.formData();
  const driverData = {
    firstName: data.get('firstName'),
    lastName: data.get('lastName'),
    birthDate: data.get('birthDate'),
    natRegNum: data.get('natregnumber'),
    address: data.get('address'),
    licenses: data.getAll('licenses'),
    vehicleVin: data.get('vehicle') === '0' ? null : data.get('vehicle'),
    gasCardNum: data.get('gascard') === '0' ? null : data.get('gascard'),
  };

  let url = process.env.REACT_APP_DRIVER_URL;

  if (method === 'PUT') {
    const { driverID } = params;
    driverData.driverID = driverID;
  }

  const response = await fetch(url, {
    method: method,
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(driverData),
  });

  if (response.status === 422) {
    return response;
  }

  if (!response.ok) {
    throw json({ message: 'Kon de bestuurder niet opslaan' }, { status: 500 });
  }
  return redirect('/drivers');
}

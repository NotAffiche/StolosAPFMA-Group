import React, { useState } from 'react';
import Select from 'react-select';
import foto from '../images/notAvailable.png';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBan, faFloppyDisk } from '@fortawesome/free-solid-svg-icons';
import classes from '../css/Edit.module.css';

import EditCard from './EditCard';
import { FuelTypes } from './DataFuelTypes';
import {
  Form,
  json,
  redirect,
  useActionData,
  useNavigate,
  useNavigation,
} from 'react-router-dom';

const GascardForm = ({ method, gascard, drivers }) => {
  const data = useActionData();
  const navigate = useNavigate();
  const navigation = useNavigation();

  let currentFuelTypes = [];
  if (gascard) {
    gascard.fuelTypes.forEach(fueltype => {
      currentFuelTypes.push({
        value: fueltype,
        label: FuelTypes.filter(type => type.value === fueltype)[0].label,
      });
    });
  }

  const availableDrivers = [];
  if (method === 'put') {
    if (gascard && gascard.driverId !== null) {
      const currentDriver = drivers.filter(
        driver => driver.gasCardNum === gascard.cardNumber
      )[0];
      availableDrivers.push({
        value: gascard.driverId,
        label: `${currentDriver.firstName} ${currentDriver.lastName} - Huidige bestuurder`,
      });
      availableDrivers.push({
        value: 0,
        label: 'X Bestuurder verwijderen',
      });
    }

    drivers
      .filter(driver => driver.gasCardNum === null)
      .forEach(selectedDrivers => {
        availableDrivers.push({
          value: selectedDrivers.driverID,
          label: `${selectedDrivers.firstName} ${selectedDrivers.lastName}`,
        });
      });
  }

  const isSubmitting = navigation.state === 'submitting';

  function cancelHandler() {
    navigate('..');
  }
  const [input, setInput] = useState();

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
        <img src={foto} alt="notAvailable" />
        <div className={classes.data}>
          <label htmlFor="cardnumber">Kaartnummer:</label>
          <input
            id="cardnumber"
            type="text"
            name="cardnumber"
            defaultValue={gascard ? gascard.cardNumber : ''}
            readOnly={gascard ? true : false}
            required
          />
          <label htmlFor="pincode">Pincode:</label>
          <input
            id="pincode"
            type="text"
            name="pincode"
            defaultValue={gascard ? gascard.pincode : ''}
            onChange={changeHandler}
          />
          <label htmlFor="expiry">Vervaldatum:</label>
          <input
            id="expiry"
            type="date"
            name="expiry"
            required
            defaultValue={gascard ? gascard.expiringDate.split('T')[0] : ''}
            onChange={changeHandler}
          />
          <label htmlFor="fueltypes">Brandstoftypes:</label>
          <Select
            id="fueltypes"
            name="fueltypes"
            isMulti
            options={FuelTypes}
            defaultValue={gascard ? currentFuelTypes : ''}
          />
          <label htmlFor="blocked">Geblokkeerd:</label>
          <select id="blocked" name="blocked">
            <option value="false" defaultChecked>
              Nee
            </option>
            <option value="true">Ja</option>
          </select>
          <label htmlFor="driverId">Bestuurder:</label>
          <Select
            id="driverId"
            name="driverId"
            options={availableDrivers}
            defaultValue={
              gascard && gascard.driverId
                ? availableDrivers[0]
                : method === 'post'
                ? { value: 0, label: 'Maak eerst de tankkaart aan' }
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

export default GascardForm;

export async function action({ request, params }) {
  const method = request.method;
  const data = await request.formData();
  const gascardData = {
    cardNumber: data.get('cardnumber'),
    pincode: data.get('pincode'),
    fuelTypes: data.getAll('fueltypes'),
    blocked: data.get('blocked') === 'true',
    expiringDate: data.get('expiry'),
    driverId: data.get('driverId') === '0' ? null : data.get('driverId'),
  };

  let url = process.env.REACT_APP_GASCARD_URL;

  const response = await fetch(url, {
    method: method,
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(gascardData),
  });

  if (response.status === 422) {
    return response;
  }

  if (!response.ok) {
    throw json({ message: 'Kon de tankkaart niet opslaan' }, { status: 500 });
  }

  return redirect('/gascards');
}

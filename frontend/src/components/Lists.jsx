import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import vehicleClasses from '../css/VehicleList.module.css';
import driverClasses from '../css/DriverList.module.css';
import gasCardClasses from '../css/GascardList.module.css';
import logo from '../images/logoLong.png';
import { faEye, faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const DriverList = ({ drivers }) => {
  const [query,setQuery] = useState("");

  return (
    <header className={driverClasses.container}>
      <article className={driverClasses.headerPage}>
        <img src={logo} alt="logo"></img>
        <h1>BESTUURDERS</h1>
        <img src={logo} alt="logo"></img>
      </article>
      <hr className={driverClasses.line} />
      <input type="text" className={driverClasses.search} placeholder="Zoeken..." onChange={l => setQuery(l.target.value)}/>
      <div>
        <ul className={driverClasses.columns}>
        {drivers.filter(driver => driver.firstName.toLowerCase().includes(query.toLowerCase().trim())||driver.lastName.toLowerCase().includes(query.toLowerCase().trim())).map((driver) => (
          <li key={driver.driverID.toString()}>
            <Link to={driver.driverID.toString()}>
              {driver.firstName} {driver.lastName}
              <span>
                <FontAwesomeIcon
                  title="bekijken"
                  className={driverClasses.eye}
                  icon={faEye}
                />
                &nbsp;&nbsp;&nbsp;|
                <Link
                  className={driverClasses.update}
                  to={`/drivers/${driver.driverID}/edit`}
                >
                  <FontAwesomeIcon title="bewerken" icon={faPen} />
                </Link>
              </span>
            </Link>
          </li>
        ))}
      </ul>
    </div>
    </header>
  );
};

export const VehicleList = ({ vehicles }) => {
  const [query,setQuery] = useState("");
  return (
    <header className={vehicleClasses.container}>
      <article className={vehicleClasses.headerPage}>
        <img src={logo} alt="logo"></img>
        <h1>VOERTUIGEN</h1>
        <img src={logo} alt="logo"></img>
      </article>

      <hr className={vehicleClasses.line} />
      <input type="text" className={vehicleClasses.search} placeholder="Zoeken..." onChange={l => setQuery(l.target.value)}/>
      <ul className={vehicleClasses.columns}>
        {vehicles.filter(vehicle => vehicle.licensePlate.toLowerCase().includes(query.toLowerCase().trim())).map((vehicle) => (
          <li key={vehicle.vin}>
            <Link to={vehicle.vin}>
              {vehicle.licensePlate}
              <span>
                <FontAwesomeIcon
                  title="bekijken"
                  className={vehicleClasses.eye}
                  icon={faEye}
                />
                &nbsp;&nbsp;&nbsp;|
                <Link
                  className={vehicleClasses.update}
                  to={`/vehicles/${vehicle.vin}/edit`}
                >
                  <FontAwesomeIcon title="bewerken" icon={faPen} />
                </Link>
              </span>
            </Link>
          </li>
        ))}
      </ul>
    </header>
  );
};

export const GascardList = ({ gascards }) => {
  const [query,setQuery] = useState("");
  return (
    <header className={gasCardClasses.container}>
      <article className={gasCardClasses.headerPage}>
        <img src={logo} alt="logo"></img>
        <h1>BRANDSTOFKAARTEN</h1>
        <img src={logo} alt="logo" />
      </article>

      <hr className={gasCardClasses.line} />
      <input type="text" className={gasCardClasses.search} placeholder="Zoeken..." onChange={l => setQuery(l.target.value)}/>
      <ul className={gasCardClasses.columns}>
      {gascards.filter(gascard => gascard.cardNumber.toLowerCase().includes(query.toLowerCase().trim())).map((gascard) => (
          <li key={gascard.cardNumber.toString()}>
            <Link to={gascard.cardNumber.toString()}>
              {gascard.cardNumber}
              <span>
                <FontAwesomeIcon
                  title="bekijken"
                  className={gasCardClasses.eye}
                  icon={faEye}
                />
                &nbsp;&nbsp;&nbsp;|
                <Link
                  className={gasCardClasses.update}
                  to={`/gascards/${gascard.cardNumber}/edit`}
                >
                  <FontAwesomeIcon title="bewerken" icon={faPen} />
                </Link>
              </span>
            </Link>
          </li>
        ))}
      </ul>
    </header>
  );
};
export default DriverList;

import { NavLink } from 'react-router-dom';
import classes from '../css/Detail.module.css';
import foto from '../images/user.png';
import DetailCard from './DetailCard';
import { useState } from 'react';
import axios from 'axios';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

const DriverItem = ({ driver }) => {
  const [vehicleVin, setVehicleVin] = useState('');
  async function getVehicleVin(data) {
    try {
      const req = await axios.get(process.env.REACT_APP_VEHICLE_URL + data);
      setVehicleVin(req.data.licensePlate);
    } catch (error) {
      console.error(error);
    }
  }
  if (driver.vehicleVin !== null) {
    getVehicleVin(driver.vehicleVin);
  }

  return (
    <>
      <DetailCard type="driver" id={driver.driverID}>
        <article>
          <div className={classes.table}>
            <div className={classes.title}>
              {driver.firstName} {driver.lastName}
            </div>
            <div className={classes.image}>
              <img
                src={foto}
                alt={`Foto van ${driver.firstName} ${driver.lastName}`}
              />
            </div>
            <div className={classes.data}>
              <p>Geboortedatum: {driver.birthDate.split('T', 1)}</p>
              <p>Rijksregisternummer: {driver.natRegNum}</p>
              <p>Adres: {driver.address}</p>
              <p>Rijbewijs: {driver.licenses.toString()}</p>
              {driver.vehicleVin && (
                <p>
                  Nummerplaat: {vehicleVin}{' '}
                  <NavLink to={`/vehicles/${driver.vehicleVin}`}>
                    <FontAwesomeIcon icon={faArrowRight} />
                  </NavLink>
                </p>
              )}
              {driver.gasCardNum && (
                <p>
                  Tankkaart: {driver.gasCardNum}{' '}
                  <NavLink to={`/gascards/${driver.gasCardNum}`}>
                    <FontAwesomeIcon icon={faArrowRight} />
                  </NavLink>
                </p>
              )}
            </div>
          </div>
        </article>
      </DetailCard>
    </>
  );
};

export default DriverItem;

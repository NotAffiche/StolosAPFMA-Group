import DetailCard from './DetailCard';
import { NavLink } from 'react-router-dom';
import classes from '../css/Detail.module.css';
import foto from '../images/notAvailable.png';
import axios from 'axios';
import { useState } from 'react';

const VehicleItem = ({ vehicle }) => {
  const [name, setName] = useState('');

  async function getDriver(data) {
    try {
      const req = await axios.get(process.env.REACT_APP_DRIVER_URL + data);
      setName(req.data.firstName + ' ' + req.data.lastName);
    } catch (error) {
      console.error(error);
    }
  }
  if (vehicle.driverId !== null) {
    getDriver(vehicle.driverId);
  }

  return (
    <>
      <DetailCard type="vehicle" id={vehicle.vin}>
        <article>
          <div className={classes.table}>
            <div className={classes.title}>{vehicle.brandModel}</div>
            <div className={classes.image}>
              <img src={foto} alt={`Foto van ${vehicle.brandModel}`} />
            </div>
            <div className={classes.data}>
              <p>Kenteken: {vehicle.licensePlate}</p>
              <p>Chassisnummer: {vehicle.vin}</p>
              <p>Type: {vehicle.vehicleType}</p>
              <p>Brandstof: {vehicle.fuelType}</p>
              <p>Kleur: {vehicle.color}</p>
              <p>Aantal deuren: {vehicle.doors}</p>
              {vehicle.driverId && (
                <p>
                  Bestuurder:
                  <NavLink to={`/drivers/${vehicle.driverId}`}>{name}</NavLink>
                </p>
              )}
            </div>
          </div>
        </article>
      </DetailCard>
    </>
  );
};

export default VehicleItem;

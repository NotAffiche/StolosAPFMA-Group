import DetailCard from './DetailCard';
import { NavLink } from 'react-router-dom';
import classes from '../css/Detail.module.css';
import foto from '../images/notAvailable.png';
import axios from 'axios';
import { useState } from 'react';

const GascardItem = ({ gascard }) => {
  const [name, setName] = useState('');
  async function getDriver(data) {
    try {
      const req = await axios.get(process.env.REACT_APP_DRIVER_URL + data);
      setName(req.data.firstName + ' ' + req.data.lastName);
    } catch (error) {
      console.error(error);
    }
  }
  if (gascard.driverId !== null) {
    getDriver(gascard.driverId);
  }

  return (
    <>
      <DetailCard type="gascard" id={gascard.cardNumber}>
        <article>
          <div className={classes.table}>
            <div className={classes.title}>{gascard.cardNumber}</div>
            <div className={classes.image}>
              {<img src={foto} alt={`Foto van ${gascard.cardNumber}`} />}
            </div>
            <div className={classes.data}>
              <p>Vervaldatum: {gascard.expiringDate}</p>
              <p>
                Pincode:{' '}
                {gascard.pincode !== null
                  ? gascard.pincode.toString()
                  : 'N.V.T.'}
              </p>
              <p>Brandstoftypes: {gascard.fuelTypes.toString()}</p>
              <p>Geblokkeerd: {gascard.blocked === true ? 'Ja' : 'Nee'}</p>
              {gascard.driverId && (
                <p>
                  Bestuurder:{' '}
                  <NavLink to={`/drivers/${gascard.driverId}`}>
                    {name}
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

export default GascardItem;

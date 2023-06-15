import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCar, faUser, faGasPump } from '@fortawesome/free-solid-svg-icons';
import classes from '../css/Buttons.module.css';

const Buttons = props => {
  return (
    <div className={classes.button}>
      <Link to="drivers" className={classes.bestuurders}>
        <p>
          <FontAwesomeIcon icon={faUser} /> BESTUURDERS
        </p>
      </Link>
      <Link to="vehicles" className={classes.voertuigen}>
        <p>
          <FontAwesomeIcon icon={faCar} /> VOERTUIGEN
        </p>
      </Link>
      <Link to="gascards" className={classes.tankkaarten}>
        <p>
          <FontAwesomeIcon icon={faGasPump} /> TANKKAARTEN
        </p>
      </Link>
    </div>
  );
};

export default Buttons;

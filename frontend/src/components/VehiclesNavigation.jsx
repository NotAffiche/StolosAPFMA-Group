import { NavLink } from 'react-router-dom';
import classes from '../css/VehiclesNavigation.module.css';

function VehiclesNavigation() {
  return (
    <header className={classes.header}>
      <nav>
        <ul className={classes.list}>
          <li>
            <NavLink
              to="/vehicles"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              Alle voertuigen
            </NavLink>
          </li>
          <li>
            <NavLink
              to="/vehicles/new"
              className={({ isActive }) =>
                isActive ? classes.isActive : undefined
              }
            >
              Nieuw voertuig
            </NavLink>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default VehiclesNavigation;

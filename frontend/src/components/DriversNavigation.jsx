import React from 'react';
import { NavLink } from 'react-router-dom';
import classes from '../css/DriversNavigation.module.css';

function DriversNavigation() {
  return (
    <header className={classes.header}>
      <nav>
        <ul className={classes.list}>
          <li>
            <NavLink
              to="/drivers"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              Alle chauffeurs
            </NavLink>
          </li>
          <li>
            <NavLink
              to="/drivers/new"
              className={({ isActive }) =>
                isActive ? classes.isActive : undefined
              }
            >
              Nieuwe chauffeur
            </NavLink>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default DriversNavigation;

import React from 'react';
import { NavLink } from 'react-router-dom';

import classes from '../css/MainNavigation.module.css';

const MainNavigation = () => {
  return (
    <header className={classes.header}>
      <nav>
        <ul>
          <li>
            <NavLink
              to="/"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              Home
            </NavLink>
          </li>
          <li>
            <NavLink
              to="drivers"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
            >
              Bestuurders
            </NavLink>
          </li>
          <li>
            <NavLink
              to="vehicles"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
            >
              Voertuigen
            </NavLink>
          </li>
          <li>
            <NavLink
              to="gascards"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
            >
              Tankkaarten
            </NavLink>
          </li>
        </ul>
      </nav>
    </header>
  );
};

export default MainNavigation;

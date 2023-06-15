import React from 'react';
import { NavLink } from 'react-router-dom';
import classes from '../css/GascardNavigation.module.css';

function GascardNavigation() {
  return (
    <header className={classes.header}>
      <nav>
        <ul className={classes.list}>
          <li>
            <NavLink
              to="/gascards"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              Alle brandstofkaarten
            </NavLink>
          </li>
          <li>
            <NavLink
              to="/gascards/new"
              className={({ isActive }) =>
                isActive ? classes.isActive : undefined
              }
            >
              Nieuwe brandstofkaart
            </NavLink>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default GascardNavigation;

import React from 'react';
import { useRouteError } from 'react-router-dom';
import MainNavigation from '../components/MainNavigation';

import ErrorPageContent from '../components/ErrorPageContent';

const ErrorPage = () => {
  const error = useRouteError();
  let title = 'Fout';
  let message = 'Er is iets mis gegaan.';

  if (error.status === 500) {
    message = error.data.message;
  }

  if (error.status === 404) {
    title = '404 - Niet gevonden';
    message = 'Kon de pagina niet vinden.';
  }

  return (
    <>
      <MainNavigation />
      <ErrorPageContent title={title}>
        <p>{message}</p>
      </ErrorPageContent>
    </>
  );
};

export default ErrorPage;

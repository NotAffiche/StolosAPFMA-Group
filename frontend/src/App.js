import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import ErrorPage from './pages/Error';
import DriversPage, { loader as driversLoader } from './pages/Drivers';
import DriverDetailPage, {
  loader as driverDetailLoader,
  action as deleteDriverAction,
} from './pages/DriverDetail';
import RootLayout from './pages/Root';
import StartScreen from './pages/Start';
import VehiclesRootLayout from './pages/VehiclesRoot';
import EditDriverPage from './pages/EditDriver';
import EditGascardPage from './pages/EditGascard';
import GascardPage, { loader as gascardsLoader } from './pages/Gascards';
import GascardRootLayout from './pages/GascardRoot';
import GascardDetailPage, {
  loader as gascardDetailLoader,
  action as deleteGascardAction,
} from './pages/GascardDetail';
import DriversRootLayout from './pages/DriversRoot';
import VehiclesPage, { loader as vehiclesLoader } from './pages/Vehicles';
import VehicleDetailPage, {
  loader as vehicleDetailLoader,
  action as deleteVehicleAction,
} from './pages/VehicleDetail';
import EditVehiclePage from './pages/EditVehicle';
import { action as manipulateDriverAction } from './components/DriverForm';
import { action as manipulateVehicleAction } from './components/VehicleForm';
import { action as manipulateGascardAction } from './components/GascardForm';
import NewDriver from './pages/NewDriver';
import NewVehicle from './pages/NewVehicle';
import NewGascard from './pages/NewGascard';

// Router
const router = createBrowserRouter([
  {
    path: '/',
    element: <RootLayout />,
    errorElement: <ErrorPage />,
    children: [
      {
        index: true,
        element: <StartScreen />,
      },
      {
        path: 'vehicles',
        element: <VehiclesRootLayout />,
        children: [
          {
            index: true,
            element: <VehiclesPage />,
            loader: vehiclesLoader,
          },
          {
            path: ':vin',
            id: 'vehicle-detail',
            loader: vehicleDetailLoader,
            children: [
              {
                index: true,
                element: <VehicleDetailPage />,
                action: deleteVehicleAction,
              },
              {
                path: 'edit',
                element: <EditVehiclePage />,
                action: manipulateVehicleAction,
              },
            ],
          },
          {
            path: 'new',
            element: <NewVehicle />,
            action: manipulateVehicleAction,
          },
        ],
      },
      {
        path: 'drivers',
        element: <DriversRootLayout />,
        children: [
          {
            index: true,
            element: <DriversPage />,
            loader: driversLoader,
          },
          {
            path: ':driverID',
            id: 'driver-detail',
            loader: driverDetailLoader,
            children: [
              {
                index: true,
                element: <DriverDetailPage />,
                action: deleteDriverAction,
              },
              {
                path: 'edit',
                element: <EditDriverPage />,
                action: manipulateDriverAction,
              },
            ],
          },
          {
            path: 'new',
            element: <NewDriver />,
            action: manipulateDriverAction,
          },
        ],
      },
      {
        path: 'gascards',
        element: <GascardRootLayout />,
        children: [
          {
            index: true,
            element: <GascardPage />,
            loader: gascardsLoader,
          },
          {
            path: ':cardNumber',
            id: 'gascard-detail',
            loader: gascardDetailLoader,
            children: [
              {
                index: true,
                element: <GascardDetailPage />,
                action: deleteGascardAction,
              },
              {
                path: 'edit',
                element: <EditGascardPage />,
                action: manipulateGascardAction,
              },
            ],
          },
          {
            path: 'new',
            element: <NewGascard />,
            action: manipulateGascardAction,
          },
        ],
      },
    ],
  },
]);
const App = props => {
  return <RouterProvider router={router} />;
};

export default App;

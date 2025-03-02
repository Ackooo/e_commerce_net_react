import React from 'react';
import ReactDOM from 'react-dom/client';
import './app/layout/styles.css';
import reportWebVitals from './reportWebVitals';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import { RouterProvider } from 'react-router-dom';
import { router } from './app/router/Routes';
//import { StoreProvier } from './app/context/StoreContext';
//import { configureStore } from './app/store/configureStore';
import { Provider } from 'react-redux';
import { store } from './app/store/configureStore';
//import { fetchProductsAsync } from './features/catalog/catalogSlice';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';

//const store = configureStore();

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

//test purpose
//store.dispatch(fetchProductsAsync());


root.render(
  <React.StrictMode>
    {/* <App /> */}
    {/* provide react context to app */}
    {/*<StoreProvier> moved to redux*/} 
      {/* <Provider store={store}> */}
      <Provider store={store}>
      <RouterProvider router={router} />
      </Provider>
    
    {/*</StoreProvier>*/}
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

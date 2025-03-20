import { useCallback, useEffect, useState } from "react";
import Header from './Header';
import { Container, CssBaseline, ThemeProvider, createTheme } from "@mui/material";
import { Outlet, useLocation } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import LoadingComponent from "./LoadingComponent";
import { useAppDispatch } from "../store/configureStore";
import { fetchBasketAsync } from "../../features/basket/basketSlice";
import { fetchCurrentUser } from "../../features/account/accountSlice";
import HomePage from "../../features/home/HomePage";

function App() {
  // const {setBasket} = useStoreContext();
  const location = useLocation();
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);

  //nije funkcija zbog renderovanja kruznog zbog dependency
  //ovako se nece menjati pri svakom renderu
  //sto znai da se dependency u useeffect ne menja i nece pozvati ponovo
  const initApp = useCallback(async () => {
    try {
      await dispatch(fetchCurrentUser());
      await dispatch(fetchBasketAsync());
    } catch (error) {
      console.log(error);
    }
  }, [dispatch])

  useEffect(() => {
    initApp().then(() => setLoading(false));

    // const buyerId = getCookie('buyerId');
    // dispatch(fetchCurrentUser());
    // if(buyerId){
    //   //nije potreban parametar jer je buyerId u cookie, a to ce ici sa svakim request-om
    //   agent.Basket.get()
    //   //basket se vraca sa api - this is stored in storeContext
    //   //.then(basket => setBasket(basket))
    //   .then(basket => dispatch(setBasket(basket)))
    //   .catch(error => console.log(error))
    //   .finally(()=> setLoading(false));
    // }else{
    //   setLoading(false);
    // }
    // }, [setBasket]) //setBasket je dependancy, must be set, proveriti
  }, [initApp] /*[dispatch]*/)

  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    }
  })

  function handleThemeChange() {
    setDarkMode(!darkMode);
  }


  return (
    <ThemeProvider theme={theme}>
      <ToastContainer position="top-right" hideProgressBar theme="colored" />
      <CssBaseline />
      <Header darkMode={darkMode} handleThemeChange={handleThemeChange} />
      {loading ? <LoadingComponent message="Initialising..." />
        : location.pathname === '/' ? <HomePage />
          : <Container sx={{ mt: 4 }}>
            <Outlet />
          </Container>
      }
    </ThemeProvider>
  );
}

export default App;

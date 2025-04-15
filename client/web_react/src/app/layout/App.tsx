import { useCallback, useEffect, useState } from "react";
import Header from "./Header";
import {
  Box,
  Container,
  CssBaseline,
  ThemeProvider,
  createTheme,
} from "@mui/material";
import { Outlet, useLocation } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import LoadingComponent from "./LoadingComponent";
import { useAppDispatch } from "../store/configureStore";
import { fetchBasketAsync } from "../../features/basket/basketSlice";
import { fetchCurrentUser } from "../../features/account/accountSlice";
import HomePage from "../../features/home/HomePage";
import Footer from "./Footer";

function App() {
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
  }, [dispatch]);

  useEffect(() => {
    initApp().then(() => setLoading(false));
  }, [initApp]);

  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? "dark" : "light";
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: paletteType === "light" ? "#eaeaea" : "#121212",
      },
    },
  });

  function handleThemeChange() {
    setDarkMode(!darkMode);
  }

  return (
    <ThemeProvider theme={theme}>
      <ToastContainer position="top-right" hideProgressBar theme="colored" />
      <CssBaseline />

      <Box display="flex" flexDirection="column" minHeight="100vh">
        <Header darkMode={darkMode} handleThemeChange={handleThemeChange} />

        <Box component="main" flexGrow={1}>
          {loading ? (
            <LoadingComponent message="Initialising..." />
          ) : location.pathname === "/" ? (
            <HomePage />
          ) : (
            <Container sx={{ mt: 4 }}>
              <Outlet />
            </Container>
          )}
        </Box>

        <Footer />
      </Box>
    </ThemeProvider>
  );
}

export default App;

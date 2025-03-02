//import { useEffect, useState } from "react";
//import agent from "../../app/api/agent";
// import { Basket } from "../../app/models/basket";
// import LoadingComponent from "../../app/layout/LoadingComponent";
import { Button, Grid, Typography } from "@mui/material";
//import { useStoreContext } from "../../app/context/StoreContext";
import BasketSummary from "./BasketSummary";
import { Link } from "react-router-dom";
import { useAppSelector } from "../../app/store/configureStore";
import BasketTable from "./BasketTable";

export default function BasketPage() {
  //moved to app init - from storeContext
  // const [loading, setLoading] = useState(true);
  // const [basket, setBasket] = useState<Basket | null>(null);
  // useEffect(()=>{
  //     agent.Basket.get()
  //     .then(basket=>setBasket(basket))
  //     .catch(error=> console.log(error))
  //     .finally(()=>setLoading(false))
  // }, [])
  // if(loading) return <LoadingComponent message='Loading basket...' />




  //use all from storeContext
  //TODO: kako ovaj import radi
  //const { basket, setBasket, removeItem } = useStoreContext(); moved to redux below
  //svaki button isti loading ce biti
  //const [loading, setLoading] = useState(false);

  const {basket} = useAppSelector(state=>state.basket);

  //use to distinguish which button
  /*const [status, setStatus] = useState({
    loading: false,
    name: ''
  })*/

  /*
  function handleAddItem(productId: number, name: string) {
    setStatus({ loading: true, name });
    agent.Basket.addItem(productId)
      //.then(basket => setBasket(basket))
      .then(basket=>dispatch(setBasket(basket)))
      .catch(error => console.log(error))
      .finally(() => setStatus({ loading: true, name: '' }))
  }

  function handleRemoveItem(productId: number, quantity = 1, name: string) {
    setStatus({ loading: true, name });
    agent.Basket.removeItem(productId, quantity)
      //.then(() => removeItem(productId, quantity))
      .then(()=>dispatch(removeItem({productId, quantity})))
      .catch(error => console.log(error))
      .finally(() => setStatus({ loading: true, name: '' }))
  }

*/

  if (!basket) return <Typography variant='h3'>Your basket is empty</Typography>

  return (
    // <h1>Buyer Id = {basket.buyerId}</h1>

    <>
     <BasketTable items={basket.items}/>
      <Grid container>
        <Grid item xs={6} />
        <Grid item xs={6}>
          <BasketSummary />
          <Button
            component={Link}
            to='/checkout'
            variant='contained'
            size='large'
            fullWidth
          >
            Checkout
          </Button>
        </Grid>
      </Grid>

    </>




  )

}
import { Divider, Grid, Table, TableBody, TableCell, TableContainer, TableRow, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
//import { Product } from "../../app/models/product";
//import agent from "../../app/api/agent";
import NotFound from "../../app/errors/NotFound";
import LoadingComponent from "../../app/layout/LoadingComponent";
//import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { addBasketItemAsync, removeBasketItemAsync/*, setBasket*/ } from "../basket/basketSlice";
import { fetchProductAsync, productSelectors } from "./catalogSlice";

export default function ProductDetails() {
    //const {basket, setBasket, removeItem} = useStoreContext();
    const {basket, status} = useAppSelector(state=>state.basket);
    const dispatch = useAppDispatch();
    const {id} = useParams<{ id: string }>();
    //const [product, setProducts] = useState<Product | null>(null);
    //let idAsString:string = id!;
    const product = useAppSelector(state => productSelectors.selectById(state, id!)); //samo id kod njega
    //const [loading, setLoading] = useState(true);
    const {status: productStatus} = useAppSelector(state => state.catalog); //nadimak za catalogov status jer konflikt u imenu sa basket.status
    const [quantity, setQuantity] = useState(0);
    //const [submitting, setSubmitting] = useState(false);
    const item = basket?.items.find(i=> i.productId === product?.id);


    // obavezan depndancy da se ne bi uaplo u endless loop
    //bice pozvan jednom pri kreiranju i svaki put kad se dep promeni, u nasem slucaju id
    useEffect(() => {
        if(item) setQuantity(item.quantity);
        //axios.get(`http://localhost:5000/api/products/${id}`)
        //id && to make sure we have id before use
        //mozda nije potrebno* proveriti
        /*
        id && agent.Catalog.details(parseInt(id))
            .then(response => setProducts(response.data))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
            */

        if(!product && id) dispatch(fetchProductAsync(parseInt(id)));
    }, [id, item, dispatch, product])
    //proveriti logiku 2 depend. u jednom useeffect


    function handleInputChange(event: any){
        if(event.target.value >= 0){
            setQuantity(parseInt(event.target.value));
        }
    }

    function handleUpdateCart(){
        //setSubmitting(true);
        if(!item || quantity > item.quantity){
            const updatedQuantity = item ? quantity - item.quantity : quantity;
            //override the type safety
            //agent.Basket.addItem(product?.id!, updatedQuantity)
            //.then(basket => setBasket(basket))
            //.then(basket=>dispatch(setBasket(basket)))
            //.catch(error => console.log(error))
            //.finally(()=> setSubmitting(false));
            dispatch(addBasketItemAsync({productId: product?.id! , quantity: updatedQuantity}))
        }else{
            const updatedQuantity = item.quantity - quantity;
            //agent.Basket.removeItem(product?.id!, updatedQuantity)
            //.then(()=> removeItem(product?.id!, updatedQuantity))
            //.then(()=> dispatch(removeItem({productId: product?.id!,quantity: updatedQuantity})))
            //.catch(error=>console.log(error))
            //.finally(()=>setSubmitting(false));
            dispatch(removeBasketItemAsync({productId:product?.id!, quantity: updatedQuantity}))
        }
    }


    // if (loading) return <h3>Loading...</h3>
    // if (!product) return <h3>Product not found...</h3>
    //if (loading) return <LoadingComponent message="Loading product..." />

    if (productStatus.includes('pending')) return <LoadingComponent message="Loading product..." />

    if (!product) return <NotFound />

    return (
        <Grid container spacing={6}>
            <Grid item xs={6}>
                <img src={product.pictureUrl} alt={product.name} style={{ width: '100%' }} />
            </Grid>
            <Grid item xs={6}>
                <Typography variant='h3'>{product.name}</Typography>
                <Divider sx={{ mb: 2 }} />
                <Typography variant='h4' color='secondary'>${(product.price / 100).toFixed(2)}</Typography>
                <TableContainer>
                    <Table>
                        <TableBody>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>{product.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Description</TableCell>
                                <TableCell>{product.description}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Type</TableCell>
                                <TableCell>{product.type}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Brand</TableCell>
                                <TableCell>{product.brand}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Quantity in stock</TableCell>
                                <TableCell>{product.quantityInStock}</TableCell>
                            </TableRow>
                            
                        </TableBody>
                    </Table>
                </TableContainer>
                <Grid container spacing={2}>
                    <Grid item xs={6}>
                        {/* text field is wrapper around html input */}
                        <TextField
                        onChange={handleInputChange}
                        variant="outlined"
                        type="number"
                        label="Quantity in Cart"
                        fullWidth
                        value={quantity}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <LoadingButton
                        disabled={item?.quantity === quantity || !item && quantity ===0}
                        //loading={submitting}                        
                        loading={status.includes('pending')}
                        onClick={handleUpdateCart}
                            sx={{height: '55px'}}
                            color="primary"
                            size="large"
                            variant="contained"
                            fullWidth
                        >
                            {item ? "Update Quantity" : "Add to Cart"}
                        </LoadingButton>
                    </Grid>
                </Grid>
            </Grid>

        </Grid>


    )
}
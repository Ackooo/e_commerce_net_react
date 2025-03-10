//import agent from "../../app/api/agent";
import { Box, Grid, Pagination, Paper, Typography } from "@mui/material";
import LoadingComponent from "../../app/layout/LoadingComponent";
//import { Product } from "../../app/models/product"
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { fetchFilters, fetchProductsAsync, productSelectors, setPageNumber, setProductParams } from "./catalogSlice";
import ProductList from "./ProductList";
import { useEffect } from "react";
import ProductSearch from "./ProductSearch";
import RadioButtonGroup from "../../app/components/RadioButtonGroup";
import CheckboxButtons from "../../app/components/CheckboxButtons";
import AppPagination from "../../app/components/AppPagination";


const sortOptions = [
    { value: 'name', label: 'Alphabetical' },
    { value: 'priceDesc', label: 'Price - High to low' },
    { value: 'price', label: 'Price - Low to high' },
]


export default function Catalog() {

    //local state
    //const [products, setProducts] = useState<Product[]>([]);
    //
    // local state - use as soon as component is destroyed
    //in redux state stays within the app
    //

    const products = useAppSelector(productSelectors.selectAll);
    const { productsLoaded, status, filtersLoaded, brands, types, productParams, metaData } = useAppSelector(state => state.catalog);
    const dispatch = useAppDispatch();

    //loading state, moved to redux state
    //const [loading, setLoading] = useState(true);

    useEffect(() => {
        //   fetch('http://localhost:5000/api/products')
        //     .then(response => response.json())
        //     .then(data => setProducts(data))
        //

        /*         agent.Catalog.list()
                .then(products => setProducts(products))
                .catch(error => console.log(error))
                .finally(()=> setLoading(false)) */

        if (!productsLoaded) dispatch(fetchProductsAsync());
        //if(!filtersLoaded)dispatch(fetchFilters());



        //three dependancies => if one change, useefect called again => call api multiple times
        //solution ==> split useEffect

    }, [productsLoaded, dispatch/*, filtersLoaded*/])


    useEffect(() => {
        if (!filtersLoaded) dispatch(fetchFilters());
    }, [dispatch, filtersLoaded])

    //if (loading) return <LoadingComponent message="Loading products..." />
    //if (status.includes('pending') || !metaData) return <LoadingComponent message="Loading products..." />
    if(!filtersLoaded) return <LoadingComponent message="Loading products..." />

    return (
        <Grid container columnSpacing={4}>
            <Grid item xs={3}>
                <Paper sx={{ mb: 2 }}>
                    <ProductSearch />
                </Paper>
                <Paper sx={{ mb: 2, p: 2 }}>
                    <RadioButtonGroup
                        selectedValue={productParams.orderBy}
                        options={sortOptions}
                        onChange={(e) => dispatch(setProductParams({ orderBy: e.target.value }))}

                    />
                </Paper>
                <Paper sx={{ mb: 2, p: 2 }}>
                    <CheckboxButtons
                        items={brands}
                        checked={productParams.brands}
                        onChange={(items: string[]) => dispatch(setProductParams({ brands: items }))}
                    />
                </Paper>
                <Paper sx={{ mb: 2, p: 2 }}>
                    <CheckboxButtons
                        items={types}
                        checked={productParams.types}
                        onChange={(items: string[]) => dispatch(setProductParams({ types: items }))}
                    />
                </Paper>
            </Grid>
            <Grid item xs={9}>
                <ProductList products={products}></ProductList>
            </Grid>
            <Grid item xs={3} />
            <Grid item xs={9} sx={{mb:2}}>
                {metaData &&
                <AppPagination
                    metaData={metaData}
                    onPageChange={(page: number) => dispatch(setPageNumber({ pageNumber: page }))}
                />}
            </Grid>

        </Grid>
    )
}
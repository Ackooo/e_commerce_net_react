import { debounce, TextField } from "@mui/material";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { setProductParams } from "./catalogSlice";
import { useState } from "react";

export default function ProductSearch(){
    const {productParams} = useAppSelector(state=>state.catalog);
    const [searchTerm, setSearchTerm] = useState(productParams.searchTerm);
    const dispatch = useAppDispatch();

    const debouncedSearc = debounce((event:any)=>{
        dispatch(setProductParams({SearchTerm: event.target.value}))
    }, 500)


    return(
        <TextField
        label='Search products'
        variant="outlined"
        fullWidth
        value={searchTerm || ''}
        onChange={(event:any) => {
            setSearchTerm(event.target.value);
            debouncedSearc(event);
        }}
        />
    )
}
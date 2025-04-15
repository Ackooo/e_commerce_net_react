import { Box, debounce, TextField } from "@mui/material";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { setProductParams } from "./catalogSlice";
import { useState } from "react";

export default function ProductSearch() {
  const { productParams } = useAppSelector((state) => state.catalog);
  const [searchTerm, setSearchTerm] = useState(productParams.searchTerm);
  const dispatch = useAppDispatch();

  const debouncedSearc = debounce((event: any) => {
    dispatch(setProductParams({ SearchTerm: event.target.value }));
  }, 500);

  return (
    <Box display="flex" justifyContent="center" alignItems="center" gap={2}>
      <TextField
        placeholder="Search products…"
        variant="outlined"
        size="small"
        value={searchTerm || ""}
        onChange={(event: any) => {
          setSearchTerm(event.target.value);
          debouncedSearc(event);
        }}
        sx={{
          backgroundColor: "white",
          borderRadius: 1,
          width: 750,
          input: { paddingY: 1, paddingX: 2, color: "black" },
          mr: 2,
        }}
      />
    </Box>
  );
}


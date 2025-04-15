import ProductList from "./ProductList";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { setPageNumber, setProductParams } from "./catalogSlice";
import {
  Box,
  Button,
  Collapse,
  FormControl,
  Grid,
  IconButton,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  SelectChangeEvent,
  Slider,
  Typography,
} from "@mui/material";
import ProductSearch from "./ProductSearch";
import CheckboxButtons from "../../app/components/CheckboxButtons";
import AppPagination from "../../app/components/AppPagination";
import useProducts from "../../app/hooks/useProducts";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { useState } from "react";
import { number } from "yup";

const sortOptions = [
  { value: "name", label: "Alphabetical" },
  { value: "priceDesc", label: "Price - High to low" },
  { value: "price", label: "Price - Low to high" },
];

export default function Catalog() {
  const { products, filtersLoaded, brands, types, metaData } = useProducts();
  const { productParams } = useAppSelector((state) => state.catalog);
  const dispatch = useAppDispatch();

  const [priceOpen, setPriceOpen] = useState(false);
  const [brandOpen, setBrandOpen] = useState(false);
  const [typeOpen, setTypeOpen] = useState(false);

  if (!filtersLoaded) return <LoadingComponent message="Loading products..." />;

  return (
    <Box>
      <Box sx={{ mb: 3 }}>
        <ProductSearch />
      </Box>
      <Grid container columnSpacing={4}>
        <Grid item xs={3}>
          <Paper sx={{ p: 2, mb: 2 }}>
            <FormControl fullWidth size="small">
              <InputLabel id="sort-select-label">Sort By</InputLabel>
              <Select
                labelId="sort-select-label"
                value={productParams.orderBy}
                label="Sort By"
                onChange={(e: SelectChangeEvent) =>
                  dispatch(setProductParams({ orderBy: e.target.value }))
                }
              >
                {sortOptions.map((option) => (
                  <MenuItem key={option.value} value={option.value}>
                    {option.label}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Paper>

          <Paper sx={{ p: 2, mb: 2 }}>
            <Box
              display="flex"
              justifyContent="space-between"
              alignItems="center"
              mb={1}
            >
              <Box
                display="flex"
                alignItems="center"
                sx={{ cursor: "pointer" }}
                onClick={() => setPriceOpen((prev) => !prev)}
              >
                <Typography variant="subtitle1" fontWeight={600}>
                  Filter by Price
                </Typography>
                <IconButton size="small" sx={{ ml: 1 }}>
                  {priceOpen ? (
                    <ExpandLess fontSize="small" />
                  ) : (
                    <ExpandMore fontSize="small" />
                  )}
                </IconButton>
              </Box>
              <Typography variant="body2" color="text.secondary">
                ${productParams.minPrice} - ${productParams.maxPrice}
              </Typography>
            </Box>
            <Collapse in={priceOpen} timeout="auto" unmountOnExit>
              <Box px={1} mt={2}>
                <Slider
                  value={[
                    productParams.minPrice ?? 0,
                    productParams.maxPrice ?? 150000,
                  ]}
                  min={0}
                  max={10000}
                  step={100}
                  onChange={(_, newValue) => {
                    const [min, max] = newValue as number[];
                    dispatch(
                      setProductParams({ minPrice: min, maxPrice: max }),
                    );
                  }}
                  valueLabelDisplay="auto"
                />
              </Box>
              <Box display="flex" justifyContent="flex-end" mt={1}>
                <Button
                  size="small"
                  onClick={() =>
                    dispatch(setProductParams({ minPrice: 0, maxPrice: 10000 }))
                  }
                >
                  Reset
                </Button>
              </Box>
            </Collapse>
          </Paper>

          <Paper sx={{ p: 2, mb: 2 }}>
            <Box
              display="flex"
              justifyContent="space-between"
              alignItems="center"
              mb={1}
            >
              <Typography
                variant="subtitle1"
                fontWeight={600}
                onClick={() => setBrandOpen((prev: any) => !prev)}
                sx={{
                  cursor: "pointer",
                  display: "flex",
                  alignItems: "center",
                }}
              >
                Filter by Brand
                <IconButton size="small" sx={{ ml: 1 }}>
                  {brandOpen ? (
                    <ExpandLess fontSize="small" />
                  ) : (
                    <ExpandMore fontSize="small" />
                  )}
                </IconButton>
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {productParams.brands.length} selected
              </Typography>
            </Box>
            <Collapse in={brandOpen} timeout="auto" unmountOnExit>
              <CheckboxButtons
                items={brands}
                checked={productParams.brands}
                onChange={(items: string[]) =>
                  dispatch(setProductParams({ brands: items }))
                }
              />

              {productParams.brands.length > 0 && (
                <Box display="flex" justifyContent="flex-end" mt={2}>
                  <Button
                    size="small"
                    onClick={() => dispatch(setProductParams({ brands: [] }))}
                  >
                    Clear
                  </Button>
                </Box>
              )}
            </Collapse>
          </Paper>

          <Paper sx={{ p: 2 }}>
            <Box
              display="flex"
              justifyContent="space-between"
              alignItems="center"
              mb={1}
            >
              <Box
                display="flex"
                alignItems="center"
                sx={{ cursor: "pointer" }}
                onClick={() => setTypeOpen((prev) => !prev)}
              >
                <Typography variant="subtitle1" fontWeight={600}>
                  Filter by Type
                </Typography>
                <IconButton size="small" sx={{ ml: 1 }}>
                  {typeOpen ? (
                    <ExpandLess fontSize="small" />
                  ) : (
                    <ExpandMore fontSize="small" />
                  )}
                </IconButton>
              </Box>
              <Typography variant="body2" color="text.secondary">
                {productParams.types.length} selected
              </Typography>
            </Box>
            <Collapse in={typeOpen} timeout="auto" unmountOnExit>
              <CheckboxButtons
                items={types}
                checked={productParams.types}
                onChange={(items: string[]) =>
                  dispatch(setProductParams({ types: items }))
                }
              />

              {productParams.types.length > 0 && (
                <Box display="flex" justifyContent="flex-end" mt={2}>
                  <Button
                    size="small"
                    onClick={() => dispatch(setProductParams({ types: [] }))}
                  >
                    Clear
                  </Button>
                </Box>
              )}
            </Collapse>
          </Paper>
        </Grid>
        <Grid item xs={9}>
          <ProductList products={products} />
        </Grid>
        <Grid item xs={3} />
        <Grid item xs={9} sx={{ mb: 2 }}>
          {metaData && (
            <AppPagination
              metaData={metaData}
              onPageChange={(page: number) =>
                dispatch(setPageNumber({ pageNumber: page }))
              }
            />
          )}
        </Grid>
      </Grid>
    </Box>
  );
}


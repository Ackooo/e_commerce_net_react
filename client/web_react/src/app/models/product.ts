export interface Product {
  id: number
  name: string
  description: string
  price: number
  pictureUrl: string
  brand: string
  type?: string
  quantityInStock?: number
}
export interface ProductParams {
  orderBy: string;
  searchTerm?: string;
  types: string[];
  brands: string[];
  pageNumber: number;
  pageSize: number;
}
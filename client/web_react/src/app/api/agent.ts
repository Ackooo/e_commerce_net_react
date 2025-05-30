import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { router } from "../router/Routes";
import { PaginatedResponse } from "../models/pagination";
import { store } from "../store/configureStore";

const simulateDelay = () => new Promise(resolve => setTimeout(resolve, 500));
//TODO: config api url
axios.defaults.baseURL = process.env.E_COMMERCE_NET_REACT_API_URL;
axios.defaults.withCredentials = true;
const responseBody = (response: AxiosResponse) => response.data;

axios.interceptors.request.use(config=>{
    const token = store.getState().account.user?.token;
    if(token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    //TODO: testing purpose
    await simulateDelay();

    //lower case because of axios properties
    const pagination = response.headers['pagination'];
    if(pagination){
        response.data = new PaginatedResponse(response.data, JSON.parse(pagination));       
    }
    
    return response
}, (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if (data.errors) {
                const modelStateErrors: string[] = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modelStateErrors.push(data.errors[key])
                    }
                }
                throw modelStateErrors.flat();
            }
            toast.error(data.title); break;
        case 401:
            toast.error(data.title ); break;
        case 403:
            toast.error(data.title ); 
            toast.error('Not allowed!'); break;
        case 500:
            //only way to add something in history of the browser outside the react component
            router.navigate('/server-error', { state: { error: data } });
            break;
        default:
            break;
    }
    return Promise.reject(error.response);
})

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, { params }).then(responseBody),
    post: (url: string, body: {}, p0?: { withCredentials: boolean }) => {        
        const config = p0 ? { withCredentials: p0.withCredentials } : {}; 
        return axios.post(url, body, config).then(responseBody);
      },
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
    postForm: (url:string, data:FormData) => axios.post(url, data, {
        headers: {'Content-type':'multipart/form-data'}
    }).then(responseBody),
    putForm: (url:string, data:FormData) => axios.put(url, data, {
        headers: {'Content-type':'multipart/form-data'}
    }).then(responseBody)
}

function createFormData(item:any){
    let formData = new FormData();
    for(const key in item){
        formData.append(key, item[key])
    }
    return formData;
}

const Vendor={
    createProduct: (product:any) =>requests.postForm('products', createFormData(product)),
    updateProduct: (product:any) =>requests.putForm('products', createFormData(product)),
    deleteProduct: (id: number) => requests.delete(`products/${id}`),
}

const Catalog = {
    list: (params: URLSearchParams) => requests.get('products', params),
    details: (id: number) => requests.get(`products/${id}`),
    fetchFilters: () => requests.get('products/filters')
}

const TestErrors = {
    get400Error: () => requests.get('buggy/bad-request'),
    get401Error: () => requests.get('buggy/unauthorised'),
    get404Error: () => requests.get('buggy/not-found'),
    get500Error: () => requests.get('buggy/server-error'),
    getValidationError: () => requests.get('buggy/validation-error'),
}

const Basket = {
    get: () => requests.get('basket'),
    addItem: (productId: number, quantity = 1) => requests.post(`basket/additem?productId=${productId}&quantity=${quantity}`, {}),
    removeItem: (productId: number, quantity = 1) => requests.delete(`basket/removeItem?productId=${productId}&quantity=${quantity}`)
}

const Account = {
    login: (values: any) => requests.post('account/login', values),
    register: (values: any) => requests.post('account/register', values, { withCredentials: true }),
    currentUser: () => requests.get('account/currentUser'),
    fetchAddress: () => requests.get('account/savedAddress'),
    fetchCultures: () => requests.get('account/availableCultures'),
    setCulture: (value: any) => requests.post(`account/setCulture?culture=${value}`, {}, { withCredentials: true }),

}

const Orders = {
    list: () => requests.get('orders'),
    fetch: (id:string) => requests.get(`orders/${id}`),
    create: (values:any) => requests.post('orders', values)
}

const Payments = {
    createPaymentIntent: () => requests.post('payments', {})
}

const agent = {
    Catalog,
    TestErrors,
    Basket,
    Account,
    Orders,
    Payments,
    Vendor
}

export default agent;
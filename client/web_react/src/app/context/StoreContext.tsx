//component to wrap around the app and have state available anyhere in app
//for notification number for exmpl
//on highest evel -> index.tsx
import { PropsWithChildren, createContext, useContext, useState } from "react";
import { Basket } from "../models/basket";
interface StoreContextValue {
    basket: Basket | null;
    setBasket: (basket: Basket) => void;
    removeItem: (productId: number, quantity: number) => void;
}
export const StoreContext = createContext<StoreContextValue | undefined>(undefined);
//react hook always starts with use
export function useStoreContext() {
    const context = useContext(StoreContext);

    if (context === undefined) {
        throw Error('Oops - we do not seem to be inside the provider');
    }
    return context;
}

export function StoreProvier({ children }: PropsWithChildren<any>) {
    const [basket, setBasket] = useState<Basket | null>(null);

    function removeItem(productId: number, quantity: number) {
        if (!basket) return;
        const items = [...basket.items]; //spread operator, creates a new copy of array and store in items
        //praksa - create copy of the state, modify, and store that new state
        const itemIndex = items.findIndex(i => i.productId === productId);
        if (itemIndex >= 0) {
            items[itemIndex].quantity -= quantity;
            if (items[itemIndex].quantity === 0) items.splice(itemIndex, 1);//splice mutates the state
            setBasket(prevState => {
                return { ...prevState!, items } //! override the safety func
            })
        }
    }
    return (
        <StoreContext.Provider value={{ basket, setBasket, removeItem }}>
            {children}
        </StoreContext.Provider>
    )
}
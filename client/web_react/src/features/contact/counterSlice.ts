import { createSlice } from "@reduxjs/toolkit";
export interface CounterState {
    data: number;
    title: string;
}

const initialState: CounterState = {
    data: 42,
    title: 'another redux counter toolkit'
}
export const counterSlice = createSlice({
    name: 'counter',
    initialState,
    reducers: {
        increment: (state, action) => {
            state.data += action.payload
            //mutira state i ne bi smelo, ali toolkit koristi immer biblioteku koja handle-uje to
        },
        decrement: (state, action) => {
            state.data -= action.payload
        }
    }
})

export const {increment, decrement} = counterSlice.actions;//creators that toolkit creates
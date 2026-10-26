import { ReactNode } from "react";


export type  DataColumn<T> ={
    key: keyof T;
    label: string;
    render?:(item:T) => ReactNode;
}


export type DataViewConfig<T> = {
    columns: DataColumn<T>[];
}

export type DataViewProps<T> ={
    data:T[];
    config:DataViewConfig<T>;
}
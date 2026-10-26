import { Author } from "./author";
import { Book } from "./book";


export interface Publisher{
    id:string;
    publishername: string;
    address:string;
    city:string;
    website:string;
    email:string;
    phone:string;

    createdat?: string | null;
    updatedat?: string | null;

    authors?: Author[];
    books?:Book[];

} 

interface PublisherPayload{
    
    publishername: string;
    address:string;
    city:string;
    website:string;
    email:string;
    phone:string;


} 


export type CreatePublisherRequest = PublisherPayload;
export type UpdatePublisherRequest = PublisherPayload;
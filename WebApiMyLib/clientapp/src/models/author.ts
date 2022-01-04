import { Book } from "./book";

export type Author = {
    id?: number,
    firstName: string,
    lastName: string,
    books: Book[],
};

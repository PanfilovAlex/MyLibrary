import { Author } from './author';
import { Category } from './category';

export type Book = {
    id?: number,
    title: string,
    authors: Author[],
    categories: Category[],
};

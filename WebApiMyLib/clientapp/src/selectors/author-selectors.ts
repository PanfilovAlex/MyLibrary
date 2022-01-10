import { Author } from '../models/author';

export function getFullName(author: Author): string {
    return `${author.firstName} ${author.lastName}`;
}
